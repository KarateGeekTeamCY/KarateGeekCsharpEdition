﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class LoginConnection : CoreDatabaseConnection
    {
        private string sql=null;
        private NpgsqlDataReader dr;
        private UserConnection uConn = new UserConnection();
        private Cryptography cr = new Cryptography();
        
        public Boolean compare(string username , string password) {

            password = cr.cryptography(password);
            sql ="select * from users where username ='" + username + "' and password ='" + password + "';";
            dr = this.Query(sql);

            if(dr.HasRows == true){
                return true;
            }else{
                return false;
            }
        }

    }
}
