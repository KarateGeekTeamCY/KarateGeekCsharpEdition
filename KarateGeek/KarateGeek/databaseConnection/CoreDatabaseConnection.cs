using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data;          // DataSet, DataTable
using Npgsql;

using System.Diagnostics;   // has Debug.WriteLine()

namespace KarateGeek.databaseConnection
{
    public class CoreDatabaseConnection
    {
        private NpgsqlConnection _conn = null;

        //public static string _SERVER    = "192.168.1.115";
        public static string _SERVER    = "127.0.0.1";
        public static string _PORT      = "5432";
        public static string _USER      = "postgres";
        public static string _PASSWORD  = "admin";
        public static string _DATABASE  = "karategeek";
        
        
        public NpgsqlConnection conn
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new NpgsqlConnection("Server=" + _SERVER + "; Port=" + _PORT + "; User Id=" + _USER + "; Password=" + _PASSWORD + "; Database=" + _DATABASE + ";");

                    /* Use the following code to test with a remote DB (to check for latency issues): */
                    //_conn = new NpgsqlConnection("Server=nicholas.ddns.net; Port=52728; User Id=karate; Password=xI85nO; Database=karate_db;");

                    //{ /* temporary (and UGLY) hack to test remote DB with different schema name... */
                    //    NpgsqlCommand comm = _conn.CreateCommand();
                    //    comm.CommandText = "SET search_path TO schema_v1;";
                    //    //comm.CommandText = "SET DateStyle TO Euro;";
                    //    _conn.Open();
                    //    comm.ExecuteNonQuery();
                    //}
                }
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

        public Boolean NonQuery(string sql)
        {
            Debug.WriteLine("Executing SQL NonQuery: " + sql);  // Much faster than Console.WriteLine()
                                                                // and auto-disabled in release mode.


            NpgsqlCommand comm = this.conn.CreateCommand();
            comm.CommandText = sql;

            comm.ExecuteNonQuery();

            this.Disconnect();
            return true;
        }


        public DataSet Query(string sql)
        {
            Debug.WriteLine("Executing SQL Query:    " + sql);  // Much faster than Console.WriteLine()
                                                                // and auto-disabled in release mode.

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, conn);

            DataSet results = new DataSet();

            adapter.Fill(results, "table");

            this.Disconnect();
            return results;
        }


        public NpgsqlDataAdapter AdapterForQuery(string sql)
        {
            Debug.WriteLine("Executing SQL Query:    " + sql);  // Much faster than Console.WriteLine()
            // and auto-disabled in release mode.

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, conn);

            this.Disconnect();
            return adapter;
        }

    }
}
