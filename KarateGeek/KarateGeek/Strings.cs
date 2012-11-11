using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarateGeek
{
    static class Strings
    {


        public const string phase2 = "FINAL";
        public const string phase4 = "SEMI FINAL";
        public const string phase8 = "PHASE OF 8";
        public const string phase16 = "PHASE OF 16";
        public const string phase32 = "PHASE OF 32";
        public const string phase64 = "PHASE OF 64";
        public const string phase128 = "PHASE OF 128";

        public static string[] phase = { phase2, phase4, phase8, phase16,
                                                 phase32, phase64, phase128
                                               };


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
        public const string rank01 = "White  –  6th kyu";     // TODO: change "static " to "const"??
        public const string rank02 = "Yellow –  5th kyu";
        public const string rank03 = "Orange –  4th kyu";
        public const string rank04 = "Green  –  3th kyu";
        public const string rank05 = "Blue   –  2nd kyu";
        public const string rank06 = "Brown  –  1st kyu";
        public const string rank07 = "Black  –  1st dan";
        public const string rank08 = "Black  –  2nd dan";
        public const string rank09 = "Black  –  3th dan";
        public const string rank10 = "Black  –  4th dan";
        public const string rank11 = "Black  –  5th dan";
        public const string rank12 = "White/Red – 6th dan";
        public const string rank13 = "White/Red – 7th dan";
        public const string rank14 = "White/Red – 8th dan";
        public const string rank15 = "Red    –  9th dan";
        public const string rank16 = "Red    –  10th dan";

        public static string[] rank = { rank01, rank02, rank03, rank04,
                                                 rank05, rank06, rank07, rank08,
                                                 rank09, rank10, rank11, rank12,
                                                 rank13, rank14, rank15, rank16
                                               };

        //
        // game types
        //

        public const string indKata    = "IMD|KATA";
        public const string teamKata   = "TEAM|KATA";
        public const string syncKata   = "SYNCHRONIZED KATA";
        public const string indKumite  = "IND|KUMITE";
        public const string teamKumite = "TEAM|KUMITE";
        public const string enbu       = "EN-BU";
        public const string fugugo     = "FUGU-GO";
        public const string team       = "TEAM";
        public const string individual = "IND";

        //
        // judging types
        //
        public const string flag = "FLAG";         // einai to flag system

        public const string point = "POINT";       //  einai to sistima me tis 
                                                    // diaforetikes vathmologies
                                                    // analoga me tin epitixia
                                                    // tis texnikis

        public const string score = "SCORE";       // einai i arithmitiki vathmologia
                                                    // me vasi kritiria texnikis



        //
        // kumite points
        //

        public const string ippon = "IPPON";
        public const string wazaari = "WAZA-ARI";
        public const string jyogai = "JUO-GAI";
        public const string chui = "CHUI";
        public const string keikoku = "KEI-KOKU";
        public const string tento = "TEN-TO";
        public const string doctorStop = "DOCTOR STOP";



        //
        // person sex
        //
        public const string male = "MALE";
        public const string female = "FEMALE";




        //public static string ;

    }
}
