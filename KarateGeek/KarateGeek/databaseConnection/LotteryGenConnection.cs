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


        public int getNumOfGoodPlacements(long athleteId, int place, Boolean official)
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
            String unfinishedSql = "SELECT club_id FROM athletes WHERE id = ";

            String club1 = this.Query(unfinishedSql + athPair.Item1 + " ;").Tables[0].Rows[0][0].ToString();
            String club2 = this.Query(unfinishedSql + athPair.Item2 + " ;").Tables[0].Rows[0][0].ToString();

            return String.IsNullOrWhiteSpace(club1) ? false : club1.Equals(club2, StringComparison.Ordinal);
        }


        /* Writes a tournament pair to the database.
         * CONVENTION: For semi-complete pairs, the caller provides a negative athlete id. */
        private void writeTournamentPair(long id1, long id2, int phase, int position, long tournamentId)
        {
            /* UNTESTED!! */
            String writegame = "INSERT INTO games (phase, position, tournament_id ) "
                             + "VALUES ( " + phase + ", " + position + ", " + tournamentId + " );";
            String writepair_first  = "INSERT INTO game_participations (athlete_id, team_id, game_id ) "
                                    + "VALUES ( " + id1 + ", NULL, ( SELECT currval('games_id_seq') ));";
            String writepair_second = "INSERT INTO game_participations (athlete_id, team_id, game_id ) "
                                    + "VALUES ( " + id2 + ", NULL, ( SELECT currval('games_id_seq') ));";

            if (id1 >= 0 || id2 >= 0) this.NonQuery(writegame);
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

    }
}
