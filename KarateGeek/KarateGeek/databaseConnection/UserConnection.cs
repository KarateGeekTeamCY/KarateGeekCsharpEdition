using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace KarateGeek.databaseConnection
{
    class UserConnection : CoreDatabaseConnection
    {
        private string username = "root";
        private string password = "root";
        private string sql = null;

        public void insertNewUser()
        {
            password = cr.cryptography(password);
            sql = "insert into users ( username, password) values ( '"
                + username + "', '"
                + password + "' );";

            this.NonQuery(sql);

        }

        
    }
}
