using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class AddressConnection : CoreDatabaseConnection
    {
        public string InsertNewAddress(string addressStreetName, string addressStreetNumber, string City, string addressPostalCode, string countryCode)
        {
            string sql;
            DataSet dr = null;

            sql = "select id from cities where name = '" + City + "'; ";
            dr = this.Query(sql);
            long cityId = long.Parse(dr.Tables[0].Rows[0][0].ToString()); 
            
            sql = "insert into addresses (street, number, city_id," +
                "postal_code, country_code ) values ( '"
                + addressStreetName + "', '"
                + addressStreetNumber + "', '"
                + cityId + "', '"
                + addressPostalCode + "','"
                + countryCode + "' );";

            this.NonQuery(sql);


            sql = "select currval('addresses_id_seq');";
            dr = this.Query(sql);
            long addressId = long.Parse(dr.Tables[0].Rows[0][0].ToString()); 

            return "" + addressId;

        }



        public string UpdateAddress(int id, string addressStreetName, string addressStreetNumber, string City, string addressPostalCode, string countryCode)
        {
            string sql;
            DataSet dr = null;

            sql = "select id from cities where name = '" + City + "'; ";
            dr = this.Query(sql);
            long cityId = long.Parse(dr.Tables[0].Rows[0][0].ToString()); 

            sql = "update addresses set " +

                "street = '" + addressStreetName + "', " +
                "number = '" + addressStreetNumber + "', " +
                "city_id = '" + cityId + "', " +
                "postal_code = '" + addressPostalCode + "', " +
                "country_code = '" + countryCode + "' where id = '" + id + "' ;";

            this.NonQuery(sql);

            return "";
        }

        public DataSet getAddress(int id)
        {
            string sql = "select * from addresses where id= '" + id + "';";
            return this.Query(sql);
        }

        public void deleteEventAddress(int addressId , int eventId)
        {
            DataSet dsEvents = null;
            DataSet dsPersons = null;
            DataSet dsClubs = null;
            string sql1 = "select * from events where id != '" + eventId + "' and location_id = '" + addressId + "';";

            dsEvents = this.Query(sql1);

            string sql2 = "select * from persons where address_id = '" + addressId + "';";

            dsPersons = this.Query(sql2);

            string sql3 = "select * from clubs where address_id = '" + addressId + "';";

            dsClubs = this.Query(sql3);


            if (dsEvents.Tables[0].Rows.Count == 0 && dsPersons.Tables[0].Rows.Count == 0 && dsClubs.Tables[0].Rows.Count == 0)
            {
                //delete addresses and locations cascade
                string sql = "delete from addresses where id ='" + addressId + "';";
                this.Query(sql);
            }
        }

    }
}
