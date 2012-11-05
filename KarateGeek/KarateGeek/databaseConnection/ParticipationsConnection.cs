using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class ParticipationsConnection : CoreDatabaseConnection
    {
        DataTable participantIDT = new DataTable();
        DataSet dr = null;

        public void InsertNewParticipantI(int athlete_id, int tournament_id , string rank , int position)
        {
            
            string sql = "insert into tournament_participations ( athlete_id, tournament_id , rank_at_time , position , team_participation_id) values ( '"
                + athlete_id + "', '"
                + tournament_id + "', '"
                + rank + "', '"
                + position + "', null );";

            this.NonQuery(sql);
        }

        public void deleteParticipantI(int athlete_id, int tournament_id)
        {
            string sql = "delete from tournament_participations where athlete_id='" + athlete_id + "' and tournament_id='" + tournament_id + "';";
            this.Query(sql);
        }

        public DataSet getParticipantsI(int tournamentId)
        {
            string sql = "select * from tournament_participations inner join persons on athlete_id=id where tournament_id= '" + tournamentId + "';";
            return this.Query(sql);
        }
    }
}
