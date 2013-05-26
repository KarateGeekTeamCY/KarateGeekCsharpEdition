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

        public int InsertNewTournament(string name, string sex, int ageFrom, int ageTo, string levelFrom, string levelTo, string game_type, string scoring_type, int event_id)
        {

            string sql = "insert into tournaments ( name, sex, age_from, age_to , level_from, level_to, game_type, scoring_type, event_id) values ( '"
                + name + "', '"
                + sex + "', '"
                + ageFrom + "', '"
                + ageTo + "', '"
                + levelFrom + "', '"
                + levelTo + "', '"
                + game_type + "', '"
                + scoring_type + "', '"
                + event_id + "' );";

            this.NonQuery(sql);

            sql = "select currval('tournaments_id_seq');";
            dr = this.Query(sql);
            int tournamentId = int.Parse(dr.Tables[0].Rows[0][0].ToString());

            return tournamentId;
        }

        public string UpdateTournament(int tournamentId, string name, string sex, int ageFrom, int ageTo, string levelFrom, string levelTo, string game_type, string scoring_type, int event_id)
        {
            string sql = "update tournaments set " +
                 "name = '" + name + "', " +
                 "sex = '" + sex + "', " +
                 "age_from = '" + ageFrom + "', " +
                 "age_to = '" + ageTo + "', " +
                 "level_from = '" + levelFrom + "', " +
                 "level_to = '" + levelTo + "', " +
                 "game_type = '" + game_type + "', " +
                 "scoring_type = '" + scoring_type + "', " +
                 "event_id = '" + event_id + "' where id = '" + tournamentId + "' ;";

            this.NonQuery(sql);

            return "";
        }

        public void deleteTournament(int id)
        {
            string sql = "delete from tournaments where id='" + id + "';";
            this.Query(sql);
        }
        public DataSet findSimilar(string filter, int eventId)
        {
            string sql = "select * from tournaments where event_id='" + eventId + "' and lower(name) like lower('" + filter + "%') and is_finished = 'false' order by name;";
            return this.Query(sql);
        }


        public DataTable getUnlotterisedTournaments(int eventId)
        {
            string sql = "select id , name from tournaments where event_id = " + eventId + " and lottery_ready = false order by name";
            return this.Query(sql).Tables[0];
        }

    }
}
