using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KarateGeek.databaseConnection;
using Npgsql;

/** This file will be removed and the contents added to AthleteConnection! **/
using System.Data;          // DataSet, DataTable
using System.Diagnostics;   // has Debug.WriteLine()

namespace KarateGeek.databaseConnection
{
    class LotteryGenConnection : CoreDatabaseConnection
    {
        public List<long> tournamentParticipants(long tournamentId)
        {
            String sql = "SELECT athlete_id FROM tournament_participations WHERE tournament_id = '"
                         + tournamentId + "' ;";

            DataTable dt = this.Query(sql).Tables[0];

            if (dt.Rows.Count == 0) // TODO: Standardise exception handling and showing errors to user
            {
                // Very ugly "OK button" and blocking the app execution until pressed :-(
                // Also, the code showing the message box will be moved to where the exception will be handled
                System.Windows.Forms.MessageBox.Show("No participants found for this tournament."
                    + "\nPlease add the participants before creating a lottery.", "Usage hint");
                throw new Exception("No tournament participants found."); // must be handled by GUI
            }

            List<long> L = new List<long>();

            for (int index = 0; index < dt.Rows.Count; ++index)
                L.Add(long.Parse(dt.Rows[index][0].ToString()));

            Debug.WriteLine("Tournament participants in List<long> :");
            foreach(long element in L) //debug
                Debug.WriteLine(element);

            return L;
        }


        public bool getTournamentLotteryStateReady(long tournamentId)
        {
            String sql = "SELECT lottery_ready FROM tournaments WHERE id = " + tournamentId + " ;";

            String lotteryReady = this.Query(sql).Tables[0].Rows[0][0].ToString();

            return bool.Parse(lotteryReady);
        }


        public void setTournamentLotteryStateReady(long tournamentId, bool lotteryReady = true)
        {
            String sql =  "UPDATE tournaments SET lottery_ready = '" + lotteryReady + "'"
                       + " WHERE id = " + tournamentId + " ;";

            this.NonQuery(sql);
        }


        public String getTournamentGameType(long tournamentId)
        {
            String sql = "SELECT game_type FROM tournaments WHERE id = " + tournamentId + " ;";

            return this.Query(sql).Tables[0].Rows[0][0].ToString();
        }


        public String getTournamentScoringType(long tournamentId)
        {
            String sql = "SELECT scoring_type FROM tournaments WHERE id = " + tournamentId + " ;";

            return this.Query(sql).Tables[0].Rows[0][0].ToString();
        }


        public int getNumOfGoodPlacements(long athleteId, int place, bool official) // overloaded method, not used any more
        {
            /* If you don't care only about official tournaments, use the following simplified query: */
            //String sql = "SELECT ranking FROM tournament_participations WHERE athlete_id = " + athleteId
            //           + " AND ranking = 1;";

            String sql = "SELECT * FROM tournament_participations WHERE athlete_id = " + athleteId
                       + " AND ranking = " + place 
                       + " AND tournament_id IN (SELECT tournaments.id"
                       +                      "  FROM tournaments JOIN events ON events.id = tournaments.event_id"
                       +                      "  WHERE official = " + official
                       +                      " );";
                       //+                       "WHERE official = " + (official? "true": "false") + " );" ;

            return this.Query(sql).Tables[0].Rows.Count;
        }


        public int getNumOfGoodPlacements(long athleteId, int place, string generalGameType, bool official, bool sameType = true)
        {
            /* Limitation: This method gets wins in tournaments of exactly the same type. It should also take into
             * account other types of kata/kumite... (eg. for enbu it should also get the score of "regular" kata)
             */

            String sql = "SELECT * FROM tournament_participations WHERE athlete_id = " + athleteId
                       + " AND ranking = " + place
                       + " AND tournament_id IN (SELECT tournaments.id"
                       +                      "  FROM tournaments JOIN events ON events.id = tournaments.event_id"
                       +                      "  WHERE official = " + official
                       +    /* addition */    "  AND game_type " + (sameType ? "SIMILAR TO " : "NOT SIMILAR TO ") + generalGameType
                       +                      " );";

            return this.Query(sql).Tables[0].Rows.Count;
        }


        public String getBeltColor(long athleteId)
        {
            String sql = "SELECT rank FROM athletes WHERE id = " + athleteId + " ;";

            return this.Query(sql).Tables[0].Rows[0][0].ToString();
        }


