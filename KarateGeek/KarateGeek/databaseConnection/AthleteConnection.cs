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

        public string InsertNewAthlete(string firstName, string lastName, string fathersName, string sex,
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email,
           string addressStreetName, string addressStreetNumber, string addressPostalCode, string countryCode, string City, 
           string rank, string localClubId)
        {
            AddressConnection addConn = new AddressConnection();
            string addressId = addConn.InsertNewAddress(addressStreetName, addressStreetNumber, City,  addressPostalCode, countryCode);

            string personId = this.insertNewPerson(firstName, lastName, fathersName, sex, dateOfBirth,
            primaryPhoneNo, secondaryPhoneNo, email, "" + addressId);

            this._InsertAthlete(personId, rank, localClubId);

            return personId;
        }



        //public string InserrtNewAthleteFromOther(string id, string firstName, string middleName, string lastName,
        //  DateTime dateOfBirth,
        //  string primaryPhoneNo, string secondaryPhoneNo, string email,
        //  string countryCode, string City, string addressStreetName, string addressStreetNumber, string addressPostalCode,
        //  string rank, string localClubId)
        //{
        //    string sql = "";
        //    DataSet dr = null;

        //    //this.updatePerson(id, firstName, middleName, lastName,
        //    //dateOfBirth,
        //    //primaryPhoneNo, secondaryPhoneNo, email);

        //    this._InsertAthlete(id, rank, localClubId);

        //    // getting the athlete_address id
        //    sql = "select address_id from persons where id = '" + id + "'; ";
        //    dr = this.Query(sql);
        //    int addressId = int.Parse(dr.Tables[0].Rows[0][0].ToString()); 

        //    AddressConnection addConn = new AddressConnection();
        //    addConn.UpdateAddress(addressId, countryCode, City, addressStreetName, addressStreetNumber, addressPostalCode);

        //    return "";
        //}



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

        public DataSet deleteAthlete(int id){
            string sql = "delete from athletes where id='" + id + "';";
            return this.Query(sql);
        }

        public DataSet findSimilar(string filter)
        {
            //select * from persons JOIN athletes on persons.id = athletes.id where persons.athlete_first_name like 'd%';

            string sql = "select * from persons JOIN athletes on persons.id = athletes.id where first_name like '" + filter + "%';";
            return this.Query(sql);
        }


        public DataSet findPotentialParticipants(string sex , int ageFrom , int ageTo , string levelFrom , string levelTo)
        {
            string filter = "where";
            int num = 0;
            if (!sex.Equals(null) )
            {
                if (num == 0)
                {
                    filter = filter + " sex = '" + sex + "';";
                }
                else
                {
                    filter = filter + " and sex = '" + sex + "';";
                }
            }
            
                string sql = "select persons.id as persons_id,first_name,last_name,sex , date_of_birth, persons.phone as persons_phone,secondary_phone,persons.email as persons_email,rank,clubs.id as club_id,name, clubs.id as clubs_id,clubs.phone as clubs_phone, clubs.email as clubs_email from persons inner join athletes on (persons.id = athletes.id) inner join clubs on (club_id = club_id)" + filter;
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
