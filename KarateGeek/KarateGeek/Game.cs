using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using KarateGeek;


namespace KarateGeek
{
    public class Game : KarateGeek.databaseConnection.CoreDatabaseConnection
    {
        private string sql;

        public string eventId { get; set; }
        public string tournamentId { get; set; }
        public string gameId { get; set; }
        public string phase { get; set; }
        public string position { get; set; }

        public List<Athlete> participants { get; set; }


        private bool _isReady;

        public bool isReady
        {
            get
            {
                return _isReady;
            }
            set
            {
                _isReady = value;
                this.Update();
            }
        }

        private bool _isFinished;

        public bool isFinished
        {
            get
            {
                return _isFinished;
            }
            set
            {
                _isFinished = value;
                this.Update();
            }
        }


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
            this.participants.Add(new Athlete(athleteId, this.tournamentId));
            this.numOfParticipants++;
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

            sql = "select * from games join tournaments on games.tournament_id = tournaments.id where games.id = '" + this.gameId + "';";
            DataTable temp = this.Query(sql).Tables[0];


            this.phase = (string)temp.Rows[0][1].ToString();
            this.position = (string)temp.Rows[0][2].ToString();
            this.tournamentId = (string)temp.Rows[0][3].ToString();

            this._isReady = (bool)temp.Rows[0][4];
            this._isFinished = (bool)temp.Rows[0][5];

            this.eventId = (string)temp.Rows[0][15].ToString();


            sql = "select * from game_participants_total_det where game_id = '" + this.gameId + "'";
            temp = this.Query(sql).Tables[0];

            this.numOfParticipants = 0;
            this.participants = new List<Athlete>();

            foreach (DataRow dr in temp.Rows)
            {
                this.participants.Add(new Athlete((string)dr[4].ToString(), this.tournamentId));
                this.numOfParticipants++;
            }
        }

        public void Update()
        {
            sql = "UPDATE games SET is_ready = '" + this.isReady + "', " +
                "is_finished = '" + this.isFinished + "' WHERE id = '" + this.gameId + "'; ";
            this.NonQuery(sql);


            for (int i = 0; i < this.numOfParticipants; i++)
            {
                sql = "select * from game_participations where game_id = '" + this.gameId + "' and athlete_id = '" + this.participants.ElementAt(i).id + "' ; ";


                int exist = this.Query(sql).Tables[0].Rows.Count;
                if (exist == 0)
                {

                    sql = "select * from tournament_participations where tournament_id = '" 
                        + this.tournamentId + "' and athlete_id = '" + this.participants.ElementAt(i).id + "' ;";
                    DataTable temp = Query(sql).Tables[0];

                    Tournament tour = new Tournament(this.tournamentId);


                    if (tour.isInd)
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
