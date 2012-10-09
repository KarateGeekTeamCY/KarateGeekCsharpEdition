using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class LoginConnection : CoreDatabaseConnection
    {
        private string sql=null;
        private NpgsqlDataReader dr;

        public Boolean compare(string username , string password) {
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
