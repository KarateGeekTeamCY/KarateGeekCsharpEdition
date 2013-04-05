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
using System.Text.RegularExpressions;
using KarateGeek.helpers;



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
        private List<List<AthleteData>> tempEditSelectedParticipants;
        private List<AthleteData> newPossibleParticipants = new List<AthleteData>();
        private List<List<AthleteData>> newSelectedParticipants = new List<List<AthleteData>>();
        private bool editSuggestionChange = false;
        RegularExpressions regex = new RegularExpressions();
        ErrorMessages em = new ErrorMessages();

        private static string defaultCountry = "CY";
        private int countryIndex;
        //
        //event variables
        //

        //new event variables
        private string _newEventName = null;
        private string _newEventDate;
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
        private int _editEventId = -1;
        private bool _editEventSelected = false;
        private string _editEventName = null;
        private string _editEventDate;
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
        //new tournament variables
        private static int MIN_AGE = 5;
        private static int MAX_AGE = 99;

        private int _newTournamentEventId;
        private string _newTournamentEvent;
        private string _newTournamentName = null;
        private string _newTournamentSex = null;
        private int _newTournamentAgeFrom = MIN_AGE;
        private int _newTournamentAgeTo = MAX_AGE;
        private string _newTournamentLevelFrom = KarateGeek.Strings.rank[0];
        private string _newTournamentLevelTo = KarateGeek.Strings.rank[KarateGeek.Strings.rank.Length - 1];
        private string _newTournamentGameType = null;
        private string _newTournamentCatType = null;  //individual or team
        private string _newTournamentScoringType = null;
        private int _newTournamentTeamId = 0;
        private string _newEventInfo = null;
        private int _newTournamentTeam = 0;
        private int _newLevelFrom = 1;
        private int _newLevelTo = KarateGeek.Strings.rank.Length;
        private int _newTournamentId = 0;
        private int newTeamsNum = 0;


        //edit tournament variables
        private int _editTournamentEventId;
        private bool _editTournamentSelected = false;
        private string _editTournamentEvent;
        private string _editTournamentName = null;
        private string _editTournamentSex = null;
        private int _editTournamentAgeFrom = MIN_AGE;
        private int _editTournamentAgeTo = MAX_AGE;
        private string _editTournamentLevelFrom = KarateGeek.Strings.rank[0];
        private string _editTournamentLevelTo = KarateGeek.Strings.rank[KarateGeek.Strings.rank.Length - 1];
        private string _editTournamentGameType = null;
        private string _editTournamentCatType = null;  //individual or team
        private string _editTournamentScoringType = null;
        private int _editTournamentTeamId = 0;
        private string _editEventInfo = null;
        private int _editTournamentTeam = 0;
        private int _editLevelFrom = 1;
        private int _editLevelTo = KarateGeek.Strings.rank.Length;
        private int _editTournamentId = 0;
        private int editTeamsNum = 0;


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
            newSelectedParticipants.Add(new List<AthleteData>());
            bteditDeleteParticipant.Content = "<<";
            btnewDeleteParticipant.Content = "<<";
            //cities kai countries
            this.countries = countryConnection.GetCountries();

            foreach (DataRow dr in countries.Tables[0].Rows)
            {
                cmbNewECountryChooses.Items.Add(dr[1].ToString());
                cmbEditECountryChooses.Items.Add(dr[1].ToString());

            }
            countryIndex = countryConnection.getIndexOfCountryCode(defaultCountry);
            cmbNewECountryChooses.SelectedIndex = countryIndex;
            cmbEditECountryChooses.SelectedIndex = countryIndex;

            this.newEventUpdateCities("CY");
            this.editEventUpdateCities("CY");

            //events
            this.events = eventConnection.getEvents();
            cmbEditTEventChooser.Items.Add("Select Event");
            cmbNewTEventChooser.Items.Add("Select Event");

            foreach (DataRow dr in events.Tables[0].Rows)
            {
                cmbEditTEventChooser.Items.Add(dr[1].ToString());
                cmbNewTEventChooser.Items.Add(dr[1].ToString());

            }
            cmbEditTEventChooser.SelectedIndex = 0;
            cmbNewTEventChooser.SelectedIndex = 0;

            //ages
            cmbEditTAgeFrom.Items.Add("From");
            cmbEditTAgeTo.Items.Add("To");
            cmbNewTAgeFrom.Items.Add("From");
            cmbNewTAgeTo.Items.Add("To");
            for (int i = MIN_AGE; i <= MAX_AGE; i++)
            {
                cmbEditTAgeFrom.Items.Add(i.ToString());
                cmbEditTAgeTo.Items.Add(i.ToString());
                cmbNewTAgeFrom.Items.Add(i.ToString());
                cmbNewTAgeTo.Items.Add(i.ToString());
            }
            cmbEditTAgeFrom.SelectedIndex = 0;
            cmbEditTAgeTo.SelectedIndex = 0;
            cmbNewTAgeFrom.SelectedIndex = 0;
            cmbNewTAgeTo.SelectedIndex = 0;

            //levels
            cmbEditTLevelFrom.Items.Add("From");
            cmbEditTLevelTo.Items.Add("To");
            cmbNewTLevelFrom.Items.Add("From");
            cmbNewTLevelTo.Items.Add("To");
            for (int i = 0; i < KarateGeek.Strings.rank.Length; i++)
            {
                cmbEditTLevelFrom.Items.Add(KarateGeek.Strings.rank[i]);
                cmbEditTLevelTo.Items.Add(KarateGeek.Strings.rank[i]);
                cmbNewTLevelFrom.Items.Add(KarateGeek.Strings.rank[i]);
                cmbNewTLevelTo.Items.Add(KarateGeek.Strings.rank[i]);
            }

            cmbEditTLevelFrom.SelectedIndex = 0;
            cmbEditTLevelTo.SelectedIndex = 0;
            cmbNewTLevelFrom.SelectedIndex = 0;
            cmbNewTLevelTo.SelectedIndex = 0;


            //games
            cmbEditTGame.Items.Add("Select game type");
            cmbEditTGame.SelectedIndex = 0;
            cmbNewTGame.Items.Add("Select game type");
            cmbNewTGame.SelectedIndex = 0;

            //judging type to allaksame se scoring alla den egine 
            //allagi se listener giati den exei refactor
            cmbEditTJudging.Items.Add("Select scoring type");
            cmbNewTJudging.Items.Add("Select scoring type");
            cmbNewTJudging.SelectedIndex = 0;
            cmbEditTJudging.SelectedIndex = 0;

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

            cmbNewTteamsNumber.Items.Add("Teams");
            cmbNewTteamsNumber.Items.Add("2");
            cmbNewTteamsNumber.Items.Add("3");
            cmbNewTteamsNumber.Items.Add("4");
            cmbNewTteamsNumber.Items.Add("5");
            cmbNewTteamsNumber.Items.Add("6");
            cmbNewTteamsNumber.Items.Add("7");
            cmbNewTteamsNumber.Items.Add("8");
            cmbNewTteamsNumber.Items.Add("9");
            cmbNewTteamsNumber.Items.Add("10");

            cmbNewTteamsNumber.SelectedIndex = 0;
        }
        #endregion

        #region new event
        private void newEventName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newEventName = newEventName.Text;
            if (setNewEventEnableSaveEnable())
            {
                btnNewESave.IsEnabled = true;
            }
            else
            {
                btnNewESave.IsEnabled = false;
            }
        }

        private void newEventDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Nullable<DateTime> selectedDate = newEventDate.SelectedDate;
            if (selectedDate.HasValue)
                _newEventDate = selectedDate.Value.ToShortDateString();
            else
                _newEventDate = string.Empty;
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
            if (checkNullOrEmptyFields("event", true) && checkWrongFields("event", true))
            {
                eventConnection.InsertNewEvent(_newEventName, _newEventDate, _newEventAddress, _newEventAddressNum, _newEventPCode, _newEventLocation, _newEventPhone, _newEventEmail, _newEventCity, _newEventCountryCode, _newEventOfficial);
                MessageBox.Show("Succesfully saved!", "Event Add", MessageBoxButton.OK);
                initializeNewEvent();
                if (cmbNewTEventChooser.SelectedIndex == 0)
                {
                    //add new event to new event chooser
                    cmbNewTEventChooser.Items.Clear();
                    this.events = eventConnection.getEvents();
                    cmbNewTEventChooser.Items.Add("Select Event");

                    foreach (DataRow dr in events.Tables[0].Rows)
                    {
                        cmbNewTEventChooser.Items.Add(dr[1].ToString());
                    }
                    cmbNewTEventChooser.SelectedIndex = 0;
                }
                if (cmbEditTEventChooser.SelectedIndex == 0)
                {
                    //add edit event to new event chooser
                    cmbEditTEventChooser.Items.Clear();
                    this.events = eventConnection.getEvents();
                    cmbEditTEventChooser.Items.Add("Select Event");

                    foreach (DataRow dr in events.Tables[0].Rows)
                    {
                        cmbEditTEventChooser.Items.Add(dr[1].ToString());
                    }
                    cmbEditTEventChooser.SelectedIndex = 0;
                }
            }
        }

        #endregion
        #endregion

        #region edit event

        #region autocomplete

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
                    _editEventSelected = true;
                    location_id = int.Parse(editFilteredEvents.Tables[0].Rows[index][4].ToString());

                    this.editEventName.Text = editFilteredEvents.Tables[0].Rows[index][1].ToString();
                    _editEventName = editFilteredEvents.Tables[0].Rows[index][1].ToString();  //to vazw etsi giati exei apenergopithei o listener
                    if (setEditEventSaveEnable())
                    {
                        btnEditESave.IsEnabled = true;
                    }
                    else
                    {
                        btnEditESave.IsEnabled = false;
                    }
                    string selectedDate = editFilteredEvents.Tables[0].Rows[index][2].ToString();
                    if (string.IsNullOrEmpty(selectedDate))
                    {
                        this.editEventDate.SelectedDate = null;
                    }
                    else
                    {
                        this.editEventDate.SelectedDate = (DateTime)editFilteredEvents.Tables[0].Rows[index][2];
                    }

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

        #endregion

        private void editEventName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = editEventName.Text;
            if (name == "" && !_editEventSelected)
            {
                initializeEditEvent();
            }
            editEventList();
        }

        private void editEventDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Nullable<DateTime> selectedDate = editEventDate.SelectedDate;
            if (selectedDate.HasValue)
                _editEventDate = selectedDate.Value.ToShortDateString();
            else
                _editEventDate = string.Empty;
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

        #region other listeners

        private void editEventName_LostFocus(object sender, RoutedEventArgs e)
        {
            editESuggestionList.Visibility = Visibility.Collapsed;
        }

        private void editEventName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            editEventList();
        }

        #endregion

        #region buttons
        private void btnEditEBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.sender.Show();
        }

        private void btnEditESave_Click(object sender, RoutedEventArgs e)
        {
            if (_editEventId != -1)
            {
                if (checkNullOrEmptyFields("event", false) && checkWrongFields("event", false))
                {
                    eventConnection.UpdateEvent(_editEventId, _editEventName, _editEventDate, _editEventAddress, _editEventAddressNum, _editEventPCode, _editEventLocation, _editEventPhone, _editEventEmail, _editEventCity, _editEventCountryCode, _editEventOfficial);
                    MessageBox.Show("Succesfully saved!", "Event Edit", MessageBoxButton.OK);
                    initializeEditEvent();
                }
            }
            else
            {
                MessageBox.Show("Please select one event to update", "Event Edit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnEditEDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((_newTournamentEventId == _editEventId) && (_editTournamentEventId == _editEventId))
            {
                switch (warningNewEditTournamentDeletion())
                {
                    case "OK":
                        eventConnection.deleteEvent(_editEventId);
                        MessageBox.Show("Succesfully deleted!", "Event Delete", MessageBoxButton.OK);
                        initializeEditEvent();
                        initializeNewTournament(true);
                        initializeEditTournament(true);
                        break;
                    case "Cancel":
                        break;
                }
            }
            else if ((_newTournamentEventId == _editEventId))
            {
                switch (warningEventNewTournamentDeletion())
                {
                    case "OK":
                        eventConnection.deleteEvent(_editEventId);
                        MessageBox.Show("Succesfully deleted!", "Event Delete", MessageBoxButton.OK);
                        initializeEditEvent();
                        initializeNewTournament(true);
                        initializeEditTournament(true);
                        break;
                    case "Cancel":
                        break;
                }
            }
            else if (_editTournamentEventId == _editEventId)
            {
                switch (warningEventEditTournamentDeletion())
                {
                    case "OK":
                        eventConnection.deleteEvent(_editEventId);
                        MessageBox.Show("Succesfully deleted!", "Event Delete", MessageBoxButton.OK);
                        initializeEditEvent();
                        initializeNewTournament(true);
                        initializeEditTournament(true);
                        break;
                    case "Cancel":
                        break;
                }
            }
            else
            {
                eventConnection.deleteEvent(_editEventId);
                MessageBox.Show("Succesfully deleted!", "Event Delete", MessageBoxButton.OK);
                initializeEditEvent();
                initializeNewTournament(true);
                initializeEditTournament(true);
            }

        }

        #endregion
        #endregion

        #region new tournament

        private void cmbNewTEventChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewTEventChooser.SelectedIndex;
            int previousIndex = 0;
            if (index != -1)
            {
                if (newAreParticipantsForDeletion())
                {
                    switch (warningParticipantsDeletionMessage())
                    {
                        case "OK":
                            if (index != 0)
                            {
                                if (index < cmbNewTEventChooser.Items.Count)
                                {
                                    _newTournamentEvent = cmbNewTEventChooser.Items[index].ToString();
                                    newEventAutocomplete(_newTournamentEvent);
                                }
                                cmbNewTEventChooser.SelectionChanged -= new SelectionChangedEventHandler(cmbNewTEventChooser_SelectionChanged);
                                initializeNewTournament(false);
                                cmbNewTEventChooser.SelectedIndex = index;
                                cmbNewTEventChooser.SelectionChanged += new SelectionChangedEventHandler(cmbNewTEventChooser_SelectionChanged);
                            }
                            break;
                        case "Cancel":
                            for (int i = 1; i < cmbNewTEventChooser.Items.Count; i++)
                            {
                                if (_newTournamentEvent.Equals(cmbNewTEventChooser.Items[i].ToString()))
                                {
                                    previousIndex = i;
                                    break;
                                }
                            }
                            cmbNewTEventChooser.SelectionChanged -= new SelectionChangedEventHandler(cmbNewTEventChooser_SelectionChanged);
                            cmbNewTEventChooser.SelectedIndex = previousIndex;
                            cmbNewTEventChooser.SelectionChanged += new SelectionChangedEventHandler(cmbNewTEventChooser_SelectionChanged);
                            break;
                    }
                }
                else
                {
                    if (index != 0)
                    {
                        if (index < cmbNewTEventChooser.Items.Count)
                        {
                            _newTournamentEvent = cmbNewTEventChooser.Items[index].ToString();
                            newEventAutocomplete(_newTournamentEvent);
                        }
                        cmbNewTEventChooser.SelectionChanged -= new SelectionChangedEventHandler(cmbNewTEventChooser_SelectionChanged);
                        initializeNewTournament(false);
                        cmbNewTEventChooser.SelectedIndex = index;
                        cmbNewTEventChooser.SelectionChanged += new SelectionChangedEventHandler(cmbNewTEventChooser_SelectionChanged);
                    }
                }
            }

        }

        private void newEventAutocomplete(string eventName)
        {
            DataSet dsE = null;
            DataSet dsL = null;
            DataSet dsA = null;
            DataSet dsC = null;
            DataSet dsCy = null;
            int locationId;
            int addressId;
            int cityId;

            dsE = eventConnection.getEventsByName(eventName);
            _newTournamentEventId = int.Parse(dsE.Tables[0].Rows[0][0].ToString());
            locationId = int.Parse(dsE.Tables[0].Rows[0][4].ToString());
            dsL = locationConnection.getLocation(locationId);
            addressId = locationId;
            dsA = addressConnection.getAddress(addressId);
            cityId = int.Parse(dsA.Tables[0].Rows[0][3].ToString());
            dsC = cityConnection.GetCityNameByCityId(cityId);
            dsCy = countryConnection.getCountryNameByCode(dsA.Tables[0].Rows[0][5].ToString());

            _newEventInfo = "Name:       " + dsE.Tables[0].Rows[0][1].ToString() + "\n";
            _newEventInfo += "Date:         " + dsE.Tables[0].Rows[0][2].ToString() + "\n";
            _newEventInfo += "City:          " + dsC.Tables[0].Rows[0][0].ToString() + "\n";
            _newEventInfo += "Country:    " + dsCy.Tables[0].Rows[0][0].ToString() + "\n";
            _newEventInfo += "Address:    " + dsA.Tables[0].Rows[0][1].ToString() + " , " + dsA.Tables[0].Rows[0][2].ToString() + "\n";
            _newEventInfo += "P. Code:    " + dsA.Tables[0].Rows[0][4].ToString() + "\n";
            _newEventInfo += "Location:   " + dsL.Tables[0].Rows[0][1].ToString() + "\n";
            _newEventInfo += "Phone:      " + dsL.Tables[0].Rows[0][2].ToString() + "\n";
            _newEventInfo += "Email:        " + dsL.Tables[0].Rows[0][3].ToString();

            newEventInfo.Text = _newEventInfo;
        }

        private void tbNewTName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _newTournamentName = tbNewTName.Text;
        }

        private void NewTrdButtonMixed_Checked(object sender, RoutedEventArgs e)
        {
            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        _newTournamentSex = KarateGeek.Strings.mixed;

                        newSelectedParticipants = new List<List<AthleteData>>();
                        newSelectedParticipants.Add(new List<AthleteData>());
                        if (_newTournamentCatType == Strings.individual)
                        {
                            newShowPossibleParticipantsByDB();
                            newShowSelectedParticipantsI();
                        }
                        else
                        {
                            newInitializeTeams();
                            newShowPossibleParticipantsByDB();
                            newShowSelectedParticipantsT(0);
                        }
                        break;
                    case "Cancel":
                        //disables the female radio button listener in order to restore female checked. after that it enables it again
                        switch (_newTournamentSex)
                        {
                            case KarateGeek.Strings.male:
                                NewTrdButtonMale.Checked -= new RoutedEventHandler(NewTrdButtonMale_Checked);
                                NewTrdButtonMale.IsChecked = true;
                                NewTrdButtonMale.Checked += new RoutedEventHandler(NewTrdButtonMale_Checked);
                                break;
                            case KarateGeek.Strings.female:
                                NewTrdButtonFemale.Checked -= new RoutedEventHandler(NewTrdButtonFemale_Checked);
                                NewTrdButtonFemale.IsChecked = true;
                                NewTrdButtonFemale.Checked += new RoutedEventHandler(NewTrdButtonFemale_Checked);
                                break;
                        }
                        break;
                }
            }
            else
            {
                _newTournamentSex = KarateGeek.Strings.mixed;
                newShowPossibleParticipantsByDB();
            }
        }

        private void NewTrdButtonMale_Checked(object sender, RoutedEventArgs e)
        {
            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        _newTournamentSex = KarateGeek.Strings.male;

                        newSelectedParticipants = new List<List<AthleteData>>();
                        newSelectedParticipants.Add(new List<AthleteData>());
                        if (_newTournamentCatType == Strings.individual)
                        {
                            newShowPossibleParticipantsByDB();
                            newShowSelectedParticipantsI();
                        }
                        else
                        {
                            newInitializeTeams();
                            newShowPossibleParticipantsByDB();
                            newShowSelectedParticipantsT(0);
                        }
                        break;
                    case "Cancel":
                        //disables the female radio button listener in order to restore female checked. after that it enables it again
                        switch (_newTournamentSex)
                        {
                            case KarateGeek.Strings.mixed:
                                NewTrdButtonMixed.Checked -= new RoutedEventHandler(NewTrdButtonMixed_Checked);
                                NewTrdButtonMixed.IsChecked = true;
                                NewTrdButtonMixed.Checked += new RoutedEventHandler(NewTrdButtonMixed_Checked);
                                break;
                            case KarateGeek.Strings.female:
                                NewTrdButtonFemale.Checked -= new RoutedEventHandler(NewTrdButtonFemale_Checked);
                                NewTrdButtonFemale.IsChecked = true;
                                NewTrdButtonFemale.Checked += new RoutedEventHandler(NewTrdButtonFemale_Checked);
                                break;
                        }
                        break;
                }
            }
            else
            {
                _newTournamentSex = KarateGeek.Strings.male;
                newShowPossibleParticipantsByDB();
            }

        }

        private void NewTrdButtonFemale_Checked(object sender, RoutedEventArgs e)
        {
            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        _newTournamentSex = KarateGeek.Strings.female;

                        newSelectedParticipants = new List<List<AthleteData>>();
                        newSelectedParticipants.Add(new List<AthleteData>());
                        if (_newTournamentCatType == Strings.individual)
                        {
                            newShowPossibleParticipantsByDB();
                            newShowSelectedParticipantsI();
                        }
                        else
                        {
                            newInitializeTeams();
                            newShowPossibleParticipantsByDB();
                            newShowSelectedParticipantsT(0);
                        }
                        break;
                    case "Cancel":
                        switch (_newTournamentSex)
                        {
                            case KarateGeek.Strings.mixed:
                                NewTrdButtonMixed.Checked -= new RoutedEventHandler(NewTrdButtonMixed_Checked);
                                NewTrdButtonMixed.IsChecked = true;
                                NewTrdButtonMixed.Checked += new RoutedEventHandler(NewTrdButtonMixed_Checked);
                                break;
                            case KarateGeek.Strings.male:
                                NewTrdButtonMale.Checked -= new RoutedEventHandler(NewTrdButtonMale_Checked);
                                NewTrdButtonMale.IsChecked = true;
                                NewTrdButtonMale.Checked += new RoutedEventHandler(NewTrdButtonMale_Checked);
                                break;
                        }
                        break;
                }
            }
            else
            {
                _newTournamentSex = KarateGeek.Strings.female;
                newShowPossibleParticipantsByDB();
            }
        }

        private void cmbNewTAgeFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewTAgeFrom.SelectedIndex;
            int previousIndex = 0;

            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbNewTAgeFrom.Items.Count && index != -1)
                                _newTournamentAgeFrom = int.Parse(cmbNewTAgeFrom.Items[index].ToString());
                        }else{
                            _newTournamentAgeFrom = MIN_AGE;
                        }
                            newSelectedParticipants = new List<List<AthleteData>>();
                            newSelectedParticipants.Add(new List<AthleteData>());
                            newShowPossibleParticipantsByDB();
                            if (_newTournamentCatType == Strings.individual)
                            {
                                newShowSelectedParticipantsI();
                            }
                            else
                            {
                                newInitializeTeams();
                                newShowSelectedParticipantsT(0);
                            }
                        
                        break;
                    case "Cancel":
                        for (int i = 1; i < cmbNewTAgeFrom.Items.Count; i++)
                        {
                            if (_newTournamentAgeFrom == int.Parse(cmbNewTAgeFrom.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        if (previousIndex == 0)
                            _newTournamentAgeFrom = MIN_AGE;
                  
                        cmbNewTAgeFrom.SelectionChanged -= new SelectionChangedEventHandler(cmbNewTAgeFrom_SelectionChanged);
                        cmbNewTAgeFrom.SelectedIndex = previousIndex;
                        cmbNewTAgeFrom.SelectionChanged += new SelectionChangedEventHandler(cmbNewTAgeFrom_SelectionChanged);
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbNewTAgeFrom.Items.Count && index != -1)
                        _newTournamentAgeFrom = int.Parse(cmbNewTAgeFrom.Items[index].ToString());   
                }
                else
                {
                    _newTournamentAgeFrom = MIN_AGE;
                }
                newShowPossibleParticipantsByDB();
            }

        }

        private void cmbNewTAgeTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewTAgeTo.SelectedIndex;

            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbNewTAgeTo.Items.Count && index != -1)
                                _newTournamentAgeTo = int.Parse(cmbNewTAgeTo.Items[index].ToString());
                        }else{
                            _newTournamentAgeTo = MAX_AGE;
                        }
                            newSelectedParticipants = new List<List<AthleteData>>();
                            newSelectedParticipants.Add(new List<AthleteData>());
                            newShowPossibleParticipantsByDB();
                            if (_newTournamentCatType == Strings.individual)
                            {
                                newShowSelectedParticipantsI();
                            }
                            else
                            {
                                newInitializeTeams();
                                newShowSelectedParticipantsT(0);
                            }
                        
                        break;
                    case "Cancel":
                        int previousIndex = 0;
                        for (int i = 1; i < cmbNewTAgeTo.Items.Count; i++)
                        {
                            if (_newTournamentAgeTo == int.Parse(cmbNewTAgeTo.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        if (previousIndex == 0)
                            _newTournamentAgeTo = MAX_AGE;
                        cmbNewTAgeTo.SelectionChanged -= new SelectionChangedEventHandler(cmbNewTAgeTo_SelectionChanged);
                        cmbNewTAgeTo.SelectedIndex = previousIndex;
                        cmbNewTAgeTo.SelectionChanged += new SelectionChangedEventHandler(cmbNewTAgeTo_SelectionChanged);
                        break;
                }
            }
            else
            {

                if (index != 0)
                {
                    if (index < cmbNewTAgeTo.Items.Count && index != -1)
                        _newTournamentAgeTo = int.Parse(cmbNewTAgeTo.Items[index].ToString());
               
                }
                else
                {
                    _newTournamentAgeTo = MAX_AGE;
                }
                newShowPossibleParticipantsByDB();
            }
        }

        private void cmbNewTLevelFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewTLevelFrom.SelectedIndex;
            

            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbNewTLevelFrom.Items.Count && index != -1)
                                _newTournamentLevelFrom = cmbNewTLevelFrom.Items[index].ToString();
                            _newLevelFrom = index;
                        }else
                        {
                            _newLevelFrom = 1;
                            _newTournamentLevelFrom = cmbNewTLevelFrom.Items[_newLevelFrom].ToString();
                        }
                            newSelectedParticipants = new List<List<AthleteData>>();
                            newSelectedParticipants.Add(new List<AthleteData>());
                            newShowPossibleParticipantsByDB();
                            if (_newTournamentCatType == Strings.individual)
                            {
                                newShowSelectedParticipantsI();
                            }
                            else
                            {
                                newInitializeTeams();
                                newShowSelectedParticipantsT(0);
                            }
                        break;
                    case "Cancel":
                        int previousIndex = 0;
                        for (int i = 1; i < cmbNewTLevelFrom.Items.Count; i++)
                        {
                            if (_newTournamentLevelFrom.Equals(cmbNewTLevelFrom.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        if (previousIndex == 0)
                            _newLevelFrom = 1;
                        else
                            _newLevelFrom = previousIndex;
                        _newTournamentLevelFrom = cmbNewTLevelFrom.Items[_newLevelFrom].ToString();
                        cmbNewTLevelFrom.SelectionChanged -= new SelectionChangedEventHandler(cmbNewTLevelFrom_SelectionChanged);
                        cmbNewTLevelFrom.SelectedIndex = previousIndex;
                        cmbNewTLevelFrom.SelectionChanged += new SelectionChangedEventHandler(cmbNewTLevelFrom_SelectionChanged);
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbNewTLevelFrom.Items.Count && index != -1)
                        _newTournamentLevelFrom = cmbNewTLevelFrom.Items[index].ToString();
                    _newLevelFrom = index;

                    
                }
                else
                {
                    _newLevelFrom = 1;
                    _newTournamentLevelFrom = cmbNewTLevelFrom.Items[_newLevelFrom].ToString();
                }
                newShowPossibleParticipantsByDB();
            }
        }

        private void cmbNewTLevelTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewTLevelTo.SelectedIndex;
            

            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbNewTLevelTo.Items.Count && index != -1)
                                _newTournamentLevelTo = cmbNewTLevelTo.Items[index].ToString();
                            
                            _newLevelTo = index;
                        }else
                        {
                            _newLevelTo = KarateGeek.Strings.rank.Length;
                            _newTournamentLevelTo = cmbNewTLevelTo.Items[_newLevelTo].ToString();
                        }
                            newSelectedParticipants = new List<List<AthleteData>>();
                            newSelectedParticipants.Add(new List<AthleteData>());
                            newShowPossibleParticipantsByDB();
                            if (_newTournamentCatType == Strings.individual)
                            {
                                newShowSelectedParticipantsI();
                            }
                            else
                            {
                                newInitializeTeams();
                                newShowSelectedParticipantsT(0);
                            } 
                        break;
                    case "Cancel":
                        int previousIndex = 0;
                        for (int i = 1; i < cmbNewTLevelTo.Items.Count; i++)
                        {
                            if (_newTournamentLevelTo.Equals(cmbNewTLevelTo.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        if(previousIndex == 0)
                            _newLevelTo = KarateGeek.Strings.rank.Length;
                        else
                            _newLevelTo = previousIndex;
                        _newTournamentLevelTo = cmbNewTLevelTo.Items[_newLevelTo].ToString();
                        cmbNewTLevelTo.SelectionChanged -= new SelectionChangedEventHandler(cmbNewTLevelTo_SelectionChanged);
                        cmbNewTLevelTo.SelectedIndex = previousIndex;
                        cmbNewTLevelTo.SelectionChanged += new SelectionChangedEventHandler(cmbNewTLevelTo_SelectionChanged);
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbNewTLevelTo.Items.Count && index != -1)
                        _newTournamentLevelTo = cmbNewTLevelTo.Items[index].ToString();
                    _newLevelTo = index;

                }
                else
                {
                    _newLevelTo = KarateGeek.Strings.rank.Length;
                    _newTournamentLevelTo = cmbNewTLevelTo.Items[_newLevelTo].ToString();
                }
                newShowPossibleParticipantsByDB();
            }
        }

        private void NewTrdButtonIndiv_Checked(object sender, RoutedEventArgs e)
        {
            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        _newTournamentCatType = KarateGeek.Strings.individual;

                        newSelectedParticipants = new List<List<AthleteData>>();
                        newSelectedParticipants.Add(new List<AthleteData>());
                        newShowPossibleParticipantsByDB();
                        newShowSelectedParticipantsI();

                        cmbNewTGame.Items.Clear();
                        cmbNewTGame.Items.Add("Select game type");

                        String[] gameType = KarateGeek.Strings.indKata.Split('|');
                        cmbNewTGame.Items.Add(gameType[1]);
                        gameType = KarateGeek.Strings.indKumite.Split('|');
                        cmbNewTGame.Items.Add(gameType[1]);
                        gameType = KarateGeek.Strings.fugugo.Split('|');
                        cmbNewTGame.Items.Add(gameType[1]);

                        cmbNewTGame.SelectedIndex = 0;

                        newIndividualGrid.Visibility = System.Windows.Visibility.Visible;
                        newTeamGrid.Visibility = System.Windows.Visibility.Hidden;

                        break;
                    case "Cancel":
                        switch (_newTournamentCatType)
                        {
                            case KarateGeek.Strings.team:
                                NewTrdButtonTeam.Checked -= new RoutedEventHandler(NewTrdButtonTeam_Checked);
                                NewTrdButtonTeam.IsChecked = true;
                                NewTrdButtonTeam.Checked += new RoutedEventHandler(NewTrdButtonTeam_Checked);
                                break;
                            case KarateGeek.Strings.synchronized:
                                NewTrdButtonSync.Checked -= new RoutedEventHandler(NewTrdButtonSync_Checked);
                                NewTrdButtonSync.IsChecked = true;
                                NewTrdButtonSync.Checked += new RoutedEventHandler(NewTrdButtonSync_Checked);
                                break;
                        }
                        break;

                }
            }
            else
            {
                _newTournamentCatType = KarateGeek.Strings.individual;

                cmbNewTGame.Items.Clear();
                cmbNewTGame.Items.Add("Select game type");

                String[] gameType = KarateGeek.Strings.indKata.Split('|');
                cmbNewTGame.Items.Add(gameType[1]);
                gameType = KarateGeek.Strings.indKumite.Split('|');
                cmbNewTGame.Items.Add(gameType[1]);
                gameType = KarateGeek.Strings.fugugo.Split('|');
                cmbNewTGame.Items.Add(gameType[1]);

                cmbNewTGame.SelectedIndex = 0;

                newIndividualGrid.Visibility = System.Windows.Visibility.Visible;
                newTeamGrid.Visibility = System.Windows.Visibility.Hidden;
                newShowPossibleParticipantsByDB();
            }

        }

        private void NewTrdButtonTeam_Checked(object sender, RoutedEventArgs e)
        {
            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        _newTournamentCatType = KarateGeek.Strings.team;


                        newSelectedParticipants = new List<List<AthleteData>>();
                        newSelectedParticipants.Add(new List<AthleteData>());
                        newShowPossibleParticipantsByDB();
                        newInitializeTeams();

                        cmbNewTGame.Items.Clear();
                        cmbNewTGame.Items.Add("Select game type");

                        String[] gameType = KarateGeek.Strings.teamKata.Split('|');
                        cmbNewTGame.Items.Add(gameType[1]);
                        gameType = KarateGeek.Strings.teamKumite.Split('|');
                        cmbNewTGame.Items.Add(gameType[1]);

                        cmbNewTGame.SelectedIndex = 0;
                        newTeamGrid.Visibility = System.Windows.Visibility.Visible;
                        newIndividualGrid.Visibility = System.Windows.Visibility.Hidden;
                        break;
                    case "Cancel":
                        switch (_newTournamentCatType)
                        {
                            case KarateGeek.Strings.individual:
                                NewTrdButtonIndiv.Checked -= new RoutedEventHandler(NewTrdButtonIndiv_Checked);
                                NewTrdButtonIndiv.IsChecked = true;
                                NewTrdButtonIndiv.Checked += new RoutedEventHandler(NewTrdButtonIndiv_Checked);
                                break;
                            case KarateGeek.Strings.synchronized:
                                NewTrdButtonSync.Checked -= new RoutedEventHandler(NewTrdButtonSync_Checked);
                                NewTrdButtonSync.IsChecked = true;
                                NewTrdButtonSync.Checked += new RoutedEventHandler(NewTrdButtonSync_Checked);
                                break;
                        }
                        break;
                }
            }
            else
            {
                _newTournamentCatType = KarateGeek.Strings.team;

                cmbNewTGame.Items.Clear();
                cmbNewTGame.Items.Add("Select game type");

                String[] gameType = KarateGeek.Strings.teamKata.Split('|');
                cmbNewTGame.Items.Add(gameType[1]);
                gameType = KarateGeek.Strings.teamKumite.Split('|');
                cmbNewTGame.Items.Add(gameType[1]);

                cmbNewTGame.SelectedIndex = 0;
                newTeamGrid.Visibility = System.Windows.Visibility.Visible;
                newIndividualGrid.Visibility = System.Windows.Visibility.Hidden;

            }
        }

        private void NewTrdButtonSync_Checked(object sender, RoutedEventArgs e)
        {
            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        _newTournamentCatType = KarateGeek.Strings.synchronized;
                        newSelectedParticipants = new List<List<AthleteData>>();
                        newSelectedParticipants.Add(new List<AthleteData>());
                        newShowPossibleParticipantsByDB();
                        newInitializeTeams();

                        cmbNewTGame.Items.Clear();
                        cmbNewTGame.Items.Add("Select game type");

                        String[] gameType = KarateGeek.Strings.syncKata.Split('|');
                        cmbNewTGame.Items.Add(gameType[1]);
                        gameType = KarateGeek.Strings.enbu.Split('|');
                        cmbNewTGame.Items.Add(gameType[1]);

                        cmbNewTGame.SelectedIndex = 0;
                        newTeamGrid.Visibility = System.Windows.Visibility.Visible;
                        newIndividualGrid.Visibility = System.Windows.Visibility.Hidden;
                        break;
                    case "Cancel":
                        switch (_newTournamentCatType)
                        {
                            case KarateGeek.Strings.individual:
                                NewTrdButtonIndiv.Checked -= new RoutedEventHandler(NewTrdButtonIndiv_Checked);
                                NewTrdButtonIndiv.IsChecked = true;
                                NewTrdButtonIndiv.Checked += new RoutedEventHandler(NewTrdButtonIndiv_Checked);
                                break;
                            case KarateGeek.Strings.team:
                                NewTrdButtonTeam.Checked -= new RoutedEventHandler(NewTrdButtonTeam_Checked);
                                NewTrdButtonTeam.IsChecked = true;
                                NewTrdButtonTeam.Checked += new RoutedEventHandler(NewTrdButtonTeam_Checked);
                                break;
                        }
                        break;
                }
            }
            else
            {
                _newTournamentCatType = KarateGeek.Strings.synchronized;

                cmbNewTGame.Items.Clear();
                cmbNewTGame.Items.Add("Select game type");

                String[] gameType = KarateGeek.Strings.syncKata.Split('|');
                cmbNewTGame.Items.Add(gameType[1]);
                gameType = KarateGeek.Strings.enbu.Split('|');
                cmbNewTGame.Items.Add(gameType[1]);

                cmbNewTGame.SelectedIndex = 0;
                newTeamGrid.Visibility = System.Windows.Visibility.Visible;
                newIndividualGrid.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void cmbNewTGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewTGame.SelectedIndex;

            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbNewTGame.Items.Count && index != -1)
                            {
                                _newTournamentGameType = _newTournamentCatType + "|" + cmbNewTGame.Items[index].ToString();
                                //ginetai i diagrafi twn athlitwn kai stin sinexeia allazei to cat type

                                newSelectedParticipants = new List<List<AthleteData>>();
                                newSelectedParticipants.Add(new List<AthleteData>());
                                newShowPossibleParticipantsByDB();
                                if (_newTournamentCatType == Strings.individual)
                                {
                                    newShowSelectedParticipantsI();
                                }
                                else
                                {
                                    newInitializeTeams();
                                    newShowSelectedParticipantsT(0);
                                }
                                //apo edw katw allazei to cat type
                                if (_newTournamentGameType.Equals(Strings.indKata))
                                {
                                    cmbNewTJudging.Items.Clear();
                                    cmbNewTJudging.Items.Add("Select game type");
                                    cmbNewTJudging.Items.Add(Strings.score);
                                    cmbNewTJudging.Items.Add(Strings.flag);
                                }
                                else if (_newTournamentGameType.Equals(Strings.teamKata))
                                {
                                    cmbNewTJudging.Items.Clear();
                                    cmbNewTJudging.Items.Add(Strings.score);
                                }
                                else if (_newTournamentGameType.Equals(Strings.syncKata))
                                {
                                    cmbNewTJudging.Items.Clear();
                                    cmbNewTJudging.Items.Add(Strings.score);
                                }
                                else if (_newTournamentGameType.Equals(Strings.indKumite))
                                {
                                    cmbNewTJudging.Items.Clear();
                                    cmbNewTJudging.Items.Add(Strings.point);
                                }
                                else if (_newTournamentGameType.Equals(Strings.teamKumite))
                                {
                                    cmbNewTJudging.Items.Clear();
                                    cmbNewTJudging.Items.Add(Strings.point);
                                }
                                else if (_newTournamentGameType.Equals(Strings.enbu))
                                {
                                    cmbNewTJudging.Items.Clear();
                                    cmbNewTJudging.Items.Add(Strings.score);
                                }
                                else if (_newTournamentGameType.Equals(Strings.fugugo))
                                {
                                    cmbNewTJudging.Items.Clear();
                                    cmbNewTJudging.Items.Add(Strings.point + "/" + Strings.flag);
                                }
                                cmbNewTJudging.SelectedIndex = 0;
                            }
                            else
                            {
                                cmbNewTJudging.Items.Clear();
                                cmbNewTJudging.Items.Add("Select scoring type");
                                cmbNewTJudging.SelectedIndex = 0;
                            }


                        }
                        break;
                    case "Cancel":
                        int previousIndex = 0;
                        string[] gameType = _newTournamentGameType.Split('|');

                        for (int i = 1; i < cmbNewTGame.Items.Count; i++)
                        {
                            if (gameType[1].Equals(cmbNewTGame.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        cmbNewTGame.SelectionChanged -= new SelectionChangedEventHandler(cmbNewTGame_SelectionChanged);
                        cmbNewTGame.SelectedIndex = previousIndex;
                        cmbNewTGame.SelectionChanged += new SelectionChangedEventHandler(cmbNewTGame_SelectionChanged);
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbNewTGame.Items.Count && index != -1)
                    {
                        _newTournamentGameType = _newTournamentCatType + "|" + cmbNewTGame.Items[index].ToString();

                        //apo edw katw allazei to cat type
                        if (_newTournamentGameType.Equals(Strings.indKata))
                        {
                            cmbNewTJudging.Items.Clear();
                            cmbNewTJudging.Items.Add("Select scoring type");
                            cmbNewTJudging.Items.Add(Strings.score);
                            cmbNewTJudging.Items.Add(Strings.flag);
                        }
                        else if (_newTournamentGameType.Equals(Strings.teamKata))
                        {
                            cmbNewTJudging.Items.Clear();
                            cmbNewTJudging.Items.Add(Strings.score);
                        }
                        else if (_newTournamentGameType.Equals(Strings.syncKata))
                        {
                            cmbNewTJudging.Items.Clear();
                            cmbNewTJudging.Items.Add(Strings.score);
                        }
                        else if (_newTournamentGameType.Equals(Strings.indKumite))
                        {
                            cmbNewTJudging.Items.Clear();
                            cmbNewTJudging.Items.Add(Strings.point);
                        }
                        else if (_newTournamentGameType.Equals(Strings.teamKumite))
                        {
                            cmbNewTJudging.Items.Clear();
                            cmbNewTJudging.Items.Add(Strings.point);
                        }
                        else if (_newTournamentGameType.Equals(Strings.enbu))
                        {
                            cmbNewTJudging.Items.Clear();
                            cmbNewTJudging.Items.Add(Strings.score);
                        }
                        else if (_newTournamentGameType.Equals(Strings.fugugo))
                        {
                            cmbNewTJudging.Items.Clear();
                            cmbNewTJudging.Items.Add(Strings.point + "/" + Strings.flag);
                        }
                        cmbNewTJudging.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbNewTJudging.Items.Clear();
                        cmbNewTJudging.Items.Add("Select scoring type");
                        cmbNewTJudging.SelectedIndex = 0;
                    }
                }

            }
        }

        private void cmbNewTJudging_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewTJudging.SelectedIndex;
            if (newAreParticipantsForDeletion())
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        //ginetai i diagrafi twn athlitwn kai stin sinexeia allazei to cat type
                        newSelectedParticipants = new List<List<AthleteData>>();
                        newSelectedParticipants.Add(new List<AthleteData>());
                        newShowPossibleParticipantsByDB();
                        if (_newTournamentCatType == Strings.individual)
                        {
                            newShowSelectedParticipantsI();
                        }
                        else
                        {
                            newInitializeTeams();
                            newShowSelectedParticipantsT(0);
                        }
                        if (index < cmbNewTJudging.Items.Count && index != -1 && !cmbNewTJudging.Items[index].ToString().Equals("Select scoring type"))
                            _newTournamentScoringType = cmbNewTJudging.Items[index].ToString();
                        break;
                    case "Cancel":
                        int previousIndex = 0;

                        for (int i = 1; i < cmbNewTJudging.Items.Count; i++)
                        {
                            if (_newTournamentScoringType.Equals(cmbNewTJudging.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        cmbNewTJudging.SelectionChanged -= new SelectionChangedEventHandler(cmbNewTJudging_SelectionChanged);
                        cmbNewTJudging.SelectedIndex = previousIndex;
                        cmbNewTJudging.SelectionChanged += new SelectionChangedEventHandler(cmbNewTJudging_SelectionChanged);
                        break;
                }
            }
            else
            {
                if (index < cmbNewTJudging.Items.Count && index != -1 && !cmbNewTJudging.Items[index].ToString().Equals("Select scoring type"))
                    _newTournamentScoringType = cmbNewTJudging.Items[index].ToString();
            }
        }

        private void cmbNewTteamsNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewTteamsNumber.SelectedIndex;
            int newTeams = 0;
            int team = 65;

            if (index != 0)
            {
                if (index < cmbNewTteamsNumber.Items.Count && index != -1)
                    newTeamsNum = int.Parse(cmbNewTteamsNumber.Items[index].ToString());

                newTeams = newTeamsNum - newSelectedParticipants.Count;
                if (newTeams > 0)
                {
                    for (int i = 0; i < newTeams; i++)
                    {
                        newSelectedParticipants.Add(new List<AthleteData>());
                    }
                }
                else if (newTeams < 0)
                {
                    for (int i = 0; i < Math.Abs(newTeams); i++)
                    {
                        foreach (AthleteData participant in newSelectedParticipants.ElementAt(newSelectedParticipants.Count - 1))
                        {
                            newPossibleParticipants.Add(participant);
                        }
                        newSelectedParticipants.RemoveAt(newSelectedParticipants.Count - 1);
                    }
                    newShowPossibleParticipants();
                }


                cmbNewTteamSelection.Items.Clear();
                for (int i = 0; i < newTeamsNum; i++)
                {
                    cmbNewTteamSelection.Items.Add("Team " + (char)team);
                    team++;
                }
                cmbNewTteamSelection.SelectedIndex = 0;
            }


        }

        private void cmbNewTteamSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbNewTteamSelection.SelectedIndex;
            if (index != -1)
            {
                _newTournamentTeam = index;
                newShowSelectedParticipantsT(_newTournamentTeam);
                // _tournamentTeamId = participantConnection.getTeamId(_tournamentTeam, _tournamentId);
                //selectedParticipantsT();
            }

        }

        private void btnNewTBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.sender.Show();
        }

        private void btnNewTSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkNullOrEmptyFields("tournament", true) && checkWrongFields("tournament", true) && newCheckTeams())
            {
                _newTournamentId = tournamentConnection.InsertNewTournament(_newTournamentName, _newTournamentSex, _newTournamentAgeFrom, _newTournamentAgeTo, _newTournamentLevelFrom, _newTournamentLevelTo, _newTournamentGameType, _newTournamentScoringType, _newTournamentEventId);
                newParticipantsInsertionDB();
                MessageBox.Show("Succesfully saved!", "Tournament Add", MessageBoxButton.OK);
                initializeNewTournament(true);
            }
        }

        #endregion

        #region edit tournament

        private void cmbEditTEventChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTEventChooser.SelectedIndex;

            if (index != -1)
            {
                if (index != 0)
                {
                    if (index < cmbEditTEventChooser.Items.Count)
                    {
                        _editTournamentEvent = cmbEditTEventChooser.Items[index].ToString();
                        editAutocomplete(_editTournamentEvent);
                    }
                    cmbEditTEventChooser.SelectionChanged -= new SelectionChangedEventHandler(cmbEditTEventChooser_SelectionChanged);
                    initializeEditTournament(false);
                    cmbEditTEventChooser.SelectedIndex = index;
                    cmbEditTEventChooser.SelectionChanged += new SelectionChangedEventHandler(cmbEditTEventChooser_SelectionChanged);
                }
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
            string name = tbEditTName.Text;
            if (name == "" && !_editTournamentSelected)
            {
                initializeEditTournament(false);
            }
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
                    _editTournamentSelected = true;
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
                    else if (sex.Equals(KarateGeek.Strings.female))
                    {
                        this.EditTrdButtonFemale.IsChecked = true;
                    }
                    else
                    {
                        this.EditTrdButtonMixed.IsChecked = true;
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
                    else if (gameCatType[0].Equals(Strings.team))
                    {
                        _editTournamentCatType = Strings.team;
                        this.EditTrdButtonTeam.IsChecked = true;
                        teamsNum = participantConnection.getNumTeams(_editTournamentId);

                        editSelectedParticipants = new List<List<AthleteData>>();
                        tempEditSelectedParticipants = new List<List<AthleteData>>();

                        for (int i = 0; i < teamsNum; i++)
                        {
                            editSelectedParticipants.Add(new List<AthleteData>());
                            tempEditSelectedParticipants.Add(new List<AthleteData>());
                        }

                        cmbEditTteamsNumber.SelectedIndex = teamsNum - 1;
                    }
                    else
                    {
                        _editTournamentCatType = Strings.synchronized;
                        this.EditTrdButtonSync.IsChecked = true;
                        teamsNum = participantConnection.getNumTeams(_editTournamentId);

                        editSelectedParticipants = new List<List<AthleteData>>();
                        tempEditSelectedParticipants = new List<List<AthleteData>>();

                        for (int i = 0; i < teamsNum; i++)
                        {
                            editSelectedParticipants.Add(new List<AthleteData>());
                            tempEditSelectedParticipants.Add(new List<AthleteData>());
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

        private void EditTrdButtonMixed_Checked(object sender, RoutedEventArgs e)
        {
            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        _editTournamentSex = KarateGeek.Strings.mixed;
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
                            editInitializeTeams();
                            editShowPossibleParticipantsByDB();
                            editShowSelectedParticipantsT(0);
                        }
                        break;
                    case "Cancel":
                        switch (_editTournamentSex)
                        {
                            case KarateGeek.Strings.male:
                                EditTrdButtonMale.Checked -= new RoutedEventHandler(EditTrdButtonMale_Checked);
                                EditTrdButtonMale.IsChecked = true;
                                EditTrdButtonMale.Checked += new RoutedEventHandler(EditTrdButtonMale_Checked);
                                break;
                            case KarateGeek.Strings.female:
                                EditTrdButtonFemale.Checked -= new RoutedEventHandler(EditTrdButtonFemale_Checked);
                                EditTrdButtonFemale.IsChecked = true;
                                EditTrdButtonFemale.Checked += new RoutedEventHandler(EditTrdButtonFemale_Checked);
                                break;
                        }
                        break;
                }
            }
            else
            {
                _editTournamentSex = KarateGeek.Strings.mixed;
                editShowPossibleParticipantsByDB();
            }
        }

        private void EditTrdButtonMale_Checked(object sender, RoutedEventArgs e)
        {
            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningParticipantsDeletionMessage())
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
                            editInitializeTeams();
                            editShowPossibleParticipantsByDB();
                            editShowSelectedParticipantsT(0);
                        }
                        break;
                    case "Cancel":
                        switch (_editTournamentSex)
                        {
                            case KarateGeek.Strings.mixed:
                                EditTrdButtonMixed.Checked -= new RoutedEventHandler(EditTrdButtonMixed_Checked);
                                EditTrdButtonMixed.IsChecked = true;
                                EditTrdButtonMixed.Checked += new RoutedEventHandler(EditTrdButtonMixed_Checked);
                                break;
                            case KarateGeek.Strings.female:
                                EditTrdButtonFemale.Checked -= new RoutedEventHandler(EditTrdButtonFemale_Checked);
                                EditTrdButtonFemale.IsChecked = true;
                                EditTrdButtonFemale.Checked += new RoutedEventHandler(EditTrdButtonFemale_Checked);
                                break;
                        }
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
                switch (warningParticipantsDeletionMessage())
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
                            editInitializeTeams();
                            editShowPossibleParticipantsByDB();
                            editShowSelectedParticipantsT(0);
                        }
                        break;
                    case "Cancel":
                        switch (_editTournamentSex)
                        {
                            case KarateGeek.Strings.mixed:
                                EditTrdButtonMixed.Checked -= new RoutedEventHandler(EditTrdButtonMixed_Checked);
                                EditTrdButtonMixed.IsChecked = true;
                                EditTrdButtonMixed.Checked += new RoutedEventHandler(EditTrdButtonMixed_Checked);
                                break;
                            case KarateGeek.Strings.male:
                                EditTrdButtonMale.Checked -= new RoutedEventHandler(EditTrdButtonMale_Checked);
                                EditTrdButtonMale.IsChecked = true;
                                EditTrdButtonMale.Checked += new RoutedEventHandler(EditTrdButtonMale_Checked);
                                break;
                        }
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
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbEditTAgeFrom.Items.Count && index != -1)
                                _editTournamentAgeFrom = int.Parse(cmbEditTAgeFrom.Items[index].ToString());
                        }else {
                            _editTournamentAgeFrom = MIN_AGE;
                        }
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
                                editInitializeTeams();
                                editShowSelectedParticipantsT(0);
                            }
                        
                        break;
                    case "Cancel":
                        int previousIndex = 0;
                        for (int i = 1; i < cmbEditTAgeFrom.Items.Count; i++)
                        {
                            if (_editTournamentAgeFrom == int.Parse(cmbEditTAgeFrom.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        if (previousIndex == 0)
                            _editTournamentAgeFrom = MIN_AGE;
                        cmbEditTAgeFrom.SelectionChanged -= new SelectionChangedEventHandler(cmbEditTAgeFrom_SelectionChanged);
                        cmbEditTAgeFrom.SelectedIndex = previousIndex;
                        cmbEditTAgeFrom.SelectionChanged += new SelectionChangedEventHandler(cmbEditTAgeFrom_SelectionChanged);
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbEditTAgeFrom.Items.Count && index != -1)
                        _editTournamentAgeFrom = int.Parse(cmbEditTAgeFrom.Items[index].ToString());
                    
                }
                else
                {
                    _editTournamentAgeFrom = MIN_AGE;
                }
                editShowPossibleParticipantsByDB();
            }

        }

        private void cmbEditTAgeTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTAgeTo.SelectedIndex;

            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbEditTAgeTo.Items.Count && index != -1)
                                _editTournamentAgeTo = int.Parse(cmbEditTAgeTo.Items[index].ToString());
                        }else{
                            _editTournamentAgeTo = MAX_AGE;
                        }
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
                                editInitializeTeams();
                                editShowSelectedParticipantsT(0);
                            }
                        
                        break;
                    case "Cancel":
                        int previousIndex = 0;
                        for (int i = 1; i < cmbEditTAgeTo.Items.Count; i++)
                        {
                            if (_editTournamentAgeTo == int.Parse(cmbEditTAgeTo.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        if (previousIndex == 0)
                            _editTournamentAgeTo = MAX_AGE;
                        cmbEditTAgeTo.SelectionChanged -= new SelectionChangedEventHandler(cmbEditTAgeTo_SelectionChanged);
                        cmbEditTAgeTo.SelectedIndex = previousIndex;
                        cmbEditTAgeTo.SelectionChanged += new SelectionChangedEventHandler(cmbEditTAgeTo_SelectionChanged);
                        break;
                }
            }
            else
            {

                if (index != 0)
                {
                    if (index < cmbEditTAgeTo.Items.Count && index != -1)
                        _editTournamentAgeTo = int.Parse(cmbEditTAgeTo.Items[index].ToString());

                }
                else
                {
                    _editTournamentAgeTo = MAX_AGE;
                }
                editShowPossibleParticipantsByDB();
            }
        }

        private void cmbEditTLevelFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTLevelFrom.SelectedIndex;
            

            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbEditTLevelFrom.Items.Count && index != -1)
                                _editTournamentLevelFrom = cmbEditTLevelFrom.Items[index].ToString();

                            _editLevelFrom = index;
                        }   else
                        {
                            _editLevelFrom = 1;
                            _editTournamentLevelFrom = cmbEditTLevelFrom.Items[_editLevelFrom].ToString();
                        }
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
                                editInitializeTeams();
                                editShowSelectedParticipantsT(0);
                            }                        
                        break;
                    case "Cancel":
                        int previousIndex = 0;
                        for (int i = 1; i < cmbEditTLevelFrom.Items.Count; i++)
                        {
                            if (_editTournamentLevelFrom.Equals(cmbEditTLevelFrom.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        if(previousIndex == 0)
                            _editLevelFrom = 1;
                        else
                            _editLevelFrom = previousIndex;
                        _editTournamentLevelFrom = cmbEditTLevelFrom.Items[_editLevelFrom].ToString();
                        cmbEditTLevelFrom.SelectionChanged -= new SelectionChangedEventHandler(cmbEditTLevelFrom_SelectionChanged);
                        cmbEditTLevelFrom.SelectedIndex = previousIndex;
                        cmbEditTLevelFrom.SelectionChanged += new SelectionChangedEventHandler(cmbEditTLevelFrom_SelectionChanged);
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbEditTLevelFrom.Items.Count && index != -1)
                        _editTournamentLevelFrom = cmbEditTLevelFrom.Items[index].ToString();
                    _editLevelFrom = index;
                   
                }
                else
                {
                    _editLevelFrom = 1;
                    _editTournamentLevelFrom = cmbEditTLevelFrom.Items[_editLevelFrom].ToString();
                }
                editShowPossibleParticipantsByDB();
            }
        }

        private void cmbEditTLevelTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTLevelTo.SelectedIndex;
            

            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbEditTLevelTo.Items.Count && index != -1)
                                _editTournamentLevelTo = cmbEditTLevelTo.Items[index].ToString();
                            _editLevelTo = index;
                        }   else
                        {
                            _editLevelTo = KarateGeek.Strings.rank.Length;
                            _editTournamentLevelTo = cmbEditTLevelTo.Items[_editLevelTo].ToString();
                        }
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
                                editInitializeTeams();
                                editShowSelectedParticipantsT(0);
                            } 
                        break;
                    case "Cancel":
                        int previousIndex = 0;
                        for (int i = 1; i < cmbEditTLevelTo.Items.Count; i++)
                        {
                            if (_editTournamentLevelTo.Equals(cmbEditTLevelTo.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        if(previousIndex == 0)
                            _editLevelTo = KarateGeek.Strings.rank.Length;
                        else
                            _editLevelTo = previousIndex;
                        _editTournamentLevelTo = cmbEditTLevelTo.Items[_editLevelTo].ToString();
                        cmbEditTLevelTo.SelectionChanged -= new SelectionChangedEventHandler(cmbEditTLevelTo_SelectionChanged);
                        cmbEditTLevelTo.SelectedIndex = previousIndex;
                        cmbEditTLevelTo.SelectionChanged += new SelectionChangedEventHandler(cmbEditTLevelTo_SelectionChanged);
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbEditTLevelTo.Items.Count && index != -1)
                        _editTournamentLevelTo = cmbEditTLevelTo.Items[index].ToString();
                    _editLevelTo = index;
                    
                }
                else
                {
                    _editLevelTo = KarateGeek.Strings.rank.Length;
                    _editTournamentLevelTo = cmbEditTLevelTo.Items[_editLevelTo].ToString();
                }
                editShowPossibleParticipantsByDB();
            }
        }

        private void EditTrdButtonIndiv_Checked(object sender, RoutedEventArgs e)
        {
            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        editParticpantsDeletionDB();
                        _editTournamentCatType = KarateGeek.Strings.individual;

                        editSelectedParticipants = new List<List<AthleteData>>();
                        editSelectedParticipants.Add(new List<AthleteData>());
                        editShowPossibleParticipantsByDB();
                        editShowSelectedParticipantsI();

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

                        break;
                    case "Cancel":
                        switch (_editTournamentCatType)
                        {
                            case KarateGeek.Strings.team:
                                EditTrdButtonTeam.Checked -= new RoutedEventHandler(EditTrdButtonTeam_Checked);
                                EditTrdButtonTeam.IsChecked = true;
                                EditTrdButtonTeam.Checked += new RoutedEventHandler(EditTrdButtonTeam_Checked);
                                break;
                            case KarateGeek.Strings.synchronized:
                                EditTrdButtonSync.Checked -= new RoutedEventHandler(EditTrdButtonSync_Checked);
                                EditTrdButtonSync.IsChecked = true;
                                EditTrdButtonSync.Checked += new RoutedEventHandler(EditTrdButtonSync_Checked);
                                break;
                        }
                        break;

                }
            }
            else
            {
                _editTournamentCatType = KarateGeek.Strings.individual;

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
            }

        }

        private void EditTrdButtonTeam_Checked(object sender, RoutedEventArgs e)
        {
            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        editParticpantsDeletionDB();
                        _editTournamentCatType = KarateGeek.Strings.team;


                        editSelectedParticipants = new List<List<AthleteData>>();
                        editSelectedParticipants.Add(new List<AthleteData>());
                        editShowPossibleParticipantsByDB();
                        editInitializeTeams();

                        cmbEditTGame.Items.Clear();
                        cmbEditTGame.Items.Add("Select game type");

                        String[] gameType = KarateGeek.Strings.teamKata.Split('|');
                        cmbEditTGame.Items.Add(gameType[1]);
                        gameType = KarateGeek.Strings.teamKumite.Split('|');
                        cmbEditTGame.Items.Add(gameType[1]);

                        cmbEditTGame.SelectedIndex = 0;
                        editTeamGrid.Visibility = System.Windows.Visibility.Visible;
                        editIndividualGrid.Visibility = System.Windows.Visibility.Hidden;
                        break;
                    case "Cancel":
                        switch (_editTournamentCatType)
                        {
                            case KarateGeek.Strings.individual:
                                EditTrdButtonIndiv.Checked -= new RoutedEventHandler(EditTrdButtonIndiv_Checked);
                                EditTrdButtonIndiv.IsChecked = true;
                                EditTrdButtonIndiv.Checked += new RoutedEventHandler(EditTrdButtonIndiv_Checked);
                                break;
                            case KarateGeek.Strings.synchronized:
                                EditTrdButtonSync.Checked -= new RoutedEventHandler(EditTrdButtonSync_Checked);
                                EditTrdButtonSync.IsChecked = true;
                                EditTrdButtonSync.Checked += new RoutedEventHandler(EditTrdButtonSync_Checked);
                                break;
                        }
                        break;
                }
            }
            else
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
        }

        private void EditTrdButtonSync_Checked(object sender, RoutedEventArgs e)
        {
            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        editParticpantsDeletionDB();
                        _editTournamentCatType = KarateGeek.Strings.synchronized;
                        editSelectedParticipants = new List<List<AthleteData>>();
                        editSelectedParticipants.Add(new List<AthleteData>());
                        editShowPossibleParticipantsByDB();
                        editInitializeTeams();

                        cmbEditTGame.Items.Clear();
                        cmbEditTGame.Items.Add("Select game type");

                        String[] gameType = KarateGeek.Strings.syncKata.Split('|');
                        cmbEditTGame.Items.Add(gameType[1]);
                        gameType = KarateGeek.Strings.enbu.Split('|');
                        cmbEditTGame.Items.Add(gameType[1]);

                        cmbEditTGame.SelectedIndex = 0;
                        editTeamGrid.Visibility = System.Windows.Visibility.Visible;
                        editIndividualGrid.Visibility = System.Windows.Visibility.Hidden;
                        break;
                    case "Cancel":
                        switch (_editTournamentCatType)
                        {
                            case KarateGeek.Strings.individual:
                                EditTrdButtonIndiv.Checked -= new RoutedEventHandler(EditTrdButtonIndiv_Checked);
                                EditTrdButtonIndiv.IsChecked = true;
                                EditTrdButtonIndiv.Checked += new RoutedEventHandler(EditTrdButtonIndiv_Checked);
                                break;
                            case KarateGeek.Strings.team:
                                EditTrdButtonTeam.Checked -= new RoutedEventHandler(EditTrdButtonTeam_Checked);
                                EditTrdButtonTeam.IsChecked = true;
                                EditTrdButtonTeam.Checked += new RoutedEventHandler(EditTrdButtonTeam_Checked);
                                break;
                        }
                        break;
                }
            }
            else
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
        }

        private void cmbEditTGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTGame.SelectedIndex;

            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        if (index != 0)
                        {
                            if (index < cmbEditTGame.Items.Count && index != -1)
                            {
                                _editTournamentGameType = _editTournamentCatType + "|" + cmbEditTGame.Items[index].ToString();
                                //ginetai i diagrafi twn athlitwn kai stin sinexeia allazei to cat type
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
                                    editInitializeTeams();
                                    editShowSelectedParticipantsT(0);
                                }
                                //apo edw katw allazei to cat type
                                if (_editTournamentGameType.Equals(Strings.indKata))
                                {
                                    cmbEditTJudging.Items.Clear();
                                    cmbEditTJudging.Items.Add("Select game type");
                                    cmbEditTJudging.Items.Add(Strings.score);
                                    cmbEditTJudging.Items.Add(Strings.flag);
                                }
                                else if (_editTournamentGameType.Equals(Strings.teamKata))
                                {
                                    cmbEditTJudging.Items.Clear();
                                    cmbEditTJudging.Items.Add(Strings.score);
                                }
                                else if (_editTournamentGameType.Equals(Strings.syncKata))
                                {
                                    cmbEditTJudging.Items.Clear();
                                    cmbEditTJudging.Items.Add(Strings.score);
                                }
                                else if (_editTournamentGameType.Equals(Strings.indKumite))
                                {
                                    cmbEditTJudging.Items.Clear();
                                    cmbEditTJudging.Items.Add(Strings.point);
                                }
                                else if (_editTournamentGameType.Equals(Strings.teamKumite))
                                {
                                    cmbEditTJudging.Items.Clear();
                                    cmbEditTJudging.Items.Add(Strings.point);
                                }
                                else if (_editTournamentGameType.Equals(Strings.enbu))
                                {
                                    cmbEditTJudging.Items.Clear();
                                    cmbEditTJudging.Items.Add(Strings.score);
                                }
                                else if (_editTournamentGameType.Equals(Strings.fugugo))
                                {
                                    cmbEditTJudging.Items.Clear();
                                    cmbEditTJudging.Items.Add(Strings.point + "/" + Strings.flag);
                                }
                                cmbEditTJudging.SelectedIndex = 0;
                            }
                            else
                            {
                                cmbEditTJudging.Items.Clear();
                                cmbEditTJudging.Items.Add("Select scoring type");
                                cmbEditTJudging.SelectedIndex = 0;
                            }

                        }
                        break;
                    case "Cancel":
                        int previousIndex = 0;
                        string[] gameType = _editTournamentGameType.Split('|');

                        for (int i = 1; i < cmbEditTGame.Items.Count; i++)
                        {
                            if (gameType[1].Equals(cmbEditTGame.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        cmbEditTGame.SelectionChanged -= new SelectionChangedEventHandler(cmbEditTGame_SelectionChanged);
                        cmbEditTGame.SelectedIndex = previousIndex;
                        cmbEditTGame.SelectionChanged += new SelectionChangedEventHandler(cmbEditTGame_SelectionChanged);
                        break;
                }
            }
            else
            {
                if (index != 0)
                {
                    if (index < cmbEditTGame.Items.Count && index != -1)
                    {
                        _editTournamentGameType = _editTournamentCatType + "|" + cmbEditTGame.Items[index].ToString();

                        //apo edw katw allazei to cat type
                        if (_editTournamentGameType.Equals(Strings.indKata))
                        {
                            cmbEditTJudging.Items.Clear();
                            cmbEditTJudging.Items.Add("Select game type");
                            cmbEditTJudging.Items.Add(Strings.score);
                            cmbEditTJudging.Items.Add(Strings.flag);
                        }
                        else if (_editTournamentGameType.Equals(Strings.teamKata))
                        {
                            cmbEditTJudging.Items.Clear();
                            cmbEditTJudging.Items.Add(Strings.score);
                        }
                        else if (_editTournamentGameType.Equals(Strings.syncKata))
                        {
                            cmbEditTJudging.Items.Clear();
                            cmbEditTJudging.Items.Add(Strings.score);
                        }
                        else if (_editTournamentGameType.Equals(Strings.indKumite))
                        {
                            cmbEditTJudging.Items.Clear();
                            cmbEditTJudging.Items.Add(Strings.point);
                        }
                        else if (_editTournamentGameType.Equals(Strings.teamKumite))
                        {
                            cmbEditTJudging.Items.Clear();
                            cmbEditTJudging.Items.Add(Strings.point);
                        }
                        else if (_editTournamentGameType.Equals(Strings.enbu))
                        {
                            cmbEditTJudging.Items.Clear();
                            cmbEditTJudging.Items.Add(Strings.score);
                        }
                        else if (_editTournamentGameType.Equals(Strings.fugugo))
                        {
                            cmbEditTJudging.Items.Clear();
                            cmbEditTJudging.Items.Add(Strings.point + "/" + Strings.flag);
                        }
                        cmbEditTJudging.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbEditTJudging.Items.Clear();
                        cmbEditTJudging.Items.Add("Select scoring type");
                        cmbEditTJudging.SelectedIndex = 0;
                    }
                }

            }
        }

        private void cmbEditTJudging_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbEditTJudging.SelectedIndex;
            if (editAreParticipantsForDeletion() && !editSuggestionChange)
            {
                switch (warningParticipantsDeletionMessage())
                {
                    case "OK":
                        //ginetai i diagrafi twn athlitwn kai stin sinexeia allazei to cat type
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
                            editInitializeTeams();
                            editShowSelectedParticipantsT(0);
                        }
                        if (index < cmbEditTJudging.Items.Count && index != -1 && !cmbEditTJudging.Items[index].ToString().Equals("Select game type"))
                            _editTournamentScoringType = cmbEditTJudging.Items[index].ToString();
                        break;
                    case "Cancel":
                        int previousIndex = 0;

                        for (int i = 1; i < cmbEditTJudging.Items.Count; i++)
                        {
                            if (_editTournamentScoringType.Equals(cmbEditTJudging.Items[i].ToString()))
                            {
                                previousIndex = i;
                                break;
                            }
                        }
                        cmbEditTJudging.SelectionChanged -= new SelectionChangedEventHandler(cmbEditTJudging_SelectionChanged);
                        cmbEditTJudging.SelectedIndex = previousIndex;
                        cmbEditTJudging.SelectionChanged += new SelectionChangedEventHandler(cmbEditTJudging_SelectionChanged);
                        break;
                }
            }
            else
            {
                if (index < cmbEditTJudging.Items.Count && index != -1 && !cmbEditTJudging.Items[index].ToString().Equals("Select game type"))
                    _editTournamentScoringType = cmbEditTJudging.Items[index].ToString();
            }
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

        #region other listeners

        private void tbEditTName_LostFocus(object sender, RoutedEventArgs e)
        {
            editTSuggestionList.Visibility = Visibility.Collapsed;
        }

        private void tbEditTName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            editTournamentList();
        }

        #endregion

        private void btnEditTBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.sender.Show();
        }

        private void btnEditTSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkNullOrEmptyFields("tournament", false) && checkWrongFields("tournament", false) && editCheckTeams())
            {
                tournamentConnection.UpdateTournament(_editTournamentId, _editTournamentName, _editTournamentSex, _editTournamentAgeFrom, _editTournamentAgeTo, _editTournamentLevelFrom, _editTournamentLevelTo, _editTournamentGameType, _editTournamentScoringType, _editTournamentEventId);
                editParticpantsDeletionDB();
                editParticipantsInsertionDB();
                MessageBox.Show("Succesfully saved!", "Tournament Edit", MessageBoxButton.OK);
                initializeEditTournament(true);
            }
        }

        private void btnEditTDelete_Click(object sender, RoutedEventArgs e)
        {
            editParticpantsDeletionDB();
            tournamentConnection.deleteTournament(_editTournamentId);

            MessageBox.Show("Succesfully deleted!", "Tournament Delete", MessageBoxButton.OK);
            initializeEditTournament(true);
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

        public void newShowPossibleParticipantsByDB()
        {
            newPossibleParticipants = this.newAthletesFilter();
            lbNewTparticipants.ItemsSource = null;

            if (newPossibleParticipants.Count > 0)
            {
                lbNewTparticipants.ItemsSource = newPossibleParticipants;
            }
            else
            {
                lbNewTparticipants.ItemsSource = null;
            }
        }

        public void editShowSelectedParticipantsByDB(int selectedTeam)
        {
            DataSet ds;

            if ((bool)EditTrdButtonIndiv.IsChecked)
            {
                editSelectedParticipants = new List<List<AthleteData>>();
                editSelectedParticipants.Add(new List<AthleteData>());
                tempEditSelectedParticipants = new List<List<AthleteData>>();
                tempEditSelectedParticipants.Add(new List<AthleteData>());

                ds = participantConnection.getParticipantsI(_editTournamentId);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AthleteData participant = new AthleteData();
                    participant.id = int.Parse(dr[0].ToString());
                    participant.athlete_name = dr[6].ToString() + " " + dr[7].ToString();
                    editSelectedParticipants.ElementAt(0).Add(participant);
                    tempEditSelectedParticipants.ElementAt(0).Add(participant);
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
                    tempEditSelectedParticipants.ElementAt(team).Add(participant);
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

        public void newShowPossibleParticipants()
        {
            lbNewTparticipants.ItemsSource = null;
            if (newPossibleParticipants.Count > 0)
            {
                lbNewTparticipants.ItemsSource = newPossibleParticipants;
            }
            else
            {
                lbNewTparticipants.ItemsSource = null;
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

        public void newShowSelectedParticipantsI()
        {
            lbNewTSelectedParticipants.ItemsSource = null;
            if (newSelectedParticipants.Count > 0)
            {
                lbNewTSelectedParticipants.ItemsSource = newSelectedParticipants.ElementAt(0);
            }
            else
            {
                lbNewTSelectedParticipants.ItemsSource = null;
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

        public void newShowSelectedParticipantsT(int selectedTeam)
        {
            lbNewTTeams.ItemsSource = null;
            if (newSelectedParticipants.ElementAt(selectedTeam).Count > 0)
            {
                lbNewTTeams.ItemsSource = newSelectedParticipants.ElementAt(selectedTeam);
            }
            else
            {
                lbNewTTeams.ItemsSource = null;
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

        public void newParticipantsInsertionDB()
        {
            DataSet ds;
            string rank = null;
            int i = 0;
            int j = 0;

            switch (_newTournamentCatType)
            {
                case (Strings.individual):
                    foreach (AthleteData participant in newSelectedParticipants.ElementAt(0))
                    {
                        ds = athleteConnection.findAthlete(participant.id);
                        rank = ds.Tables[0].Rows[0][1].ToString();
                        participantConnection.InsertNewParticipantI(participant.id, _newTournamentId, rank);
                    }
                    break;
                case (Strings.team):
                    foreach (List<AthleteData> list in newSelectedParticipants)
                    {
                        _newTournamentTeamId = participantConnection.InsertNewTeam(i, _newTournamentId);
                        foreach (AthleteData participant in list)
                        {
                            ds = athleteConnection.findAthlete(participant.id);
                            rank = ds.Tables[0].Rows[0][1].ToString();
                            participantConnection.InsertNewParticipantT(participant.id, _newTournamentId, rank, _newTournamentTeamId);
                        }
                        i++;
                    }
                    break;
                case (Strings.synchronized):
                    foreach (List<AthleteData> list in newSelectedParticipants)
                    {
                        _newTournamentTeamId = participantConnection.InsertNewTeam(i, _newTournamentId);
                        foreach (AthleteData participant in list)
                        {
                            ds = athleteConnection.findAthlete(participant.id);
                            rank = ds.Tables[0].Rows[0][1].ToString();
                            participantConnection.InsertNewParticipantT(participant.id, _newTournamentId, rank, _newTournamentTeamId);
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
                    foreach (AthleteData item in tempEditSelectedParticipants.ElementAt(0))
                    {
                        participantConnection.deleteParticipantI(item.id, _editTournamentId);
                    }
                    break;
                case (Strings.team):
                    foreach (List<AthleteData> list in tempEditSelectedParticipants)
                    {
                        foreach (AthleteData participant in list)
                        {
                            participantConnection.deleteParticipantI(participant.id, _editTournamentId);
                        }
                    }
                    break;
                case (Strings.synchronized):
                    foreach (List<AthleteData> list in tempEditSelectedParticipants)
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

        private List<AthleteData> newAthletesFilter()
        {
            DataSet filteredAthletes;
            List<AthleteData> list = new List<AthleteData>();

            filteredAthletes = participantConnection.findPotentialParticipants(_newTournamentSex, _newTournamentAgeFrom, _newTournamentAgeTo, _newLevelFrom - 1, _newLevelTo - 1, _newTournamentId);
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

        private void btnewAddParticipant_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)NewTrdButtonIndiv.IsChecked)
            {
                List<AthleteData> list = new List<AthleteData>();

                foreach (Object item in lbNewTparticipants.SelectedItems)
                {
                    newSelectedParticipants.ElementAt(0).Add((AthleteData)item);

                    newPossibleParticipants.RemoveAt(lbNewTparticipants.Items.IndexOf(item));
                }
                newShowSelectedParticipantsI();
                newShowPossibleParticipants();
            }
            else if ((bool)NewTrdButtonTeam.IsChecked || (bool)NewTrdButtonSync.IsChecked)
            {
                if (newTeamsNum >= 2)
                {
                    List<AthleteData> list = new List<AthleteData>();

                    foreach (Object item in lbNewTparticipants.SelectedItems)
                    {
                        newSelectedParticipants.ElementAt(_newTournamentTeam).Add((AthleteData)item);

                        newPossibleParticipants.RemoveAt(lbNewTparticipants.Items.IndexOf(item));
                    }
                    newShowSelectedParticipantsT(_newTournamentTeam);
                    newShowPossibleParticipants();
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

        private void btnewDeleteParticipant_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)NewTrdButtonIndiv.IsChecked)
            {
                AthleteData participant = new AthleteData();

                foreach (Object item in lbNewTSelectedParticipants.SelectedItems)
                {
                    participant = (AthleteData)item;
                    newSelectedParticipants.ElementAt(0).RemoveAt(lbNewTSelectedParticipants.Items.IndexOf(item));
                    newPossibleParticipants.Add(participant);

                }
                newShowPossibleParticipants();
                newShowSelectedParticipantsI();
            }
            else if ((bool)NewTrdButtonTeam.IsChecked || (bool)NewTrdButtonSync.IsChecked)
            {
                AthleteData participant = new AthleteData();

                foreach (Object item in lbNewTTeams.SelectedItems)
                {
                    participant = (AthleteData)item;
                    newSelectedParticipants.ElementAt(_newTournamentTeam).RemoveAt(lbNewTTeams.Items.IndexOf(item));
                    newPossibleParticipants.Add(participant);

                }
                newShowPossibleParticipants();
                newShowSelectedParticipantsT(_newTournamentTeam);
            }

        }

        #endregion

        #region helping methods

        #region participants methods

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

        private bool newAreParticipantsForDeletion()
        {
            bool flag = false;
            foreach (List<AthleteData> list in newSelectedParticipants)
            {
                if (list.Count > 0)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private string warningParticipantsDeletionMessage()
        {
            return MessageBox.Show("Changes will delete selected participants. Press OK to continue.", "Message!",
               MessageBoxButton.OKCancel,
               MessageBoxImage.Information).ToString();
        }

        private string warningEventNewTournamentDeletion()
        {
            return MessageBox.Show("You are adding new tournament to this event! Deletion will erase these changes! Procced Anyway?", "Event Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning).ToString();
        }

        private string warningEventEditTournamentDeletion()
        {
            return MessageBox.Show("You are editing a tournament in this event! Deletion will erase these changes! Procced Anyway?", "Event Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning).ToString();
        }

        private string warningNewEditTournamentDeletion()
        {
            return MessageBox.Show("You are both adding and editing a tournament in this event! Deletion will erase these changes! Procced Anyway?", "Event Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning).ToString();
        }
        #endregion

        #region checks methods

        private bool checkNullOrEmptyFields(string type, bool newMode)
        {
            if (type.Equals("event"))
            {
                if (newMode)
                {
                    if (string.IsNullOrEmpty(_newEventName))
                    {
                        em.nullErrorMessage("Event Name");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(_editEventName))
                    {
                        em.errorMessage("Event Name");
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
                    if (cmbNewTEventChooser.SelectedIndex == 0)
                    {
                        em.errorMessage("Event");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_newTournamentName))
                    {
                        em.errorMessage("Tournament Name");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_newTournamentSex))
                    {
                        em.errorMessage("Sex");
                        return false;
                    }
                    /*else if (cmbNewTAgeFrom.SelectedIndex == 0)
                    {
                        em.errorMessage("Age From");
                        return false;
                    }
                    else if (cmbNewTAgeTo.SelectedIndex == 0)
                    {
                        em.errorMessage("Age To");
                        return false;
                    }
                    else if (cmbNewTLevelFrom.SelectedIndex == 0)
                    {
                        em.errorMessage("LevelFrom");
                        return false;
                    }
                    else if (cmbNewTLevelTo.SelectedIndex == 0)
                    {
                        em.errorMessage("Level To");
                        return false;
                    }*/
                    else if (string.IsNullOrEmpty(_newTournamentCatType))
                    {
                        em.errorMessage("Game Type");
                        return false;
                    }
                    else if (cmbNewTGame.SelectedIndex == 0)
                    {
                        em.errorMessage("Game");
                        return false;
                    }
                    else if ((cmbNewTJudging.SelectedIndex == 0) && (cmbNewTJudging.Items.Count > 1))
                    {
                        em.errorMessage("Scoring type");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (cmbEditTEventChooser.SelectedIndex == 0)
                    {
                        em.errorMessage("Event");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_editTournamentName))
                    {
                        em.errorMessage("Tournament Name");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(_editTournamentSex))
                    {
                        em.errorMessage("Sex");
                        return false;
                    }
                    /*else if (cmbEditTAgeFrom.SelectedIndex == 0)
                    {
                        em.errorMessage("Age From");
                        return false;
                    }
                    else if (cmbEditTAgeTo.SelectedIndex == 0)
                    {
                        em.errorMessage("Age To");
                        return false;
                    }
                    else if (cmbEditTLevelFrom.SelectedIndex == 0)
                    {
                        em.errorMessage("LevelFrom");
                        return false;
                    }
                    else if (cmbEditTLevelTo.SelectedIndex == 0)
                    {
                        em.errorMessage("Level To");
                        return false;
                    }*/
                    else if (string.IsNullOrEmpty(_editTournamentCatType))
                    {
                        em.errorMessage("Game Type");
                        return false;
                    }
                    else if (cmbEditTGame.SelectedIndex == 0)
                    {
                        em.errorMessage("Game");
                        return false;
                    }
                    else if ((cmbEditTJudging.SelectedIndex == 0) && (cmbEditTJudging.Items.Count > 1))
                    {
                        em.errorMessage("Scoring type");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        private bool checkWrongFields(string type, bool newMode)
        {
            if (type.Equals("event"))
            {
                if (newMode)
                {   /*
                    if (!string.IsNullOrEmpty(_newEventLocation) && !regex.isCharsOrSpace(_newEventLocation))
                    {
                        em.charsSpaceErrorMessage("Location");
                        return false;
                    }*/
                    if (!string.IsNullOrEmpty(_newEventPhone) && !regex.isDigitsOnly(_newEventPhone))
                    {
                        em.digitsErrorMessage("Phone Num.");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newEventEmail) && !regex.isEmailValid(_newEventEmail))
                    {
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newEventAddress) && !regex.isCharsSpaceOrDots(_newEventAddress))
                    {
                        em.charsSpaceDotsErrorMessage("Address");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newEventAddressNum) && !regex.isCharsOrDigits(_newEventAddressNum))
                    {
                        em.charsDigitsErrorMessage("Address Num.");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_newEventPCode) && !regex.isDigitsOnly(_newEventPCode))
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
                    /*
                    if (!string.IsNullOrEmpty(_editEventLocation) && !regex.isCharsOrSpace(_editEventLocation))
                    {
                        em.charsSpaceErrorMessage("Location");
                        return false;
                    }*/
                    if (!string.IsNullOrEmpty(_editEventPhone) && !regex.isDigitsOnly(_editEventPhone))
                    {
                        em.digitsErrorMessage("Phone Num.");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_editEventEmail) && !regex.isEmailValid(_editEventEmail))
                    {
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_editEventAddress) && !regex.isCharsSpaceOrDots(_editEventAddress))
                    {
                        em.charsSpaceDotsErrorMessage("Address");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_editEventAddressNum) && !regex.isCharsOrDigits(_editEventAddressNum))
                    {
                        em.charsDigitsErrorMessage("Address Num.");
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(_editEventPCode) && !regex.isDigitsOnly(_editEventPCode))
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
                    if (!regex.isSqlSpecialChar(_newTournamentName))
                    {
                        em.sqlSpecialChar("Name");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (!regex.isSqlSpecialChar(_editTournamentName))
                    {
                        em.sqlSpecialChar("Name");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        private bool newCheckTeams()
        {
            int i = 0;

            switch (_newTournamentCatType)
            {
                case Strings.team:
                    switch (_newTournamentGameType)
                    {
                        case Strings.teamKata:
                            foreach (List<AthleteData> list in newSelectedParticipants)
                            {
                                if (list.Count < 3 || list.Count > 4)
                                {
                                    errorTeamMessage(i, 2);
                                    return false;
                                }
                                i++;
                            }
                            break;
                        case Strings.teamKumite:
                            foreach (List<AthleteData> list in newSelectedParticipants)
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
                    switch (_newTournamentGameType)
                    {
                        case Strings.enbu:
                            foreach (List<AthleteData> list in newSelectedParticipants)
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
                            foreach (List<AthleteData> list in newSelectedParticipants)
                            {
                                if (list.Count != 3)
                                {
                                    errorTeamMessage(i, 1);
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
                                if (list.Count < 3 || list.Count > 4)
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
                                    errorTeamMessage(i, 1);
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

            }

            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
            cmbEditTteamSelection.SelectedIndex = team;
        }

        private bool setNewEventEnableSaveEnable()
        {
            if (string.IsNullOrEmpty(_newEventName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool setEditEventSaveEnable()
        {
            if (string.IsNullOrEmpty(_editEventName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region initialize methods

        private void initializeNewEvent()
        {
            newEventName.Text = null;
            newEventDate.SelectedDate = null;
            newEventLocation.Text = null;
            newEventPhone.Text = null;
            newEventEmail.Text = null;
            newEventAddress.Text = null;
            newEventAddressNum.Text = null;
            newEventPCode.Text = null;
            cmbNewECountryChooses.SelectedIndex = countryIndex;
            newEventOfficial.IsChecked = true;
            Dispatcher.BeginInvoke(new Action(() => { newEventName.Focus(); }));
        }

        private void initializeEditEvent()
        {
            _editEventId = -1;
            _editEventSelected = false;
            editEventName.Text = null;
            editEventDate.SelectedDate = null;
            editEventLocation.Text = null;
            editEventPhone.Text = null;
            editEventEmail.Text = null;
            editEventAddress.Text = null;
            editEventAddressNum.Text = null;
            editEventPCode.Text = null;
            cmbEditECountryChooses.SelectedIndex = countryIndex;
            editEventOfficial.IsChecked = true;
            Dispatcher.BeginInvoke(new Action(() => { editEventName.Focus(); }));
        }

        private void initializeNewTournament(Boolean clearAll)
        {

            _newTournamentId = 0;
            _newTournamentTeamId = 0;
            newSelectedParticipants = new List<List<AthleteData>>();
            newSelectedParticipants.Add(new List<AthleteData>());
            if (clearAll)
                newEventInfo.Text = null;
            //events
            cmbNewTEventChooser.Items.Clear();
            this.events = eventConnection.getEvents();
            cmbNewTEventChooser.Items.Add("Select Event");
            foreach (DataRow dr in events.Tables[0].Rows)
            {
                cmbNewTEventChooser.Items.Add(dr[1].ToString());
            }
            cmbNewTEventChooser.SelectedIndex = 0;
            tbNewTName.Text = null;
            NewTrdButtonMale.IsChecked = false;
            NewTrdButtonFemale.IsChecked = false;
            cmbNewTAgeFrom.SelectedIndex = 0;
            cmbNewTAgeTo.SelectedIndex = 0;
            cmbNewTLevelFrom.SelectedIndex = 0;
            cmbNewTLevelTo.SelectedIndex = 0;
            NewTrdButtonIndiv.IsChecked = false;
            NewTrdButtonTeam.IsChecked = false;
            NewTrdButtonSync.IsChecked = false;
            cmbNewTGame.Items.Clear();
            cmbNewTGame.Items.Add("Select game type");
            cmbNewTGame.SelectedIndex = 0;
            cmbNewTJudging.Items.Clear();
            cmbNewTJudging.Items.Add("Select scoring type");
            cmbNewTJudging.SelectedIndex = 0;
            cmbNewTteamsNumber.SelectedIndex = 0;
            cmbNewTteamSelection.Items.Clear();
            newIndividualGrid.Visibility = System.Windows.Visibility.Visible;
            newTeamGrid.Visibility = System.Windows.Visibility.Hidden;
            lbNewTTeams.ItemsSource = null;
            lbNewTparticipants.ItemsSource = null;
            newShowSelectedParticipantsI();
            Dispatcher.BeginInvoke(new Action(() => { tbNewTName.Focus(); }));


        }

        private void initializeEditTournament(Boolean clearAll)
        {

            _editTournamentId = 0;
            _editTournamentTeamId = 0;
            editSelectedParticipants = new List<List<AthleteData>>();
            editSelectedParticipants.Add(new List<AthleteData>());
            _editTournamentSelected = false;
            if (clearAll)
                editEventInfo.Text = null;
            //events
            cmbEditTEventChooser.Items.Clear();
            this.events = eventConnection.getEvents();
            cmbEditTEventChooser.Items.Add("Select Event");
            foreach (DataRow dr in events.Tables[0].Rows)
            {
                cmbEditTEventChooser.Items.Add(dr[1].ToString());
            }
            cmbEditTEventChooser.SelectedIndex = 0;
            tbEditTName.Text = null;
            EditTrdButtonMale.IsChecked = false;
            EditTrdButtonFemale.IsChecked = false;
            cmbEditTAgeFrom.SelectedIndex = 0;
            cmbEditTAgeTo.SelectedIndex = 0;
            cmbEditTLevelFrom.SelectedIndex = 0;
            cmbEditTLevelTo.SelectedIndex = 0;
            EditTrdButtonIndiv.IsChecked = false;
            EditTrdButtonTeam.IsChecked = false;
            EditTrdButtonSync.IsChecked = false;
            cmbEditTGame.Items.Clear();
            cmbEditTGame.Items.Add("Select game type");
            cmbEditTGame.SelectedIndex = 0;
            cmbEditTJudging.Items.Clear();
            cmbEditTJudging.Items.Add("Select scoring type");
            cmbEditTJudging.SelectedIndex = 0;
            cmbEditTteamsNumber.SelectedIndex = 0;
            cmbEditTteamSelection.Items.Clear();
            editIndividualGrid.Visibility = System.Windows.Visibility.Visible;
            editTeamGrid.Visibility = System.Windows.Visibility.Hidden;
            lbEditTTeams.ItemsSource = null;
            lbEditTparticipants.ItemsSource = null;
            editShowSelectedParticipantsI();
            Dispatcher.BeginInvoke(new Action(() => { tbEditTName.Focus(); }));
        }

        private void editInitializeTeams()
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

        private void newInitializeTeams()
        {
            cmbNewTteamsNumber.Items.Clear();
            cmbNewTteamsNumber.Items.Add("Teams");
            cmbNewTteamsNumber.Items.Add("2");
            cmbNewTteamsNumber.Items.Add("3");
            cmbNewTteamsNumber.Items.Add("4");
            cmbNewTteamsNumber.Items.Add("5");
            cmbNewTteamsNumber.Items.Add("6");
            cmbNewTteamsNumber.Items.Add("7");
            cmbNewTteamsNumber.Items.Add("8");
            cmbNewTteamsNumber.Items.Add("9");
            cmbNewTteamsNumber.Items.Add("10");


            cmbNewTteamSelection.Items.Clear();

            cmbNewTteamsNumber.SelectedIndex = 0;


        }


        #endregion

        #region event methods
        private void editEventList()
        {
            _editEventName = editEventName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            if (setEditEventSaveEnable())
            {
                btnEditESave.IsEnabled = true;
            }
            else
            {
                btnEditESave.IsEnabled = false;
            }

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

        #endregion

        #region tournament methods

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

        private void Window_closed(object sender, EventArgs e)
        {
            MainMenu menu = (MainMenu)this.sender;
            menu.reloadCalendarEvents();
            this.Close();
            this.sender.Show();

        }





        #endregion

    }
}
