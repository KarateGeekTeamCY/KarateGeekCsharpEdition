using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using KarateGeek.helpers;

namespace KarateGeek.databaseConnection
{
    class UserConnection : CoreDatabaseConnection
    {
        private string username = "admin";
        private string password = "admin";
        private string sql = null;
        private Cryptography cr = new Cryptography();

        public void insertNewUser()
        {
            password = cr.cryptography(password);

            sql = "insert into users(id , username ,password, person_management, event_management , lottery , game_support , reports , settings)"
                    + "values('0', '" + username + "' , '" + password + "' , '1' , '1' , '1' , '1' , '1' , '1');";

            this.NonQuery(sql);

        }


        public void insertNewUser(string personid, string username, string password, 
            bool personManagement, bool eventManagement, bool lottery, bool gameSupport, bool reports, bool settings)
        {
            password = cr.cryptography(password);

            sql = "insert into users(id , username ,password, person_management, event_management , lottery , game_support , reports , settings)"
                    + "values('0', '" + username + "' , '" + password + "' , '" + 
                    personManagement + "' , '" +
                    eventManagement + "' , '" +
                    lottery + "' , '" +
                    gameSupport + "' , '" +
                    reports + "' , '" +
                    settings + "');";

            this.NonQuery(sql);

        }


    }
}
