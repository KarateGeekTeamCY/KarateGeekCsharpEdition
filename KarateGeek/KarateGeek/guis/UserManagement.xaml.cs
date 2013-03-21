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
using System.Data;

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Window
    {
        private Window sender;

      //  private databaseConnection.UserConnection conn = new databaseConnection.UserConnection();


        private string userName = "";
        private string pass1 = "";
        private string pass2 = "";

        private int userId=-1;
        private bool personMan = false;
        private bool eventMan = false;
        private bool lottery = false;
        private bool eventSup = false;
        private bool clubMan = false;
        private bool userMan = false;
        private bool reports = false;
        
        private DataTable filteredUsers;
        private List<ListData> userNameListForAutoComplete;
        private UserConnection userConnection = new UserConnection();


        public UserManagement(Window sender)
        {
            this.sender = sender;
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
            }else{
                userConnection.updateUser(userId, this.userName, this.pass1,
                this.personMan, this.eventMan, this.lottery, this.eventSup,
                this.clubMan, this.userMan, this.reports);

                MessageBox.Show("Succesfully updated!");
            }
        }

        private void btnSaveAsNew_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Equals(""))
            {
                string result = MessageBox.Show("You didn't choose any valid user name, please try again.", "No user name!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information).ToString();
                return;
            }

            if (pass1.Equals("") || pass2.Equals(""))
            {
                string result = MessageBox.Show("You didn't choose any valid password, please try again.", "No password!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information).ToString();
                return;
            }

            if (pass1 != pass2)
            {
                string result = MessageBox.Show("The second password didn't much the first one, please try again.", "Password mismuch!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information).ToString();
                return;
            }
            else {
                userConnection.insertNewUser(this.userName, this.pass1,
                this.personMan, this.eventMan, this.lottery, this.eventSup,
                this.clubMan , this.userMan , this.reports);
                MessageBox.Show("Succesfully saved!");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (userId != -1)
            {
                userConnection.deleteUser(userId);
                MessageBox.Show("Succesfully deleted!");
            }
            else
            {
                MessageBox.Show("Select one user to delete!");
            }
        }

        #endregion

        #region check box listeners

        private void chbPersonMan_Checked(object sender, RoutedEventArgs e)
        {
            this.personMan = (bool)this.chbPersonMan.IsChecked;
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

        private void chbClubMan_Checked(object sender, RoutedEventArgs e)
        {
            this.clubMan = (bool)this.chbClubMan.IsChecked;
        }

        private void chbUserMan_Checked(object sender, RoutedEventArgs e)
        {
            this.userMan = (bool)this.chbUserMan.IsChecked;
        }

        private void chbReportsMan_Checked(object sender, RoutedEventArgs e)
        {
            this.reports = (bool)this.chbReportsMan.IsChecked;
        }

       

        #endregion

        #region text changed listeners

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.userName = this.txtUserName.Text;
            List<ListData> autoList = new List<ListData>();
            autoList.Clear();

            userNameListForAutoComplete = this.UserfilterNames();

            foreach (ListData item in userNameListForAutoComplete)
            {
                 autoList.Add(item);   
            }

            if (autoList.Count > 0)
            {
                uSuggestionList.DataContext = autoList;
                uSuggestionList.Visibility = System.Windows.Visibility.Visible;
            }
            else if (txtUserName.Text.Equals(""))
            {
                uSuggestionList.Visibility = Visibility.Collapsed;
                uSuggestionList.DataContext = null;
            }
            else
            {
                uSuggestionList.Visibility = Visibility.Collapsed;
                uSuggestionList.DataContext = null;
            }
        }

        private List<ListData> UserfilterNames()
        {
            List<ListData> list = new List<ListData>();
     
            this.filteredUsers = userConnection.findSimilar(this.txtUserName.Text);

            foreach (DataRow dr in filteredUsers.Rows)
            {
                ListData suggestion = new ListData();
                suggestion.id = int.Parse(dr[0].ToString());
                suggestion.name = dr[1].ToString();
                list.Add(suggestion);
            }
            return list;
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Visible;
            //this.sugestionList.ItemsSource = list;
        
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

        private void uSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index;
            if (uSuggestionList.ItemsSource != null)
            {
                uSuggestionList.Visibility = System.Windows.Visibility.Collapsed;
                txtUserName.TextChanged -= new TextChangedEventHandler(txtUserName_TextChanged);

                index = uSuggestionList.SelectedIndex;

                if (uSuggestionList.SelectedIndex != -1)
                {
                    ListData item = (ListData)uSuggestionList.SelectedItem;
                    userId = item.id;
                    userName = item.name;
                    
                    this.txtUserName.Text = filteredUsers.Rows[index][1].ToString();

                    this.chbPersonMan.IsChecked = (bool)filteredUsers.Rows[index][3];
                    this.chbEventMan.IsChecked = (bool)filteredUsers.Rows[index][4];
                    this.chbLoteryMan.IsChecked = (bool)filteredUsers.Rows[index][5];
                    this.chbEventSup.IsChecked = (bool)filteredUsers.Rows[index][6];
                    this.chbClubMan.IsChecked = (bool)filteredUsers.Rows[index][7];
                    this.chbUserMan.IsChecked = (bool)filteredUsers.Rows[index][8];
                    this.chbReportsMan.IsChecked = (bool)filteredUsers.Rows[index][9];                         
                }
                txtUserName.TextChanged += new TextChangedEventHandler(txtUserName_TextChanged);
            }
            //this.sugestioListScroler.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Window_close(object sender, EventArgs e)
        {
            this.Close();
            this.sender.Show();
        }

       
        
    }
}
