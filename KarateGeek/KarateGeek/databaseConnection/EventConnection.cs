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

        public string InsertNewEvent(string name, string eventdate, string addressStreetName,
           string addressStreetNumber, string addressPostalCode, string location, string phone,
           string email, string city , string countryCode, Boolean official)
        {
            AddressConnection addConn = new AddressConnection();
            string addressId = addConn.InsertNewAddress(addressStreetName, addressStreetNumber, city, addressPostalCode, countryCode);

            LocationConnection locationConnection = new LocationConnection();
            string locationId = locationConnection.InsertNewLocation(location, phone, email, "" + addressId);

            string eventId = this._InsertEvent(name, eventdate, official , addressId);

            return eventId;
        }

        public string UpdateEvent(int eventId, string name, string eventdate, string addressStreetName,
           string addressStreetNumber, string addressPostalCode, string location, string phone,
           string email, string city, string countryCode, Boolean official)
        {
            string sql = "";
            int addressId;
            DataSet drL = null;

            LocationConnection locationConnection = new LocationConnection();
            sql = "select location_id from events where id = '" + eventId + "'; ";

            drL = this.Query(sql);
            int locationId = int.Parse(drL.Tables[0].Rows[0][0].ToString());
            addressId = locationId;
            this._UpdateEvent(eventId, name, eventdate, official, locationId);

            locationConnection.UpdateLocation(locationId, location, phone, email);
            AddressConnection addConn = new AddressConnection();
            addConn.UpdateAddress(addressId, addressStreetName, addressStreetNumber, city, addressPostalCode, countryCode);

            return "";
        }

        public void deleteEvent(int id)
        {
            DataSet dsE = null;
            int location_id;
            int address_id;

            string sql = "select location_id from events where  id= '" + id + "';";
            dsE =  this.Query(sql);
            location_id = int.Parse(dsE.Tables[0].Rows[0][0].ToString());
            address_id = location_id;
          
            //delete events and tournaments on cascade
            sql = "delete from events where id='" + id + "';";
            this.Query(sql);

            new AddressConnection().deleteEventAddress(address_id, id);
        }

        public DataSet findSimilar(string filter)
        {
            string sql = "select * from events where lower(name) like lower('" + filter + "%') and is_finished = 'false' order by name;";
            return this.Query(sql);
        }

        private string _InsertEvent(string name, string eventdate, Boolean official, string locationId)
        {
            string sql;
            if (string.IsNullOrEmpty(eventdate))
            {
                sql = "insert into events ( name, official, location_id) values ( '"
                    + name + "', '"
                    + official + "', '"
                    + locationId + "' );";

            }
            else
            {
                sql = "insert into events ( name, date, official, location_id) values ( '"
                    + name + "', '"
                    + eventdate + "', '"
                    + official + "', '"
                    + locationId + "' );";
            }
            this.NonQuery(sql);

            sql = "select currval('events_id_seq');";
            dr = this.Query(sql);
            long eventId = long.Parse(dr.Tables[0].Rows[0][0].ToString());

            return "" + eventId;
        }

        private string _UpdateEvent(int eventId, string name, string eventdate, Boolean official, int locationId)
        {
            string sql;
            if (string.IsNullOrEmpty(eventdate))
            {
                sql = "update events set " +
                 "name = '" + name + "', " +
                 "official = '" + official + "', " +
                 "location_id = '" + locationId + "' where id = '" + eventId + "' ;";
            }
            else
            {
                sql = "update events set " +
                 "name = '" + name + "', " +
                 "date = '" + eventdate + "', " +
                 "official = '" + official + "', " +
                 "location_id = '" + locationId + "' where id = '" + eventId + "' ;";

            }
            
            this.NonQuery(sql);

            return "";
        }

        public DataSet getEvents()
        {
            string sql = "select * from events where is_finished = 'false' order by name;";
            return this.Query(sql);
        }

        public DataSet getEventsWithDate()
        {
            string sql = "select * from events where date is not null;";
            return this.Query(sql);
        }

        public DataTable getUnlotterisedEvents()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("select id, name from events e1 ");
            sb.Append("where date >= '");
            sb.Append(DateTime.Today.ToString("yyyy-M-d"));
            sb.Append("' and e1.id in (select event_id from tournaments where event_id = e1.id and lottery_ready = false)");

            return this.Query(sb.ToString()).Tables[0];
        }

        public DataSet getEventsByName(string name)
        {
            string sql = "select * from events where name = '" + name + "';";
            return this.Query(sql);
        }

        public DataSet getEventsBydate(string date)
        {
            string sql = "select * from events where date = '" + date + "' and date is not null;";
            return this.Query(sql);
        }


    }
}
