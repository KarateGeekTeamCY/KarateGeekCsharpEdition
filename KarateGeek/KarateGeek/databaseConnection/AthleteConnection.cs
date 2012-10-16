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

        public string InserrtNewAthlete(string firstName, string lastName, string fathersName,
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email,
           string countryCode, string City, string addressStreetName, string addressStreetNumber, string addressPostalCode,
           string rank, string localClubId)
        {
            AddressConnection addConn = new AddressConnection();
            string addressId = addConn.InsertNewAddress(addressStreetName, addressStreetNumber, City,  addressPostalCode, countryCode);

            string personId = this.insertNewPerson(firstName, lastName, fathersName, dateOfBirth,
            primaryPhoneNo, secondaryPhoneNo, email, "" + addressId);

            this._InsertAthlete(personId, rank, localClubId);

            return personId;
        }



        public string InserrtNewAthleteFromOther(string id, string firstName, string middleName, string lastName,
          DateTime dateOfBirth,
          string primaryPhoneNo, string secondaryPhoneNo, string email,
          string countryCode, string City, string addressStreetName, string addressStreetNumber, string addressPostalCode,
          string rank, string localClubId)
        {
            string sql = "";
            DataSet dr = null;

            this.updatePerson(id, firstName, middleName, lastName,
            dateOfBirth,
            primaryPhoneNo, secondaryPhoneNo, email);

            this._InsertAthlete(id, rank, localClubId);

            // getting the address id
            sql = "select address_id from persons where id = '" + id + "'; ";
            dr = this.Query(sql);
            long addressId = long.Parse(dr.Tables[0].Rows[0][0].ToString()); 

            AddressConnection addConn = new AddressConnection();
            addConn.UpdateAddress("" + addressId, countryCode, City, addressStreetName, addressStreetNumber, addressPostalCode);

            return "";
        }



        public string UpdateAthlete(string id, string firstName, string middleName, string lastName,
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email,
           string countryCode, string City, string addressStreetName, string addressStreetNumber, string addressPostalCode,
           string rank, string localClubId)
        {
            string sql = "";
            DataSet dr = null;

            this._UpdatetAthlete(id, rank, localClubId);

            this.updatePerson(id, firstName, middleName, lastName,
            dateOfBirth,
            primaryPhoneNo, secondaryPhoneNo, email);

           
            // getting the address id
            sql = "select address_id from persons where id = '" + id + "'; ";
            
            dr = this.Query(sql);
            long addressId = long.Parse(dr.Tables[0].Rows[0][0].ToString()); 

            AddressConnection addConn = new AddressConnection();
            addConn.UpdateAddress("" + addressId, countryCode, City, addressStreetName, addressStreetNumber, addressPostalCode);

            return "";
        }



        public LinkedList<string> findSimilar(string filter)
        {
            LinkedList<string> items = new LinkedList<string>();

            return items;
        }


        private string _InsertAthlete(string PersonId, string rank, string localClubId)
        {

            string sql = "insert into athletes ( id, rank, club_id) values ( '"
                + PersonId + "', '"
                + rank + "', '"
                + 1 + "' );"; //edw egine allagi gia na fanei oti xreiazetai to id apo to club pou tha einai eidi perasmeno

            this.NonQuery(sql);


            return "";
        }



        private string _UpdatetAthlete(string PersonId, string rank, string localClubId)
        {
            string sql = "update athletes set " +

                "rank = '" + rank + "', " +
                "club_id = '" + localClubId + "' where id = '" + PersonId + "' ;";

            this.NonQuery(sql);

            return "";
        }
    }
}
