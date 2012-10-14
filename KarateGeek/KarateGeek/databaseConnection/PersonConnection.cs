using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class PersonConnection : CoreDatabaseConnection
    {

        protected string insertNewPerson(string firstName, string middleName, string lastName,
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email, string addressId)
        {
            string sql;
            NpgsqlDataReader dr = null;

            sql = "insert into table Persons (first_name, middle_name, last_name, " +
                "date_of_birth, primary_phone_number, secondary_phone_number, email, " +
                "address_id) values ( '"
                + firstName + "', '"
                + middleName + "', '"
                + lastName + "', '"
                + dateOfBirth.ToShortDateString() + "', '"
                + primaryPhoneNo + "', '"
                + secondaryPhoneNo + "', '"
                + email + "', '"
                + addressId + "' );";

            this.NonQuery(sql);

            sql = "select currval('persons_id_seq');";
            dr = this.Query(sql);
            dr.Read();
            long personId = dr.GetInt64(0);

            return "" + personId;
        }


        protected string updatePerson(string id, string firstName, string middleName, string lastName,
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email)
        {
            string sql;
        
        sql = "update Persons set " +

                "middle_name = '" + middleName + "', " +
                "last_name = '" + lastName + "', " +
                "date_of_birth = '" + dateOfBirth.ToShortDateString() + "', " +
                "primary_phone_number = '" + primaryPhoneNo + "', " +
                "secondary_phone_number = '" + secondaryPhoneNo + "', " +
                "email = '" + email + "' where id = '" + id + "' ;";

            this.NonQuery(sql);


            return "";
        
        }



    }
}
