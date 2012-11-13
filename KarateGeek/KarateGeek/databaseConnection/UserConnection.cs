using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using KarateGeek.helpers;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class UserConnection : CoreDatabaseConnection
    {
        private string defUsername = "admin";
        private string defPassword = "admin";
        private string sql = null;
        private Cryptography cr = new Cryptography();

        public void insertNewUser()
        {
            defPassword = cr.cryptography(defPassword);

            sql = "insert into users(username ,password, person_management, event_management , lottery , game_support , reports , settings)"
                    + "values('" + defUsername + "' , '" + defPassword + "' , '1' , '1' , '1' , '1' , '1' , '1');";

            this.NonQuery(sql);

        }


        public void insertNewUser(string username, string password,
            bool personManagement, bool eventManagement, bool lottery, bool gameSupport, bool reports, bool settings)
        {
            password = cr.cryptography(password);

            sql = "insert into users(username ,password, person_management, event_management , lottery , game_support , reports , settings) values ('" +
                    username + "' , '" +
                    password + "' , '" +
                    personManagement + "' , '" +
                    eventManagement + "' , '" +
                    lottery + "' , '" +
                    gameSupport + "' , '" +
                    reports + "' , '" +
                    settings + "');";

            this.NonQuery(sql);

        }

        public DataTable findSimilar(string like)
        {
            return this.Query("select * from users where lower(username) like lower('" + like + "%');").Tables[0];
        }


    }
}
