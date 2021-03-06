﻿using System;
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

using KarateGeek.helpers; // for lottery generator testing; THIS LINE WILL BE REMOVED

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void btnPersonManagement_Click(object sender, RoutedEventArgs e)
        {
            PersonManagement pm = new PersonManagement();
            pm.Activate();

            this.Close();
            pm.Show();
            
        }

        private void btnEventmanagement_Click(object sender, RoutedEventArgs e)
        {
            EventTournamentManagement et = new EventTournamentManagement();
            et.Activate();
            this.Close();
            et.Show();
        }

        private void btnClubmanagement_Click(object sender, RoutedEventArgs e)
        {
            ClubManagement cl = new ClubManagement();
            cl.Activate();

            this.Close();
            cl.Show();
        }

       

        private void mainCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = mainCalendar.SelectedDate.Value;

            EventTournamentManagement et = new EventTournamentManagement(selectedDate);
            et.Activate();
            this.Close();
            et.Show();
        }

        /* EXPERIMENTAL and not ready, will crash 'n' burn if clicked:*/
        private void btnLotery_Click(object sender, RoutedEventArgs e)
        {
            LotteryGenerator lg = new LotteryGenerator(1);
        }

        
    }
}
