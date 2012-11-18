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

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for ClubManagement.xaml
    /// </summary>
    public partial class ClubManagement : Window
    {
        private int _clubId;
        private string _clubName = null;
        private string _clubPhone = null;
        private string _clubEmail = null;
        private string _clubAddress = null;
        private string _clubAddressNum = null;
        private string _clubTK = null;
        private string _clubCity = null;
        private string _clubCountryCode = null;
        private string _clubLogoSource = null;
        private DataSet countries;
        private DataSet cities;
        private DataSet filteredClubs;
        private ClubConnection clubConnection;
        private CountryConnection countryConnection;
        private AddressConnection addressConnection;
        List<ListData> clubNameListForAutoComplete;
        private Window sender;

        public ClubManagement(Window sender)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            clubConnection = new ClubConnection();

            countryConnection = new CountryConnection();
            this.countries = countryConnection.GetCountries();

            foreach (DataRow dr in countries.Tables[0].Rows)
            {
                cmbCCountryChooses.Items.Add(dr[1].ToString());
                
            }
            cmbCCountryChooses.SelectedIndex = 0;

            this.clubUpdateCities("CY");

            this.sender = sender;
        }

       
        
        private void btCBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.gif,*.jpg,*.jpeg,*.bmp,*.png)|*.gif;*.jpg;*.jpeg;*.bmp;*.png";
            dlg.InitialDirectory = "C:\\Users\\Public\\Pictures";
            dlg.Title = "Select image for logo";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                _clubLogoSource = filename;
                
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filename, UriKind.Absolute);
                bitmap.EndInit();
                clubLogo.Source = bitmap;
            }
        }

       

        private void clubName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubName = clubName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            clubNameListForAutoComplete = this.ClubfilterNames();

            foreach (ListData item in clubNameListForAutoComplete)
            {
                 autoList.Add(item);   
            }

            if (autoList.Count > 0)
            {
                cSuggestionList.DataContext = autoList;
                cSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (clubName.Text.Equals(""))
            {
                cSuggestionList.Visibility = Visibility.Collapsed;
                cSuggestionList.DataContext = null;
            }
            else
            {
                cSuggestionList.Visibility = Visibility.Collapsed;
                cSuggestionList.DataContext = null;
            }
        }

        private List<ListData> ClubfilterNames()
        {
            List<ListData> list = new List<ListData>();
     
            this.filteredClubs = clubConnection.findSimilar(this.clubName.Text);

            foreach (DataRow dr in filteredClubs.Tables[0].Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString();
                list.Add(suggestion);
            }
            return list;
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Visible;
            //this.sugestionList.ItemsSource = list;
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
                clubName.TextChanged -= new TextChangedEventHandler(clubName_TextChanged);

                index = cSuggestionList.SelectedIndex;

                if (cSuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)cSuggestionList.SelectedItem;
                    _clubId = item.id;
                    name = cSuggestionList.SelectedItem.ToString();
                    address_id = int.Parse(filteredClubs.Tables[0].Rows[index][5].ToString());

                    this.clubName.Text = filteredClubs.Tables[0].Rows[index][1].ToString();
                    this.clubPhone.Text = filteredClubs.Tables[0].Rows[index][2].ToString();
                    this.clubEmail.Text = filteredClubs.Tables[0].Rows[index][3].ToString();
                    
                    ds = addressConnection.getAddress(address_id);

                    this.clubAddress.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.clubAddressNum.Text = ds.Tables[0].Rows[0][2].ToString();
                    this.clubTK.Text = ds.Tables[0].Rows[0][4].ToString();

                    string clubCity = ds.Tables[0].Rows[0][3].ToString();
                    int ix = ds.Tables[0].Columns.Count;
                    string clubCountry = ds.Tables[0].Rows[0][5].ToString();


                    //
                    //the fix for the country selection error
                    //
                    CountryConnection countryconn = new CountryConnection();
                    DataSet countriname = countryconn.getCountryNameByCode(clubCountry);
                    clubCountry = countriname.Tables[0].Rows[0][0].ToString();




                    for (int i = 0; i < this.cmbCCountryChooses.Items.Count; i++)
                    {
                        if (clubCountry.Equals(cmbCCountryChooses.Items[i].ToString()))
                        {
                            country_position = i;
                            break;
                        }
                    }


                    this.cmbCCountryChooses.SelectedIndex = country_position;


                    CityConnection cityConnection = new CityConnection();
                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(clubCity));
                    clubCity = cityNa.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbCCityChooses.Items.Count; i++)
                    {
                        if (clubCity.Equals(cmbCCityChooses.Items[i].ToString()))
                        {
                            city_position = i;
                            break;
                        }
                    }
                    this.cmbCCityChooses.SelectedIndex = city_position;
                    
                }
                clubName.TextChanged += new TextChangedEventHandler(clubName_TextChanged);
            }
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Hidden;
        }

        private void clubUpdateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.cities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbCCityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbCCityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbCCityChooses.Items.Add(dr[1].ToString());
            }
            cmbCCityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbCCityChooses.Items.Refresh();




        }

        private void clubPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubPhone = clubPhone.Text;
        }

        private void clubEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubEmail = clubEmail.Text;
        }

        private void clubAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubAddress = clubAddress.Text;
        }

        private void clubAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubAddressNum = clubAddressNum.Text;
        }

        private void clubTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubTK = clubTK.Text;
        }

        private void cmbCCityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbCCityChooses.SelectedIndex;
            if (index < cmbCCityChooses.Items.Count && index != -1)
                _clubCity = cmbCCityChooses.Items[index].ToString();
        }

        private void cmbCCountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbCCountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            _clubCountryCode = countries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this.clubUpdateCities(_clubCountryCode);
        }

        private void btnCSaveNew_Click(object sender, RoutedEventArgs e)
        {
            clubConnection.InsertNewCLub(_clubName, _clubPhone , _clubEmail , _clubLogoSource , _clubAddress , _clubAddressNum , _clubTK , _clubCountryCode , _clubCity);
            MessageBox.Show("Succesfully saved!");
        }

        private void btnCBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.sender.Show();
        }

        private void btnCSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
