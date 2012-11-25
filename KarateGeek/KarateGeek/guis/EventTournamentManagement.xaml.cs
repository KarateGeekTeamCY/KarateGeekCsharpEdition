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
        private Window sender;

        private DataSet editFilteredEvents;
        private DataSet editFilteredTournaments;
        private DataSet countries;
        private DataSet cities;
        private DataSet events;
        private List<ListData> eventNameListForAutoComplete;
        private List<ListData> tournamentNameListForAutoComplete;
        private List<AthleteData> editPossibleParticipants = new List<AthleteData>();
        private List<List<AthleteData>> editSelectedParticipants = new List<List<AthleteData>>();
        private bool editSuggestionChange = false;
        private bool newEventMode = true;
        private bool newTournamentMode = true;

        //
        //event variables
        //

        //new event variables
        private string _newEventName = null;
        private DateTime _newEventDate;
        private string _newEventLocation = null;
        private string _newEventPhone = null;
        private string _newEventEmail = null;
        private string _newEventAddress = null;
        private string _newEventAddressNum = null;
        private string _newEventPCode = null;
        private string _newEventCity = null;
        private string _newEventCountryCode = null;
        private Boolean _newEventOfficial;


        //edit event variables
        private int _editEventId;
        private string _editEventName = null;
        private DateTime _editEventDate;
        private string _editEventLocation = null;
        private string _editEventPhone = null;
        private string _editEventEmail = null;
        private string _editEventAddress = null;
        private string _editEventAddressNum = null;
        private string _editEventPCode = null;
        private string _editEventCity = null;
        private string _editEventCountryCode = null;
        private Boolean _editEventOfficial;

        //
        //tournament variables
        //

        //edit tournament variables
        private int _editTournamentEventId;
        private string _editTournamentEvent;
        private string _editTournamentName = null;
        private string _editTournamentSex = null;
        private int _editTournamentAgeFrom = 0;
        private int _editTournamentAgeTo = 0;
        private string _editTournamentLevelFrom = null;
        private string _editTournamentLevelTo = null;
        private string _editTournamentGameType = null;
        private string _editTournamentCatType = null;  //individual or team
        private string _editTournamentScoringType = null;
        private int _editTournamentTeamId = 0;
        private string _editEventInfo = null;
        private int _editTournamentTeam = 0;
        private int _editLevelFrom = 0;
        private int _editLevelTo = 0;
        private int _editTournamentId = 0;
        private int editTeamsNum = 0;
        private int editTeamNumber = 0;


        public EventTournamentManagement(Window sender)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            initialize();   //initialize gui and others

            this.sender = sender;
        }

        //constructor pou pernei san orisma tin imerominia pou tou exei perastei apo to main
        public EventTournamentManagement(DateTime dateSelection, Window sender)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            initialize();   //initialize gui and others
            this.newEventDate.SelectedDate = dateSelection;

            this.sender = sender;
        }

        public EventTournamentManagement(string name, Window sender)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            initialize();
            autocompleteByName(name);

            this.sender = sender;
        }


        private void initialize()
        {


            bteditDeleteParticipant.Content = "<<";
            //cities kai countries
            this.countries = countryConnection.GetCountries();

            foreach (DataRow dr in countries.Tables[0].Rows)
            {
                cmbNewECountryChooses.Items.Add(dr[1].ToString());
                cmbEditECountryChooses.Items.Add(dr[1].ToString());

            }
            cmbNewECountryChooses.SelectedIndex = 54;
            cmbEditECountryChooses.SelectedIndex = 54;

            this.newEventUpdateCities("CY");
            this.editEventUpdateCities("CY");

            //events
            this.events = eventConnection.getEvents();
            cmbEditTEventChooser.Items.Add("Select Event");

            foreach (DataRow dr in events.Tables[0].Rows)
            {
                cmbEditTEventChooser.Items.Add(dr[1].ToString());

            }
            cmbEditTEventChooser.SelectedIndex = 0;

            //ages
            cmbEditTAgeFrom.Items.Add("From");
            cmbEditTAgeTo.Items.Add("To");
            for (int i = 5; i <= 80; i++)
            {
                cmbEditTAgeFrom.Items.Add(i.ToString());
                cmbEditTAgeTo.Items.Add(i.ToString());
            }
            cmbEditTAgeFrom.SelectedIndex = 0;
            cmbEditTAgeTo.SelectedIndex = 0;

            //levels
            cmbEditTLevelFrom.Items.Add("From");
            cmbEditTLevelTo.Items.Add("To");
            for (int i = 0; i < KarateGeek.Strings.rank.Length; i++)
            {
                cmbEditTLevelFrom.Items.Add(KarateGeek.Strings.rank[i]);
                cmbEditTLevelTo.Items.Add(KarateGeek.Strings.rank[i]);
            }

            cmbEditTLevelFrom.SelectedIndex = 0;
            cmbEditTLevelTo.SelectedIndex = 0;


            //games
            cmbEditTGame.Items.Add("Select game type");
            cmbEditTGame.SelectedIndex = 0;

            //judging type
            cmbEditTJudging.Items.Add("Select judging type");



            //tournament number of teams
            cmbEditTteamsNumber.Items.Add("Teams");
            cmbEditTteamsNumber.Items.Add("2");
            cmbEditTteamsNumber.Items.Add("3");
            cmbEditTteamsNumber.Items.Add("4");
            cmbEditTteamsNumber.Items.Add("5");
            cmbEditTteamsNumber.Items.Add("6");
            cmbEditTteamsNumber.Items.Add("7");
            cmbEditTteamsNumber.Items.Add("8");
            cmbEditTteamsNumber.Items.Add("9");
            cmbEditTteamsNumber.Items.Add("10");

            cmbEditTteamsNumber.SelectedIndex = 0;
        }
        #endregion

        #region new event
        private void newEventName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newEventName = newEventName.Text;
        }

        private void newEventDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _newEventDate = newEventDate.SelectedDate.Value;
        }

        private void newEventLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newEventLocation = newEventLocation.Text;
        }

        private void newEventPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newEventPhone = newEventPhone.Text;
        }

        private void newEventEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newEventEmail = newEventEmail.Text;
        }

        private void newEventAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newEventAddress = newEventAddress.Text;
        }

        private void newEventAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newEventAddressNum = newEventAddressNum.Text;
        }

        private void newEventPCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newEventPCode = newEventPCode.Text;
        }

        private void cmbNewECountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewECountryChooses.SelectedIndex;

            _newEventCountryCode = countries.Tables[0].Rows[index][0].ToString();
            this.newEventUpdateCities(_newEventCountryCode);
        }

        private void cmbNewECityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewECityChooses.SelectedIndex;
            if (index < cmbNewECityChooses.Items.Count && index != -1)
                _newEventCity = cmbNewECityChooses.Items[index].ToString();
        }

        private void newEventOfficial_Checked(object sender, RoutedEventArgs e)
        {
            _newEventOfficial = (bool)newEventOfficial.IsChecked;
        }

        #region buttons
        private void btnNewEBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.sender.Show();
        }


        private void btnNewESave_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields("event", true))
            {
                eventConnection.InsertNewEvent(_newEventName, _newEventDate, _newEventAddress, _newEventAddressNum, _newEventPCode, _newEventLocation, _newEventPhone, _newEventEmail, _newEventCity, _newEventCountryCode, _newEventOfficial);
                MessageBox.Show("Succesfully saved!");
                EventTournamentManagement etm = new EventTournamentManagement(this.sender); // FIXME: this moves the window to the center of the screen
                etm.Activate();
                etm.Show();
                this.Close();
            }
        }


        #endregion
        #endregion

        #region edit event

        private void editEventName_TextChanged(object sender, TextChangedEventArgs e)
        {
            editEventList();
        }

        private void editEventDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _editEventDate = editEventDate.SelectedDate.Value;
        }

        private void editEventLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editEventLocation = editEventLocation.Text;
        }

        private void editEventPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editEventPhone = editEventPhone.Text;
        }

        private void editEventEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editEventEmail = editEventEmail.Text;
        }

        private void editEventAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editEventAddress = editEventAddress.Text;
        }

        private void editEventAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editEventAddressNum = editEventAddressNum.Text;
        }

        private void editEventPCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            _editEventPCode = editEventPCode.Text;
        }

        private void cmbEditECountryChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditECountryChooses.SelectedIndex;

            _editEventCountryCode = countries.Tables[0].Rows[index][0].ToString();
            this.editEventUpdateCities(_editEventCountryCode);
        }

        private void cmbEditECityChooses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditECityChooses.SelectedIndex;
            if (index < cmbEditECityChooses.Items.Count && index != -1)
                _editEventCity = cmbEditECityChooses.Items[index].ToString();
        }

        private void editEventOfficial_Checked(object sender, RoutedEventArgs e)
        {
            _editEventOfficial = (bool)editEventOfficial.IsChecked;
        }

        private void editESuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int country_position = 0;
            int city_position = 0;
            int index;
            int location_id;
            int address_id;
            DataSet dsA;
            DataSet dsL;

            if (editESuggestionList.ItemsSource != null)
            {
                editESuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                editEventName.TextChanged -= new TextChangedEventHandler(editEventName_TextChanged);

                index = editESuggestionList.SelectedIndex;

                if (editESuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)editESuggestionList.SelectedItem;
                    _editEventId = item.id;
                    location_id = int.Parse(editFilteredEvents.Tables[0].Rows[index][4].ToString());

                    this.editEventName.Text = editFilteredEvents.Tables[0].Rows[index][1].ToString();
                    this.editEventDate.SelectedDate = (DateTime)editFilteredEvents.Tables[0].Rows[index][2];
                    this.editEventOfficial.IsChecked = (Boolean)editFilteredEvents.Tables[0].Rows[index][3];

                    dsL = locationConnection.getLocation(location_id);

                    this.editEventLocation.Text = dsL.Tables[0].Rows[0][1].ToString();
                    this.editEventPhone.Text = dsL.Tables[0].Rows[0][2].ToString();
                    this.editEventEmail.Text = dsL.Tables[0].Rows[0][3].ToString();
                    address_id = location_id;

                    dsA = addressConnection.getAddress(address_id);

                    this.editEventAddress.Text = dsA.Tables[0].Rows[0][1].ToString();
                    this.editEventAddressNum.Text = dsA.Tables[0].Rows[0][2].ToString();
                    this.editEventPCode.Text = dsA.Tables[0].Rows[0][4].ToString();

                    string editEventCity = dsA.Tables[0].Rows[0][3].ToString();
                    int ix = dsA.Tables[0].Columns.Count;
                    string editEventCountry = dsA.Tables[0].Rows[0][5].ToString();


                    //
                    //the fix for the country selection error
                    //
                    DataSet countriname = countryConnection.getCountryNameByCode(editEventCountry);
                    editEventCountry = countriname.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbEditECountryChooses.Items.Count; i++)
                    {
                        if (editEventCountry.Equals(cmbEditECountryChooses.Items[i].ToString()))
                        {
                            country_position = i;
                            break;
                        }
                    }


                    this.cmbEditECountryChooses.SelectedIndex = country_position;

                    DataSet cityNa = cityConnection.GetCityNameByCityId(int.Parse(editEventCity));
                    editEventCity = cityNa.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < this.cmbEditECityChooses.Items.Count; i++)
                    {
                        if (editEventCity.Equals(cmbEditECityChooses.Items[i].ToString()))
                        {
                            city_position = i;
                            break;
                        }
                    }
                    this.cmbEditECityChooses.SelectedIndex = city_position;
                }
                editEventName.TextChanged += new TextChangedEventHandler(editEventName_TextChanged);
            }
        }

        #region buttons
        private void btnEditEBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.sender.Show();
        }

        private void btnEditESave_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields("event", false))
            {
                eventConnection.UpdateEvent(_editEventId, _editEventName, _editEventDate, _editEventAddress, _editEventAddressNum, _editEventPCode, _editEventLocation, _editEventPhone, _editEventEmail, _editEventCity, _editEventCountryCode, _editEventOfficial);
                MessageBox.Show("Succesfully saved!");
            }
        }

        private void btnEditEDelete_Click(object sender, RoutedEventArgs e)
        {
            eventConnection.deleteEvent(_editEventId);
            MessageBox.Show("Succesfully deleted!");
            EventTournamentManagement etm = new EventTournamentManagement(this.sender); // FIXME: this moves the window to the center of the screen
            etm.Activate();
            etm.Show();
            this.Close();
        }

        #endregion
        #endregion

        #region edit tournament

        private void cmbEditTEventChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTEventChooser.SelectedIndex;
            if (index != 0)
            {
                if (index < cmbEditTEventChooser.Items.Count && index != -1)
                    _editTournamentEvent = cmbEditTEventChooser.Items[index].ToString();
                editAutocomplete(_editTournamentEvent);
            }
        }

        private void editAutocomplete(string eventName)
        {
            DataSet dsE = null;
            DataSet dsL = null;
            DataSet dsA = null;
            DataSet dsC = null;
            int locationId;
            int addressId;
            int cityId;

            dsE = eventConnection.getEventsByName(eventName);
            _editTournamentEventId = int.Parse(dsE.Tables[0].Rows[0][0].ToString());
            locationId = int.Parse(dsE.Tables[0].Rows[0][4].ToString());
            dsL = locationConnection.getLocation(locationId);
            addressId = locationId;
            dsA = addressConnection.getAddress(addressId);
            cityId = int.Parse(dsA.Tables[0].Rows[0][3].ToString());
            dsC = cityConnection.GetCityNameByCityId(cityId);

            _editEventInfo = "Name: " + dsE.Tables[0].Rows[0][1].ToString() + "\n";
            _editEventInfo += "Date: " + dsE.Tables[0].Rows[0][2].ToString() + "\n";
            _editEventInfo += "City: " + dsC.Tables[0].Rows[0][0].ToString() + "\n";
            _editEventInfo += "Address: " + dsA.Tables[0].Rows[0][1].ToString() + "\n";
            _editEventInfo += "Num: " + dsA.Tables[0].Rows[0][2].ToString() + "    Postal Code:" + dsA.Tables[0].Rows[0][4].ToString() + "\n";
            _editEventInfo += "Location: " + dsL.Tables[0].Rows[0][1].ToString() + "\n";
            _editEventInfo += "Phone: " + dsL.Tables[0].Rows[0][2].ToString() + "\n";
            _editEventInfo += "Email: " + dsL.Tables[0].Rows[0][3].ToString();

            editEventInfo.Text = _editEventInfo;
        }

        private void tbEditTName_TextChanged(object sender, TextChangedEventArgs e)
        {
            editTournamentList();
        }


        private void editTSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
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


            if (editTSuggestionList.ItemsSource != null)
            {
                editTSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                tbEditTName.TextChanged -= new TextChangedEventHandler(tbEditTName_TextChanged);

                index = editTSuggestionList.SelectedIndex;

                if (editTSuggestionList.SelectedIndex != -1)
                {
                    editSuggestionChange = true;  //dixnei oti ginetai allagi twn listeners mesa apo suggestion change
                    ListData item = (ListData)editTSuggestionList.SelectedItem;
                    _editTournamentId = item.id;
                    _editTournamentName = item.name;

                    this.tbEditTName.Text = editFilteredTournaments.Tables[0].Rows[index][1].ToString();
                    sex = editFilteredTournaments.Tables[0].Rows[index][2].ToString();
                    ageFrom = editFilteredTournaments.Tables[0].Rows[index][3].ToString();
                    ageTo = editFilteredTournaments.Tables[0].Rows[index][4].ToString();
                    levelFrom = editFilteredTournaments.Tables[0].Rows[index][5].ToString();
                    levelTo = editFilteredTournaments.Tables[0].Rows[index][6].ToString();
                    game = editFilteredTournaments.Tables[0].Rows[index][7].ToString();
                    scoringType = editFilteredTournaments.Tables[0].Rows[index][8].ToString();

                    string[] gameCatType = game.Split('|');

                    if (sex.Equals(KarateGeek.Strings.male))
                    {
                        this.EditTrdButtonMale.IsChecked = true;
                    }
                    else
                    {
                        this.EditTrdButtonFemale.IsChecked = true;
                    }

                    for (int i = 0; i < this.cmbEditTAgeFrom.Items.Count; i++)
                    {
                        if (ageFrom.Equals(cmbEditTAgeFrom.Items[i].ToString()))
                        {
                            agePosition = i;
                            break;
                        }
                    }
                    this.cmbEditTAgeFrom.SelectedIndex = agePosition;

                    for (int i = 0; i < this.cmbEditTAgeTo.Items.Count; i++)
                    {
                        if (ageTo.Equals(cmbEditTAgeTo.Items[i].ToString()))
                        {
                            agePosition = i;
                            break;
                        }
                    }
                    this.cmbEditTAgeTo.SelectedIndex = agePosition;

                    for (int i = 0; i < this.cmbEditTLevelFrom.Items.Count; i++)
                    {
                        if (levelFrom.Equals(cmbEditTLevelFrom.Items[i].ToString()))
                        {
                            levelPosition = i;
                            break;
                        }
                    }
                    this.cmbEditTLevelFrom.SelectedIndex = levelPosition;

                    for (int i = 0; i < this.cmbEditTLevelTo.Items.Count; i++)
                    {
                        if (levelTo.Equals(cmbEditTLevelTo.Items[i].ToString()))
                        {
                            levelPosition = i;
                            break;
                        }
                    }
                    this.cmbEditTLevelTo.SelectedIndex = levelPosition;

                    if (gameCatType[0].Equals(Strings.individual))
                    {
                        _editTournamentCatType = Strings.individual;
                        this.EditTrdButtonIndiv.IsChecked = true;
                    }
                    else
                    {
                        _editTournamentCatType = Strings.team;
                        this.EditTrdButtonTeam.IsChecked = true;
                        teamsNum = participantConnection.getNumTeams(_editTournamentId);

                        editSelectedParticipants = new List<List<AthleteData>>();

                        for (int i = 0; i < teamsNum; i++)
                        {
                            editSelectedParticipants.Add(new List<AthleteData>());
                        }

                        cmbEditTteamsNumber.SelectedIndex = teamsNum - 1;
                    }

                    for (int i = 0; i < this.cmbEditTGame.Items.Count; i++)
                    {
                        if (gameCatType[1].Equals(cmbEditTGame.Items[i].ToString()))
                        {
                            gamePosition = i;
                            break;
                        }
                    }
                    this.cmbEditTGame.SelectedIndex = gamePosition;

                    for (int i = 0; i < this.cmbEditTJudging.Items.Count; i++)
                    {
                        if (scoringType.Equals(cmbEditTJudging.Items[i].ToString()))
                        {
                            scoringPosition = i;
                            break;
                        }
                    }
                    this.cmbEditTJudging.SelectedIndex = scoringPosition;



                    editShowPossibleParticipantsByDB();
                    editShowSelectedParticipantsByDB(0);
                    editSuggestionChange = false;
                }
                tbEditTName.TextChanged += new TextChangedEventHandler(tbEditTName_TextChanged);
            }
        }

        private void EditTrdButtonMale_Checked(object sender, RoutedEventArgs e)
        {
            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        _editTournamentSex = KarateGeek.Strings.male;
                        editParticpantsDeletionDB();

                        editSelectedParticipants = new List<List<AthleteData>>();
                        editSelectedParticipants.Add(new List<AthleteData>());
                        if (_editTournamentCatType == Strings.individual)
                        {
                            editShowPossibleParticipantsByDB();
                            editShowSelectedParticipantsI();
                        }
                        else
                        {
                            initializeTeams();
                            editShowPossibleParticipantsByDB();
                            editShowSelectedParticipantsT(0);
                        }
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                _editTournamentSex = KarateGeek.Strings.male;
                editShowPossibleParticipantsByDB();
            }

        }

        private void EditTrdButtonFemale_Checked(object sender, RoutedEventArgs e)
        {
            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        _editTournamentSex = KarateGeek.Strings.female;
                        editParticpantsDeletionDB();

                        editSelectedParticipants = new List<List<AthleteData>>();
                        editSelectedParticipants.Add(new List<AthleteData>());
                        if (_editTournamentCatType == Strings.individual)
                        {
                            editShowPossibleParticipantsByDB();
                            editShowSelectedParticipantsI();
                        }
                        else
                        {
                            initializeTeams();
                            editShowPossibleParticipantsByDB();
                            editShowSelectedParticipantsT(0);
                        }
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                _editTournamentSex = KarateGeek.Strings.female;
                editShowPossibleParticipantsByDB();
            }
        }

        private void cmbEditTAgeFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTAgeFrom.SelectedIndex;
            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbEditTAgeFrom.Items.Count && index != -1)
                                _editTournamentAgeFrom = int.Parse(cmbEditTAgeFrom.Items[index].ToString());
                            editParticpantsDeletionDB();
                            editSelectedParticipants = new List<List<AthleteData>>();
                            editSelectedParticipants.Add(new List<AthleteData>());
                            editShowPossibleParticipantsByDB();
                            if (_editTournamentCatType == Strings.individual)
                            {
                                editShowSelectedParticipantsI();
                            }
                            else
                            {
                                initializeTeams();
                                editShowSelectedParticipantsT(0);
                            }
                        }
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbEditTAgeFrom.Items.Count && index != -1)
                        _editTournamentAgeFrom = int.Parse(cmbEditTAgeFrom.Items[index].ToString());
                    editShowPossibleParticipantsByDB();
                }
            }

        }

        private void cmbEditTAgeTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTAgeTo.SelectedIndex;

            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbEditTAgeTo.Items.Count && index != -1)
                                _editTournamentAgeTo = int.Parse(cmbEditTAgeTo.Items[index].ToString());

                            editParticpantsDeletionDB();
                            editSelectedParticipants = new List<List<AthleteData>>();
                            editSelectedParticipants.Add(new List<AthleteData>());
                            editShowPossibleParticipantsByDB();
                            if (_editTournamentCatType == Strings.individual)
                            {
                                editShowSelectedParticipantsI();
                            }
                            else
                            {
                                initializeTeams();
                                editShowSelectedParticipantsT(0);
                            }
                        }
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {

                if (index != 0)
                {
                    if (index < cmbEditTAgeTo.Items.Count && index != -1)
                        _editTournamentAgeTo = int.Parse(cmbEditTAgeTo.Items[index].ToString());
                    editShowPossibleParticipantsByDB();
                }
            }
        }

        private void cmbEditTLevelFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTLevelFrom.SelectedIndex;
            _editLevelFrom = index;

            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbEditTLevelFrom.Items.Count && index != -1)
                                _editTournamentLevelFrom = cmbEditTLevelFrom.Items[index].ToString();

                            editParticpantsDeletionDB();
                            editSelectedParticipants = new List<List<AthleteData>>();
                            editSelectedParticipants.Add(new List<AthleteData>());
                            editShowPossibleParticipantsByDB();
                            if (_editTournamentCatType == Strings.individual)
                            {
                                editShowSelectedParticipantsI();
                            }
                            else
                            {
                                initializeTeams();
                                editShowSelectedParticipantsT(0);
                            }
                        }
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbEditTLevelFrom.Items.Count && index != -1)
                        _editTournamentLevelFrom = cmbEditTLevelFrom.Items[index].ToString();

                    editShowPossibleParticipantsByDB();
                }
            }
        }

        private void cmbEditTLevelTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTLevelTo.SelectedIndex;
            _editLevelTo = index;

            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbEditTLevelTo.Items.Count && index != -1)
                                _editTournamentLevelTo = cmbEditTLevelTo.Items[index].ToString();
                            editParticpantsDeletionDB();
                            editSelectedParticipants = new List<List<AthleteData>>();
                            editSelectedParticipants.Add(new List<AthleteData>());
                            editShowPossibleParticipantsByDB();
                            if (_editTournamentCatType == Strings.individual)
                            {
                                editShowSelectedParticipantsI();
                            }
                            else
                            {
                                initializeTeams();
                                editShowSelectedParticipantsT(0);
                            }
                        }
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbEditTLevelTo.Items.Count && index != -1)
                        _editTournamentLevelTo = cmbEditTLevelTo.Items[index].ToString();
                    editShowPossibleParticipantsByDB();
                }
            }
        }

        private void EditTrdButtonIndiv_Checked(object sender, RoutedEventArgs e)
        {
            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningMessage())
                {
                    case "OK":
                        _editTournamentCatType = KarateGeek.Strings.individual;

                        editSelectedParticipants = new List<List<AthleteData>>();
                        editSelectedParticipants.Add(new List<AthleteData>());
                        cmbEditTGame.Items.Clear();
                        cmbEditTGame.Items.Add("Select game type");

                        String[] gameType = KarateGeek.Strings.indKata.Split('|');
                        cmbEditTGame.Items.Add(gameType[1]);
                        gameType = KarateGeek.Strings.indKumite.Split('|');
                        cmbEditTGame.Items.Add(gameType[1]);
                        gameType = KarateGeek.Strings.fugugo.Split('|');
                        cmbEditTGame.Items.Add(gameType[1]);

                        cmbEditTGame.SelectedIndex = 0;

                        editIndividualGrid.Visibility = System.Windows.Visibility.Visible;
                        editTeamGrid.Visibility = System.Windows.Visibility.Hidden;
                        editShowPossibleParticipantsByDB();
                        editShowSelectedParticipantsI();
                        break;
                    case "Cancel":
                        break;

                }
            }
            else
            {
                _editTournamentCatType = KarateGeek.Strings.individual;
                editShowPossibleParticipantsByDB();
            }

        }

        private void EditTrdButtonTeam_Checked(object sender, RoutedEventArgs e)
        {
            _editTournamentCatType = KarateGeek.Strings.team;
            cmbEditTGame.Items.Clear();
            cmbEditTGame.Items.Add("Select game type");

            String[] gameType = KarateGeek.Strings.teamKata.Split('|');
            cmbEditTGame.Items.Add(gameType[1]);
            gameType = KarateGeek.Strings.teamKumite.Split('|');
            cmbEditTGame.Items.Add(gameType[1]);

            cmbEditTGame.SelectedIndex = 0;
            editTeamGrid.Visibility = System.Windows.Visibility.Visible;
            editIndividualGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void EditTrdButtonSync_Checked(object sender, RoutedEventArgs e)
        {
            _editTournamentCatType = KarateGeek.Strings.synchronized;
            cmbEditTGame.Items.Clear();
            cmbEditTGame.Items.Add("Select game type");

            String[] gameType = KarateGeek.Strings.syncKata.Split('|');
            cmbEditTGame.Items.Add(gameType[1]);
            gameType = KarateGeek.Strings.enbu.Split('|');
            cmbEditTGame.Items.Add(gameType[1]);

            cmbEditTGame.SelectedIndex = 0;
            editTeamGrid.Visibility = System.Windows.Visibility.Visible;
            editIndividualGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void cmbEditTGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTGame.SelectedIndex;

            cmbEditTJudging.Items.Clear();


            if (index != 0)
            {
                if (index < cmbEditTGame.Items.Count && index != -1)
                {
                    _editTournamentGameType = _editTournamentCatType + "|" + cmbEditTGame.Items[index].ToString();

                    if (_editTournamentGameType.Equals(Strings.indKata))
                    {
                        cmbEditTJudging.Items.Clear();
                        cmbEditTJudging.Items.Add("Select game type");
                        cmbEditTJudging.Items.Add(Strings.score);
                        cmbEditTJudging.Items.Add(Strings.flag);
                    }
                    else if (_editTournamentGameType.Equals(Strings.teamKata))
                    {
                        cmbEditTJudging.Items.Add(Strings.score);
                    }
                    else if (_editTournamentGameType.Equals(Strings.syncKata))
                    {
                        cmbEditTJudging.Items.Add(Strings.score);
                    }
                    else if (_editTournamentGameType.Equals(Strings.indKumite))
                    {
                        cmbEditTJudging.Items.Add(Strings.point);
                    }
                    else if (_editTournamentGameType.Equals(Strings.teamKumite))
                    {
                        cmbEditTJudging.Items.Add(Strings.point);
                    }
                    else if (_editTournamentGameType.Equals(Strings.enbu))
                    {
                        cmbEditTJudging.Items.Add(Strings.score);
                    }
                    else if (_editTournamentGameType.Equals(Strings.fugugo))
                    {
                        cmbEditTJudging.Items.Add(Strings.point + "/" + Strings.flag);
                    }
                    cmbEditTJudging.SelectedIndex = 0;
                }


            }
        }

        private void cmbEditTJudging_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTJudging.SelectedIndex;

            if (index < cmbEditTJudging.Items.Count && index != -1 && !cmbEditTJudging.Items[index].ToString().Equals("Select game type"))
                _editTournamentScoringType = cmbEditTJudging.Items[index].ToString();
        }


        private void cmbEditTteamsNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTteamsNumber.SelectedIndex;
            int newTeams = 0;
            int team = 65;

            if (index != 0)
            {
                if (index < cmbEditTteamsNumber.Items.Count && index != -1)
                    editTeamsNum = int.Parse(cmbEditTteamsNumber.Items[index].ToString());
                if (!editSuggestionChange)
                {
                    newTeams = editTeamsNum - editSelectedParticipants.Count;
                    if (newTeams > 0)
                    {
                        for (int i = 0; i < newTeams; i++)
                        {
                            editSelectedParticipants.Add(new List<AthleteData>());
                        }
                    }
                    else if (newTeams < 0)
                    {
                        for (int i = 0; i < Math.Abs(newTeams); i++)
                        {
                            foreach (AthleteData participant in editSelectedParticipants.ElementAt(editSelectedParticipants.Count - 1))
                            {
                                editPossibleParticipants.Add(participant);
                            }
                            editSelectedParticipants.RemoveAt(editSelectedParticipants.Count - 1);
                        }
                        editShowPossibleParticipants();
                    }

                }
                cmbEditTteamSelection.Items.Clear();
                for (int i = 0; i < editTeamsNum; i++)
                {
                    cmbEditTteamSelection.Items.Add("Team " + (char)team);
                    team++;
                }
                cmbEditTteamSelection.SelectedIndex = 0;
            }


        }

        private void cmbEditTteamSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTteamSelection.SelectedIndex;
            if (index != -1)
            {
                _editTournamentTeam = index;
                editShowSelectedParticipantsT(_editTournamentTeam);
                // _tournamentTeamId = participantConnection.getTeamId(_tournamentTeam, _tournamentId);
                //selectedParticipantsT();
            }

        }

        private void btnEditTBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.sender.Show();
        }

        private void btnEditTSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields("tournament", true) && editCheckTeams())
            {
                tournamentConnection.UpdateTournament(_editTournamentId, _editTournamentName, _editTournamentSex, _editTournamentAgeFrom, _editTournamentAgeTo, _editTournamentLevelFrom, _editTournamentLevelTo, _editTournamentGameType, _editTournamentScoringType, _editTournamentEventId);
                editParticpantsDeletionDB();
                editParticipantsInsertionDB();
                MessageBox.Show("Succesfully saved!");
            }
        }

        //private void btnTSaveNew_Click(object sender, RoutedEventArgs e)
        //{
        //    if (checkFields("tournament" , true) && checkTeams())
        //    {
        //        _tournamentId = tournamentConnection.InsertNewTournament(_tournamentName, _tournamentSex, _tournamentAgeFrom, _tournamentAgeTo, _tournamentLevelFrom, _tournamentLevelTo, _tournamentGameType, _tournamentScoringType, _tournamentEventId);
        //        participantsInsertionDB();
        //        MessageBox.Show("Succesfully saved!");
        //    }
        //}

        private void btnEditTDelete_Click(object sender, RoutedEventArgs e)
        {
            editParticpantsDeletionDB();
            tournamentConnection.deleteTournament(_editTournamentId);

            MessageBox.Show("Succesfully deleted!");
            EventTournamentManagement etm = new EventTournamentManagement(this.sender);
            etm.Activate();
            etm.Show();
            this.Close();
        }

        #endregion

        #region participants
        //find possible participants for selection
        public void editShowPossibleParticipantsByDB()
        {
            editPossibleParticipants = this.editAthletesFilter();
            lbEditTparticipants.ItemsSource = null;

            if (editPossibleParticipants.Count > 0)
            {
                lbEditTparticipants.ItemsSource = editPossibleParticipants;
            }
            else
            {
                lbEditTparticipants.ItemsSource = null;
            }
        }

        public void editShowSelectedParticipantsByDB(int selectedTeam)
        {
            DataSet ds;

            if ((bool)EditTrdButtonIndiv.IsChecked)
            {
                editSelectedParticipants = new List<List<AthleteData>>();
                editSelectedParticipants.Add(new List<AthleteData>());

                ds = participantConnection.getParticipantsI(_editTournamentId);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AthleteData participant = new AthleteData();
                    participant.id = int.Parse(dr[0].ToString());
                    participant.athlete_name = dr[6].ToString() + " " + dr[7].ToString();
                    editSelectedParticipants.ElementAt(0).Add(participant);
                }
                editShowSelectedParticipantsI();
            }
            else
            {
                int team = 0;
                ds = participantConnection.getParticipantsT(_editTournamentId);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AthleteData participant = new AthleteData();
                    participant.id = int.Parse(dr[0].ToString());
                    participant.athlete_name = dr[3].ToString() + " " + dr[4].ToString();

                    team = int.Parse(dr[1].ToString());
                    editSelectedParticipants.ElementAt(team).Add(participant);
                }
                editShowSelectedParticipantsT(0);
            }
        }

        public void editShowPossibleParticipants()
        {
            lbEditTparticipants.ItemsSource = null;
            if (editPossibleParticipants.Count > 0)
            {
                lbEditTparticipants.ItemsSource = editPossibleParticipants;
            }
            else
            {
                lbEditTparticipants.ItemsSource = null;
            }
        }

        public void editShowSelectedParticipantsI()
        {
            lbEditTSelectedParticipants.ItemsSource = null;
            if (editSelectedParticipants.Count > 0)
            {
                lbEditTSelectedParticipants.ItemsSource = editSelectedParticipants.ElementAt(0);
            }
            else
            {
                lbEditTSelectedParticipants.ItemsSource = null;
            }
        }

        public void editShowSelectedParticipantsT(int selectedTeam)
        {
            lbEditTTeams.ItemsSource = null;
            if (editSelectedParticipants.ElementAt(selectedTeam).Count > 0)
            {
                lbEditTTeams.ItemsSource = editSelectedParticipants.ElementAt(selectedTeam);
            }
            else
            {
                lbEditTTeams.ItemsSource = null;
            }
        }

        public void editParticipantsInsertionDB()
        {
            DataSet ds;
            string rank = null;
            int i = 0;
            int j = 0;

            switch (_editTournamentCatType)
            {
                case (Strings.individual):
                    foreach (AthleteData participant in editSelectedParticipants.ElementAt(0))
                    {
                        ds = athleteConnection.findAthlete(participant.id);
                        rank = ds.Tables[0].Rows[0][1].ToString();
                        participantConnection.InsertNewParticipantI(participant.id, _editTournamentId, rank);
                    }
                    break;
                case (Strings.team):
                    foreach (List<AthleteData> list in editSelectedParticipants)
                    {
                        _editTournamentTeamId = participantConnection.InsertNewTeam(i, _editTournamentId);
                        foreach (AthleteData participant in list)
                        {
                            ds = athleteConnection.findAthlete(participant.id);
                            rank = ds.Tables[0].Rows[0][1].ToString();
                            participantConnection.InsertNewParticipantT(participant.id, _editTournamentId, rank, _editTournamentTeamId);
                        }
                        i++;
                    }
                    break;
                case (Strings.synchronized):
                    foreach (List<AthleteData> list in editSelectedParticipants)
                    {
                        _editTournamentTeamId = participantConnection.InsertNewTeam(i, _editTournamentId);
                        foreach (AthleteData participant in list)
                        {
                            ds = athleteConnection.findAthlete(participant.id);
                            rank = ds.Tables[0].Rows[0][1].ToString();
                            participantConnection.InsertNewParticipantT(participant.id, _editTournamentId, rank, _editTournamentTeamId);
                        }
                        i++;
                    }
                    break;
            }
        }

        public void editParticpantsDeletionDB()
        {
            switch (_editTournamentCatType)
            {
                case (Strings.individual):
                    foreach (AthleteData item in editSelectedParticipants.ElementAt(0))
                    {
                        participantConnection.deleteParticipantI(item.id, _editTournamentId);
                    }
                    break;
                case (Strings.team):
                    foreach (List<AthleteData> list in editSelectedParticipants)
                    {
                        foreach (AthleteData participant in list)
                        {
                            participantConnection.deleteParticipantI(participant.id, _editTournamentId);
                        }
                    }
                    break;
            }
        }

        private List<AthleteData> editAthletesFilter()
        {
            DataSet filteredAthletes;
            List<AthleteData> list = new List<AthleteData>();

            filteredAthletes = participantConnection.findPotentialParticipants(_editTournamentSex, _editTournamentAgeFrom, _editTournamentAgeTo, _editLevelFrom - 1, _editLevelTo - 1, _editTournamentId);
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

        private void bteditAddParticipant_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)EditTrdButtonIndiv.IsChecked)
            {
                List<AthleteData> list = new List<AthleteData>();

                foreach (Object item in lbEditTparticipants.SelectedItems)
                {
                    editSelectedParticipants.ElementAt(0).Add((AthleteData)item);

                    editPossibleParticipants.RemoveAt(lbEditTparticipants.Items.IndexOf(item));
                }
                editShowSelectedParticipantsI();
                editShowPossibleParticipants();
            }
            else if ((bool)EditTrdButtonTeam.IsChecked || (bool)EditTrdButtonSync.IsChecked)
            {
                if (editTeamsNum >= 2)
                {
                    List<AthleteData> list = new List<AthleteData>();

                    foreach (Object item in lbEditTparticipants.SelectedItems)
                    {
                        editSelectedParticipants.ElementAt(_editTournamentTeam).Add((AthleteData)item);

                        editPossibleParticipants.RemoveAt(lbEditTparticipants.Items.IndexOf(item));
                    }
                    editShowSelectedParticipantsT(_editTournamentTeam);
                    editShowPossibleParticipants();
                }
                else
                {
                    MessageBox.Show("Select Number of Teams first!");
                }
            }
            else
            {
                MessageBox.Show("Please select Game Type!");
            }
        }

        private void bteditDeleteParticipant_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)EditTrdButtonIndiv.IsChecked)
            {
                AthleteData participant = new AthleteData();

                foreach (Object item in lbEditTSelectedParticipants.SelectedItems)
                {
                    participant = (AthleteData)item;
                    editSelectedParticipants.ElementAt(0).RemoveAt(lbEditTSelectedParticipants.Items.IndexOf(item));
                    editPossibleParticipants.Add(participant);

                }
                editShowPossibleParticipants();
                editShowSelectedParticipantsI();
            }
            else if ((bool)EditTrdButtonTeam.IsChecked || (bool)EditTrdButtonSync.IsChecked)
            {
                AthleteData participant = new AthleteData();

                foreach (Object item in lbEditTTeams.SelectedItems)
                {
                    participant = (AthleteData)item;
                    editSelectedParticipants.ElementAt(_editTournamentTeam).RemoveAt(lbEditTTeams.Items.IndexOf(item));
                    editPossibleParticipants.Add(participant);

                }
                editShowPossibleParticipants();
                editShowSelectedParticipantsT(_editTournamentTeam);
            }

        }


        #endregion

        #region helping methods
        //general
        private bool editAreParticipantsForDeletion()
        {
            bool flag = false;
            foreach (List<AthleteData> list in editSelectedParticipants)
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

        private bool checkFields(string type, bool mode)
        {
            if (type.Equals("event"))
            {
                if (mode)
                {
                    if (_newEventName == null)
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
                    if (_editEventName == null)
                    {
                        errorMessage("Event Name");
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
                if (mode)
                {
                    return true;
                }
                else
                {
                    if (cmbEditTEventChooser.SelectedIndex == 0)
                    {
                        errorMessage("Event");
                        return false;
                    }
                    else if (_editTournamentName == null)
                    {
                        errorMessage("Tournament Name");
                        return false;
                    }
                    else if (_editTournamentSex == null)
                    {
                        errorMessage("Sex");
                        return false;
                    }
                    else if (_editTournamentAgeFrom == 0)
                    {
                        errorMessage("Age From");
                        return false;
                    }
                    else if (_editTournamentAgeTo == 0)
                    {
                        errorMessage("Age To");
                        return false;
                    }
                    else if (_editTournamentLevelFrom == null)
                    {
                        errorMessage("LevelFrom");
                        return false;
                    }
                    else if (_editTournamentLevelTo == null)
                    {
                        errorMessage("Level To");
                        return false;
                    }
                    else if (_editTournamentCatType == null)
                    {
                        errorMessage("Game Type");
                        return false;
                    }
                    else if (_editTournamentGameType == null)
                    {
                        errorMessage("Game");
                        return false;
                    }
                    else if (_editTournamentScoringType == null)
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
        }

        private bool editCheckTeams()
        {
            int i = 0;

            switch (_editTournamentCatType)
            {
                case Strings.team:
                    switch (_editTournamentGameType)
                    {
                        case Strings.teamKata:
                            foreach (List<AthleteData> list in editSelectedParticipants)
                            {
                                if (list.Count != 3)
                                {
                                    errorTeamMessage(i, 1);
                                    return false;
                                }
                                i++;
                            }
                            break;
                        case Strings.teamKumite:
                            foreach (List<AthleteData> list in editSelectedParticipants)
                            {
                                if (list.Count < 3 || list.Count > 4)
                                {
                                    errorTeamMessage(i, 2);
                                    return false;
                                }
                                i++;
                            }
                            break;
                    }
                    break;
                case Strings.synchronized:
                    switch (_editTournamentGameType)
                    {
                        case Strings.enbu:
                            foreach (List<AthleteData> list in editSelectedParticipants)
                            {
                                if (list.Count != 2)
                                {
                                    errorTeamMessage(i, 3);
                                    return false;
                                }
                                i++;
                            }
                            break;
                        case Strings.syncKata:
                            foreach (List<AthleteData> list in editSelectedParticipants)
                            {
                                if (list.Count != 3)
                                {
                                    errorTeamMessage(i, 4);
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
            string message = "Team " + (char)(teamName + team) + " must have ";
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
            cmbEditTteamSelection.SelectedIndex = team;
        }

        private void initializeTeams()
        {
            cmbEditTteamsNumber.Items.Clear();
            cmbEditTteamsNumber.Items.Add("Teams");
            cmbEditTteamsNumber.Items.Add("2");
            cmbEditTteamsNumber.Items.Add("3");
            cmbEditTteamsNumber.Items.Add("4");
            cmbEditTteamsNumber.Items.Add("5");
            cmbEditTteamsNumber.Items.Add("6");
            cmbEditTteamsNumber.Items.Add("7");
            cmbEditTteamsNumber.Items.Add("8");
            cmbEditTteamsNumber.Items.Add("9");
            cmbEditTteamsNumber.Items.Add("10");


            cmbEditTteamSelection.Items.Clear();

            cmbEditTteamsNumber.SelectedIndex = 0;


        }

        //event methods
        private void editEventList()
        {
            _editEventName = editEventName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            eventNameListForAutoComplete = this.editEventsfilterNames();

            foreach (ListData item in eventNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                editESuggestionList.DataContext = autoList;
                editESuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (editEventName.Text.Equals(""))
            {
                editESuggestionList.Visibility = Visibility.Collapsed;
                editESuggestionList.DataContext = null;
            }
            else
            {
                editESuggestionList.Visibility = Visibility.Collapsed;
                editESuggestionList.DataContext = null;
            }
        }


        private List<ListData> editEventsfilterNames()
        {
            List<ListData> list = new List<ListData>();

            this.editFilteredEvents = eventConnection.findSimilar(this.editEventName.Text);

            foreach (DataRow dr in editFilteredEvents.Tables[0].Rows)
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
            editEventName.Text = name;
        }

        private void newEventUpdateCities(string countryCode)
        {
            this.cities = cityConnection.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbNewECityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbNewECityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbNewECityChooses.Items.Add(dr[1].ToString());
            }
            cmbNewECityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbNewECityChooses.Items.Refresh();
        }

        private void editEventUpdateCities(string countryCode)
        {
            this.cities = cityConnection.GetCities(countryCode);

            //cmbACityChooses = new ComboBox();

            int count = cmbEditECityChooses.Items.Count;
            for (int i = 0; i < count; i++)
            {
                cmbEditECityChooses.Items.RemoveAt(0);
            }

            foreach (DataRow dr in cities.Tables[0].Rows)
            {
                cmbEditECityChooses.Items.Add(dr[1].ToString());
            }
            cmbEditECityChooses.SelectedIndex = 0; //deixnei poio tha einai to proepilegmeno

            cmbEditECityChooses.Items.Refresh();
        }

        //tournament methods

        private void editTournamentList()
        {
            _editTournamentName = tbEditTName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            tournamentNameListForAutoComplete = this.editTournamentsfilterNames();

            foreach (ListData item in tournamentNameListForAutoComplete)
            {
                autoList.Add(item);
            }

            if (autoList.Count > 0)
            {
                editTSuggestionList.DataContext = autoList;
                editTSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (tbEditTName.Text.Equals(""))
            {
                editTSuggestionList.Visibility = Visibility.Collapsed;
                editTSuggestionList.DataContext = null;
            }
            else
            {
                editTSuggestionList.Visibility = Visibility.Collapsed;
                editTSuggestionList.DataContext = null;
            }
        }

        private List<ListData> editTournamentsfilterNames()
        {
            List<ListData> list = new List<ListData>();


            this.editFilteredTournaments = tournamentConnection.findSimilar(this.tbEditTName.Text, _editTournamentEventId);

            foreach (DataRow dr in editFilteredTournaments.Tables[0].Rows)
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
