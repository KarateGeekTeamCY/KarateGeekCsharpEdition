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



        private TournamentConnection _tournamentConn = new TournamentConnection();
        private TournamentGameParticipationsConnection _tournamentParticipationConn = new TournamentGameParticipationsConnection();
        private EventConnection _eventConn = new EventConnection();
        private GameConnection _gameConn = new GameConnection();
        private TournamentGameParticipationsConnection _gameParticipationConn = new TournamentGameParticipationsConnection();

        private DataTable _TournamantsDT;
        private DataTable _currentGamesDT;
        private DataTable _futureGamesDT;
        private DataTable _currentGameParticipationsDT;
        private DataTable _futureGameParticipationsDT;

        private List<string> _availableTournaments = new List<string>();
        private List<string> _currentGames = new List<string>();
        private List<string> _futureGames = new List<string>();
        private List<string> _currentGameParticipants = new List<string>();
        private List<string> _futureGameParticipants = new List<string>();


        private Tournament tournament;


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
            //cboTurnamentSelector.SelectedIndex = 0;

        }


        #region buttons

        private void btnStartNextGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion



        private void cboTurnamentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = this.cboTurnamentSelector.SelectedIndex;
            this._tournamentId = _TournamantsDT.Rows[i][0].ToString();

            this.tournament = new Tournament(this._tournamentId);
            loadGames();
            //todo finf the first phase check the length 
        }


        private void loadGames()
        {
            if (this.tournament.isInd)
            {
                loadIndevidual();
            }

            if (this.tournament.isTeam)
            {
                loadTeam();
            }

            if (this.tournament.isSync)
            {
                loadSync();
            }

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

                    }
                    else
                    {

                    }

                    break;
                case Strings.indKumite:


                    break;
                case Strings.fugugo:


                    break;
            }
        }

        private void loadTeam()
        {
            switch (this.tournament.gameType)
            {
                case Strings.teamKata:


                    break;
                case Strings.teamKumite:


                    break;
            }
        }

        private void loadSync()
        {
            switch (this.tournament.gameType)
            {
                case Strings.syncKata:



                    break;
                case Strings.enbu:



                    break;
            }
        }

        //
        // INDEVIDUAL
        //
        private List<Athlete> getKataIndSinglePositioning()
        {
            string sql;
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            sql = "select game_participations.athlete_id, mean_score from tournament_participations join game_participacions on tournament_participations.athlete_id = game_participations.athlete_id join game_score on game_participation.athlete_id = game_score.athlete_id where tournament_id = '" + this.tournament.id + "' ORDER BY mean_score DESC ;";
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


    }
}
