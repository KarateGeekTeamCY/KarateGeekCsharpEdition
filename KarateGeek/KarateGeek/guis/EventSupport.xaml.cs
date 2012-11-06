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

        private int  _tournamentId;

        private bool _isTeam;
        private string _gameType;
        private string judgingType;
        private TournamentConnection _tournamentConn;
        private TournamentGameParticipationsConnection _tournamentParticipationConn;
        private EventConnection _eventConn;
        private List<string> _todaysEventTournaments = new List<string>();




        #endregion private declaretions




        public EventSupport()
        {
            InitializeComponent();
            







        }
        





        #region buttons
        private void btnStartNextGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}
