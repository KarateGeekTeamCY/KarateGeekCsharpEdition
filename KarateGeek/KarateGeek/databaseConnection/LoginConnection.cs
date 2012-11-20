using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using Npgsql;
using KarateGeek.helpers;

namespace KarateGeek.databaseConnection
{
    class LoginConnection : CoreDatabaseConnection
    {
        private string sql = null;
        private DataSet dr;
        private Cryptography cr = new Cryptography();
        protected bool personMngPerm;
        protected bool eventMngPerm;
        protected bool lotteryPerm;
        protected bool eventSupPerm;
        protected bool clubMngPerm;
        protected bool userMngPerm;
        protected bool reportsPerm;
        

        //na valete ton constructor se sxolio meta tin prwti fora ektelesis

        //public LoginConnection()
        //{

        //    UserConnection us = new UserConnection();
        //    us.insertNewUser();

        //}

      
        public Boolean compare(string username , string password) {

            password = cr.cryptography(password);
            //auto tha allaksei me to kainourgio erwtima otan ginei i vasi
            sql ="select * from users where username ='" + username + "' and password ='" + password + "';";

            dr = this.Query(sql);


            if (dr.Tables[0].Rows.Count != 0)
            {
                setRights();
                return true;
            }
            else
            {
                return false;
            }
        } 

        public void setRights(){

            this.personMngPerm = (bool)dr.Tables[0].Rows[0][3];
            this.eventMngPerm = (bool)dr.Tables[0].Rows[0][4];
            this.lotteryPerm = (bool)dr.Tables[0].Rows[0][5];
            this.eventSupPerm = (bool)dr.Tables[0].Rows[0][6];
            this.clubMngPerm = (bool)dr.Tables[0].Rows[0][7];
            this.userMngPerm = (bool)dr.Tables[0].Rows[0][8];
            this.reportsPerm = (bool)dr.Tables[0].Rows[0][9];
        }

        public bool getPersonMngPerm()
        {
            return personMngPerm;
        }

        public bool getEventMngPerm()
        {
            return eventMngPerm;
        }

        public bool getLotteryPerm()
        {
            return lotteryPerm;
        }

        public bool getEventSupPerm()
        {
            return eventSupPerm;
        }

        public bool getClubMngPerm()
        {
            return clubMngPerm;
        }

        public bool getUserMngPerm()
        {
            return userMngPerm;
        }

        public bool getReportsPerm()
        {
            return reportsPerm;
        }

    }
}
