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
    /// Interaction logic for ChooseKataWinner.xaml
    /// </summary>
    public partial class ChooseKataWinner : Window
    {

        private EventSupport _sender;
        private Game _game;
        private Tournament _tournament;
        private bool _isTeam;
        private bool _isSync;


        private string partAId, partBId;
        private Team team;


        CoreDatabaseConnection conn = new CoreDatabaseConnection();
        DataTable ids;


        public ChooseKataWinner(EventSupport sender, Tournament tournament, DataTable tiecroup)
        {
            InitializeComponent();


            this._sender = sender;
            this._game = game;
            this._tournament = tournament;
            this.ids = tiecroup;


            this._isTeam = this._tournament.isTeam;
            this._isSync = this._tournament.isSync;


            if (this._isTeam || this._isSync)
            {
                foreach (DataRow cteam in ids.Rows)
                {
                    string teamid = cteam[0].ToString();
                    Team team = new Team(teamid);

                    string teammem = this.team.participants.ElementAt(0).lastName + " " + this.team.participants.ElementAt(0).firstName + "\n";
                    teammem += this.team.participants.ElementAt(1).lastName + " " + this.team.participants.ElementAt(1).firstName + "\n";
                    teammem += this.team.participants.ElementAt(2).lastName + " " + this.team.participants.ElementAt(2).firstName + "\n";

                    this.tieList.Items.Add(teammem);
                }
            }
            else
            {
                foreach (DataRow cath in ids.Rows)
                {
                    string athid = cath[0].ToString();
                    Athlete ath = new Athlete(athid, this._tournament.id.ToString());

                    string athName = this.team.participants.ElementAt(0).lastName + " " + this.team.participants.ElementAt(0).firstName + "\n";

                    this.tieList.Items.Add(athName);
                }
            }

            this.Show();
        }


        private void saveWinner_Click(object sender, RoutedEventArgs e)
        {
            int indexofwinner = this.tieList.SelectedIndex;
            string winnerid = this.ids.Rows[indexofwinner][0].ToString();

        }


        private void setWinner(string winnerid)
        {
            string sql;

            if (this._isTeam || this._isSync)
            {
                sql = "UPDATE game_scores SET mean_score = (mean_score + 1) WHERE team_id = '" + winnerid + "' AND game_id IN"
                    + " (SELECT id FROM games WHERE tournament_id = '" + this._tournament.id + "' AND phase = '" + this._game.phase + "') ;";
                conn.NonQuery(sql);
            }
            else
            {
                sql = "UPDATE game_scores SET mean_score = (mean_score + 1) WHERE athlete_id = '" + winnerid + "' AND game_id IN"
                    + " (SELECT id FROM games WHERE tournament_id = '" + this._tournament.id + "' AND phase = '" + this._game.phase + "') ;";
                conn.NonQuery(sql);
            }

            _sender.tournament.load();
            _sender.advanceAthlites(this._game);
            _sender.tournament.load();
            _sender.loadGames();


            this.Close();
        }
    }
}








