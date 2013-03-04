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

using KarateGeek.lottery;
using KarateGeek.databaseConnection;

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for LotteryGraph.xaml
    /// </summary>
    public partial class LotteryGraph : Window
    {
        private long tournamentId;

        private bool teamKumiteCheckEnabled = false;

        private const double ASCIIGraphMinFontSize =  6;
        private const double ASCIIGraphDefFontSize = 12;    // size 12 works well for many screens
        private const double ASCIIGraphMaxFontSize = 36;

        private double ASCIIGraphFontSize {                 // within permitted font size range 
            get {
                  return terminal.FontSize;
                }
            set {
                  terminal.FontSize = (value < ASCIIGraphMinFontSize) ? ASCIIGraphMinFontSize :
                                      (value > ASCIIGraphMaxFontSize) ? ASCIIGraphMaxFontSize :
                                      value;
                }
        }


        public LotteryGraph(long tournamentId)
        {
            InitializeComponent();

            this.tournamentId = tournamentId;

            /** DO NOT EVER USE THIS CODE AGAIN! AAAAAAAAARGH! THIS IS TOTALLY WRONG; JUST CALL THE OVERLOADED LotteryPrinter(long tournamentId) CONSTRUCTOR! */
            /* using KarateGeek.databaseConnection; */
            //var sets = new LotteryPrinterConnection().getPrintableLotterySetsFromDB(tournamentId);

            //terminal.Content = "_\n" + new LotteryPrinter(sets, tournamentId).ToString();
            /** !! */

            this.Title = "Tournament progress"; // ??

            if (Strings.teamKumite.Equals(new LotteryGenConnection().getTournamentGameType(this.tournamentId), StringComparison.Ordinal))
                teamKumiteCheckEnabled = true;
            else
                this.Show();

            this.updateGraph();
        }


        public void updateGraph()
        {
            if (teamKumiteCheckEnabled) { // else Show()n in the constructor
                if (!new LotteryPrinterConnection().hasEnoughElementsToPrint(this.tournamentId))  // prevent crashes in the Team Kumite case
                    return;
                else {
                    try {
                        this.Show();     // the red X button should be overridden so that it calls this.Hide() instead of this.Close()          
                    } catch {
                    }
                }
            }

            // Prepending "_\n" is a workaround for a .NET bug (see the comment in the class
            // LotteryPrinter). The newline char ensures that, if it ever gets fixed, our code won't break:
            terminal.Content = "_\n" + new LotteryPrinter(this.tournamentId).ToString();
            this.Activate();    // might be annoying for the user?
        }


        /** TODO: Remove this overload, it's here only for testing/comparison purposes... */
        public void updateGraph(string content, string title = "Displaying externally provided content (for comparison)")   // overloaded
        {
            this.Title = title;
            terminal.Content = content;
            this.Activate();
        }


        private void terminal_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                double temp = this.treeView.ContentVerticalOffset;

                if (e.Delta < 0)
                    --ASCIIGraphFontSize;
                else
                    ++ASCIIGraphFontSize;

                this.treeView.ScrollToVerticalOffset(temp);
            }
        }
    }
}