        public int getAge(long athleteId)
        {
            String sql = "SELECT date_of_birth FROM athletes NATURAL JOIN persons"
                       + " WHERE id = " + athleteId + " ;";

            DataTable dt = this.Query(sql).Tables[0];

            if (dt.Rows.Count == 0) // TODO: Standardise exception handling and showing errors to user
                throw new Exception("Athlete id not in database");

            DateTime birthdate = DateTime.Parse(dt.Rows[0][0].ToString());

            // See http://stackoverflow.com/a/1404 for a public domain example :)
            DateTime now = DateTime.Today;  // we could also use the date of the event
            int age = now.Year - birthdate.Year;
            if (birthdate > now.AddYears(-age)) age--;

            Debug.WriteLine("Athlete's day of birth is " + birthdate + " and their calculated age is: " + age);
            return age;
        }


        public bool sameClubAthletePair(Tuple<long, long, int, int> athPair) // a bit hackish
        {                                                                    // using Strings to handle NULL
            if (athPair.Item1 < 0 || athPair.Item2 < 0)
                return false;

            String unfinishedSql = "SELECT club_id FROM athletes WHERE id = ";

            String club1 = this.Query(unfinishedSql + athPair.Item1 + " ;").Tables[0].Rows[0][0].ToString();
            String club2 = this.Query(unfinishedSql + athPair.Item2 + " ;").Tables[0].Rows[0][0].ToString();

            return String.IsNullOrWhiteSpace(club1) ? false : club1.Equals(club2, StringComparison.Ordinal);
        }


        public long getTeamOfAthlete(long tournamentId, long athleteId) // returns teamId
        {
            /* TODO: TEST THIS SQL QUERY STRING AGAIN! */
            String sql = "SELECT team_id FROM tournament_participations"
                       + " WHERE tournament_id = " + tournamentId + " AND athlete_id = " + athleteId + " ;";

            String teamId = this.Query(sql).Tables[0].Rows[0][0].ToString();

            return long.Parse(teamId); // if null, what?
        }


        /* Writes a tournament pair to the database (important: it should be "private" to ensure atomicity).
         * 
         * CONVENTION: For semi-complete pairs, the caller provides a negative athlete id.
         *
         * Specifically, we use -1 for !is_ready pairs and -2 for is_ready "pairs".
         * The latter are just single athlete participations, or single team
         * participations (reusing the same code for the DB transaction).                 */
        private void writeTournamentPair(long id1, long id2, int phase, int position, long tournamentId)
        {
            Debug.Assert(id1 >= -1 && id2 >= -2);

            bool isReady = (id1 >= 0 && id2 >= 0) || (id1 >= 0 && id2 == -2);

            String writegame = "INSERT INTO games (phase, position, tournament_id, is_ready ) "
                             + "VALUES ( " + phase + ", " + position + ", " + tournamentId + ", " + isReady + " );";
            String writepair_first  = "INSERT INTO game_participations (athlete_id, team_id, game_id ) "
                                    + "VALUES ( " + id1 + ", NULL, ( SELECT currval('games_id_seq') ));";
            String writepair_second = "INSERT INTO game_participations (athlete_id, team_id, game_id ) "
                                    + "VALUES ( " + id2 + ", NULL, ( SELECT currval('games_id_seq') ));";

            if (id1 >= -1 || id2 >= -1) this.NonQuery(writegame);
            if (id1 >= 0) this.NonQuery(writepair_first);
            if (id2 >= 0) this.NonQuery(writepair_second);
        }


        /* Writes all tournament pairs to the database, atomically: */
        /* Is this the correct approach for atomicity? */
        public bool writeAllTournamentPairs(List<Tuple<long, long, int, int>> Pairs,
            long tournamentId, bool doCommit)
        {
            this.NonQuery("BEGIN;");

            foreach (var pair in Pairs)
                writeTournamentPair(pair.Item1, pair.Item2, pair.Item3, pair.Item4, tournamentId);

            if (doCommit) {
                return this.NonQuery("COMMIT;"); // supposed to return true if successful (currently it's always true)
            } else {
                this.NonQuery("ROLLBACK;"); // very useful for checking syntax etc.
                return false;               // always false, since we didn't write anything
            }
        }


        /** The following methods aren't strictly related to lotteries, and might be moved elsewhere: **/


        public DataTable getTournamentGameTable(long tournamentId)  // overloaded method
        {
            /* The following query string can be used as a template for many others, eg. by
             * supplying phase and/or position manually, or choosing only !games.is_ready rows...
             */
            String sql =  "SELECT *" //maybe something other than "*"?
                       + " FROM games LEFT OUTER JOIN game_participations"
                       + " ON games.id = game_participations.game_id"
                       + " WHERE games.tournament_id = " + tournamentId
                       + " ORDER BY games.phase DESC, games.position;"; // phase in descending order

            return this.Query(sql).Tables[0];
        }


