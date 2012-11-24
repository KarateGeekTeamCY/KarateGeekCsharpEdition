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
using System.Data;
using KarateGeek.databaseConnection;

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for ChooseWinner.xaml
    /// </summary>
    public partial class ChooseWinner : Window
    {
        private Window _sender;
        private Game _game;
        private Tournament _tournament;
        private bool _isTeam;
        private string partAId, partBId;

        private Team a, b;

        CoreDatabaseConnection conn = new CoreDatabaseConnection();
        DataTable temp;



        public ChooseWinner(Window sender, Game game, Tournament tournament)
        {
            InitializeComponent();
            this._sender = sender;
            this._game = game;
            this._tournament = tournament;


            this._isTeam = this._tournament.isTeam;


            if (this._isTeam)
            {
                conn = new CoreDatabaseConnection();
                temp = conn.Query("SELECT DISTINCT  team_id FROM game_participations WHERE game_id = '" + this._game.gameId + "' order by team|_id;").Tables[0];

                partAId = temp.Rows[0][0].ToString();
                partBId = temp.Rows[0][1].ToString();
                a = new Team(partAId);
                b = new Team(partBId);

                this.lblAthleteAname.Content = a.participants.ElementAt(0).lastName + " " + a.participants.ElementAt(0).firstName + "\n";
                this.lblAthleteAname.Content += a.participants.ElementAt(1).lastName + " " + a.participants.ElementAt(1).firstName + "\n";
                this.lblAthleteAname.Content += a.participants.ElementAt(2).lastName + " " + a.participants.ElementAt(2).firstName;


                this.lblAthleteBname.Content = b.participants.ElementAt(0).lastName + " " + b.participants.ElementAt(0).firstName + "\n";
                this.lblAthleteBname.Content += b.participants.ElementAt(1).lastName + " " + b.participants.ElementAt(1).firstName + "\n";
                this.lblAthleteBname.Content += b.participants.ElementAt(2).lastName + " " + b.participants.ElementAt(2).firstName;

                this.lblInfo.Content = "Please shoose the winner";
            }
            else
            {
                this.partAId = game.participants.ElementAt(0).lastName + " " + game.participants.ElementAt(0).id;
                this.partBId = game.participants.ElementAt(0).lastName + " " + game.participants.ElementAt(1).id;

                this.lblAthleteAname.Content = game.participants.ElementAt(0).lastName + " " + game.participants.ElementAt(0).firstName;
                this.lblAthleteBname.Content = game.participants.ElementAt(1).lastName + " " + game.participants.ElementAt(1).firstName;
                this.lblInfo.Content = "Please shoose the winner";
            }

        }





        private void btnAthleteAisWinner_Click(object sender, RoutedEventArgs e)
        {
            if (this._isTeam)
            {
                foreach (Athlete ath in a.participants)
                {
                    conn.NonQuery("insert into game_points (game_id , athlete_id , team_id , technical_point , technical_point_desc) values ('"
                                + this._game.gameId + "','"
                                + ath.id + "', '"
                                + this.a.id + "', '"
                                + "1" + "', '"
                                + Strings.bychoice + "'); ");
                }
            }
            else
            {
                conn.NonQuery("insert into game_points (game_id , athlete_id , technical_point , technical_point_desc) values ('"
                            + this._game.eventId + "','"
                            + this.partAId + "','"
                            + "1" + "', '"
                            + Strings.bychoice + "'); ");
            }
        }

        private void btnAthleteBisWinner_Click(object sender, RoutedEventArgs e)
        {
            if (this._isTeam)
            {
                foreach (Athlete ath in b.participants)
                {
                    conn.NonQuery("insert into game_points (game_id , athlete_id , team_id , technical_point , technical_point_desc) values ('"
                                + this._game.gameId + "','"
                                + ath.id + "', '"
                                + this.b.id + "', '"
                                + "1" + "', '"
                                + Strings.bychoice + "'); ");
                }
            }
            else
            {
                conn.NonQuery("insert into game_points (game_id , athlete_id , technical_point , technical_point_desc) values ('"
                            + this._game.eventId + "','"
                            + this.partBId + "','"
                            + "1" + "', '"
                            + Strings.bychoice + "'); ");
            }
        }
    }
}
