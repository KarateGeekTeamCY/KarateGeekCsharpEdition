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

        public string InsertNewCity(string name)
        {
            sql = "insert into cities (name) values ('" + name + "');" ;
            this.NonQuery(sql);
            
            return "";
        }

        public string UpdateCity(int id, string name)
        {
            sql = "update cities set name = '" + name + "' where id = '" + id + "' ;";

            return "";
        }

        public DataSet GetCities()
        {
            sql = "select * from cities";
            return this.Query(sql);
        }
    }
}
