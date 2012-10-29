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

        public string InsertNewJudge(string firstName, string lastName, string fathersName, string sex,
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email,
           string addressStreetName, string addressStreetNumber, string addressPostalCode, string countryCode, string City, 
           string rank, string judge_class)
        {
            AddressConnection addConn = new AddressConnection();
            string addressId = addConn.InsertNewAddress(addressStreetName, addressStreetNumber, City,  addressPostalCode, countryCode);

            string personId = this.insertNewPerson(firstName, lastName, fathersName, sex, dateOfBirth,
            primaryPhoneNo, secondaryPhoneNo, email, "" + addressId);
            int id = int.Parse(personId);
            this._InsertJudge(id, rank , judge_class);
            
            return personId;
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


         public DataSet findSimilar(string filter)
         {
             string sql = "select * from persons JOIN judges on persons.id = judges.id where first_name like '" + filter + "%';";
             return this.Query(sql);
         }

         public DataSet deleteJudge(int id)
         {
             string sql = "delete from judges where id='" + id + "';";
             return this.Query(sql);
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
