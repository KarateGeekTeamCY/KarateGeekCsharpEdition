using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class DatabaseInitializer : CoreDatabaseConnection
    {
        public DatabaseInitializer()
        {
            dropAll();
            createSchema();
            createCountriesTable();
            createAddressesTable();
            //edw kalountai kai oi ipoloipes
        }

        private Boolean dropAll(){
            string sql = "drop schema IF EXISTS sch1 CASCADE;";
            this.NonQuery(sql);
            return true;
        }

        private Boolean createSchema(){
            string sql = "create schema sch1";
            this.NonQuery(sql);
            return true;
        }

        private Boolean createCountriesTable(){
            string sql = "CREATE TABLE sch1.countries (code char(2) PRIMARY KEY," +
                         "name varchar(80) NOT NULL);";
            this.NonQuery(sql);
            return true;
        }
        
        private Boolean createAddressesTable(){
            string sql = "CREATE TABLE sch1.addresses (id SERIAL PRIMARY KEY, street varchar(50) NOT NULL," +
                         "number varchar(12) NOT NULL, city varchar(50) NOT NULL, postal_code varchar(12) NOT NULL," +
                         "country_code char(2) DEFAULT 'CY' REFERENCES sch1.countries);";
            this.NonQuery(sql);
            return true;
        }

        private Boolean createPersonsTable(){
            string sql = "CREATE TABLE sch1.persons (id SERIAL PRIMARY KEY, first_name varchar(50) NOT NULL," +
                        "middle_name varchar(50) NOT NULL, last_name varchar(50) NOT NULL, date_of_birth date," +
                        "phone char(15) NOT NULL, secondary_phone char(15), email varchar(50), address_id integer NOT NULL REFERENCES sch1.add resses," +
                        "CHECK (email ~* '^[_a-zA-Z0-9-+]+(.[_a-zA-Z0-9-+]+)*@[a-zA-Z0-9-]+(.[a-zA-Z0-9-]+)*.([a-zA-Z]{2,6})$'));";
            this.NonQuery(sql);
            return true;
        }

        private Boolean createAthletesTable(){
            string sql = "CREATE TABLE sch1.athletes (id int REFERENCES sch1.persons ON DELETE NO ACTION, " +
                         "rank varchar(50) NOT NULL, club_id integer NOT NULL REFERENCES sch1.clubs);";
            this.NonQuery(sql);
            return true;
        }

         private Boolean createJudgesTable(){
            string sql = "CREATE TABLE sch1.judges (id int REFERENCES sch1.persons ON DELETE NO ACTION, " +
                         "rank varchar(50) NOT NULL, class character(1) NOT NULL);";
            this.NonQuery(sql);
            return true;
        }

        private Boolean createLocationsTable(){
            string sql = "CREATE TABLE sch1.locations (id integer REFERENCES sch1.addresses," +
                         "name varchar(80),phone char(15), description varchar(255));";
            this.NonQuery(sql);
            return true;
        }

         private Boolean createEventsTable(){
            string sql = "CREATE TABLE sch1.events (id SERIAL PRIMARY KEY,name varchar(80) NOT NULL," +
                         "official boolean NOT NULL DEFAULT true, day timestamp NOT NULL description varchar(255) " +
                         "location integer REFERENCES sch1.locations);";
            this.NonQuery(sql);
            return true;
        }


        private Boolean createTournamentsTable(){
            string sql = "CREATE TABLE sch1.tournaments (id SERIAL PRIMARY KEY,rank_start varchar(50) NOT NULL," + 
                         "rank_end varchar(50), age_start smallint NOT NULL, age_end smallint, event_id integer REFERENCES sch1.events);";
            this.NonQuery(sql);
            return true;
        }

        private Boolean createGame_TypesTable(){
            string sql = "CREATE TABLE sch1.game_types (id int PRIMARY KEY,);";
            this.NonQuery(sql);
            return true;
        }

        private Boolean createGamesTable(){
            string sql = "CREATE TABLE sch1.games (id int PRIMARY KEY,);";
            this.NonQuery(sql);
            return true;
        }

        private Boolean createTourParticipationsTable(){
            string sql = "CREATE TABLE sch1.tour_participations (id int PRIMARY KEY,);";
            this.NonQuery(sql);
            return true;
        }

        private Boolean createGameParticipationsTable(){
            string sql = "CREATE TABLE sch1.tour_participations (id int PRIMARY KEY,);";
            this.NonQuery(sql);
            return true;
        }

         private Boolean createGameScoresTable(){
            string sql = "CREATE TABLE sch1.tour_participations (id int PRIMARY KEY,);";
            this.NonQuery(sql);
            return true;
        }

         private Boolean createTechnicalPointsTable(){
            string sql = "CREATE TABLE sch1.tour_participations (id int PRIMARY KEY,);";
            this.NonQuery(sql);
            return true;
        }

         private Boolean createParticipationTeamsTable(){
            string sql = "CREATE TABLE sch1.tour_participations (id int PRIMARY KEY,);";
            this.NonQuery(sql);
            return true;
        }

         private Boolean createClubsTable(){
            string sql = "CREATE TABLE sch1.tour_participations (id int PRIMARY KEY,);";
            this.NonQuery(sql);
            return true;
        }

         private Boolean createAppUsersTable(){
            string sql = "CREATE TABLE sch1.app_users (username varchar(50) PRIMARY KEY, " +
                         "password char(32) NOT NULL, person_mngment boolean NOT NULL DEFAULT false, " +
                         "event_mngment boolean NOT NULL DEFAULT false, lottery boolean NOT NULL DEFAULT false, " +
                         "game_support boolean NOT NULL DEFAULT false, reports boolean NOT NULL DEFAULT false, " +
                         "settings boolean NOT NULL DEFAULT false);";
            this.NonQuery(sql);
            return true;
        }

        private Boolean createAppSetupTable(){
            string sql = "CREATE TABLE sch1.app_setup (id boolean PRIMARY KEY, salt char(50));";
            this.NonQuery(sql);
            return true;
        }


    

    }
}
