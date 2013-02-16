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
using KarateGeek.lottery;

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for LotteryGraph.xaml
    /// </summary>
    public partial class LotteryGraph : Window
    {

        private const double ASCIIGraphMinFontSize = 6;
        private const double ASCIIGraphDefFontSize = 12;    // size 12 works well for many screens
        private const double ASCIIGraphMaxFontSize = 36;

        public LotteryGraph(long tournamentId)
        {
            InitializeComponent();
            var sets = new LotteryPrinterConnection().getPrintableLotterySetsFromDB(tournamentId);
            
            // Prepending "_\n" is a workaround for a .NET bug (see the comment in the class
            // LotteryPrinter). The newline char ensures that, if it ever gets fixed, our code won't break:
            terminal.Content = "_\n" + new LotteryPrinter(sets, tournamentId).ToString();
        }

        private double ASCIIGraphFontSize {                 // within permitted font size range 
            get
            {
                return terminal.FontSize;
            }
            set
            {
                terminal.FontSize = (value < ASCIIGraphMinFontSize) ? ASCIIGraphMinFontSize :
                                    (value > ASCIIGraphMaxFontSize) ? ASCIIGraphMaxFontSize :
                                    value;
            }
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
