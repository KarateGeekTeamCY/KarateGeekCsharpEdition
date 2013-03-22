﻿using System;
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
using System.Diagnostics;
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

        /** EXPERIMENTAL code, for testing purposes (added by Nicholas): */
        private LotteryGraph graph;



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

            string sql = "select * from tournaments where event_id = '" + this._eventId + "' AND lottery_ready = 't' AND is_finished = 'f' ORDER BY name; ";
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

            //cleanTeamKumite();

            this.tournament = new Tournament(this._tournamentId);

            loadGames();

        }


        private void cleanTeamKumite()
        {
            CoreDatabaseConnection con = new CoreDatabaseConnection();
            string sql = "";



        }



        //
        //  nothing here yet
        //  need to implement
        //
        #region buttons


        private void btnRefreshe_Click(object sender, RoutedEventArgs e)
        {
            this.tournament.load();
            this.loadGames();
        }



        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _sender.Show();
            this.Close();

            /** EXPERIMENTAL (ugly) code, for testing purposes (added by Nicholas): */
            if (this.graph != null) this.graph.Close();
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

            /** EXPERIMENTAL (ugly) code, for testing purposes (added by Nicholas): */
            if (this.graph == null)
            {
                /** EXPERIMENTAL code, for testing purposes (added by Nicholas): */
                this.graph = new LotteryGraph(long.Parse(this.tournament.id));      // predictably, this could crash for Team Kumite because it needs at least 1 record in the table (tournaments JOIN games ON tournaments.id = games.tournament_id)
                // solution: we used the hasEnoughElementsToPrint() method in the LotteryGraph GUI, especially for team kumite.
            }
            else { this.graph.updateGraph(); }


            try { this.graph.Show(); }
            catch (InvalidOperationException e)
            { this.graph = new LotteryGraph(long.Parse(this.tournament.id)); }


            this.Activate();

            this.listBoxCurrentGameList.UnselectAll();
        }





        #region string creators


        private List<string> loadStrings(List<Game> games)
        {

            List<string> temp = new List<string>();

            int type = 0;

            if ((this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.score)
                            || this.tournament.gameType == Strings.teamKata)
            { type = 1; }

            else if (this.tournament.gameType == Strings.syncKata
                        || this.tournament.gameType == Strings.enbu)
            { type = 2; }

            else if ((this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.flag)
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
                        //VV           Click to add game members
                        temp.Append("Click to add game members.");
                    }
                    else
                    {
                        temp.Append("\nEmpty...\n");
                    }

                    break;
                case 1:

                    A = gm.participants.ElementAt(0);
                    //temp.Append(A.lastName + " " + A.firstName + "\nVS\n" + "Waiting...");
                    temp.Append(string.Format("\n{0,-25} VS {1,-25}\n", A.lastName + " " + A.firstName, "Waiting...")); //TODO: align correctly

                    break;
                case 2:

                    A = gm.participants.ElementAt(0);
                    B = gm.participants.ElementAt(1);
                    //temp.Append(A.lastName + " " + A.firstName + "\nVS\n" + B.lastName + " " + B.firstName);
                    temp.Append(string.Format("\n{0,-25} VS {1,-25}\n", A.lastName + " " + A.firstName, B.lastName + " " + B.firstName)); //TODO: align correctly

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

            sql = "select gp.athlete_id, mean_score FROM game_participations gp join games g on gp.game_id = g.id join game_score gs on gs.game_id = g.id where tournament_id = '"
                + this.tournament.id
                + "' AND  phase = '" + this._indexCurrentphase
                + "' ORDER BY mean_score DESC ;";


            //sql = "select game_participations.athlete_id, mean_score from tournament_participations join game_participacions on tournament_participations.athlete_id = game_participations.athlete_id join game_score on game_participation.athlete_id = 
            //game_score.athlete_id where tournament_id = '" + this.tournament.id + "' AND phase '" + this._indexCurrentphase + "' ORDER BY mean_score DESC ;";

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
            sql = "SELECT athlete_id FROM game_flag WHERE game_id = '" + id + "' ;";
            DataTable temp = conn.Query(sql).Tables[0];

            return new Athlete(temp.Rows[0][0].ToString(), this.tournament.id);
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

                int scoreA = int.Parse(temp.Rows[0][1].ToString());
                int scoreB = int.Parse(temp.Rows[1][1].ToString());

                if (scoreA != scoreB)
                {
                    if (scoreA > scoreB)
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
            sql = "SELECT DISTINCT game_participations.team_id, mean_score "
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
                                flagSys = new FlagSystem(this, gm);
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

            Game gm = null;
            bool ready = false;
            int gameIndex = this.listBoxCurrentGameList.SelectedIndex;

            if (gameIndex == -1)
                gameIndex = 1;

            switch (this._indexCurrentphase)
            {
                case 0:
                    gm = tournament.games2.ElementAt(gameIndex);
                    ready = this.tournament.phase2Done;

                    break;
                case 1:
                    gm = tournament.games4.ElementAt(gameIndex);
                    ready = this.tournament.phase4Done;

                    break;
                case 2:
                    gm = tournament.games8.ElementAt(gameIndex);
                    ready = this.tournament.phase8Done;

                    break;
                case 3:
                    gm = tournament.games16.ElementAt(gameIndex);
                    ready = this.tournament.phase16Done;

                    break;
                case 4:
                    gm = tournament.games32.ElementAt(gameIndex);
                    ready = this.tournament.phase32Done;

                    break;
                case 5:
                    gm = tournament.games64.ElementAt(gameIndex);
                    ready = this.tournament.phase64Done;

                    break;
                case 6:
                    gm = tournament.games128.ElementAt(gameIndex);
                    ready = this.tournament.phase128Done;

                    break;
            }



            if ((this.tournament.gameType == Strings.indKumite)
                || (this.tournament.gameType == Strings.teamKumite)
                || (this.tournament.gameType == Strings.fugugo)
                || (this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.flag))
            {
                //
                //  this is the vs case
                //
                this.advanceVsGame(gm);

            }
            else
            {
                //
                //  the presentetion game winner
                //  advancing method
                //
                if (ready)
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
                        setRankingByPhase(new Team(winner), _indexNextPhase.ToString());


                    }
                    else
                    {
                        setRankingByPhase(new Athlete(winner, tournament.id), _indexNextPhase.ToString());


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
                        for (int i = (next3 - 2); i <= next3; i++)
                        {
                            nextgameid = findGameId(this.tournament.id, nextPhase.ToString(), i.ToString());

                            sql = "INSERT INTO game_participations (" + winnertype + ", game_id, prev_position )"
                                + " VALUES (" + winner + ", " + nextgameid + ", " + gm.position + "); ";

                            conn.NonQuery(sql);
                        }

                        setRankingByPhase(new Team(winner), _indexNextPhase.ToString());
                    }
                    else
                    {
                        nextgameid = findGameId(this.tournament.id.ToString(), nextPhase.ToString(), nextPoss.ToString());

                        sql = "INSERT INTO game_participations (" + winnertype + ", game_id, prev_position )"
                                + " VALUES (" + winner + ", " + nextgameid + ", " + gm.position + "); ";
                        conn.NonQuery(sql);

                        setRankingByPhase(new Athlete(winner, tournament.id), _indexNextPhase.ToString());
                    }

                }
            }
        }


        public void advancePresentationWinners()
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


            if (aWinners != null)
                aWinners = checkTie(aWinners);

            if (tWinners != null)
                tWinners = checkTie(tWinners);


            if ((aWinners != null) || (tWinners != null))
            {
                int i = 1;
                double last = Math.Pow(2, (_indexCurrentphase + 1));

                if (this.tournament.gameType == Strings.indKata && this.tournament.judgingType == Strings.score)
                    foreach (Athlete athlete in aWinners)
                    {

                        if ((double)i < last + 1)
                            if (!(_indexNextPhase == -1))
                                addParticipantToGame(findGameId(this.tournament.id.ToString(), _indexNextPhase.ToString(), i.ToString()), athlete);

                        insertRank(athlete, i.ToString());
                        i++;
                    }


                i = 1;

                if (this.tournament.gameType == Strings.enbu
                || this.tournament.gameType == Strings.syncKata)
                    foreach (Team team in tWinners)
                    {

                        if ((double)i < last + 1)
                            if (!(_indexNextPhase == -1))
                                addParticipantToGame(findGameId(this.tournament.id.ToString(), _indexNextPhase.ToString(), i.ToString()), team);

                        insertRank(team, i.ToString());
                        i++;
                    }


                i = 1;
                last = Math.Pow(2, (_indexCurrentphase + 1));

                if (this.tournament.gameType == Strings.teamKata)
                {
                    int rank = 1;

                    foreach (Team team in tWinners)
                    {
                        if ((double)rank < last + 1)
                            if (!(_indexNextPhase == -1))
                                foreach (Athlete a in team.participants)
                                {
                                    addParticipantToGame(findGameId(this.tournament.id.ToString(), _indexNextPhase.ToString(), i.ToString()), a, team);
                                    i++;
                                }
                        insertRank(team, rank.ToString());
                        rank++;
                    }
                }


                this.tournament.load();
                this.loadGames();
            }



        }

        //aths list is ordered
        List<Athlete> checkTie(List<Athlete> aths)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();

            //int last = (_indexCurrentphase == 1) ? 0 : (int)Math.Pow(2, (_indexCurrentphase + 1));
            //Debug.Assert(_indexCurrentphase != 0);
            int last = (int)Math.Pow(2, (_indexCurrentphase + 1));

            last--; // zero based array system what a weird thing 

            string lastid = aths.ElementAt(last).id;


            sql = "SELECT mean_score FROM game_score WHERE athlete_id = '" + lastid + "' AND game_id"
                + " IN (SELECT id FROM games WHERE tournament_id = '" + this.tournament.id + "' AND phase = '" + _indexCurrentphase + "') ;";

            string scorea = conn.Query(sql).Tables[0].Rows[0][0].ToString();

            string nextid = aths.ElementAt(last + 1).id;

            sql = "SELECT mean_score FROM game_score WHERE athlete_id = '" + nextid + "' AND game_id"
                + " IN (SELECT id FROM games WHERE tournament_id = '" + this.tournament.id + "' AND phase = '" + _indexCurrentphase + "') ;";

            string scoreb = conn.Query(sql).Tables[0].Rows[0][0].ToString();


            if (scorea == scoreb)
            {
                sql = "select gp.athlete_id, mean_score FROM game_participations gp "
                    + "join games g on gp.game_id = g.id join game_score gs on gs.game_id = g.id "
                    + "where tournament_id = '" + this.tournament.id
                    + "' AND  phase = '" + this._indexCurrentphase
                    + "' AND  mean_score = '" + scorea
                    + "' ORDER BY mean_score DESC ;";

                DataTable temp = conn.Query(sql).Tables[0];


                ChooseKataWinner katawin = new ChooseKataWinner(this, this.tournament, temp, this._indexCurrentphase);
                return null;
            }
            else
            {
                return aths;
            }

        }


        List<Team> checkTie(List<Team> teams)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();

            Debug.Assert(_indexCurrentphase != 0);
            int last = (int)Math.Pow(2, (_indexCurrentphase + 1));

            last--; // zero based array system what a weird thing 

            string lastid = teams.ElementAt(last).id;


            sql = "SELECT SUM(mean_score) FROM game_score WHERE team_id = '" + lastid + "' AND game_id"
                + " IN (SELECT id FROM games WHERE tournament_id = '" + this.tournament.id + "' AND phase = '" + _indexCurrentphase + "') GROUP BY team_id ;";

            string scorea = conn.Query(sql).Tables[0].Rows[0][0].ToString();



            string nextid = teams.ElementAt(last + 1).id;

            sql = "SELECT SUM(mean_score) FROM game_score WHERE team_id = '" + nextid + "' AND game_id"
                + " IN (SELECT id FROM games WHERE tournament_id = '" + this.tournament.id + "' AND phase = '" + _indexCurrentphase + "') GROUP BY team_id ;";

            string scoreb = conn.Query(sql).Tables[0].Rows[0][0].ToString();


            if (scorea == scoreb)
            {
                sql = "SELECT gp.team_id, SUM(mean_score)"
                    + "FROM game_participations gp "
                    + "JOIN games g ON gp.game_id = g.id "
                    + "JOIN game_score gs ON gs.game_id = g.id "
                    + "WHERE tournament_id = '" + this.tournament.id + "' AND  phase = '" + this._indexCurrentphase + "' AND '" + scoreb + "' = "
                            + "(SELECT SUM(mean_score) "
                                + "FROM game_score WHERE game_id IN "
                                + "(SELECT ig.id FROM game_participations igp "
                                                + "JOIN games ig ON igp.game_id = ig.id "
                                                + "JOIN game_score igs ON igs.game_id = ig.id "
                                + "WHERE ig.tournament_id = g.tournament_id AND ig.phase = g.phase AND igp.team_id = gp.team_id )) "
                    + "GROUP BY gp.team_id ORDER BY SUM(mean_score) DESC  ;";


                DataTable temp = conn.Query(sql).Tables[0];

                ChooseKataWinner katawin = new ChooseKataWinner(this, this.tournament, temp, this._indexCurrentphase);
                return null;
            }
            else
            {
                return teams;
            }
        }


        private void insertRank(Athlete ath, string rank)
        {
            CoreDatabaseConnection conn = new CoreDatabaseConnection();

            string sql = "UPDATE tournament_participations SET ranking = " + rank
                + " WHERE athlete_id = " + ath.id
                + " AND tournament_id = " + tournament.id + " ;";

            conn.NonQuery(sql);
        }


        private void insertRank(Team team, string rank)
        {
            CoreDatabaseConnection conn = new CoreDatabaseConnection();

            foreach (Athlete ath in team.participants)
                insertRank(ath, rank);

            string sql = "UPDATE team_tournament_participations SET ranking = " + rank
                + " WHERE id = " + team.id
                + " AND tournament_id = " + tournament.id + " ;";

            conn.NonQuery(sql);
        }


        //
        //  for vs games
        //
        private void setRankingByPhase(Athlete athlete, string phase)
        {
            string ranking = Math.Pow(2, int.Parse(phase) + 1).ToString();

            insertRank(athlete, ranking);
        }


        private void setRankingByPhase(Team team, string phase)
        {
            string ranking = Math.Pow(2, int.Parse(phase) + 1).ToString();

            insertRank(team, ranking);
        }


        private void addParticipantToGame(string gameid, Athlete ath)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();

            sql = "INSERT INTO game_participations (athlete_id, game_id) VAlUES ( " + ath.id + ", " + gameid + " ); ";
            conn.NonQuery(sql);
        }

        private void addParticipantToGame(string gameid, Athlete ath, Team tm)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();

            sql = "INSERT INTO game_participations (team_id, athlete_id, game_id) VAlUES ( " + tm.id + ", " + ath.id + " , " + gameid + " ); ";
            conn.NonQuery(sql);
        }


        private void addParticipantToGame(string gameid, Team team)
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();

            foreach (Athlete ath in team.participants)
            {
                sql = "INSERT INTO game_participations (team_id, athlete_id, game_id) VAlUES ( " + team.id + ", " + ath.id + " , " + gameid + " ); ";
                conn.NonQuery(sql);
            }
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

            while (((next3 % 3) != 0) && (next3 != 0))
                next3++;

            return next3;
        }


        private void Window_close(object sender, EventArgs e)
        {
            this.Close();
            this._sender.Show();
        }


    }
}



