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
