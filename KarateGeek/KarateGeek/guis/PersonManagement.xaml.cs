using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KarateGeek.databaseConnection;
using System.Data;
using System.Text.RegularExpressions;
using KarateGeek.helpers;

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for PersonManagement.xaml
    /// </summary>
    public partial class PersonManagement : Window
    {
        #region definitions
        //
        //application specific variables
        //
        private DataSet cities;
        private DataSet countries;
        private DataSet newFilteredAthletes;
        private DataSet editFilteredAthletes;
        private DataSet newFilteredJudges;
        private DataSet editFilteredJudges;
        private DataSet clubs;
        private AddressConnection addressConnection = new AddressConnection();
        private CountryConnection countryConnection = new CountryConnection();
        private PersonConnection personsConnection = new PersonConnection();
        private ClubConnection clubConnection = new ClubConnection();
        List<ListData> newAthleteNameListForAutoComplete;
        List<ListData> editAthleteNameListForAutoComplete;
        List<ListData> newJudgeNameListForAutoComplete;
        List<ListData> editJudgeNameListForAutoComplete;
        RegularExpressions regex = new RegularExpressions();
        ErrorMessages em = new ErrorMessages();

        //
        //athlete specific variables
        //

        //New athlete variables
        private AthleteConnection athleteConnection = new AthleteConnection();
        private int _newPersonId = -1;
        private string _newAthleteFirstName = null;
        private string _newAthleteLastName = null;
        private string _newAthleteFathersName = null;
        private string _newAthleteSex = null;
        private string _newAthleteFirstPhone = null;
        private string _newAthleteSecondPhone = null;
        private string _newAthleteEmail = null;
        private string _newAthleteAddress = null;
        private string _newAthleteAddressNum = null;
        private string _newAthleteTK = null;
        private string _newAthleteCity = null;
        private string _newAthleteCountryCode = null;
        private string _newAthleteRank = null;
        private string _newAthleteClubId = null;
        private string _newAthleteDateOfBirth;

        //Edit athlete variables

        private int _editPersonId = -1;
        private string _editAthleteFirstName = null;
        private string _editAthleteLastName = null;
        private string _editAthleteFathersName = null;
        private string _editAthleteSex = null;
        private string _editAthleteFirstPhone = null;
        private string _editAthleteSecondPhone = null;
        private string _editAthleteEmail = null;
        private string _editAthleteAddress = null;
        private string _editAthleteAddressNum = null;
        private string _editAthleteTK = null;
        private string _editAthleteCity = null;
        private string _editAthleteCountryCode = null;
        private string _editAthleteRank = null;
        private string _editAthleteClubId = null;
        private string _editAthleteDateOfBirth;




        //
        //judge specific variables
        // 

        //new judge variables
        private JudgeConnection judgeConnection = new JudgeConnection();
        private int _newJudgeId = -1;
        private string _newJudgeFirstName = null;
        private string _newJudgeLastName = null;
        private string _newJudgeFathersName = null;
        private string _newJudgeSex = null;
        private string _newJudgeFirstPhone = null;
        private string _newJudgeSecondPhone = null;
        private string _newJudgeEmail = null;
        private string _newJudgeAddress = null;
        private string _newJudgeAddressNum = null;
        private string _newJudgeTK = null;
        private string _newJudgeCity = null;
        private string _newJudgeCountryCode = null;
        private string _newJudgeRank = null;
        private string _newJudgeClass = null;
        private string _newJudgeDateOfBirth;

        //edit judge variables
        private int _editJudgeId = -1;
        private string _editJudgeFirstName = null;
        private string _editJudgeLastName = null;
        private string _editJudgeFathersName = null;
        private string _editJudgeSex = null;
        private string _editJudgeFirstPhone = null;
        private string _editJudgeSecondPhone = null;
        private string _editJudgeEmail = null;
        private string _editJudgeAddress = null;
        private string _editJudgeAddressNum = null;
        private string _editJudgeTK = null;
        private string _editJudgeCity = null;
        private string _editJudgeCountryCode = null;
        private string _editJudgeRank = null;
        private string _editJudgeClass = null;
        private string _editJudgeDateOfBirth;


        //
        //mixali min frikaris afto einai gia na grouparis kodika
        //arxizi me #region <name>
        //lai telion me #endregion
        //
        private Window sender;

        public PersonManagement(Window sender)
        {
            this.sender = sender;
            //auto anoigei to parathiro sto kentro tis othonis
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //here should be the loading of the locations and clubs and countries
            InitializeComponent();

            //prosthetoume tis xwres kai arxikopoioume stin kipro
            this.countries = countryConnection.GetCountries();

            foreach (DataRow dr in countries.Tables[0].Rows)
            {
                cmbNewACountryChooses.Items.Add(dr[1].ToString());
                cmbNewJCountryChooses.Items.Add(dr[1].ToString());
                cmbEditACountryChooses.Items.Add(dr[1].ToString());
                cmbEditJCountryChooses.Items.Add(dr[1].ToString());
            }
            cmbNewACountryChooses.SelectedIndex = 54;
            cmbNewJCountryChooses.SelectedIndex = 54;
            cmbEditACountryChooses.SelectedIndex = 54;
            cmbEditJCountryChooses.SelectedIndex = 54;

            this.athleteUpdateCities("CY");



            //prosthetoume athlete_rank gia new athlete
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank01);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank02);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank03);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank04);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank05);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank06);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank07);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank08);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank09);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank10);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank11);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank12);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank13);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank14);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank15);
            cmbNewAthleteRankChooses.Items.Add(KarateGeek.Strings.rank16);

            cmbNewAthleteRankChooses.SelectedIndex = 0;

            //prosthetoume athlete_rank gia edit athlete
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank01);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank02);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank03);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank04);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank05);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank06);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank07);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank08);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank09);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank10);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank11);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank12);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank13);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank14);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank15);
            cmbEditAthleteRankChooses.Items.Add(KarateGeek.Strings.rank16);

            cmbEditAthleteRankChooses.SelectedIndex = 0;


            //prosthetoume judge class gia new judge
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank01);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank02);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank03);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank04);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank05);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank06);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank07);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank08);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank09);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank10);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank11);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank12);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank13);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank14);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank15);
            cmbNewJudgeRankChooses.Items.Add(KarateGeek.Strings.rank16);



            cmbNewJudgeRankChooses.SelectedIndex = 0;

            //prosthetoume judge class gia edit judge
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank01);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank02);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank03);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank04);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank05);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank06);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank07);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank08);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank09);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank10);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank11);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank12);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank13);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank14);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank15);
            cmbEditJudgeRankChooses.Items.Add(KarateGeek.Strings.rank16);



            cmbEditJudgeRankChooses.SelectedIndex = 0;

            //prosthetoume clubs gia new athlete
            this.clubs = clubConnection.GetClubs();

            foreach (DataRow dr in clubs.Tables[0].Rows)
            {
                cmbNewAClubChooses.Items.Add(dr[1]);


            }
            cmbNewAClubChooses.SelectedIndex = 0;

            //prosthetoume clubs gia edit athlete
            this.clubs = clubConnection.GetClubs();

            foreach (DataRow dr in clubs.Tables[0].Rows)
            {
                cmbEditAClubChooses.Items.Add(dr[1]);


            }
            cmbEditAClubChooses.SelectedIndex = 0;


            //prosthetoume judge classes gia new judge
            cmbNewJClassChooses.Items.Add("A");
            cmbNewJClassChooses.Items.Add("B");
            cmbNewJClassChooses.SelectedIndex = 0;

            //prosthetoume judge classes gia edit judge
            cmbEditJClassChooses.Items.Add("A");
            cmbEditJClassChooses.Items.Add("B");
            cmbEditJClassChooses.SelectedIndex = 0;
        }


        #endregion
        //private Boolean nameflag = true;

        #region new athlete

        #region autocomplete

        private List<ListData> newAthletesfilterNames()
        {
            List<ListData> list = new List<ListData>();

            this.newFilteredAthletes = personsConnection.similarPersonsNotInAthletes(this.NewAthleteFirstName.Text);

            foreach (DataRow dr in newFilteredAthletes.Tables[0].Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString() + " " + dr[2].ToString();
                list.Add(suggestion);
            }
            return list;
        }

        private void NewASuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = null;
            string sex = null;
            string rank = null;
            string club = null;
            int rank_position = 0;
            int club_position = 0;
            int country_position = 0;
            int city_position = 0;
            int index;
            int address_id;
            addressConnection = new AddressConnection();
            DataSet ds;
            DataSet ds2;
            DataSet ds3;

            if (NewASuggestionList.ItemsSource != null)
            {
                NewASuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                NewAthleteFirstName.TextChanged -= new TextChangedEventHandler(NewAthleteFirstName_TextChanged);

                index = NewASuggestionList.SelectedIndex;

                if (NewASuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)NewASuggestionList.SelectedItem;
                    _newPersonId = item.id;
                    ds2 = athleteConnection.findAthlete(_newPersonId);
                    ds3 = athleteConnection.findAthleteClub(_newPersonId);

                    name = NewASuggestionList.SelectedItem.ToString();
                    sex = newFilteredAthletes.Tables[0].Rows[index][4].ToString();

                    address_id = int.Parse(newFilteredAthletes.Tables[0].Rows[index][9].ToString());

                    this.NewAthleteFirstName.Text = newFilteredAthletes.Tables[0].Rows[index][1].ToString();
                    this._newAthleteFirstName = this.NewAthleteFirstName.Text;
                    this.NewAthleteLastName.Text = newFilteredAthletes.Tables[0].Rows[index][2].ToString();
                    this.NewAthleteFatherName.Text = newFilteredAthletes.Tables[0].Rows[index][3].ToString();

                    if (sex.Equals("male"))
                    {
                        this.NewArdButton1.IsChecked = true;
                    }
                    else
                    {
                        this.NewArdButton2.IsChecked = true;
                    }

                    this.NewAthleteDateOfBirth.SelectedDate = (DateTime)newFilteredAthletes.Tables[0].Rows[index][5];
                    this.NewAthleteFirstPhone.Text = newFilteredAthletes.Tables[0].Rows[index][6].ToString();
                    this.NewAthleteSecondPhone.Text = newFilteredAthletes.Tables[0].Rows[index][7].ToString();
                    this.NewAthleteEmail.Text = newFilteredAthletes.Tables[0].Rows[index][8].ToString();


                    ds = addressConnection.getAddress(address_id);

                    this.NewAthleteStreetName.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.NewAthleteAddressNum.Text = ds.Tables[0].Rows[0][2].ToString();
                    this.NewAthleteTK.Text = ds.Tables[0].Rows[0][4].ToString();

                    string athCity = ds.Tables[0].Rows[0][3].ToString();
                    int ix = ds.Tables[0].Columns.Count;
                    string athCountry = ds.Tables[0].Rows[0][5].ToString();


                    //
                    //the fix for the country selection error
                    //
                    CountryConnection countryconn = new CountryConnection();
                    DataSet countriname = countryconn.getCountryNameByCode(athCountry);
                    athCountry = countriname.Tables[0].Rows[0][0].ToString();




                    for (int i = 0; i < this.cmbNewACountryChooses.Items.Count; i++)
                    {
                        if (athCountry.Equals(cmbNewACountryChooses.Items[i].ToString()))
                        {
                            country_position = i;
                            break;
                        }
                    }


                    this.cmbNewACountryChooses.SelectedIndex = country_position;


                    CityConnection cityConnection = new CityConnection();
                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(athCity));
                    athCity = cityNa.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbNewAthleteCityChooses.Items.Count; i++)
                    {
                        if (athCity.Equals(cmbNewAthleteCityChooses.Items[i].ToString()))
                        {
                            city_position = i;
                            break;
                        }
                    }
                    this.cmbNewAthleteCityChooses.SelectedIndex = city_position;

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        rank = ds2.Tables[0].Rows[0][1].ToString();
                        //vriskei tin zwni pou exei o kathenas se poia thesi einai
                        for (int i = 0; i < cmbNewAthleteRankChooses.Items.Count; i++)
                        {
                            if (rank.Equals(cmbNewAthleteRankChooses.Items[i]))
                            {
                                rank_position = i;
                                break;
                            }
                        }
                        this.cmbNewAthleteRankChooses.SelectedIndex = rank_position;

                    }

                    if (ds3.Tables[0].Rows.Count > 0)
                    {

                        club = ds3.Tables[0].Rows[0][4].ToString();
                        //vriskei to club pou einai o kathenas
                        for (int i = 0; i < cmbNewAClubChooses.Items.Count; i++)
                        {
                            if (club.Equals(cmbNewAClubChooses.Items[i]))
                            {
                                club_position = i;
                                break;
                            }
                        }
                        this.cmbNewAClubChooses.SelectedIndex = club_position;
                    }
                }
                NewAthleteFirstName.TextChanged += new TextChangedEventHandler(NewAthleteFirstName_TextChanged);
            }
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Hidden;
        }

        private void newAthleteList()
        {
            _newAthleteFirstName = NewAthleteFirstName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            newAthleteNameListForAutoComplete = this.newAthletesfilterNames();

            foreach (ListData item in newAthleteNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                NewASuggestionList.ItemsSource = autoList;
                NewASuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (NewAthleteFirstName.Text.Equals(""))
            {
                NewASuggestionList.Visibility = Visibility.Collapsed;
                NewASuggestionList.ItemsSource = null;
            }
            else
            {
                NewASuggestionList.Visibility = Visibility.Collapsed;
                NewASuggestionList.ItemsSource = null;
            }
        }

        #endregion


        private void NewAthleteFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = NewAthleteFirstName.Text;

            if (name == "")
            {
                initializeNewAthlete();
            }

            newAthleteList();
        }

        private void NewAthleteLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newAthleteLastName = NewAthleteLastName.Text;

        }

        private void NewAthleteFathersName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newAthleteFathersName = NewAthleteFatherName.Text;

        }

        private void NewAthleteDateOfBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Nullable<DateTime> selectedDate = NewAthleteDateOfBirth.SelectedDate;
            if (selectedDate.HasValue)
                _newAthleteDateOfBirth = selectedDate.Value.ToShortDateString();
            else
                _newAthleteDateOfBirth = string.Empty;
        }

        private void NewAthleteFirstPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newAthleteFirstPhone = NewAthleteFirstPhone.Text;
        }

        private void NewAthleteSecondPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newAthleteSecondPhone = NewAthleteSecondPhone.Text;
        }

        private void NewAthleteEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newAthleteEmail = NewAthleteEmail.Text;

        }

        private void NewAthleteStreetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newAthleteAddress = NewAthleteStreetName.Text;
        }

        private void NewAthleteAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newAthleteAddressNum = NewAthleteAddressNum.Text;
        }

        private void NewAthleteTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newAthleteTK = NewAthleteTK.Text;
        }

        private void cmbNewACityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewAthleteCityChooses.SelectedIndex;
            if (index < cmbNewAthleteCityChooses.Items.Count && index != -1)
                _newAthleteCity = cmbNewAthleteCityChooses.Items[index].ToString();
        }

        private void cmbNewACountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewACountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            _newAthleteCountryCode = countries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this.athleteUpdateCities(_newAthleteCountryCode);
        }

        private void athleteUpdateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.cities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbNewAthleteCityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbNewAthleteCityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbNewAthleteCityChooses.Items.Add(dr[1].ToString());
            }
            cmbNewAthleteCityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbNewAthleteCityChooses.Items.Refresh();
        }

        private void cmbNewARankChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewAthleteRankChooses.SelectedIndex;
            _newAthleteRank = cmbNewAthleteRankChooses.Items[index].ToString();
        }

        private void cmbNewAClubChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewAClubChooses.SelectedIndex;
            _newAthleteClubId = clubs.Tables[0].Rows[index][0].ToString();
        }

        private void NewArdButton1_Checked(object sender, RoutedEventArgs e)
        {
            _newAthleteSex = Strings.male;
        }

        private void NewArdButton2_Checked(object sender, RoutedEventArgs e)
        {
            _newAthleteSex = Strings.female;
        }

        #region other listeners

        private void NewAthleteFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            NewASuggestionList.Visibility = Visibility.Collapsed;
        }

        private void NewAthleteFirstName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            newAthleteList();
        }

        #endregion

        #region buttons

        private void btnNewABack_Click(object sender, RoutedEventArgs e)
        {
            this.sender.Show();
            this.Close();
        }

        private void btnNewASave_Click(object sender, RoutedEventArgs e)
        {
            bool insertAthlete;
            if (checkNullOrEmptyFields("athlete", true) && checkWrongFields("athlete", true))
            {
                insertAthlete = athleteConnection.InsertNewAthlete(_newPersonId, _newAthleteFirstName, _newAthleteLastName, _newAthleteFathersName, _newAthleteSex, _newAthleteDateOfBirth, _newAthleteFirstPhone, _newAthleteSecondPhone, _newAthleteEmail, _newAthleteAddress, _newAthleteAddressNum, _newAthleteTK, _newAthleteCountryCode, _newAthleteCity, _newAthleteRank, _newAthleteClubId);
                if (insertAthlete)
                {
                    MessageBox.Show("Succesfully saved!", "Athlete Add", MessageBoxButton.OK);
                    initializeNewAthlete();
                }
                else
                {
                    MessageBox.Show("Error. Athlete not succesfully saved!", "Athlete add", MessageBoxButton.OK , MessageBoxImage.Error);
                }
            }


        }

        #endregion

        #endregion

        #region edit athlete

        #region autocomplete

        private List<ListData> editAthletesfilterNames()
        {
            List<ListData> list = new List<ListData>();

            this.editFilteredAthletes = personsConnection.similarAthletes(this.EditAthleteFirstName.Text);

            foreach (DataRow dr in editFilteredAthletes.Tables[0].Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString() + " " + dr[2].ToString();
                list.Add(suggestion);
            }
            return list;
        }

        private void EditASuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = null;
            string sex = null;
            string rank = null;
            string club = null;
            int rank_position = 0;
            int club_position = 0;
            int country_position = 0;
            int city_position = 0;
            int index;
            int address_id;
            addressConnection = new AddressConnection();
            DataSet ds;
            DataSet ds2;
            DataSet ds3;

            if (EditASuggestionList.ItemsSource != null)
            {
                EditASuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                EditAthleteFirstName.TextChanged -= new TextChangedEventHandler(EditAthleteFirstName_TextChanged);

                index = EditASuggestionList.SelectedIndex;

                if (EditASuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)EditASuggestionList.SelectedItem;
                    _editPersonId = item.id;
                    ds2 = athleteConnection.findAthlete(_editPersonId);
                    ds3 = athleteConnection.findAthleteClub(_editPersonId);

                    name = EditASuggestionList.SelectedItem.ToString();
                    sex = editFilteredAthletes.Tables[0].Rows[index][4].ToString();

                    address_id = int.Parse(editFilteredAthletes.Tables[0].Rows[index][9].ToString());

                    this.EditAthleteFirstName.Text = editFilteredAthletes.Tables[0].Rows[index][1].ToString();
                    this._editAthleteFirstName = this.EditAthleteFirstName.Text;
                    this.EditAthleteLastName.Text = editFilteredAthletes.Tables[0].Rows[index][2].ToString();
                    this.EditAthleteFatherName.Text = editFilteredAthletes.Tables[0].Rows[index][3].ToString();

                    if (sex.Equals("male"))
                    {
                        this.EditArdButton1.IsChecked = true;
                    }
                    else
                    {
                        this.EditArdButton2.IsChecked = true;
                    }

                    this.EditAthleteDateOfBirth.SelectedDate = (DateTime)editFilteredAthletes.Tables[0].Rows[index][5];
                    this.EditAthleteFirstPhone.Text = editFilteredAthletes.Tables[0].Rows[index][6].ToString();
                    this.EditAthleteSecondPhone.Text = editFilteredAthletes.Tables[0].Rows[index][7].ToString();
                    this.EditAthleteEmail.Text = editFilteredAthletes.Tables[0].Rows[index][8].ToString();


                    ds = addressConnection.getAddress(address_id);

                    this.EditAthleteStreetName.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.EditAthleteAddressNum.Text = ds.Tables[0].Rows[0][2].ToString();
                    this.EditAthleteTK.Text = ds.Tables[0].Rows[0][4].ToString();

                    string athCity = ds.Tables[0].Rows[0][3].ToString();
                    int ix = ds.Tables[0].Columns.Count;
                    string athCountry = ds.Tables[0].Rows[0][5].ToString();


                    //
                    //the fix for the country selection error
                    //
                    CountryConnection countryconn = new CountryConnection();
                    DataSet countriname = countryconn.getCountryNameByCode(athCountry);
                    athCountry = countriname.Tables[0].Rows[0][0].ToString();




                    for (int i = 0; i < this.cmbEditACountryChooses.Items.Count; i++)
                    {
                        if (athCountry.Equals(cmbEditACountryChooses.Items[i].ToString()))
                        {
                            country_position = i;
                            break;
                        }
                    }


                    this.cmbEditACountryChooses.SelectedIndex = country_position;


                    CityConnection cityConnection = new CityConnection();
                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(athCity));
                    athCity = cityNa.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbEditAthleteCityChooses.Items.Count; i++)
                    {
                        if (athCity.Equals(cmbEditAthleteCityChooses.Items[i].ToString()))
                        {
                            city_position = i;
                            break;
                        }
                    }
                    this.cmbEditAthleteCityChooses.SelectedIndex = city_position;

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        rank = ds2.Tables[0].Rows[0][1].ToString();
                        //vriskei tin zwni pou exei o kathenas se poia thesi einai
                        for (int i = 0; i < cmbEditAthleteRankChooses.Items.Count; i++)
                        {
                            if (rank.Equals(cmbEditAthleteRankChooses.Items[i]))
                            {
                                rank_position = i;
                                break;
                            }
                        }
                        this.cmbEditAthleteRankChooses.SelectedIndex = rank_position;

                    }

                    if (ds3.Tables[0].Rows.Count > 0)
                    {

                        club = ds3.Tables[0].Rows[0][4].ToString();
                        //vriskei to club pou einai o kathenas
                        for (int i = 0; i < cmbEditAClubChooses.Items.Count; i++)
                        {
                            if (club.Equals(cmbEditAClubChooses.Items[i]))
                            {
                                club_position = i;
                                break;
                            }
                        }
                        this.cmbEditAClubChooses.SelectedIndex = club_position;
                    }
                }
                EditAthleteFirstName.TextChanged += new TextChangedEventHandler(EditAthleteFirstName_TextChanged);
            }
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Hidden;
        }

        private void editAthleteList()
        {
            _editAthleteFirstName = EditAthleteFirstName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            editAthleteNameListForAutoComplete = this.editAthletesfilterNames();

            foreach (ListData item in editAthleteNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                EditASuggestionList.ItemsSource = autoList;
                EditASuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (EditAthleteFirstName.Text.Equals(""))
            {
                EditASuggestionList.Visibility = Visibility.Collapsed;
                EditASuggestionList.ItemsSource = null;
            }
            else
            {
                EditASuggestionList.Visibility = Visibility.Collapsed;
                EditASuggestionList.ItemsSource = null;
            }
        }

        #endregion

        private void EditAthleteFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = EditAthleteFirstName.Text;
            if (name == "")
            {
                initializeEditAthlete();
            }
            editAthleteList();
        }

        private void EditAthleteLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editAthleteLastName = EditAthleteLastName.Text;

        }

        private void EditAthleteFathersName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editAthleteFathersName = EditAthleteFatherName.Text;

        }

        private void EditAthleteDateOfBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Nullable<DateTime> selectedDate = EditAthleteDateOfBirth.SelectedDate;
            if (selectedDate.HasValue)
                _editAthleteDateOfBirth = selectedDate.Value.ToShortDateString();
            else
                _editAthleteDateOfBirth = string.Empty;
        }

        private void EditAthleteFirstPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editAthleteFirstPhone = EditAthleteFirstPhone.Text;
        }

        private void EditAthleteSecondPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editAthleteSecondPhone = EditAthleteSecondPhone.Text;
        }

        private void EditAthleteEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editAthleteEmail = EditAthleteEmail.Text;

        }

        private void EditAthleteStreetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editAthleteAddress = EditAthleteStreetName.Text;
        }

        private void EditAthleteAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editAthleteAddressNum = EditAthleteAddressNum.Text;
        }

        private void EditAthleteTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editAthleteTK = EditAthleteTK.Text;
        }

        private void cmbEditACityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditAthleteCityChooses.SelectedIndex;
            if (index < cmbEditAthleteCityChooses.Items.Count && index != -1)
                _editAthleteCity = cmbEditAthleteCityChooses.Items[index].ToString();
        }

        private void cmbEditACountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditACountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            _editAthleteCountryCode = countries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this.editAthleteUpdateCities(_editAthleteCountryCode);
        }

        private void editAthleteUpdateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.cities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbEditAthleteCityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbEditAthleteCityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbEditAthleteCityChooses.Items.Add(dr[1].ToString());
            }
            cmbEditAthleteCityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbEditAthleteCityChooses.Items.Refresh();




        }

        private void cmbEditARankChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditAthleteRankChooses.SelectedIndex;
            _editAthleteRank = cmbEditAthleteRankChooses.Items[index].ToString();
        }

        private void cmbEditAClubChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditAClubChooses.SelectedIndex;
            _editAthleteClubId = clubs.Tables[0].Rows[index][0].ToString();
        }

        private void EditArdButton1_Checked(object sender, RoutedEventArgs e)
        {
            _editAthleteSex = Strings.male;
        }

        private void EditArdButton2_Checked(object sender, RoutedEventArgs e)
        {
            _editAthleteSex = Strings.female;
        }

        #region other listeners

        private void EditAthleteFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            EditASuggestionList.Visibility = Visibility.Collapsed;
        }

        private void EditAthleteFirstName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            editAthleteList();
        }

        #endregion

        #region buttons

        private void btnEditABack_Click(object sender, RoutedEventArgs e)
        {
            this.sender.Show();
            this.Close();
        }

        private void btnEditASave_Click(object sender, RoutedEventArgs e)
        {
            if (checkNullOrEmptyFields("athlete", false) && checkWrongFields("athlete", false) && _editPersonId != -1)
            {
                athleteConnection.UpdateAthlete(_editPersonId, _editAthleteFirstName, _editAthleteLastName, _editAthleteFathersName, _editAthleteSex, _editAthleteDateOfBirth, _editAthleteFirstPhone, _editAthleteSecondPhone, _editAthleteEmail, _editAthleteAddress, _editAthleteAddressNum, _editAthleteTK, _editAthleteCountryCode, _editAthleteCity, _editAthleteRank, _editAthleteClubId);
                MessageBox.Show("Succesfully saved!", "Athlete Edit" , MessageBoxButton.OK );
                initializeEditAthlete();
            }
        }

        private void btnEditADelete_Click(object sender, RoutedEventArgs e)
        {
            if (_editPersonId != -1)
            {
                if (athleteConnection.deleteAthlete(_editPersonId))
                {
                    MessageBox.Show("Succesfully deleted!", "Athlete Delete" , MessageBoxButton.OK);
                    initializeEditAthlete();
                }
                else
                {
                    MessageBox.Show("Cannot be deleted because of tournament participation!", "Athlete Delete", MessageBoxButton.OK , MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select one athlete to delete.", "Athlete Delete" , MessageBoxButton.OK , MessageBoxImage.Information);
            }

        }
        #endregion

        #endregion

        #region new judge


        private List<ListData> newJudgefiltersNames()
        {
            List<ListData> list = new List<ListData>();

            this.newFilteredJudges = personsConnection.similarPersonsNotInJudges(this.newJudgeFirstName.Text);

            foreach (DataRow dr in newFilteredJudges.Tables[0].Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString() + " " + dr[2].ToString();
                list.Add(suggestion);
            }
            return list;
        }

        #region autocomplete

        private void newJSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = null;
            string sex = null;
            string rank = null;
            int country_position = 0;
            int city_position = 0;
            string judge_class = null;
            int rank_position = 0;
            int class_position = 0;
            int address_id;

            addressConnection = new AddressConnection();
            DataSet ds;
            DataSet ds2;
            DataSet ds3;

            if (newJSuggestionList.ItemsSource != null)
            {
                newJSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                newJudgeFirstName.TextChanged -= new TextChangedEventHandler(newJudgeFirstName_TextChanged);

                int index = newJSuggestionList.SelectedIndex;

                if (newJSuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)newJSuggestionList.SelectedItem;
                    _newJudgeId = item.id;
                    ds2 = judgeConnection.findJudge(_newPersonId);
                    ds3 = athleteConnection.findAthlete(_newPersonId);

                    name = newJSuggestionList.SelectedItem.ToString();
                    sex = newFilteredJudges.Tables[0].Rows[index][4].ToString();

                    address_id = int.Parse(newFilteredJudges.Tables[0].Rows[index][9].ToString());

                    this.newJudgeFirstName.Text = newFilteredJudges.Tables[0].Rows[index][1].ToString();
                    this._newJudgeFirstName = this.newJudgeFirstName.Text;
                    this.newJudgeLastName.Text = newFilteredJudges.Tables[0].Rows[index][2].ToString();
                    this.newJudgeFatherName.Text = newFilteredJudges.Tables[0].Rows[index][3].ToString();

                    if (sex.Equals("male"))
                    {
                        this.newJrdButton1.IsChecked = true;
                    }
                    else
                    {
                        this.newJrdButton2.IsChecked = true;
                    }

                    this.newJudgeDateOfBirth.SelectedDate = (DateTime)newFilteredJudges.Tables[0].Rows[index][5];
                    this.newJudgeFirstPhone.Text = newFilteredJudges.Tables[0].Rows[index][6].ToString();
                    this.newJudgeSecondPhone.Text = newFilteredJudges.Tables[0].Rows[index][7].ToString();
                    this.newJudgeEmail.Text = newFilteredJudges.Tables[0].Rows[index][8].ToString();

                    ds = addressConnection.getAddress(address_id);

                    this.newJudgeStreetName.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.newJudgeAddressNum.Text = ds.Tables[0].Rows[0][2].ToString();
                    this.newJudgeTK.Text = ds.Tables[0].Rows[0][4].ToString();

                    string athCity = ds.Tables[0].Rows[0][3].ToString();
                    int ix = ds.Tables[0].Columns.Count;
                    string athCountry = ds.Tables[0].Rows[0][5].ToString();


                    //
                    //the fix for the country selection error
                    //
                    CountryConnection countryconn = new CountryConnection();
                    DataSet countriname = countryconn.getCountryNameByCode(athCountry);
                    athCountry = countriname.Tables[0].Rows[0][0].ToString();




                    for (int i = 0; i < this.cmbNewACountryChooses.Items.Count; i++)
                    {
                        if (athCountry.Equals(cmbNewACountryChooses.Items[i].ToString()))
                        {
                            country_position = i;
                            break;
                        }
                    }


                    this.cmbNewJCountryChooses.SelectedIndex = country_position;


                    CityConnection cityConnection = new CityConnection();
                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(athCity));
                    athCity = cityNa.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbNewAthleteCityChooses.Items.Count; i++)
                    {
                        if (athCity.Equals(cmbNewAthleteCityChooses.Items[i].ToString()))
                        {
                            city_position = i;
                            break;
                        }
                    }
                    this.cmbNewJCityChooses.SelectedIndex = city_position;


                    if (ds2.Tables[0].Rows.Count > 0)
                    {

                        rank = ds2.Tables[0].Rows[0][1].ToString();
                        judge_class = ds2.Tables[0].Rows[0][2].ToString();

                        //vriskei tin zwni pou exei o kathenas se poia thesi einai
                        for (int i = 0; i < cmbNewJudgeRankChooses.Items.Count; i++)
                        {
                            if (rank.Equals(cmbNewJudgeRankChooses.Items[i]))
                            {
                                rank_position = i;
                                break;
                            }
                        }
                        this.cmbNewJudgeRankChooses.SelectedIndex = rank_position;




                        for (int i = 0; i < cmbNewJClassChooses.Items.Count; i++)
                        {
                            if (judge_class.Equals(cmbNewJClassChooses.Items[i]))
                            {
                                class_position = i;
                                break;
                            }
                        }
                        this.cmbNewJClassChooses.SelectedIndex = class_position;

                    }
                    else if (ds3.Tables[0].Rows.Count > 0)
                    {
                        rank = ds3.Tables[0].Rows[0][1].ToString();

                        //vriskei tin zwni pou exei o kathenas se poia thesi einai
                        for (int i = 0; i < cmbNewJudgeRankChooses.Items.Count; i++)
                        {
                            if (rank.Equals(cmbNewJudgeRankChooses.Items[i]))
                            {
                                rank_position = i;
                                break;
                            }
                        }
                        this.cmbNewJudgeRankChooses.SelectedIndex = rank_position;
                    }

                }
                newJudgeFirstName.TextChanged += new TextChangedEventHandler(newJudgeFirstName_TextChanged);
            }
        }

        private void newJudgeList()
        {
            _newJudgeFirstName = newJudgeFirstName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            newJudgeNameListForAutoComplete = this.newJudgefiltersNames();

            foreach (ListData item in newJudgeNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                newJSuggestionList.ItemsSource = autoList;
                newJSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }


            else if (newJudgeFirstName.Text.Equals(""))
            {
                newJSuggestionList.Visibility = Visibility.Collapsed;
                newJSuggestionList.ItemsSource = null;
            }


            else
            {
                newJSuggestionList.Visibility = Visibility.Collapsed;
                newJSuggestionList.ItemsSource = null;
            }
        }

        #endregion

        private void newJudgeFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = newJudgeFirstName.Text;

            if (name == "")
            {
                initializeNewJudge();
            }
            newJudgeList();
        }

        private void newJudgeLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newJudgeLastName = newJudgeLastName.Text;
        }

        private void newJudgeFatherName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newJudgeFathersName = newJudgeFatherName.Text;
        }

        private void newJudgeDateOfBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Nullable<DateTime> selectedDate = newJudgeDateOfBirth.SelectedDate;
            if (selectedDate.HasValue)
                _newJudgeDateOfBirth = selectedDate.Value.ToShortDateString();
            else
                _newJudgeDateOfBirth = string.Empty;
        }

        private void newJudgeFirstPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newJudgeFirstPhone = newJudgeFirstPhone.Text;
        }

        private void newJudgeSecondPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newJudgeSecondPhone = newJudgeSecondPhone.Text;
        }

        private void newJudgeEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newJudgeEmail = newJudgeEmail.Text;
        }

        private void newJudgeStreetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newJudgeAddress = newJudgeStreetName.Text;
        }

        private void newJudgeAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newJudgeAddressNum = newJudgeAddressNum.Text;
        }

        private void newJudgeTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newJudgeTK = newJudgeTK.Text;
        }

        private void cmbNewJCityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewJCityChooses.SelectedIndex;
            if (index < cmbNewJCityChooses.Items.Count && index != -1)
                _newJudgeCity = cmbNewJCityChooses.Items[index].ToString();
        }

        private void cmbNewJCountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewJCountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            _newJudgeCountryCode = countries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this.judgeUpdateCities(_newJudgeCountryCode);
        }

        private void judgeUpdateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.cities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbNewJCityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbNewJCityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbNewJCityChooses.Items.Add(dr[1].ToString());
            }
            cmbNewJCityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbNewJCityChooses.Items.Refresh();
        }

        private void newJrdButton1_Checked(object sender, RoutedEventArgs e)
        {
            _newJudgeSex = Strings.male;
        }

        private void newJrdButton2_Checked(object sender, RoutedEventArgs e)
        {
            _newJudgeSex = Strings.female;
        }

        private void cmbNewJRankChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewJudgeRankChooses.SelectedIndex;
            _newJudgeRank = cmbNewAthleteRankChooses.Items[index].ToString();
        }

        private void cmbNewJClassChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewJClassChooses.SelectedIndex;
            _newJudgeClass = cmbNewJClassChooses.Items[index].ToString();
        }

        #region other listeners

        private void newJudgeFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            newJSuggestionList.Visibility = Visibility.Collapsed;
        }

        private void newJudgeFirstName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            newJudgeList();
        }

        #endregion

        #region buttons
        private void btnNewJBack_Click(object sender, RoutedEventArgs e)
        {

            this.sender.Show();
            this.Close();
        }

        private void btnNewJSave_Click(object sender, RoutedEventArgs e)
        {
            bool judgeInsert;
            if (checkNullOrEmptyFields("judge", true) && checkWrongFields("judge", true))
            {
                judgeInsert = judgeConnection.InsertNewJudge(_newJudgeId, _newJudgeFirstName, _newJudgeLastName, _newJudgeFathersName, _newJudgeSex, _newJudgeDateOfBirth, _newJudgeFirstPhone, _newJudgeSecondPhone, _newJudgeEmail, _newJudgeAddress, _newJudgeAddressNum, _newJudgeTK, _newJudgeCountryCode, _newJudgeCity, _newJudgeRank, _newJudgeClass);
                if (judgeInsert)
                {
                    MessageBox.Show("Succesfully saved!","Judge Add",MessageBoxButton.OK);
                    initializeNewJudge();
                }
                else
                {
                    MessageBox.Show("Error. Judge not succesfully inserted","Judge Add", MessageBoxButton.OK , MessageBoxImage.Error);
                }

            }

        }

        #endregion

        #endregion

        #region edit judge

        private List<ListData> editJudgefiltersNames()
        {
            List<ListData> list = new List<ListData>();

            this.editFilteredJudges = personsConnection.similarJudges(this.editJudgeFirstName.Text);

            foreach (DataRow dr in editFilteredJudges.Tables[0].Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString() + " " + dr[2].ToString();
                list.Add(suggestion);
            }
            return list;
        }

        #region autocomplete

        private void editJSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = null;
            string sex = null;
            string rank = null;
            int country_position = 0;
            int city_position = 0;
            string judge_class = null;
            int rank_position = 0;
            int class_position = 0;
            int address_id;

            addressConnection = new AddressConnection();
            DataSet ds;
            DataSet ds2;
            DataSet ds3;

            if (editJSuggestionList.ItemsSource != null)
            {
                editJSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                editJudgeFirstName.TextChanged -= new TextChangedEventHandler(editJudgeFirstName_TextChanged);

                int index = editJSuggestionList.SelectedIndex;

                if (editJSuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)editJSuggestionList.SelectedItem;
                    _editJudgeId = item.id;
                    ds2 = judgeConnection.findJudge(_editPersonId);
                    ds3 = athleteConnection.findAthlete(_editPersonId);

                    name = editJSuggestionList.SelectedItem.ToString();
                    sex = editFilteredJudges.Tables[0].Rows[index][4].ToString();

                    address_id = int.Parse(editFilteredJudges.Tables[0].Rows[index][9].ToString());

                    this.editJudgeFirstName.Text = editFilteredJudges.Tables[0].Rows[index][1].ToString();
                    this._editJudgeFirstName = this.editJudgeFirstName.Text;
                    this.editJudgeLastName.Text = editFilteredJudges.Tables[0].Rows[index][2].ToString();
                    this.editJudgeFatherName.Text = editFilteredJudges.Tables[0].Rows[index][3].ToString();

                    if (sex.Equals("male"))
                    {
                        this.editJrdButton1.IsChecked = true;
                    }
                    else
                    {
                        this.editJrdButton2.IsChecked = true;
                    }

                    this.editJudgeDateOfBirth.SelectedDate = (DateTime)editFilteredJudges.Tables[0].Rows[index][5];
                    this.editJudgeFirstPhone.Text = editFilteredJudges.Tables[0].Rows[index][6].ToString();
                    this.editJudgeSecondPhone.Text = editFilteredJudges.Tables[0].Rows[index][7].ToString();
                    this.editJudgeEmail.Text = editFilteredJudges.Tables[0].Rows[index][8].ToString();

                    ds = addressConnection.getAddress(address_id);

                    this.editJudgeStreetName.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.editJudgeAddressNum.Text = ds.Tables[0].Rows[0][2].ToString();
                    this.editJudgeTK.Text = ds.Tables[0].Rows[0][4].ToString();

                    string athCity = ds.Tables[0].Rows[0][3].ToString();
                    int ix = ds.Tables[0].Columns.Count;
                    string athCountry = ds.Tables[0].Rows[0][5].ToString();


                    //
                    //the fix for the country selection error
                    //
                    CountryConnection countryconn = new CountryConnection();
                    DataSet countriname = countryconn.getCountryNameByCode(athCountry);
                    athCountry = countriname.Tables[0].Rows[0][0].ToString();




                    for (int i = 0; i < this.cmbEditACountryChooses.Items.Count; i++)
                    {
                        if (athCountry.Equals(cmbEditACountryChooses.Items[i].ToString()))
                        {
                            country_position = i;
                            break;
                        }
                    }


                    this.cmbEditJCountryChooses.SelectedIndex = country_position;


                    CityConnection cityConnection = new CityConnection();
                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(athCity));
                    athCity = cityNa.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbEditAthleteCityChooses.Items.Count; i++)
                    {
                        if (athCity.Equals(cmbEditAthleteCityChooses.Items[i].ToString()))
                        {
                            city_position = i;
                            break;
                        }
                    }
                    this.cmbEditJCityChooses.SelectedIndex = city_position;


                    if (ds2.Tables[0].Rows.Count > 0)
                    {

                        rank = ds2.Tables[0].Rows[0][1].ToString();
                        judge_class = ds2.Tables[0].Rows[0][2].ToString();

                        //vriskei tin zwni pou exei o kathenas se poia thesi einai
                        for (int i = 0; i < cmbEditJudgeRankChooses.Items.Count; i++)
                        {
                            if (rank.Equals(cmbEditJudgeRankChooses.Items[i]))
                            {
                                rank_position = i;
                                break;
                            }
                        }
                        this.cmbEditJudgeRankChooses.SelectedIndex = rank_position;




                        for (int i = 0; i < cmbEditJClassChooses.Items.Count; i++)
                        {
                            if (judge_class.Equals(cmbEditJClassChooses.Items[i]))
                            {
                                class_position = i;
                                break;
                            }
                        }
                        this.cmbEditJClassChooses.SelectedIndex = class_position;

                    }
                    else if (ds3.Tables[0].Rows.Count > 0)
                    {
                        rank = ds3.Tables[0].Rows[0][1].ToString();

                        //vriskei tin zwni pou exei o kathenas se poia thesi einai
                        for (int i = 0; i < cmbEditJudgeRankChooses.Items.Count; i++)
                        {
                            if (rank.Equals(cmbEditJudgeRankChooses.Items[i]))
                            {
                                rank_position = i;
                                break;
                            }
                        }
                        this.cmbEditJudgeRankChooses.SelectedIndex = rank_position;
                    }

                }
                editJudgeFirstName.TextChanged += new TextChangedEventHandler(editJudgeFirstName_TextChanged);
            }
        }



        private void editJudgeList()
        {
            _editJudgeFirstName = editJudgeFirstName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            editJudgeNameListForAutoComplete = this.editJudgefiltersNames();

            foreach (ListData item in editJudgeNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                editJSuggestionList.ItemsSource = autoList;
                editJSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }


            else if (editJudgeFirstName.Text.Equals(""))
            {
                editJSuggestionList.Visibility = Visibility.Collapsed;
                editJSuggestionList.ItemsSource = null;
            }


            else
            {
                editJSuggestionList.Visibility = Visibility.Collapsed;
                editJSuggestionList.ItemsSource = null;
            }
        }

        #endregion

        private void editJudgeFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = editJudgeFirstName.Text;
            if (name == "")
            {
                initializeEditJudge();
            }
            editJudgeList();
        }

        private void editJudgeLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editJudgeLastName = editJudgeLastName.Text;
        }

        private void editJudgeFatherName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editJudgeFathersName = editJudgeFatherName.Text;
        }

        private void editJudgeDateOfBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Nullable<DateTime> selectedDate = editJudgeDateOfBirth.SelectedDate;
            if (selectedDate.HasValue)
                _editAthleteDateOfBirth = selectedDate.Value.ToShortDateString();
            else
                _editAthleteDateOfBirth = string.Empty;
        }

        private void editJudgeFirstPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editJudgeFirstPhone = editJudgeFirstPhone.Text;
        }

        private void editJudgeSecondPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editJudgeSecondPhone = editJudgeSecondPhone.Text;
        }

        private void editJudgeEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editJudgeEmail = editJudgeEmail.Text;
        }

        private void editJudgeStreetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editJudgeAddress = editJudgeStreetName.Text;
        }

        private void editJudgeAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editJudgeAddressNum = editJudgeAddressNum.Text;
        }

        private void editJudgeTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editJudgeTK = editJudgeTK.Text;
        }

        private void cmbEditJCityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditJCityChooses.SelectedIndex;
            if (index < cmbEditJCityChooses.Items.Count && index != -1)
                _editJudgeCity = cmbEditJCityChooses.Items[index].ToString();
        }

        private void cmbEditJCountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditJCountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            _editJudgeCountryCode = countries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this.editJudgeUpdateCities(_editJudgeCountryCode);
        }

        private void editJudgeUpdateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.cities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbEditJCityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbEditJCityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbEditJCityChooses.Items.Add(dr[1].ToString());
            }
            cmbEditJCityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbEditJCityChooses.Items.Refresh();
        }

        private void editJrdButton1_Checked(object sender, RoutedEventArgs e)
        {
            _editJudgeSex = Strings.male;
        }

        private void editJrdButton2_Checked(object sender, RoutedEventArgs e)
        {
            _editJudgeSex = Strings.female;
        }

        private void cmbEditJRankChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditJudgeRankChooses.SelectedIndex;
            _editJudgeRank = cmbEditAthleteRankChooses.Items[index].ToString();
        }

        private void cmbEditJClassChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditJClassChooses.SelectedIndex;
            _editJudgeClass = cmbEditJClassChooses.Items[index].ToString();
        }

        #region other listeners

        private void editJudgeFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            editJSuggestionList.Visibility = Visibility.Collapsed;
        }

        private void editJudgeFirstName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            editJudgeList();
        }

        #endregion

        #region buttons
        private void btnEditJBack_Click(object sender, RoutedEventArgs e)
        {

            this.sender.Show();
            this.Close();
        }

        private void btnEditJSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkNullOrEmptyFields("judge", false) && checkWrongFields("judge", false) && _editJudgeId != -1)
            {
                judgeConnection.UpdateJudge(_editJudgeId, _editJudgeFirstName, _editJudgeLastName, _editJudgeFathersName, _editJudgeSex, _editJudgeDateOfBirth, _editJudgeFirstPhone, _editJudgeSecondPhone, _editJudgeEmail, _editJudgeAddress, _editJudgeAddressNum, _editJudgeTK, _editJudgeCountryCode, _editJudgeCity, _editJudgeRank, _editJudgeClass);
                MessageBox.Show("Succesfully saved!","Judge Edit", MessageBoxButton.OK);
            }


        }

        private void btnEditJDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_editJudgeId != -1)
            {
                if (judgeConnection.deleteJudge(_editJudgeId))
                {
                    MessageBox.Show("Succesfully deleted!","Judge Delete",MessageBoxButton.OK);
                    initializeEditJudge();
                }
                else
                {
                    MessageBox.Show("Cannot be deleted because of tournament participation!","Judge Delete",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select one judge to delete.","Judge Delete",MessageBoxButton.OK,MessageBoxImage.Information);
            }


        }
        #endregion
        #endregion

        #region checks


        private bool checkNullOrEmptyFields(string person, bool newMode)
        {
            if (person.Equals("athlete"))
            {
                if (newMode)
                {
                    if (string.IsNullOrEmpty(_newAthleteFirstName))
                    {
                        em.nullErrorMessage("First Name");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_newAthleteLastName))
                    {
                        em.nullErrorMessage("Last Name");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_newAthleteSex))
                    {
                        em.nullErrorMessage("Sex");
                        return false;
                    }
                    else if(string.IsNullOrEmpty(_newAthleteDateOfBirth)){
                        em.nullErrorMessage("Date Of Birth");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_newAthleteFirstPhone))
                    {
                        em.nullErrorMessage("Phone Num.");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(_editAthleteFirstName))
                    {
                        em.nullErrorMessage("First Name");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_editAthleteLastName))
                    {
                        em.nullErrorMessage("Last Name");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_editAthleteSex))
                    {
                        em.nullErrorMessage("Sex");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_editAthleteDateOfBirth))
                    {
                        em.nullErrorMessage("Date Of Birth");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_editAthleteFirstPhone))
                    {
                        em.nullErrorMessage("Phone Num.");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (newMode)
                {
                    if (string.IsNullOrEmpty(_newJudgeFirstName))
                    {
                        em.nullErrorMessage("First Name");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_newJudgeLastName))
                    {
                        em.nullErrorMessage("Last Name");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_newJudgeSex))
                    {
                        em.nullErrorMessage("Sex");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_newJudgeDateOfBirth))
                    {
                        em.nullErrorMessage("Date Of Birth");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_newJudgeFirstPhone))
                    {
                        em.nullErrorMessage("Phone Num.");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(_editJudgeFirstName))
                    {
                        em.nullErrorMessage("First Name");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_editJudgeLastName))
                    {
                        em.nullErrorMessage("Last Name");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_editJudgeSex))
                    {
                        em.nullErrorMessage("Sex");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_editJudgeDateOfBirth))
                    {
                        em.nullErrorMessage("Date Of Birth");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_editJudgeFirstPhone))
                    {
                        em.nullErrorMessage("Phone Num.");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        private bool checkWrongFields(string person, bool newMode)
        {
            if (person.Equals("athlete"))
            {
                if (newMode)
                {
                    if (!regex.isCharsOrSpace(_newAthleteFirstName))
                    {
                        em.charsSpaceErrorMessage("First Name");
                        return false;
                    }
                    else if (!regex.isCharsOnly(_newAthleteLastName))
                    {
                        em.charsErrorMessage("Last Name");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newAthleteFathersName) && !regex.isCharsOrSpace(_newAthleteFathersName))
                    {

                        em.charsSpaceErrorMessage("Fathers Name");
                        return false;

                    }
                    else if (!regex.isDigitsOnly(_newAthleteFirstPhone))
                    {
                        em.digitsErrorMessage("Phone Num.");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newAthleteSecondPhone) && !regex.isDigitsOnly(_newAthleteSecondPhone))
                    {
                        em.digitsErrorMessage("Second Num.");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_newAthleteEmail) && !regex.isEmailValid(_newAthleteEmail))
                    {
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newAthleteAddress) && !regex.isCharsSpaceOrDots(_newAthleteAddress))
                    {
                        em.charsSpaceDotsErrorMessage("Address");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_newAthleteAddressNum) && !regex.isCharsOrDigits(_newAthleteAddressNum))
                    {

                        em.charsDigitsErrorMessage("Address Num.");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_newAthleteTK) && !regex.isDigitsOnly(_newAthleteTK))
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
                    if (!regex.isCharsOrSpace(_editAthleteFirstName))
                    {
                        em.charsSpaceErrorMessage("First Name");
                        return false;
                    }
                    else if (!regex.isCharsOnly(_editAthleteLastName))
                    {
                        em.charsErrorMessage("Last Name");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_editAthleteFathersName) && !regex.isCharsOrSpace(_editAthleteFathersName))
                    {
                        em.charsSpaceErrorMessage("Fathers Name");
                        return false;
                    }
                    else if (!regex.isDigitsOnly(_editAthleteFirstPhone))
                    {
                        em.digitsErrorMessage("Phone Num.");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_editAthleteSecondPhone) && !regex.isDigitsOnly(_editAthleteSecondPhone))
                    {
                        em.digitsErrorMessage("Second Num.");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_editAthleteEmail) && !regex.isEmailValid(_editAthleteEmail))
                    {
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_editAthleteAddress) && !regex.isCharsSpaceOrDots(_editAthleteAddress))
                    {
                        em.charsSpaceDotsErrorMessage("Address");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_editAthleteAddressNum) && !regex.isCharsOrDigits(_editAthleteAddressNum))
                    {
                        em.charsDigitsErrorMessage("Address Num.");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_editAthleteTK) && !regex.isDigitsOnly(_editAthleteTK))
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
            else
            {
                if (newMode)
                {
                    if (!regex.isCharsOrSpace(_newJudgeFirstName))
                    {
                        em.charsSpaceErrorMessage("First Name");
                        return false;
                    }
                    else if (!regex.isCharsOnly(_newJudgeLastName))
                    {
                        em.charsErrorMessage("Last Name");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newJudgeFathersName) && !regex.isCharsOrSpace(_newJudgeFathersName))
                    {

                        em.charsSpaceErrorMessage("Fathers Name");
                        return false;

                    }
                    else if (!regex.isDigitsOnly(_newJudgeFirstPhone))
                    {
                        em.digitsErrorMessage("Phone Num.");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newJudgeSecondPhone) && !regex.isDigitsOnly(_newJudgeSecondPhone))
                    {

                        em.digitsErrorMessage("Second Num.");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_newJudgeEmail) && !regex.isEmailValid(_newJudgeEmail))
                    {
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_newJudgeAddress) && !regex.isCharsSpaceOrDots(_newJudgeAddress))
                    {

                        em.charsSpaceDotsErrorMessage("Address");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_newJudgeAddressNum) && !regex.isCharsOrDigits(_newJudgeAddressNum))
                    {

                        em.charsDigitsErrorMessage("Address Num.");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_newJudgeTK) && !regex.isDigitsOnly(_newJudgeTK))
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
                    if (!regex.isCharsOrSpace(_editJudgeFirstName))
                    {
                        em.charsSpaceErrorMessage("First Name");
                        return false;
                    }
                    else if (!regex.isCharsOnly(_editJudgeLastName))
                    {
                        em.charsErrorMessage("Last Name");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_editJudgeFathersName) && !regex.isCharsOrSpace(_editJudgeFathersName))
                    {

                        em.charsSpaceErrorMessage("Fathers Name");
                        return false;

                    }
                    else if (!regex.isDigitsOnly(_editJudgeFirstPhone))
                    {
                        em.digitsErrorMessage("Phone Num.");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_editJudgeSecondPhone) && !regex.isDigitsOnly(_editJudgeSecondPhone))
                    {

                        em.digitsErrorMessage("Second Num.");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_editJudgeEmail) && !regex.isEmailValid(_editJudgeEmail))
                    {
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_editJudgeAddress) && !regex.isCharsSpaceOrDots(_editJudgeAddress))
                    {

                        em.charsSpaceDotsErrorMessage("Address");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_editJudgeAddressNum) && !regex.isCharsOrDigits(_editJudgeAddressNum))
                    {

                        em.charsDigitsErrorMessage("Address Num.");
                        return false;

                    }
                    else if (!string.IsNullOrEmpty(_editJudgeTK) && !regex.isDigitsOnly(_editJudgeTK))
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
        }



        #endregion

        #region initialize methods
        private void initializeNewAthlete()
        {
            _newPersonId = -1;              //gia na min menei to id apo to autocomplete mpike argotera mporei an dimiourgisei provlima
            NewAthleteFirstName.Text = null;
            NewAthleteLastName.Text = null;
            NewAthleteFatherName.Text = null;
            NewArdButton1.IsChecked = false;
            NewArdButton2.IsChecked = false;
            NewAthleteDateOfBirth.SelectedDate = null;
            NewAthleteFirstPhone.Text = null;
            NewAthleteSecondPhone.Text = null;
            NewAthleteEmail.Text = null;
            NewAthleteStreetName.Text = null;
            NewAthleteAddressNum.Text = null;
            NewAthleteTK.Text = null;
            cmbNewACountryChooses.SelectedIndex = 54;
            cmbNewAthleteRankChooses.SelectedIndex = 0;
            cmbNewAClubChooses.SelectedIndex = 0;
            Dispatcher.BeginInvoke(new Action(() => { NewAthleteFirstName.Focus(); }));  //dinei to focus sto newathletefirstName
        }

        private void initializeEditAthlete()
        {
            _editPersonId = -1;
            EditAthleteFirstName.Text = null;
            EditAthleteLastName.Text = null;
            EditAthleteFatherName.Text = null;
            EditArdButton1.IsChecked = false;
            EditArdButton2.IsChecked = false;
            EditAthleteDateOfBirth.SelectedDate = null;
            EditAthleteFirstPhone.Text = null;
            EditAthleteSecondPhone.Text = null;
            EditAthleteEmail.Text = null;
            EditAthleteStreetName.Text = null;
            EditAthleteAddressNum.Text = null;
            EditAthleteTK.Text = null;
            cmbEditACountryChooses.SelectedIndex = 54;
            cmbEditAthleteRankChooses.SelectedIndex = 0;
            cmbEditAClubChooses.SelectedIndex = 0;
            Dispatcher.BeginInvoke(new Action(() => { EditAthleteFirstName.Focus(); }));
        }

        private void initializeNewJudge()
        {
            _newJudgeId = -1;
            newJudgeFirstName.Text = null;
            newJudgeLastName.Text = null;
            newJudgeFatherName.Text = null;
            newJrdButton1.IsChecked = false;
            newJrdButton2.IsChecked = false;
            newJudgeDateOfBirth.SelectedDate = null;
            newJudgeFirstPhone.Text = null;
            newJudgeSecondPhone.Text = null;
            newJudgeEmail.Text = null;
            newJudgeStreetName.Text = null;
            newJudgeAddressNum.Text = null;
            newJudgeTK.Text = null;
            cmbNewJCountryChooses.SelectedIndex = 54;
            cmbNewJudgeRankChooses.SelectedIndex = 0;
            cmbNewJClassChooses.SelectedIndex = 0;
            Dispatcher.BeginInvoke(new Action(() => { newJudgeFirstName.Focus(); }));
        }

        private void initializeEditJudge()
        {
            _editJudgeId = -1;
            editJudgeFirstName.Text = null;
            editJudgeLastName.Text = null;
            editJudgeFatherName.Text = null;
            editJrdButton1.IsChecked = false;
            editJrdButton2.IsChecked = false;
            editJudgeDateOfBirth.SelectedDate = null;
            editJudgeFirstPhone.Text = null;
            editJudgeSecondPhone.Text = null;
            editJudgeEmail.Text = null;
            editJudgeStreetName.Text = null;
            editJudgeAddressNum.Text = null;
            editJudgeTK.Text = null;
            cmbEditJCountryChooses.SelectedIndex = 54;
            cmbEditJudgeRankChooses.SelectedIndex = 0;
            cmbEditJClassChooses.SelectedIndex = 0;
            Dispatcher.BeginInvoke(new Action(() => { editJudgeFirstName.Focus(); }));
        }


        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
