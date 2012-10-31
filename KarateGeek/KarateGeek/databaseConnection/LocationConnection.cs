using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class LocationConnection : CoreDatabaseConnection
    {
        public string InsertNewLocation(string location , string phone , string email, string addressId)
        {
            string sql;
            DataSet dr = null;

            sql = "insert into locations (id, name , phone, email) values ( '"
                + addressId + "','"
                + location + "', '"
                + phone + "', '"
                + email + "');";

            this.NonQuery(sql);

            return "" + addressId;

        }

        public string UpdateLocation(int id, string location , string phone , string email)
        {
            string sql;

            sql = "update locations set " +
                "name = '" + location + "', " +
                "phone = '" + phone + "', " +
                "email = '" + email + "' " +
                "where id = '" + id + "' ;";

            this.NonQuery(sql);

            return "";
        }

        public DataSet getLocation(int id)
        {
            string sql = "select * from locations where id= '" + id + "';";
            return this.Query(sql);
        }
    }
}
