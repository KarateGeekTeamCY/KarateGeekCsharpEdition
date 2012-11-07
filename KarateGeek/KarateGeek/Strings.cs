using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarateGeek
{
    static class Strings
    {
        

        //
        // ranking types
        //
        //9th dan - 10th dan- Wide Red optional as Sensei Ilija Yorga himself wore a Black Belt, Master's title
        //6th dan - 8th dan - White and Red, Master's title
        //1st dan - 5th dan - Black
        //1st kyu - Brown (at least 25 Month of traning or more)
        //2nd kyu - Blue (at least 18 Month of traning or more)
        //3rd kyu - Green (at least 15 Month of traning or more)
        //4th kyu - Orange (at least 9 Month of traning or more)
        //5th kyu - Yellow (at least 7 Month of traning or more)
        //6th kyu - White (at least 5 Month of traning or more)
        //
        public static readonly string rank01 = "White  –  6th kyu";
        public static readonly string rank02 = "Yellow –  5th kyu";
        public static readonly string rank03 = "Orange –  4th kyu";
        public static readonly string rank04 = "Green  –  3th kyu";
        public static readonly string rank05 = "Blue   –  2nd kyu";
        public static readonly string rank06 = "Brown  –  1st kyu";
        public static readonly string rank07 = "Black  –  1st dan";
        public static readonly string rank08 = "Black  –  2nd dan";
        public static readonly string rank09 = "Black  –  3th dan";
        public static readonly string rank10 = "Black  –  4th dan";
        public static readonly string rank11 = "Black  –  5th dan";
        public static readonly string rank12 = "White/Red – 6th dan";
        public static readonly string rank13 = "White/Red – 7th dan";
        public static readonly string rank14 = "White/Red – 8th dan";
        public static readonly string rank15 = "Red    –  9th dan";
        public static readonly string rank16 = "Red    –  10th dan";

        public static readonly string[] rank = { rank01, rank02, rank03, rank04,
                                                 rank05, rank06, rank07, rank08,
                                                 rank09, rank10, rank11, rank12,
                                                 rank13, rank14, rank15, rank16
                                               };

        //
        // game types
        //
        public static string indKata = "IND|KATA";
        public static string teamKata = "TEAM|KATA";
        public static string syncKata = "TEAM|SYNCHRONIZED KATA";
        public static string indKumite = "IND|KUMITE";
        public static string teamKumite = "TEAM|KUMITE";
        public static string enbu = "TEAM|EN-BU";
        public static string fugugo = "TEAM|FUGU-GO";
        public static string team = "TEAM";
        public static string individual = "IND";

        //
        // judging types
        //
        public static string flag = "FLAG";         // einai to flag system

        public static string point = "POINT";       //  einai to sistima me tis 
                                                    // diaforetikes vathmologies
                                                    // analoga me tin epitixia
                                                    // tis texnikis

        public static string score = "SCORE";       // einai i arithmitiki vathmologia
                                                    // me vasi kritiria texnikis



        //
        // kumite points
        //

        public static string ippon = "IPPON";
        public static string wazaari = "WAZA-ARI";
        public static string jyogai = "JUO-GAI";
        public static string chui = "CHUI";
        public static string keikoku = "KEI-KOKU";
        public static string tento = "TEN-TO";
        public static string doctorStop = "DOCTOR STOP";



        //
        // person sex
        //
        public static string male = "male";
        public static string female = "female";




        //public static string ;

    }
}
