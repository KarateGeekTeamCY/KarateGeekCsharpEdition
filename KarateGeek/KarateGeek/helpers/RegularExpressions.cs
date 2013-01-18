using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;


namespace KarateGeek.helpers
{
    class RegularExpressions
    {
        public bool isEmailValid(string email)
        {
            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Please enter a valid email");
                return false;
            }
        }

        public bool isCharsOnly(string str)
        {
            Regex regex = new Regex(@"^[a-zA-Z]+$");
            Match match = regex.Match(str);
            if (match.Success)
                return true;
            else
                return false;
        }

        public bool isCharsOrSpace(string str)
        {
            Regex regex = new Regex(@"^[a-zA-Z ]+$");
            Match match = regex.Match(str);
            if (match.Success)
                return true;
            else
                return false;
        }

        public bool isDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public bool isCharsOrDigits(string str)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
            Match match = regex.Match(str);
            if (match.Success)
                return true;
            else
                return false;
        }

        public bool isCharsDigitsOrSpace(string str)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9 ]+$");
            Match match = regex.Match(str);
            if (match.Success)
                return true;
            else
                return false;
        }
    }
}
