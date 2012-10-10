using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class LoginConnection : CoreDatabaseConnection
    {
        private string sql = null;
        private NpgsqlDataReader dr;
        private Cryptography cr = new Cryptography();

        public LoginConnection()
        {
            DatabaseInitializer db = new DatabaseInitializer();
        }
        public Boolean compare(string username, string password)
        {

            password = cr.cryptography(password);
            //tha allaxthei auto me to kanoniko otan dimiourgithei i vasi
            sql = "select * from users where username ='" + username + "' and password ='" + password + "';";
            dr = this.Query(sql);

            if (dr.HasRows == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
