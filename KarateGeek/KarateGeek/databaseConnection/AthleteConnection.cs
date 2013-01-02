using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class AthleteConnection : PersonConnection
    {
        DataTable athletes = new DataTable();


        public bool InsertNewAthlete(int personId, string firstName, string lastName, string fathersName, string sex,
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email,
           string addressStreetName, string addressStreetNumber, string addressPostalCode, string countryCode, string City,
           string rank, string localClubId)
        {
            string addressId = null;
            string athleteId = null;
            bool athleteInsertion = false;

            if (personId == -1)
            {
                AddressConnection addConn = new AddressConnection();
                addressId = addConn.InsertNewAddress(addressStreetName, addressStreetNumber, City, addressPostalCode, countryCode);
                athleteId = insertNewPerson(firstName, lastName, fathersName, sex, dateOfBirth, primaryPhoneNo, secondaryPhoneNo, email, addressId);
                _InsertAthlete(athleteId, rank, localClubId);

                athleteInsertion = true;
            }
            else
            {
                _InsertAthlete(personId.ToString(), rank, localClubId);
                athleteInsertion = true;
            }

            return athleteInsertion;
        }

        public string UpdateAthlete(int id, string firstName, string lastName, string fathersName, string sex,
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email,
           string addressStreetName, string addressStreetNumber, string addressPostalCode, string countryCode, string City,
           string rank, string localClubId)
        {
            string sql = "";
            DataSet dr = null;

            this._UpdatetAthlete(id, rank, localClubId);

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

        public bool deleteAthlete(int id)
        {
            JudgeConnection judgeConnection = new JudgeConnection();
            TournamentConnection tournamentConnection = new TournamentConnection();
            DataSet ds;
            DataSet ds2;
            string sql = null;


            ds = judgeConnection.findJudge(id);

            sql = "select * from tournament_participations where athlete_id='" + id + "';";
            ds2 = this.Query(sql);
            if (ds2.Tables[0].Rows.Count == 0)
            {
                sql = "delete from athletes where id='" + id + "';";
                this.NonQuery(sql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    sql = "delete from persons where id='" + id + "';";
                    this.NonQuery(sql);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        public DataSet findAthlete(int id)
        {
            string sql = "select * from athletes where id = '" + id + "';";
            return this.Query(sql);
        }

        public DataSet findAthleteClub(int id)
        {
            string sql = "select * from athletes inner join clubs on club_id = clubs.id where athletes.id = '" + id + "';";
            return this.Query(sql);
        }

        private string _InsertAthlete(string PersonId, string rank, string localClubId)
        {

            string sql = "insert into athletes ( id, rank, club_id) values ( '"
                + PersonId + "', '"
                + rank + "', '"
                + localClubId + "' );"; //edw egine allagi gia na fanei oti xreiazetai to id apo to athlete_club pou tha einai eidi perasmeno

            this.NonQuery(sql);


            return "";
        }

        private string _UpdatetAthlete(int PersonId, string rank, string localClubId)
        {
            string sql = "update athletes set " +

                "rank = '" + rank + "', " +
                "club_id = '" + localClubId + "' where id = '" + PersonId + "' ;";

            this.NonQuery(sql);

            return "";
        }

        public DataTable GetAthletes()
        {
            string sql = "select * from athletes join persons on persons.id = athletes.id;";

            DataSet temp = Query(sql);
            return temp.Tables[0];

        }

        public int getNumOfScheduledParticipation(long athleteId) // returns the number of scheduled participations
        {                                                         // where the tournament hasn't taken place yet.
            String sql = "SELECT ranking FROM tournament_participations WHERE athlete_id = " + athleteId
                + " AND ranking IS NULL;";

            return this.Query(sql).Tables[0].Rows.Count;
        }

    }
}
