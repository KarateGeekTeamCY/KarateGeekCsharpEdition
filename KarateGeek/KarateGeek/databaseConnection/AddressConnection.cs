using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class AddressConnection : CoreDatabaseConnection
    {
        public string InsertNewAddress(string countryCode, string City, string addressStreetName, string addressStreetNumber, string addressPostalCode)
        {
            string sql;
            NpgsqlDataReader dr = null;

            sql = "insert into table addresses (city, street_name, street_number, " +
                "street_postal_code, country_code ) values ( '"
                + City + "', '"
                + addressStreetName + "', '"
                + addressStreetNumber + "', '"
                + addressPostalCode + "', '"
                + countryCode + "' );";

            this.NonQuery(sql);

            sql = "select currval('addresses_id_seq');";
            dr = this.Query(sql);
            dr.Read();
            long addressId = dr.GetInt64(0);

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
