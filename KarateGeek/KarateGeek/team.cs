using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KarateGeek
{
    public class Team : KarateGeek.databaseConnection.CoreDatabaseConnection
    {
        private string sql;

        public string id { get; set; }
        public string tournamentId { get; set; }

        private string _ranking;
        public string ranking
        {
            get { return _ranking; }
            set
            {
                this._ranking = value;
                this.update();
            }
        }

        public List<Athlete> participants { get; set; }

        public Team(string id)
        {
            this.id = id;
            this.participants = new List<Athlete>();
            this.load();
        }

        public void load()
        {

            sql = "SELECT * FROM team_tournament_participations WHERE id = '" + id + "'; ";
            DataTable temp = Query(sql).Tables[0];

            this.tournamentId = temp.Rows[0][3].ToString(); 

            if (temp.Rows[0][1] == null)
            {
                this.ranking = "128";
            }
            else
            {
                this.ranking = temp.Rows[0][1].ToString();
            }
            

            sql = "SELECT * FROM tournament_participations where tournament_id = '" + this.tournamentId
                + "' AND team_id = '" + this.id + "' ; ";

            temp = Query(sql).Tables[0];

            foreach (DataRow dr in temp.Rows)
            {
                this.participants.Add(new Athlete(dr[0].ToString(), this.tournamentId));
            }
        }


        private void update()
        {
            if (this._ranking != "")
            {
                sql = "UPDATE team_tournament_participations SET ranking = '" + _ranking
                    + "' WHERE id = '" + this.id
                    + "' AND tournament_id ='" + this.tournamentId + "' ; ";
                this.NonQuery(sql);

            }

        }






    }
}
