using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class ScoreConnection : CoreDatabaseConnection
    {
        private string sql;





        public string InsertNewScore(string gameId, string athleteId, string technical, string technicalDesc)
        {
            sql = "insert into gane_scores (game_id, athlete_id, technical_score, technical_score_desc) values ('"
                + gameId + "', '" + athleteId + "', '" + technical + "', '" + technicalDesc + "');";
            this.NonQuery(sql);
            return "";
        }


        public string InsertNewScore(string gameId, string athleteId, string judge1Id, string judge2Id, string judge3Id, string judge4Id, string judge5Id, 
            string isPoints, string score1, string score2, string score3, string score4, string score5)
        {
            sql = "insert into gane_scores (game_id, athlete_id, head_judge_id, judge1, judge2, judge3, judge4, "+
                "is_points, head_score, score1, score2, score3, score4) values ('" + 
                gameId + "', '" + athleteId + "', '" +
                judge1Id + "', '" + judge2Id + "', '" + judge3Id + "', '" + judge4Id + "', '" + judge5Id + "', '" + isPoints +"', '" + 
                score1 + "', '" + score2 + "' ,'" + score3 + "' ,'" + score4 + "' ,'" + score5 + "');";

            this.NonQuery(sql);
            return "";
        }


        public DataSet GetScoreById(string gameId, string athleteId)
        {
            sql = "select * from scores where game_id = '" + gameId + "' and athlete_id = '" + athleteId + "' ;";
            return this.Query(sql);
        }


    }
}
