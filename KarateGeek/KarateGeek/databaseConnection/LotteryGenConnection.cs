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

            if (dt.Rows.Count == 0) throw new System.Exception("No tournament participants found."); //ouch

            List<long> L = new List<long>();

            for (int index = 0; index < dt.Rows.Count; ++index)
                L.Add(long.Parse(dt.Rows[index][0].ToString()));

            Debug.WriteLine("Tournament participants in List<long> :");
            foreach(long element in L) //debug
                Debug.WriteLine(element);

            return L;
        }
    }
}
