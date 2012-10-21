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


    }
}
