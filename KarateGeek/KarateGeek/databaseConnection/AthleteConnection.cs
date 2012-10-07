﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class AthleteConnection : CoreDatabaseConnection
    {
        DataTable athlets = new DataTable();

        public Boolean InserrtNewAthlete(string firstName, string middleName, string lastName,
           DateTime dateOfBirth,
           string primaryPhoneNo, string secondaryPhoneNo, string email,
           string countryCode, string City, string addressStreetName, string addressStreetNumber, string addressPostalCode,
           string rank, string localClubId)
        {
            string sql = "";

            // getting the next person id
            sql = "select nextval('seq_persons_id'); ";
            NpgsqlDataReader dr = this.Query(sql);
            dr.Read();
            int personId = dr.GetInt32(0);

            // getting the next address id
            sql = "select nextval('seq_address_id'); ";
            dr = this.Query(sql);
            dr.Read();
            int addressId = dr.GetInt32(0);



            sql = "insert into table Persons ( id, first_name, middle_name, last_name, "+
                "date_of_birth, primary_phone_number, secondary_phone_number, email, "+
                "address_id) values ( '" 
                + personId + "', '"
                + firstName + "', '"
                + middleName + "', '"
                + lastName + "', '"
                + dateOfBirth.ToShortDateString() + "', '"
                + primaryPhoneNo + "', '"
                + secondaryPhoneNo + "', '"
                + email + "', '"
                + addressId + "' );";

            this.NonQuery(sql);



            sql = "insert into table athlets ( person_id, rank, local_association_id) values ( '"
                + personId + "', '"
                + rank + "', '"
                + localClubId + "' );";

            this.NonQuery(sql);



            sql = "insert into table addresses ( id, city, street_name, street_number, " +
                "street_postal_code, country_code ) values ( '"
                + addressId + "', '"
                + City + "', '"
                + addressStreetName + "', '"
                + addressStreetNumber + "', '"
                + addressPostalCode + "', '"
                + countryCode + "' );";

            this.NonQuery(sql);


            return true;
        }





    }
}