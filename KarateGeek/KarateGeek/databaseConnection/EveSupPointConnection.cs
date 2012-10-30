using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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



    }
}
