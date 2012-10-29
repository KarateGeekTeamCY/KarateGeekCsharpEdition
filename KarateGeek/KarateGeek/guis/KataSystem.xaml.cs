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





        public KataSystem(string gameId, string participationId)
        {
            InitializeComponent();
            this._gameId = gameId;
            this._ParticipationId = participationId;

            GameConnection gameconn = new GameConnection();
            this._game = gameconn.GetGameById(gameId).Tables[0];

            JudgeConnection judgeconn = new JudgeConnection();
            this._judges = judgeconn.GetJudges().Tables[0];

            foreach (DataRow dr in _judges.Rows)
            { 
                this.eventJudgePickerA.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerB.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerC.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerD.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerE.Items.Add("" + dr[1] + " " + dr[2]);
            }



            string gametype = this._game.Rows[0][4].ToString() ;
            if (gametype == "")
            {
                

            }
            else
            {


            }

        }


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










    }
}
