using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarateGeek
{
    static class Strings
    {


        public const string phase2   = "FINAL";
        public const string phase4   = "SEMI-FINAL";
        public const string phase8   = "PHASE OF 8";
        public const string phase16  = "PHASE OF 16";
        public const string phase32  = "PHASE OF 32";
        public const string phase64  = "PHASE OF 64";
        public const string phase128 = "PHASE OF 128";
        public const string phase256 = "PHASE OF 256";

        public static readonly string[] phase = { phase2, phase4, phase8, phase16,
                                                  phase32, phase64, phase128, phase256
                                                };


        //
        // ranking types
        //

        /*  Info:
         *
         *  9th dan - 10th dan  –  Wide Red (optional as Sensei Ilija Yorga himself wore a Black Belt) Master's title
         *  6th dan - 8th dan   –  White and Red, Master's title
         *  1st dan - 5th dan   –  Black
         *  1st kyu             –  Brown (at least 25 Month of traning or more)
         *  2nd kyu             –  Blue (at least 18 Month of traning or more)
         *  3rd kyu             –  Green (at least 15 Month of traning or more)
         *  4th kyu             –  Orange (at least 9 Month of traning or more)
         *  5th kyu             –  Yellow (at least 7 Month of traning or more)
         *  6th kyu             –  White (at least 5 Month of traning or more)
         */

        /*
        public const string rank01 = "White";
        public const string rank02 = "Yellow";
        public const string rank03 = "Orange";
        public const string rank04 = "Green";
        public const string rank05 = "Blue";
        public const string rank06 = "Purple";
        public const string rank07 = "Brown I";
        public const string rank08 = "Brown II";
        public const string rank09 = "Brown III";
        public const string rank10 = "Black";
        //public const string rank10 = "Black  –  4th dan";
        //public const string rank11 = "Black  –  5th dan";
        //public const string rank12 = "White/Red – 6th dan";
        //public const string rank13 = "White/Red – 7th dan";
        //public const string rank14 = "White/Red – 8th dan";
        //public const string rank15 = "Red    –  9th dan";
        //public const string rank16 = "Red    –  10th dan";

        public static readonly string[] rank = { rank01, rank02, rank03, rank04,
                                                 rank05, rank06, rank07, rank08,
                                                 rank09, rank10
                                               };

        */

        public const string rank01 = "White (9th kyū)";
        public const string rank02 = "Yellow (8th kyū)";
        public const string rank03 = "Orange (7th kyū)";
        public const string rank04 = "Green (6th kyū)";
        public const string rank05 = "Blue (5th kyū)";
        public const string rank06 = "Purple (4th kyū)";
        public const string rank07 = "Brown I (3rd kyū)";
        public const string rank08 = "Brown II (2nd kyū)";
        public const string rank09 = "Brown III (1st kyū)";
        public const string rank10 = "Black (1st dan)";
        public const string rank11 = "Black (2nd dan)";
        public const string rank12 = "Black (3rd dan)";
        public const string rank13 = "Black (4th dan)";
        public const string rank14 = "Black (5th dan)";
        public const string rank15 = "Black (6th dan)";
        public const string rank16 = "Black (7th dan)";
        public const string rank17 = "Black (8th dan)";
        public const string rank18 = "Black (9th dan)";
        public const string rank19 = "Black (10th dan)";

        public static readonly string[] rank = { rank01, rank02, rank03, rank04,
                                                 rank05, rank06, rank07, rank08,
                                                 rank09, rank10, rank11, rank12,
                                                 rank13, rank14, rank15, rank16,
                                                 rank17, rank18, rank19
                                               };

        //
        // game types (tournament types)
        //


        public const string left = "LEFT";
        public const string right = "RIGHT";

        public const string aka = "白-AKA";
        public const string shiro = "赤-SHIRO";

        public const string prefixInd  = "IND";
        public const string prefixTeam = "TEAM";
        public const string prefixSync = "SYNC";

        public const string gameTypeDelim = "|";

        public const string suffixKata     = "KATA";                // suffixes are useful for SQL's "LIKE" etc.
        //public const string suffixSyncKata = "SYNCHRONIZED KATA"; // NOTE: suffixSyncKata has the same suffix as suffixKata on purpose!
        public const string suffixKumite   = "KUMITE";
        public const string suffixEnbu     = "EN-BU";
        public const string suffixFugugo   = "FUGU-GO";

        public const string indKata    = prefixInd  + gameTypeDelim + suffixKata;   // score or flag
        public const string teamKata   = prefixTeam + gameTypeDelim + suffixKata;   // score or flag
        public const string syncKata   = prefixSync + gameTypeDelim + suffixKata;
        public const string indKumite  = prefixInd  + gameTypeDelim + suffixKumite; // points
        public const string teamKumite = prefixTeam + gameTypeDelim + suffixKumite; // points
        public const string enbu       = prefixSync + gameTypeDelim + suffixEnbu;   // score
        public const string fugugo     = prefixInd  + gameTypeDelim + suffixFugugo; // points/flag

        public const string individual   = prefixInd;   // for compatibility
        public const string team         = prefixTeam;  // for compatibility
        public const string synchronized = prefixSync;  // for compatibility


        //
        // scoring types
        //

        public const string flag  = "FLAG";         // einai to flag system

        public const string point = "POINT";        // einai to sistima me tis
                                                    // diaforetikes vathmologies
                                                    // analoga me tin epitixia
                                                    // tis texnikis

        public const string score = "SCORE";        // einai i arithmitiki vathmologia
                                                    // me vasi kritiria texnikis

        //
        // kumite points
        //

        public const string ippon      = "IPPON";
        public const string wazaari    = "WAZA-ARI";
        public const string jyogai     = "JUO-GAI";
        public const string chui       = "CHUI";
        public const string keikoku    = "KEI-KOKU";
        public const string tento      = "TEN-TO";
        public const string doctorStop = "DOCTOR STOP";
        public const string bychoice   = "BY_CHOICE";


        //
        // person sex
        //

        public const string male   = "MALE";
        public const string female = "FEMALE";
        public const string mixed = "MIXED";

    }
}