        //public DataTable getTournamentGameTable(long tournamentId, bool isReady = true) //overloaded
        //{
        //    String sql =  "SELECT *" //maybe something other than "*"?
        //               + " FROM games LEFT OUTER JOIN game_participations"
        //               + " ON games.id = game_participations.game_id"
        //               + " WHERE games.tournament_id = " + tournamentId
        //               + " AND games.is_ready = " + isReady
        //               + " ORDER BY games.phase DESC, games.position;"; // phase in descending order
        //
        //    return this.Query(sql).Tables[0];
        //}


        public DataTable getTournamentGameTable(long tournamentId, bool isFinished = false)  // overloaded method
        {
            String sql =  "SELECT *" //maybe something other than "*"?
                       + " FROM games LEFT OUTER JOIN game_participations"
                       + " ON games.id = game_participations.game_id"
                       + " WHERE games.tournament_id = " + tournamentId
                       + " AND games.is_finished = " + isFinished
                       + " ORDER BY games.phase DESC, games.position;"; // phase in descending order

            return this.Query(sql).Tables[0];
        }


        public DataTable getTournamentGameTable(long tournamentId, bool isFinished = false, bool isReady = true)  // overloaded method
        {
            String sql =  "SELECT *" //maybe something other than "*"?
                       + " FROM games LEFT OUTER JOIN game_participations"
                       + " ON games.id = game_participations.game_id"
                       + " WHERE games.tournament_id = " + tournamentId
                       + " AND games.is_ready = " + isReady
                       + " AND games.is_finished = " + isFinished
                       + " ORDER BY games.phase DESC, games.position;"; // phase in descending order

            return this.Query(sql).Tables[0];
        }


        public DataTable getTournamentGameTableWithNames(long tournamentId, bool isFinished = false, bool isReady = true)
        {
            String sql = "SELECT first_name, fathers_name, last_name, phase, position, is_ready, is_finished"
                       + " FROM persons INNER JOIN (SELECT *"
                       +                         "  FROM games LEFT OUTER JOIN game_participations"
                       +                         "  ON games.id = game_participations.game_id"
                       +                         "  WHERE games.tournament_id = " + tournamentId
                       +                         "  AND games.is_ready = " + isReady
                       +                         "  AND games.is_finished = " + isFinished
                       +                         " ) AS gjoined"
                       + " ON persons.id = gjoined.athlete_id"
                       + " ORDER BY gjoined.phase DESC, gjoined.position;"; // phase in descending order

            return this.Query(sql).Tables[0];
        }


        public void printTournamentGameTable(long tournamentId)  // as an example of using getTournamentGameTable()
        {
            /* untested: */
            DataTable dt = getTournamentGameTable(tournamentId);

            Console.WriteLine("Athlete participations (all data), sorted by phase, then position:");
            for (int row=0; row < dt.Rows.Count; ++row)
                Console.WriteLine("id:{0,6}  phase:{1,2}  position:{2,2}  tournament_id:{3,4}  is_ready:{4,6}  "
                                 + "is_finished:{5,6}  athlete_id:{6,4}  team_id:{7,4}  game_id:{8,6}",
                                  dt.Rows[row][0], dt.Rows[row][1], dt.Rows[row][2], dt.Rows[row][3], dt.Rows[row][4],
                                  dt.Rows[row][5], dt.Rows[row][6], dt.Rows[row][7], dt.Rows[row][8]);
        }


        public void printTournamentGameTableWithNames(long tournamentId)  // as an example of using getTournamentGameTableWithNames()
        {
            /* untested: */
            DataTable dt = getTournamentGameTableWithNames(tournamentId, isReady: true);

            Console.WriteLine("Athlete \"ready\" participations (with their names), sorted by phase, then position:");
            for (int row = 0; row < dt.Rows.Count; ++row)
                Console.WriteLine("first_name: {0,4}  fathers_name: {1,4}  last_name: {2,4}  phase:{3,2}  position:{4,2}  "
                                 + "is_ready:{5,6}  is_finished:{6,6}",
                                  dt.Rows[row][0], dt.Rows[row][1], dt.Rows[row][2], dt.Rows[row][3],
                                  dt.Rows[row][4], dt.Rows[row][5], dt.Rows[row][6]);
        }

    }
}
