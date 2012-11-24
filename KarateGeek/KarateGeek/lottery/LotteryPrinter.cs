using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KarateGeek.databaseConnection;
using System.Data;          // DataSet, DataTable
using System.Diagnostics;   // has Debug.Assert()

namespace KarateGeek.lottery
{
    /** This class constructs an ASCII-art style drawing of a lottery, using instances
     *  of "LotteryBox" as building blocks...                                          */
    class LotteryPrinter
    {
        
        /** Class fields/properties: **/

        private char[][] bigBox = null;         // 2D character buffer, set by constructor

        private bool isListOfTeams = false;     // false: input is a list of athletes          (a bit ugly)
                                                // true:  input is a list of teams of athletes


        /** Class methods: **/

        public LotteryPrinter(List<long> lotteryList, bool isListOfTeams)
        {
            this.isListOfTeams = isListOfTeams;

            /** The following is just an experimental proof-of-concept implementation of the constructor: */
            //char[][] tmpBigBox = new char[lotteryList.Count * (isListOfTeams ? 5 : 3)][]; // 5 is WRONG!

            Debug.Assert(!isListOfTeams);

            char[][] tmpBigBox = new char[lotteryList.Count * (isListOfTeams ? 5 : 3)][]; // 5 is WRONG!

            int line = 0;

            foreach (var memberId in lotteryList) {
                var firstlast = new CoreDatabaseConnection().Query(
                                    " SELECT last_name, first_name FROM athletes NATURAL JOIN persons"
                                  + " WHERE id = " + memberId + " ;"
                                ).Tables[0].Rows[0];
                string name = firstlast[0].ToString() + " " + firstlast[1].ToString();

                char[][] tmpBox = new LotteryBox(name, BoxTypeLeft.unconnected, BoxTypeRight.unconnected).get();
                for (int i = 0; i < tmpBox.Length; ++i) {
                    tmpBigBox[line] = tmpBox[i];
                    ++line;
                }
            }
        }


        private char[][] makeBox(List<string> team)
        {
            return null;
        }


        //public LotteryPrinter(long tournamentId)
        //{
            
        //}


        public char[][] get()                   // returns the constructed bigBox (probably useless)
        {
            return bigBox;
        }


        public override string ToString()       // return the 2D character array as a (huge) string
        {
            StringBuilder sb = new StringBuilder();

            foreach (char[] line in bigBox)
                sb.Append(line).Append('\n');

            return sb.ToString();
        }

    }
}
