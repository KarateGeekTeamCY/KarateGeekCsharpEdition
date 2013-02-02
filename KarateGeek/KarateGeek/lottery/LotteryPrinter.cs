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

        public LotteryPrinter(List<Tuple<List<long>, bool, int, int>> lotterySets, long tournamentId)
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

            bigBox = makeBigBox(lotterySets);

            /** The following is just an experimental proof-of-concept implementation of the constructor: */
            //bigBox = makeBox(lotteryList);
        }


        private char[][] allocateBigBox()  // 2D jagged char array filled with spaces
        {
            return null;
        }


        private char[][] makeBigBox(List<Tuple<List<long>, bool, int, int>> lotterySets)
        {
            //char[][] tmpBigBox = allocateBigBox();

            //TournamentTreeToBox(..., ...);
            
            return TournamentTreeToBox(lotterySets);
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


        private void insertSmallBox(char[][] bigBox, LotteryBox smallBox, int x, int y)
        {
            if (y + smallBox.realHeight - 1 > bigBox.Length )
                throw new Exception("External character box not big enough!");

            var tmpSmallBox = smallBox.get();

            for (int line = 0; line < smallBox.realHeight; ++line)
                tmpSmallBox[line].CopyTo(bigBox[line + y], x);
        }


        private List<Tuple<List<long>, bool, int, int>> sortPhaseDescPositionAsc(List<Tuple<List<long>, bool, int, int>> unsorted)
        {
            return unsorted.OrderBy(x => x.Item4).OrderByDescending(x => x.Item3).ToList();
        }


        private char[][] TournamentTreeToBox(List<Tuple<List<long>, bool, int, int>> Sets) // implementation ONLY for "versus"-type tournaments!
        {
            Sets = sortPhaseDescPositionAsc(Sets); // line probably not needed at all

            LotteryPrinterConnection conn = new LotteryPrinterConnection();


            // get smallbox size

            /* get the size of the largest small box: */

            int maxSetCount = 0;
            Tuple<List<long>, bool, int, int> maxSet = null;

            foreach (var set in Sets)
                if (set.Item1.Count > maxSetCount) {
                    maxSetCount = set.Item1.Count;
                    maxSet = set;
                }

            var testBoxData = conn.getAthleteNameList(maxSet);
            int smallBoxHeight = new LotteryBox(testBoxData, BoxTypeLeft.connected, BoxTypeRight.connected_down).realHeight;
            int smallBoxWidthFirstPhase = new LotteryBox(testBoxData, BoxTypeLeft.unconnected, BoxTypeRight.connected_down).realWidth;
            int smallBoxWidthMiddlePhases = new LotteryBox(testBoxData, BoxTypeLeft.connected, BoxTypeRight.connected_down).realWidth;
            int smallBoxWidthLastPhase = new LotteryBox(testBoxData, BoxTypeLeft.connected, BoxTypeRight.unconnected).realWidth;


            // allocate "big box" of suitable size
         
            /* int numOfNonEmptySmallBoxesOfFirstPhase = Sets.OrderByDescending(x => x.Item4).OrderByDescending(x => x.Item3).First().Item4; */ //real number of non-empty boxes of the 1st phase
            int numOfSmallBoxesOfFirstPhase = (int)Math.Pow(2, Sets.First().Item3);
            int numOfPhases = Sets.First().Item3 + 1;

            int bigBoxHeight = numOfSmallBoxesOfFirstPhase * (smallBoxHeight + 1);
            //int bigBoxWidth  = smallBoxWidthFirstPhase + (numOfPhases - 2) * smallBoxWidthMiddlePhases + smallBoxWidthLastPhase;
            int bigBoxWidth  = (numOfPhases - 1) * smallBoxWidthMiddlePhases + smallBoxWidthLastPhase;

            char[][] tmpBigBox = new char[bigBoxHeight][];

            for (int line = 0; line < bigBoxHeight; ++line)
                tmpBigBox[line] = new StringBuilder().Append('□', bigBoxWidth).ToString().ToCharArray();

            // build "small boxes" one-by-one while traversing the list "Sets", and insert them into the "big box"

            bool directiondown = true;
            for (int phase = numOfPhases - 1; phase >= 0; phase--)
            {
                for (int position = 1; position <= (int)Math.Pow(2, phase); ++position)
                {
                    Tuple<List<long>, bool, int, int> head;

                    if (Sets == null || Sets.Count == 0)
                        head = null;
                    else
                    {
                        head = Sets.First();

                        if (head.Item3 == phase && head.Item4 == position)  // assumes ordered set
                            Sets = Sets.Skip(1).ToList();
                        else
                            head = null;
                    }
                   
                    // (head == null) is handled by LotteryPrinterConnection.getAthleteNameList()
                    LotteryBox smallBox = new LotteryBox( conn.getAthleteNameList(head, maxSetCount),
                                                          (phase == numOfPhases - 1) ? BoxTypeLeft.unconnected : BoxTypeLeft.connected,
                                                          (phase == 0) ? BoxTypeRight.unconnected : directiondown ? BoxTypeRight.connected_down : BoxTypeRight.connected_up
                                                        );

                    insertSmallBox(tmpBigBox, smallBox, (phase == numOfPhases - 1) ? (smallBoxWidthMiddlePhases - smallBoxWidthFirstPhase) : (numOfPhases - (phase + 1)) * smallBoxWidthMiddlePhases, (position - 1) * (smallBoxHeight + 1)); // WRONG position calculation

                    //int offset = getOffset(smallBoxHeight + 1, (numOfPhases - 1) - phase);
                    //insertSmallBox(tmpBigBox, smallBox, (phase == numOfPhases - 1) ? (smallBoxWidthMiddlePhases - smallBoxWidthFirstPhase) : (numOfPhases - (phase + 1)) * smallBoxWidthMiddlePhases, (position == 1) ? offset : 2 * offset); // WRONG position calculation

                    directiondown = !directiondown;
                }
            }

            return tmpBigBox;
        }

        
        private int getOffset(int boxSize, int depth)
        {
            return (int) (boxSize * Math.Pow(2, depth - 1) - boxSize / 2.0);
        }


        private int startingY(int bigBoxHeight, int smallBoxHeight, int depth)
        {
            ////depth =  numOfPhases -  currentphase
            //int drawSpace = (bigBoxHeight / (int)Math.Pow(2, depth));
            //int offset = drawSpace - smallBoxHeight * (int)Math.Pow(2, currentPhase);
            ////tha kanei set to offet se mia alli methodo an einai arnitiko eimaste ston teliko
            //return (bigBoxHeight - drawSpace) / 2;
            return 0;
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
