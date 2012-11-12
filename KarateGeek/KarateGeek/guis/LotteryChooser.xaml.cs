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

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for LotteryChooser.xaml
    /// </summary>
    public partial class LotteryChooser : Window
    {
        public LotteryChooser()
        {
            InitializeComponent();
            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.Height = new GridLength(45);
            RowDefinition gridRow2 = new RowDefinition();
            gridRow2.Height = new GridLength(45);
            RowDefinition gridRow3 = new RowDefinition();
            gridRow3.Height = new GridLength(45);

            content.RowDefinitions.Add(gridRow1);
            content.RowDefinitions.Add(gridRow2);
            content.RowDefinitions.Add(gridRow3);

            TextBox txtBox = new TextBox();
            txtBox.Text = "Author Name";
            txtBox.FontSize = 14;
            txtBox.FontWeight = FontWeights.Bold;
            txtBox.Foreground = new SolidColorBrush(Colors.Green);
            txtBox.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(txtBox, 0);
            Grid.SetColumn(txtBox, 1);

            content.Children.Add(txtBox);
        }
    }
}
