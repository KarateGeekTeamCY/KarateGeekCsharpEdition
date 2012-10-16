using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class CoreDatabaseConnection
    {
        private NpgsqlConnection _conn = null;


        public NpgsqlConnection conn
        {
            get
            {
                if (_conn == null)
                    _conn = new NpgsqlConnection("Server=127.0.0.1; Port=5432; User Id=postgres; Password=admin; Database=karategeek;");
                if (_conn.State != ConnectionState.Open)
                    _conn.Open();
                return _conn;

            }
            set
            {
                _conn = value;
            }
        }



        public Boolean Disconnect()
        {
            _conn.Close();
            _conn = null;
            return true;
        }

        protected Boolean NonQuery(string sql)
        {
            NpgsqlCommand comm = this.conn.CreateCommand();
            comm.CommandText = sql;
            comm.ExecuteNonQuery();
            return true;
        }

        protected DataSet Query(string sql)
        {
            //NpgsqlCommand comm = this.conn.CreateCommand();
            //comm.CommandText = sql;

            //string queryString = "SELECT CustomerID, CompanyName FROM dbo.Customers";
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, conn);

            DataSet results = new DataSet();

            adapter.Fill(results, "Customers");

            this.Disconnect();

            return results;
        }


    }
}
