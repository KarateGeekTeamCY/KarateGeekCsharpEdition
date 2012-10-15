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
        private Cryptography cr = new Cryptography();

        //na valete ton constructor se sxolio meta tin prwti fora ektelesis
<<<<<<< HEAD
        public LoginConnection()
        {
            //
            // uncoment both next lines if rooning for the first time

            //UserConnection us = new UserConnection();
            //us.insertNewUser();
            
        }
=======
        //public LoginConnection()
        //{

        //    UserConnection us = new UserConnection();
        //    us.insertNewUser();

        //}
>>>>>>> 47beb633c1b82e682f42e4a7fe7fac071920edce
        
        public Boolean compare(string username , string password) {

            password = cr.cryptography(password);
            //auto tha allaksei me to kainourgio erwtima otan ginei i vasi
            sql ="select * from users where username ='" + username + "' and password ='" + password + "';";

            dr = this.Query(sql);

            if (dr.HasRows == true)
            {
                dr.Close();
                return true;
            }
            else
            {
                dr.Close();
                return false;
            }
        }

    }
}
