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
    public partial class EventTournamentManagement : Window
    {
        #region definitions and initialization
        //
        //application variables
        //
        EventConnection eventConnection = new EventConnection();
        CountryConnection countryConnection = new CountryConnection();
        CityConnection cityConnection = new CityConnection();
        LocationConnection locationConnection = new LocationConnection();
        AddressConnection addressConnection = new AddressConnection();
        TournamentConnection tournamentConnection = new TournamentConnection();
        ParticipationsConnection participantConnection = new ParticipationsConnection();
        AthleteConnection athleteConnection = new AthleteConnection();

        private DataSet filteredEvents;
        private DataSet filteredTournaments;
        private DataSet filteredAthletes;
        private DataSet countries;
        private DataSet cities;
        private DataSet events;
        private List<ListData> eventNameListForAutoComplete;
        private List<ListData> tournamentNameListForAutoComplete;
        private List<AthleteData> possibleParticipants = new List<AthleteData>();
        private List<List<AthleteData>> selectedParticipants = new List<List<AthleteData>>();
        private bool suggestionChange = false;

        //
        //event variables
        //
        private int _eventId;
        private string _eventName = null;
        private DateTime _eventDate;
        private string _eventLocation = null;
        private string _eventPhone = null;
        private string _eventEmail = null;
        private string _eventAddress = null;
        private string _eventAddressNum = null;
        private string _eventPCode = null;
        private string _eventCity = null;
        private string _eventCountryCode = null;
        private Boolean _eventOfficial;

        //
        //tournament variables
        //
        private int _tournamentEventId;
        private string _tournamentEvent;
        private string _tournamentName = null;
        private string _tournamentSex = null;
        private int _tournamentAgeFrom = 0;
        private int _tournamentAgeTo = 0;
        private string _tournamentLevelFrom = null;
        private string _tournamentLevelTo = null;
        private string _tournamentGameType = null;
        private string _tournamentCatType = null;  //individual or team
        private string _tournamentScoringType = null;
        private int _tournamentTeamId = 0;
        private string _eventInfo = null;
        private int _tournamentTeam = 0;
        private int _levelFrom = 0;
        private int _levelTo = 0;
        private int _tournamentId = 0;
        private int teamsNum = 0;
        private int teamNumber = 0;


        public EventTournamentManagement()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            initialize();   //initialize gui and others


        }

        //constructor pou pernei san orisma tin imerominia pou tou exei perastei apo to main
        public EventTournamentManagement(DateTime dateSelection)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            initialize();   //initialize gui and others
            this.eventDate.SelectedDate = dateSelection;
        }

        public EventTournamentManagement(string name)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            initialize();
            autocompleteByName(name);
        }


        private void initialize()
        {


            btdeleteParticipant.Content = "<<";
            //cities kai countries
            this.countries = countryConnection.GetCountries();

            foreach (DataRow dr in countries.Tables[0].Rows)
            {
                cmbECountryChooses.Items.Add(dr[1].ToString());

            }
            cmbECountryChooses.SelectedIndex = 54;

            this.eventUpdateCities("CY");

            //events
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
            cmbTLevelTo.Items.Add("To");
            for (int i = 0; i < KarateGeek.Strings.rank.Length; i++)
            {
                cmbTLevelFrom.Items.Add(KarateGeek.Strings.rank[i]);
                cmbTLevelTo.Items.Add(KarateGeek.Strings.rank[i]);
            }

            cmbTLevelFrom.SelectedIndex = 0;
            cmbTLevelTo.SelectedIndex = 0;


            //games
            cmbTGame.Items.Add("Select game type");
            cmbTGame.SelectedIndex = 0;

            //judging type
            cmbTJudging.Items.Add("Select judging type");



            //tournament number of teams
            cmbTteamsNumber.Items.Add("Teams");
            cmbTteamsNumber.Items.Add("2");
            cmbTteamsNumber.Items.Add("3");
            cmbTteamsNumber.Items.Add("4");
            cmbTteamsNumber.Items.Add("5");
            cmbTteamsNumber.Items.Add("6");
            cmbTteamsNumber.Items.Add("7");
            cmbTteamsNumber.Items.Add("8");
            cmbTteamsNumber.Items.Add("9");
            cmbTteamsNumber.Items.Add("10");

            cmbTteamsNumber.SelectedIndex = 0;
        }
        #endregion

        #region event

        private void eventName_TextChanged(object sender, TextChangedEventArgs e)
        {
            eventList();
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

        private void eventPCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            _eventPCode = eventPCode.Text;
        }

        private void cmbECountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbECountryChooses.SelectedIndex;

            _eventCountryCode = countries.Tables[0].Rows[index][0].ToString();
            this.eventUpdateCities(_eventCountryCode);
        }

        private void cmbECityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbECityChooses.SelectedIndex;
            if (index < cmbECityChooses.Items.Count && index != -1)
                _eventCity = cmbECityChooses.Items[index].ToString();
        }

        private void eventOfficial_Checked(object sender, RoutedEventArgs e)
        {
            _eventOfficial = (bool)eventOfficial.IsChecked;
        }

        private void eSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int country_position = 0;
            int city_position = 0;
            int index;
            int location_id;
            int address_id;
            DataSet dsA;
            DataSet dsL;

            if (eSuggestionList.ItemsSource != null)
            {
                eSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                eventName.TextChanged -= new TextChangedEventHandler(eventName_TextChanged);

                index = eSuggestionList.SelectedIndex;

                if (eSuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)eSuggestionList.SelectedItem;
                    _eventId = item.id;
                    location_id = int.Parse(filteredEvents.Tables[0].Rows[index][4].ToString());

                    this.eventName.Text = filteredEvents.Tables[0].Rows[index][1].ToString();
                    this.eventDate.SelectedDate = (DateTime)filteredEvents.Tables[0].Rows[index][2];
                    this.eventOfficial.IsChecked = (Boolean)filteredEvents.Tables[0].Rows[index][3];

                    dsL = locationConnection.getLocation(location_id);

                    this.eventLocation.Text = dsL.Tables[0].Rows[0][1].ToString();
                    this.eventPhone.Text = dsL.Tables[0].Rows[0][2].ToString();
                    this.eventEmail.Text = dsL.Tables[0].Rows[0][3].ToString();
                    address_id = location_id;

                    dsA = addressConnection.getAddress(address_id);

                    this.eventAddress.Text = dsA.Tables[0].Rows[0][1].ToString();
                    this.eventAddressNum.Text = dsA.Tables[0].Rows[0][2].ToString();
                    this.eventPCode.Text = dsA.Tables[0].Rows[0][4].ToString();

                    string eventCity = dsA.Tables[0].Rows[0][3].ToString();
                    int ix = dsA.Tables[0].Columns.Count;
                    string eventCountry = dsA.Tables[0].Rows[0][5].ToString();


                    //
                    //the fix for the country selection error
                    //
                    DataSet countriname = countryConnection.getCountryNameByCode(eventCountry);
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


        private void btnEBack_Click(object sender, RoutedEventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.Activate();
            this.Close();
            menu.Show();
        }

        private void btnESave_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields("event"))
            {
                eventConnection.UpdateEvent(_eventId, _eventName, _eventDate, _eventAddress, _eventAddressNum, _eventPCode, _eventLocation, _eventPhone, _eventEmail, _eventCity, _eventCountryCode, _eventOfficial);
                MessageBox.Show("Succesfully saved!");
            }
        }

        private void btnESaveNew_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields("event"))
            {
                eventConnection.InsertNewEvent(_eventName, _eventDate, _eventAddress, _eventAddressNum, _eventPCode, _eventLocation, _eventPhone, _eventEmail, _eventCity, _eventCountryCode, _eventOfficial);
                MessageBox.Show("Succesfully saved!");
                EventTournamentManagement etm = new EventTournamentManagement();
                etm.Activate();
                etm.Show();
                this.Close();
            }
        }

        private void btnEDelete_Click(object sender, RoutedEventArgs e)
        {
            eventConnection.deleteEvent(_eventId);
            MessageBox.Show("Succesfully deleted!");
            EventTournamentManagement etm = new EventTournamentManagement();
            etm.Activate();
            etm.Show();
            this.Close();
        }

        #endregion

        #region tournament

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
            addressId = locationId;
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
            tournamentList();
        }


        private void tSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sex = null;
            string ageFrom = null;
            string ageTo = null;
            string levelFrom = null;
            string levelTo = null;
            string game = null;

            string scoringType = null;
            int agePosition = 0;
            int levelPosition = 0;
            int gamePosition = 0;
            int scoringPosition = 0;
            int index;
            int teamsNum;


            if (tSuggestionList.ItemsSource != null)
            {
                tSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                tbTName.TextChanged -= new TextChangedEventHandler(tbTName_TextChanged);

                index = tSuggestionList.SelectedIndex;

                if (tSuggestionList.SelectedIndex != -1)
                {
                    suggestionChange = true;  //dixnei oti ginetai allagi twn listeners mesa apo suggestion change
                    ListData item = (ListData)tSuggestionList.SelectedItem;
                    _tournamentId = item.id;
                    _tournamentName = item.name;

                    this.tbTName.Text = filteredTournaments.Tables[0].Rows[index][1].ToString();
                    sex = filteredTournaments.Tables[0].Rows[index][2].ToString();
                    ageFrom = filteredTournaments.Tables[0].Rows[index][3].ToString();
                    ageTo = filteredTournaments.Tables[0].Rows[index][4].ToString();
                    levelFrom = filteredTournaments.Tables[0].Rows[index][5].ToString();
                    levelTo = filteredTournaments.Tables[0].Rows[index][6].ToString();
                    game = filteredTournaments.Tables[0].Rows[index][7].ToString();
                    scoringType = filteredTournaments.Tables[0].Rows[index][8].ToString();

                    string[] gameCatType = game.Split('|');

                    if (sex.Equals(KarateGeek.Strings.male))
                    {
                        this.TrdButtonMale.IsChecked = true;
                    }
                    else
                    {
                        this.TrdButtonFemale.IsChecked = true;
                    }

                    for (int i = 0; i < this.cmbTAgeFrom.Items.Count; i++)
                    {
                        if (ageFrom.Equals(cmbTAgeFrom.Items[i].ToString()))
                        {
                            agePosition = i;
                            break;
                        }
                    }
                    this.cmbTAgeFrom.SelectedIndex = agePosition;

                    for (int i = 0; i < this.cmbTAgeTo.Items.Count; i++)
                    {
                        if (ageTo.Equals(cmbTAgeTo.Items[i].ToString()))
                        {
                            agePosition = i;
                            break;
                        }
                    }
                    this.cmbTAgeTo.SelectedIndex = agePosition;

                    for (int i = 0; i < this.cmbTLevelFrom.Items.Count; i++)
                    {
                        if (levelFrom.Equals(cmbTLevelFrom.Items[i].ToString()))
                        {
                            levelPosition = i;
                            break;
                        }
                    }
                    this.cmbTLevelFrom.SelectedIndex = levelPosition;

                    for (int i = 0; i < this.cmbTLevelTo.Items.Count; i++)
                    {
                        if (levelTo.Equals(cmbTLevelTo.Items[i].ToString()))
                        {
                            levelPosition = i;
                            break;
                        }
                    }
                    this.cmbTLevelTo.SelectedIndex = levelPosition;

                    if (gameCatType[0].Equals(Strings.individual))
                    {
                        _tournamentCatType = Strings.individual;
                        this.TrdButtonIndiv.IsChecked = true;
                    }
                    else
                    {
                        _tournamentCatType = Strings.team;
                        this.TrdButtonTeam.IsChecked = true;
                        teamsNum = participantConnection.getNumTeams(_tournamentId);

                        selectedParticipants = new List<List<AthleteData>>();

                        for (int i = 0; i < teamsNum; i++)
                        {
                            selectedParticipants.Add(new List<AthleteData>());
                        }

                        cmbTteamsNumber.SelectedIndex = 1;
                    }

                    for (int i = 0; i < this.cmbTGame.Items.Count; i++)
                    {
                        if (gameCatType[1].Equals(cmbTGame.Items[i].ToString()))
                        {
                            gamePosition = i;
                            break;
                        }
                    }
                    this.cmbTGame.SelectedIndex = gamePosition;

                    for (int i = 0; i < this.cmbTJudging.Items.Count; i++)
                    {
                        if (scoringType.Equals(cmbTJudging.Items[i].ToString()))
                        {
                            scoringPosition = i;
                            break;
                        }
                    }
                    this.cmbTJudging.SelectedIndex = scoringPosition;



                    showPossibleParticipantsByDB();
                    showSelectedParticipantsByDB(0);
                    suggestionChange = false;
                }
                tbTName.TextChanged += new TextChangedEventHandler(tbTName_TextChanged);
            }
        }

        private void TrdButtonMale_Checked(object sender, RoutedEventArgs e)
        {
            if (areParticipantsForDeletion() && !suggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        _tournamentSex = KarateGeek.Strings.male;

                        selectedParticipants = new List<List<AthleteData>>();
                        selectedParticipants.Add(new List<AthleteData>());
                        showPossibleParticipantsByDB();
                        showSelectedParticipantsI();
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                _tournamentSex = KarateGeek.Strings.male;
                showPossibleParticipantsByDB();
            }

        }

        private void TrdButtonFemale_Checked(object sender, RoutedEventArgs e)
        {
            if (areParticipantsForDeletion() && !suggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        _tournamentSex = KarateGeek.Strings.female;

                        selectedParticipants = new List<List<AthleteData>>();
                        selectedParticipants.Add(new List<AthleteData>());
                        showPossibleParticipantsByDB();
                        showSelectedParticipantsI();
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                _tournamentSex = KarateGeek.Strings.female;
                showPossibleParticipantsByDB();
            }
        }

        private void cmbTAgeFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTAgeFrom.SelectedIndex;
            if (areParticipantsForDeletion() && !suggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbTAgeFrom.Items.Count && index != -1)
                                _tournamentAgeFrom = int.Parse(cmbTAgeFrom.Items[index].ToString());
                            showPossibleParticipantsByDB();

                        }
                        selectedParticipants = new List<List<AthleteData>>();
                        selectedParticipants.Add(new List<AthleteData>());
                        showPossibleParticipantsByDB();
                        showSelectedParticipantsI();
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbTAgeFrom.Items.Count && index != -1)
                        _tournamentAgeFrom = int.Parse(cmbTAgeFrom.Items[index].ToString());
                    showPossibleParticipantsByDB();

                }
            }

        }

        private void cmbTAgeTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTAgeTo.SelectedIndex;

            if (areParticipantsForDeletion() && !suggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbTAgeTo.Items.Count && index != -1)
                                _tournamentAgeTo = int.Parse(cmbTAgeTo.Items[index].ToString());
                            showPossibleParticipantsByDB();
                        }
                        selectedParticipants = new List<List<AthleteData>>();
                        selectedParticipants.Add(new List<AthleteData>());
                        showPossibleParticipantsByDB();
                        showSelectedParticipantsI();
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {

                if (index != 0)
                {
                    if (index < cmbTAgeTo.Items.Count && index != -1)
                        _tournamentAgeTo = int.Parse(cmbTAgeTo.Items[index].ToString());
                    showPossibleParticipantsByDB();
                }
            }
        }

        private void cmbTLevelFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTLevelFrom.SelectedIndex;
            _levelFrom = index;

            if (areParticipantsForDeletion() && !suggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbTLevelFrom.Items.Count && index != -1)
                                _tournamentLevelFrom = cmbTLevelFrom.Items[index].ToString();

                            showPossibleParticipantsByDB();
                        }
                        selectedParticipants = new List<List<AthleteData>>();
                        selectedParticipants.Add(new List<AthleteData>());
                        showPossibleParticipantsByDB();
                        showSelectedParticipantsI();
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbTLevelFrom.Items.Count && index != -1)
                        _tournamentLevelFrom = cmbTLevelFrom.Items[index].ToString();

                    showPossibleParticipantsByDB();
                }
            }
        }

        private void cmbTLevelTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTLevelTo.SelectedIndex;
            _levelTo = index;

            if (areParticipantsForDeletion() && !suggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbTLevelTo.Items.Count && index != -1)
                                _tournamentLevelTo = cmbTLevelTo.Items[index].ToString();
                            showPossibleParticipantsByDB();
                        }
                        selectedParticipants = new List<List<AthleteData>>();
                        selectedParticipants.Add(new List<AthleteData>());
                        showPossibleParticipantsByDB();
                        showSelectedParticipantsI();
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbTLevelTo.Items.Count && index != -1)
                        _tournamentLevelTo = cmbTLevelTo.Items[index].ToString();
                    showPossibleParticipantsByDB();
                }
            }
        }

        private void TrdButtonIndiv_Checked(object sender, RoutedEventArgs e)
        {
            _tournamentCatType = KarateGeek.Strings.individual;

            this.selectedParticipants.Add(new List<AthleteData>());
            cmbTGame.Items.Clear();
            cmbTGame.Items.Add("Select game type");

            String[] gameType = KarateGeek.Strings.indKata.Split('|');
            cmbTGame.Items.Add(gameType[1]);
            gameType = KarateGeek.Strings.indKumite.Split('|');
            cmbTGame.Items.Add(gameType[1]);
            gameType = KarateGeek.Strings.fugugo.Split('|');
            cmbTGame.Items.Add(gameType[1]);

            cmbTGame.SelectedIndex = 0;

            individualGrid.Visibility = System.Windows.Visibility.Visible;
            teamGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void TrdButtonTeam_Checked(object sender, RoutedEventArgs e)
        {
            _tournamentCatType = KarateGeek.Strings.team;
            cmbTGame.Items.Clear();
            cmbTGame.Items.Add("Select game type");

            String[] gameType = KarateGeek.Strings.teamKata.Split('|');
            cmbTGame.Items.Add(gameType[1]);
            gameType = KarateGeek.Strings.teamKumite.Split('|');
            cmbTGame.Items.Add(gameType[1]);

            cmbTGame.SelectedIndex = 0;
            teamGrid.Visibility = System.Windows.Visibility.Visible;
            individualGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void TrdButtonSync_Checked(object sender, RoutedEventArgs e)
        {
            _tournamentCatType = KarateGeek.Strings.synchronized;
            cmbTGame.Items.Clear();
            cmbTGame.Items.Add("Select game type");

            String[] gameType = KarateGeek.Strings.syncKata.Split('|');
            cmbTGame.Items.Add(gameType[1]);
            gameType = KarateGeek.Strings.enbu.Split('|');
            cmbTGame.Items.Add(gameType[1]);

            cmbTGame.SelectedIndex = 0;
            teamGrid.Visibility = System.Windows.Visibility.Visible;
            individualGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void cmbTGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTGame.SelectedIndex;

            cmbTJudging.Items.Clear();


            if (index != 0)
            {
                if (index < cmbTGame.Items.Count && index != -1)
                {
                    _tournamentGameType = _tournamentCatType + "|" + cmbTGame.Items[index].ToString();

                    if (_tournamentGameType.Equals(Strings.indKata))
                    {
                        cmbTJudging.Items.Clear();
                        cmbTJudging.Items.Add("Select game type");
                        cmbTJudging.Items.Add(Strings.score);
                        cmbTJudging.Items.Add(Strings.flag);
                    }
                    else if (_tournamentGameType.Equals(Strings.teamKata))
                    {
                        cmbTJudging.Items.Add(Strings.score);
                    }
                    else if (_tournamentGameType.Equals(Strings.syncKata))
                    {
                        cmbTJudging.Items.Add(Strings.score);
                    }
                    else if (_tournamentGameType.Equals(Strings.indKumite))
                    {
                        cmbTJudging.Items.Add(Strings.point);
                    }
                    else if (_tournamentGameType.Equals(Strings.teamKumite))
                    {
                        cmbTJudging.Items.Add(Strings.point);
                    }
                    else if (_tournamentGameType.Equals(Strings.enbu))
                    {
                        cmbTJudging.Items.Add(Strings.score);
                    }
                    else if (_tournamentGameType.Equals(Strings.fugugo))
                    {
                        cmbTJudging.Items.Add(Strings.point + "/" + Strings.flag);
                    }
                    cmbTJudging.SelectedIndex = 0;
                }


            }
        }

        private void cmbTJudging_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTJudging.SelectedIndex;

            if (index < cmbTJudging.Items.Count && index != -1 && !cmbTJudging.Items[index].ToString().Equals("Select game type"))
                _tournamentScoringType = cmbTJudging.Items[index].ToString();
        }


        private void cmbTteamsNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTteamsNumber.SelectedIndex;
            int newTeams = 0;
            int team = 65;

            if (index != 0)
            {
                if (index < cmbTteamsNumber.Items.Count && index != -1)
                    teamsNum = int.Parse(cmbTteamsNumber.Items[index].ToString());
                if (!suggestionChange)
                {
                    newTeams = teamsNum - selectedParticipants.Count;
                    if (newTeams > 0)
                    {
                        for (int i = 0; i < newTeams; i++)
                        {
                            selectedParticipants.Add(new List<AthleteData>());
                        }
                    }
                    else if (newTeams < 0)
                    {
                        for (int i = 0; i < Math.Abs(newTeams); i++)
                        {
                            foreach (AthleteData participant in selectedParticipants.ElementAt(selectedParticipants.Count - 1))
                            {
                                possibleParticipants.Add(participant);
                            }
                            selectedParticipants.RemoveAt(selectedParticipants.Count - 1);
                        }
                        showPossibleParticipants();
                    }

                }
                cmbTteamSelection.Items.Clear();
                for (int i = 0; i < teamsNum; i++)
                {
                    cmbTteamSelection.Items.Add("Team " + (char)team);
                    team++;
                }
                cmbTteamSelection.SelectedIndex = 0;
            }


        }

        private void cmbTteamSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbTteamSelection.SelectedIndex;
            if (index != -1)
            {
                _tournamentTeam = index;
                showSelectedParticipantsT(_tournamentTeam);
                // _tournamentTeamId = participantConnection.getTeamId(_tournamentTeam, _tournamentId);
                //selectedParticipantsT();
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
            if (checkFields("tournament") && checkTeams())
            {
                tournamentConnection.UpdateTournament(_tournamentId, _tournamentName, _tournamentSex, _tournamentAgeFrom, _tournamentAgeTo, _tournamentLevelFrom, _tournamentLevelTo, _tournamentGameType, _tournamentScoringType, _tournamentEventId);
                particpantsDeletionDB();
                participantsInsertionDB();
                MessageBox.Show("Succesfully saved!");
            }
        }

        private void btnTSaveNew_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields("tournament") && checkTeams())
            {
                _tournamentId = tournamentConnection.InsertNewTournament(_tournamentName, _tournamentSex, _tournamentAgeFrom, _tournamentAgeTo, _tournamentLevelFrom, _tournamentLevelTo, _tournamentGameType, _tournamentScoringType, _tournamentEventId);
                participantsInsertionDB();
                MessageBox.Show("Succesfully saved!");
            }
        }

        private void btnTDelete_Click(object sender, RoutedEventArgs e)
        {
            particpantsDeletionDB();
            tournamentConnection.deleteTournament(_tournamentId);
            
            MessageBox.Show("Succesfully deleted!");
            EventTournamentManagement etm = new EventTournamentManagement();
            etm.Activate();
            etm.Show();
            this.Close();
        }

        #endregion

        #region participants
        //find possible participants for selection
        public void showPossibleParticipantsByDB()
        {
            possibleParticipants = this.athletesFilter();
            lbTparticipants.ItemsSource = null;

            if (possibleParticipants.Count > 0)
            {
                lbTparticipants.ItemsSource = possibleParticipants;
            }
            else
            {
                lbTparticipants.ItemsSource = null;
            }
        }

        public void showSelectedParticipantsByDB(int selectedTeam)
        {
            DataSet ds;

            if ((bool)TrdButtonIndiv.IsChecked)
            {
                selectedParticipants = new List<List<AthleteData>>();
                selectedParticipants.Add(new List<AthleteData>());

                ds = participantConnection.getParticipantsI(_tournamentId);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AthleteData participant = new AthleteData();
                    participant.id = int.Parse(dr[0].ToString());
                    participant.athlete_name = dr[6].ToString() + " " + dr[7].ToString();
                    selectedParticipants.ElementAt(0).Add(participant);
                }
                showSelectedParticipantsI();
            }
            else
            {
                int team = 0;
                ds = participantConnection.getParticipantsT(_tournamentId);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AthleteData participant = new AthleteData();
                    participant.id = int.Parse(dr[0].ToString());
                    participant.athlete_name = dr[3].ToString() + " " + dr[4].ToString();

                    team = int.Parse(dr[1].ToString());
                    selectedParticipants.ElementAt(team).Add(participant);
                }
                showSelectedParticipantsT(0);
            }
        }

        public void showPossibleParticipants()
        {
            lbTparticipants.ItemsSource = null;
            if (possibleParticipants.Count > 0)
            {
                lbTparticipants.ItemsSource = possibleParticipants;
            }
            else
            {
                lbTparticipants.ItemsSource = null;
            }
        }

        public void showSelectedParticipantsI()
        {
            lbTSelectedParticipants.ItemsSource = null;
            if (selectedParticipants.Count > 0)
            {
                lbTSelectedParticipants.ItemsSource = selectedParticipants.ElementAt(0);
            }
            else
            {
                lbTSelectedParticipants.ItemsSource = null;
            }
        }

        public void showSelectedParticipantsT(int selectedTeam)
        {
            lbTTeams.ItemsSource = null;
            if (selectedParticipants.ElementAt(selectedTeam).Count > 0)
            {
                lbTTeams.ItemsSource = selectedParticipants.ElementAt(selectedTeam);
            }
            else
            {
                lbTTeams.ItemsSource = null;
            }
        }

        public void participantsInsertionDB()
        {
            DataSet ds;
            string rank = null;
            int i = 0;
            int j = 0;

            switch (_tournamentCatType)
            {
                case (Strings.individual):
                    foreach (AthleteData participant in selectedParticipants.ElementAt(0))
                    {
                        ds = athleteConnection.findAthlete(participant.id);
                        rank = ds.Tables[0].Rows[0][1].ToString();
                        participantConnection.InsertNewParticipantI(participant.id, _tournamentId, rank, 4);
                    }
                    break;
                case (Strings.team):
                    foreach (List<AthleteData> list in selectedParticipants)
                    {
                        _tournamentTeamId = participantConnection.InsertNewTeam(4, i, _tournamentId);
                        foreach (AthleteData participant in list)
                        {
                            ds = athleteConnection.findAthlete(participant.id);
                            rank = ds.Tables[0].Rows[0][1].ToString();
                            participantConnection.InsertNewParticipantT(participant.id, _tournamentId, rank, 4, _tournamentTeamId);
                        }
                        i++;
                    }
                    break;
            }
        }

        public void particpantsDeletionDB()
        {
            switch (_tournamentCatType)
            {
                case (Strings.individual):
                    foreach (AthleteData item in selectedParticipants.ElementAt(0))
                    {
                        participantConnection.deleteParticipantI(item.id, _tournamentId);
                    }
                    break;
                case (Strings.team):
                    foreach (List<AthleteData> list in selectedParticipants)
                    {
                        foreach (AthleteData participant in list)
                        {
                            participantConnection.deleteParticipantI(participant.id, _tournamentId);
                        }
                    }
                    break;
            }
        }

        private List<AthleteData> athletesFilter()
        {
            List<AthleteData> list = new List<AthleteData>();

            this.filteredAthletes = participantConnection.findPotentialParticipants(_tournamentSex, _tournamentAgeFrom, _tournamentAgeTo, _levelFrom - 1, _levelTo - 1, _tournamentId);
            int i = 1;
            foreach (DataRow dr in filteredAthletes.Tables[0].Rows)
            {
                AthleteData suggestion = new AthleteData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.athlete_name = dr[1].ToString() + " " + dr[2].ToString();
                list.Add(suggestion);
                i++;
            }
            return list;
        }

        private void btaddParticipant_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)TrdButtonIndiv.IsChecked)
            {
                List<AthleteData> list = new List<AthleteData>();

                foreach (Object item in lbTparticipants.SelectedItems)
                {
                    selectedParticipants.ElementAt(0).Add((AthleteData)item);

                    possibleParticipants.RemoveAt(lbTparticipants.Items.IndexOf(item));
                }
                showSelectedParticipantsI();
                showPossibleParticipants();
            }
            else if ((bool)TrdButtonTeam.IsChecked)
            {
                List<AthleteData> list = new List<AthleteData>();

                foreach (Object item in lbTparticipants.SelectedItems)
                {
                    selectedParticipants.ElementAt(_tournamentTeam).Add((AthleteData)item);

                    possibleParticipants.RemoveAt(lbTparticipants.Items.IndexOf(item));
                }
                showSelectedParticipantsT(_tournamentTeam);
                showPossibleParticipants();
            }
        }

        private void btdeleteParticipant_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)TrdButtonIndiv.IsChecked)
            {
                AthleteData participant = new AthleteData();

                foreach (Object item in lbTSelectedParticipants.SelectedItems)
                {
                    participant = (AthleteData)item;
                    selectedParticipants.ElementAt(0).RemoveAt(lbTSelectedParticipants.Items.IndexOf(item));
                    possibleParticipants.Add(participant);

                }
                showPossibleParticipants();
                showSelectedParticipantsI();
            }
        }


        #endregion


        #region helping methods
        //general
        private bool areParticipantsForDeletion()
        {
            bool flag = false;
            foreach (List<AthleteData> list in selectedParticipants)
            {
                if (list.Count > 0)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private string warningMessage()
        {
            string result = MessageBox.Show("Changes will delete selected participants. Press OK to continue.", "Message!",
               MessageBoxButton.OKCancel,
               MessageBoxImage.Information).ToString();

            return result;
        }

        private bool checkFields(string type)
        {
            if (type.Equals("event"))
            {
                if (_eventName == null)
                {
                    errorMessage("Event Name");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (cmbTEventChooser.SelectedIndex == 0)
                {
                    errorMessage("Event");
                    return false;
                }
                else if (_tournamentName == null)
                {
                    errorMessage("Tournament Name");
                    return false;
                }
                else if (_tournamentSex == null)
                {
                    errorMessage("Sex");
                    return false;
                }
                else if (_tournamentAgeFrom == 0)
                {
                    errorMessage("Age From");
                    return false;
                }
                else if (_tournamentAgeTo == 0)
                {
                    errorMessage("Age To");
                    return false;
                }
                else if (_tournamentLevelFrom == null)
                {
                    errorMessage("LevelFrom");
                    return false;
                }
                else if (_tournamentLevelTo == null)
                {
                    errorMessage("Level To");
                    return false;
                }
                else if (_tournamentCatType == null)
                {
                    errorMessage("Game Type");
                    return false;
                }
                else if (_tournamentGameType == null)
                {
                    errorMessage("Game");
                    return false;
                }
                else if (_tournamentScoringType == null)
                {
                    errorMessage("Judging type");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private bool checkTeams()
        {
            int i = 0;

            switch (_tournamentCatType)
            {
                case Strings.team:
                    switch (_tournamentGameType)
                    {
                        case Strings.teamKata:
                            foreach(List<AthleteData> list in selectedParticipants){
                                if (list.Count != 3)
                                {
                                    errorTeamMessage(i,1);
                                    return false;
                                }
                                i++;
                            }
                            break;
                        case Strings.teamKumite:
                             foreach(List<AthleteData> list in selectedParticipants){
                                if ( list.Count < 3 || list.Count > 4 )
                                {
                                    errorTeamMessage(i,2);
                                    return false;
                                }
                                i++;
                            }
                            break;
                    }
                    break;
                case Strings.synchronized:
                    switch (_tournamentGameType)
                    {
                        case Strings.enbu:
                            foreach(List<AthleteData> list in selectedParticipants){
                                if (list.Count != 2)
                                {
                                    errorTeamMessage(i,3);
                                    return false;
                                }
                                i++;
                            }
                            break;
                        case Strings.syncKata:
                             foreach(List<AthleteData> list in selectedParticipants){
                                if (list.Count != 3)
                                {
                                    errorTeamMessage(i,4);
                                    return false;
                                }
                                i++;
                            }
                            break;
                    }
                    break;
            }

            return true;
        }

        private void errorMessage(string formField)
        {
            string message = "Please complete" + formField;
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }

        private void errorTeamMessage(int team, int problem)
        {
            int teamName = 65;
            string message = "Team " + (char)(teamName + team) +" must have ";
            switch (problem)
            {
                case 1:
                    message = message + "3 athletes!";
                    break;
                case 2:
                    message = message + "3 or 4 athletes!";
                    break;
                case 3:
                    message = message + "2 athletes!";
                    break;
                case 4:
                    message = message + "3 athletes!";
                    break;

            }
            
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
            cmbTteamSelection.SelectedIndex = team;
        }

        //event methods
        private void eventList()
        {
            _eventName = eventName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            eventNameListForAutoComplete = this.EventsfilterNames();

            foreach (ListData item in eventNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                eSuggestionList.DataContext = autoList;
                eSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (eventName.Text.Equals(""))
            {
                eSuggestionList.Visibility = Visibility.Collapsed;
                eSuggestionList.DataContext = null;
            }
            else
            {
                eSuggestionList.Visibility = Visibility.Collapsed;
                eSuggestionList.DataContext = null;
            }
        }


        private List<ListData> EventsfilterNames()
        {
            List<ListData> list = new List<ListData>();

            this.filteredEvents = eventConnection.findSimilar(this.eventName.Text);

            foreach (DataRow dr in filteredEvents.Tables[0].Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString();
                list.Add(suggestion);
            }
            return list;
        }

        private void autocompleteByName(string name)
        {
            eventName.Text = name;
        }

        private void eventUpdateCities(string countryCode)
        {
            this.cities = cityConnection.GetCities(countryCode);

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

        //tournament methods

        private void tournamentList()
        {
            _tournamentName = tbTName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            tournamentNameListForAutoComplete = this.tournamentsfilterNames();

            foreach (ListData item in tournamentNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                tSuggestionList.DataContext = autoList;
                tSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (tbTName.Text.Equals(""))
            {
                tSuggestionList.Visibility = Visibility.Collapsed;
                tSuggestionList.DataContext = null;
            }
            else
            {
                tSuggestionList.Visibility = Visibility.Collapsed;
                tSuggestionList.DataContext = null;
            }
        }

        private List<ListData> tournamentsfilterNames()
        {
            List<ListData> list = new List<ListData>();


            this.filteredTournaments = tournamentConnection.findSimilar(this.tbTName.Text, _tournamentEventId);

            foreach (DataRow dr in filteredTournaments.Tables[0].Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString();
                list.Add(suggestion);
            }
            return list;
        }

        #endregion





    }
}
