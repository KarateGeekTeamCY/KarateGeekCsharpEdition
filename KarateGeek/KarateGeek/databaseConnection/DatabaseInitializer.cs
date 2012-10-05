using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarateGeek.databaseConnection
{
    class DatabaseInitializer : CoreDatabaseConnection
    {
        public void foo()
        {
            this.Connect();
            this.NonQuery("create table nikolas (id int, primary key(id))");
            this.Disconnect();

        
        }
        

    }
}
