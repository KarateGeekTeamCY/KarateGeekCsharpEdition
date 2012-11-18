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
        private Window sender;


        public KumiteTeamMaker(Game game, Window sender)
        {
            InitializeComponent();
            this.game = game;
            this.sender = sender;
            //this.sender.Visibility = System.Windows.Visibility.Hidden;
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();

            KarateGeek.databaseConnection.CoreDatabaseConnection conn = new databaseConnection.CoreDatabaseConnection();

            DataTable temp = conn.Query("select * from game_participants where tournament_id = '" + game.tournamentId + "' and game_id = '" + game.gameId + "'; ").Tables[0];

            string phase, teamAid, teamBid;

            teamAid = (string)temp.Rows[0][2];
            teamBid = (string)temp.Rows[1][2];

            gamePartIdA = (string)temp.Rows[0][0];
            gamePartIdB = (string)temp.Rows[1][0];

            temp = conn.Query("select phase from games where tournament_id = '" + game.tournamentId + "' and game_id = '" + game.gameId + "' ;").Tables[0];
            phase = (string)temp.Rows[0][0];


            StringBuilder sb = new StringBuilder();
            sb.Append("select persons.id, last_name, first_name from tournament_participants join persons on tournament_participants.athlete_id = persons.id");
            sb.Append("  where tournament_id = '").Append(game.tournamentId);
            sb.Append("' and team_id = '").Append(teamAid);
            sb.Append("' and athelet_id not in ");
            sb.Append("  (select athlete_id from game_participations join games on game_participations.game_id = games.id");
            sb.Append("  where  tournament_id = '").Append(game.tournamentId);
            sb.Append("' and phase = '").Append(phase).Append("' ); ");

            athsA = conn.Query(sb.ToString()).Tables[0];

            foreach (DataRow dr in athsA.Rows)
                listA.Add((string)dr[1] + " " + (string)dr[2]);



            sb = new StringBuilder();
            sb.Append("select persons.id, last_name, first_name from tournament_participants join persons on tournament_participants.athlete_id = persons.id");
            sb.Append("  where tournament_id = '").Append(game.tournamentId);
            sb.Append("' and team_id = '").Append(teamBid);
            sb.Append("' and athelet_id not in ");
            sb.Append("  (select athlete_id from game_participations join games on game_participations.game_id = games.id");
            sb.Append("  where  tournament_id = '").Append(game.tournamentId);
            sb.Append("' and phase = '").Append(phase).Append("' ); ");

            athsB = conn.Query(sb.ToString()).Tables[0];

            foreach (DataRow dr in athsB.Rows)
                listB.Add((string)dr[1] + " " + (string)dr[2]);


            this.listBoxTeamA.ItemsSource = listA;
            this.listBoxTeamB.ItemsSource = listB;

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

                if (result == "Ok")
                {
                    save();
                    //this.sender.Visibility = System.Windows.Visibility.Visible;
                    this.Close();
                }
            }
        }


        private void save()
        {

            databaseConnection.CoreDatabaseConnection conn = new databaseConnection.CoreDatabaseConnection();

            conn.NonQuery("update game_participations set athlete_id = '" + (string)athsA.Rows[listBoxTeamA.SelectedIndex][0] + "' where id = '" + gamePartIdA + "'; ");
            conn.NonQuery("update game_participations set athlete_id = '" + (string)athsB.Rows[listBoxTeamA.SelectedIndex][0] + "' where id = '" + gamePartIdB + "'; ");

            game.Update();
        }


    }
}
