using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class TournamentConnection : CoreDatabaseConnection
    {
        DataTable tournament = new DataTable();
        DataSet dr = null;

        public string InsertNewTournament(string name, string sex, int ageFrom, int ageTo , string levelFrom, string levelTo, string game_type, string game, int event_id)
        {

            string sql = "insert into tournaments ( name, sex, age_from, age_to , level_from, level_to, game_type, game, event_id) values ( '"
                + name + "', '"
                + sex + "', '"
                + ageFrom + "', '"
                + ageTo + "', '"
                + levelFrom + "', '"
                + levelTo + "', '"
                + game_type + "', '"
                + game + "', '"
                + event_id + "' );";

            this.NonQuery(sql);

            sql = "select currval('tournaments_id_seq');";
            dr = this.Query(sql);
            long tournamentId = long.Parse(dr.Tables[0].Rows[0][0].ToString());

            return "" + tournamentId;
        }
    }
}
