using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;   // has Debug.Assert()

namespace KarateGeek.lottery
{

    enum BoxTypeLeft {  // types of LotteryBox, depending on how they are connected on the left side
        unconnected,
        connected
    }

    enum BoxTypeRight { // types of LotteryBox, depending on how they are connected on the right side
        unconnected,
        connected_down,
        connected_up
    }


    /** This class constructs ASCII-art style text "boxes" with unicode line drawing
     * characters (the "light" ones), suitably formatted... Used by our LotteryPrinter class. */
    class LotteryBox
    {
        /* Example of two connected boxes (boxWidth = 18):
         * 
         * ┏━━━━━━━━━━┓         // "heavy", boxWidth = 10
         * ┃  Athlete 1's name ┣━┓
         * ┗━━━━━━━━━━┛  ┃
         * 
         *
         * BTW, the following string can be printed (with a mono font) as a test!
         * 
         *   "┌──────────────────┐\n"   // "light", boxWidth = 18 (light works better!)
           + "│ Athlete 1's name ├──┐\n"
           + "└──────────────────┘  │\n"
           + "                      ├──\n"
           + "┌──────────────────┐  │\n"
           + "│ Athlete 2's name ├──┘\n"
           + "└──────────────────┘\n";
         * 
         * 
         * 
         **/

        /** Class fields/properties: **/

        private char[][] box = null;    // "jagged array" as a 2D character buffer, set by the constructor

        private const int defaultWidth = 18;    // not counting the margin, so with the margin it's 20 chars
                                                // and with the "──┐├──" part it's 25 (!) chars

        public readonly BoxTypeLeft  typeLeft;
        public readonly BoxTypeRight typeRight;

        public int boxHeight {          // height (without margins etc.), in character rows
            get;
            set;
        }

        public int boxWidth {           // width (without margins etc.), in character columns
            get;
            set;
        }

        public int realHeight{  // the REAL height of the whole box, for now (boxHeight + 2)
                                // maybe useful for whoever creates LotteryBox instances (eg. LotteryPrinter)...
            get { return boxHeight + 2;
                }
        }

        public int realWidth{   // the REAL width of the whole box, either (boxWidth + 2) or (boxWidth + 7)
                                // maybe useful for whoever creates LotteryBox instances (eg. LotteryPrinter)...
            get { if (this.typeRight == BoxTypeRight.unconnected) return boxWidth + 2; 
                  else return boxWidth + 7; // 5 characters are needed for the "──┐├──" part...
                }
        }

        /** Class methods: **/

        public LotteryBox(string athlete, BoxTypeLeft typeLeft, BoxTypeRight typeRight)     // overloaded constructor
           : this(new List<string>() { athlete }, typeLeft, typeRight) { }


        public LotteryBox(List<string> team, BoxTypeLeft typeLeft, BoxTypeRight typeRight)  // overloaded constructor
        {
            this.typeLeft = typeLeft;
            this.typeRight = typeRight;
            this.boxHeight = team.Count;
            this.boxWidth = defaultWidth;

            this.box = makeBox(team);
        }


        private char[][] makeBox(List<string> team)
        {
            char[][] tmpBox = new char[this.realHeight][];
            StringBuilder sb = new StringBuilder(); // "sb" gets .Clear()ed after building every line of the tmpBox...

            tmpBox[0] = sb.Append('┌').Append('─', this.boxWidth).Append('┐').ToString().ToCharArray();
            sb.Clear();

            int line = 1;                           // "line" and "mid" are used in the foreach loop
            int mid = (team.Count + 1) / 2;

            foreach (var athlete in team) {
                if (line == mid && typeLeft == BoxTypeLeft.connected)
                    sb.Append('┤');
                else
                    sb.Append('│');

                sb.Append(Formatted(athlete));

                if (typeRight == BoxTypeRight.unconnected)
                    sb.Append('│');
                else{
                    if (line < mid) {
                        if (typeRight == BoxTypeRight.connected_down)
                            sb.Append('│');
                        else    // BoxTypeRight is connected_up
                            sb.Append("│  │");
                    }
                    else if (line == mid) {
                        if (typeRight == BoxTypeRight.connected_down)
                            sb.Append("├──┐");
                        else    // BoxTypeRight is connected_up
                            sb.Append("├──┘");
                    }
                    else { // (line > mid)
                        if (typeRight == BoxTypeRight.connected_down)
                            sb.Append("│  │");
                        else    // BoxTypeRight is connected_up
                            sb.Append('│');
                    }
                }

                tmpBox[line] = sb.ToString().ToCharArray();
                sb.Clear();
                ++line;
            }


            Debug.Assert(line == boxHeight - 1);
            tmpBox[boxHeight - 1] = sb.Append('└').Append('─', this.boxWidth).Append('┘').ToString().ToCharArray();
            //sb.Clear();

            foreach(var ln in tmpBox)
                Debug.Assert(ln.Length == boxWidth + 2 || ln.Length == boxWidth + 7); // ?

            return tmpBox;
        }


        private StringBuilder Formatted(string athlete)
        {          
            int len = Math.Min(athlete.Length, boxWidth);
            string athName = athlete.Substring(0, len);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < (boxWidth - len) / 2; ++i)
                sb.Append(' ');

            sb.Append(athName);

            for (int i = 0; i < (boxWidth - len + 1) / 2; ++i)
                sb.Append(' ');

            Debug.Assert(sb.Length == boxWidth);

            return sb;
        }


        public char[][] get()       // returns the constructed LotteryBox
        {
            return box;
        }

    }
}
