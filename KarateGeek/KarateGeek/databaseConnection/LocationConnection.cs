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

            sql = "insert into locations (name , phone, email," +
                "address_id) values ( '"
                + location + "', '"
                + phone + "', '"
                + email + "', '"
                + addressId + "');";

            this.NonQuery(sql);


            sql = "select currval('locations_id_seq');";
            dr = this.Query(sql);
            long locationId = long.Parse(dr.Tables[0].Rows[0][0].ToString());

            return "" + locationId;

        }

        public string UpdateLocation(int id, string location , string phone , string email, int addressId)
        {
            string sql;
            DataSet dr = null;

            sql = "update locations set " +

                "name = '" + location + "', " +
                "phone = '" + phone + "', " +
                "email = '" + email + "', " +
                "address_id = '" + addressId + "'where id = '" + id + "' ;";

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
