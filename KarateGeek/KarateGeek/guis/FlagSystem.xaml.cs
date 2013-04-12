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

        private EventSupport _sender;

        private string _judgeAId = "";
        private string _judgeBId = "";
        private string _judgeCId = "";
        private string _judgeDId = "";
        private string _judgeEId = "";

        KarateGeek.helpers.JudgeMem memory = new helpers.JudgeMem();

        private Boolean _judgeAchooseWhite;
        private Boolean _judgeBchooseWhite;
        private Boolean _judgeCchooseWhite;
        private Boolean _judgeDchooseWhite;
        private Boolean _judgeEchooseWhite;

        private Boolean _aClicked = false;
        private Boolean _bClicked = false;
        private Boolean _cClicked = false;
        private Boolean _dClicked = false;
        private Boolean _eClicked = false;


        private Game _game;
        private Tournament _tournament;


        private string _gameId = "";
        private string _participantA = "";
        private string _participantB = "";


        private DataTable _DTjudges;
        private DataTable _DTparticipations;
        private DataTable _DTgame;



        //private Style darkGray = new Style { TargetType = typeof(Button) };
        //private Style lightGray = new Style { TargetType = typeof(Button) };





        #endregion




        public FlagSystem(EventSupport sender, Game gm)
        {
            InitializeComponent();

            this._game = gm;
            this._sender = sender;

            this._tournament = new Tournament(this._game.tournamentId);



            //this.darkGray.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Gray));
            //this.lightGray.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.LightGray));



            //this.reda.Style = lightGray;
            //this.redb.Style = lightGray;
            //this.redc.Style = lightGray;
            //this.redd.Style = lightGray;
            //this.rede.Style = lightGray;

            this.reda.Content = Strings.aka;
            this.redb.Content = Strings.aka;
            this.redc.Content = Strings.aka;
            this.redd.Content = Strings.aka;
            this.rede.Content = Strings.aka;

            //this.whitea.Style = lightGray;
            //this.whiteb.Style = lightGray;
            //this.whitec.Style = lightGray;
            //this.whited.Style = lightGray;
            //this.whitee.Style = lightGray;

            this.whitea.Content = Strings.shiro;
            this.whiteb.Content = Strings.shiro;
            this.whitec.Content = Strings.shiro;
            this.whited.Content = Strings.shiro;
            this.whitee.Content = Strings.shiro;

            //
            //  to be removed
            //
            //this._turnamentId = turnamentId;
            this._gameId = gm.gameId;
            //this._isTeam = isTeam;
            //
            //

            this._loadDataTables();

            foreach (DataRow dr in _DTjudges.Rows)
            {
                this.eventJudgePickerA.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerB.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerC.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerD.Items.Add("" + dr[1] + " " + dr[2]);
                this.eventJudgePickerE.Items.Add("" + dr[1] + " " + dr[2]);
            }


            string ids = this.memory.load(@"flag.mem");

            if (ids != null)
            {
                string[] idsArray = ids.Split('|');

                this.eventJudgePickerA.SelectedIndex = int.Parse(idsArray[0]);
                this.eventJudgePickerB.SelectedIndex = int.Parse(idsArray[1]);
                this.eventJudgePickerC.SelectedIndex = int.Parse(idsArray[2]);
                this.eventJudgePickerD.SelectedIndex = int.Parse(idsArray[3]);
                this.eventJudgePickerE.SelectedIndex = int.Parse(idsArray[4]);
            }
            else
            {
                this.eventJudgePickerA.SelectedIndex = 0;
                this.eventJudgePickerB.SelectedIndex = 0;
                this.eventJudgePickerC.SelectedIndex = 0;
                this.eventJudgePickerD.SelectedIndex = 0;
                this.eventJudgePickerE.SelectedIndex = 0;
            }


            TournamentGameParticipationsConnection tourparconn = new TournamentGameParticipationsConnection();
            this._DTparticipations = tourparconn.GetParticipation(_gameId).Tables[0];

            _participantA = _DTparticipations.Rows[0][4].ToString();
            _participantB = _DTparticipations.Rows[1][4].ToString();

            string tourid = this._tournament.id;
            Athlete athA = new Athlete(_participantA, tourid);
            Athlete athB = new Athlete(_participantB, tourid);

            this.RedCompetitor.Content = athA.lastName + " " + athA.firstName;
            this.WhiteCompetitor.Content = athB.lastName + " " + athB.firstName;


            //this._sender.Hide();
            this.Show();

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
            this._aClicked = true;

            this.reda.Background = Brushes.Red;
            this.whitea.Background = Brushes.DarkGray;

        }

        private void whitea_Click(object sender, RoutedEventArgs e)
        {
            this._judgeAchooseWhite = true;
            this._aClicked = true;

            this.reda.Background = Brushes.DarkGray;
            this.whitea.Background = Brushes.White;
        }

        private void redb_Click(object sender, RoutedEventArgs e)
        {

            this._judgeBchooseWhite = false;
            this._bClicked = true;

            this.redb.Background = Brushes.Red;
            this.whiteb.Background = Brushes.DarkGray;

        }

        private void whiteb_Click(object sender, RoutedEventArgs e)
        {
            this._judgeBchooseWhite = true;
            this._bClicked = true;

            this.redb.Background = Brushes.DarkGray;
            this.whiteb.Background = Brushes.White;
        }

        private void redc_Click(object sender, RoutedEventArgs e)
        {
            this._judgeCchooseWhite = false;
            this._cClicked = true;

            this.redc.Background = Brushes.Red;
            this.whitec.Background = Brushes.DarkGray;
        }

        private void whitec_Click(object sender, RoutedEventArgs e)
        {
            this._judgeCchooseWhite = true;
            this._cClicked = true;

            this.redc.Background = Brushes.DarkGray;
            this.whitec.Background = Brushes.White;
        }

        private void redd_Click(object sender, RoutedEventArgs e)
        {
            this._judgeDchooseWhite = false;
            this._dClicked = true;

            this.redd.Background = Brushes.Red;
            this.whited.Background = Brushes.DarkGray;
        }

        private void whited_Click(object sender, RoutedEventArgs e)
        {
            this._judgeDchooseWhite = true;
            this._dClicked = true;

            this.redd.Background = Brushes.DarkGray;
            this.whited.Background = Brushes.White;
        }

        private void rede_Click(object sender, RoutedEventArgs e)
        {
            this._judgeEchooseWhite = false;
            this._eClicked = true;

            this.rede.Background = Brushes.Red;
            this.whitee.Background = Brushes.DarkGray;
        }

        private void whitee_Click(object sender, RoutedEventArgs e)
        {
            this._judgeEchooseWhite = true;
            this._eClicked = true;

            this.rede.Background = Brushes.DarkGray;
            this.whitee.Background = Brushes.White;
        }

        #endregion

        #region menu buttons save back etc


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            //
            //  this need testing
            //
            if (_aClicked && _bClicked && _cClicked && _dClicked && _eClicked)
            {
                int whitecount = 0;

                if (_judgeAchooseWhite)
                    whitecount++;

                if (_judgeBchooseWhite)
                    whitecount++;

                if (_judgeCchooseWhite)
                    whitecount++;

                if (_judgeDchooseWhite)
                    whitecount++;

                if (_judgeEchooseWhite)
                    whitecount++;


                EveSupFlagConnection econn = new EveSupFlagConnection();
                if (whitecount > 2)
                {
                    econn.InsertNewflagInd(_gameId, _participantB, _judgeAId, _judgeBId, _judgeCId, _judgeDId, _judgeEId,
                        _judgeAchooseWhite, _judgeBchooseWhite, _judgeCchooseWhite, _judgeDchooseWhite, _judgeEchooseWhite);
                }
                else
                {
                    // NOTE the _judgeAchooseWhite are with !(NOT) that means that if the judge dind't choose the white
                    // site then he automaticaly choose red site
                    econn.InsertNewflagInd(_gameId, _participantA, _judgeAId, _judgeBId, _judgeCId, _judgeDId, _judgeEId,
                        !_judgeAchooseWhite, !_judgeBchooseWhite, !_judgeCchooseWhite, !_judgeDchooseWhite, !_judgeEchooseWhite);
                }

                CoreDatabaseConnection conn = new CoreDatabaseConnection();
                string sql = "update games set is_finished = 'true' where id = '" + this._game.gameId + "'; ";
                conn.NonQuery(sql);


                this._sender.update();
                _sender.Visibility = System.Windows.Visibility.Visible;
                this.Close();
            }
            else
            {
                string result = MessageBox.Show("Please choose all flag colors.", "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _sender.Visibility = System.Windows.Visibility.Visible;
            this.Close();
        }

        #endregion



        #region judge listeners

        private void eventJudgePickerA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeAId = this._DTjudges.Rows[this.eventJudgePickerA.SelectedIndex][0].ToString();

            this.updatejudgemem();
        }

        private void eventJudgePickerB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeBId = this._DTjudges.Rows[this.eventJudgePickerB.SelectedIndex][0].ToString();

            this.updatejudgemem();
        }

        private void eventJudgePickerC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeCId = this._DTjudges.Rows[this.eventJudgePickerC.SelectedIndex][0].ToString();

            this.updatejudgemem();
        }

        private void eventJudgePickerD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeDId = this._DTjudges.Rows[this.eventJudgePickerD.SelectedIndex][0].ToString();

            this.updatejudgemem();
        }

        private void eventJudgePickerE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._judgeEId = this._DTjudges.Rows[this.eventJudgePickerE.SelectedIndex][0].ToString();

            this.updatejudgemem();
        }

        private void updatejudgemem()
        {
            string ids;
            ids = this.eventJudgePickerA.SelectedIndex + "|" + this.eventJudgePickerB.SelectedIndex + "|" + this.eventJudgePickerC.SelectedIndex + "|" + this.eventJudgePickerD.SelectedIndex + "|" + this.eventJudgePickerE.SelectedIndex;
            this.memory.save(@"flag.mem", ids);
        }

        #endregion

        private void Window_close(object sender, EventArgs e)
        {
            this.Close();
            this._sender.Show();
        }

        private void switchSides_Click(object sender, RoutedEventArgs e)
        {
            string temp;


            temp = _participantB;
            _participantB = _participantA;
            _participantA = temp;

            temp = this.RedCompetitor.Content.ToString();
            this.RedCompetitor.Content = this.WhiteCompetitor.Content;
            this.WhiteCompetitor.Content = temp;

        }

    }
}
