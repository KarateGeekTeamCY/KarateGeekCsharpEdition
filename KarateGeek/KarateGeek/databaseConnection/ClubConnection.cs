using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;
using System.IO;
using KarateGeek.helpers;

namespace KarateGeek.databaseConnection
{

    class ClubConnection : CoreDatabaseConnection
    {
        public bool insertNewCLub(string name, string phone, string email, string addressStreetName, string addressStreetNumber, string addressPostalCode , string countryCode, string City)
        {
            AddressConnection addConn = new AddressConnection();
            string addressId = addConn.InsertNewAddress(addressStreetName, addressStreetNumber, City, addressPostalCode, countryCode);

            string sql = "insert into Clubs (name, phone, email, address_id) values ("+
                "'" + name + "', " +
                "'" + phone + "', " +
                "'" + email + "', " +
                "'" + addressId + "')";
            
            return this.NonQuery(sql);
        }

        public DataSet findSimilar(string filter)
        {
            string sql = "select * from clubs where name like lower('" + filter + "%') order by name;";
            return this.Query(sql);
        }

        public bool updateClub(int id,string name, string phone, string email, string addressStreetName, string addressStreetNumber, string addressPostalCode, string countryCode, string City)
        {
            DataSet dr = null;
            string sql1 = "select address_id from clubs where id = '" + id + "'; ";
            
            dr = this.Query(sql1);
            int addressId = int.Parse(dr.Tables[0].Rows[0][0].ToString());

            AddressConnection addConn = new AddressConnection();
            addConn.UpdateAddress(addressId, addressStreetName, addressStreetNumber, City, addressPostalCode, countryCode);

            string sql2 = "update clubs set " +
                "name = '" + name + "', " +
                "phone = '" + phone + "', " +
                "email = '" + email + "', " +
                "address_id = '" + addressId + "' where id = '" + id + "' ;";

            return this.NonQuery(sql2);
        }


        public bool deleteClub(int id)
        {
            DataSet ds = this.Query("select * from athletes where club_id = '" + id + "';");
            if (ds.Tables[0].Rows.Count == 0)
            {
                string sql = "delete from clubs where id = '" + id + "';";
                return this.NonQuery(sql);
            }
            else
            {
                return false;
            }
        }

        public DataSet getClubs()
        {
            string sql = "select * from clubs order by name;";
            return this.Query(sql);
        }
    }

}
