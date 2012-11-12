﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class Athlete : CoreDatabaseConnection
    {

        private string sql;

        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string midleName { get; set; }
        public string sex { get; set; }
        public string age { get; set; }
        public string rank { get; set; }
        public string firstPhone { get; set; }
        public string secondPhone { get; set; }
        public string email { get; set; }
        public string addressId { get; set; }
        public string addressStreet { get; set; }
        public string addressNumber { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string clubId { get; set; }
        public string clubName { get; set; }


        DataTable athletes = new DataTable();

        public Athlete(string athleteId)
        {
            this.id = athleteId;
            this.load();
        }



        public void load()
        {
            sql = "select * from athlete_view where id = '" + id + "'; ";
            DataTable temp = this.Query(sql).Tables[0];

            this.firstName = (string)temp.Rows[0][1];
            this.lastName = (string)temp.Rows[0][2];
            this.midleName = (string)temp.Rows[0][3];
            this.sex = (string)temp.Rows[0][4];
            this.age = (string)temp.Rows[0][5];
            this.firstPhone = (string)temp.Rows[0][6];
            this.secondPhone = (string)temp.Rows[0][7];
            this.email = (string)temp.Rows[0][8];
            this.addressId = (string)temp.Rows[0][9];
            this.addressStreet = (string)temp.Rows[0][10];
            this.city = (string)temp.Rows[0][11];
            this.country = (string)temp.Rows[0][12];
            this.addressNumber = (string)temp.Rows[0][13];
            this.rank = (string)temp.Rows[0][14];
        }




















        //public string InsertNewAthlete(string firstName, string lastName, string fathersName, string sex,
        //   DateTime dateOfBirth,
        //   string primaryPhoneNo, string secondaryPhoneNo, string email,
        //   string addressStreetName, string addressStreetNumber, string addressPostalCode, string countryCode, string City,
        //   string rank, string localClubId)
        //{
        //    AddressConnection addConn = new AddressConnection();
        //    string addressId = addConn.InsertNewAddress(addressStreetName, addressStreetNumber, City, addressPostalCode, countryCode);

        //    string personId = this.insertNewPerson(firstName, lastName, fathersName, sex, dateOfBirth,
        //    primaryPhoneNo, secondaryPhoneNo, email, "" + addressId);

        //    this._InsertAthlete(personId, rank, localClubId);

        //    return personId;
        //}



        ////public string InserrtNewAthleteFromOther(string id, string firstName, string middleName, string lastName,
        ////  DateTime dateOfBirth,
        ////  string primaryPhoneNo, string secondaryPhoneNo, string email,
        ////  string countryCode, string City, string addressStreetName, string addressStreetNumber, string addressPostalCode,
        ////  string rank, string localClubId)
        ////{
        ////    string sql = "";
        ////    DataSet dr = null;

        ////    //this.updatePerson(id, firstName, middleName, lastName,
        ////    //dateOfBirth,
        ////    //primaryPhoneNo, secondaryPhoneNo, email);

        ////    this._InsertAthlete(id, rank, localClubId);

        ////    // getting the athlete_address id
        ////    sql = "select address_id from persons where id = '" + id + "'; ";
        ////    dr = this.Query(sql);
        ////    int addressId = int.Parse(dr.Tables[0].Rows[0][0].ToString()); 

        ////    AddressConnection addConn = new AddressConnection();
        ////    addConn.UpdateAddress(addressId, countryCode, City, addressStreetName, addressStreetNumber, addressPostalCode);

        ////    return "";
        ////}



        //public string UpdateAthlete(int id, string firstName, string lastName, string fathersName, string sex,
        //   DateTime dateOfBirth,
        //   string primaryPhoneNo, string secondaryPhoneNo, string email,
        //   string addressStreetName, string addressStreetNumber, string addressPostalCode, string countryCode, string City,
        //   string rank, string localClubId)
        //{
        //    string sql = "";
        //    DataSet dr = null;

        //    this._UpdatetAthlete(id, rank, localClubId);

        //    // getting the athlete_address id
        //    sql = "select address_id from persons where id = '" + id + "'; ";

        //    dr = this.Query(sql);
        //    int addressId = int.Parse(dr.Tables[0].Rows[0][0].ToString());

        //    this.updatePerson(id, firstName, lastName, fathersName, sex, dateOfBirth,
        //    primaryPhoneNo, secondaryPhoneNo, email, addressId);

        //    AddressConnection addConn = new AddressConnection();
        //    addConn.UpdateAddress(addressId, addressStreetName, addressStreetNumber, City, addressPostalCode, countryCode);

        //    return "";
        //}

        //public void deleteAthlete(int id)
        //{
        //    JudgeConnection judgeConnection = new JudgeConnection();
        //    DataSet ds;
        //    string sql = null;

        //    ds = judgeConnection.findJudge(id);
        //    sql = "delete from athletes where id='" + id + "';";
        //    this.Query(sql);
        //    if (ds.Tables[0].Rows.Count == 0)
        //    {
        //        sql = "delete from persons where id='" + id + "';";
        //        this.Query(sql);
        //    }

        //}

        //public DataSet findAthlete(int id)
        //{
        //    string sql = "select * from athletes where id = '" + id + "';";
        //    return this.Query(sql);
        //}

        //public DataSet findAthleteClub(int id)
        //{
        //    string sql = "select * from athletes inner join clubs on club_id = clubs.id where athletes.id = '" + id + "';";
        //    return this.Query(sql);
        //}

        //private string _InsertAthlete(string PersonId, string rank, string localClubId)
        //{

        //    string sql = "insert into athletes ( id, rank, club_id) values ( '"
        //        + PersonId + "', '"
        //        + rank + "', '"
        //        + localClubId + "' );"; //edw egine allagi gia na fanei oti xreiazetai to id apo to athlete_club 
        //    //pou tha einai eidi perasmeno

        //    this.NonQuery(sql);


        //    return "";
        //}



        //private string _UpdatetAthlete(int PersonId, string rank, string localClubId)
        //{
        //    string sql = "update athletes set " +

        //        "rank = '" + rank + "', " +
        //        "club_id = '" + localClubId + "' where id = '" + PersonId + "' ;";

        //    this.NonQuery(sql);

        //    return "";
        //}



        //public DataTable GetAthlete(string id)
        //{
        //    string sql = "select * from athlete_view where id = '" + id + "';";

        //    return Query(sql).Tables[0];
        //    //return temp;

        //}



        //public int getNumOfScheduledParticipation(long athleteId) // returns the number of scheduled participations
        //{                                                         // where the tournament hasn't taken place yet.
        //    String sql = "SELECT ranking FROM tournament_participations WHERE athlete_id = " + athleteId
        //        + " AND ranking IS NULL;";

        //    return this.Query(sql).Tables[0].Rows.Count;
        //}

    }
}
