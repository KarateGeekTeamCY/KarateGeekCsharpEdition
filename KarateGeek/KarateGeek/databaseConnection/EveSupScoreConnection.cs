using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class EveSupScoreConnection : CoreDatabaseConnection
    {
        private string sql;





        


        public string InsertNewScoreInd(string gameId, string athleteId, string judge1Id, string judge2Id, string judge3Id, string judge4Id, string judge5Id, 
                                                                        string score1, string score2, string score3, string score4, string score5)
        {
            sql = "insert into gane_scores (game_id, athlete_id, judge1, judge2, judge3, judge4, judge5, "+
                "score1, score2, score3, score4, score5) values ('" + 
                gameId + "', '" + athleteId + "', '" +
                judge1Id + "', '" + judge2Id + "', '" + judge3Id + "', '" + judge4Id + "', '" + judge5Id + "', '" + 
                score1 + "', '" + score2 + "' ,'" + score3 + "' ,'" + score4 + "' ,'" + score5 + "');";

            this.NonQuery(sql);
            return "";
        }


        public string InsertNewScoreTeam(string gameId, string athleteId, string teamId, string judge1Id, string judge2Id, string judge3Id, string judge4Id, string judge5Id,
                                                                        string score1, string score2, string score3, string score4, string score5)
        {
            sql = "insert into gane_scores (game_id, athlete_id, team_id, judge1, judge2, judge3, judge4, judge5, " +
                "score1, score2, score3, score4, score5) values ('" +
                gameId + "', '" + athleteId + "', '" + teamId + "', '" +
                judge1Id + "', '" + judge2Id + "', '" + judge3Id + "', '" + judge4Id + "', '" + judge5Id + "', '" +
                score1 + "', '" + score2 + "' ,'" + score3 + "' ,'" + score4 + "' ,'" + score5 + "');";

            this.NonQuery(sql);
            return "";
        }


        public DataSet GetScoreById(string gameId, string athleteId)
        {
            sql = "select * from game_scores where game_id = '" + gameId + "' and athlete_id = '" + athleteId + "' ;";
            return this.Query(sql);
        }


    }
}
