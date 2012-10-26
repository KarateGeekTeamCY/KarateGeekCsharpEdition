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
using System.Data;
using KarateGeek.databaseConnection;


namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for EventTournamentManagement.xaml
    /// </summary>
    public partial class EventTournamentManagement : Window
    {
        //
        //application specific variables
        //
        private DataSet cities;
        private DataSet countries;
        private DataSet events;
        private DataSet filteredEvents;
        private AddressConnection addressConnection;
        private CityConnection cityConnection;
        private CountryConnection countryConnection;
        private LocationConnection locationConnection;
        List<string> eventNameListForAutoComplete;

        //
        //event specific variables
        //

        private EventConnection eventConnection;
   
        private string _eventName = null;
        private string _eventLocation = null;
        private string _eventPhone = null;
        private string _eventEmail = null;
        private string _eventAddress = null;
        private string _eventAddressNum = null;
        private string _eventTK = null;
        private string _eventCity = null;
        private string _eventCountryCode = null;
        private int _eventId;
        private Boolean _eventOfficial;
        private DateTime _eventDate;


        //
        //tournament specific variables
        //
        private TournamentConnection tournamentConnection;

        private int    _tournamentEventId;
        private string _tournamentEvent;
        private string _tournamentName = null;
        private string _tournamentSex = null;
        private int    _tournamentAgeFrom = 0;
        private int    _tournamentAgeTo = 0;
        private string _tournamentLevelFrom = null;
        private string _tournamentLevelTo = null;
        private string _tounamentGameType = null;
        private string _tournamentGame = null;
        private string _eventInfo = null;

        public EventTournamentManagement()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            eventConnection = new EventConnection();
            tournamentConnection = new TournamentConnection();

            initialize();
            
        }
        //constructor pou pernei san orisma tin imerominia pou tou exei perastei apo to main
        public EventTournamentManagement(DateTime dateSelection)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            eventConnection = new EventConnection();
            tournamentConnection = new TournamentConnection();
            initialize();
   
            this.eventDate.SelectedDate = dateSelection;
        }

        private void initialize() {
            addressConnection = new AddressConnection();
            locationConnection = new LocationConnection();
            //cities kai countries
            cityConnection = new CityConnection();
            countryConnection = new CountryConnection();
            this.countries = countryConnection.GetCountries();

            foreach (DataRow dr in countries.Tables[0].Rows)
            {
                cmbECountryChooses.Items.Add(dr[1].ToString());

            }
            cmbECountryChooses.SelectedIndex = 0;

            this.eventUpdateCities("CY");


            //events
            eventConnection = new EventConnection();
            this.events = eventConnection.getEvents();
            cmbTEventChooser.Items.Add("Select Event");

            foreach (DataRow dr in events.Tables[0].Rows)
            {
                cmbTEventChooser.Items.Add(dr[1].ToString());

            }
            cmbTEventChooser.SelectedIndex = 0;

            //ages
            cmbTAgeFrom.Items.Add("From");
            cmbTAgeTo.Items.Add("To");
            for (int i = 5; i <= 60; i++)
            {
                cmbTAgeFrom.Items.Add(i.ToString());
                cmbTAgeTo.Items.Add(i.ToString());
            }
            cmbTAgeFrom.SelectedIndex = 0;
            cmbTAgeTo.SelectedIndex = 0;

            //levels
            cmbTLevelFrom.Items.Add("From");
            cmbTLevelFrom.Items.Add("A");
            cmbTLevelFrom.Items.Add("B");
            cmbTLevelFrom.Items.Add("C");
            cmbTLevelFrom.Items.Add("D");

            cmbTLevelTo.Items.Add("To");
            cmbTLevelTo.Items.Add("A");
            cmbTLevelTo.Items.Add("B");
            cmbTLevelTo.Items.Add("C");
            cmbTLevelTo.Items.Add("D");

            cmbTLevelFrom.SelectedIndex = 0;
            cmbTLevelTo.SelectedIndex = 0;


            //games
            cmbTGame.Items.Add("Select game type");
            cmbTGame.Items.Add("Game A");
            cmbTGame.Items.Add("Game B");
            cmbTGame.Items.Add("Game C");
            cmbTGame.Items.Add("Game D");
            cmbTGame.Items.Add("Game E");

            cmbTGame.SelectedIndex = 0;
        }
        #region event

        private void eventName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _eventName = eventName.Text;
            List<string> autoList = new List<string>();
            autoList.Clear();

            eventNameListForAutoComplete = this.EventsfilterNames();

            foreach (string item in eventNameListForAutoComplete)
            {
                if (!string.IsNullOrEmpty(eventName.Text))
                {
                    if (item.StartsWith(_eventName))
                    {
                        autoList.Add(item);
                    }
                }
            }

            if (autoList.Count > 0)
            {
                eSuggestionList.ItemsSource = autoList;
                eSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (eventName.Text.Equals(""))
            {
                eSuggestionList.Visibility = Visibility.Collapsed;
                eSuggestionList.ItemsSource = null;
            }
            else
            {
                eSuggestionList.Visibility = Visibility.Collapsed;
                eSuggestionList.ItemsSource = null;
            }
        }

        private List<string> EventsfilterNames()
        {
            List<string> list = new List<string>();
            string suggestion = null;

            this.filteredEvents = eventConnection.findSimilar(this.eventName.Text);

            if (this.filteredEvents.Tables[0].Rows.Count > 0)
            {
                _eventId = int.Parse(filteredEvents.Tables[0].Rows[0][0].ToString());
            }

            foreach (DataRow dr in filteredEvents.Tables[0].Rows)
            {
                suggestion = dr[1].ToString();
                list.Add(suggestion);
            }
            return list;
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Visible;
            //this.sugestionList.ItemsSource = list;
        }

        private void eSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int country_position = 0;
            int city_position = 0;
            int index;
            int location_id;
            int address_id;
            addressConnection = new AddressConnection();
            locationConnection = new LocationConnection();
            DataSet dsA;
            DataSet dsL;

            if (eSuggestionList.ItemsSource != null)
            {
                eSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                eventName.TextChanged -= new TextChangedEventHandler(eventName_TextChanged);

                index = eSuggestionList.SelectedIndex;

                if (eSuggestionList.SelectedIndex != -1)
                {
                    _eventName = eSuggestionList.SelectedItem.ToString();
                    location_id = int.Parse(filteredEvents.Tables[0].Rows[index][4].ToString());

                    this.eventName.Text = filteredEvents.Tables[0].Rows[index][1].ToString();
                    this.eventDate.SelectedDate = (DateTime)filteredEvents.Tables[0].Rows[index][2];
                    this.eventOfficial.IsChecked = (Boolean)filteredEvents.Tables[0].Rows[index][3];

                    dsL = locationConnection.getLocation(location_id);

                    this.eventLocation.Text = dsL.Tables[0].Rows[0][1].ToString();
                    this.eventPhone.Text = dsL.Tables[0].Rows[0][2].ToString();
                    this.eventEmail.Text = dsL.Tables[0].Rows[0][3].ToString();
                    address_id = int.Parse(dsL.Tables[0].Rows[0][4].ToString());

                    dsA = addressConnection.getAddress(address_id);

                    this.eventAddress.Text = dsA.Tables[0].Rows[0][1].ToString();
                    this.eventAddressNum.Text = dsA.Tables[0].Rows[0][2].ToString();
                    this.eventTK.Text = dsA.Tables[0].Rows[0][4].ToString();

                    string eventCity = dsA.Tables[0].Rows[0][3].ToString();
                    int ix = dsA.Tables[0].Columns.Count;
                    string eventCountry = dsA.Tables[0].Rows[0][5].ToString();


                    //
                    //the fix for the country selection error
                    //
                    CountryConnection countryconn = new CountryConnection();
                    DataSet countriname = countryconn.getCountryNameByCode(eventCountry);
                    eventCountry = countriname.Tables[0].Rows[0][0].ToString();




                    for (int i = 0; i < this.cmbECountryChooses.Items.Count; i++)
                    {
                        if (eventCountry.Equals(cmbECountryChooses.Items[i].ToString()))
                        {
                            country_position = i;
                            break;
                        }
                    }


                    this.cmbECountryChooses.SelectedIndex = country_position;


                    CityConnection cityConnection = new CityConnection();
                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(eventCity));
                    eventCity = cityNa.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbECityChooses.Items.Count; i++)
                    {
                        if (eventCity.Equals(cmbECityChooses.Items[i].ToString()))
                        {
                            city_position = i;
                            break;
                        }
                    }
                    this.cmbECityChooses.SelectedIndex = city_position;
                }
                eventName.TextChanged += new TextChangedEventHandler(eventName_TextChanged);
            }
        }

        private void eventDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _eventDate = eventDate.SelectedDate.Value;
        }

        private void eventLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            _eventLocation = eventLocation.Text;
        }

        private void eventPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _eventPhone = eventPhone.Text;
        }

        private void eventEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _eventEmail = eventEmail.Text;
        }

        private void eventAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            _eventAddress = eventAddress.Text;
        }

        private void eventAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _eventAddressNum = eventAddressNum.Text;
        }

        private void eventTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _eventTK = eventTK.Text;
        }

        private void cmbECountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbECountryChooses.SelectedIndex;
            //athlete_country = cmbACountryChooses.Items[index].ToString();
            _eventCountryCode = countries.Tables[0].Rows[index][0].ToString();
            //setCountryCode(athlete_country);
            this.eventUpdateCities(_eventCountryCode);
        }

        private void cmbECityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbECityChooses.SelectedIndex;
            if (index < cmbECityChooses.Items.Count && index != -1)
                _eventCity = cmbECityChooses.Items[index].ToString();
        }

        private void eventUpdateCities(string countryCode)
        {
            CityConnection citiesconn = new CityConnection();
            this.cities = citiesconn.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbECityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbECityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbECityChooses.Items.Add(dr[1].ToString());
            }
            cmbECityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbECityChooses.Items.Refresh();




        }


        private void eventOfficial_Checked(object sender, RoutedEventArgs e)
        {
                _eventOfficial = true;
          
        }
        private void eventOfficial_Unchecked(object sender, RoutedEventArgs e)
        {
                _eventOfficial = false;
        }
        

        private void btnEBack_Click(object sender, RoutedEventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.Activate();
            this.Close();
            menu.Show();
        }

        private void btnESave_Click(object sender, RoutedEventArgs e)
        {
            eventConnection.UpdateEvent(_eventId, _eventName, _eventDate, _eventAddress, _eventAddressNum, _eventTK, _eventLocation, _eventPhone, _eventEmail, _eventCity, _eventCountryCode, _eventOfficial); 
            MessageBox.Show("Succesfully saved!");
        }

        private void btnESaveNew_Click(object sender, RoutedEventArgs e)
        {
            eventConnection.InsertNewEvent(_eventName, _eventDate, _eventAddress, _eventAddressNum, _eventTK, _eventLocation, _eventPhone, _eventEmail, _eventCity, _eventCountryCode, _eventOfficial);
            MessageBox.Show("Succesfully saved!");
            EventTournamentManagement etm = new EventTournamentManagement();
            etm.Activate();
            etm.Show();
            this.Close();
        }

        private void btnEClear_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void cmbTEventChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTEventChooser.SelectedIndex;
            if (index != 0)
            {
                if (index < cmbTEventChooser.Items.Count && index != -1)
                    _tournamentEvent = cmbTEventChooser.Items[index].ToString();
                autocomplete(_tournamentEvent);
            }    
        }

        private void autocomplete(string eventName)
        {
            DataSet dsE = null; 
            DataSet dsL = null;
            DataSet dsA = null;
            DataSet dsC = null;
            int locationId;
            int addressId;
            int cityId;

            dsE = eventConnection.getEventsByName(eventName);
            _tournamentEventId = int.Parse(dsE.Tables[0].Rows[0][0].ToString());
            locationId = int.Parse(dsE.Tables[0].Rows[0][4].ToString());
            dsL = locationConnection.getLocation(locationId);
            addressId = int.Parse(dsL.Tables[0].Rows[0][4].ToString());
            dsA = addressConnection.getAddress(addressId);
            cityId = int.Parse(dsA.Tables[0].Rows[0][3].ToString());
            dsC = cityConnection.GetCityNameByCityId(cityId);

            _eventInfo = "Name: " + dsE.Tables[0].Rows[0][1].ToString() + "\n";
            _eventInfo += "Date: " + dsE.Tables[0].Rows[0][2].ToString() + "\n";
            _eventInfo += "City: " + dsC.Tables[0].Rows[0][0].ToString() + "\n";
            _eventInfo += "Address: " + dsA.Tables[0].Rows[0][1].ToString() + "\n";
            _eventInfo += "Num: " + dsA.Tables[0].Rows[0][2].ToString() + "    Postal Code:" + dsA.Tables[0].Rows[0][4].ToString() + "\n";
            _eventInfo += "Location: " + dsL.Tables[0].Rows[0][1].ToString() + "\n";
            _eventInfo += "Phone: " + dsL.Tables[0].Rows[0][2].ToString() + "\n";
            _eventInfo += "Email: " + dsL.Tables[0].Rows[0][3].ToString();
           
            eventInfo.Text = _eventInfo;
        }

        private void tbTName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _tournamentName = tbTName.Text;
        }

        private void TrdButtonMale_Checked(object sender, RoutedEventArgs e)
        {
            _tournamentSex = "male";
        }

        private void TrdButtonFemale_Checked(object sender, RoutedEventArgs e)
        {
            _tournamentSex = "female";
        }

        private void cmbTAgeFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTAgeFrom.SelectedIndex;
            if (index != 0)
            {
                if (index < cmbTAgeFrom.Items.Count && index != -1)
                    _tournamentAgeFrom = int.Parse(cmbTAgeFrom.Items[index].ToString());
            }  
        }

        private void cmbTAgeTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTAgeTo.SelectedIndex;
            if (index != 0)
            {
                if (index < cmbTAgeTo.Items.Count && index != -1)
                    _tournamentAgeTo = int.Parse(cmbTAgeTo.Items[index].ToString());
            }    
        }

        private void cmbTLevelFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTLevelFrom.SelectedIndex;
            if (index != 0)
            {
                if (index < cmbTLevelFrom.Items.Count && index != -1)
                    _tournamentLevelFrom = cmbTAgeFrom.Items[index].ToString();
            }    
        }

        private void cmbTLevelTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTLevelTo.SelectedIndex;
            if (index != 0)
            {
                if (index < cmbTLevelTo.Items.Count && index != -1)
                    _tournamentLevelTo = cmbTAgeTo.Items[index].ToString();
            }    
        }

        private void TrdButtonIndiv_Checked(object sender, RoutedEventArgs e)
        {
            _tounamentGameType = "individual";
        }

        private void TrdButtonTeam_Checked(object sender, RoutedEventArgs e)
        {
            _tounamentGameType = "team";
        }

        private void cmbTGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTGame.SelectedIndex;
            if (index != 0)
            {
                if (index < cmbTGame.Items.Count && index != -1)
                    _tournamentGame = cmbTGame.Items[index].ToString();
            }    
        }

        private void btnTBack_Click(object sender, RoutedEventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.Activate();
            this.Close();
            menu.Show();
        }

        private void btnTSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTSaveNew_Click(object sender, RoutedEventArgs e)
        {
            tournamentConnection.InsertNewTournament(_tournamentName, _tournamentSex, _tournamentAgeFrom, _tournamentAgeTo, _tournamentLevelFrom, _tournamentLevelTo, _tounamentGameType, _tournamentGame, _tournamentEventId);
            MessageBox.Show("Succesfully saved!");
            EventTournamentManagement etm = new EventTournamentManagement();
            etm.Activate();
            etm.Show();
            this.Close();
        }

        private void btnTClear_Click(object sender, RoutedEventArgs e)
        {

        }

       

        #region tournament


        #endregion
    }
        
}
