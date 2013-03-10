﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;
using KarateGeek.databaseConnection;

namespace KarateGeek
{
    public class Athlete : CoreDatabaseConnection
    {

        private string sql;

        public string tournamentId { get; set; }

        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        //public string midleName { get; set; }
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
        public string teamId { get; set; }



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


        DataTable athletes = new DataTable();

        public Athlete(string athleteId, string tournamentId)
        {
            this.id = athleteId;
            this.tournamentId = tournamentId;
            this.load();
        }



        public void load()
        {
            sql = "select * from athlete_view where id = '" + id + "'; ";
            DataTable temp = this.Query(sql).Tables[0];

            this.firstName = (string)temp.Rows[0][1];
            this.lastName = (string)temp.Rows[0][2];
            //this.midleName = (string)temp.Rows[0][3];
            this.sex = (string)temp.Rows[0][4];
            this.age = (string)temp.Rows[0][5].ToString();
            this.firstPhone = (string)temp.Rows[0][6];
            //this.secondPhone = (string)temp.Rows[0][7].ToString();
            //this.email = (string)temp.Rows[0][8];
            this.addressId = (string)temp.Rows[0][9].ToString();
            //this.addressStreet = (string)temp.Rows[0][10];
            //this.city = (string)temp.Rows[0][11];
            //this.country = (string)temp.Rows[0][12];
            //this.addressNumber = (string)temp.Rows[0][13];
            this.rank = (string)temp.Rows[0][14];

            sql = "select * from tournament_participations where athlete_id = '" + this.id + "' and tournament_id = '" + this.tournamentId + "'; ";
            temp = Query(sql).Tables[0];


            this.teamId = temp.Rows[0][1].ToString();
            this.ranking = temp.Rows[0][4].ToString();
        }

        private void update()
        {
            if (_ranking == "")
            { _ranking = "" + 128; }

            sql = "UPDATE tournament_participations SET ranking = '" + _ranking
                + "' WHERE athlete_id = '" + this.id
                + "' AND tournament_id ='" + this.tournamentId + "' ; ";
            this.NonQuery(sql);

        }

    }
}
