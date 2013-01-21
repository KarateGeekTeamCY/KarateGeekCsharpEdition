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
using KarateGeek.databaseConnection;

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private string username; 
        private string password;
        private Boolean loginCheck;
        private LoginConnection lgConn;
        private MainMenu menu;

        public LogIn()
        {
            InitializeComponent();

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            //
            //
            //
            this.host.Text = CoreDatabaseConnection.host;

            
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            lgConn = new LoginConnection();

            /* Temporarily disabling the following two lines to make our life easier during testing: */
            //username=userNameTB.Text;
            //password=passwordTB.Password;
            username = "admin";
            password=  "admin";
            /* end of modification */

            loginCheck = lgConn.compare(username, password);
            if (loginCheck)
            {
                menu = new MainMenu();
                checkRights();
                menu.Activate();
                this.Close();
                menu.Show();
            }
            else
            {
                MessageBox.Show("Username or password wrong!");
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void checkRights(){
            if (lgConn.getPersonMngPerm())
            {
                menu.btnPersonManagement.IsEnabled = true;
            }
            if (lgConn.getEventMngPerm())
            {
                menu.btnEventmanagement.IsEnabled = true;
            }
            if (lgConn.getLotteryPerm())
            {
                menu.btnLottery.IsEnabled = true;
            }
            if (lgConn.getEventSupPerm())
            {
                menu.btnEventSupport.IsEnabled = true;
            }
            if (lgConn.getClubMngPerm())
            {
                menu.btnClubmanagement.IsEnabled = true;
            }
            if (lgConn.getUserMngPerm())
            {
                menu.btnUserManagement.IsEnabled = true;
            }
            if (lgConn.getReportsPerm())
            {
                menu.btnReports.IsEnabled = true;
            }
        }

        private void host_TextChanged(object sender, TextChangedEventArgs e)
        {
            CoreDatabaseConnection.host = this.host.Text;
        }

        
    }
}
