using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KarateGeek.databaseConnection;
using System.Data;
using KarateGeek.helpers;

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for ClubManagement.xaml
    /// </summary>
    public partial class ClubManagement : Window
    {
        private ClubConnection clubConnection;
        private CountryConnection countryConnection;
        private AddressConnection addressConnection;
        private DataSet clubCountries;
        private DataSet clubCities;
        private static string defaultCountry = "CY";
        private int countryIndex;
        ErrorMessages em = new ErrorMessages();
        RegularExpressions regex = new RegularExpressions();


        #region new Club Definitions

        private string _newClubName = null;
        private string _newClubPhone = null;
        private string _newClubEmail = null;
        private string _newClubAddress = null;
        private string _newClubAddressNum = null;
        private string _newClubTK = null;
        private string _newClubCity = null;
        private string _newClubCountryCode = null;
     //   private DataSet filteredNewClubs;
     //   List<ListData> newClubNameListForAutoComplete;
        
        #endregion

        #region edit Club Definitions


        private int _editClubId = -1;
        private string _editClubName = null;
        private string _editClubPhone = null;
        private string _editClubEmail = null;
        private string _editClubAddress = null;
        private string _editClubAddressNum = null;
        private string _editClubTK = null;
        private string _editClubCity = null;
        private string _editClubCountryCode = null;
        private DataSet filteredEditClubs;
        List<ListData> editClubNameListForAutoComplete;

        #endregion


        #region edit Club Definitions

        #endregion



        private Window sender;

        public ClubManagement(Window sender)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            clubConnection = new ClubConnection();

            countryConnection = new CountryConnection();
            this.clubCountries = countryConnection.GetCountries();

            foreach (DataRow dr in clubCountries.Tables[0].Rows)
            {
                cmbNewCCountryChooses.Items.Add(dr[1].ToString());
                cmbEditCCountryChooses.Items.Add(dr[1].ToString());
                
            }
            countryIndex = countryConnection.getIndexOfCountryCode(defaultCountry);
            cmbNewCCountryChooses.SelectedIndex = countryIndex;
            cmbEditCCountryChooses.SelectedIndex = countryIndex;

            this.newClubUpdateCities(defaultCountry);
            this.editClubUpdateCities(defaultCountry);

            this.sender = sender;
        }

        #region new Club
        private void newClubName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newClubName = newClubName.Text;
            if (setNewClubSaveEnable())
                btnNewCSave.IsEnabled = true;
            else
                btnNewCSave.IsEnabled = false;
        }

        private void newClubUpdateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.clubCities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbNewCCityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbNewCCityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in clubCities.Tables[0].Rows)
            {
                cmbNewCCityChooses.Items.Add(dr[1].ToString());
            }
            cmbNewCCityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbNewCCityChooses.Items.Refresh();

        }

        private void newClubPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newClubPhone = newClubPhone.Text;
        }

        private void newClubEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newClubEmail = newClubEmail.Text;
        }

        private void newClubAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newClubAddress = newClubAddress.Text;
        }

        private void newClubAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newClubAddressNum = newClubAddressNum.Text;
        }

        private void newClubTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newClubTK = newClubTK.Text;
        }

        private void cmbNewCCityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewCCityChooses.SelectedIndex;
            if (index < cmbNewCCityChooses.Items.Count && index != -1)
                _newClubCity = cmbNewCCityChooses.Items[index].ToString();
        }

        private void cmbNewCCountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewCCountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            _newClubCountryCode = clubCountries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this.newClubUpdateCities(_newClubCountryCode);
        }

        private void btnNewCSave_Click(object sender, RoutedEventArgs e)
        {
            bool insertClub;
            if (checkNullOrEmptyFields(true) && checkWrongFields(true))
            {
                _newClubName = _newClubName.Replace('\'', '’');
                insertClub = clubConnection.insertNewCLub(_newClubName, _newClubPhone, _newClubEmail, _newClubAddress, _newClubAddressNum, _newClubTK, _newClubCountryCode, _newClubCity);
                if (insertClub)
                {
                    MessageBox.Show("Succesfully saved!", "Club Add", MessageBoxButton.OK);
                    initializeNewClub();
                }
                else
                {
                    MessageBox.Show("Error. Club not succesfully saved!", "Club add", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnNewCBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.sender.Show();
        }




        #endregion

        #region edit Club

        
        private void editClubList()
        {
            _editClubName = editClubName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            if (setEditClubSaveEnable())
            {
                btnEditCSave.IsEnabled = true;
            }
            else
            {
                btnEditCSave.IsEnabled = false;
            }

            editClubNameListForAutoComplete = this.ClubfilterNames();

            foreach (ListData item in editClubNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                cSuggestionList.ItemsSource = autoList;
                cSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (editClubName.Text.Equals(""))
            {
                cSuggestionList.Visibility = Visibility.Collapsed;
                cSuggestionList.ItemsSource = null;
            }
            else
            {
                cSuggestionList.Visibility = Visibility.Collapsed;
                cSuggestionList.ItemsSource = null;
            }
        }

        private List<ListData> ClubfilterNames()
        {
            List<ListData> list = new List<ListData>();

            this.filteredEditClubs = clubConnection.findSimilar(this.editClubName.Text.Replace('\'', '’'));

            foreach (DataRow dr in filteredEditClubs.Tables[0].Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString();
                list.Add(suggestion);
            }
            return list;
        }

        private void cSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = null;
            int country_position = 0;
            int city_position = 0;
            int index;
            int address_id;
            addressConnection = new AddressConnection();
            DataSet ds;

            if (cSuggestionList.ItemsSource != null)
            {
                cSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                editClubName.TextChanged -= new TextChangedEventHandler(editClubName_TextChanged);

                index = cSuggestionList.SelectedIndex;

                if (cSuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)cSuggestionList.SelectedItem;
                    _editClubId = item.id;
                    name = cSuggestionList.SelectedItem.ToString();
                    address_id = int.Parse(filteredEditClubs.Tables[0].Rows[index][4].ToString());

                    this.editClubName.Text = filteredEditClubs.Tables[0].Rows[index][1].ToString();
                    _editClubName = this.editClubName.Text;
                    if (setEditClubSaveEnable())
                    {
                        btnEditCSave.IsEnabled = true;
                    }
                    else
                    {
                        btnEditCSave.IsEnabled = false;
                    }
                    this.editClubPhone.Text = filteredEditClubs.Tables[0].Rows[index][2].ToString();
                    this.editClubEmail.Text = filteredEditClubs.Tables[0].Rows[index][3].ToString();
                  
                    ds = addressConnection.getAddress(address_id);

                    this.editClubAddress.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.editClubAddressNum.Text = ds.Tables[0].Rows[0][2].ToString();
                    this.editClubTK.Text = ds.Tables[0].Rows[0][4].ToString();

                    string clubCity = ds.Tables[0].Rows[0][3].ToString();
                    int ix = ds.Tables[0].Columns.Count;
                    string clubCountry = ds.Tables[0].Rows[0][5].ToString();




                    //
                    //the fix for the country selection error
                    //
                    CountryConnection countryconn = new CountryConnection();
                    DataSet countriname = countryconn.getCountryNameByCode(clubCountry);
                    clubCountry = countriname.Tables[0].Rows[0][0].ToString();




                    for (int i = 0; i < this.cmbEditCCountryChooses.Items.Count; i++)
                    {
                        if (clubCountry.Equals(cmbEditCCountryChooses.Items[i].ToString()))
                        {
                            country_position = i;
                            break;
                        }
                    }


                    this.cmbEditCCountryChooses.SelectedIndex = country_position;


                    CityConnection cityConnection = new CityConnection();
                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(clubCity));
                    clubCity = cityNa.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbEditCCityChooses.Items.Count; i++)
                    {
                        if (clubCity.Equals(cmbEditCCityChooses.Items[i].ToString()))
                        {
                            city_position = i;
                            break;
                        }
                    }
                    this.cmbEditCCityChooses.SelectedIndex = city_position;
                    
                }
                editClubName.TextChanged += new TextChangedEventHandler(editClubName_TextChanged);
            }
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Hidden;
        } 

        private void editClubName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = editClubName.Text;

            if (name == "")
            {
                initializeEditClub();
            }

            editClubList();
        }

        private void editClubUpdateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.clubCities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbEditCCityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbEditCCityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in clubCities.Tables[0].Rows)
            {
                cmbEditCCityChooses.Items.Add(dr[1].ToString());
            }
            cmbEditCCityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbEditCCityChooses.Items.Refresh();




        }

        private void editClubPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editClubPhone = editClubPhone.Text;
        }

        private void editClubEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editClubEmail = editClubEmail.Text;
        }

        private void editClubAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editClubAddress = editClubAddress.Text;
        }

        private void editClubAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editClubAddressNum = editClubAddressNum.Text;
        }

        private void editClubTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editClubTK = editClubTK.Text;
        }

        private void cmbEditCCountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditCCountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            _editClubCountryCode = clubCountries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this.editClubUpdateCities(_editClubCountryCode);
        }

        private void cmbEditCCityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditCCityChooses.SelectedIndex;
            if (index < cmbEditCCityChooses.Items.Count && index != -1)
                _editClubCity = cmbEditCCityChooses.Items[index].ToString();
        }

        private void btnEditCBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.sender.Show();
        }

        private void btnEditCSave_Click(object sender, RoutedEventArgs e)
        {
            bool saveClub;
            if (_editClubId != -1)
            {
                if (checkNullOrEmptyFields(false) && checkWrongFields(false))
                {
                    _editClubName = _editClubName.Replace('\'', '’');
                    saveClub = clubConnection.updateClub(_editClubId, _editClubName, _editClubPhone, _editClubEmail, _editClubAddress, _editClubAddressNum, _editClubTK, _editClubCountryCode, _editClubCity);
                    if (saveClub)
                    {
                        MessageBox.Show("Succesfully updated!", "Club Edit", MessageBoxButton.OK);
                        initializeEditClub();
                    }
                    else
                    {
                        MessageBox.Show("Error. Club not succesfully updated!", "Club edit", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select one club to edit.", "Club Edit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnEditCDelete_Click(object sender, RoutedEventArgs e)
        {
            bool deleteClub;
            if (_editClubId != -1)
            {
                switch (warningDeletionMessage(_editClubName))
                {
                    case "OK":
                        deleteClub = clubConnection.deleteClub(_editClubId);
                        if (deleteClub)
                        {
                            MessageBox.Show("Succesfully deleted", "Club Edit", MessageBoxButton.OK);
                            initializeEditClub();
                        }
                        else
                        {
                            MessageBox.Show("Error. Club not succesfully deleted! There are athletes on this club!", "Club Edit", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                }
            }
            else
            {    
                MessageBox.Show("Please select one club to delete.", "Club Delete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region initialize methods
        private void initializeNewClub()
        {
            newClubName.Text = null;
            newClubPhone.Text = null;
            newClubEmail.Text = null;
            newClubAddress.Text = null;
            newClubAddressNum.Text = null;
            newClubTK.Text = null;
            cmbNewCCountryChooses.SelectedIndex = countryIndex;
            Dispatcher.BeginInvoke(new Action(() => { newClubName.Focus(); }));
        }

        private void initializeEditClub()
        {
            editClubName.Text = null;
            editClubPhone.Text = null;
            editClubEmail.Text = null;
            editClubAddress.Text = null;
            editClubAddressNum.Text = null;
            editClubTK.Text = null;
            cmbEditCCountryChooses.SelectedIndex = countryIndex;
            Dispatcher.BeginInvoke(new Action(() => { editClubName.Focus(); }));
        }
        #endregion

        #region checkMethods
        private bool checkNullOrEmptyFields(bool newMode)
        {
            if (newMode)
            {
                if (string.IsNullOrEmpty(_newClubName))
                {
                    em.errorMessage("Club/Association");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_editClubName))
                {
                    em.errorMessage("Club/Association");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private bool checkWrongFields(bool newMode)
        {
            if (newMode)
            {
                if (!regex.isSqlSpecialChar(_newClubName))
                {
                    em.sqlSpecialChar("Club/Association");
                    return false;
                }
                 else if (!string.IsNullOrEmpty(_newClubPhone) && !regex.isDigitsOnly(_newClubPhone))
                    {
                        em.digitsErrorMessage("Phone Num.");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newClubEmail) && !regex.isEmailValid(_newClubEmail))
                    {
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newClubAddress) && !regex.isCharsSpaceOrDots(_newClubAddress))
                    {
                        em.charsSpaceDotsErrorMessage("Address");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_newClubAddressNum) && !regex.isCharsOrDigits(_newClubAddressNum))
                    {

                        em.charsDigitsErrorMessage("Address Num.");
                        return false;

                    }
                else if (!string.IsNullOrEmpty(_newClubTK) && !regex.isDigitsOnly(_newClubTK))
                {

                    em.digitsErrorMessage("Postal Code");
                    return false;

                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (!regex.isSqlSpecialChar(_editClubName))
                {
                    em.sqlSpecialChar("Club/Association");
                    return false;
                }
                else if (!string.IsNullOrEmpty(_editClubPhone) && !regex.isDigitsOnly(_editClubPhone))
                {
                    em.digitsErrorMessage("Phone Num.");
                    return false;
                }
                else if (!string.IsNullOrEmpty(_editClubEmail) && !regex.isEmailValid(_editClubEmail))
                {
                    return false;
                }
                else if (!string.IsNullOrEmpty(_editClubAddress) && !regex.isCharsSpaceOrDots(_editClubAddress))
                {
                    em.charsSpaceDotsErrorMessage("Address");
                    return false;

                }
                else if (!string.IsNullOrEmpty(_editClubAddressNum) && !regex.isCharsOrDigits(_editClubAddressNum))
                {

                    em.charsDigitsErrorMessage("Address Num.");
                    return false;

                }
                else if (!string.IsNullOrEmpty(_editClubTK) && !regex.isDigitsOnly(_editClubTK))
                {

                    em.digitsErrorMessage("Postal Code");
                    return false;

                }
                else
                {
                    return true;
                }
            }
        }

        private bool setNewClubSaveEnable()
        {
            if (string.IsNullOrEmpty(_newClubName))
                return false;
            else
                return true;
        }

        private bool setEditClubSaveEnable()
        {
            if (string.IsNullOrEmpty(_editClubName))
                return false;
            else
                return true;
        }

        #endregion

        #region helpers
        
        private string warningDeletionMessage(string name)
        {
            return MessageBox.Show("Are you sure you want to delete " + name + "? \nPress OK to continue.", "Message",
               MessageBoxButton.OKCancel,
               MessageBoxImage.Information).ToString();
        }

        #endregion

        private void Window_close(object sender, EventArgs e)
        {
            this.Close();
            this.sender.Show();
        }

        private void editClubName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            editClubList();
        }

        private void editClubName_LostFocus(object sender, RoutedEventArgs e)
        {
            cSuggestionList.Visibility = Visibility.Collapsed;
        }

       

    }
}
