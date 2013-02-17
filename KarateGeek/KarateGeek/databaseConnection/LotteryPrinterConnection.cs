using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KarateGeek.databaseConnection;
using Npgsql;

using System.Data;          // DataSet, DataTable
using System.Diagnostics;   // has Debug.WriteLine()

namespace KarateGeek.databaseConnection
{
    class LotteryPrinterConnection : CoreDatabaseConnection
    {
        public List<string> getAthleteNameList(Tuple<List<long>, bool, int, int> input, int emptyListSize = 1)
        {
            if (emptyListSize < 1) emptyListSize = 1;   // workaround for a crashing bug (that would never occur with real-world data anyway)

            if (input == null || input.Item1.ToList().Count == 0) {
                List<string> l = new List<string>();
                for (int i = 0; i < emptyListSize; ++i)
                    l.Add("");

                return l;
            }

            List<long> ids = input.Item1;

            string sql;
            List<string> athleteNameList = new List<string>();

            foreach(var id in ids) {
                sql = "SELECT last_name, first_name "
                    + "FROM athletes NATURAL JOIN persons "
                    + "WHERE id = " + id + " ;";

                var firstlast = this.Query(sql).Tables[0].Rows[0];

                athleteNameList.Add(
                    firstlast[0].ToString() + " " + firstlast[1].ToString()
                );
            }

            return athleteNameList;
        }


        //public List<Tuple<List<long>, bool, int, int>> getPrintableLotterySetsFromDB(long tournamentId)
        //{
        //    string sql = "SELECT athlete_id, is_ready, phase, position " // ", team_id" could be added here
        //               + "FROM tournaments t "
        //               + "JOIN games g ON t.id = g.tournament_id "
        //               + "LEFT JOIN game_participations gp ON gp.game_id = g.id "
        //               + "WHERE tournament_id = " + tournamentId
        //               + " ORDER BY g.phase DESC, g.position; ";

        //    var result = this.Query(sql).Tables[0];

        //    var sets = new List<Tuple<List<long>, bool, int, int>>();

        //    var team = new List<long>();

        //    bool isReady, firstTime = true;
        //    int prevPhase = -1, phase, prevPosition = -1, position;
        //    string id;

        //    foreach (DataRow row in result.Rows)
        //    {
        //        isReady = bool.Parse(row[1].ToString());
        //        phase = int.Parse(row[2].ToString());
        //        position = int.Parse(row[3].ToString());

        //        if (firstTime)
        //        {
        //            prevPhase = phase;
        //            prevPosition = position;
        //            firstTime = false;
        //        }

        //        if (prevPhase == phase && prevPosition == position) {
        //            id = row[0].ToString();
        //            if(!string.IsNullOrEmpty(id))
        //                team.Add(long.Parse(id));
        //        } else {
        //            sets.Add(new Tuple<List<long>, bool, int, int>(team, isReady, phase, position));
        //            team = new List<long>(); // team.Clear() is fucked up

        //            id = row[0].ToString();
        //            if (!string.IsNullOrEmpty(id))
        //                team.Add(long.Parse(id));
        //        }

        //        prevPhase = phase;
        //        prevPosition = position;
        //    }

        //    return sets;
        //}


        public List<Tuple<List<long>, bool, int, int>> getPrintableLotterySetsFromDB(long tournamentId)
        {
            string sql = "SELECT athlete_id, is_ready, phase, position " // ", team_id" could be added here
                       + "FROM tournaments t "
                       + "JOIN games g ON t.id = g.tournament_id "
                       + "LEFT JOIN game_participations gp ON gp.game_id = g.id "
                       + "WHERE tournament_id = " + tournamentId
                       + " ORDER BY g.phase DESC, g.position; ";

            var result = this.Query(sql).Tables[0];

            var sets = new List<Tuple<List<long>, bool, int, int>>();

            var team = new List<long>();

            bool isReady = true;                    // initialisation of isReady does not matter; it stops the IDE from complaining
            int phase = -1, prevPhase = -1,
                position = -1, prevPosition = -1;   // initialisation does not matter; it stops the IDE from complaining
            string id;

            foreach (DataRow row in result.Rows)
            {
                isReady = bool.Parse(row[1].ToString());    // assumed to stay the same within a "team" list
                phase = int.Parse(row[2].ToString());
                position = int.Parse(row[3].ToString());

                if ((prevPhase != phase || prevPosition != position) && team.Count > 0) {
                    sets.Add(new Tuple<List<long>, bool, int, int>(team, isReady, prevPhase, prevPosition));
                    team = new List<long>();                // team.Clear() is fucked up
                }

                id = row[0].ToString();
                if (!string.IsNullOrEmpty(id))
                    team.Add(long.Parse(id));

                prevPhase = phase;
                prevPosition = position;
            }

            /* adding the last team: */
            if (team.Count > 0)
                sets.Add(new Tuple<List<long>, bool, int, int>(team, isReady, phase, position));

            Debug.Assert(sets.Count > 0);   // What should we do here? (this is false sometimes, eg. Team kumite, and may cause further crashes)
            return sets;
        }


        public void savePrintableLotteryString(long tournamentId, string graph)
        {
            string sql = "INSERT INTO lottery_graph VALUES ('" + tournamentId + "', '" + graph + "');";

            this.NonQuery(sql);
        }


        public void updatePrintableLotteryString(long tournamentId, string graph)
        {
            string sql = "UPDATE lottery_graph SET graph = '" + graph + "' "
                       +" WHERE id = '" + tournamentId + "';";

            this.NonQuery(sql);
        }

    }
}
