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
            sql = "select * from game_participants_total_det where game_id = '" + gameId + "';";
            return this.Query(sql);
        }


        public DataSet GetParticipation(string gameId, string teamId)
        {
            sql = "select * from game_participants_total_det where game_id = '" + gameId + "' and team_id = '" + teamId + "';";
            return this.Query(sql);
        }

        public string insertParticipant(string athleteId, string gameId)
        {
            sql = "INSERT INTO game_participations (athlete_id, team_id, game_id ) "
                                    + "VALUES ( " + athleteId + ", NULL, '" + gameId + "');";

            this.NonQuery(sql);

            return "";
        }

        public string insertParticipant(string athleteId, string teamId, string gameId)
        {
            sql = "INSERT INTO game_participations (athlete_id, team_id, game_id ) "
                                    + "VALUES ( " + athleteId + ", '" + teamId + "', '" + gameId + "');";

            this.NonQuery(sql);

            return "";
        }



        //private void writeTournamentPair(long id1, long id2, int phase, int position, long tournamentId)
        //{
        //    bool isReady = id1 >= 0 && id2 >= 0;

        //    String writegame = "INSERT INTO games (phase, position, tournament_id, is_ready ) "
        //                     + "VALUES ( " + phase + ", " + position + ", " + tournamentId + ", " + isReady + " );";
        //    String writepair_first = "INSERT INTO game_participations (athlete_id, team_id, game_id ) "
        //                            + "VALUES ( " + id1 + ", NULL, ( SELECT currval('games_id_seq') ));";
        //    String writepair_second = "INSERT INTO game_participations (athlete_id, team_id, game_id ) "
        //                            + "VALUES ( " + id2 + ", NULL, ( SELECT currval('games_id_seq') ));";

        //    if (id1 >= 0 || id2 >= 0) this.NonQuery(writegame);
        //    if (id1 >= 0) this.NonQuery(writepair_first);
        //    if (id2 >= 0) this.NonQuery(writepair_second);
        //}




    }
}
