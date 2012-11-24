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
using KarateGeek.lottery;

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for LotteryChooser.xaml
    /// </summary>
    public partial class LotteryChooser : Window
    {
        private DataTable futureUnlotterisedEvents;
        private DataTable futureUnlotterisedTournaments;
        private EventConnection eventConn = new EventConnection();
        private TournamentConnection tournamentConn = new TournamentConnection();
        private int eventId;
        private int tournamentId;
        private LotteryGenerator lg;
        private Window sender;

        public LotteryChooser(Window sender)
        {
            InitializeComponent();

            futureUnlotterisedEvents = eventConn.getUnlotterisedEvents();
            foreach (DataRow dr in futureUnlotterisedEvents.Rows)
            {
                cboEventCombo.Items.Add(dr[1].ToString());
            }

            this.sender = sender;

            string node = "┌──────────────────┐\n"
                        + "│ Athlete 1's name ├──┐\n"
                        + "└──────────────────┘  │\n"
                        + "                      ├──┤\n"
                        + "┌──────────────────┐  │\n"
                        + "│ Athlete 2's name ├──┘\n"
                        + "└──────────────────┘\n";

            //terminal.Content = node;
            terminal.Content = null;
        }

        private void cboEventCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cboEventCombo.SelectedIndex;
            eventId = int.Parse(futureUnlotterisedEvents.Rows[index][0].ToString());

            if (index < cboEventCombo.Items.Count && index != -1)
                futureUnlotterisedTournaments = tournamentConn.getUnlotterisedTournaments(eventId);
            foreach (DataRow dr in futureUnlotterisedTournaments.Rows)
            {
                cboTournamentCombo.Items.Add(dr[1].ToString());
            }

        }


        private void cboTournamentCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cboTournamentCombo.SelectedIndex;
            tournamentId = int.Parse(futureUnlotterisedTournaments.Rows[index][0].ToString());

            lg = LotteryGeneratorFactory.Create(tournamentId);
            lg.shuffle();

            /* experimental (and totally, totally broken in most cases): */
            terminal.Content = new LotteryPrinter(lg.getLottery(), isListOfTeams: false).ToString();
        }

        private void btnShuffle_Click(object sender, RoutedEventArgs e)
        {
            lg.shuffle();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            lg.confirmLottery(doCommit: true);

            //temp
            new KarateGeek.databaseConnection.LotteryGenConnection().printTournamentGameTableWithNames(tournamentId);

            LotteryChooser lc = new LotteryChooser(this.sender);
            lc.Activate();
            this.Close();
            lc.Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.sender.Show();
            this.Close();
        }
    }
}
