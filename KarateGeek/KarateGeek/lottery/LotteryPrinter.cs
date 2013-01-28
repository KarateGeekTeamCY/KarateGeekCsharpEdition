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

        private bool isListOfTeams;             // false: input is a list of athletes          (a bit ugly)
                                                // true:  input is a list of teams of athletes
        private long tournamentId;


        /** Class methods: **/

        public LotteryPrinter(List<long> lotteryList, long tournamentId)
        {
            this.tournamentId = tournamentId;

            switch (new LotteryGenConnection().getTournamentGameType(tournamentId))
            {
                case Strings.enbu:
                case Strings.syncKata:
                case Strings.teamKata:
                case Strings.teamKumite: this.isListOfTeams = true;
                                         break;

                default:                 this.isListOfTeams = false;
                                         break;
            }

            /** The following is just an experimental proof-of-concept implementation of the constructor: */

            bigBox = makeBox(lotteryList);
        }


        private char[][] makeBox(List<long> lotteryList) // monster method, and it doesn't even work for all cases...
                                                         // only useful as a template
        {
            /** The following is just an experimental proof-of-concept implementation of makeBox(): */

            char[][] tmpBigBox = new char[lotteryList.Count * (isListOfTeams ? 5 : 3) + (lotteryList.Count - 1)][]; // 5 is WRONG!

            int line = 0;
            bool directiondown = true;

            foreach (var memberId in lotteryList) {
                string name;
                List<string> nameList;
                char[][] tmpBox;

                if (!isListOfTeams) {
                    var firstlast = new CoreDatabaseConnection().Query(
                                        "SELECT last_name, first_name FROM athletes NATURAL JOIN persons "
                                      + "WHERE id = " + memberId + " ;"
                                    ).Tables[0].Rows[0];
                    name = firstlast[0].ToString() + " " + firstlast[1].ToString();

                    tmpBox = new LotteryBox(name, BoxTypeLeft.unconnected, directiondown ? BoxTypeRight.connected_down : BoxTypeRight.connected_up).get();
                }
                else {
                    var firstlast = new CoreDatabaseConnection().Query(
                                        " SELECT p.last_name, p.first_name"
                                      + " FROM (tournament_participations t JOIN athletes a ON t.athlete_id = a.id)"
                                      + " NATURAL JOIN persons p"
                                      + " WHERE tournament_id = " + tournamentId + " AND team_id = " + memberId + " ;"
                                    ).Tables[0];

                    nameList = new List<string>();
                    for (int i = 0; i < firstlast.Rows.Count; ++i)
                        nameList.Add(firstlast.Rows[i][0] + " " + firstlast.Rows[i][1]);

                    tmpBox = new LotteryBox(nameList, BoxTypeLeft.unconnected, directiondown ? BoxTypeRight.connected_down : BoxTypeRight.connected_up).get();
                }

                tmpBigBox[line] = "kdshbfvjdsbfvlkjdsbvfkjsdbvjkDSbvkjSdbfkjsdbfkjSDbnv.kjSDbnvkSJNvkjDSnvk>                                                  ".ToCharArray();

                for (int i = 0; i < tmpBox.Length; ++i) {
                    tmpBigBox[line] = tmpBox[i];

                    //tmpBigBox[line] = String.Concat(tmpBigBox[line].ToString(), tmpBox[i].ToString()).ToCharArray();

                    //tmpBox[i].CopyTo(tmpBigBox[line], 10);
                    
                    
                    ++line;
                }
                directiondown = !directiondown;
                ++line;
            }
            return tmpBigBox;
        }


        private char[][] TournamentTreeToBox(List<long> lotteryList) {


            return null;
        }


        public char[][] get()                   // returns the constructed bigBox (probably useless)
        {
            return bigBox;
        }


        public override string ToString()       // returns the 2D character array as a (huge) string
        {
            StringBuilder sb = new StringBuilder();

            foreach (char[] line in bigBox)
                sb.Append(line).Append('\n');

            return sb.ToString();               // When using this as a GUI label, the first underscore
                                                // always gets printed with the following character on top
                                                // of it. This is a .NET bug (?), not ours. sb is 100% OK.
        }

    }
}
