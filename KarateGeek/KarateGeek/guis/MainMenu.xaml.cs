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
using System.Diagnostics;   // has Debug.WriteLine()
using System.Data;
using KarateGeek.databaseConnection;

using KarateGeek.lottery; // for lottery generator testing; THIS LINE WILL BE REMOVED

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


            //myCalendar.SelectedDates.Add(CType(reader.GetDateTime(0), Date))
            EventConnection evCon = new EventConnection();
            DataTable dt = evCon.getEvents().Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                this.mainCalendar.SelectedDates.Add((DateTime)dr[2]);
            }

            this.mainCalendar.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>(mainCalendar_SelectedDatesChanged);

            //this.mainCalendar.
        }

        private void btnPersonManagement_Click(object sender, RoutedEventArgs e)
        {
            PersonManagement pm = new PersonManagement(this);
            pm.Activate();

            this.Hide();
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
            DataTable ds;
            EventConnection eventConnection = new EventConnection();
            DateTime selectedDate = mainCalendar.SelectedDate.Value;

            ds = eventConnection.getEventsBydate(selectedDate.ToString("yyyy-M-d")).Tables[0];

            if (ds.Rows.Count == 0)
            {
                EventTournamentManagement et = new EventTournamentManagement(selectedDate);
                et.Activate();
                this.Close();
                et.Show();
            }
            else
            {
                EventTournamentManagement et = new EventTournamentManagement(ds.Rows[0][1].ToString());
                et.Activate();
                et.eSuggestionList.SelectedIndex = 0;
                this.Close();
                et.Show();
            }

        }

        /* EXPERIMENTAL and not ready, might crash 'n' burn if clicked:*/
        /* Do not just remove this code; it's a usage example for the lottery class. */
        private void btnLottery_Click(object sender, RoutedEventArgs e)
        {
            LotteryChooser lc = new LotteryChooser();
            lc.Activate();
            this.Hide();
            lc.Show();
            //LotteryGenerator lg = LotteryGeneratorFactory.Create(tournamentId: 1); // use 1 to test kata

            //// Some silly test code:
            //Debug.WriteLine("Our first try:");
            //lg.shuffle();
            //foreach(var element in lg.getLottery())
            //    Debug.WriteLine("Athlete number: " + element);

            //Debug.WriteLine("Our second try:");
            //lg.shuffle();
            //foreach (var element in lg.getLottery())
            //    Debug.WriteLine("Athlete number: " + element);

            //Debug.WriteLine("Our third try, randomise MORE:");
            //lg.randomisationFactor = 4000; //!!
            //lg.shuffle();
            //foreach (var element in lg.getLottery())
            //    Debug.WriteLine("Athlete number: " + element);

            //// This might write to the DB:
            ////lg.confirmLottery(doCommit: true);   // this will commit
            ////lg.confirmLottery(doCommit: false);  // this will rollback (default value)
            //lg.confirmLottery();

            //// printing lottery to the console, now from the database (THIS IS JUST AN EXAMPLE/TEST):
            //new LotteryGenConnection().printTournamentGameTable(tournamentId: 1);
            //new LotteryGenConnection().printTournamentGameTableWithNames(tournamentId: 1);
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {

            KarateGeek.Reports.ReportsForm reports = new Reports.ReportsForm();
            reports.Show();

        }

        private void btnEventSupport_Click(object sender, RoutedEventArgs e)
        {
            EventSupport eventSup = new EventSupport(this);

        }

        private void btnUserManagement_Click(object sender, RoutedEventArgs e)
        {
            UserManagement userMan = new UserManagement(this);
            userMan.Show();
        }





    }
}
