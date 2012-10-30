using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class TournamentGameParticipationsConnection : CoreDatabaseConnection
    {
        private string sql = "";


        public DataSet GetParticipation(string gameId)
        {
            sql = "select * from game_participations where game_id = '" + gameId + "';";
            return this.Query(sql);
        }

        


    }
}
