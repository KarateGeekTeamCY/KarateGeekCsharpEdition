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
        private DateTime date;
        private DataSet ds;
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




        public PersonManagement()
        {
            //here should be the loading of the locations and clubs and countries
            InitializeComponent();

            CityConnection cities = new CityConnection();
            ds = cities.GetCities();
            athleteConn = new AthleteConnection();


            //prostetoume cities oses theloume
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cmbACityChooses.Items.Add(dr[1].ToString() ); 
            }




            //cmbACityChooses.Items.Add("Nicosia");
            cmbACityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno
            //cmbACityChooses.Items.Add("Limassol");
            //prosthetoume xwres


            cmbACountryChooses.Items.Add("Cyprus");
            cmbACountryChooses.SelectedIndex = 0;
            cmbACountryChooses.Items.Add("Greece");
            //prosthetoume rank
            cmbARankChooses.Items.Add("White");
            cmbARankChooses.SelectedIndex = 0;
            cmbARankChooses.Items.Add("Yellow");
            //prosthetoume clubs
            cmbAClubChooses.Items.Add("Pro Kata Club (P.K.C.)");
            cmbAClubChooses.SelectedIndex = 0;
            cmbAClubChooses.Items.Add("Allo Club");
        }

        private void athleteFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {

            first_name = athleteFirstName.Text;
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
            city = cmbACityChooses.Items[index].ToString();
        }

        private void cmbACountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbACountryChooses.SelectedIndex;
            country = cmbACountryChooses.Items[index].ToString();
            setCountryCode(country);
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
        }

        private void btnADelete_Click(object sender, RoutedEventArgs e)
        {

        }


        private void setCountryCode(string country){
            if(country.Equals("Cyprus")){
                country_code = "CY";
            }else if(country.Equals("Greece")){
                country_code = "GR";
            }
        }
       

    }
}
