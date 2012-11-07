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

        public void InsertNewParticipantI(int athlete_id, int tournament_id , string rank , int position)
        {

            string sql = "insert into tournament_participations ( athlete_id, tournament_id , rank_at_time , ranking, team_id ) values ( '"
                + athlete_id + "', '"
                + tournament_id + "', '"
                + rank + "', '"
                + position + "', null );";

            this.NonQuery(sql);
        }

        public void InsertNewTeam(int ranking, int tournament_id)
        {
            string sql = "insert into team_tournament_participations ( ranking , tournament_id ) values ( '"
                + ranking + "', '"
                + tournament_id + "');";

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

        public DataSet getTeams(int tournamentId)
        {
            string sql = "select * from tournament_participations inner join persons on athlete_id=id where tournament_id= '" + tournamentId + "';";
            return this.Query(sql);
        }

        public DataSet findPotentialParticipants(string sex, int ageFrom, int ageTo, int levelFrom, int levelTo, int tournamentId)
        {
            string filter = "where";
            string sql = null;
            int num = 0;
            if (!sex.Equals(""))
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

            if (levelFrom > 0 && levelTo > 0)
            {
                if (num == 0)
                {

                    for (int i = levelFrom; i < levelTo; i++)
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
                        if (levelFrom == levelTo && levelFrom > 0)
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

            if (tournamentId != 0)
            {
                sql = "select * from (select persons.id as persons_id,first_name,last_name,sex , extract(year from age(date_of_birth)) as age, persons.phone as persons_phone,secondary_phone,persons.email as persons_email,rank, club_id from persons inner join athletes on (persons.id = athletes.id)) as t1 " + filter + " and persons_id not in(select athlete_id from tournament_participations where tournament_id='" + tournamentId + "');";
            }
            else
            {
                sql = "select * from (select persons.id as persons_id,first_name,last_name,sex , extract(year from age(date_of_birth)) as age, persons.phone as persons_phone,secondary_phone,persons.email as persons_email,rank, club_id from persons inner join athletes on (persons.id = athletes.id)) as t1 " + filter + ";";
            }

           
            return this.Query(sql);

        }
    }
}
