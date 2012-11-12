using System;
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
           DateTime dateOfBirth,
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
                + dateOfBirth.ToShortDateString() + "', '"
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
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email, int addressId)
        {
            string sql;
        
        sql = "update Persons set " +
                "first_name = '" + firstName + "', " +
                "last_name = '" + lastName + "', " +
                "fathers_name = '" + fathersName + "', " +
                "sex = '" + sex + "', " +
                "date_of_birth = '" + dateOfBirth.ToShortDateString() + "', " +
                "phone = '" + primaryPhoneNo + "', " +
                "secondary_phone = '" + secondaryPhoneNo + "', " +
                "email = '" + email + "'," + "address_id = '" + addressId + "' where id = '" + id + "' ;";

            this.NonQuery(sql);


            return "";
        
        }


        public DataSet findSimilar(string filter)
        {
            string sql = "select * from persons where first_name like '" + filter + "%';";
            return this.Query(sql);
        }
    }
}
