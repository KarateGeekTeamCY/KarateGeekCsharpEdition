﻿using System;
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
        CountryConnection countryConnection;
        private ClubConnection clubConnection = new ClubConnection();
        List<string> athleteNameListForAutoComplete;
        List<string> judgeNameListForAutoComplete;

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
        

        public PersonManagement()
        {
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
            cmbACountryChooses.SelectedIndex = 0;
            cmbJCountryChooses.SelectedIndex = 0;

            this.athleteUpdateCities("CY");

            //9th dan - 10th dan- Wide Red optional as Sensei Ilija Yorga himself wore a Black Belt, Master's title
            //6th dan - 8th dan - White and Red, Master's title
            //1st dan - 5th dan - Black
            //1st kyu - Brown (at least 25 Month of traning or more)
            //2nd kyu - Blue (at least 18 Month of traning or more)
            //3rd kyu - Green (at least 15 Month of traning or more)
            //4th kyu - Orange (at least 9 Month of traning or more)
            //5th kyu - Yellow (at least 7 Month of traning or more)
            //6th kyu - White (at least 5 Month of traning or more)

            //prosthetoume athlete_rank
            cmbAthleteRankChooses.Items.Add("White        - 6th kyu");
            cmbAthleteRankChooses.Items.Add("Yellow       - 5th kyu");
            cmbAthleteRankChooses.Items.Add("Orange       - 4th kyu");
            cmbAthleteRankChooses.Items.Add("Green        - 3th kyu");
            cmbAthleteRankChooses.Items.Add("Blue         - 2nd kyu");
            cmbAthleteRankChooses.Items.Add("Brown        - 1st kyu");
            cmbAthleteRankChooses.Items.Add("Black        - 1st dan");
            cmbAthleteRankChooses.Items.Add("Black        - 2nd dan");
            cmbAthleteRankChooses.Items.Add("Black        - 3th dan");
            cmbAthleteRankChooses.Items.Add("Black        - 4th dan");
            cmbAthleteRankChooses.Items.Add("Black        - 5th dan");
            cmbAthleteRankChooses.Items.Add("White/Red    - 6th dan");
            cmbAthleteRankChooses.Items.Add("White/Red    - 7th dan");
            cmbAthleteRankChooses.Items.Add("White/Red    - 8th dan");
            cmbAthleteRankChooses.Items.Add("Red          - 9th dan");
            cmbAthleteRankChooses.Items.Add("Red          - 10th dan");
            cmbAthleteRankChooses.SelectedIndex = 0;

            //prosthetoume judge class
            cmbJudgeRankChooses.Items.Add("White        - 6th kyu");
            cmbJudgeRankChooses.Items.Add("Yellow       - 5th kyu");
            cmbJudgeRankChooses.Items.Add("Orange       - 4th kyu");
            cmbJudgeRankChooses.Items.Add("Green        - 3th kyu");
            cmbJudgeRankChooses.Items.Add("Blue         - 2nd kyu");
            cmbJudgeRankChooses.Items.Add("Brown        - 1st kyu");
            cmbJudgeRankChooses.Items.Add("Black        - 1st dan");
            cmbJudgeRankChooses.Items.Add("Black        - 2nd dan");
            cmbJudgeRankChooses.Items.Add("Black        - 3th dan");
            cmbJudgeRankChooses.Items.Add("Black        - 4th dan");
            cmbJudgeRankChooses.Items.Add("Black        - 5th dan");
            cmbJudgeRankChooses.Items.Add("White/Red    - 6th dan");
            cmbJudgeRankChooses.Items.Add("White/Red    - 7th dan");
            cmbJudgeRankChooses.Items.Add("White/Red    - 8th dan");
            cmbJudgeRankChooses.Items.Add("Red          - 9th dan");
            cmbJudgeRankChooses.Items.Add("Red          - 10th dan");
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
            _athleteFirstName = athleteFirstName.Text;
            List<string> autoList = new List<string>();
            autoList.Clear();

            athleteNameListForAutoComplete = this.AthletesfilterNames();

            foreach (string item in athleteNameListForAutoComplete)
            {
                if (!string.IsNullOrEmpty(athleteFirstName.Text))
                {
                    if (item.StartsWith(_athleteFirstName))
                    {
                        autoList.Add(item);
                    }
                }
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

        

        private List<string> AthletesfilterNames()
        {
            List<string> list = new List<string>();
            string suggestion = null;

            this.filteredAthletes = athleteConnection.findSimilar(this.athleteFirstName.Text);

            if (this.filteredAthletes.Tables[0].Rows.Count >0)
            {
                _personId = int.Parse(filteredAthletes.Tables[0].Rows[0][0].ToString());
            }
            
            foreach (DataRow dr in filteredAthletes.Tables[0].Rows)
            {
                suggestion = dr[1].ToString() + " " + dr[2].ToString();
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
            int rank_position = 0;
            int country_position = 0;
            int city_position = 0;
            int index;
            int address_id;
            addressConnection = new AddressConnection();
            DataSet ds;

            if (aSuggestionList.ItemsSource != null)
            {
                aSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                athleteFirstName.TextChanged -= new TextChangedEventHandler(athleteFirstName_TextChanged);

                index = aSuggestionList.SelectedIndex;
                
                if (aSuggestionList.SelectedIndex != -1)
                {
                    name = aSuggestionList.SelectedItem.ToString();
                    sex = filteredAthletes.Tables[0].Rows[index][4].ToString();
                    rank = filteredAthletes.Tables[0].Rows[index][11].ToString();
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
                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(athCity) );
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
            MainMenu menu = new MainMenu();
            menu.Activate();
            this.Close();
            menu.Show();
        }

        private void btnASave_Click(object sender, RoutedEventArgs e)
        {
            athleteConnection.UpdateAthlete(_personId, _athleteFirstName, _athleteLastName, _athleteFathersName, _athleteSex, _athleteDateOfBirth, _athleteFirstPhone, _athleteSecondPhone, _athleteEmail, _athleteAddress, _athleteAddressNum, _athleteTK , _athleteCountryCode, _athleteCity, _athleteRank, _athleteClubId);
            MessageBox.Show("Succesfully saved!");
        }

        private void btnASaveNew_Click(object sender, RoutedEventArgs e)
        {
            athleteConnection.InsertNewAthlete(_athleteFirstName, _athleteLastName, _athleteFathersName, _athleteSex, _athleteDateOfBirth, _athleteFirstPhone, _athleteSecondPhone, _athleteEmail, _athleteAddress, _athleteAddressNum, _athleteTK , _athleteCountryCode, _athleteCity, _athleteRank, _athleteClubId);
            MessageBox.Show("Succesfully saved!");
            PersonManagement pm = new PersonManagement();
            pm.Activate();
            pm.Show();
            this.Close();
        }

        private void btnAthleteDelete_Click(object sender, RoutedEventArgs e)
        {
            athleteConnection.deleteAthlete(_personId);
            MessageBox.Show("Succesfully deleted!");
            PersonManagement pm = new PersonManagement();
            pm.Activate();
            pm.Show();
            this.Close();
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
            _judgeFirstName = judgeFirstName.Text;
            List<string> autoList = new List<string>();
            autoList.Clear();

            judgeNameListForAutoComplete = this.JudgefilterNames();

            foreach (string item in judgeNameListForAutoComplete)
            {
                if (!string.IsNullOrEmpty(judgeFirstName.Text))
                {
                    if (item.StartsWith(_judgeFirstName))
                    {
                        autoList.Add(item);
                    }
                }
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



        private List<string> JudgefilterNames()
        {
            List<string> list = new List<string>();
            string suggestion = null;

            this.filteredJudges = judgeConnection.findSimilar(this.judgeFirstName.Text);

            if (this.filteredJudges.Tables[0].Rows.Count > 0)
            {
                _personId = int.Parse(filteredJudges.Tables[0].Rows[0][0].ToString());
            }
            
            foreach (DataRow dr in filteredJudges.Tables[0].Rows)
            {
                suggestion = dr[1].ToString() + " " + dr[2].ToString();
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

            if (jSuggestionList.ItemsSource != null)
            {
                jSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                judgeFirstName.TextChanged -= new TextChangedEventHandler(judgeFirstName_TextChanged);

                int index = jSuggestionList.SelectedIndex;

                if (jSuggestionList.SelectedIndex != -1)
                {
                    name = jSuggestionList.SelectedItem.ToString();
                    sex = filteredJudges.Tables[0].Rows[index][4].ToString();
                    rank = filteredJudges.Tables[0].Rows[index][11].ToString();
                    judge_class = filteredJudges.Tables[0].Rows[index][12].ToString();
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
            MainMenu menu = new MainMenu();
            menu.Activate();
            this.Close();
            menu.Show();
        }

        private void btnJSave_Click(object sender, RoutedEventArgs e)
        {
            judgeConnection = new JudgeConnection();
            judgeConnection.UpdateJudge(_personId, _judgeFirstName, _judgeLastName, _judgeFathersName, _judgeSex, _judgeDateOfBirth, _judgeFirstPhone, _judgeSecondPhone, _judgeEmail, _judgeAddress, _judgeAddressNum, _judgeTK , _judgeCountryCode, _judgeCity, _judgeRank, _judgeClass);
            MessageBox.Show("Succesfully saved!");
        }
        private void btnJSaveNew_Click(object sender, RoutedEventArgs e)
        {
            judgeConnection = new JudgeConnection();


            judgeConnection.InsertNewJudge(_judgeFirstName, _judgeLastName, _judgeFathersName, _judgeSex, _judgeDateOfBirth, _judgeFirstPhone, _judgeSecondPhone, _judgeEmail, _judgeAddress, _judgeAddressNum, _judgeTK , _judgeCountryCode, _judgeCity, _judgeRank, _judgeClass);
            MessageBox.Show("Succesfully saved!");
            PersonManagement pm = new PersonManagement();
            pm.Activate();
            pm.Show();
            this.Hide();
        }

        private void btnJDelete_Click(object sender, RoutedEventArgs e)
        {
            judgeConnection = new JudgeConnection();
            judgeConnection.deleteJudge(_personId);
            MessageBox.Show("Succesfully deleted!");
            PersonManagement pm = new PersonManagement();
            pm.Activate();
            pm.Show();
            this.Hide();
        }

      








        #endregion 

       

        


    }
}
