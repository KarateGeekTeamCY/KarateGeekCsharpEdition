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



        public string UpdateAddress(string id, string countryCode, string City, string addressStreetName, string addressStreetNumber, string addressPostalCode)
        {

            string sql = "update addresses set " +

                "city = '" + City + "', " +
                "street_name = '" + addressStreetName + "', " +
                "street_number = '" + addressStreetNumber + "', " +
                "street_postal_code = '" + addressPostalCode + "', " +
                "country_code = '" + countryCode + "' where id = '" + id + "' ;";

            this.NonQuery(sql);

            return "";
        }

    }
}
