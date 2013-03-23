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

        public void InsertNewParticipantI(int athlete_id, int tournament_id , string rank)
        {

            string sql = "insert into tournament_participations ( athlete_id, tournament_id, rank_at_time) values ( '"
                + athlete_id + "', '"
                + tournament_id + "', '"
                + rank + "' );";

            this.NonQuery(sql);
        }

        public void InsertNewParticipantT(int athlete_id, int tournament_id, string rank, int team)
        {

            string sql = "insert into tournament_participations ( athlete_id, tournament_id , rank_at_time, team_id ) values ( '"
                + athlete_id + "', '"
                + tournament_id + "', '"
                + rank + "', '"
                + team + "');";

            this.NonQuery(sql);
        }

        public int InsertNewTeam(int team , int tournament_id)
        {
            DataSet dr;
            string sql = "insert into team_tournament_participations (  team ,tournament_id ) values ( '"
                + team + "', '"
                + tournament_id + "');";

            this.NonQuery(sql);


            sql = "select currval('team_tournament_participations_id_seq');";
            dr = this.Query(sql);
            int eventId = int.Parse(dr.Tables[0].Rows[0][0].ToString());

            return eventId;
        }

        public void deleteParticipantI(int athlete_id, int tournament_id)
        {
            string sql = "delete from tournament_participations where athlete_id='" + athlete_id + "' and tournament_id='" + tournament_id + "';";
            this.Query(sql);
        }

        public string UpdateParticipationsI(int tournamentId)
        {
            string sql = "update tournament_participations set " +
                 "tournament_id = '" + tournamentId + "'" +
                  "where tournament_id = '1' ;";

            this.NonQuery(sql);

            return "";
        }

        public string UpdateParticipationsT(int tournamentId)
        {
            string sql = "update team_tournament_participations set " +
                 "tournament_id = '" + tournamentId + "'" +
                  "where tournament_id = '1' ;";

            this.NonQuery(sql);
            return "";
        }

        public DataSet getParticipantsI(int tournamentId)
        {
            string sql = "select * from tournament_participations inner join persons on athlete_id=id where tournament_id= '" + tournamentId + "';";
            return this.Query(sql);
        }

        public DataSet getParticipantsT(int tournamentId)
        {
            string sql = "select * from(select athlete_id,team from team_tournament_participations inner join tournament_participations on id=team_id and tournament_participations.tournament_id= '" + tournamentId + "' ) as t1 inner join persons on id=athlete_id;";
            return this.Query(sql);
        }

        public int getNumTeams(int tournamentId)
        {
            DataSet ds;
            string sql = "select count(*) from team_tournament_participations where tournament_id= '" + tournamentId + "';";
            ds = this.Query(sql);
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }

        public void deleteLastNRows(int rows, int tournamentId)
        {
            string sql = "delete from team_tournament_participations where id in(select id from team_tournament_participations where tournament_id = '" + tournamentId + "' order by id desc limit " + rows + ");";
            this.Query(sql);
          
        }

        public DataSet getTeams(int tournamentId)
        {
            string sql = "select * from tournament_participations inner join persons on athlete_id=id where tournament_id= '" + tournamentId + "';";
            return this.Query(sql);
        }

        public int getTeamId(int team, int tournamentId)
        {
            DataSet ds;
            string sql = "select id from team_tournament_participations where team= '" + team + "' and tournament_id = '" + tournamentId + "';";
            ds = this.Query(sql);
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }

        public DataSet findPotentialParticipants(string sex, int ageFrom, int ageTo, int levelFrom, int levelTo, int tournamentId)
        {
            string filter = "where";
            string sql = null;
            int num = 0;
            if (sex != null && sex != Strings.mixed)
            {
                if (num == 0)
                {
                    filter = filter + " sex = '" + sex + "'";
                    num++;
                }
                else
                {
                    filter = filter + " and sex = '" + sex + "'";
                }
            }
            if (ageFrom != 0)
            {
                if (num == 0)
                {
                    filter = filter + " age >= '" + ageFrom + "'";
                    num++;
                }
                else
                {
                    filter = filter + " and age >= '" + ageFrom + "'";
                }
            }

            if (ageTo != 0)
            {
                if (num == 0)
                {
                    filter = filter + " age <= '" + ageTo + "'";
                    num++;
                }
                else
                {
                    filter = filter + " and age <= '" + ageTo + "'";
                }
            }

            if ((levelFrom >= 0 && levelTo >= 0 ))
            {
                if (num == 0)
                {

                    for (int i = levelFrom; i <= levelTo; i++)
                    {
                        if (i == levelFrom)
                        {
                            filter = filter + " rank = '" + KarateGeek.Strings.rank[i] + "'";
                        }
                        else
                        {
                            filter = filter + " or rank = '" + KarateGeek.Strings.rank[i] + "'";
                        }

                    }
                    num++;

                }
                else
                {
                    for (int i = levelFrom; i <= levelTo; i++)
                    {
                        if (levelFrom == levelTo && levelFrom >= 0)
                            filter = filter + " and rank = '" + KarateGeek.Strings.rank[levelFrom] + "'";
                        else if (i == levelFrom)
                            filter = filter + " and (rank = '" + KarateGeek.Strings.rank[i] + "'";
                        else if (i == levelTo)
                            filter = filter + " or rank = '" + KarateGeek.Strings.rank[i] + "')";
                        else
                            filter = filter + " or rank = '" + KarateGeek.Strings.rank[i] + "'";
                    }

                }
            }

            if (num > 0)
            {
                sql = "select * from (select persons.id as persons_id,first_name,last_name,sex , extract(year from age(date_of_birth)) as age, persons.phone as persons_phone,secondary_phone,persons.email as persons_email,rank, club_id from persons inner join athletes on (persons.id = athletes.id)) as t1 " + filter + " and persons_id not in(select athlete_id from tournament_participations where tournament_id='" + tournamentId + "');";
            }
            else
            {
                sql = "select * from (select persons.id as persons_id,first_name,last_name,sex , extract(year from age(date_of_birth)) as age, persons.phone as persons_phone,secondary_phone,persons.email as persons_email,rank, club_id from persons inner join athletes on (persons.id = athletes.id)) as t1 where persons_id not in(select athlete_id from tournament_participations where tournament_id='" + tournamentId + "');";
            }

           
            return this.Query(sql);

        }
    }
}
