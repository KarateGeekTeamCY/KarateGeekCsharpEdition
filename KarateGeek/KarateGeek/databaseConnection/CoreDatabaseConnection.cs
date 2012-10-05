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
        public NpgsqlConnection conn
        {
            get
            {
                if (conn != null || conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                    conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgress;Password=admin;Database=karateGeek;");

                conn.Open();
                return conn;
            }
            set
            {
                conn = value;
            }
        }


        protected Boolean Close()
        {
            this.conn.Clone();
            return true;
        }

        protected Boolean NonQuire(string sql)
        {
            NpgsqlCommand comm = this.conn.CreateCommand();
            comm.CommandText = sql;
            comm.ExecuteNonQuery();
            return true;
        }

        protected NpgsqlDataReader Quire(string sql)
        {
            NpgsqlCommand comm = this.conn.CreateCommand();
            comm.CommandText = sql;
            return comm.ExecuteReader();
        }
    }
}
