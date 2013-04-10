﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;


namespace KarateGeek.databaseConnection
{
    class PersonConnection : CoreDatabaseConnection
    {

        protected string insertNewPerson(string firstName, string lastName, string fathersName, string sex,
           string dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email, string addressId)
        {
            string sql;
            DataSet dr = null;

            sql = "insert into Persons (first_name, last_name, fathers_name, sex," +
                "date_of_birth, phone, secondary_phone, email, " +
                "address_id) values ( '"
                + firstName + "', '"
                + lastName + "', '"
                + fathersName + "', '"
                + sex + "', '"
                + dateOfBirth + "', '"
                + primaryPhoneNo + "', '"
                + secondaryPhoneNo + "', '"
                + email + "', '"
                + addressId + "' );";

            this.NonQuery(sql);

            sql = "select currval('persons_id_seq');";
            dr = this.Query(sql);
            long personId = long.Parse( dr.Tables[0].Rows[0][0].ToString() ); 

            return "" + personId;
        }


        protected string updatePerson(int id, string firstName, string lastName, string fathersName, string sex,
           string dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email, int addressId)
        {
            string sql;
        
        sql = "update Persons set " +
                "first_name = '" + firstName + "', " +
                "last_name = '" + lastName + "', " +
                "fathers_name = '" + fathersName + "', " +
                "sex = '" + sex + "', " +
                "date_of_birth = '" + dateOfBirth + "', " +
                "phone = '" + primaryPhoneNo + "', " +
                "secondary_phone = '" + secondaryPhoneNo + "', " +
                "email = '" + email + "'," + "address_id = '" + addressId + "' where id = '" + id + "' ;";

            this.NonQuery(sql);


            return "";
        
        }

        public DataSet similarPersonsNotInAthletes(string filter)
        {
            string sql = "select * from persons where (lower(first_name) like lower('" + filter + "%') or lower(last_name) like lower('" + filter + "%')) and id not in (select id from athletes) order by first_name, last_name;";
            return this.Query(sql);
        }

        public DataSet similarAthletes(string filter)
        {
            string sql = "select * from persons natural join athletes where lower(first_name) like lower('" + filter + "%') or lower(last_name) like lower('" + filter + "%') order by first_name, last_name;";
            return this.Query(sql);
        }

        public DataSet similarPersonsNotInJudges(string filter)
        {
            string sql = "select * from persons where (lower(first_name) like lower('" + filter + "%') or lower(last_name) like lower('" + filter + "%')) and id not in (select id from judges) order by first_name, last_name;";
            return this.Query(sql);
        }

        public DataSet similarJudges(string filter)
        {
            string sql = "select * from persons natural join judges where lower(first_name) like lower('" + filter + "%') or lower(last_name) like lower('" + filter + "%') order by first_name, last_name;";
            return this.Query(sql);
        }
    }
}
