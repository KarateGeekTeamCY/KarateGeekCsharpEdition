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
        
        

        private TournamentConnection _tournamentConn;
        private TournamentGameParticipationsConnection _tournamentParticipationConn;
        private EventConnection _eventConn;
        private GameConnection _gameConn;

        private DataTable _TournamantsDT;
        private DataTable _currentGamesDT;
        private DataTable _futureGamesDT;
        private DataTable _currentGameParticipationsDT;
        private DataTable _futureGameParticipationsDT;

        private List<string> _availableTournaments;
        private List<string> _currentGames;
        private List<string> _futureGames;
        private List<List<string>> _currentGameParticipants;
        private List<List<string>> _futureGameParticipants;


        #endregion private declaretions


        public EventSupport(Window sender)
        {
            
            InitializeComponent();
            this._sender = sender;


            DateTime date = DateTime.Today;
            string datestring = date.ToString("yyyy-M-d");

            _eventConn = new EventConnection();

            DataSet todayEventDS = _eventConn.getEventsBydate( datestring);
            DataTable todayEventDT = todayEventDS.Tables[0];

            if (todayEventDT.Rows.Count == 0)
            {
                string result = MessageBox.Show("Unfortunately there is no scheduled event for today. Press OK to return to main menu.", "No event for today.",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();

                //this._sender.Show();
                this.Close();
                return;
            }
            else {
                this.Show();
                this._sender.Hide();

                this._eventId = todayEventDT.Rows[0][0].ToString();
            }

            _tournamentConn = new TournamentConnection();
            this._TournamantsDT = this._tournamentConn.findSimilar("", int.Parse(this._eventId) ).Tables[0];

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


        //#region gui elements
        
        private void cboTurnamentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = this.cboTurnamentSelector.SelectedIndex;
            this._tournamentId = _TournamantsDT.Rows[i][0].ToString();
            
            this._gameType =  (string)_TournamantsDT.Rows[i][7];
            string [] type = _gameType.Split('|');
            
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

            this._currentGamesDT = this._gameConn.GetGamesByTurnamentPhase( _tournamentId, "" + currentIndex).Tables[0];
            this._futureGamesDT = this._gameConn.GetGamesByTurnamentPhase(_tournamentId, "" + futureIndex).Tables[0];









        
        }





    }
}
