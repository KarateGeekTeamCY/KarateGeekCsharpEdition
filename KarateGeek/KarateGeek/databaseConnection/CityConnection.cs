using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class CityConnection : CoreDatabaseConnection
    {
        private string  sql = "";

        public string InsertNewCity(string countryCode, string name)
        {
            sql = "insert into cities (name, country_code) values ('" + name + "', '" + countryCode + "');" ;
            this.NonQuery(sql);
            
            return "";
        }

        public string UpdateCity(int id, string name)
        {
            sql = "update cities set name = '" + name + "' where id = '" + id + "' ;";

            return "";
        }

        public DataSet GetCities(string countryId)
        {
            sql = "select * from cities where country_code = '" + countryId + "' ";
            return this.Query(sql);
        }

        public DataSet GetCitiesByCountryId(int id)
        {
            sql = "select * from cities where country_code = '" + id + "' ";
            return this.Query(sql);
        }

        public DataSet GetCityNameByCityId(int id)
        {
            sql = "select name from cities where id = '" + id + "' ";
            return this.Query(sql);
        }
    }
}
