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
                //+ " AND ranking = 1;";

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
            /* If you don't care only about official tournaments, use the following simplified query: */
            //String sql = "SELECT ranking FROM tournament_participations WHERE athlete_id = " + athleteId
            //+ " AND ranking = 1;";

            String sql = "SELECT rank FROM athletes WHERE id = " + athleteId + " ;";

            return this.Query(sql).Tables[0].Rows[0].ToString();
        }

    }
}
