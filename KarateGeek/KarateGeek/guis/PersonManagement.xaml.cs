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
    /// Interaction logic for PersonManagement.xaml
    /// </summary>
    public partial class PersonManagement : Window
    {
        public PersonManagement()
        {
            InitializeComponent();
        }


        //
        // this will show the available persons in athlete management tab
        //
        private void athleteFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
        
            List<string> items = new List<string>();

            items.Add("athlete one");
            items.Add("athlete two");

            this.availablePersonsList.Visibility = System.Windows.Visibility.Visible;
            this.availablePersonsList.ItemsSource = items;

        }

        private void availablePersonsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Object[] selected = (Object[])e.AddedItems;

            this.availablePersonsList.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
