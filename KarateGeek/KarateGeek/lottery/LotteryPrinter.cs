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
     *  of "LotteryBox" as building blocks...
     *
     *  It can be used both directly (passing a LotteryGenerator "LotterySets" structure
     *  to its constructor, before approving the lottery) and indirectly (querying the
     *  database itself for tournaments with already-approved lotteries, passing only the
     *  tournamentId to the LotteryPrinter constructor).                                  */
    class LotteryPrinter
    {
        
        /** Class fields/properties: **/

        private char[][] bigBox = null;         // 2D character buffer, set by constructor

        private bool isListOfTeams;             // false: input is a list of athletes          (a bit ugly)
                                                // true:  input is a list of teams of athletes
        private long tournamentId;

        private readonly char spaceChar = '□';
        //private readonly char spaceChar = ' ';

        private const int defaultMaxNameLength = 18;    // default 18, same as the "defaultWidth" for the LotteryBox class
        private int maxNameLength;


        /** Class methods: **/

        // overloaded constructor (useful for the lotteries of "unlotterised" tournaments)
        public LotteryPrinter(List<Tuple<List<long>, bool, int, int>> lotterySets, long tournamentId, int maxNameLength = defaultMaxNameLength)
        {
            this.tournamentId = tournamentId;
            this.maxNameLength = maxNameLength;

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


        // overloaded constructor (useful for "lotterised" tournaments)
        public LotteryPrinter(long tournamentId, int maxNameLength = defaultMaxNameLength)
        {
            this.tournamentId = tournamentId;
            this.maxNameLength = maxNameLength;

            switch (new LotteryGenConnection().getTournamentGameType(tournamentId))
            {
                case Strings.enbu:
                case Strings.syncKata:
                case Strings.teamKata:
                case Strings.teamKumite: this.isListOfTeams = true;
                    break;

                default: this.isListOfTeams = false;
                    break;
            }

            /* Now get the "lotterySets" by querying the database: */

            LotteryPrinterConnection conn = new LotteryPrinterConnection();

            var lotterySets = conn.getPrintableLotterySetsFromDB(tournamentId);            // not implemented yet

            bigBox = makeBigBox(lotterySets);

            /* ... */
        }


        private char[][] allocateBigBox(int bigBoxHeight, int bigBoxWidth)  // 2D jagged char array filled with spaces
        {
            char[][] tmpBigBox = new char[bigBoxHeight][];

            for (int line = 0; line < bigBoxHeight; ++line)
                tmpBigBox[line] = new StringBuilder().Append(spaceChar, bigBoxWidth).ToString().ToCharArray();

            return tmpBigBox;
        }


        private char[][] makeBigBox(List<Tuple<List<long>, bool, int, int>> lotterySets)
        {
            // TODO: call either TournamentTreeToBox() or TournamentExpoLotteryToBox(), depending on this.tournamentId

            return TournamentTreeToBox(lotterySets);
        }


        /** The following is just an obsolete, experimental proof-of-concept implementation of makeBigBox(): */
        private char[][] makeBox(List<long> lotteryList) // monster method, and it doesn't even work for all cases...
                                                         // only useful as a template
        {
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

                    tmpBox = new LotteryBox(name, BoxTypeLeft.unconnected, directiondown ? BoxTypeRight.connected_down : BoxTypeRight.connected_up, this.maxNameLength).get();
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

                    tmpBox = new LotteryBox(nameList, BoxTypeLeft.unconnected, directiondown ? BoxTypeRight.connected_down : BoxTypeRight.connected_up, this.maxNameLength).get();
                }

                for (int i = 0; i < tmpBox.Length; ++i) {
                    tmpBigBox[line] = tmpBox[i];
                    ++line;
                }

                directiondown = !directiondown;
                ++line;
            }
            return tmpBigBox;
        }


        private void insertSmallBox(char[][] bigBox, LotteryBox smallBox, int x, int y)
        {
            //if (y + smallBox.realHeight - 1 > bigBox.Length)
            //    throw new Exception("External character box not big enough!");

            Debug.Assert(y + smallBox.realHeight <= bigBox.Length + 1);

            Debug.WriteLine("insertSmallBox() debug message: x: {0,10}, y: {1,10}", x, y);

            var tmpSmallBox = smallBox.get();

            for (int line = 0; line < smallBox.realHeight; ++line) // allow 1-char overlapping: Blanks get overwritten, other characters do not
                if (bigBox[line + y][x] == spaceChar)
                    tmpSmallBox[line].CopyTo(bigBox[line + y], x);
                else
                    tmpSmallBox[line].Skip(1).ToArray().CopyTo(bigBox[line + y], x + 1);
        }


        private bool closingCharAhead(char[][] box, char openingChar, int row, int col)
        {
            char closingChar = (openingChar == '┐') ? '├' : '┘';   // else opening char is '├' or '┌'... a bit hackish ;)
            char nonClosingChar = (openingChar == '┐') ? '┘' : '├';

            for (row = row + 1; row < box.Length; ++row) {
                if (box[row][col] == nonClosingChar)
                    return false;
                if (box[row][col] == closingChar)
                    return true;
            }

            return false;   // bigBox.Length reached
        }


        private void fixConnectionsOf(char[][] bigBox, int start, int interval) // assumes that all lines of the jagged array have the same length as the 1st one
        {
            bool fillNeeded;
            char deletionMark = '✓';    // choose something unique!

            for (int col = start; col < bigBox[0].Length; col += interval) {
                fillNeeded = false;

                for (int row = 0; row < bigBox.Length; ++row) {
                    if (bigBox[row][col] == '┐' && closingCharAhead(bigBox, openingChar: '┐', row: row, col: col))
                        fillNeeded = true;

                    if (bigBox[row][col] == '┘')
                        fillNeeded = false;

                    if (bigBox[row][col] == '├') {

                        if (!fillNeeded)
                            bigBox[row][col] = '┌';

                        if (closingCharAhead(bigBox, openingChar: '├', row: row, col: col))
                            fillNeeded = true;
                        else {
                            if (fillNeeded)
                                bigBox[row][col] = '└';
                            else
                                bigBox[row][col] = deletionMark;            // !!
                            fillNeeded = false;
                        }
                            
                    }

                    if (bigBox[row][col] == spaceChar && fillNeeded)
                        bigBox[row][col] = '│';
                }

            }

            // remove unneeded "├──┤" connectors:
            for (int row = 0; row < bigBox.Length; ++row)
                for (int col = start; col < bigBox[0].Length; col += interval)
                    if (bigBox[row][col] == deletionMark)
                        "   │".ToCharArray().CopyTo(bigBox[row], col);
        }


        private List<Tuple<List<long>, bool, int, int>> sortPhaseDescPositionAsc(List<Tuple<List<long>, bool, int, int>> unsorted)
        {
            return unsorted.OrderBy(x => x.Item4).OrderByDescending(x => x.Item3).ToList();
        }


        private char[][] TournamentTreeToBox(List<Tuple<List<long>, bool, int, int>> Sets) // implementation ONLY for "versus"-type tournaments!
        {
            Sets = sortPhaseDescPositionAsc(Sets); // defensive coding; line probably not needed at all, we pass already-ordered Sets

            { //debug
                int i = 1;
                foreach (var set in Sets) {
                    Debug.WriteLine("\nset #{0,2}:", i);
                    ++i;
                    if (set.Item1.Count > 0)
                        foreach (long id in set.Item1)
                            Debug.WriteLine("  id:{0,4}  phase:{1,4}  position:{2,4}", id, set.Item3, set.Item4);
                }
            }

            LotteryPrinterConnection conn = new LotteryPrinterConnection();


            /** Get smallbox size (the dimensions of the largest "small box"): */

            int maxSetCount = 0;
            Tuple<List<long>, bool, int, int> maxSet = null;

            foreach (var set in Sets)
                if (set.Item1.Count > maxSetCount)
                {
                    maxSetCount = set.Item1.Count;
                    maxSet = set;
                }

            var testBoxData = conn.getAthleteNameList(maxSet);
            int smallBoxHeight = new LotteryBox(testBoxData, BoxTypeLeft.connected, BoxTypeRight.connected_down, this.maxNameLength).realHeight;
            int smallBoxWidthFirstPhase = new LotteryBox(testBoxData, BoxTypeLeft.unconnected, BoxTypeRight.connected_down, this.maxNameLength).realWidth;
            int smallBoxWidthMiddlePhases = new LotteryBox(testBoxData, BoxTypeLeft.connected, BoxTypeRight.connected_down, this.maxNameLength).realWidth;
            int smallBoxWidthLastPhase = new LotteryBox(testBoxData, BoxTypeLeft.connected, BoxTypeRight.unconnected, this.maxNameLength).realWidth;


            /** Allocate "big box" of suitable size: */

            /* int numOfNonEmptySmallBoxesOfFirstPhase = Sets.OrderByDescending(x => x.Item4).OrderByDescending(x => x.Item3).First().Item4; //real number of non-empty boxes of the 1st phase */
            int numOfSmallBoxesOfFirstPhase = (int)Math.Pow(2, Sets.First().Item3);
            int numOfPhases = Sets.First().Item3 + 1;

            int charOverlap = 3; // hardcoded for now, TODO: find a cleaner way to do it...

            int bigBoxHeight = numOfSmallBoxesOfFirstPhase * (smallBoxHeight + 1);
            //int bigBoxHeight = (int)(0.5 * smallBoxHeight) + 2 * (int)(0.5 * smallBoxHeight) * (numOfSmallBoxesOfFirstPhase - 1) + smallBoxHeight; // ??
            int bigBoxWidth = smallBoxWidthFirstPhase + (numOfPhases - 2) * (smallBoxWidthMiddlePhases - charOverlap) + (smallBoxWidthLastPhase - charOverlap);

            char[][] tmpBigBox = allocateBigBox(bigBoxHeight + 100, bigBoxWidth); // FIXME: "+ 100" is a workaround for a crashing bug (perhaps a rounding error?)


            /** Build "small boxes" one-by-one while traversing the list "Sets", and insert them into the "big box": */

            bool directiondown = true;
            for (int phase = numOfPhases - 1; phase >= 0; --phase)
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

                    // the special (head == null) case is handled by the LotteryPrinterConnection.getAthleteNameList() method
                    LotteryBox smallBox = new LotteryBox( conn.getAthleteNameList(head, maxSetCount),
                                                          (phase == numOfPhases - 1) ? BoxTypeLeft.unconnected : BoxTypeLeft.connected,
                                                          (phase == 0) ? BoxTypeRight.unconnected : directiondown ? BoxTypeRight.connected_down : BoxTypeRight.connected_up,
                                                          this.maxNameLength
                                                        );

                    int depth = (numOfPhases - 1) - phase;
                    int offset = getOffset(smallBoxHeight + 1, depth);

                    Debug.WriteLine("TournamentTreeToBox() message:  phase: {0,6}, position: {1,3}, offset: {2,5}",
                                    phase, position, offset);

                    insertSmallBox( tmpBigBox,
                                    smallBox,
                                    x: (phase == numOfPhases - 1) ? 0 : depth * (smallBoxWidthMiddlePhases - charOverlap) - (smallBoxWidthMiddlePhases - smallBoxWidthFirstPhase),
                                    y: ((position == 1) ? offset : offset + 2 * offset * (position - 1)) - (smallBoxHeight + 1) / 2 + ((phase == numOfPhases - 1) ? position - 1 : 0)
                                  );

                    directiondown = !directiondown;
                }
            }

            fixConnectionsOf(tmpBigBox, smallBoxWidthFirstPhase - charOverlap, smallBoxWidthMiddlePhases - charOverlap);

            return tmpBigBox;
        }

        
        private int getOffset(int boxHeight, int depth)
        {
            int offset = (int)Math.Floor(boxHeight * Math.Pow(2, depth - 1));  // floor??

            Debug.WriteLine("getOffset() debug message:      boxHeight: {0,2}, depth: {1,6}, offset: {2,5}",
                boxHeight, depth, offset);

            return offset;
        }


        public char[][] get()                   // returns the constructed bigBox (probably useless)
        {
            return bigBox;
        }


        private bool isEmpty(char[] line)
        {
            foreach (char c in line)
                if (c != spaceChar)
                    return false;

            return true;
        }


        public override string ToString()       // returns the 2D character array as a (huge) string
        {
            StringBuilder sb = new StringBuilder();

            foreach (char[] line in bigBox)
                //if (!isEmpty(line))
                    sb.Append(line).Append('\n');

            return sb.ToString();               // When using this as a GUI label, the first underscore
                                                // always gets printed with the following character on top
                                                // of it. This is a .NET bug (?), not ours. sb is 100% OK.
        }

    }
}
