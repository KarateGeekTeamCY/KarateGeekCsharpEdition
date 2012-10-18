using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class LoginConnection : CoreDatabaseConnection
    {
        private string sql = null;
        private DataSet dr;
        private Cryptography cr = new Cryptography();

        //na valete ton constructor se sxolio meta tin prwti fora ektelesis

        //public LoginConnection()
        //{

        //    UserConnection us = new UserConnection();
        //    us.insertNewUser();

        //}

      
        public Boolean compare(string username , string password) {

            password = cr.cryptography(password);
            //auto tha allaksei me to kainourgio erwtima otan ginei i vasi
            sql ="select * from users where username ='" + username + "' and password ='" + password + "';";

            dr = this.Query(sql);


            if (dr.Tables[0].Rows.Count != 0)
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
