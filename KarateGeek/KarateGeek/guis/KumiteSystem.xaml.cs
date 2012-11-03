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
//using System.Windows.Forms;


namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for KumiteSystem.xaml
    /// </summary>
    public partial class KumiteSystem : Window
    {

        #region private declaretions

        private int pointsIndex = 0;
        private LinkedList<Point> _pointsHistory = new LinkedList<Point>();
        //private LinkedList<Point> pointsRight = new LinkedList<Point>();





        #endregion



        public KumiteSystem()
        {
            InitializeComponent();
        }


        #region point buttons


        private void ipponA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.ippon, 1, pointsIndex, "left");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();

        }

        private void wazaariA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.wazaari, 1, pointsIndex, "left");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();
        }

        private void jyogaiA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.jyogai, 1, pointsIndex, "left");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();
        }

        private void chuiA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.chui, 1, pointsIndex, "left");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();
        }

        private void keikokuA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.keikoku, 1, pointsIndex, "left");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();
        }

        private void tentoA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.tento, 1, pointsIndex, "left");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();
        }

        private void doctorStopA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ipponB_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.ippon, 1, pointsIndex, "right");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();

        }

        private void wazaariB_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.wazaari, 1, pointsIndex, "right");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();
        }

        private void jyogaiB_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.jyogai, 1, pointsIndex, "right");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();
        }

        private void chuiB_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.chui, 1, pointsIndex, "right");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();
        }

        private void keikokuB_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.keikoku, 1, pointsIndex, "right");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();
        }

        private void tentoB_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.tento, 1, pointsIndex, "right");
            pointsIndex++;
            this._pointsHistory.AddLast(p);

            updateHistory();
        }

        private void doctorstopB_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion


        private void updateHistory()
        {
            StringBuilder sb = new StringBuilder();
            bool endFlag = false;
            int historyIndex = 0;

            sb.Append("");

            while (!endFlag)
            {

                sb.Append(_pointsHistory.ElementAt(historyIndex).time);
                sb.Append(": ");

                if (_pointsHistory.ElementAt(historyIndex).side == "right")
                    sb.Append("Comp. B -> ");
                else
                    sb.Append("Comp. A -> ");

                sb.Append(_pointsHistory.ElementAt(historyIndex).descreption);
                sb.Append("\n");
                historyIndex++;

            }
            this.listBoxHistory.Items.Add(sb.ToString());
        }

        private void checkWinner()
        {



        }

        private void listBoxHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string result = MessageBox.Show("Do you want to delete this record? If the answer is NO then press No to replace the record or Cancel to continue normally", "Important.", 
                MessageBoxButton.YesNoCancel ,
                MessageBoxImage.Question).ToString();

            if (result == "yes")
            { 
            
            
            }
            
            if( result == "no")
            {
            
            
            }
        }

    }


    public class Point
    {

        public string descreption { get; set; }
        public int points { get; set; }
        public int pointsIndex { get; set; }
        public string time { get; set; }
        public string side { get; set; }

        public Point(string dsc, int points, int PointIndex, string side)
        {
            this.descreption = dsc;
            this.points = points;
            this.pointsIndex = pointsIndex;
            this.time = DateTime.Now.ToString("HH:mm:ss tt");
            this.side = side;
        }

    }


}
