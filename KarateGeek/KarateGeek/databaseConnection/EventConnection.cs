using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek.databaseConnection
{
    class EventConnection : CoreDatabaseConnection
    {
        DataTable eventDT = new DataTable();
        DataSet dr = null;

        public string InsertNewEvent(string name, DateTime eventdate, string addressStreetName,
           string addressStreetNumber, string addressPostalCode, string location, string phone,
           string email, string city , string countryCode, Boolean official)
        {
            AddressConnection addConn = new AddressConnection();
            string addressId = addConn.InsertNewAddress(addressStreetName, addressStreetNumber, city, addressPostalCode, countryCode);

            LocationConnection locationConnection = new LocationConnection();
            string locationId = locationConnection.InsertNewLocation(location, phone, email, "" + addressId);

            string eventId = this._InsertEvent(name, eventdate, official , locationId);

            return eventId;
        }

        private string _InsertEvent(string name, DateTime eventdate, Boolean official, string locationId)
        {

            string sql = "insert into events ( name, date, official, location_id) values ( '"
                + name + "', '"
                + eventdate.ToShortDateString() + "', '"
                + official + "', '"
                + locationId + "' );"; //edw egine allagi gia na fanei oti xreiazetai to id apo to athlete_club pou tha einai eidi perasmeno

            this.NonQuery(sql);

            sql = "select currval('events_id_seq');";
            dr = this.Query(sql);
            long addressId = long.Parse(dr.Tables[0].Rows[0][0].ToString());

            return "" + addressId;
        }
    }
}
