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

        public string UpdateEvent(int eventId, string name, DateTime eventdate, string addressStreetName,
           string addressStreetNumber, string addressPostalCode, string location, string phone,
           string email, string city, string countryCode, Boolean official)
        {
            string sql = "";
            DataSet drA = null;
            DataSet drL = null;

            LocationConnection locationConnection = new LocationConnection();
            sql = "select location_id from events where id = '" + eventId + "'; ";

            drL = this.Query(sql);
            int locationId = int.Parse(drL.Tables[0].Rows[0][0].ToString());
            // getting the location_address id
            sql = "select address_id from locations where id = '" + locationId + "'; ";

            drA = this.Query(sql);
            int addressId = int.Parse(drA.Tables[0].Rows[0][0].ToString());

            this._UpdateEvent(eventId, name, eventdate, official, locationId);

            locationConnection.UpdateLocation(locationId, location, phone, email, addressId);
            AddressConnection addConn = new AddressConnection();
            addConn.UpdateAddress(addressId, addressStreetName, addressStreetNumber, city, addressPostalCode, countryCode);

            return "";
        }

        public DataSet findSimilar(string filter)
        {
            string sql = "select * from events where name like '" + filter + "%';";
            return this.Query(sql);
        }

        private string _InsertEvent(string name, DateTime eventdate, Boolean official, string locationId)
        {

            string sql = "insert into events ( name, date, official, location_id) values ( '"
                + name + "', '"
                + eventdate.ToShortDateString() + "', '"
                + official + "', '"
                + locationId + "' );"; 

            this.NonQuery(sql);

            sql = "select currval('events_id_seq');";
            dr = this.Query(sql);
            long eventId = long.Parse(dr.Tables[0].Rows[0][0].ToString());

            return "" + eventId;
        }

        private string _UpdateEvent(int eventId, string name, DateTime eventdate, Boolean official, int locationId)
        {

            string sql = "update events set " +
                 "name = '" + name + "', " +
                 "date = '" + eventdate.ToShortDateString() + "', " +
                 "official = '" + official + "', " +
                 "location_id = '" + locationId + "' where id = '" + eventId + "' ;";

            this.NonQuery(sql);

            return "";
        }

        public DataSet getEvents()
        {
            string sql = "select * from events;";
            return this.Query(sql);
        }

        public DataSet getEventsByName(string name)
        {
            string sql = "select * from events where name = '" + name + "';";
            return this.Query(sql);
        }
    }
}
