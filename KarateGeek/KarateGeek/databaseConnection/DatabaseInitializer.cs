using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace KarateGeek.databaseConnection
{
    class DatabaseInitializer : CoreDatabaseConnection
    {
        public void foo()
        {
            this.NonQuery("drop table nikolas ;");
            this.NonQuery("create table nikolas (id int, primary key(id)) ;");
            this.Disconnect();

        }
        
    }
}
