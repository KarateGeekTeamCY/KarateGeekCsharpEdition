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

namespace KarateGeek.guis
{
    /// <summary>
    /// Interaction logic for KumiteTeamMaker.xaml
    /// </summary>
    public partial class KumiteTeamMaker : Window
    {
        private bool aFlag = false, bFlag = false;
        private DataTable athsA, athsB;
        private string gamePartIdA, gamePartIdB;
        private Game game;
        private EventSupport sender;


        public KumiteTeamMaker(Game game, EventSupport sender)
        {
            InitializeComponent();
            this.game = game;
            this.sender = sender;
            //this.sender.Visibility = System.Windows.Visibility.Hidden;
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();

            KarateGeek.databaseConnection.CoreDatabaseConnection conn = new databaseConnection.CoreDatabaseConnection();

            DataTable temp = conn.Query("SELECT * FROM game_participations gp JOIN games ON games.id = gp.game_id  WHERE tournament_id = '" + game.tournamentId + "' AND game_id = '" + game.gameId + "'; ").Tables[0];

            string phase, teamAid, teamBid;

            teamAid = (string)temp.Rows[0][2].ToString();
            teamBid = (string)temp.Rows[1][2].ToString();

            gamePartIdA = (string)temp.Rows[0][0].ToString();
            gamePartIdB = (string)temp.Rows[1][0].ToString();

            temp = conn.Query("SELECT phase FROM games WHERE tournament_id = '" + game.tournamentId + "' and id = '" + game.gameId + "' ;").Tables[0];
            phase = (string)temp.Rows[0][0].ToString();


            StringBuilder sb = new StringBuilder();
            sb.Append("select persons.id, last_name, first_name from tournament_participations tp join persons on tp.athlete_id = persons.id");
            sb.Append("  where tournament_id = '").Append(game.tournamentId);
            sb.Append("' and team_id = '").Append(teamAid);
            sb.Append("' and persons.id not in ");
            sb.Append("  (select athlete_id from game_participations gp join games on gp.game_id = games.id");
            sb.Append("  where  tournament_id = '").Append(game.tournamentId);
            sb.Append("' and phase = '").Append(phase).Append("' and athlete_id is not null ); ");

            athsA = conn.Query(sb.ToString()).Tables[0];

            foreach (DataRow dr in athsA.Rows)
                listA.Add((string)dr[1] + " " + (string)dr[2]);



            sb = new StringBuilder();
            sb.Append("select persons.id, last_name, first_name from tournament_participations tp join persons on tp.athlete_id = persons.id");
            sb.Append("  where tournament_id = '").Append(game.tournamentId);
            sb.Append("' and team_id = '").Append(teamBid);
            sb.Append("' and tp.athlete_id not in ");
            sb.Append("  (select athlete_id from game_participations gp join games on gp.game_id = games.id");
            sb.Append("  where  tournament_id = '").Append(game.tournamentId);
            sb.Append("' and phase = '").Append(phase).Append("' and athlete_id is not null ); ");

            athsB = conn.Query(sb.ToString()).Tables[0];

            foreach (DataRow dr in athsB.Rows)
                listB.Add((string)dr[1] + " " + (string)dr[2]);


            this.listBoxTeamA.ItemsSource = listA;
            this.listBoxTeamB.ItemsSource = listB;


            this.Show();
        }

        private void listBoxTeamA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!aFlag)
                aFlag = true;
            else
            { }
            confirm();
        }

        private void listBoxTeamB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!bFlag)
                bFlag = true;
            else
            { }
            confirm();
        }

        private void confirm()
        {
            if (aFlag && bFlag)
            {
                string result = MessageBox.Show("Are you sure you want to assign "
                    + (string)athsA.Rows[this.listBoxTeamA.SelectedIndex][1] + " " + (string)athsA.Rows[this.listBoxTeamA.SelectedIndex][2]
                    + " to " + (string)athsB.Rows[this.listBoxTeamB.SelectedIndex][1] + "" + (string)athsB.Rows[this.listBoxTeamB.SelectedIndex][2] + " in this game?", "Pair ready!",
                MessageBoxButton.OK,
                MessageBoxImage.Information).ToString();

                if (result == "OK")
                {
                    save();
                    this.sender.Visibility = System.Windows.Visibility.Visible;
                    this.sender.loadGames();
                    this.Close();
                }
            }
        }


        private void save()
        {

            databaseConnection.CoreDatabaseConnection conn = new databaseConnection.CoreDatabaseConnection();

            conn.NonQuery("update game_participations set athlete_id = '" + athsA.Rows[listBoxTeamA.SelectedIndex][0].ToString() + "' where id = '" + gamePartIdA + "'; ");
            conn.NonQuery("update game_participations set athlete_id = '" + athsB.Rows[listBoxTeamA.SelectedIndex][0].ToString() + "' where id = '" + gamePartIdB + "'; ");

            game.Update();
        }


    }
}
