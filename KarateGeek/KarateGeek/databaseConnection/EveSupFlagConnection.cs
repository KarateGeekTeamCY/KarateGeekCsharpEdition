using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class EveSupFlagConnection : CoreDatabaseConnection
    {
        private string sql;




        public string InsertNewflagInd(string gameId, string athleteId, string judge1Id, string judge2Id, string judge3Id, string judge4Id, string judge5Id,
                                                                        bool flag1, bool flag2, bool flag3, bool flag4, bool flag5)
        {
            sql = "INSERT INTO game_flag (game_id, athlete_id, judge1, judge2, judge3, judge4, judge5, " +
                "flag1, flag2, flag3, flag4, flag5) VALUES ('" +
                gameId + "', '" + athleteId + "', '" +
                judge1Id + "', '" + judge2Id + "', '" + judge3Id + "', '" + judge4Id + "', '" + judge5Id + "', '" +
                flag1 + "', '" + flag2 + "' ,'" + flag3 + "' ,'" + flag4 + "' ,'" + flag5 + "');";

            this.NonQuery(sql);
            return "";
        }


        public string InsertNewflagTeam(string gameId, string teamId, string judge1Id, string judge2Id, string judge3Id, string judge4Id, string judge5Id,
                                                                        bool flag1, bool flag2, bool flag3, bool flag4, bool flag5)
        {
            sql = "INSERT INTO game_flag (game_id, team_id, judge1, judge2, judge3, judge4, judge5, " +
                "flag1, flag2, flag3, flag4, flag5) VALUES ('" +
                gameId + "', '" + teamId + "', '" +
                judge1Id + "', '" + judge2Id + "', '" + judge3Id + "', '" + judge4Id + "', '" + judge5Id + "', '" +
                flag1 + "', '" + flag2 + "' ,'" + flag3 + "' ,'" + flag4 + "' ,'" + flag5 + "');";

            this.NonQuery(sql);
            return "";
        }


        public DataSet GetflagById(string gameId, string athleteId)
        {
            sql = "SELECT * from game_flags WHERE game_id = '" + gameId + "' AND athlete_id = '" + athleteId + "' ;";
            return this.Query(sql);
        }


    }
}
