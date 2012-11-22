using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class JudgeConnection : PersonConnection
    {
        DataTable judges = new DataTable();

        public string InsertNewJudge(int personId, string firstName, string lastName, string fathersName, string sex,
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email,
           string addressStreetName, string addressStreetNumber, string addressPostalCode, string countryCode, string City,
           string rank, string judge_class)
        {
            DataTable dt;
            string addressId = null;
            string judgeId = null;

            if (personId == -1)
            {
                AddressConnection addConn = new AddressConnection();
                addressId = addConn.InsertNewAddress(addressStreetName, addressStreetNumber, City, addressPostalCode, countryCode);
                judgeId = insertNewPerson(firstName, lastName, fathersName, sex, dateOfBirth, primaryPhoneNo, secondaryPhoneNo, email, addressId);
                this._InsertJudge(personId, rank, judge_class);

            }
            else if (personId >= 0)
            {
                string sql = "select id from persons natural join athletes where id = " + personId + ";";
                dt = this.Query(sql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this._InsertJudge(personId, rank, judge_class);
                }
                else
                {

                }
            }
           

            return personId.ToString();
        }


        private string _InsertJudge(int id, string rank, string judge_class)
        {

            string sql = "insert into judges ( id, rank, class) values ( '"
                + id + "', '"
                + rank + "', '"
                + judge_class + "' );"; //edw egine allagi gia na fanei oti xreiazetai to id apo to athlete_club pou tha einai eidi perasmeno

            this.NonQuery(sql);


            return "";
        }

        public string UpdateJudge(int id, string firstName, string lastName, string fathersName, string sex,
          DateTime dateOfBirth,
          string primaryPhoneNo, string secondaryPhoneNo, string email,
          string addressStreetName, string addressStreetNumber, string addressPostalCode, string countryCode, string City,
          string rank, string judge_class)
        {
            string sql = "";
            DataSet dr = null;

            this._UpdateJudge(id, rank, judge_class);

            // getting the athlete_address id
            sql = "select address_id from persons where id = '" + id + "'; ";

            dr = this.Query(sql);
            int addressId = int.Parse(dr.Tables[0].Rows[0][0].ToString());

            this.updatePerson(id, firstName, lastName, fathersName, sex, dateOfBirth,
            primaryPhoneNo, secondaryPhoneNo, email, addressId);

            AddressConnection addConn = new AddressConnection();
            addConn.UpdateAddress(addressId, addressStreetName, addressStreetNumber, City, addressPostalCode, countryCode);

            return "";
        }

        public DataSet findJudge(int id)
        {
            string sql = "select * from judges where id = '" + id + "';";
            return this.Query(sql);
        }

        public bool deleteJudge(int id)
        {
            AthleteConnection athleteConnection = new AthleteConnection();
            DataSet ds;
            DataSet ds2;
            string sql = null;

            ds = athleteConnection.findAthlete(id);
            sql = "select * from tournament_participations where athlete_id='" + id + "';";
            ds2 = this.Query(sql);
            if (ds2.Tables[0].Rows.Count == 0)
            {
                sql = "delete from judges where id='" + id + "';";
                this.NonQuery(sql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    sql = "delete from persons where id='" + id + "';";
                    this.NonQuery(sql);
                }
                return true;
            }
            return false;
        }


        public DataSet GetJudges()
        {
            string sql = "select * from persons join judges on persons.id = judges.id;";
            return this.Query(sql);
        }

        private string _UpdateJudge(int JudgeId, string rank, string judge_class)
        {
            string sql = "update judges set " +

                "rank = '" + rank + "', " +
                "class = '" + judge_class + "' where id = '" + JudgeId + "' ;";

            this.NonQuery(sql);

            return "";
        }

    }
}
