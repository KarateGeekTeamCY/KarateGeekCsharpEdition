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
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Window
    {
        private Window sender;

        private databaseConnection.UserConnection conn = new databaseConnection.UserConnection();


        private string userName = "";
        private string pass1 = "";
        private string pass2 = "";

        private bool person = false;
        private bool eventMan = false;
        private bool eventSup = false;
        private bool lottery = false;
        private bool reports = false;
        private bool settings = false;



        public UserManagement(Window sender)
        {
            this.sender = sender;
            this.sender.Hide();
            InitializeComponent();
        }


        #region button listers

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.sender.Show();
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Equals(""))
            {
                string result = MessageBox.Show("You didn't choose any valid user name, please try again.", "No user name!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information).ToString();
                return;
            }

            if (pass1.Equals(""))
            {
                string result = MessageBox.Show("You didn't choose any valid password, please try again.", "No password!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information).ToString();
                return;
            }

            if (pass2.Equals(""))
            {
                string result = MessageBox.Show("You didn't choose any valid password, please try again.", "No password!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information).ToString();
                return;
            }

            if (pass1 != pass2)
            {
                string result = MessageBox.Show("The second password didn't much the firs one, please try again.", "Password mismuch!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information).ToString();
                return;
            }


            conn.insertNewUser(this.userName, this.pass1,
                this.person, this.eventMan, this.lottery, this.eventSup,
                this.reports, this.settings);

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region check box listeners

        private void chbPersonMan_Checked(object sender, RoutedEventArgs e)
        {
            this.person = (bool)this.chbPersonMan.IsChecked;
        }

        private void chbEventMan_Checked(object sender, RoutedEventArgs e)
        {
            this.eventMan = (bool)this.chbEventMan.IsChecked;
        }

        private void chbLoteryMan_Checked(object sender, RoutedEventArgs e)
        {
            this.lottery = (bool)this.chbLoteryMan.IsChecked;
        }

        private void chbEventSup_Checked(object sender, RoutedEventArgs e)
        {
            this.eventSup = (bool)this.chbEventSup.IsChecked;
        }

        private void chbReportsMan_Checked(object sender, RoutedEventArgs e)
        {
            this.reports = (bool)this.chbReportsMan.IsChecked;
        }

        private void chbSettings_Checked(object sender, RoutedEventArgs e)
        {
            this.settings = (bool)this.chbSettings.IsChecked;
        }

        #endregion

        #region text changed listeners

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.userName = this.txtUserName.Text;
        }

        private void txtPasswordOne_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.pass1 = this.txtPasswordOne.Password;
        }

        private void txtPasswordTwo_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.pass2 = this.txtPasswordTwo.Password;
        }

        #endregion
    }
}
