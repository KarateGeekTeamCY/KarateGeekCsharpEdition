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


        public List<Tuple<List<long>, bool, int, int>> getPrintableLotterySetsFromDB(long tournamentId)    // not implemented yet
        {
            return null;
        }

    }
}
