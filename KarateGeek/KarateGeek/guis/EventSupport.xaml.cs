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

        private bool _isTeam;

        private string _gameType;
        private string _judgingType;

        private string _eventId;
        private string _tournamentId;
        private int _lastExecutedGame;



        private TournamentConnection _tournamentConn;
        private TournamentGameParticipationsConnection _tournamentParticipationConn;
        private EventConnection _eventConn;
        private GameConnection _gameConn;
        private TournamentGameParticipationsConnection _gameParticipationConn;

        private DataTable _TournamantsDT;
        private DataTable _currentGamesDT;
        private DataTable _futureGamesDT;
        private DataTable _currentGameParticipationsDT;
        private DataTable _futureGameParticipationsDT;

        private List<string> _availableTournaments;
        private List<string> _currentGames;
        private List<string> _futureGames;
        private List<string> _currentGameParticipants;
        private List<string> _futureGameParticipants;


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

            _tournamentConn = new TournamentConnection();
            this._TournamantsDT = this._tournamentConn.findSimilar("", int.Parse(this._eventId)).Tables[0];

            foreach (DataRow dr in _TournamantsDT.Rows)
            {
                this._availableTournaments.Add("" + dr[1]);
            }

            this.cboTurnamentSelector.ItemsSource = _availableTournaments;
            cboTurnamentSelector.SelectedIndex = 0;


        }


        #region buttons

        private void btnStartNextGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion


        #region the load shit

        private void cboTurnamentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = this.cboTurnamentSelector.SelectedIndex;
            this._tournamentId = _TournamantsDT.Rows[i][0].ToString();

            this._gameType = (string)_TournamantsDT.Rows[i][7];
            string[] type = _gameType.Split('|');

            if (type[0] == Strings.team)
                this._isTeam = true;
            else
                this._isTeam = false;

            this._judgingType = (string)_TournamantsDT.Rows[i][8];


            _LoadPhases();




        }

        private void _LoadPhases()
        {
            DataTable gamesTempDT = _gameConn.getFuturePhaces(this._tournamentId).Tables[0];

            int currentIndex, futureIndex;

            int phaseCount = gamesTempDT.Rows.Count;

            if (phaseCount == 2)
            {

                if (int.Parse(gamesTempDT.Rows[0][0].ToString()) < int.Parse(gamesTempDT.Rows[1][0].ToString()))
                {
                    futureIndex = int.Parse(gamesTempDT.Rows[0][0].ToString());
                    currentIndex = int.Parse(gamesTempDT.Rows[1][0].ToString());
                }
                else
                {
                    currentIndex = int.Parse(gamesTempDT.Rows[1][0].ToString());
                    futureIndex = int.Parse(gamesTempDT.Rows[0][0].ToString());
                }


                this._currentGamesDT = this._gameConn.GetGamesByTurnamentPhase(_tournamentId, "" + currentIndex).Tables[0];
                this._futureGamesDT = this._gameConn.GetGamesByTurnamentPhase(_tournamentId, "" + futureIndex).Tables[0];

            }
            else
            {
                currentIndex = int.Parse(gamesTempDT.Rows[1][0].ToString());
                this._currentGamesDT = this._gameConn.GetGamesByTurnamentPhase(_tournamentId, "" + currentIndex).Tables[0];

            }



            this._gameParticipationConn = new TournamentGameParticipationsConnection();




            foreach (DataRow dr in _currentGamesDT.Rows)
            {
                this._currentGameParticipationsDT = this._gameParticipationConn.GetParticipation(dr[0].ToString()).Tables[0];
                int parts = _currentGameParticipationsDT.Rows.Count;
                string game = "";

                switch (parts)
                {
                    case 1:
                        game = _currentGameParticipationsDT.Rows[0][5].ToString() + _currentGameParticipationsDT.Rows[0][6].ToString();
                        break;
                    case 2:
                        game = _currentGameParticipationsDT.Rows[0][5].ToString() + _currentGameParticipationsDT.Rows[0][6].ToString();
                        game = "\nVS";
                        game = _currentGameParticipationsDT.Rows[1][5].ToString() + _currentGameParticipationsDT.Rows[1][6].ToString();
                        break;
                    default:
                        int I = _currentGameParticipationsDT.Rows.Count;

                        for (int i = 0; i < I; i++)
                        {
                            game = _currentGameParticipationsDT.Rows[i][5].ToString() + " " + _currentGameParticipationsDT.Rows[i][6].ToString();

                            if (1 != (I - 1))
                                game += " - ";

                        }
                        break;
                }
                this._currentGameParticipants.Add(game);
            }



            foreach (DataRow dr in this._futureGamesDT.Rows)
            {
                this._futureGameParticipationsDT = this._gameParticipationConn.GetParticipation(dr[0].ToString()).Tables[0];
                int parts = _futureGameParticipationsDT.Rows.Count;
                string game = "";

                switch (parts)
                {
                    case 1:
                        game = _futureGameParticipationsDT.Rows[0][5].ToString() + _futureGameParticipationsDT.Rows[0][6].ToString();
                        break;
                    case 2:
                        game = _futureGameParticipationsDT.Rows[0][5].ToString() + _futureGameParticipationsDT.Rows[0][6].ToString();
                        game = "\nVS";
                        game = _futureGameParticipationsDT.Rows[1][5].ToString() + _futureGameParticipationsDT.Rows[1][6].ToString();
                        break;
                    default:
                        int I = _futureGameParticipationsDT.Rows.Count;

                        for (int i = 0; i < I; i++)
                        {
                            game = _futureGameParticipationsDT.Rows[i][5].ToString() + " " + _futureGameParticipationsDT.Rows[i][6].ToString();

                            if (1 != (I - 1))
                                game += " - ";
                        }
                        break;
                }
                this._futureGameParticipants.Add(game);
            }

            this.listBoxCurrentGameList.ItemsSource = this._currentGameParticipants;
            this.listBoxNextGameList.ItemsSource = this._futureGameParticipants;
        }

        #endregion the load shet


        // #region the progress shit 


        private void listBoxCurrentGameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             _lastExecutedGame = this.listBoxCurrentGameList.SelectedIndex;

            if (_gameType == Strings.fugugo)
            {

                if ((int.Parse(this._currentGamesDT.Rows[_lastExecutedGame][1].ToString()) % 2) == 0)
                {
                    _judgingType = Strings.point;
                }
                else
                {
                    _judgingType = Strings.score;
                }
            }


            switch (this._judgingType)
            {
                case Strings.flag:
                    FlagSystem flagS = new FlagSystem(this, this._tournamentId, this._currentGamesDT.Rows[_lastExecutedGame][0].ToString(), this._isTeam);
                    break;

                case Strings.point:
                    KumiteSystem kumiteS = new KumiteSystem(this, this._currentGamesDT.Rows[_lastExecutedGame][0].ToString());
                    break;

                case Strings.score:
                    KataSystem kataS = new KataSystem(this, this._tournamentId, this._currentGamesDT.Rows[_lastExecutedGame][0].ToString(), this._isTeam);
                    break;

            }


        }




        public void AdvanceGameStatistics(string gameId)
        {
            switch (this._judgingType)
            {
                case Strings.flag:

                    EveSupFlagConnection flagC = new EveSupFlagConnection();
                    _gameParticipationConn = new TournamentGameParticipationsConnection();

                    DataTable gameParticipants = this._gameParticipationConn.GetParticipation(""+_lastExecutedGame).Tables[0];



                    DataTable flag = flagC.GetflagById(
                        this._currentGamesDT.Rows[_lastExecutedGame][2].ToString(),

                        gameParticipants.Rows[0][0].ToString()
                        ).Tables[0];           // 2 4



                    int flagCount = 0;

                    for (int i = 4; i <= 8; i++)
                        if (((bool)flag.Rows[0][i]) == true)
                            flagCount++;


                    if (flagCount > 2)
                    {
                        string newGameId = _CheckIfExistAndInsert();
                        this._gameParticipationConn.insertParticipant(
                            gameParticipants.Rows[0][0].ToString(),
                            newGameId);
                    }
                    else
                    {
                        string newGameId = _CheckIfExistAndInsert();
                        this._gameParticipationConn.insertParticipant(
                            gameParticipants.Rows[0][1].ToString(),
                            newGameId);
                    }


                    break;

                case Strings.point:
                    EveSupPointConnection pointC = new EveSupPointConnection();
                    DataTable points = pointC.getPointsOfAthleteAtGame(this._currentGamesDT.Rows[_lastExecutedGame][2].ToString(),
                        this._currentGamesDT.Rows[_lastExecutedGame][4].ToString());

                    double pointsA = (double)points.Rows[0][2];
                    double pointsB = (double)points.Rows[1][2];

                    if (pointsA > pointsB)
                    {
                        string newGameId = _CheckIfExistAndInsert();
                        this._gameParticipationConn.insertParticipant(
                            points.Rows[0][1].ToString(), newGameId);
                    }
                    else
                    {
                        string newGameId = _CheckIfExistAndInsert();
                        this._gameParticipationConn.insertParticipant(
                            points.Rows[1][1].ToString(), newGameId);
                    }



                    break;

                case Strings.score:
                    EveSupScoreConnection scoreC = new EveSupScoreConnection();
                    DataTable score = scoreC.GetScoreById(this._currentGamesDT.Rows[_lastExecutedGame][2].ToString(),
                        this._currentGamesDT.Rows[_lastExecutedGame][4].ToString()).Tables[0];









                    break;

            }



        }


        private string _CheckIfExistAndInsert()
        {
            int nextPhase, nextPos;

            nextPhase = (int)this._currentGamesDT.Rows[_lastExecutedGame][1] - 1;
            nextPos = (int)Math.Ceiling(((double)this._currentGamesDT.Rows[_lastExecutedGame][2]) / 2);

            // check if there axisting game at that phase position
            DataTable temp = _gameConn.GetGamesByTurnamentPhasePosition(this._tournamentId, "" + nextPhase, "" + nextPos).Tables[0];
            //if 0 then create a new game at that phase possition
            if (temp.Rows.Count == 0)
            {
                return _gameConn.InsertNewGame(this._tournamentId, "" + nextPhase, "" + nextPos, this._gameType);
            }


            return "-1";
        }







    }



    class Game : KarateGeek.databaseConnection.CoreDatabaseConnection
    {
        private string sql;

        public string touenamentId { get; set; }
        public string gameId { get; set; }
        public string phase { get; set; }
        public string position { get; set; }
        public List<string> participantIds { get; set; }
        public List<string> participantTeamIds { get; set; }
        public List<string> participants { get; set; }
        public bool isReady { get; set; }
        public bool isFinished { get; set; }
        public int numOfParticipants { get; set; }


        public Game(string gameId)
        {
            this.gameId = gameId;
            this.load();
        }


        public void load()
        {

            sql = "select * from games where id = '" + this.gameId + "';";
            DataTable temp = this.Query(sql).Tables[0];

            
            this.phase = temp.Rows[0][1].ToString();
            this.position = temp.Rows[0][2].ToString();
            this.touenamentId = temp.Rows[0][3].ToString();
            this.isReady = (bool)temp.Rows[0][4];
            this.isFinished = (bool)temp.Rows[0][5];

            sql = "select * from game_participants_total_det where game_id = '" + this.gameId + "'";
            temp = this.Query(sql).Tables[0];

            this.numOfParticipants = 0;

            foreach (DataRow dr in temp.Rows)
            {
                this.participantIds.Add(dr[4].ToString());
                this.participantTeamIds.Add(dr[3].ToString());
                this.participants.Add(dr[5].ToString() + " " + dr[6].ToString());
                this.numOfParticipants++;
            }
        }

        public void Update()
        {

            //UPDATE table_name
            //SET column1=value, column2=value2,...
            //WHERE some_column=some_value
            sql = "UPDATE games SET is_ready = '" + this.isReady + "', " +
                "is_finished = '" + this.isFinished + "' where game_id = '" + this.gameId + "'; ";

            for (int i = 0; i < this.numOfParticipants; i++)
            {
                sql = "select * from game_participants where game_id = '" + this.gameId + "' and athlete_id = '" + this.participantIds.ElementAt(i) + "' ; ";

                int exist = this.Query(sql).Tables[0].Rows.Count;
                if (exist == 0)
                {
                    sql = "insert into game_participations";
                }

            }


        }





    }



}
