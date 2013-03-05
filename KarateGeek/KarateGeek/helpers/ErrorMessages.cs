using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace KarateGeek.helpers
{
    class ErrorMessages
    {

        public void nullErrorMessage(string formField)
        {
            string message = "Please complete " + formField;
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }

        public void charsErrorMessage(string formField)
        {
            string message = "Please enter only Characters in the " + formField + " field";
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }

        public void digitsErrorMessage(string formField)
        {
            string message = "Please enter only Digits in the " + formField + " field";
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }

        public void charsDigitsErrorMessage(string formField)
        {
            string message = "Please enter Characters and Digits in the " + formField + " field";
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }

        public void charsSpaceErrorMessage(string formField)
        {
            string message = "Please enter Characters or spaces in the " + formField + " field";
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }

        public void charsSpaceDotsErrorMessage(string formField)
        {
            string message = "Please enter Characters, Spaces or Dots in the " + formField + " field";
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }

        public void charsSpaceDigitsErrorMessage(string formField)
        {
            string message = "Please enter Characters,Digits and spaces " + formField + " field";
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }

        public void errorMessage(string formField)
        {
            string message = "Please complete " + formField;
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }

        static public void menuSelectionErrorMessage(string formField)
        {
            string message = "Please choose a " + formField + " first.";
            MessageBox.Show(message, "Message!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();
        }
    }
}
