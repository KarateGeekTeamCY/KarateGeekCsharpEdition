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

        public LogIn()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            LoginConnection lgConn = new LoginConnection();

            /* Temporarily disabling the following two lines to make our life easier during testing: */
            //username=userNameTB.Text;
            //password=passwordTB.Password;
            username = "admin";
            password=  "admin";
            /* end of modification */

            loginCheck = lgConn.compare(username, password);
            if (loginCheck)
            {
                MainMenu menu = new MainMenu();
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
    }
}
