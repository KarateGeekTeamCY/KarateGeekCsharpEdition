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
using System.Collections;
//using System.Windows.Forms;


namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for KumiteSystem.xaml
    /// </summary>
    /// 

    public partial class KumiteSystem : Window
    {

        #region private declarations

        private string _gameId;
        private string _leftAthleteId = null;
        private string _rightAthleteId = null;
        private string _leftAthleteTeamId = null;
        private string _rightAthleteTeamId = null;



        private int pointsIndex = 0;
        private List<Point> _pointsHistory = new List<Point>();

        private bool replaceFlag = false;
        private int toReplaceIndex = -1;
        private Point toReplacePoint;
        private Game game;
        private int scoreLeft = 0;
        private int scoreRight = 0;
        private EventSupport _sender;



        #endregion  private declarations



        public KumiteSystem(EventSupport sender, Game game)
        {

            InitializeComponent();
            this.game = game;
            this._sender = sender;
            //this._gameId = gameId;
            this._LoadData();
            this.Show();
        }

        private void _LoadData()
        {
            this.competitorA.Text = this.game.participants.ElementAt(0).firstName + " " + this.game.participants.ElementAt(0).lastName;
            this.competitorB.Text = this.game.participants.ElementAt(1).firstName + " " + this.game.participants.ElementAt(1).lastName;
            this._gameId = this.game.gameId;

            this._leftAthleteId = this.game.participants.ElementAt(0).id;
            this._rightAthleteId = this.game.participants.ElementAt(1).id;

            this._leftAthleteTeamId = this.game.participants.ElementAt(0).teamId;
            this._rightAthleteTeamId = this.game.participants.ElementAt(1).teamId;
        }




        #region point buttons

        private void addPointToHistory(string pointDSC, int point, string site)
        {
            Point p = new Point(pointDSC, point, pointsIndex, site);
            pointsIndex++;

            if (replaceFlag)
                this.toReplacePoint.update(pointDSC, point, site);
            else
                this._pointsHistory.Add(p);

            if (this.replaceFlag)
                this.replaceFlag = false;

            updateHistory();

        }

        //
        //  LEFT SITE Athilete
        //

        private void ipponA_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.ippon, 10, Strings.left);
        }

        private void wazaariA_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.wazaari, 4, Strings.left);
        }

        private void jyogaiA_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.jyogai, 2, Strings.left);
        }

        private void chuiA_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.chui, 4, Strings.right);
        }

        private void keikokuA_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.keikoku, 2, Strings.right);
        }

        private void tentoA_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.tento, 1, Strings.right);
        }

        private void doctorStopA_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.doctorStop, 8, Strings.right);
        }

        //
        //  RIGHT SITE Athilete
        //

        private void ipponB_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.ippon, 10, Strings.right);

        }

        private void wazaariB_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.wazaari, 4, Strings.right);
        }

        private void jyogaiB_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.jyogai, 2, Strings.right);
        }

        private void chuiB_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.chui, 4, Strings.left);
        }

        private void keikokuB_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.keikoku, 2, Strings.left);
        }

        private void tentoB_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.tento, 1, Strings.left);
        }

        private void doctorstopB_Click(object sender, RoutedEventArgs e)
        {
            addPointToHistory(Strings.doctorStop, 8, Strings.left);
        }

        #endregion



        private void updateHistory()
        {
            int historyIndex = 0;

            List<string> temp = new List<string>();

            scoreLeft = 0;
            scoreRight = 0;

            foreach (Point p in _pointsHistory)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(p.time);
                sb.Append(": ");


                if (p.side == Strings.right)
                {
                    scoreRight += p.points;
                    sb.Append("Comp. B -> ");
                }
                else // if (p.side == Strings.left)
                {
                    scoreLeft += p.points;
                    sb.Append("Comp. A -> ");
                }

                if (scoreLeft >= 8 )
                {
                    Style red =  new Style { TargetType = typeof(TextBox) };
                    red.Setters.Add(new Setter(TextBox.BackgroundProperty, Brushes.Red));

                    scoreA.Style = red;              
                }

                if (scoreRight >= 8)
                {
                    Style red = new Style { TargetType = typeof(TextBox) };
                    red.Setters.Add(new Setter(TextBox.BackgroundProperty, Brushes.Red));

                    scoreB.Style = red;
                
                }


                sb.Append(p.description);
                sb.Append("\n");
                scoreA.Text = scoreLeft.ToString();
                scoreB.Text = scoreRight.ToString();
                historyIndex++;
                temp.Add(sb.ToString());
            }

            this.listBoxHistory.ItemsSource = temp;
        }



        private void listBoxHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.toReplaceIndex = this.listBoxHistory.SelectedIndex;

            if (toReplaceIndex != -1)
            {
                this.toReplacePoint = this._pointsHistory.ElementAt(toReplaceIndex);

                Window ync = new YesNoCancelReplacement(this, title: "", content: "Please choose an action:");
                ync.Activate();
                ync.Show();
            }
        }



        public void yncListener(string ans)
        {
            switch (ans)
            {
                case "delete":
                    this._pointsHistory.Remove(toReplacePoint);
                    this.updateHistory();
                    break;
                case "change":
                    this.replaceFlag = true;
                    break;
                case "cancel":
                default: break;
            }
        }


        #region management buttons

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this._sender.Show();
            this.Close();
        }
        


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            bool ist = (new Tournament(this.game.tournamentId).isTeam);

            if (ist)
            {
                insertPointToDB(_leftAthleteId, _leftAthleteTeamId, new Point("init", 0, pointsIndex, Strings.left));
                insertPointToDB(_rightAthleteId, _rightAthleteTeamId, new Point("init", 0, pointsIndex, Strings.right));
            }
            else
            {
                insertPointToDB(_leftAthleteId, new Point("init", 0, pointsIndex, Strings.left));
                insertPointToDB(_rightAthleteId, new Point("init", 0, pointsIndex, Strings.right));
            
            }

            foreach (Point p in _pointsHistory)
            {

                if (p.side == Strings.left)
                {
                    if (_leftAthleteTeamId == "")
                        insertPointToDB(_leftAthleteId, p);
                    else
                        insertPointToDB(_leftAthleteId, _leftAthleteTeamId, p);
                }
                else // if (p.side == Strings.left)
                {
                    if (_rightAthleteTeamId == "")
                        insertPointToDB(_rightAthleteId, p);
                    else
                        insertPointToDB(_rightAthleteId, _rightAthleteTeamId, p);
                }
            }


            this.game.isFinished = true;


            this._sender.tournament.load();
            this._sender.advanceAthletes();
            this._sender.tournament.load();

            this._sender.loadGames();

            this.Close();
        }



        private void insertPointToDB(string athId, string teamid, Point p)
        {
            databaseConnection.CoreDatabaseConnection conn = new databaseConnection.CoreDatabaseConnection();

            conn.NonQuery("INSERT INTO game_points (game_id , athlete_id , team_id , technical_point , technical_point_desc) VALUES ("
                + _gameId + ","
                + athId + ", "
                + teamid + ","
                + p.points + ", '"
                + p.description + "'); ");
        }



        private void insertPointToDB(string athId, Point p)
        {
            databaseConnection.CoreDatabaseConnection conn = new databaseConnection.CoreDatabaseConnection();

            conn.NonQuery("INSERT INTO game_points (game_id , athlete_id , technical_point , technical_point_desc) VALUES ("
                + _gameId + ","
                + athId + ","
                + p.points + ", '"
                + p.description + "'); ");
        }



        #endregion

        private void Window_close(object sender, EventArgs e)
        {
            this.Close();
            this._sender.Show();
        }

        
    }



    public class Point
    {

        public string description { get; set; }
        public int points { get; set; }
        public int pointsIndex { get; set; }
        public string time { get; set; }
        public string side { get; set; }



        public Point(string dsc, int points, int PointIndex, string side)
        {
            this.description = dsc;
            this.points = points;
            this.pointsIndex = pointsIndex;
            this.time = DateTime.Now.ToString("HH:mm:ss tt");
            this.side = side;
        }



        public void update(string dsc, int points, string side)
        {
            this.description = dsc;
            this.points = points;
            this.side = side;
        }
    }
}
