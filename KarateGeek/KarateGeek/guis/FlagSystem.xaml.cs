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
    /// Interaction logic for FlagSystem.xaml
    /// </summary>
    public partial class FlagSystem : Window
    {

        #region private declaretion

        private Window _sender;

        private string _judgeAId = "";
        private string _judgeBId = "";
        private string _judgeCId = "";
        private string _judgeDId = "";
        private string _judgeEId = "";

        private DataTable _DTcompetitors;

        private Boolean _judgeAchooseWhite;
        private Boolean _judgeBchooseWhite;
        private Boolean _judgeCchooseWhite;
        private Boolean _judgeDchooseWhite;
        private Boolean _judgeEchooseWhite;

        private string _gameId = "";
        private string _turnamentId = "";
        private string _participationId = "";
        private bool _isTeam ;


        private DataTable _DTjudges;
        private DataTable _DTparticipations;
        private DataTable _DTgame;



        private Style darkGray = new Style { TargetType = typeof(Button) };
        private Style lightGray = new Style { TargetType = typeof(Button) };
            




        #endregion 




        public FlagSystem(Window sender, string turnamentId, string gameId, Boolean isTeam)
        {
            InitializeComponent();

            darkGray.Setters.Add(new Setter(Button.BackgroundProperty , Brushes.Gray));
            this.reda.Style = darkGray ;

            lightGray.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.LightGray));
            this.whitea.Style = lightGray;


            _sender = sender;

            this._turnamentId = turnamentId;
            this._gameId = gameId;
            this._isTeam = isTeam;

            this._loadDataTables();

            foreach (DataRow dr in _DTjudges.Rows)
            {
                this.eventJudgePickerA.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerB.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerC.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerD.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerE.Items.Add("" + dr[1] + " " + dr[2]);
            }


            string gametype = this._DTgame.Rows[0][4].ToString();

            TournamentGameParticipationsConnection tourparconn = new TournamentGameParticipationsConnection();
            this._DTparticipations = tourparconn.GetParticipation(_gameId).Tables[0];

            // getting paeticipant id
            if (_isTeam)
                _participationId = _DTparticipations.Rows[0][1].ToString();
            else
                _participationId = _DTparticipations.Rows[0][0].ToString();



        }







        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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


        #endregion


        #region color choosing button listeners

        private void reda_Click(object sender, RoutedEventArgs e)
        {
            this._judgeAchooseWhite = false;
            
            this.reda.Style = darkGray ;
            this.whitea.Style = lightGray;
            
        }

        private void whitea_Click(object sender, RoutedEventArgs e)
        {
            this._judgeAchooseWhite = true;

            this.reda.Style = lightGray;
            this.whitea.Style = darkGray;
        }

        private void redb_Click(object sender, RoutedEventArgs e)
        {

            this._judgeBchooseWhite = false;

            this.redb.Style = darkGray;
            this.whiteb.Style = lightGray;

        }

        private void whiteb_Click(object sender, RoutedEventArgs e)
        {
            this._judgeBchooseWhite = true;

            this.redb.Style = lightGray;
            this.whiteb.Style = darkGray;
        }

        private void redc_Click(object sender, RoutedEventArgs e)
        {
            this._judgeCchooseWhite = false;

            this.redc.Style = darkGray;
            this.whitec.Style = lightGray;
        }

        private void whitec_Click(object sender, RoutedEventArgs e)
        {
            this._judgeCchooseWhite = true;

            this.redc.Style = lightGray;
            this.whitec.Style = darkGray;
        }

        private void redd_Click(object sender, RoutedEventArgs e)
        {
            this._judgeDchooseWhite = false;

            this.redd.Style = darkGray;
            this.whited.Style = lightGray;
        }

        private void whited_Click(object sender, RoutedEventArgs e)
        {
            this._judgeDchooseWhite = true;

            this.redd.Style = lightGray;
            this.whited.Style = darkGray;
        }

        private void rede_Click(object sender, RoutedEventArgs e)
        {
            this._judgeEchooseWhite = false;

            this.rede.Style = darkGray;
            this.whitee.Style = lightGray;
        }

        private void whitee_Click(object sender, RoutedEventArgs e)
        {
            this._judgeEchooseWhite = true;

            this.rede.Style = lightGray;
            this.whitee.Style = darkGray;
        }

        #endregion

        #region menu buttons save back etc

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            EveSupFlagConnection conn = new EveSupFlagConnection();


            if (_isTeam)
            {
                conn.InsertNewflagTeam(_gameId, _participationId, _judgeAId, _judgeBId, _judgeCId, _judgeDId, _judgeEId,
                    _judgeAchooseWhite, _judgeBchooseWhite, _judgeCchooseWhite, _judgeDchooseWhite, _judgeEchooseWhite);
            }
            else 
            {
                conn.InsertNewflagInd(_gameId, _participationId, _judgeAId, _judgeBId, _judgeCId, _judgeDId, _judgeEId,
                    _judgeAchooseWhite, _judgeBchooseWhite, _judgeCchooseWhite, _judgeDchooseWhite, _judgeEchooseWhite);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this._sender.Show();
            this.Close();
        }

        #endregion


    }
}
