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
    /// Interaction logic for ClubManagement.xaml
    /// </summary>
    public partial class ClubManagement : Window
    {
        private string _clubName = null;
        private string _clubPhone = null;
        private string _clubEmail = null;
        private string _clubAddress = null;
        private string _clubAddressNum = null;
        private string _clubTK = null;
        private string _clubCity = null;
        private string _clubCountryCode = null;
        private string _clubLogoSource = null;
        private ClubConnection clubConnection;

        public ClubManagement()
        {
            InitializeComponent();
            clubConnection = new ClubConnection();

        }

       
        
        private void btCBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.gif,*.jpg,*.jpeg,*.bmp,*.png)|*.gif;*.jpg;*.jpeg;*.bmp;*.png";
            dlg.InitialDirectory = "C:\\Users\\Public\\Pictures";
            dlg.Title = "Select image for logo";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                clubLogoPath.Text = filename;
                
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filename, UriKind.Absolute);
                bitmap.EndInit();
                clubLogo.Source = bitmap;
            }
        }

        private void clubLogoPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubLogoSource = clubLogoPath.Text;
        }

        private void clubName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubName = clubName.Text;
        }

        private void clubPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubPhone = clubPhone.Text;
        }

        private void clubEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubEmail = clubEmail.Text;
        }

        private void clubAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubAddress = clubAddress.Text;
        }

        private void clubAddressNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubAddressNum = clubAddressNum.Text;
        }

        private void clubTK_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clubTK = clubTK.Text;
        }

        private void btnCSaveNew_Click(object sender, RoutedEventArgs e)
        {
            clubConnection.InsertNewCLub(_clubName , _clubPhone , _clubEmail , _clubLogoSource , "1" , "CY");
        }

        
    }
}
