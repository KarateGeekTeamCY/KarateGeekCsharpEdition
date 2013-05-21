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
    public partial class ChooseWinner : Window
    {
        private EventSupport _sender;
        private Game _game;
        private Tournament _tournament;
        private bool _isTeam;


        private string partAId, partBId;
        private Team teamA, teamB;


        CoreDatabaseConnection conn = new CoreDatabaseConnection();
        DataTable temp;



        public ChooseWinner(EventSupport sender, Game game, Tournament tournament)
        {
            InitializeComponent();

            this._sender = sender;
            this._game = game;
            this._tournament = tournament;

            this.Topmost = true;

            this._isTeam = this._tournament.isTeam;


            if (this._isTeam)
            {
                conn = new CoreDatabaseConnection();
                temp = conn.Query("SELECT DISTINCT  team_id FROM game_participations WHERE game_id = '" + this._game.gameId + "' order by team_id;").Tables[0];

                partAId = temp.Rows[0][0].ToString();
                partBId = temp.Rows[1][0].ToString();

                teamA = new Team(partAId);
                teamB = new Team(partBId);

                //this.lblAthleteAname.Content = teamA.participants.ElementAt(0).lastName + " " + teamA.participants.ElementAt(0).firstName + "\n";
                //this.lblAthleteAname.Content += teamA.participants.ElementAt(1).lastName + " " + teamA.participants.ElementAt(1).firstName + "\n";
                //this.lblAthleteAname.Content += teamA.participants.ElementAt(2).lastName + " " + teamA.participants.ElementAt(2).firstName;


                //this.lblAthleteBname.Content = teamB.participants.ElementAt(0).lastName + " " + teamB.participants.ElementAt(0).firstName + "\n";
                //this.lblAthleteBname.Content += teamB.participants.ElementAt(1).lastName + " " + teamB.participants.ElementAt(1).firstName + "\n";
                //this.lblAthleteBname.Content += teamB.participants.ElementAt(2).lastName + " " + teamB.participants.ElementAt(2).firstName;


                this.btnAthleteAisWinner.Content = teamA.participants.ElementAt(0).lastName + " " + teamA.participants.ElementAt(0).firstName + "\n";
                this.btnAthleteAisWinner.Content += teamA.participants.ElementAt(1).lastName + " " + teamA.participants.ElementAt(1).firstName + "\n";
                this.btnAthleteAisWinner.Content += teamA.participants.ElementAt(2).lastName + " " + teamA.participants.ElementAt(2).firstName;


                this.btnAthleteBisWinner.Content = teamB.participants.ElementAt(0).lastName + " " + teamB.participants.ElementAt(0).firstName + "\n";
                this.btnAthleteBisWinner.Content += teamB.participants.ElementAt(1).lastName + " " + teamB.participants.ElementAt(1).firstName + "\n";
                this.btnAthleteBisWinner.Content += teamB.participants.ElementAt(2).lastName + " " + teamB.participants.ElementAt(2).firstName;


                this.lblInfo.Content = "Please choose the winner";
            }
            else
            {
                this.partAId = game.participants.ElementAt(0).id;
                this.partBId = game.participants.ElementAt(1).id;

                //this.lblAthleteAname.Content = game.participants.ElementAt(0).lastName + " " + game.participants.ElementAt(0).firstName;
                //this.lblAthleteBname.Content = game.participants.ElementAt(1).lastName + " " + game.participants.ElementAt(1).firstName;

                this.btnAthleteAisWinner.Content = game.participants.ElementAt(0).lastName + " " + game.participants.ElementAt(0).firstName;
                this.btnAthleteBisWinner.Content = game.participants.ElementAt(1).lastName + " " + game.participants.ElementAt(1).firstName;

                this.lblInfo.Content = "Please choose the winner";
            }

        }




        private void btnAthleteAisWinner_Click(object sender, RoutedEventArgs e)
        {

            setWinner(partAId, teamA);
        }



        private void btnAthleteBisWinner_Click(object sender, RoutedEventArgs e)
        {

            setWinner(partBId, teamB);
        }



        private void setWinner(string athId, Team tm)
        {
            //
            //  NOTE
            //  bug:
            //
            //  to athid einai to onoma tou athliti prepi na gini to id 
            //
            if (this._isTeam)
            {
                foreach (Athlete ath in tm.participants)
                {
                    conn.NonQuery("insert into game_points (game_id , athlete_id , team_id , technical_point , technical_point_desc) values ('"
                                + this._game.gameId + "','"
                                + ath.id + "', '"
                                + tm.id + "', '"
                                + "1" + "', '"
                                + Strings.bychoice + "'); ");
                }
            }
            else
            {
                conn.NonQuery("insert into game_points (game_id , athlete_id , technical_point , technical_point_desc) values ('"
                            + this._game.gameId + "','"
                            + athId + "','"
                            + "1" + "', '"
                            + Strings.bychoice + "'); ");
            }

            _sender.tournament.load();
            _sender.advanceAthlites(this._game);
            _sender.tournament.load();
            _sender.loadGames();


            this.Close();
        }

    }
}
