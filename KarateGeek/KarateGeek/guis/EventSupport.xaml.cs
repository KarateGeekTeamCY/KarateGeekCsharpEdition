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
    /// Interaction logic for EventSupport.xaml
    /// </summary>
    public partial class EventSupport : Window
    {


        #region private declaretions

        private Window _sender;


        private string _eventId;
        private string _tournamentId;

        private int _indexlastExecutedGame;
        private int _indexCurrentphase;
        private int _indexNextPhase;

        private bool _firstLoad = true;

        private Tournament tournament;

        private TournamentConnection _tournamentConn = new TournamentConnection();
        private EventConnection _eventConn = new EventConnection();



        private DataTable _TournamantsDT;

        private List<string> _availableTournaments = new List<string>();
        private List<string> _currentGames = new List<string>();
        private List<string> _futureGames = new List<string>();




        #endregion private declaretions


        public EventSupport(Window sender)
        {

            InitializeComponent();
            this._sender = sender;


            DateTime date = DateTime.Today;
            string datestring = date.ToString("yyyy-M-d");

            _eventConn = new EventConnection();

            DataSet todayEventDS = _eventConn.getEventsBydate(datestring);
            DataTable todayEventDT = todayEventDS.Tables[0];

            if (todayEventDT.Rows.Count == 0)
            {
                string result = MessageBox.Show("Unfortunately there is no scheduled event for today. Press OK to return to main menu.", "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();

                //this._sender.Show();
                this.Close();
                return;
            }
            else
            {
                this.Show();
                this._sender.Hide();

                this._eventId = todayEventDT.Rows[0][0].ToString();
            }

            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            string sql = "select * from tournaments where event_id = '" + this._eventId + "'; ";
            _TournamantsDT = conn.Query(sql).Tables[0];

            foreach (DataRow dr in _TournamantsDT.Rows)
            {
                this._availableTournaments.Add("" + dr[1]);
            }

            this.cboTurnamentSelector.ItemsSource = _availableTournaments;
        }

        private void cboTurnamentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = this.cboTurnamentSelector.SelectedIndex;
            this._tournamentId = (string)_TournamantsDT.Rows[i][0];

            this.tournament = new Tournament(this._tournamentId);

            loadGames();
            //todo finf the first phase check the length 
        }


        #region buttons

        private void btnStartNextGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion


        #region game loading functions

        private void loadGames()
        {
            if (this.tournament.isInd)
                loadIndevidual();

            if (this.tournament.isTeam)
                loadTeam();

            if (this.tournament.isSync)
                loadSync();

            //if (this.tournament.games128.Count != 0)
            //{
            //}
            //else if (this.tournament.games64.Count != 0)
            //{
            //}
            //else if (this.tournament.games32.Count != 0)
            //{
            //}
            //else if (this.tournament.games16.Count != 0)
            //{
            //}
            //else if (this.tournament.games8.Count != 0)
            //{
            //}
            //else if (this.tournament.games4.Count != 0)
            //{
            //}
            //else
            //{
            //}
        }

        private void loadIndevidual()
        {

            switch (this.tournament.gameType)
            {
                case Strings.indKata:
                    if (this.tournament.judgingType == Strings.flag)
                    {

                        if (this._firstLoad)
                            foreach (Athlete ath in tournament.participants)
                                ath.ranking = "128";


                        this.loadIndevidualVersus();
                    }
                    else
                    {

                        if (this._firstLoad)
                            foreach (Athlete ath in tournament.participants)
                                ath.ranking = "128";


                        this.loadIndevidualSingle();
                    }
                    break;
                case Strings.indKumite:

                    if (this._firstLoad)
                        foreach (Athlete ath in tournament.participants)
                            ath.ranking = "128";


                    this.loadIndevidualVersus();
                    break;
                case Strings.fugugo:

                    if (this._firstLoad)
                        foreach (Athlete ath in tournament.participants)
                            ath.ranking = "128";


                    this.loadIndevidualVersus();
                    break;
            }
        }

        private void loadIndevidualVersus()
        {
            if (this.tournament.games128.Count != 0 && (!this.tournament.games128.Last().isFinished))
            {
                this._indexCurrentphase = 7;
                this._indexNextPhase = 6;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games128)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;


            }
            else if (this.tournament.games64.Count != 0 && (!this.tournament.games64.Last().isFinished))
            {
                this._indexCurrentphase = 6;
                this._indexNextPhase = 5;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games32.Count != 0 && (!this.tournament.games32.Last().isFinished))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games16.Count != 0 && (!this.tournament.games16.Last().isFinished))
            {
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games8.Count != 0 && (!this.tournament.games8.Last().isFinished))
            {
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games4.Count != 0 && (!this.tournament.games4.Last().isFinished))
            {
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games2)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games2.Count != 0 && (!this.tournament.games2.Last().isFinished))
            {
                this._indexCurrentphase = 1;
                this._indexNextPhase = -1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games2)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase there no next phase you are in finale
                //
                List<string> future = new List<string>();
                this.listBoxNextGameList.ItemsSource = future;
            }
            else
            {
                //tournament is finished anounce the winner
            }


        }

        private void loadIndevidualSingle()
        {

            if (this.tournament.games128.Count != 0 && (!this.tournament.games128.Last().isFinished))
            {
                this._indexCurrentphase = 7;
                this._indexNextPhase = 6;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games128)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;


            }
            else if (this.tournament.games64.Count != 0 && (!this.tournament.games64.Last().isFinished))
            {
                this._indexCurrentphase = 6;
                this._indexNextPhase = 5;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games32.Count != 0 && (!this.tournament.games32.Last().isFinished))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games16.Count != 0 && (!this.tournament.games16.Last().isFinished))
            {
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games8.Count != 0 && (!this.tournament.games8.Last().isFinished))
            {
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games4.Count != 0 && (!this.tournament.games4.Last().isFinished))
            {
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    //Athlete B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                // next Phase? no next phase
                //
                List<string> future = new List<string>();
                this.listBoxNextGameList.ItemsSource = future;
            }

            else
            {
                //tournament is finished anounce the winner
            }
        }


        private void loadTeam()
        {
            switch (this.tournament.gameType)
            {
                case Strings.teamKata:

                    if (this._firstLoad)
                        foreach (Athlete ath in tournament.participants)
                            ath.ranking = "128";


                    loadIndevidualSingle();

                    break;
                case Strings.teamKumite:

                    if (this._firstLoad)
                        foreach (Athlete ath in tournament.participants)
                            ath.ranking = "128";

                    loadTamVersus();

                    break;
            }
        }

        private void loadTamVersus()
        {
            if (this.tournament.games128.Count != 0 && (!this.tournament.games128.Last().isFinished))
            {
                this._indexCurrentphase = 7;
                this._indexNextPhase = 6;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games128)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;


            }
            else if (this.tournament.games64.Count != 0 && (!this.tournament.games64.Last().isFinished))
            {
                this._indexCurrentphase = 6;
                this._indexNextPhase = 5;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games32.Count != 0 && (!this.tournament.games32.Last().isFinished))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games16.Count != 0 && (!this.tournament.games16.Last().isFinished))
            {
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games8.Count != 0 && (!this.tournament.games8.Last().isFinished))
            {
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games4.Count != 0 && (!this.tournament.games4.Last().isFinished))
            {
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games2)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games2.Count != 0 && (!this.tournament.games2.Last().isFinished))
            {
                this._indexCurrentphase = 1;
                this._indexNextPhase = -1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games2)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        current.Add("Click to add game members.");
                    }
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase there no next phase you are in finale
                //
                List<string> future = new List<string>();
                this.listBoxNextGameList.ItemsSource = future;
            }
            else
            {
                //tournament is finished anounce the winner
            }


        }


        private void loadSync()
        {
            switch (this.tournament.gameType)
            {
                case Strings.syncKata:

                    if (this._firstLoad)
                        foreach (Athlete ath in tournament.participants)
                            ath.ranking = "128";


                    loadKataSyncSingle();


                    break;
                case Strings.enbu:

                    if (this._firstLoad)
                        foreach (Athlete ath in tournament.participants)
                            ath.ranking = "128";


                    loadEmbuSyncSingle();

                    break;
            }
        }

        private void loadKataSyncSingle()
        {

            if (this.tournament.games128.Count != 0 && (!this.tournament.games128.Last().isFinished))
            {
                this._indexCurrentphase = 7;
                this._indexNextPhase = 6;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games128)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;


            }
            else if (this.tournament.games64.Count != 0 && (!this.tournament.games64.Last().isFinished))
            {
                this._indexCurrentphase = 6;
                this._indexNextPhase = 5;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games32.Count != 0 && (!this.tournament.games32.Last().isFinished))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games16.Count != 0 && (!this.tournament.games16.Last().isFinished))
            {
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games8.Count != 0 && (!this.tournament.games8.Last().isFinished))
            {
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games4.Count != 0 && (!this.tournament.games4.Last().isFinished))
            {
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName + " - " + C.lastName + " " + C.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                // next Phase? no next phase
                //
                List<string> future = new List<string>();
                this.listBoxNextGameList.ItemsSource = future;
            }

            else
            {
                //tournament is finished anounce the winner
            }
        }

        private void loadEmbuSyncSingle()
        {

            if (this.tournament.games128.Count != 0 && (!this.tournament.games128.Last().isFinished))
            {
                this._indexCurrentphase = 7;
                this._indexNextPhase = 6;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games128)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;


            }
            else if (this.tournament.games64.Count != 0 && (!this.tournament.games64.Last().isFinished))
            {
                this._indexCurrentphase = 6;
                this._indexNextPhase = 5;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games32.Count != 0 && (!this.tournament.games32.Last().isFinished))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games16.Count != 0 && (!this.tournament.games16.Last().isFinished))
            {
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games8.Count != 0 && (!this.tournament.games8.Last().isFinished))
            {
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games4.Count != 0 && (!this.tournament.games4.Last().isFinished))
            {
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    Athlete A = gm.participants.ElementAt(0);
                    Athlete B = gm.participants.ElementAt(1);
                    //Athlete C = gm.participants.ElementAt(2);
                    current.Add(A.lastName + " " + A.firstName + " - " + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                // next Phase? no next phase
                //
                List<string> future = new List<string>();
                this.listBoxNextGameList.ItemsSource = future;
            }

            else
            {
                //tournament is finished anounce the winner
            }
        }


        #endregion game loading functions


        #region winner checks
        //
        // INDIVIDUAL
        //
        private List<Athlete> getKataIndSinglePositioning()
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "select game_participations.athlete_id, mean_score from tournament_participations join game_participacions on tournament_participations.athlete_id = game_participations.athlete_id join game_score on game_participation.athlete_id = game_score.athlete_id where tournament_id = '" + this.tournament.id + "' AND phase '" + this._indexCurrentphase +"' ORDER BY mean_score DESC ;";
            DataTable temp = conn.Query(sql).Tables[0];

            List<Athlete> aths = new List<Athlete>();

            foreach (DataRow dr in temp.Rows)
                aths.Add(new Athlete((string)dr[0], this.tournament.id));

            return aths;
        }

        private Athlete getKataIndVersusWinner(string gameId)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "select * from game_participacions join game_flag on game_participacions.game_id = game_flag.game_id where game_id = '" + gameId + "' ;";
            DataTable temp = conn.Query(sql).Tables[0];

            return new Athlete((string)temp.Rows[0][0], this.tournament.id);
        }

        private Athlete getKumiteIndVersusWinner(string gameId)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "select athlete_id, sum(technical_point) from game_participacions join game_points on game_participacions.game_id = game_flag.game_id where game_id = '" + gameId + "' group by (game_participations.athlete_id) ;";
            DataTable temp = conn.Query(sql).Tables[0];

            if ((int)temp.Rows[0][1] > (int)temp.Rows[1][1])
                return new Athlete((string)temp.Rows[0][0], this.tournament.id);
            else
                return new Athlete((string)temp.Rows[1][0], this.tournament.id);
        }


        //
        // TEAM
        //
        private List<team> getKataTeamIndPositioning()
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "select game_participations.team_id, sum(mean_score) from team_tournament_participations join game_participations on team_tournament_participations.id = game_participations.team_id join game_score on team_tournament_participations.id = game_score.team_id where tournament_id = '" + this.tournament.id + "' group by game_participations.team_id ORDER BY sum(mean_score) DESC ;";
            DataTable temp = conn.Query(sql).Tables[0];

            List<team> teams = new List<team>();

            foreach (DataRow dr in temp.Rows)
                teams.Add(new team((string)dr[0]));

            return teams;
        }

        private team getKumiteTeamWinner(string gameId)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "select team_id, sum(technical_point) from game_participacions join game_points on game_participacions.game_id = game_flag.game_id where game_id = '" + gameId + "' group by (game_participations.team_id) ;";
            DataTable temp = conn.Query(sql).Tables[0];


            if ((int)temp.Rows[0][1] > (int)temp.Rows[1][1])
                return new team((string)temp.Rows[0][0]);
            else
                return new team((string)temp.Rows[1][0]);
        }

        //
        // SYNCRONIZED
        //
        private List<team> getSyncKataWinner(string gameId)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "select game_participations.team_id, mean_score from team_tournament_participations join game_participacions on team_tournament_participations.id = game_participations.team_id join game_score on team_tournament_participation.id = game_score.team_id where tournament_id = '" + this.tournament.id + "' ORDER BY mean_score DESC ;";
            DataTable temp = conn.Query(sql).Tables[0];

            List<team> teams = new List<team>();

            foreach (DataRow dr in temp.Rows)
                teams.Add(new team((string)dr[0]));

            return teams;
        }

        private List<team> getEmbuWinner(string gameId)
        {
            return this.getSyncKataWinner(gameId);
        }
        #endregion winner checks


        //
        // game starting functionality
        //
        private void listBoxCurrentGameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Game gm = null;
            this._indexlastExecutedGame = this.listBoxCurrentGameList.SelectedIndex;


            switch (this._indexCurrentphase)
            {
                case 1:
                    gm = tournament.games2.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 2:
                    gm = tournament.games4.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 3:
                    gm = tournament.games8.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 4:
                    gm = tournament.games16.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 5:
                    gm = tournament.games32.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 6:
                    gm = tournament.games64.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 7:
                    gm = tournament.games128.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
            }

            KataSystem kataSys;
            KumiteSystem kumiteSys;
            FlagSystem flagSys;


            switch (tournament.gameType)
            {
                case Strings.indKata:

                    switch (tournament.judgingType)
                    {
                        case Strings.flag:
                            flagSys = new FlagSystem(this, gm);
                            break;
                        case Strings.score:
                            kataSys = new KataSystem(this, gm);
                            break;
                    }

                    break;
                case Strings.indKumite:

                    kumiteSys = new KumiteSystem(this, gm);

                    break;
                case Strings.fugugo:

                    if (this._indexCurrentphase % 2 == 1)
                    {
                        kumiteSys = new KumiteSystem(this, gm);
                    }
                    else
                    {
                        kataSys = new KataSystem(this, gm);
                    }

                    break;
                case Strings.teamKata:
                    kataSys = new KataSystem(this, gm);
                    break;
                case Strings.teamKumite:

                    if (this._currentGames.ElementAt(this.listBoxCurrentGameList.SelectedIndex) == "Click to add game members.")
                    {
                        KumiteTeamMaker choser = new KumiteTeamMaker(gm, this);
                    }
                    else
                    {
                        kumiteSys = new KumiteSystem(this, gm);
                    }

                    break;
                case Strings.syncKata:

                    kataSys = new KataSystem(this, gm);

                    break;
                case Strings.enbu:

                    kataSys = new KataSystem(this, gm);
                    break;
            }
        }




        public void advanceAthlites()
        {
            Game gm = null;
            switch (this._indexCurrentphase)
            {
                case 1:
                    gm = tournament.games2.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 2:
                    gm = tournament.games4.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 3:
                    gm = tournament.games8.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 4:
                    gm = tournament.games16.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 5:
                    gm = tournament.games32.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 6:
                    gm = tournament.games64.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
                case 7:
                    gm = tournament.games128.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                    break;
            }
            switch (tournament.gameType)
            {
                case Strings.indKata:

                    switch (tournament.judgingType)
                    {
                        case Strings.flag:
                            //gm.gameId
                            Athlete winner = this.getKataIndVersusWinner(gm.gameId);
                            switch (this._indexCurrentphase)
                            {
                                case 1:
                                    tournament.games2.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 2:
                                    tournament.games4.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 3:
                                    tournament.games8.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 4:
                                    tournament.games16.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 5:
                                    tournament.games32.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 6:
                                    tournament.games64.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 7:
                                    tournament.games128.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                    break;
                            }


                            break;
                        case Strings.score:
                            DataTable dt = new CoreDatabaseConnection().Query("SELECT * FROM games where phase = '" + this._indexCurrentphase + "' and is_finished = false ;").Tables[0];
                            if (dt.Rows.Count == 0)
                            {
                                List<Athlete> winners = this.getKataIndSinglePositioning();
 
                                //switch (this._indexCurrentphase)
                                //{
                                //    case 1:
                                //        tournament.games2.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                //        break;
                                //    case 2:
                                //        tournament.games4.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                //        break;
                                //    case 3:
                                //        tournament.games8.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                //        break;
                                //    case 4:
                                //        tournament.games16.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                //        break;
                                //    case 5:
                                //        tournament.games32.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                //        break;
                                //    case 6:
                                //        tournament.games64.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                //        break;
                                //    case 7:
                                //        tournament.games128.ElementAt((int)Math.Ceiling(this._indexCurrentphase / 2.0)).AddParticipant(winner.id);
                                //        break;
                                //}

                            }
                            break;

                    }

                    break;
                case Strings.indKumite:



                    break;
                case Strings.fugugo:

                    if (this._indexCurrentphase % 2 == 1)
                    {


                    }
                    else
                    {



                    }

                    break;
                case Strings.teamKata:



                    break;
                case Strings.teamKumite:

                    if (this._currentGames.ElementAt(this.listBoxCurrentGameList.SelectedIndex) == "Click to add game members.")
                    {



                    }
                    else
                    {



                    }

                    break;
                case Strings.syncKata:




                    break;
                case Strings.enbu:





                    break;
            }




        }






























    }
}
