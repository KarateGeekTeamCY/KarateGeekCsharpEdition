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
                                                                        double score1, double score2, double score3, double score4, double score5, double mean)
        {
            sql = "insert into gane_scores (game_id, athlete_id, judge1, judge2, judge3, judge4, judge5, " +
                "score1, score2, score3, score4, score5, mean_score) values ('" +
                gameId + "', '" + athleteId + "', '" +
                judge1Id + "', '" + judge2Id + "', '" + judge3Id + "', '" + judge4Id + "', '" + judge5Id + "', '" +
                score1 + "', '" + score2 + "' ,'" + score3 + "' ,'" + score4 + "' ,'" + score5 + "' ,'" + mean + "');";

            this.NonQuery(sql);
            return "";
        }


        public string InsertNewScoreTeam(string gameId, string teamId, string judge1Id, string judge2Id, string judge3Id, string judge4Id, string judge5Id,
                                                                        double score1, double score2, double score3, double score4, double score5, double mean)
        {
            sql = "INSERT INTO gane_scores (game_id, team_id, judge1, judge2, judge3, judge4, judge5, " +
                "score1, score2, score3, score4, score5, mean_score) VALUES ('" +
                gameId + "', '" + teamId + "', '" +
                judge1Id + "', '" + judge2Id + "', '" + judge3Id + "', '" + judge4Id + "', '" + judge5Id + "', '" +
                score1 + "', '" + score2 + "' ,'" + score3 + "' ,'" + score4 + "' ,'" + score5 + "' ,'" + mean + "');";

            this.NonQuery(sql);
            return "";
        }


        public DataSet GetScoreById(string gameId, string athleteId)
        {
            sql = "SELECT * FROM total_score_system WHERE game_id = '" + gameId + "' AND athlete_id = '" + athleteId + "' ;";
            return this.Query(sql);
        }


        public DataTable GetPhaseRAnking(string tournamentId, string phase)
        {
            sql = "SELECT * FROM total_score_system WHERE tournament_id = '" + tournamentId + "' AND phase = '" + phase + "'; ";
            return this.Query(sql).Tables[0];

        }




    }
}
