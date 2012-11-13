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
        public bool isInd { get; set; }
        public bool isSync { get; set; }


        public List<Game> games2 { get; set; }
        public List<Game> games4 { get; set; }
        public List<Game> games8 { get; set; }
        public List<Game> games16 { get; set; }
        public List<Game> games32 { get; set; }
        public List<Game> games64 { get; set; }
        public List<Game> games128 { get; set; }

        public List<Athlete> participants { get; set; }
        public List<string> teams { get; set; }


        public Tournament(string tournamentId)
        {
            this.id = tournamentId;
            this.isInd = false;
            this.isTeam = false;
            this.isSync = false;
            this.load();
        }

        private void load()
        {
            sql = "SELECT * FROM tournaments WHERE id = '" + this.id + "';";
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

            switch ((string)this.gameType.Split('|')[0])
            {
                case Strings.individual:
                    this.isInd = true;
                    break;
                case Strings.team:
                    this.isTeam = true;
                    break;
                case "SYNC":
                    this.isSync = true;
                    break;
            }


            #region load games

            sql = "SELECT * FROM games WHERE tournament_id = '" + this.id + "' AND phase = 7 ORDER BY position ASC;";
            temp = Query(sql).Tables[0];

            this.games128 = new List<Game>();


            foreach (DataRow dr in temp.Rows)
            {
                this.games128.Add(new Game((string)dr[0]));
            }

            sql = "SELECT * FROM games WHERE tournament_id = '" + this.id + "' AND phase = 6 ORDER BY position ASC;";
            temp = Query(sql).Tables[0];

            this.games64 = new List<Game>();


            foreach (DataRow dr in temp.Rows)
            {
                this.games64.Add(new Game((string)dr[0]));
            }

            sql = "SELECT * FROM games WHERE tournament_id = '" + this.id + "' AND phase = 5 ORDER BY position ASC;";
            temp = Query(sql).Tables[0];

            this.games32 = new List<Game>();


            foreach (DataRow dr in temp.Rows)
            {
                this.games32.Add(new Game((string)dr[0]));
            }

            sql = "SELECT * FROM games WHERE tournament_id = '" + this.id + "' AND phase = 4 ORDER BY position ASC;";
            temp = Query(sql).Tables[0];

            this.games16 = new List<Game>();


            foreach (DataRow dr in temp.Rows)
            {
                this.games16.Add(new Game((string)dr[0]));
            }

            sql = "SELECT * FROM games WHERE tournament_id = '" + this.id + "' AND phase = 3 ORDER BY position ASC;";
            temp = Query(sql).Tables[0];

            this.games8 = new List<Game>();


            foreach (DataRow dr in temp.Rows)
            {
                this.games8.Add(new Game((string)dr[0]));
            }

            sql = "SELECT * FROM games WHERE tournament_id = '" + this.id + "' AND phase = 2 ORDER BY position ASC;";
            temp = Query(sql).Tables[0];

            this.games4 = new List<Game>();


            foreach (DataRow dr in temp.Rows)
            {
                this.games4.Add(new Game((string)dr[0]));
            }

            sql = "SELECT * FROM games WHERE tournament_id = '" + this.id + "' AND phase = 1 ORDER BY position ASC;";
            temp = Query(sql).Tables[0];

            this.games2 = new List<Game>();


            foreach (DataRow dr in temp.Rows)
            {
                this.games2.Add(new Game((string)dr[0]));
            }

            #endregion load games

            #region load participants and teams if exist

            sql = "SELECT * FROM tournament_participations where tournament_id = '" + this.id + "';";
            temp = this.Query(sql).Tables[0];

            foreach (DataRow dr in temp.Rows)
            {
                this.participants.Add(new Athlete((string)dr[0]));
            }

            sql = "SELECT * FROM team_tournament_participations where tournament_id = '" + this.id + "';";
            temp = this.Query(sql).Tables[0];

            foreach (DataRow dr in temp.Rows)
            {
                this.teams.Add((string)dr[0]);
            }

            #endregion load participants and teams if exist


        }


    }
}
