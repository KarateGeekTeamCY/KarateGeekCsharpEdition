using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;
using System.IO;
using KarateGeek.helpers;

namespace KarateGeek.databaseConnection
{

    class ClubConnection : CoreDatabaseConnection
    {

        private string sql = "";


        public string InsertNewCLub(string name, string phone, string email, string logosource, string addressStreetName, string addressStreetNumber, string addressPostalCode , string countryCode, string City)
        {
            //FileStream fs = new FileStream(logosource, FileMode.Open, FileAccess.Read);
            //BinaryReader binr = new BinaryReader(new BufferedStream(fs));
            //byte[] logoinbites = binr.ReadBytes(Convert.ToInt32(fs.Length));

            ImageConverter ic = new ImageConverter();
            AddressConnection addConn = new AddressConnection();
            string addressId = addConn.InsertNewAddress(addressStreetName, addressStreetNumber, City, addressPostalCode, countryCode);

            sql = "insert into Clubs (name, phone, email, logo, address_id, country_code) values ("+
                "'" + name + "', " +
                "'" + phone + "', " +
                "'" + email + "', " +
                "'" + ic.ToByteTable(logosource) + "' , " +
                "'" + addressId + "', " +
                "'" + countryCode + "' " +
                ")";
            this.NonQuery(sql);

            return "";
        }

        public DataSet findSimilar(string filter)
        {
            string sql = "select * from clubs where name like '" + filter + "%';";
            return this.Query(sql);
        }

        public string UpdateClub()
        {
    
    
            return "";
        }



        public DataSet GetClubs()
        {
            sql = "select * from clubs ;";
            return this.Query(sql);
        }




       
    }

}
