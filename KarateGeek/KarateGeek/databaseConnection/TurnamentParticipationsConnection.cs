using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class TurnamentParticipationsConnection : CoreDatabaseConnection
    {
        private string sql = "";


        public DataSet GetTeamParticipation(string gameId)
        {
            sql = "select * from team_turnament_participations where game_id = '" + gameId + "';";


            return this.Query(sql);
        }

        public DataSet GetParticipations(string gameId)
        {
            sql = "select * from game_participation where game_id = '" + gameId + "';";
            return this.Query(sql);
        }


    }
}
