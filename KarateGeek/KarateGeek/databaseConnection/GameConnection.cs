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

            return this.Query("select currval('games_id_seq');").Tables[0].Rows[0][0].ToString();
        }

        



        public DataSet GetGamesByTurnament( string turnamentId )
        {
            sql = "SELECT * FROM games WHERE tournament_id = '" + turnamentId + "';";
            return this.Query(sql);
        }


        public DataSet GetGamesByTurnamentPhase( string turnamentId, string phase)
        {
            sql = "SELECT * FROM games WHERE turnament_id = '" + turnamentId + "' AND phase = '" + phase + "';" ;
            return this.Query(sql);
        }


        public DataSet GetGamesByTurnamentPhasePosition(string turnamentId, string phase, string position)
        {
            sql = "SELECT * FROM games WHERE turnament_id = '" + turnamentId + "' AND phase = '" + phase + "' AND position = '" + position + "';";
            return this.Query(sql);
        }


        public DataSet GetGameById(string gameId)
        {
            sql = "SELECT * FROM games WHERE id = '" + gameId + "' ;" ;
            return this.Query(sql);
        }

        public DataSet getFuturePhaces(string tournamentId)
        {
            sql = "SELECT DISTINCT phase FROM games WHERE is_finished = false AND tournament_id = '" + tournamentId + "' ;";
            return this.Query(sql);
        }



    }
}
