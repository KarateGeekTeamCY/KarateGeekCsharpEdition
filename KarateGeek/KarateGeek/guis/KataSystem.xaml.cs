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
        private string _ParticipationId = "";
        private bool _isIndividual = true;


        private DataTable _judges;
        private DataTable _participation;
        private DataTable _game;

        #endregion



        public KataSystem(string gameId, string participationId)
        {
            InitializeComponent();
            this._gameId = gameId;
            this._ParticipationId = participationId;


            this._loadDataTables();


            foreach (DataRow dr in _judges.Rows)
            {
                this.eventJudgePickerA.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerB.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerC.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerD.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerE.Items.Add("" + dr[1] + " " + dr[2]);
            }
            //string runka = Strings.rank1;


            string gametype = this._game.Rows[0][4].ToString();
            if (gametype == "")
            {


            }
            else
            {


            }


        }


        #region private functions

        private string _loadDataTables()
        {
            GameConnection gameconn = new GameConnection();
            this._game = gameconn.GetGameById(this._gameId).Tables[0];

            JudgeConnection judgeconn = new JudgeConnection();
            this._judges = judgeconn.GetJudges().Tables[0];





            return "";
        }


        #endregion


        #region score listeners

        private void scoreA_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this._scoreA = double.Parse(this.scoreA.Text);
        }

        private void scoreB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this._scoreB = double.Parse(this.scoreB.Text);
        }

        private void scoreC_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this._scoreC = double.Parse(this.scoreC.Text);
        }

        private void scoreD_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this._scoreD = double.Parse(this.scoreD.Text);
        }

        private void scoreE_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this._scoreE = double.Parse(this.scoreE.Text);
        }

        #endregion

        #region judge listeners

        private void eventJudgePickerA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeAId = this._judges.Rows[this.eventJudgePickerA.SelectedIndex][0].ToString() ;
        }

        private void eventJudgePickerB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeBId = this._judges.Rows[this.eventJudgePickerB.SelectedIndex][0].ToString();
        }

        private void eventJudgePickerC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeCId = this._judges.Rows[this.eventJudgePickerC.SelectedIndex][0].ToString();
        }

        private void eventJudgePickerD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeDId = this._judges.Rows[this.eventJudgePickerD.SelectedIndex][0].ToString();
        }

        private void eventJudgePickerE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeEId = this._judges.Rows[this.eventJudgePickerE.SelectedIndex][0].ToString();
        }

        #endregion







    }
}
