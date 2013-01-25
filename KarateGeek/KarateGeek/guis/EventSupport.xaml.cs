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
            this._availableTournaments = new List<string>();


            foreach (DataRow dr in _TournamantsDT.Rows)
            {
                this._availableTournaments.Add(dr[1].ToString());
            }

            this.cboTurnamentSelector.ItemsSource = _availableTournaments;
        }



        private void cboTurnamentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = this.cboTurnamentSelector.SelectedIndex;
            this._tournamentId = _TournamantsDT.Rows[i][0].ToString();

            this.tournament = new Tournament(this._tournamentId);

            loadGames();
        }



        //
        //  nothing here yet 
        //  need to implement 
        //
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
            List<Game> curentGames = new List<Game>();
            List<Game> futureGames = new List<Game>();



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



            //
            // the actual strings list creation for the gui to show
            //
            this.listBoxCurrentGameList.ItemsSource = this.loadStrings(curentGames);

            if (futureGames != null)
            { this.listBoxNextGameList.ItemsSource = this.loadStrings(futureGames); }
            else
            {
                //
                // final game msg
                //
            }
        }



        #region string creators


        private List<string> loadStrings(List<Game> games)
        {

            List<string> temp = new List<string>();

            int type = 0;

            if ((this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.score)
                            || this.tournament.gameType == Strings.indKata
                            || this.tournament.gameType == Strings.teamKata)
            { type = 1; }

            else if (this.tournament.gameType == Strings.syncKata
                        || this.tournament.gameType == Strings.enbu)
            { type = 2; }

            else if ((this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.score)
                        || this.tournament.gameType == Strings.indKumite
                        || this.tournament.gameType == Strings.fugugo
                        || this.tournament.gameType == Strings.teamKumite)
            { type = 3; }



            foreach (Game gm in games)
            {
                if (type == 1)
                { temp.Add(this.createPresentationString(gm)); }

                else if (type == 2)
                { temp.Add(this.createTeamPresentationString(gm)); }

                else if (type == 3)
                { temp.Add(this.createVersusString(gm)); }
            }

            return temp;
        }



        private string createPresentationString(Game gm)
        {
            string temp = "";
            if (gm.participants.Count == 0)
            { temp = "Waiting..."; }

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



        public Athlete getKumiteIndVersusWinner(Game gm)
        {
            string sql, gameid;
            gameid = gm.gameId;

            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "SELECT athlete_id, sum(technical_point) FROM game_points where game_id = " + gameid + " group by athlete_id;";
            DataTable temp = conn.Query(sql).Tables[0];

            int a = int.Parse(temp.Rows[0][1].ToString());
            int b = int.Parse(temp.Rows[1][1].ToString());

            if (a == b)
            {
                ChooseWinner getwin = new ChooseWinner(this, new Game(gameid), this.tournament);

                getwin.Show();

                return null;
            }
            else
            {
                if (a > b)
                    return new Athlete((string)temp.Rows[0][0].ToString(), this.tournament.id);
                else
                    return new Athlete((string)temp.Rows[1][0].ToString(), this.tournament.id);
            }

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



        private string getKumiteTeamWinner(Game gm)
        {
            string phase, teamA, teamB, sql;
            string gameid = gm.gameId;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();



            sql = "SELECT team_id FROM games JOIN game_participations gp ON games.id = gp.game_id where game_id = '" + gameid + "';";
            DataTable temp = conn.Query(sql).Tables[0];

            teamA = temp.Rows[0][0].ToString();
            teamB = temp.Rows[1][0].ToString();


            phase = gm.phase;

            sql = "SELECT * FROM games join game_participations ON games.id = game_participations.game_id"
                + " WHERE team_id = " + teamA
                + " AND tournament_id = " + tournament.id
                + " AND phase = " + gm.phase
                + " AND is_ready = 't' AND is_finished = 't' ;";

            temp = conn.Query(sql).Tables[0];

            if (temp.Rows.Count == 3)
            {

               

                sql = "select team_id, sum(technical_point) from game_points"
                    + " where game_id in( select game_id from games "
                            + " where tournament_id = " + tournament.id
                            + " and phase = " + phase + " )"
                    + " and (team_id = " + teamA + " OR team_id = " + teamB + " ) group by team_id ;";

                

                temp = conn.Query(sql).Tables[0];

                if (int.Parse(temp.Rows[0][1].ToString()) != int.Parse(temp.Rows[1][1].ToString()))
                {
                    if (int.Parse(temp.Rows[0][1].ToString()) > int.Parse(temp.Rows[1][1].ToString()))
                        return temp.Rows[0][0].ToString();
                    else
                        return temp.Rows[1][0].ToString();
                }
                else
                {
                    ChooseWinner choice = new ChooseWinner(this, gm, tournament);
                    choice.Show();
                    return "-1";
                }
            }
            else
            {
                //
                // case were not all the games are finished
                // so that the advancing code can take respective actions.
                //
                return "-1";
            }
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

                            if (this.listBoxCurrentGameList.SelectedItem.ToString() == "Click to add game members.")
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
            this.advanceAthletes();
            this.tournament.load();
            this.loadGames();

        }


        public void advanceAthlites(Game gm)
        {
            this.advanceVsGame(gm);
        }

        public void advanceAthletes()
        {
            //
            //  checks if the tournament type is versus or presentation
            //  and then does the apropriate preparetion for the winner advancement
            //
            if ((this.tournament.gameType == Strings.indKumite)
                || (this.tournament.gameType == Strings.teamKumite)
                || (this.tournament.gameType == Strings.fugugo)
                || (this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.flag))
            {
                //
                //  this is the vs case
                //
                Game gm = null;

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

                this.advanceVsGame(gm);

            }
            else
            {
                //
                //  the presentetion game winner 
                //  advancing method
                //
                this.advancePresentationWinners();
            }

            //
            // i don't now if thats necessary here but will live it 
            // as a note in case of something go wrong
            // although i am sertaind that i dont need this
            //
            // code:
            // this.update();
            //
        }



        private void advanceVsGame(Game gm)
        {
            string winner = "-1";

            if (this.tournament.gameType == Strings.indKumite)
            {
                // winner  
                Athlete temp = getKumiteIndVersusWinner(gm);
                if (temp != null)
                    winner = temp.id;

            }



            if (this.tournament.gameType == Strings.teamKumite)
                winner = getKumiteTeamWinner(gm);


            if (this.tournament.gameType == Strings.fugugo)
                if ((_indexCurrentphase % 2) == 0)
                    winner = getKumiteIndVersusWinner(gm).id;
                else
                    winner = getKataIndVersusWinner(gm).id;


            if (this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.flag)
                winner = getKataIndVersusWinner(gm).id;

            //
            // here is the actual advancing of the winners if there are any for the given moment (round)
            //
            if (winner != "-1")
            {
                int nextPhase, nextPoss;
                int currentPhase, currentPosition;
                string sql;

                if (this.tournament.gameType == Strings.indKumite || (this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.flag))
                {
                    currentPosition = int.Parse(gm.position);
                    nextPoss = findNextPossInd(currentPosition);

                    currentPhase = int.Parse(gm.phase);
                    nextPhase = currentPhase - 1;

                    if (nextPhase == -1)
                    {
                        //
                        // TO-DO
                        //
                        // its the winner there is no more rounds
                        // do something about that
                        //


                        if (this.tournament.gameType == Strings.teamKumite)
                        {
                            setRanking(new Team(winner), _indexNextPhase.ToString());
                        }
                        else
                        {
                            setRanking(new Athlete(winner, tournament.id), _indexNextPhase.ToString());
                        }

                    }
                    else
                    {

                        sql = "SELECT id FROM games WHERE tournament_id = " + this.tournament.id
                            + " AND phase = " + nextPhase
                            + " AND position = " + nextPoss + ";";
                        CoreDatabaseConnection conn = new CoreDatabaseConnection();
                        string nextgameid = conn.Query(sql).Tables[0].Rows[0][0].ToString();



                        string winnertype;
                        if (this.tournament.gameType == Strings.teamKumite)
                            winnertype = "team_id";
                        else
                            winnertype = "athlete_id";


                        if (this.tournament.gameType == Strings.teamKumite)
                        {
                            int next3 = findnext3(nextPoss);
                            for (int i = (next3 - 3); i < next3; i++)
                            {
                                nextgameid = findGameId(this.tournament.id, nextPhase.ToString(), i.ToString());

                                sql = "INSERT INTO game_participations (" + winnertype + ", game_id )"
                                + " VALUES (" + winner + ", " + nextgameid + "); ";
                                conn.NonQuery(sql);
                            }

                            setRanking(new Team(winner), _indexNextPhase.ToString());
                        }
                        else
                        {
                            nextgameid = findGameId(this.tournament.id.ToString(), nextPhase.ToString(), nextPoss.ToString());

                            sql = "INSERT INTO game_participations (" + winnertype + ", game_id )"
                                + " VALUES (" + winner + ", " + nextgameid + "); ";
                            conn.NonQuery(sql);

                            setRanking(new Athlete(winner, tournament.id), _indexNextPhase.ToString());
                        }

                    }
                }
            }
        }


        private void advancePresentationWinners()
        {
            List<Athlete> aWinners = null;
            List<Team> tWinners = null;


            if (this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.score)
                aWinners = getKataIndSinglePositioning();

            if (this.tournament.gameType == Strings.teamKata)
                tWinners = getKataTeamIndPositioning(_indexCurrentphase.ToString());

            if (this.tournament.gameType == Strings.enbu)
                tWinners = getEnbuWinner(_indexCurrentphase.ToString());


            if (this.tournament.gameType == Strings.syncKata)
                tWinners = getSyncKataWinner(_indexCurrentphase.ToString());


            int i = 1;

            if (this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.score)
            {
                foreach (Athlete ath in aWinners)
                {
                    addSingleParticipant(findGameId(this.tournament.id.ToString(), _indexNextPhase.ToString(), i.ToString()), ath);
                    i++;
                }
            }

            i = 1;

            if (this.tournament.gameType == Strings.teamKata
            || this.tournament.gameType == Strings.enbu
            || this.tournament.gameType == Strings.syncKata)
            {
                foreach (Team team in tWinners)
                {
                    addTeamParticipant(findGameId(this.tournament.id.ToString(), _indexNextPhase.ToString(), i.ToString()), team);
                    i++;
                }
            }
        }



        private void setRanking(Athlete ath, string phase)
        {
            string sql = "";
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            string ranking;

            ranking = Math.Pow(2, int.Parse(phase) + 1).ToString();


            sql = "UPDATE tournament_participations SET ranking = " + ranking
                + " WHERE athlete_id = " + ath.id
                + " AND tournament_id = " + tournament.id + " ;";

            conn.NonQuery(sql);
        }



        private void setRanking(Team tm, string phase)
        {
            string sql = "";
            CoreDatabaseConnection conn = new CoreDatabaseConnection();

            foreach (Athlete ath in tm.participants)
            {
                setRanking(ath, phase);
            }

            sql = "UPDATE team_tournament_participations SET ranking = " + phase
                + " WHERE id = " + tm.id
                + " AND tournament_id = " + tournament.id + " ;";

            conn.NonQuery(sql);


        }



        private void addSingleParticipant(string gameid, Athlete ath)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "INSERT INTO game_participations (athlete_id, game_id) VAlUES ( " + ath.id + ", " + gameid + " ); ";
            conn.NonQuery(sql);

            setRanking(ath, Math.Pow(2, _indexNextPhase).ToString());

        }


        private void addTeamParticipant(string gameid, Team team)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();

            foreach (Athlete ath in team.participants)
            {
                sql = "INSERT INTO game_participations (team_id, athlete_id, game_id) VAlUES ( " + team.id + ", " + ath.id + " , " + gameid + " ); ";
                conn.NonQuery(sql);
            }


            setRanking(team, Math.Pow(2, _indexNextPhase).ToString());
        }



        private string findGameId(string tourid, string phase, string poss)
        {
            string sql = "SELECT id FROM games WHERE tournament_id = " + tourid
                                        + " AND phase = " + phase
                                        + " AND position = " + poss + ";";
            CoreDatabaseConnection conn = new CoreDatabaseConnection();

            return conn.Query(sql).Tables[0].Rows[0][0].ToString();
        }



        private int findNextPossInd(int current)
        {
            double temp = ((double)current + 1) / 2;
            int next = (int)Math.Floor(temp);
            return next;
        }



        private int findNextPossTeamX3(int current)
        {
            int next3 = findnext3(current);

            if (((double)next3 / 2) != 0)
                current += 3;

            return findNextPossInd(current);
        }



        private int findnext3(int current)
        {
            int next3 = current;

            while (((next3 % 3) == 0) && (next3 != 0))
                next3++;

            return next3;
        }


    }
}



