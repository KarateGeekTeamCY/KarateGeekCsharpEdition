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
    /// Interaction logic for KataSystem.xaml
    /// </summary>
    public partial class KataSystem : Window
    {

        #region private declaretions

        private EventSupport _sender;

        private string _judgeAId = "";
        private string _judgeBId = "";
        private string _judgeCId = "";
        private string _judgeDId = "";
        private string _judgeEId = "";

        private double _scoreA = 0;
        private double _scoreB = 0;
        private double _scoreC = 0;
        private double _scoreD = 0;
        private double _scoreE = 0;
        private double _treamdMean = 0;

        private Game _game;
        private Tournament tour;

        private string _participationId = "";
        private bool _isTeam;
        private bool _isSync;


        private DataTable _DTjudges;
        private DataTable _DTparticipations;
        private DataTable _DTgame;





        #endregion



        public KataSystem(EventSupport sender, Game gm)
        {
            InitializeComponent();

            this._game = gm;
            this._sender = sender;

            this.tour = new Tournament(_game.tournamentId);


            this._isTeam = this.tour.isTeam;
            this._isSync = this.tour.isSync;




            this._loadDataTables();

            foreach (DataRow dr in _DTjudges.Rows)
            {
                this.eventJudgePickerA.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerB.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerC.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerD.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerE.Items.Add("" + dr[1] + " " + dr[2]);
            }
            //string runka = Strings.rank1;

            this.eventJudgePickerA.SelectedIndex = 1;
            this.eventJudgePickerB.SelectedIndex = 1;
            this.eventJudgePickerC.SelectedIndex = 1;
            this.eventJudgePickerD.SelectedIndex = 1;
            this.eventJudgePickerE.SelectedIndex = 1;

            string gametype = this._DTgame.Rows[0][4].ToString();

            TournamentGameParticipationsConnection tourparconn = new TournamentGameParticipationsConnection();
            this._DTparticipations = tourparconn.GetParticipation(this._game.gameId).Tables[0];

            // getting paeticipant id
            if (_isTeam || _isSync)
                _participationId = _DTparticipations.Rows[0][3].ToString();
            else
                _participationId = _DTparticipations.Rows[0][4].ToString();

            this.lblInfo.Content = "";
            foreach (Athlete ath in this._game.participants)
            {
                this.lblInfo.Content += ath.lastName + " " + ath.firstName + " | ";

            }

            this.scoreA.Text = "5";
            this.scoreB.Text = "5";
            this.scoreC.Text = "5";
            this.scoreD.Text = "5";
            this.scoreE.Text = "5";


            this._sender.Hide();
            this.Show();
        }


        #region private functions

        private string _loadDataTables()
        {
            GameConnection gameconn = new GameConnection();
            this._DTgame = gameconn.GetGameById(this._game.gameId).Tables[0];

            JudgeConnection judgeconn = new JudgeConnection();
            this._DTjudges = judgeconn.GetJudges().Tables[0];


            return "";
        }



        private void _computeMean()
        {
            double smaller = 10, larger = 0;

            if (smaller > _scoreA)
                smaller = _scoreA;
            if (larger < _scoreA)
                larger = _scoreA;

            if (smaller > _scoreB)
                smaller = _scoreB;
            if (larger < _scoreB)
                larger = _scoreB;

            if (smaller > _scoreC)
                smaller = _scoreC;
            if (larger < _scoreC)
                larger = _scoreC;

            if (smaller > _scoreD)
                smaller = _scoreD;
            if (larger < _scoreD)
                larger = _scoreD;

            if (smaller > _scoreE)
                smaller = _scoreE;
            if (larger < _scoreE)
                larger = _scoreE;

            this.lblSmallestScore.Content = " smallest score = " + smaller;
            this.lblLargestScore.Content = " largest score = " + larger;

            _treamdMean = (_scoreA + _scoreB + _scoreC + _scoreD + _scoreE - smaller - larger) / 3;

            this.lblTrimmedMeanScore.Content = "trimmed score = " + _treamdMean;

        }

        #endregion


        #region score listeners

        private void scoreA_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                this._scoreA = double.Parse(this.scoreA.Text);
                this._computeMean();
            }
            catch (Exception ex)
            {

            }
        }

        private void scoreB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                this._scoreB = double.Parse(this.scoreB.Text);
                this._computeMean();
            }
            catch (Exception ex)
            {

            }



        }

        private void scoreC_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

            try
            {


                this._scoreC = double.Parse(this.scoreC.Text);
                this._computeMean();
            }
            catch (Exception ex)
            {

            }

        }

        private void scoreD_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

            try
            {
                this._scoreD = double.Parse(this.scoreD.Text);
                this._computeMean();
            }
            catch (Exception ex)
            {

            }



        }

        private void scoreE_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                this._scoreE = double.Parse(this.scoreE.Text);
                this._computeMean();
            }
            catch (Exception ex)
            {

            }



        }


        #endregion


        #region judge listeners

        private void eventJudgePickerA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeAId = this._DTjudges.Rows[this.eventJudgePickerA.SelectedIndex][0].ToString();
        }

        private void eventJudgePickerB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeBId = this._DTjudges.Rows[this.eventJudgePickerB.SelectedIndex][0].ToString();
        }

        private void eventJudgePickerC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeCId = this._DTjudges.Rows[this.eventJudgePickerC.SelectedIndex][0].ToString();
        }

        private void eventJudgePickerD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeDId = this._DTjudges.Rows[this.eventJudgePickerD.SelectedIndex][0].ToString();
        }

        private void eventJudgePickerE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeEId = this._DTjudges.Rows[this.eventJudgePickerE.SelectedIndex][0].ToString();
        }

        #endregion


        #region buttons functionality

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.scoreA.Text = "0";
            this.scoreB.Text = "0";
            this.scoreC.Text = "0";
            this.scoreD.Text = "0";
            this.scoreE.Text = "0";

            this.lblSmallestScore.Content = "smallest score";
            this.lblLargestScore.Content = "largest score";
            this.lblTrimmedMeanScore.Content = "treamed mean score";

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //EveSupScoreConnection scoreconn = new EveSupScoreConnection();



            if (_isTeam)
            {
                this.InsertNewScoreTeam(_game.gameId, _participationId, _judgeAId, _judgeBId, _judgeCId, _judgeDId, _judgeEId,
                _scoreA, _scoreB, _scoreC, _scoreD, _scoreE, _treamdMean);
            }
            else if (_isSync)
            {
                this.InsertNewScoreTeam(_game.gameId, _participationId, _judgeAId, _judgeBId, _judgeCId, _judgeDId, _judgeEId,
                    _scoreA, _scoreB, _scoreC, _scoreD, _scoreE, _treamdMean);
            }
            else
            {
                this.InsertNewScoreInd(_game.gameId, _participationId, _judgeAId, _judgeBId, _judgeCId, _judgeDId, _judgeEId,
                _scoreA, _scoreB, _scoreC, _scoreD, _scoreE, _treamdMean);
            }


            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            string sql = "update games set is_finished = 'true' where id = '" + this._game.gameId + "'; ";
            conn.NonQuery(sql);

            this._sender.update();
            _sender.Visibility = System.Windows.Visibility.Visible;
            this.Close();

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _sender.Visibility = System.Windows.Visibility.Visible;
            this.Close();
        }




        private string InsertNewScoreInd(string gameId, string athleteId, string judge1Id, string judge2Id, string judge3Id, string judge4Id, string judge5Id,
                                                                        double score1, double score2, double score3, double score4, double score5, double mean)
        {

            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            string sql = "insert into game_score (game_id, athlete_id, judge1, judge2, judge3, judge4, judge5, " +
                "score1, score2, score3, score4, score5, mean_score) values ('" +
                gameId + "', '" + athleteId + "', '" +
                judge1Id + "', '" + judge2Id + "', '" + judge3Id + "', '" + judge4Id + "', '" + judge5Id + "', '" +
                score1 + "', '" + score2 + "' ,'" + score3 + "' ,'" + score4 + "' ,'" + score5 + "' ,'" + mean + "');";

            conn.NonQuery(sql);
            return "";
        }


        private string InsertNewScoreTeam(string gameId, string teamId, string judge1Id, string judge2Id, string judge3Id, string judge4Id, string judge5Id,
                                                                        double score1, double score2, double score3, double score4, double score5, double mean)
        {
            CoreDatabaseConnection conn = new CoreDatabaseConnection();
            string sql = "INSERT INTO game_score (game_id, team_id, judge1, judge2, judge3, judge4, judge5, " +
                "score1, score2, score3, score4, score5, mean_score) VALUES ('" +
                gameId + "', '" + teamId + "', '" +
                judge1Id + "', '" + judge2Id + "', '" + judge3Id + "', '" + judge4Id + "', '" + judge5Id + "', '" +
                score1 + "', '" + score2 + "' ,'" + score3 + "' ,'" + score4 + "' ,'" + score5 + "' ,'" + mean + "');";

            conn.NonQuery(sql);
            return "";
        }



        #endregion

        private void Window_close(object sender, EventArgs e)
        {
            this.Close();
            this._sender.Show();
        }




    }
}


