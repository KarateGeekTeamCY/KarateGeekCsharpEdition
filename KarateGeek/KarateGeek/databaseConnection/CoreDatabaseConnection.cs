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
        public NpgsqlConnection conn { get; set; }


        protected Boolean Connect()
        {
            conn = new NpgsqlConnection("Server=127.0.0.1; Port=5432; User Id=postgres; Password=admin; Database=karategeek;");
            conn.Open();
            return true;
        }



        protected Boolean Disconnect()
        {
            this.conn.Clone();
            return true;
        }

        protected Boolean NonQuery(string sql)
        {
            NpgsqlCommand comm = this.conn.CreateCommand();
            comm.CommandText = sql;
            comm.ExecuteNonQuery();
            return true;
        }

        protected NpgsqlDataReader Query(string sql)
        {
            NpgsqlCommand comm = this.conn.CreateCommand();
            comm.CommandText = sql;
            return comm.ExecuteReader();
        }
    }
}
