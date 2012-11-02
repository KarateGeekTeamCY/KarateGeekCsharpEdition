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
    /// Interaction logic for KumiteSystem.xaml
    /// </summary>
    public partial class KumiteSystem : Window
    {

        #region private declaretions

        private int pointsIndex = 0;
        private LinkedList<Point> pointsLeft = new LinkedList<Point>();
        private LinkedList<Point> pointsRight = new LinkedList<Point>();





        #endregion 



        public KumiteSystem()
        {
            InitializeComponent();
        }


        #region point buttons


        private void ipponA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.ippon, 1, pointsIndex);
            pointsIndex++;
            this.pointsLeft.AddLast(p);

        }

        private void wazaariA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.wazaari, 1, pointsIndex);
            pointsIndex++;
            this.pointsLeft.AddLast(p);
        }

        private void jyogaiA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.jyogai, 1, pointsIndex);
            pointsIndex++;
            this.pointsLeft.AddLast(p);
        }

        private void chuiA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.chui, 1, pointsIndex);
            pointsIndex++;
            this.pointsLeft.AddLast(p);
        }

        private void keikokuA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.keikoku, 1, pointsIndex);
            pointsIndex++;
            this.pointsLeft.AddLast(p);
        }

        private void tentoA_Click(object sender, RoutedEventArgs e)
        {
            Point p = new Point(Strings.tento, 1, pointsIndex);
            pointsIndex++;
            this.pointsLeft.AddLast(p);
        }

        private void doctorStopA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ipponB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void wazaariB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void jyogaiB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chuiB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void keikokuB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tentoB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void doctorstopB_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

    }


    public class Point
    {

        public string descreption { get; set; }
        public int points { get; set; }
        public int pointsIndex { get; set; }

        public Point(string dsc, int points, int PointIndex)
        {
            this.descreption = dsc;
            this.points = points;
            this.pointsIndex = pointsIndex;
        }
        
    }



}
