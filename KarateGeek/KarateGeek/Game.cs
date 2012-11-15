using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using KarateGeek;


namespace KarateGeek
{
    class Game : KarateGeek.databaseConnection.CoreDatabaseConnection
    {
        private string sql;

        public string eventId { get; set; }
        public string touenamentId { get; set; }
        public string gameId { get; set; }
        public string phase { get; set; }
        public string position { get; set; }

        public List<Athlete> participants { get; set; }

        public bool isReady { get; set; }
        public bool isFinished { get; set; }
        public int numOfParticipants { get; set; }


        public Game(string gameId)
        {
            this.gameId = gameId;
            this.load();
        }



        public Game(string tournamentId, string phase, string position)
        {
            this.gameId = _InsertNewGame(tournamentId, phase, position);
            this.load();
        }

        public void AddParticipant(string athleteId)
        {
            this.participants.Add(new Athlete(athleteId, this.touenamentId));
            this.Update();
        }

        private string _InsertNewGame(string tournamentId, string phase, string position)
        {
            sql = "insert into games (tournament_id, phase, position) values ( '" +
                tournamentId + "'. '" +
                phase + "', '" +
                position + "' );";
            this.NonQuery(sql);

            return (string)this.Query("select currval('games_id_seq');").Tables[0].Rows[0][0];
        }




        public void load()
        {

            sql = "select * from games join tournaments on games.tournament_id = tournaments.id where id = '" + this.gameId + "';";
            DataTable temp = this.Query(sql).Tables[0];


            this.phase = (string)temp.Rows[0][1];
            this.position = (string)temp.Rows[0][2];
            this.touenamentId = (string)temp.Rows[0][3];

            this.isReady = (bool)temp.Rows[0][4];
            this.isFinished = (bool)temp.Rows[0][5];

            this.eventId = (string)temp.Rows[0][15];


            sql = "select * from game_participants_total_det where game_id = '" + this.gameId + "'";
            temp = this.Query(sql).Tables[0];

            this.numOfParticipants = 0;
            this.participants = new List<Athlete>();

            foreach (DataRow dr in temp.Rows)
            {
                this.participants.Add(new Athlete((string)dr[4], this.touenamentId));
                this.numOfParticipants++;
            }
        }

        public void Update()
        {
            sql = "UPDATE games SET is_ready = '" + this.isReady + "', " +
                "is_finished = '" + this.isFinished + "' WHERE game_id = '" + this.gameId + "'; ";
            this.NonQuery(sql);


            for (int i = 0; i < this.numOfParticipants; i++)
            {
                sql = "select * from game_participants where game_id = '" + this.gameId + "' and athlete_id = '" + this.participants.ElementAt(i).id + "' ; ";


                int exist = this.Query(sql).Tables[0].Rows.Count;
                if (exist == 0)
                {
                    
                     sql = "select * from tournament_participations where tournament_id = '" + this.touenamentId + "', and athlete_id = '" + this.participants.ElementAt(i).id + "' ;";
                    DataTable temp = Query(sql).Tables[0];

                    if (temp.Rows.Count == 0)
                    {
                        sql = "insert into game_participations (athlete_id, game_id) values ('"
                            + this.participants.ElementAt(i).id + "', '"
                            + this.gameId + "' );";
                    }
                    else
                    {
                        sql = "insert into game_participations (athlete_id, team_id, game_id) values ('"
                            + this.participants.ElementAt(i).id + "', '"
                            + temp.Rows[0][1] + "', '"
                            + this.gameId + "' );";
                    }

                    this.NonQuery(sql);
                }

            }

            this.load();
        }


    }
}
