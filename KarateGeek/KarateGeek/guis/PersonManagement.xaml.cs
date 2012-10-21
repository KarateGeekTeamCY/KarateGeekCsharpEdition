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
        private DataSet filteredAthlets;
        private AddressConnection addressConnection;
        List<string> athleteNameListForAutoComplete;
        List<string> judgeNameListForAutoComplete;

        //
        //athlete specific variables
        //

        private AthleteConnection athleteConn;
        private string athlete_first_name = null;
        private string athlete_last_name = null;
        private string athlete_fathers_name = null;
        private string athlete_sex = null;
        private string athlete_first_phone = null;
        private string athlete_second_phone = null;
        private string athlete_email = null;
        private string athlete_address = null;
        private string athlete_address_num = null;
        private string athlete_city = null;
        private string athlete_country = null;
        private string athlete_country_code = null;
        private string athlete_rank = null;
        private string athlete_club = null;
        private DateTime athlete_dateOfBirth;


        //
        //judge specific variables
        //
        // afta mpori na allaksoun katalila mixali aplos sta etimasa
        //
        private string judge_first_name = null;
        private string judge_last_name = null;
        private string judge_fathers_name = null;
        private string judge_sex = null;
        private string judge_first_phone = null;
        private string judge_second_phone = null;
        private string judge_email = null;
        private string judge_address = null;
        private string judge_address_num = null;
        private string judge_city = null;
        private string judge_country = null;
        private string judge_country_code = null;
        private string judge_rank = null;
        private string judge_club = null;
        private DateTime judge_dateOfBirth;


        //
        //mixali min frikaris afto einai gia na grouparis kodika
        //arxizi me #region <name>
        //lai telion me #endregion
        //
        #region athlete spesific code

        public PersonManagement()
        {
            //here should be the loading of the locations and clubs and countries
            InitializeComponent();

            athleteConn = new AthleteConnection();

            //
            //NOTE
            //
            //mixali listeners vaze katefthian apo to xaml oxi karfota mesa ston kodika
            //
            //athleteFirstName.TextChanged += new TextChangedEventHandler(athleteFirstName_TextChanged);
            //prostetoume cities oses theloume



            CountryConnection countryconn = new CountryConnection();
            this.countries = countryconn.GetCountries();

            foreach (DataRow dr in countries.Tables[0].Rows)
            {
                cmbACountryChooses.Items.Add(dr[1].ToString());
            }
            cmbACountryChooses.SelectedIndex = 0;

            this._updateCities("CY");

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
            cmbARankChooses.Items.Add("White        - 6th kyu");
            cmbARankChooses.Items.Add("Yellow       - 5th kyu");
            cmbARankChooses.Items.Add("Orange       - 4th kyu");
            cmbARankChooses.Items.Add("Green        - 3th kyu");
            cmbARankChooses.Items.Add("Blue         - 2nd kyu");
            cmbARankChooses.Items.Add("Brown        - 1st kyu");
            cmbARankChooses.Items.Add("Black        - 1st dan");
            cmbARankChooses.Items.Add("Black        - 2nd dan");
            cmbARankChooses.Items.Add("Black        - 3th dan");
            cmbARankChooses.Items.Add("Black        - 4th dan");
            cmbARankChooses.Items.Add("Black        - 5th dan");
            cmbARankChooses.Items.Add("White/Red    - 6th dan");
            cmbARankChooses.Items.Add("White/Red    - 7th dan");
            cmbARankChooses.Items.Add("White/Red    - 8th dan");
            cmbARankChooses.Items.Add("Red          - 9th dan");
            cmbARankChooses.Items.Add("Red          - 10th dan");
            cmbARankChooses.SelectedIndex = 0;


            //prosthetoume clubs


            cmbAClubChooses.Items.Add("Pro Kata Club (P.K.C.)");
            cmbAClubChooses.SelectedIndex = 0;
            cmbAClubChooses.Items.Add("Allo Club");
        }

        //private Boolean nameflag = true;

        private void athleteFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            athlete_first_name = athleteFirstName.Text;
            List<string> autoList = new List<string>();
            autoList.Clear();

            athleteNameListForAutoComplete = this.filterNames();

            foreach (string item in athleteNameListForAutoComplete)
            {
                if (!string.IsNullOrEmpty(athleteFirstName.Text))
                {
                    if (item.StartsWith(athlete_first_name))
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

        private void judgeFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            judge_first_name = judgeFirstName.Text;
            List<string> autoList = new List<string>();
            autoList.Clear();

            judgeNameListForAutoComplete = this.filterNames();

            foreach (string item in judgeNameListForAutoComplete)
            {
                if (!string.IsNullOrEmpty(judgeFirstName.Text))
                {
                    if (item.StartsWith(judge_first_name))
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

        private List<string> filterNames()
        {
            string suggestion = null;
            AthleteConnection conn = new AthleteConnection();
            this.filteredAthlets = conn.findSimilar(this.athleteFirstName.Text);

            List<string> list = new List<string>();
            foreach (DataRow dr in filteredAthlets.Tables[0].Rows)
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
            int address_id;
            
            addressConnection = new AddressConnection();
            DataSet ds;

            if (aSuggestionList.ItemsSource != null)
            {
                aSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                athleteFirstName.TextChanged -= new TextChangedEventHandler(athleteFirstName_TextChanged);

                int index = aSuggestionList.SelectedIndex;
                
                if (aSuggestionList.SelectedIndex != -1)
                {
                    name = aSuggestionList.SelectedItem.ToString();
                    sex = filteredAthlets.Tables[0].Rows[index][4].ToString();
                    rank = filteredAthlets.Tables[0].Rows[index][11].ToString();
                    address_id = int.Parse(filteredAthlets.Tables[0].Rows[index][9].ToString());

                    //
                    //giati mazoxizese re mike??
                    //
                    //this.athleteFirstName.Text = name.Substring(0, name.IndexOf(" "));
                    //

                    this.athleteFirstName.Text = filteredAthlets.Tables[0].Rows[index][1].ToString();
                    this.athlete_first_name = this.athleteFirstName.Text;

                    this.athleteLastName.Text = filteredAthlets.Tables[0].Rows[index][2].ToString();
                    this.athleteFatherName.Text = filteredAthlets.Tables[0].Rows[index][3].ToString();
                    
                    if (sex.Equals("male"))
                    {
                        this.rdButton1.IsChecked = true;
                    }
                    else
                    {
                        this.rdButton2.IsChecked = true;
                    }

                    this.athleteDateOfBirth.SelectedDate = (DateTime)filteredAthlets.Tables[0].Rows[index][5];
                    this.athleteFirstPhone.Text = filteredAthlets.Tables[0].Rows[index][6].ToString();
                    this.athleteSecondPhone.Text = filteredAthlets.Tables[0].Rows[index][7].ToString();
                    this.athleteEmail.Text = filteredAthlets.Tables[0].Rows[index][8].ToString();
                    
                    ds = addressConnection.getAddress(address_id);

                    this.athleteStreetName.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.athleteAddressNum.Text = ds.Tables[0].Rows[0][2].ToString();

                    string acity = ds.Tables[0].Rows[0][3].ToString();
                    int ix = ds.Tables[0].Columns.Count;
                    string acountry = ds.Tables[0].Rows[0][5].ToString();


                    for (int i = 0; i < this.cmbACountryChooses.Items.Count; i++)
                    {
                        if (acountry.Equals(cmbACountryChooses.Items[i]))
                        {
                            rank_position = i;
                            break;
                        }
                    }
                    this.cmbACountryChooses.SelectedIndex = rank_position;


                    for (int i = 0; i < this.cmbACityChooses.Items.Count; i++)
                    {
                        if (acountry.Equals(cmbACityChooses.Items[i]))
                        {
                            rank_position = i;
                            break;
                        }
                    }
                    this.cmbACityChooses.SelectedIndex = rank_position;


                    //vriskei tin zwni pou exei o kathenas se poia thesi einai
                    for (int i = 0; i < cmbARankChooses.Items.Count; i++)
                    {
                        if (rank.Equals(cmbARankChooses.Items[i]))
                        {
                            rank_position = i;
                            break;
                        }
                    }
                    this.cmbARankChooses.SelectedIndex = rank_position;

                }
                athleteFirstName.TextChanged += new TextChangedEventHandler(athleteFirstName_TextChanged);
            }
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Hidden;
        }


        private void jSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = null;
            string sex = null;
            string rank = null;
            int rank_position = 0;
            int address_id;

            addressConnection = new AddressConnection();
            DataSet ds;

            if (jSuggestionList.ItemsSource != null)
            {
                jSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                athleteFirstName.TextChanged -= new TextChangedEventHandler(athleteFirstName_TextChanged);

                int index = jSuggestionList.SelectedIndex;

                if (jSuggestionList.SelectedIndex != -1)
                {
                    name = jSuggestionList.SelectedItem.ToString();
                    sex = filteredAthlets.Tables[0].Rows[index][4].ToString();
                    rank = filteredAthlets.Tables[0].Rows[index][11].ToString();
                    address_id = int.Parse(filteredAthlets.Tables[0].Rows[index][9].ToString());

                    //
                    //giati mazoxizese re mike??
                    //
                    //this.athleteFirstName.Text = name.Substring(0, name.IndexOf(" "));
                    //

                    this.judgeFirstName.Text = filteredAthlets.Tables[0].Rows[index][1].ToString();
                    this.judge_first_name = this.athleteFirstName.Text;

                    this.judgeLastName.Text = filteredAthlets.Tables[0].Rows[index][2].ToString();
                    this.judgeFatherName.Text = filteredAthlets.Tables[0].Rows[index][3].ToString();

                    if (sex.Equals("male"))
                    {
                        this.rdButton1.IsChecked = true;
                    }
                    else
                    {
                        this.rdButton2.IsChecked = true;
                    }

                    this.athleteDateOfBirth.SelectedDate = (DateTime)filteredAthlets.Tables[0].Rows[index][5];
                    this.athleteFirstPhone.Text = filteredAthlets.Tables[0].Rows[index][6].ToString();
                    this.athleteSecondPhone.Text = filteredAthlets.Tables[0].Rows[index][7].ToString();
                    this.athleteEmail.Text = filteredAthlets.Tables[0].Rows[index][8].ToString();

                    ds = addressConnection.getAddress(address_id);

                    this.athleteStreetName.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.athleteAddressNum.Text = ds.Tables[0].Rows[0][2].ToString();

                    string acity = ds.Tables[0].Rows[0][3].ToString();
                    int ix = ds.Tables[0].Columns.Count;
                    string acountry = ds.Tables[0].Rows[0][5].ToString();


                    for (int i = 0; i < this.cmbACountryChooses.Items.Count; i++)
                    {
                        if (acountry.Equals(cmbACountryChooses.Items[i]))
                        {
                            rank_position = i;
                            break;
                        }
                    }
                    this.cmbACountryChooses.SelectedIndex = rank_position;


                    for (int i = 0; i < this.cmbACityChooses.Items.Count; i++)
                    {
                        if (acountry.Equals(cmbACityChooses.Items[i]))
                        {
                            rank_position = i;
                            break;
                        }
                    }
                    this.cmbACityChooses.SelectedIndex = rank_position;


                    //vriskei tin zwni pou exei o kathenas se poia thesi einai
                    for (int i = 0; i < cmbARankChooses.Items.Count; i++)
                    {
                        if (rank.Equals(cmbARankChooses.Items[i]))
                        {
                            rank_position = i;
                            break;
                        }
                    }
                    this.cmbARankChooses.SelectedIndex = rank_position;

                }
                athleteFirstName.TextChanged += new TextChangedEventHandler(athleteFirstName_TextChanged);
            }
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Hidden;
        }


        private void athleteLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            athlete_last_name = athleteLastName.Text;

        }

        private void athleteFathersName_TextChanged(object sender, TextChangedEventArgs e)
        {
            athlete_fathers_name = athleteFatherName.Text;

        }

        private void athleteDateOfBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            athlete_dateOfBirth = athleteDateOfBirth.SelectedDate.Value;
        }

        private void athleteFirstPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            athlete_first_phone = athleteFirstPhone.Text;
        }

        private void athleteSecondPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            athlete_second_phone = athleteSecondPhone.Text;
        }

        private void athleteEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            athlete_email = athleteEmail.Text;
        }

        private void athleteStreetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            athlete_address = athleteStreetName.Text;
        }

        private void athleteAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            athlete_address_num = athleteAddressNum.Text;
        }

        private void cmbACityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbACityChooses.SelectedIndex;
            if (index < cmbACityChooses.Items.Count && index != -1)
                athlete_city = cmbACityChooses.Items[index].ToString();
        }

        private void cmbACountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbACountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            athlete_country_code = countries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this._updateCities(athlete_country_code);
        }

        private void _updateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.cities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbACityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbACityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbACityChooses.Items.Add(dr[1].ToString());
            }
            cmbACityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbACityChooses.Items.Refresh();




        }

        private void cmbARankChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbARankChooses.SelectedIndex;
            athlete_rank = cmbARankChooses.Items[index].ToString();
        }

        private void cmbAClubChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbAClubChooses.SelectedIndex;
            athlete_club = cmbAClubChooses.Items[index].ToString();
        }

        private void rdButton1_Checked(object sender, RoutedEventArgs e)
        {
            athlete_sex = "male";
        }

        private void rdButton2_Checked(object sender, RoutedEventArgs e)
        {
            athlete_sex = "female";
        }

        private void btnASave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("athlete_sex: " + athlete_sex);
        }

        private void btnASaveNew_Click(object sender, RoutedEventArgs e)
        {
            athleteConn.InsertNewAthlete(athlete_first_name, athlete_last_name, athlete_fathers_name, athlete_sex, athlete_dateOfBirth, athlete_first_phone, athlete_second_phone, athlete_email, athlete_address, athlete_address_num, "3025", athlete_country_code, athlete_city, athlete_rank, athlete_club);
            MessageBox.Show("Succesfully saved!");
            PersonManagement pm = new PersonManagement();
            pm.Activate();
            pm.Show();
            this.Hide();
        }

        private void btnADelete_Click(object sender, RoutedEventArgs e)
        {

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


    }
}
