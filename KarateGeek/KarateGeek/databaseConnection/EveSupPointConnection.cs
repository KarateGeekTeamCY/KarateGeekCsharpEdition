using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class EveSupPointConnection : CoreDatabaseConnection
    {

        private string sql;



        //
        // needs edit
        //
        public string InsertNewpoint(string gameId, string athleteId, string technical, string technicalDesc)
        {
            sql = "insert into gane_points (game_id, athlete_id, technical_score, technical_score_desc) values ('"
                + gameId + "', '" + athleteId + "', '" + technical + "', '" + technicalDesc + "');";
            this.NonQuery(sql);
            return "";
        }


        public DataTable getPointsOfAthleteAtGame(string gameId, string athleteId)
        {
            this.sql = "select * from total_point_system where game_id = '" + gameId + "' and athlete_id = '" + athleteId + "';";
            return this.Query(sql).Tables[0];
        }



    }
}
