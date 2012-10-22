using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class CountryConnection : CoreDatabaseConnection
    {

        private string sql = "";

        public string InsertNewCountry(string code, string name)
        {
            sql = "insert into countries (code, name) values ('" +code + "', '" + name + "');";
            this.NonQuery(sql);

            return "";
        }

        public string UpdateCountry(int code, string name)
        {
            sql = "update countries set name = '" + name + "' where code = '" + code + "' ;";

            return "";
        }

        public DataSet GetCountries()
        {
            sql = "select * from countries";
            return this.Query(sql);
        }


        public DataSet getCountryNameByCode(string code)
        {
            sql = "select name from countries where code = '" + code + "';";
            return this.Query(sql);

            //return "";
        }



    }
}
