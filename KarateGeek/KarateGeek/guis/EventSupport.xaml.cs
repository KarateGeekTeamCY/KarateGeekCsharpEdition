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
        #region private declarations

        private Window _sender;


        private string _eventId;
        private string _tournamentId;

        private int _indexlastExecutedGame;
        private int _indexCurrentphase;
        private int _indexNextPhase;

        private bool _firstLoad = true;

        public Tournament tournament;

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

            //this.Show();
        }

        private void cboTurnamentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = this.cboTurnamentSelector.SelectedIndex;
            this._tournamentId = (string)_TournamantsDT.Rows[i][0].ToString();

            this.tournament = new Tournament(this._tournamentId);

            loadGames();
            //todo finf the first phase check the length 
        }

        #region buttons

        private void btnStartNextGame_Click(object sender, RoutedEventArgs e)
        {
            this.update();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region game loading functions

        public void loadGames()
        {
            //if (this.tournament.isInd)
            //    loadIndevidual();

            //if (this.tournament.isTeam)
            //    loadTeam();

            //if (this.tournament.isSync)
            //    loadSync();

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

            //List<Athlete> teamA = new List<Athlete>();
            //List<Athlete> teamB = new List<Athlete>();
            //List<Athlete> team = new List<Athlete>();

            List<Game> curentGames = new List<Game>();
            List<Game> futureGames = new List<Game>();
            //List<Game> game = new List<Game>();


            #region find the curent and next phase games list


            if (this.tournament.games128.Count != 0 && (!this.tournament.phase128Done))
            {
                this._indexCurrentphase = 6;
                this._indexNextPhase = 5;

                curentGames = this.tournament.games128;
                futureGames = this.tournament.games64;
            }

            else if (this.tournament.games64.Count != 0 && (!this.tournament.phase64Done))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;

                curentGames = this.tournament.games64;
                futureGames = this.tournament.games32;
            }

            else if (this.tournament.games32.Count != 0 && (!this.tournament.phase32Done))
            {
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;

                curentGames = this.tournament.games32;
                futureGames = this.tournament.games16;
            }

            else if (this.tournament.games16.Count != 0 && (!this.tournament.phase16Done))
            {
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

                curentGames = this.tournament.games16;
                futureGames = this.tournament.games8;
            }

            else if (this.tournament.games8.Count != 0 && (!this.tournament.phase8Done))
            {
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

                curentGames = this.tournament.games8;
                futureGames = this.tournament.games4;
            }

            else if (this.tournament.games4.Count != 0 && (!this.tournament.phase4Done))
            {
                this._indexCurrentphase = 1;
                this._indexNextPhase = 0;

                curentGames = this.tournament.games4;
                futureGames = this.tournament.games2;
            }


            else if (this.tournament.games2.Count != 0 && (!this.tournament.phase2Done))
            {
                this._indexCurrentphase = 0;
                this._indexNextPhase = -1;

                curentGames = this.tournament.games2;
                futureGames = null;
            }


            #endregion


            //List<string> current = new List<string>();
            //List<string> future = new List<string>();



            this.listBoxCurrentGameList.ItemsSource = loadStrings(curentGames);


            //foreach (Game gm in futureGames)
            //{

            //    if ((this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.score)
            //        || this.tournament.gameType == Strings.indKata
            //        || this.tournament.gameType == Strings.teamKata)
            //    {
            //        future.Add(this.createPresentationString(gm));
            //    }



            //    if (this.tournament.gameType == Strings.syncKata
            //        || this.tournament.gameType == Strings.enbu)
            //    {
            //        future.Add(this.createTeamPresentationString(gm));
            //    }



            //    if ((this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.score)
            //        || this.tournament.gameType == Strings.indKumite
            //        || this.tournament.gameType == Strings.fugugo
            //        || this.tournament.gameType == Strings.teamKumite)
            //    {
            //        future.Add(this.createVersusString(gm));
            //    }

            //}

            this.listBoxNextGameList.ItemsSource = this.loadStrings(futureGames);
        }



        #region string creators


        private List<string> loadStrings(List<Game> games)
        {

            List<string> temp = new List<string>();

            foreach (Game gm in games)
            {

                if ((this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.score)
                    || this.tournament.gameType == Strings.indKata
                    || this.tournament.gameType == Strings.teamKata)
                {
                    temp.Add(this.createPresentationString(gm));
                }



                if (this.tournament.gameType == Strings.syncKata
                    || this.tournament.gameType == Strings.enbu)
                {
                    temp.Add(this.createTeamPresentationString(gm));
                }



                if ((this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.score)
                    || this.tournament.gameType == Strings.indKumite
                    || this.tournament.gameType == Strings.fugugo
                    || this.tournament.gameType == Strings.teamKumite)
                {
                    temp.Add(this.createVersusString(gm));
                }

            }

            return temp;
        }

        private string createPresentationString(Game gm)
        {
            string temp = "";
            if (gm.participants.Count == 0)
            {
                temp = "Waiting...";
            }
            else
            {
                Athlete ath = gm.participants[0];
                temp += (ath.lastName + " " + ath.firstName);
            }

            return temp;
        }

        private string createVersusString(Game gm)
        {
            StringBuilder temp = new StringBuilder();
            Athlete A;
            Athlete B;

            switch (gm.participants.Count)
            {
                case 0:

                    if (this.tournament.gameType == Strings.teamKumite)
                    {
                        temp.Append("Click to add game members.");
                    }
                    else
                    {
                        temp.Append("Empty...");
                    }

                    break;
                case 1:

                    A = gm.participants.ElementAt(0);
                    temp.Append(A.lastName + " " + A.firstName + "\nVS\n" + "Waiting...");

                    break;
                case 2:

                    A = gm.participants.ElementAt(0);
                    B = gm.participants.ElementAt(1);
                    temp.Append(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);

                    break;
            }





            return temp.ToString();
        }

        private string createTeamPresentationString(Game gm)
        {
            string temp = "";
            if (gm.participants.Count == 0)
            {
                temp = "Waiting...";
            }
            else
                foreach (Athlete ath in gm.participants)
                {
                    temp += (ath.lastName + " " + ath.firstName + " - ");
                }
            return temp;
        }

        #endregion string creators

        /*
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

                        //this.loadIndevidualVersus();
                    }
                    else
                    {

                        if (this._firstLoad)
                            foreach (Athlete ath in tournament.participants)
                                ath.ranking = "128";


                        //this.loadIndevidualSingle();
                    }
                    break;
                case Strings.indKumite:

                    if (this._firstLoad)
                        foreach (Athlete ath in tournament.participants)
                            ath.ranking = "128";


                    //this.loadIndevidualVersus();
                    break;
                case Strings.fugugo:

                    if (this._firstLoad)
                        foreach (Athlete ath in tournament.participants)
                            ath.ranking = "128";


                    //this.loadIndevidualVersus();
                    break;
            }
        }

        */

        /*
        private void loadIndevidualVersus()
        {

            Athlete A;
            Athlete B;

            if (this.tournament.games128.Count != 0 && (!this.tournament.games128.Last().isFinished))
            {
                this._indexCurrentphase = 6;
                this._indexNextPhase = 5;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games128)
                {
                    A = gm.participants.ElementAt(0);
                    B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    switch (gm.participants.Count)
                    {
                        case 0:
                            current.Add("Empty...");

                            break;
                        case 1:
                            A = gm.participants.ElementAt(0);
                            //Athlete B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + "Waiting...");

                            break;
                        case 2:
                            A = gm.participants.ElementAt(0);
                            B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                            break;
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;


            }
            else if (this.tournament.games64.Count != 0 && (!this.tournament.games64.Last().isFinished))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    A = gm.participants.ElementAt(0);
                    B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    switch (gm.participants.Count)
                    {
                        case 0:
                            current.Add("Empty...");

                            break;
                        case 1:
                            A = gm.participants.ElementAt(0);
                            //Athlete B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + "Waiting...");

                            break;
                        case 2:
                            A = gm.participants.ElementAt(0);
                            B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                            break;
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games32.Count != 0 && (!this.tournament.games32.Last().isFinished))
            {
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    A = gm.participants.ElementAt(0);
                    B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    switch (gm.participants.Count)
                    {
                        case 0:
                            current.Add("Empty...");

                            break;
                        case 1:
                            A = gm.participants.ElementAt(0);
                            //Athlete B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + "Waiting...");

                            break;
                        case 2:
                            A = gm.participants.ElementAt(0);
                            B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                            break;
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games16.Count != 0 && (!this.tournament.games16.Last().isFinished))
            {
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    A = gm.participants.ElementAt(0);
                    B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    switch (gm.participants.Count)
                    {
                        case 0:
                            current.Add("Empty...");

                            break;
                        case 1:
                            A = gm.participants.ElementAt(0);
                            //Athlete B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + "Waiting...");

                            break;
                        case 2:
                            A = gm.participants.ElementAt(0);
                            B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                            break;
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games8.Count != 0 && (!this.tournament.games8.Last().isFinished))
            {
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    A = gm.participants.ElementAt(0);
                    B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    switch (gm.participants.Count)
                    {
                        case 0:

                            current.Add("Empty...");

                            break;
                        case 1:
                            A = gm.participants.ElementAt(0);
                            //Athlete B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + "Waiting...");

                            break;
                        case 2:
                            A = gm.participants.ElementAt(0);
                            B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                            break;
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games4.Count != 0 && (!this.tournament.games4.Last().isFinished))
            {
                this._indexCurrentphase = 1;
                this._indexNextPhase = 0;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    A = gm.participants.ElementAt(0);
                    B = gm.participants.ElementAt(1);
                    current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games2)
                {
                    switch (gm.participants.Count)
                    {
                        case 0:
                            current.Add("Empty...");

                            break;
                        case 1:
                            A = gm.participants.ElementAt(0);
                            //Athlete B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + "Waiting...");

                            break;
                        case 2:
                            A = gm.participants.ElementAt(0);
                            B = gm.participants.ElementAt(1);
                            current.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                            break;
                    }


                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games2.Count != 0 && (!this.tournament.games2.Last().isFinished))
            {
                this._indexCurrentphase = 0;
                this._indexNextPhase = -1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games2)
                {
                    A = gm.participants.ElementAt(0);
                    B = gm.participants.ElementAt(1);
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
        */

        /*
        private void loadIndevidualSingle()
        {

            if (this.tournament.games128.Count != 0 && (!this.tournament.games128.Last().isFinished))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;


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
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;


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
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

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
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

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
                this._indexCurrentphase = 1;
                this._indexNextPhase = 0;

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
                this._indexCurrentphase = 0;
                this._indexNextPhase = -1;

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
        */

        /*
        private void loadTeam()
        {
            switch (this.tournament.gameType)
            {
                case Strings.teamKata:

                    if (this._firstLoad)
                        foreach (Athlete ath in tournament.participants)
                            ath.ranking = "128";


                   // loadIndevidualSingle();

                    break;
                case Strings.teamKumite:

                    if (this._firstLoad)
                        foreach (Athlete ath in tournament.participants)
                            ath.ranking = "128";

                    loadTamVersus();

                    break;
            }
        }
        */

        /*
        private void loadTamVersus()
        {
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            string sql;
            bool phase128Done, phase64Done, phase32Done, phase16Done, phase8Done, phase4Done, phase2Done;


            sql = "select * from games where tournament_id = '" + this.tournament.id + "' and phase = '6' and is_finished = false;";
            if (conn.Query(sql).Tables[0].Rows.Count > 0)
                phase128Done = false;
            else
                phase128Done = true;

            sql = "select * from games where tournament_id = '" + this.tournament.id + "' and phase = '5' and is_finished = false;";
            if (conn.Query(sql).Tables[0].Rows.Count > 0)
                phase64Done = false;
            else
                phase64Done = true;

            sql = "select * from games where tournament_id = '" + this.tournament.id + "' and phase = '4' and is_finished = false;";
            if (conn.Query(sql).Tables[0].Rows.Count > 0)
                phase32Done = false;
            else
                phase32Done = true;

            sql = "select * from games where tournament_id = '" + this.tournament.id + "' and phase = '3' and is_finished = false;";
            if (conn.Query(sql).Tables[0].Rows.Count > 0)
                phase16Done = false;
            else
                phase16Done = true;

            sql = "select * from games where tournament_id = '" + this.tournament.id + "' and phase = '2' and is_finished = false;";
            if (conn.Query(sql).Tables[0].Rows.Count > 0)
                phase8Done = false;
            else
                phase8Done = true;

            sql = "select * from games where tournament_id = '" + this.tournament.id + "' and phase = '1' and is_finished = false;";
            if (conn.Query(sql).Tables[0].Rows.Count > 0)
                phase4Done = false;
            else
                phase4Done = true;

            sql = "select * from games where tournament_id = '" + this.tournament.id + "' and phase = '0' and is_finished = false;";
            if (conn.Query(sql).Tables[0].Rows.Count > 0)
                phase2Done = false;
            else
                phase2Done = true;



            if (this.tournament.games128.Count != 0 && (!this.tournament.phase128Done))
            {
                this._indexCurrentphase = 6;
                this._indexNextPhase = 5;


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
            else if (this.tournament.games64.Count != 0 && (!phase64Done))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;


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
            else if (this.tournament.games32.Count != 0 && (!phase32Done))
            {
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;

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
            else if (this.tournament.games16.Count != 0 && (!phase16Done))
            {
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

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
            else if (this.tournament.games8.Count != 0 && (!phase8Done))
            {
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

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
                        future.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        future.Add("Click to add game members.");
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games4.Count != 0 && (!phase4Done))
            {
                this._indexCurrentphase = 1;
                this._indexNextPhase = 0;

                //
                // current phase
                //
                List<string> current = new List<string>();
                //this.listBoxCurrentGameList.Items = new ItemCollection();


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
                //this.listBoxCurrentGameList.Items = new ItemCollection();

                foreach (Game gm in tournament.games2)
                {
                    if (gm.numOfParticipants == 2)
                    {
                        Athlete A = gm.participants.ElementAt(0);
                        Athlete B = gm.participants.ElementAt(1);
                        future.Add(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    }
                    else
                    {
                        future.Add("Click to add game members.");
                    }
                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games2.Count != 0 && (!phase2Done))
            {
                this._indexCurrentphase = 0;
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

        */

        /*
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

        */

        /*

        private void loadKataSyncSingle()
        {

            if (this.tournament.games128.Count != 0 && (!this.tournament.phase128Done))
            {
                this._indexCurrentphase = 6;
                this._indexNextPhase = 5;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games128)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = null;
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = null;
                this.listBoxNextGameList.ItemsSource = future;


            }
            else if (this.tournament.games64.Count != 0 && (!this.tournament.phase64Done))
            {
                this._indexCurrentphase = 6;
                this._indexNextPhase = 5;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = null;
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = null;
                this.listBoxNextGameList.ItemsSource = future;


            }
            else if (this.tournament.games32.Count != 0 && (!this.tournament.phase32Done))
            {
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = null;
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games16.Count != 0 && (!this.tournament.phase16Done))
            {
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = null;
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = null;
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games8.Count != 0 && (!this.tournament.phase8Done))
            {
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = null;
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = null;
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games4.Count != 0 && (!this.tournament.phase4Done))
            {
                this._indexCurrentphase = 1;
                this._indexNextPhase = 0;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = null;
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games2)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = null;
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games2.Count != 0 && (!this.tournament.phase2Done))
            {
                this._indexCurrentphase = 0;
                this._indexNextPhase = -1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games2)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = null;
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                // next Phase? no next phase
                //
                List<string> future = new List<string>();
                this.listBoxNextGameList.ItemsSource = null;
                this.listBoxNextGameList.ItemsSource = future;
            }

            else
            {
                //tournament is finished anounce the winner
            }
        }


        */

        /*
        private void loadEmbuSyncSingle()
        {

            if (this.tournament.games128.Count != 0 && (!this.tournament.phase128Done))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games128)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = future;


            }
            else if (this.tournament.games64.Count != 0 && (!this.tournament.phase64Done))
            {
                this._indexCurrentphase = 5;
                this._indexNextPhase = 4;


                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games64)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games32.Count != 0 && (!this.tournament.phase32Done))
            {
                this._indexCurrentphase = 4;
                this._indexNextPhase = 3;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games32)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = future;

            }
            else if (this.tournament.games16.Count != 0 && (!this.tournament.phase16Done))
            {
                this._indexCurrentphase = 3;
                this._indexNextPhase = 2;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games16)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games8.Count != 0 && (!this.tournament.phase8Done))
            {
                this._indexCurrentphase = 2;
                this._indexNextPhase = 1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games4.Count != 0 && (!this.tournament.phase4Done))
            {
                this._indexCurrentphase = 1;
                this._indexNextPhase = 0;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games8)
                {
                    current.Add(createTeamPresentationString(gm));
                }
                this.listBoxCurrentGameList.ItemsSource = current;

                //
                //next Phase
                //
                List<string> future = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    future.Add(createTeamPresentationString(gm));
                }
                this.listBoxNextGameList.ItemsSource = future;
            }
            else if (this.tournament.games2.Count != 0 && (!this.tournament.phase2Done))
            {
                this._indexCurrentphase = 0;
                this._indexNextPhase = -1;

                //
                // current phase
                //
                List<string> current = new List<string>();

                foreach (Game gm in tournament.games4)
                {
                    current.Add(createTeamPresentationString(gm));
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

        */


        #endregion game loading functions

        #region winner checks
        //
        // INDIVIDUAL
        //










        private List<Athlete> getKataIndSinglePositioning()
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            //
            // untested sql
            //
            sql = "select game_participations.athlete_id, mean_score from tournament_participations join game_participacions on tournament_participations.athlete_id = game_participations.athlete_id join game_score on game_participation.athlete_id = game_score.athlete_id where tournament_id = '" + this.tournament.id + "' AND phase '" + this._indexCurrentphase + "' ORDER BY mean_score DESC ;";
            DataTable temp = conn.Query(sql).Tables[0];

            List<Athlete> aths = new List<Athlete>();

            foreach (DataRow dr in temp.Rows)
                aths.Add(new Athlete((string)dr[0].ToString(), this.tournament.id));

            return aths;
        }

        private Athlete getKataIndVersusWinner(Game gm)
        {
            string sql;
            string id = gm.gameId;

            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            //
            // untested sql
            //
            sql = "select * from game_participacions join game_flag on game_participacions.game_id = game_flag.game_id where game_id = '" + id + "' ;";
            DataTable temp = conn.Query(sql).Tables[0];

            return new Athlete((string)temp.Rows[0][0].ToString(), this.tournament.id);
        }

        public Athlete getKumiteIndVersusWinner(string gameId)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "select athlete_id, sum(technical_point) from game_participacions join game_points on game_participacions.game_id = game_points.game_id where game_id = '" + gameId + "' group by (game_participations.athlete_id) ;";
            DataTable temp = conn.Query(sql).Tables[0];

            if ((int)temp.Rows[0][1] == (int)temp.Rows[1][1])
            {
                ChooseWinner getwin = new ChooseWinner(this, new Game(gameId), this.tournament);
            }
            else
            {
                if ((int)temp.Rows[0][1] > (int)temp.Rows[1][1])
                    return new Athlete((string)temp.Rows[0][0].ToString(), this.tournament.id);
                else
                    return new Athlete((string)temp.Rows[1][0].ToString(), this.tournament.id);
            }
            return null;
        }


        //
        // TEAM
        //
        private List<Team> getKataTeamIndPositioning(string phase)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            
            //
            // unchecked sql
            //
            sql = "select game_participations.team_id, sum(mean_score) from "
                + "team_tournament_participations "
                + "join game_participations "
                + "on team_tournament_participations.id = game_participations.team_id "
                + "join game_score "
                + "on team_tournament_participations.id = game_score.team_id join games on games.id = game_score.game_id "
                + "where games.tournament_id = '" + this.tournament.id
                + "' and games.phase = '" + phase + "' group by game_participations.team_id ORDER BY sum(mean_score) DESC ;";
            DataTable temp = conn.Query(sql).Tables[0];

            List<Team> teams = new List<Team>();

            foreach (DataRow dr in temp.Rows)
                teams.Add(new Team((string)dr[0].ToString()));

            return teams;
        }

        private Team getKumiteTeamWinner(Game gm)
        {

            string phase, teamA, teamB, sql;
            string gameid = gm.gameId;


            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "select team_id from games join game_participations gp on games.id = gp.game_id where game_id = '" + gameid + "';";
            DataTable temp = conn.Query(sql).Tables[0];

            phase = gm.phase;

            teamA = temp.Rows[0][0].ToString();
            teamB = temp.Rows[1][0].ToString();


            sql = "select game_participations.team_id, sum(technical_point) from game_participations join game_points on game_participations.game_id = game_points.game_id join games on games.id = game_participations.game_id where phase = '"
                + phase + "' and game_participations.team_id = '"
                + teamA + "' OR game_participations.team_id = '"
                + teamB + "'  group by (game_participations.team_id) ;";
            temp = conn.Query(sql).Tables[0];

            //
            // TODO: create gui to make user set the winner in case of a tie
            //

            if (int.Parse(temp.Rows[0][1].ToString()) > int.Parse(temp.Rows[1][1].ToString()))
                return new Team((string)temp.Rows[0][0].ToString());
            else
                return new Team((string)temp.Rows[1][0].ToString());
        }

        //
        // SYNCRONIZED
        //
        private List<Team> getSyncKataWinner(string gameId)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "SELECT game_participations.team_id, mean_score "
                + "from team_tournament_participations join "
                + "game_participations on team_tournament_participations.id = game_participations.team_id join "
                + "game_score on team_tournament_participations.id = game_score.team_id join "
                + "games on games.id = game_score.game_id "
                + "where games.tournament_id = '" + this.tournament.id
                + "' AND phase = '" + this._indexCurrentphase
                + "' ORDER BY mean_score DESC ;";
            DataTable temp = conn.Query(sql).Tables[0];

            List<Team> teams = new List<Team>();

            foreach (DataRow dr in temp.Rows)
                teams.Add(new Team((string)dr[0].ToString()));

            return teams;
        }

        private List<Team> getEnbuWinner(string gameId)
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

            if (this.listBoxCurrentGameList.SelectedIndex != -1)
                switch (this._indexCurrentphase)
                {
                    case 0:
                        gm = tournament.games2.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                        break;
                    case 1:
                        gm = tournament.games4.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                        break;
                    case 2:
                        gm = tournament.games8.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                        break;
                    case 3:
                        gm = tournament.games16.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                        break;
                    case 4:
                        gm = tournament.games32.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                        break;
                    case 5:
                        gm = tournament.games64.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                        break;
                    case 6:
                        gm = tournament.games128.ElementAt(this.listBoxCurrentGameList.SelectedIndex);
                        break;
                }


            KataSystem kataSys;
            KumiteSystem kumiteSys;
            FlagSystem flagSys;


            if (this.listBoxCurrentGameList.SelectedIndex != -1)
                if (gm.isFinished)
                {
                    MessageBox.Show("This game have already played!", "Already played", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
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

                            if (this._indexCurrentphase % 2 == 0)
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

                            if (this.listBoxCurrentGameList.SelectedItem == "Click to add game members.")
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

        public void update()
        {
            this.tournament.load();
            this.advanceAthlites();
            this.tournament.load();
            this.loadGames();
        }


        public void advanceAthlites()
        {

            //
            //  finding the last game
            //


            Game gm = null;
            Athlete winner = null;
            int gameIndex = this.listBoxCurrentGameList.SelectedIndex;

            if (gameIndex == -1)
                gameIndex = 1;

            switch (this._indexCurrentphase)
            {
                case 0:
                    gm = tournament.games2.ElementAt(gameIndex);
                    break;
                case 1:
                    gm = tournament.games4.ElementAt(gameIndex);
                    break;
                case 2:
                    gm = tournament.games8.ElementAt(gameIndex);
                    break;
                case 3:
                    gm = tournament.games16.ElementAt(gameIndex);
                    break;
                case 4:
                    gm = tournament.games32.ElementAt(gameIndex);
                    break;
                case 5:
                    gm = tournament.games64.ElementAt(gameIndex);
                    break;
                case 6:
                    gm = tournament.games128.ElementAt(gameIndex);
                    break;
            }


            //
            //  start taking the deferent cases of the games 
            //
            string sql = "SELECT phase, position FROM games WHERE tournament_id = '" + this.tournament.id + "' ORDER BY phase DESC, position;";
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            DataTable temp = conn.Query(sql).Tables[0];

            int firstPhase = (int)temp.Rows[0][0];
            int firstGame = (int)temp.Rows[0][1];




            switch (tournament.gameType)
            {
                #region indevidual kata flag and point

                case Strings.indKata:

                    switch (tournament.judgingType)
                    {
                        case Strings.flag:

                            winner = this.getKataIndVersusWinner(gm);

                            switch (this._indexCurrentphase)
                            {
                                case 1:

                                    if (firstPhase == _indexCurrentphase)
                                        gameIndex += firstGame - 1;

                                    tournament.games2.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 2:

                                    if (firstPhase == _indexCurrentphase)
                                        gameIndex += firstGame - 1;


                                    tournament.games4.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 3:

                                    if (firstPhase == _indexCurrentphase)
                                        gameIndex += firstGame - 1;


                                    tournament.games8.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 4:

                                    if (firstPhase == _indexCurrentphase)
                                        gameIndex += firstGame - 1;


                                    tournament.games16.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 5:

                                    if (firstPhase == _indexCurrentphase)
                                        gameIndex += firstGame - 1;


                                    tournament.games32.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                    break;
                                case 6:

                                    if (firstPhase == _indexCurrentphase)
                                        gameIndex += firstGame - 1;


                                    tournament.games64.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                    break;
                            }


                            break;
                        case Strings.score:

                            DataTable dt = new CoreDatabaseConnection().Query("SELECT * FROM games where phase = '" + this._indexCurrentphase + "' and is_finished = false ;").Tables[0];

                            if (dt.Rows.Count == 0)
                            {
                                List<Athlete> winners = this.getKataIndSinglePositioning();
                                int phaseIndex = 0;
                                switch (this._indexCurrentphase)
                                {
                                    case 1:

                                        foreach (Athlete kataAth in winners)
                                        {
                                            if (phaseIndex < 4)
                                            {
                                                tournament.games4.ElementAt(phaseIndex).AddParticipant(kataAth.id);
                                            }
                                            phaseIndex++;
                                        }

                                        break;
                                    case 2:

                                        foreach (Athlete kataAth in winners)
                                        {
                                            if (phaseIndex < 8)
                                            {
                                                tournament.games8.ElementAt(phaseIndex).AddParticipant(kataAth.id);
                                            }
                                            phaseIndex++;
                                        }


                                        break;
                                    case 3:

                                        foreach (Athlete kataAth in winners)
                                        {
                                            if (phaseIndex < 16)
                                            {
                                                tournament.games16.ElementAt(phaseIndex).AddParticipant(kataAth.id);
                                            }
                                            phaseIndex++;
                                        }

                                        break;
                                    case 4:

                                        foreach (Athlete kataAth in winners)
                                        {
                                            if (phaseIndex < 32)
                                            {
                                                tournament.games32.ElementAt(phaseIndex).AddParticipant(kataAth.id);
                                            }
                                            phaseIndex++;
                                        }

                                        break;
                                    case 5:

                                        foreach (Athlete kataAth in winners)
                                        {
                                            if (phaseIndex < 64)
                                            {
                                                tournament.games64.ElementAt(phaseIndex).AddParticipant(kataAth.id);
                                            }
                                            phaseIndex++;
                                        }

                                        break;
                                }
                            }

                            break;

                    }

                    break;


                #endregion indevidual kata end region

                #region indevidual kumite


                case Strings.indKumite:

                    winner = this.getKumiteIndVersusWinner(gm.gameId);

                    if (winner != null)
                        switch (this._indexCurrentphase)
                        {
                            case 1:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games2.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 2:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1; ;

                                tournament.games4.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 3:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games8.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 4:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games16.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 5:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games32.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 6:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games64.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                        }



                    break;

                #endregion indevidual kumite end region

                #region fugugo bothe judging game phases


                case Strings.fugugo:

                    if (this._indexCurrentphase % 2 == 1)
                    {   //
                        // kata phase
                        //

                        winner = this.getKataIndVersusWinner(gm);

                        switch (this._indexCurrentphase)
                        {
                            case 1:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games2.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 2:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;


                                tournament.games4.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 3:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;


                                tournament.games8.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 4:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;


                                tournament.games16.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 5:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;


                                tournament.games32.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 6:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;


                                tournament.games64.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                        }

                    }
                    else
                    {   //
                        // kumite phase
                        //



                        winner = this.getKumiteIndVersusWinner(gm.gameId);

                        switch (this._indexCurrentphase)
                        {
                            case 1:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games2.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 2:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1; ;

                                tournament.games4.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 3:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games8.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 4:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games16.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 5:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games32.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                            case 6:

                                if (firstPhase == _indexCurrentphase)
                                    gameIndex += firstGame - 1;

                                tournament.games64.ElementAt((int)Math.Ceiling(gameIndex / 2.0)).AddParticipant(winner.id);
                                break;
                        }

                    }

                    break;

                #endregion fugugo bothe judging kame phases end region

                #region team kata

                case Strings.teamKata:

                    string teamId = gm.participants.ElementAt(0).teamId;

                    temp = conn.Query("SELECT * FROM games where phase = '" + this._indexCurrentphase + "' and is_finished = false ;").Tables[0];

                    if (temp.Rows.Count == 0)
                    {
                        List<Team> teamKataWinners = this.getKataTeamIndPositioning("" + _indexCurrentphase);
                        int phaseIndex = 0;
                        switch (this._indexCurrentphase)
                        {

                            case 1:

                                foreach (Team kataTeams in teamKataWinners)
                                {
                                    if (phaseIndex < 4 * 3)
                                    {
                                        foreach (Athlete ath in kataTeams.participants)
                                        {
                                            tournament.games4.ElementAt(phaseIndex).AddParticipant(ath.id);
                                            phaseIndex++;
                                        }
                                    }
                                }

                                break;
                            case 2:

                                foreach (Team kataTeams in teamKataWinners)
                                {
                                    if (phaseIndex < 8 * 3)
                                    {
                                        foreach (Athlete ath in kataTeams.participants)
                                        {
                                            tournament.games4.ElementAt(phaseIndex).AddParticipant(ath.id);
                                            phaseIndex++;
                                        }
                                    }
                                }


                                break;
                            case 3:

                                foreach (Team kataTeams in teamKataWinners)
                                {
                                    if (phaseIndex < 16 * 3)
                                    {
                                        foreach (Athlete ath in kataTeams.participants)
                                        {
                                            tournament.games4.ElementAt(phaseIndex).AddParticipant(ath.id);
                                            phaseIndex++;
                                        }
                                    }
                                }

                                break;
                            case 4:

                                foreach (Team kataTeams in teamKataWinners)
                                {
                                    if (phaseIndex < 32 * 3)
                                    {
                                        foreach (Athlete ath in kataTeams.participants)
                                        {
                                            tournament.games4.ElementAt(phaseIndex).AddParticipant(ath.id);
                                            phaseIndex++;
                                        }
                                    }
                                }

                                break;
                            case 5:

                                foreach (Team kataTeams in teamKataWinners)
                                {
                                    if (phaseIndex < 64 * 3)
                                    {
                                        foreach (Athlete ath in kataTeams.participants)
                                        {
                                            tournament.games4.ElementAt(phaseIndex).AddParticipant(ath.id);
                                            phaseIndex++;
                                        }
                                    }
                                }

                                break;
                        }
                    }

                    break;

                #endregion team kata end region

                #region team kumite

                case Strings.teamKumite:

                    conn = new CoreDatabaseConnection();
                    teamId = gm.participants.ElementAt(0).teamId;

                    temp = conn.Query("select * from games join game_participations on games.id = game_participations.game_id where team_id = '" + teamId + "' and is_finished = true ").Tables[0];
                    if (temp.Rows.Count == 3)
                    {
                        Team kumiteTeamWinner = this.getKumiteTeamWinner(gm);
                        conn = new CoreDatabaseConnection();

                        int firstPos = (int)conn.Query("select position from games join game_participations gp on games.id = gp.game_id where team_id = '"
                            + teamId + "' order by position ASC ; ").Tables[0].Rows[0][0];

                        firstPos--;

                        if (firstPos % 2 != 0)
                            firstPos -= 3;

                        int nextPhaseFirstPos = firstPos / 3;

                        nextPhaseFirstPos++;

                        for (int i = 0; i < 3; i++)
                        {
                            conn.NonQuery("insert into game_participations (team_id, game_id) values ( '" + teamId + "', "
                                + "(SELECT id from games where tournament_id = '" + tournament.id
                                + "' AND phase = '" + _indexNextPhase
                                + "' AND position = '" + (nextPhaseFirstPos + i) + "' ) ) ;");
                        }
                    }

                    break;

                #endregion team kumite end region

                #region sync kata


                case Strings.syncKata:

                    temp = new CoreDatabaseConnection().Query("SELECT * FROM games where phase = '" + this._indexCurrentphase + "' and is_finished = false ;").Tables[0];

                    if (temp.Rows.Count == 0)
                    {

                        List<Team> winners = this.getSyncKataWinner(this._indexCurrentphase.ToString());

                        int positionIndex = 0;

                        switch (this._indexCurrentphase)
                        {
                            case 1:
                                positionIndex = 0;
                                foreach (Team kataTeam in winners)
                                {

                                    if (positionIndex < 4)
                                        foreach (Athlete ath in kataTeam.participants)
                                        {
                                            tournament.games2.ElementAt(positionIndex).AddParticipant(ath.id);
                                        }
                                    positionIndex++;
                                }

                                break;
                            case 2:

                                positionIndex = 0;
                                foreach (Team kataTeam in winners)
                                {

                                    if (positionIndex < 8)
                                        foreach (Athlete ath in kataTeam.participants)
                                        {
                                            tournament.games4.ElementAt(positionIndex).AddParticipant(ath.id);
                                        }
                                    positionIndex++;
                                }

                                break;
                            case 3:

                                positionIndex = 0;
                                foreach (Team kataTeam in winners)
                                {

                                    if (positionIndex < 16)
                                        foreach (Athlete ath in kataTeam.participants)
                                        {
                                            tournament.games8.ElementAt(positionIndex).AddParticipant(ath.id);
                                        }
                                    positionIndex++;
                                }

                                break;
                            case 4:

                                positionIndex = 0;
                                foreach (Team kataTeam in winners)
                                {

                                    if (positionIndex < 32)
                                        foreach (Athlete ath in kataTeam.participants)
                                        {
                                            tournament.games16.ElementAt(positionIndex).AddParticipant(ath.id);
                                        }
                                    positionIndex++;
                                }

                                break;
                            case 5:

                                positionIndex = 0;
                                foreach (Team kataTeam in winners)
                                {

                                    if (positionIndex < 64)
                                        foreach (Athlete ath in kataTeam.participants)
                                        {
                                            tournament.games32.ElementAt(positionIndex).AddParticipant(ath.id);
                                        }
                                    positionIndex++;
                                }

                                break;


                        }
                    }



                    break;

                #endregion synk kata end region

                #region embu


                case Strings.enbu:

                    temp = new CoreDatabaseConnection().Query("SELECT * FROM games where phase = '" + this._indexCurrentphase + "' and is_finished = false ;").Tables[0];

                    if (temp.Rows.Count == 0)
                    {

                        List<Team> winners = this.getEnbuWinner(this._indexCurrentphase.ToString());

                        int positionIndex = 0;

                        switch (this._indexCurrentphase)
                        {
                            case 1:

                                foreach (Team kataTeam in winners)
                                {
                                    positionIndex = 0;
                                    foreach (Athlete ath in kataTeam.participants)
                                    {
                                        if (positionIndex < 4)
                                            tournament.games2.ElementAt(positionIndex).AddParticipant(ath.id);
                                        positionIndex++;
                                    }

                                }

                                break;
                            case 2:

                                foreach (Team kataTeam in winners)
                                {
                                    positionIndex = 0;
                                    foreach (Athlete ath in kataTeam.participants)
                                    {
                                        if (positionIndex < 8)
                                            tournament.games4.ElementAt(positionIndex).AddParticipant(ath.id);
                                        positionIndex++;
                                    }
                                }


                                break;
                            case 3:

                                foreach (Team kataTeam in winners)
                                {
                                    positionIndex = 0;
                                    foreach (Athlete ath in kataTeam.participants)
                                    {
                                        if (positionIndex < 16)
                                            tournament.games8.ElementAt(positionIndex).AddParticipant(ath.id);
                                        positionIndex++;
                                    }
                                }

                                break;
                            case 4:

                                foreach (Team kataTeam in winners)
                                {
                                    positionIndex = 0;
                                    foreach (Athlete ath in kataTeam.participants)
                                    {
                                        if (positionIndex < 32)
                                            tournament.games16.ElementAt(positionIndex).AddParticipant(ath.id);
                                        positionIndex++;
                                    }
                                }

                                break;
                            case 5:

                                foreach (Team kataTeam in winners)
                                {
                                    positionIndex = 0;
                                    foreach (Athlete ath in kataTeam.participants)
                                    {
                                        if (positionIndex < 64)
                                            tournament.games32.ElementAt(positionIndex).AddParticipant(ath.id);
                                        positionIndex++;
                                    }
                                }
                                break;
                        }



                    }
                    break;

                #endregion embu end region
            }




            // this.update();
        }


        private void advanceVsGame(Game gm)
        { 
            //getKumiteIndVersusWinner


        
        
        
        }



        private int findNextPossInd(int current)
        {
            double temp = ((double)current + 1) / 2;
            int next = (int)Math.Floor(temp);
            return next;
        }



        private int findNextPossTeamX3(int current)
        {
            int next3 = current;

            while ((next3 % 3) == 0)
                next3++;


           
            if (((double)next3 / 2) != 0)
                current += 3;


            return findNextPossInd(current);
        }



    }
}



