using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class GameConnection : CoreDatabaseConnection
    {
        private string sql;


        public string InsertNewGame(string tournamentId, string phase, string position, string gameType)
        {
            sql = "insert into gamse (tournament_id, phase, position, game_type) values ( '" + tournamentId + "'. '" +
                tournamentId + "'. '" +
                phase + "', '" +
                position + "', '" +
                gameType + "' );";
            this.NonQuery(sql);
            return "";
        }



        public DataSet GetGamesByTurnament( string turnamentId )
        {
            sql = "select * from games where tournament_id = '" + turnamentId + "';";
            return this.Query(sql);
        }


        public DataSet GetGamesByTurnamentPhase( string turnamentId, string phase)
        {
            sql = "select * from games where turnament_id = '" + turnamentId + "' and phase = '" + phase + "';" ;
            return this.Query(sql);
        }


        public DataSet GetGameById(string gameId)
        {
            sql = "select * from games where id = '" + gameId + "' ;" ;
            return this.Query(sql);
        }



    }
}
