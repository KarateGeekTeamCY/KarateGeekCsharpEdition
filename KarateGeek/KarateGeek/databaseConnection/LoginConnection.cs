using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class LoginConnection : CoreDatabaseConnection
    {
        private string sql = null;
        private NpgsqlDataReader dr;
<<<<<<< HEAD
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
=======
        private UserConnection uConn = new UserConnection();
        private Cryptography cr = new Cryptography();
        
        public Boolean compare(string username , string password) {

            password = cr.cryptography(password);
            sql ="select * from users where username ='" + username + "' and password ='" + password + "';";
>>>>>>> 391c17720bc6a45ca67d25c9dffd423fd671f54d
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
