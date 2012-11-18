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

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for PersonManagement.xaml
    /// </summary>
    public partial class PersonManagement : Window
    {
        //
        //application specific variables
        //
        private DataSet cities;
        private DataSet countries;
        private DataSet filteredAthletes;
        private DataSet filteredJudges;
        private DataSet clubs;
        private AddressConnection addressConnection;
        private CountryConnection countryConnection;
        private PersonConnection personsConnection = new PersonConnection();
        private ClubConnection clubConnection = new ClubConnection();
        List<ListData> athleteNameListForAutoComplete;
        List<ListData> judgeNameListForAutoComplete;

        //
        //athlete specific variables
        //

        private AthleteConnection athleteConnection;
        private int _personId;
        private string _athleteFirstName = null;
        private string _athleteLastName = null;
        private string _athleteFathersName = null;
        private string _athleteSex = null;
        private string _athleteFirstPhone = null;
        private string _athleteSecondPhone = null;
        private string _athleteEmail = null;
        private string _athleteAddress = null;
        private string _athleteAddressNum = null;
        private string _athleteTK = null;
        private string _athleteCity = null;
        private string _athleteCountryCode = null;
        private string _athleteRank = null;
        private string _athleteClubId = null;
        private DateTime _athleteDateOfBirth;


        //
        //judge specific variables
        //
        // afta mpori na allaksoun katalila mixali aplos sta etimasa
        //
        private JudgeConnection judgeConnection;
        private string _judgeFirstName = null;
        private string _judgeLastName = null;
        private string _judgeFathersName = null;
        private string _judgeSex = null;
        private string _judgeFirstPhone = null;
        private string _judgeSecondPhone = null;
        private string _judgeEmail = null;
        private string _judgeAddress = null;
        private string _judgeAddressNum = null;
        private string _judgeTK = null;
        private string _judgeCity = null;
        private string _judgeCountryCode = null;
        private string _judgeRank = null;
        private string _judgeClass = null;
        private DateTime _judgeDateOfBirth;


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

            athleteConnection = new AthleteConnection();
            judgeConnection = new JudgeConnection();
            //
            //NOTE
            //
            //mixali listeners vaze katefthian apo to xaml oxi karfota mesa ston kodika
            //
            //athleteFirstName.TextChanged += new TextChangedEventHandler(athleteFirstName_TextChanged);
            //prostetoume cities oses theloume



            countryConnection = new CountryConnection();
            this.countries = countryConnection.GetCountries();

            foreach (DataRow dr in countries.Tables[0].Rows)
            {
                cmbACountryChooses.Items.Add(dr[1].ToString());
                cmbJCountryChooses.Items.Add(dr[1].ToString());
            }
            cmbACountryChooses.SelectedIndex = 54;
            cmbJCountryChooses.SelectedIndex = 54;

            this.athleteUpdateCities("CY");



            //prosthetoume athlete_rank
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank01);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank02);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank03);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank04);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank05);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank06);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank07);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank08);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank09);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank10);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank11);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank12);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank13);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank14);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank15);
            cmbAthleteRankChooses.Items.Add(KarateGeek.Strings.rank16);

            cmbAthleteRankChooses.SelectedIndex = 0;

            //string s = Strings.ranks.rank01.ToString(); ;

            //prosthetoume judge class
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank01);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank02);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank03);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank04);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank05);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank06);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank07);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank08);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank09);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank10);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank11);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank12);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank13);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank14);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank15);
            cmbJudgeRankChooses.Items.Add(KarateGeek.Strings.rank16);



            cmbJudgeRankChooses.SelectedIndex = 0;

            //prosthetoume clubs
            this.clubs = clubConnection.GetClubs();

            foreach (DataRow dr in clubs.Tables[0].Rows)
            {
                cmbAClubChooses.Items.Add(dr[1]);


            }
            cmbAClubChooses.SelectedIndex = 0;
            //cmbAClubChooses.Items.Add("Pro Kata Club (P.K.C.)");

            //cmbAClubChooses.Items.Add("Allo Club");

            //prosthetoume judge classes
            cmbJClassChooses.Items.Add("A");
            cmbJClassChooses.Items.Add("B");
            cmbJClassChooses.SelectedIndex = 0;


        }

        //private Boolean nameflag = true;


        #region athlete specific code



        private void athleteFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            athleteList();
        }



        private List<ListData> AthletesfilterNames()
        {
            List<ListData> list = new List<ListData>();

            this.filteredAthletes = personsConnection.findSimilar(this.athleteFirstName.Text);

            foreach (DataRow dr in filteredAthletes.Tables[0].Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString() + " " + dr[2].ToString();
                list.Add(suggestion);
            }
            return list;
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Visible;
            //this.sugestionList.ItemsSource = list;
        }



        private void aSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

            if (aSuggestionList.ItemsSource != null)
            {
                aSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                athleteFirstName.TextChanged -= new TextChangedEventHandler(athleteFirstName_TextChanged);

                index = aSuggestionList.SelectedIndex;

                if (aSuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)aSuggestionList.SelectedItem;
                    _personId = item.id;
                    ds2 = athleteConnection.findAthlete(_personId);
                    ds3 = athleteConnection.findAthleteClub(_personId);

                    name = aSuggestionList.SelectedItem.ToString();
                    sex = filteredAthletes.Tables[0].Rows[index][4].ToString();

                    address_id = int.Parse(filteredAthletes.Tables[0].Rows[index][9].ToString());

                    this.athleteFirstName.Text = filteredAthletes.Tables[0].Rows[index][1].ToString();
                    this._athleteFirstName = this.athleteFirstName.Text;
                    this.athleteLastName.Text = filteredAthletes.Tables[0].Rows[index][2].ToString();
                    this.athleteFatherName.Text = filteredAthletes.Tables[0].Rows[index][3].ToString();

                    if (sex.Equals("male"))
                    {
                        this.ArdButton1.IsChecked = true;
                    }
                    else
                    {
                        this.ArdButton2.IsChecked = true;
                    }

                    this.athleteDateOfBirth.SelectedDate = (DateTime)filteredAthletes.Tables[0].Rows[index][5];
                    this.athleteFirstPhone.Text = filteredAthletes.Tables[0].Rows[index][6].ToString();
                    this.athleteSecondPhone.Text = filteredAthletes.Tables[0].Rows[index][7].ToString();
                    this.athleteEmail.Text = filteredAthletes.Tables[0].Rows[index][8].ToString();


                    ds = addressConnection.getAddress(address_id);

                    this.athleteStreetName.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.athleteAddressNum.Text = ds.Tables[0].Rows[0][2].ToString();
                    this.athleteTK.Text = ds.Tables[0].Rows[0][4].ToString();

                    string athCity = ds.Tables[0].Rows[0][3].ToString();
                    int ix = ds.Tables[0].Columns.Count;
                    string athCountry = ds.Tables[0].Rows[0][5].ToString();


                    //
                    //the fix for the country selection error
                    //
                    CountryConnection countryconn = new CountryConnection();
                    DataSet countriname = countryconn.getCountryNameByCode(athCountry);
                    athCountry = countriname.Tables[0].Rows[0][0].ToString();




                    for (int i = 0; i < this.cmbACountryChooses.Items.Count; i++)
                    {
                        if (athCountry.Equals(cmbACountryChooses.Items[i].ToString()))
                        {
                            country_position = i;
                            break;
                        }
                    }


                    this.cmbACountryChooses.SelectedIndex = country_position;


                    CityConnection cityConnection = new CityConnection();
                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(athCity));
                    athCity = cityNa.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbAthleteCityChooses.Items.Count; i++)
                    {
                        if (athCity.Equals(cmbAthleteCityChooses.Items[i].ToString()))
                        {
                            city_position = i;
                            break;
                        }
                    }
                    this.cmbAthleteCityChooses.SelectedIndex = city_position;

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        rank = ds2.Tables[0].Rows[0][1].ToString();
                        //vriskei tin zwni pou exei o kathenas se poia thesi einai
                        for (int i = 0; i < cmbAthleteRankChooses.Items.Count; i++)
                        {
                            if (rank.Equals(cmbAthleteRankChooses.Items[i]))
                            {
                                rank_position = i;
                                break;
                            }
                        }
                        this.cmbAthleteRankChooses.SelectedIndex = rank_position;

                    }

                    if (ds3.Tables[0].Rows.Count > 0)
                    {

                        club = ds3.Tables[0].Rows[0][4].ToString();
                        //vriskei to club pou einai o kathenas
                        for (int i = 0; i < cmbAClubChooses.Items.Count; i++)
                        {
                            if (club.Equals(cmbAClubChooses.Items[i]))
                            {
                                club_position = i;
                                break;
                            }
                        }
                        this.cmbAClubChooses.SelectedIndex = club_position;
                    }
                }
                athleteFirstName.TextChanged += new TextChangedEventHandler(athleteFirstName_TextChanged);
            }
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Hidden;
        }


        private void athleteLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _athleteLastName = athleteLastName.Text;

        }


        private void athleteFathersName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _athleteFathersName = athleteFatherName.Text;

        }


        private void athleteDateOfBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _athleteDateOfBirth = athleteDateOfBirth.SelectedDate.Value;
        }


        private void athleteFirstPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _athleteFirstPhone = athleteFirstPhone.Text;
        }


        private void athleteSecondPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _athleteSecondPhone = athleteSecondPhone.Text;
        }


        private void athleteEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _athleteEmail = athleteEmail.Text;
        }


        private void athleteStreetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _athleteAddress = athleteStreetName.Text;
        }


        private void athleteAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _athleteAddressNum = athleteAddressNum.Text;
        }

        private void athleteTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _athleteTK = athleteTK.Text;
        }

        private void cmbACityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbAthleteCityChooses.SelectedIndex;
            if (index < cmbAthleteCityChooses.Items.Count && index != -1)
                _athleteCity = cmbAthleteCityChooses.Items[index].ToString();
        }

        private void cmbACountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbACountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            _athleteCountryCode = countries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this.athleteUpdateCities(_athleteCountryCode);
        }


        private void athleteUpdateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.cities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbAthleteCityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbAthleteCityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbAthleteCityChooses.Items.Add(dr[1].ToString());
            }
            cmbAthleteCityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbAthleteCityChooses.Items.Refresh();




        }



        private void cmbARankChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbAthleteRankChooses.SelectedIndex;
            _athleteRank = cmbAthleteRankChooses.Items[index].ToString();
        }

        private void cmbAClubChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbAClubChooses.SelectedIndex;
            _athleteClubId = clubs.Tables[0].Rows[index][0].ToString();
        }

        private void ArdButton1_Checked(object sender, RoutedEventArgs e)
        {
            _athleteSex = "male";
        }

        private void ArdButton2_Checked(object sender, RoutedEventArgs e)
        {
            _athleteSex = "female";
        }

        private void btnABack_Click(object sender, RoutedEventArgs e)
        {
            this.sender.Show();
            this.Close();
        }

        private void btnASave_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields("athlete"))
            {
                athleteConnection.UpdateAthlete(_personId, _athleteFirstName, _athleteLastName, _athleteFathersName, _athleteSex, _athleteDateOfBirth, _athleteFirstPhone, _athleteSecondPhone, _athleteEmail, _athleteAddress, _athleteAddressNum, _athleteTK, _athleteCountryCode, _athleteCity, _athleteRank, _athleteClubId);
                MessageBox.Show("Succesfully saved!");
            }
        }

        private void btnASaveNew_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields("athlete"))
            {

                athleteConnection.InsertNewAthlete(_personId ,_athleteFirstName, _athleteLastName, _athleteFathersName, _athleteSex, _athleteDateOfBirth, _athleteFirstPhone, _athleteSecondPhone, _athleteEmail, _athleteAddress, _athleteAddressNum, _athleteTK, _athleteCountryCode, _athleteCity, _athleteRank, _athleteClubId);
                MessageBox.Show("Succesfully saved!");
                PersonManagement pm = new PersonManagement(this);
                pm.Activate();
                pm.Show();
                this.Close();
            }

        }


        private void btnAthleteDelete_Click(object sender, RoutedEventArgs e)
        {

            if (athleteConnection.deleteAthlete(_personId))
            {
                MessageBox.Show("Succesfully deleted!");
                PersonManagement pm = new PersonManagement(this);
                pm.Activate();
                pm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Cannot be deleted because of tournament participation!");
                PersonManagement pm = new PersonManagement(this);
                pm.Activate();
                pm.Show();
                this.Close();
            }

        }




        //
        // dead code alla asto kalou kakou
        //
        //private void setCountryCode(string country)
        //{
        //    if (country.Equals("Cyprus"))
        //    {
        //        athlete_country_code = "CY";
        //    }
        //    else if (country.Equals("Greece"))
        //    {
        //        athlete_country_code = "GR";
        //    }
        //}


        //
        //mixali min frikaris afto einai gia na grouparis kodika
        //arxizi me #region <name>
        //lai telion me #endregion
        //
        #endregion


        #region judge specific code













        private void judgeFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            judgeList();
        }



        private List<ListData> JudgefilterNames()
        {
            List<ListData> list = new List<ListData>();

            this.filteredJudges = personsConnection.findSimilar(this.judgeFirstName.Text);



            foreach (DataRow dr in filteredJudges.Tables[0].Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString() + " " + dr[2].ToString();
                list.Add(suggestion);
            }
            return list;
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Visible;
            //this.sugestionList.ItemsSource = list;
        }



        private void jSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

            if (jSuggestionList.ItemsSource != null)
            {
                jSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                judgeFirstName.TextChanged -= new TextChangedEventHandler(judgeFirstName_TextChanged);

                int index = jSuggestionList.SelectedIndex;

                if (jSuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)jSuggestionList.SelectedItem;
                    _personId = item.id;
                    ds2 = judgeConnection.findJudge(_personId);
                    ds3 = athleteConnection.findAthlete(_personId);

                    name = jSuggestionList.SelectedItem.ToString();
                    sex = filteredJudges.Tables[0].Rows[index][4].ToString();

                    address_id = int.Parse(filteredJudges.Tables[0].Rows[index][9].ToString());

                    //
                    //giati mazoxizese re mike??
                    //
                    //this.athleteFirstName.Text = name.Substring(0, name.IndexOf(" "));
                    //

                    this.judgeFirstName.Text = filteredJudges.Tables[0].Rows[index][1].ToString();
                    this._judgeFirstName = this.judgeFirstName.Text;
                    this.judgeLastName.Text = filteredJudges.Tables[0].Rows[index][2].ToString();
                    this.judgeFatherName.Text = filteredJudges.Tables[0].Rows[index][3].ToString();

                    if (sex.Equals("male"))
                    {
                        this.JrdButton1.IsChecked = true;
                    }
                    else
                    {
                        this.JrdButton2.IsChecked = true;
                    }

                    this.judgeDateOfBirth.SelectedDate = (DateTime)filteredJudges.Tables[0].Rows[index][5];
                    this.judgeFirstPhone.Text = filteredJudges.Tables[0].Rows[index][6].ToString();
                    this.judgeSecondPhone.Text = filteredJudges.Tables[0].Rows[index][7].ToString();
                    this.judgeEmail.Text = filteredJudges.Tables[0].Rows[index][8].ToString();

                    ds = addressConnection.getAddress(address_id);

                    this.judgeStreetName.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.judgeAddressNum.Text = ds.Tables[0].Rows[0][2].ToString();
                    this.judgeTK.Text = ds.Tables[0].Rows[0][4].ToString();

                    string athCity = ds.Tables[0].Rows[0][3].ToString();
                    int ix = ds.Tables[0].Columns.Count;
                    string athCountry = ds.Tables[0].Rows[0][5].ToString();


                    //
                    //the fix for the country selection error
                    //
                    CountryConnection countryconn = new CountryConnection();
                    DataSet countriname = countryconn.getCountryNameByCode(athCountry);
                    athCountry = countriname.Tables[0].Rows[0][0].ToString();




                    for (int i = 0; i < this.cmbACountryChooses.Items.Count; i++)
                    {
                        if (athCountry.Equals(cmbACountryChooses.Items[i].ToString()))
                        {
                            country_position = i;
                            break;
                        }
                    }


                    this.cmbJCountryChooses.SelectedIndex = country_position;


                    CityConnection cityConnection = new CityConnection();
                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(athCity));
                    athCity = cityNa.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbAthleteCityChooses.Items.Count; i++)
                    {
                        if (athCity.Equals(cmbAthleteCityChooses.Items[i].ToString()))
                        {
                            city_position = i;
                            break;
                        }
                    }
                    this.cmbJCityChooses.SelectedIndex = city_position;


                    if (ds2.Tables[0].Rows.Count > 0)
                    {

                        rank = ds2.Tables[0].Rows[0][1].ToString();
                        judge_class = ds2.Tables[0].Rows[0][2].ToString();

                        //vriskei tin zwni pou exei o kathenas se poia thesi einai
                        for (int i = 0; i < cmbJudgeRankChooses.Items.Count; i++)
                        {
                            if (rank.Equals(cmbJudgeRankChooses.Items[i]))
                            {
                                rank_position = i;
                                break;
                            }
                        }
                        this.cmbJudgeRankChooses.SelectedIndex = rank_position;




                        for (int i = 0; i < cmbJClassChooses.Items.Count; i++)
                        {
                            if (judge_class.Equals(cmbJClassChooses.Items[i]))
                            {
                                class_position = i;
                                break;
                            }
                        }
                        this.cmbJClassChooses.SelectedIndex = class_position;

                    }
                    else if (ds3.Tables[0].Rows.Count > 0)
                    {
                        rank = ds3.Tables[0].Rows[0][1].ToString();

                        //vriskei tin zwni pou exei o kathenas se poia thesi einai
                        for (int i = 0; i < cmbJudgeRankChooses.Items.Count; i++)
                        {
                            if (rank.Equals(cmbJudgeRankChooses.Items[i]))
                            {
                                rank_position = i;
                                break;
                            }
                        }
                        this.cmbJudgeRankChooses.SelectedIndex = rank_position;
                    }

                }
                judgeFirstName.TextChanged += new TextChangedEventHandler(judgeFirstName_TextChanged);
            }
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Hidden;
        }






        private void judgeLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _judgeLastName = judgeLastName.Text;
        }

        private void judgeFatherName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _judgeFathersName = judgeFatherName.Text;
        }

        private void judgeDateOfBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _judgeDateOfBirth = judgeDateOfBirth.SelectedDate.Value;
        }

        private void judgeFirstPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _judgeFirstPhone = judgeFirstPhone.Text;
        }

        private void judgeSecondPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _judgeSecondPhone = judgeSecondPhone.Text;
        }

        private void judgeEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _judgeEmail = judgeEmail.Text;
        }

        private void judgeStreetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _judgeAddress = judgeStreetName.Text;
        }

        private void judgeAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _judgeAddressNum = judgeAddressNum.Text;
        }

        private void judgeTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _judgeTK = judgeTK.Text;
        }

        private void cmbJCityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbJCityChooses.SelectedIndex;
            if (index < cmbJCityChooses.Items.Count && index != -1)
                _judgeCity = cmbJCityChooses.Items[index].ToString();
        }

        private void cmbJCountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbJCountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            _judgeCountryCode = countries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this.judgeUpdateCities(_judgeCountryCode);
        }

        private void judgeUpdateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.cities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbJCityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbJCityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbJCityChooses.Items.Add(dr[1].ToString());
            }
            cmbJCityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbJCityChooses.Items.Refresh();
        }

        private void JrdButton1_Checked(object sender, RoutedEventArgs e)
        {
            _judgeSex = "male";
        }

        private void JrdButton2_Checked(object sender, RoutedEventArgs e)
        {
            _judgeSex = "female";
        }

        private void cmbJRankChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbJudgeRankChooses.SelectedIndex;
            _judgeRank = cmbAthleteRankChooses.Items[index].ToString();
        }

        private void cmbJClassChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbJClassChooses.SelectedIndex;
            _judgeClass = cmbJClassChooses.Items[index].ToString();
        }

        private void btnJBack_Click(object sender, RoutedEventArgs e)
        {
           
            this.sender.Show();
            this.Close();
        }

        private void btnJSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields("judge"))
            {
                judgeConnection = new JudgeConnection();
                judgeConnection.UpdateJudge(_personId, _judgeFirstName, _judgeLastName, _judgeFathersName, _judgeSex, _judgeDateOfBirth, _judgeFirstPhone, _judgeSecondPhone, _judgeEmail, _judgeAddress, _judgeAddressNum, _judgeTK, _judgeCountryCode, _judgeCity, _judgeRank, _judgeClass);
                MessageBox.Show("Succesfully saved!");
            }
        }
        private void btnJSaveNew_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields("judge"))
            {
                judgeConnection = new JudgeConnection();


                judgeConnection.InsertNewJudge(_judgeFirstName, _judgeLastName, _judgeFathersName, _judgeSex, _judgeDateOfBirth, _judgeFirstPhone, _judgeSecondPhone, _judgeEmail, _judgeAddress, _judgeAddressNum, _judgeTK, _judgeCountryCode, _judgeCity, _judgeRank, _judgeClass);
                MessageBox.Show("Succesfully saved!");
                PersonManagement pm = new PersonManagement(this);
                pm.Activate();
                pm.Show();
                this.Hide();
            }
        }

        private void btnJDelete_Click(object sender, RoutedEventArgs e)
        {
            judgeConnection = new JudgeConnection();
            if (judgeConnection.deleteJudge(_personId))
            {
                MessageBox.Show("Succesfully deleted!");
                PersonManagement pm = new PersonManagement(this);
                pm.Activate();
                pm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Cannot be deleted because of tournament participation!");
                PersonManagement pm = new PersonManagement(this);
                pm.Activate();
                pm.Show();
                this.Close();
            }

            
            
        }










        #endregion


        #region checks


        private bool checkFields(string person)
        {
            if (person.Equals("athlete"))
            {
                if (_athleteFirstName == null)
                {
                    errorMessage("First Name");
                    return false;
                }
                else if (_athleteLastName == null)
                {
                    errorMessage("Last Name");
                    return false;
                }
                else if (_athleteSex == null)
                {
                    errorMessage("Sex");
                    return false;
                }
                else if (_athleteFirstPhone == null)
                {
                    errorMessage("Phone Num.");
                    return false;
                }
                else if (_athleteEmail == null)
                {
                    errorMessage("Email");
                    return false;
                }
                else if (_athleteAddress == null)
                {
                    errorMessage("Address");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (_judgeFirstName == null)
                {
                    errorMessage("First Name");
                    return false;
                }
                else if (_judgeLastName == null)
                {
                    errorMessage("Last Name");
                    return false;
                }
                else if (_judgeSex == null)
                {
                    errorMessage("Sex");
                    return false;
                }
                else if (_judgeFirstPhone == null)
                {
                    errorMessage("Phone Num.");
                    return false;
                }
                else if (_judgeEmail == null)
                {
                    errorMessage("Email");
                    return false;
                }
                else if (_judgeAddress == null)
                {
                    errorMessage("Address");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private void errorMessage(string formField)
        {
            string message = "Please complete" + formField;
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }
        #endregion

      



        private void athleteList()
        {
            _athleteFirstName = athleteFirstName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            athleteNameListForAutoComplete = this.AthletesfilterNames();

            foreach (ListData item in athleteNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                aSuggestionList.ItemsSource = autoList;
                aSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (athleteFirstName.Text.Equals(""))
            {
                aSuggestionList.Visibility = Visibility.Collapsed;
                aSuggestionList.ItemsSource = null;
            }
            else
            {
                aSuggestionList.Visibility = Visibility.Collapsed;
                aSuggestionList.ItemsSource = null;
            }
        }

        private void judgeList()
        {
            _judgeFirstName = judgeFirstName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            judgeNameListForAutoComplete = this.JudgefilterNames();

            foreach (ListData item in judgeNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                jSuggestionList.ItemsSource = autoList;
                jSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }


            else if (judgeFirstName.Text.Equals(""))
            {
                jSuggestionList.Visibility = Visibility.Collapsed;
                jSuggestionList.ItemsSource = null;
            }


            else
            {
                jSuggestionList.Visibility = Visibility.Collapsed;
                jSuggestionList.ItemsSource = null;
            }
        }

     

    }
}
