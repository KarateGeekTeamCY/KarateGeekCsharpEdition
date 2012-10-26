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


        public string InsertNewGame()
        {



            return "";
        }



        public DataSet GetGamesByTurnament( string turnamentId )
        {
            sql = "";



            return this.Query(sql);
            
        }

        public DataSet GetGamesByTurnamentPhase( string turnamentId, string phase)
        {
            sql = "";



            return this.Query(sql);

        }

        public DataSet GetGameById(string gameId)
        {
            sql = "select * from games where id = '" + gameId + "' ;";
            return this.Query(sql);
        }
    }
}
