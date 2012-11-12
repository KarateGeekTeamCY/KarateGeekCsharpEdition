using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using KarateGeek.databaseConnection;

namespace KarateGeek
{
    class Tournament : CoreDatabaseConnection
    {
        private string sql;

        public string eventId { get; set; }
        public string id { get; set; }
        public string sex { get; set; }
        public string name { get; set; }
        public int ageFrom { get; set; }
        public int ageTo { get; set; }
        public string rankFrom { get; set; }
        public string rankTo { get; set; }
        public string gameType { get; set; }
        public string judgingType { get; set; }
        public bool isTeam { get; set; }


        public List<Game> games { get; set; }


        public Tournament(string tournamentId)
        {
            this.id = tournamentId;
            this.load();
        }

        private void load()
        {
            sql = "select * from tournaments where id = '" + this.id + "';";
            DataTable temp = Query(sql).Tables[0];
            this.name = (string)temp.Rows[0][1];
            this.sex = (string)temp.Rows[0][2];
            this.ageFrom = (int)temp.Rows[0][3];
            this.ageTo = (int)temp.Rows[0][4];
            this.rankFrom = (string)temp.Rows[0][5];
            this.rankTo = (string)temp.Rows[0][6];
            this.gameType = (string)temp.Rows[0][7];
            this.judgingType = (string)temp.Rows[0][8];
            this.eventId = (string)temp.Rows[0][9];

            sql = "select * from games where tournament_id = '" + this.id + "';";
            temp = Query(sql).Tables[0];

            this.games = new List<Game>();


            foreach (DataRow dr in temp.Rows)
            {
                this.games.Add(new Game((string)dr[0]));
            }



        }


    }
}
