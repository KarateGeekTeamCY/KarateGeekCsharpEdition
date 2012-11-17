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

        private Window _sender;

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

        private string _gameId = "";
        private string _turnamentId = "";
        private string _participationId = "";
        private bool _isTeam;


        private DataTable _DTjudges;
        private DataTable _DTparticipations;
        private DataTable _DTgame;



        #endregion



        public KataSystem( Window sender, Game gm)
        {
            InitializeComponent();

            _sender = sender;

            //this._turnamentId = turnamentId;
            //this._gameId = gameId;
            //this._isTeam = isTeam;

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


            string gametype = this._DTgame.Rows[0][4].ToString();

            TournamentGameParticipationsConnection tourparconn = new TournamentGameParticipationsConnection();
            this._DTparticipations = tourparconn.GetParticipation(_gameId).Tables[0];

            // getting paeticipant id
            if (_isTeam)
                _participationId = _DTparticipations.Rows[0][1].ToString();
            else
                _participationId = _DTparticipations.Rows[0][0].ToString();




        }


        #region private functions

        private string _loadDataTables()
        {
            GameConnection gameconn = new GameConnection();
            this._DTgame = gameconn.GetGameById(this._gameId).Tables[0];

            JudgeConnection judgeconn = new JudgeConnection();
            this._DTjudges = judgeconn.GetJudges().Tables[0];


            return "";
        }



        private void _computeMean()
        {
            double smaller = 0, larger = 10; ;

            if (smaller > _scoreA)
                smaller = _scoreA;
            if (larger < _scoreA)
                larger = +_scoreA;

            if (smaller > _scoreB)
                smaller = _scoreB;
            if (larger < _scoreB)
                larger = +_scoreB;

            if (smaller > _scoreC)
                smaller = _scoreC;
            if (larger < _scoreC)
                larger = +_scoreC;

            if (smaller > _scoreD)
                smaller = _scoreD;
            if (larger < _scoreD)
                larger = +_scoreD;

            if (smaller > _scoreE)
                smaller = _scoreE;
            if (larger < _scoreE)
                larger = +_scoreE;

            this.lblSmallestScore.Content = this.lblSmallestScore.Content + " = " + smaller;
            this.lblLargestScore.Content = this.lblLargestScore.Content + " = " + larger;

            _treamdMean = (_scoreA + _scoreB + _scoreC + _scoreD + _scoreE - smaller - larger) / 3;

            this.lblTrimmedMeanScore.Content = this.lblTrimmedMeanScore.Content + " = " + _treamdMean;

        }

        #endregion


        #region score listeners

        private void scoreA_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this._scoreA = double.Parse(this.scoreA.Text);
            this._computeMean();
        }

        private void scoreB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this._scoreB = double.Parse(this.scoreB.Text);
            this._computeMean();
        }

        private void scoreC_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this._scoreC = double.Parse(this.scoreC.Text);
            this._computeMean();
        }

        private void scoreD_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this._scoreD = double.Parse(this.scoreD.Text);
            this._computeMean();
        }

        private void scoreE_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this._scoreE = double.Parse(this.scoreE.Text);
            this._computeMean();
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
            EveSupScoreConnection scoreconn = new EveSupScoreConnection();
            if(_isTeam){
                scoreconn.InsertNewScoreTeam(_gameId, _participationId, _judgeAId, _judgeBId, _judgeCId, _judgeDId, _judgeEId, 
                _scoreA, _scoreB, _scoreC, _scoreD, _scoreE, _treamdMean);
            }
            else
            {
                scoreconn.InsertNewScoreInd(_gameId, _participationId, _judgeAId, _judgeBId, _judgeCId, _judgeDId, _judgeEId,
                _scoreA, _scoreB, _scoreC, _scoreD, _scoreE, _treamdMean);
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _sender.Visibility = System.Windows.Visibility.Visible;
            this.Close();
        }

        #endregion




    }
}


