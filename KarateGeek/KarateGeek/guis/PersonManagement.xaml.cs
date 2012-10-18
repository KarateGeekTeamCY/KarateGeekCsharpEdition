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
        private AthleteConnection athleteConn;
        private string first_name = null;
        private string last_name = null;
        private string fathers_name = null;
        private string sex = null;
        private string first_phone = null;
        private string second_phone = null;
        private string email = null;
        private string address = null;
        private string address_num = null;
        private string city = null;
        private string country = null;
        private string country_code = null;
        private string rank = null;
        private string club = null;



        private DateTime date;
        private DataSet cities;
        private DataSet countries;
        private DataSet filteredAthlets;


        List<string> nameList;

        public PersonManagement()
        {
            //here should be the loading of the locations and clubs and countries
            InitializeComponent();
            athleteConn = new AthleteConnection();
            nameList = new List<string> 
            {
                "A0-Word","B0-Word","C0-Word",
                "A1-Word","B1-Word","C1-Word",
                "B2-Word","C2-Word",
                "C3-Word",
            };

            athleteFirstName.TextChanged += new TextChangedEventHandler(athleteFirstName_TextChanged);
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

            //prosthetoume rank
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

        private Boolean nameflag = true;

        private void athleteFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typedString = athleteFirstName.Text;
            List<string> autoList = new List<string>();
            autoList.Clear();

            foreach (string item in nameList)
            {
                if (!string.IsNullOrEmpty(athleteFirstName.Text))
                {
                    if (item.StartsWith(typedString))
                    {
                        autoList.Add(item);
                    }
                }
            }

            if (autoList.Count > 0)
            {
                suggestionList.ItemsSource = autoList;
                suggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (athleteFirstName.Text.Equals(""))
            {
                suggestionList.Visibility = Visibility.Collapsed;
                suggestionList.ItemsSource = null;
            }
            else
            {
                suggestionList.Visibility = Visibility.Collapsed;
                suggestionList.ItemsSource = null;
            }
        }


        //private void filterNames()
        //{
        //    AthleteConnection conn = new AthleteConnection();
        //    this.filteredAthlets = conn.findSimilar(this.first_name);

        //    List<string> list = new List<string>();
        //    foreach (DataRow dr in filteredAthlets.Tables[0].Rows)
        //    {
        //        list.Add(dr[1].ToString());
        //    }

        //    this.sugestioListScroler.Visibility = System.Windows.Visibility.Visible;
        //    this.sugestionList.ItemsSource = list;
        //}


        private void suggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (suggestionList.ItemsSource != null)
            {
                suggestionList.Visibility = System.Windows.Visibility.Collapsed;
                athleteFirstName.TextChanged -= new TextChangedEventHandler(athleteFirstName_TextChanged);
                if (suggestionList.SelectedIndex != -1)
                {
                    athleteFirstName.Text = suggestionList.SelectedItem.ToString();
                }
                athleteFirstName.TextChanged += new TextChangedEventHandler(athleteFirstName_TextChanged);
            }
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Hidden;
        }



        private void athleteLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            last_name = athleteLastName.Text;

        }

        private void athleteFathersName_TextChanged(object sender, TextChangedEventArgs e)
        {
            fathers_name = athleteFatherName.Text;

        }

        private void athleteDateOfBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            date = athleteDateOfBirth.SelectedDate.Value;
        }

        private void athleteFirstPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            first_phone = athleteFirstPhone.Text;
        }

        private void athleteSecondPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            second_phone = athleteSecondPhone.Text;
        }

        private void athleteEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            email = athleteEmail.Text;
        }

        private void athleteStreetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            address = athleteStreetName.Text;
        }

        private void athleteAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            address_num = athleteAddressNum.Text;
        }

        private void cmbACityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbACityChooses.SelectedIndex;
            if (index < cmbACityChooses.Items.Count && index != -1)
                city = cmbACityChooses.Items[index].ToString();
        }

        private void cmbACountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbACountryChooses.SelectedIndex;
            //country = cmbACountryChooses.Items[index].ToString();
            country_code = countries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(country);
            this._updateCities(country_code);
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
            rank = cmbARankChooses.Items[index].ToString();
        }

        private void cmbAClubChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbAClubChooses.SelectedIndex;
            club = cmbAClubChooses.Items[index].ToString();
        }

        private void rdButton1_Checked(object sender, RoutedEventArgs e)
        {
            sex = "male";
        }

        private void rdButton2_Checked(object sender, RoutedEventArgs e)
        {
            sex = "female";
        }

        private void btnASave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("sex: " + sex);
        }

        private void btnASaveNew_Click(object sender, RoutedEventArgs e)
        {
            athleteConn.InserrtNewAthlete(first_name, last_name, fathers_name, date, first_phone, second_phone, email, country_code, city, address, address_num, "3025", rank, club);
            MessageBox.Show("Succesfully saved!");
            PersonManagement pm = new PersonManagement();
            pm.Activate();
            pm.Show();
            this.Hide();
        }

        private void btnADelete_Click(object sender, RoutedEventArgs e)
        {

        }


        private void setCountryCode(string country)
        {
            if (country.Equals("Cyprus"))
            {
                country_code = "CY";
            }
            else if (country.Equals("Greece"))
            {
                country_code = "GR";
            }
        }




    }
}
