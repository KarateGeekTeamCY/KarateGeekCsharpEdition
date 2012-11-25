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
    /// Interaction logic for YesNoCancelReplacement.xaml
    /// </summary>
    public partial class YesNoCancelReplacement : Window
    {
        KumiteSystem sender;

        public YesNoCancelReplacement(KumiteSystem sender, string title, string content)
        {
            InitializeComponent();

            this.sender = sender;
            this.Title = title;
            this.lblQuestion.Content = content;

            this.Topmost = true;
            this.Focus();
            this.Activate();
            this.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            this.sender.yncListener("delete");
            this.Close();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            this.sender.yncListener("change");
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.sender.yncListener("cancel");
            this.Close();
        }

    }
}
