using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class DatabaseInitializer : CoreDatabaseConnection
    {

        private Cryptography cr = new Cryptography();
        private Boolean dropAll()
        {
            this.NonQuery("drop schema_v1 cascade;");
            return true;
        }

        private Boolean createCountryTable()
        {
            this.NonQuery("CREATE TABLE country(code character(2) NOT NULL," + 
                  "name character varying(50),CONSTRAINT country_pkey PRIMARY KEY (code));");
            return true;
        }

        private Boolean createAddressesTable()
        {
            this.NonQuery("CREATE TABLE addresses(id integer NOT NULL,address_street character varying(200)," +
                 "address_number character varying(10), address_city character varying(20)," + 
                 "address_postal_code character varying(10), CONSTRAINT addresses_pkey PRIMARY KEY (id)," + 
                 "CONSTRAINT addresses_country_code_fkey FOREIGN KEY (country_code) REFERENCES country (code) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION);");
            return true;
        }
        
        private Boolean createPersonsTable()
        {
            this.NonQuery("create table persons ("+
                "id int, first_name character varying(50), middle_name character varying(50), " +
                "last_name character varying(50), date_of_birth date(), phone character varying(20)" +
                "secondary_phone integer, email character varying(25), address_id integer," + 
                "CONSTRAINT persons_pkey PRIMARY KEY (id)," + 
                "CONSTRAINT persons_address_id_fkey FOREIGN KEY (address_id) REFERENCES addresses (id) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION);");
            return true;
        }

        private Boolean createApplication_users_Table()
        {
            this.NonQuery("CREATE TABLE application_users(person_id integer NOT NULL," +
            "password character varying(25),person_management bit(1),event_management bit(1), " +
            "lottery bit(1), game_support bit(1), reports bit(1),  settings bit(1), " + "CONSTRAINT application_users_pkey PRIMARY KEY (person_id));");

            return true;
        }

    }
}
