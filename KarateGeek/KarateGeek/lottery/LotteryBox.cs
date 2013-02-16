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
     * characters (the "light" ones), suitably formatted... Used by our LotteryPrinter class.
     *
     * BTW, the "heavy" variant of the line drawing characters doesn't work very well (seems
     * impossible to align, even with a monospaced font such as "Consolas").
     */
    class LotteryBox
    {
        /* Example of two connected boxes:
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
            get {
                  return boxHeight + 2;
                }
        }

        public int realWidth{   // the REAL width of the whole box
                                // maybe useful for whoever creates LotteryBox instances (eg. LotteryPrinter)...
            get { 
                  int acc = boxWidth + 2;
                  if (this.typeLeft == BoxTypeLeft.connected)
                      acc += 3; // 3 extra characters are needed for the "├──┤" part...
                  if (this.typeRight != BoxTypeRight.unconnected)
                      acc += 5; // 5 extra characters are needed for the "├──┐├──" part...
                  return acc;
                }
        }

        //private readonly char spaceChar = '□';
        private readonly char spaceChar = ' ';


        /** Class methods: **/

        public LotteryBox(string athlete, BoxTypeLeft typeLeft, BoxTypeRight typeRight, int maxNameLength = defaultWidth)       // overloaded constructor, currently unused
           : this(new List<string>() { athlete }, typeLeft, typeRight, maxNameLength) { }


        public LotteryBox(List<string> team, BoxTypeLeft typeLeft, BoxTypeRight typeRight, int maxNameLength = defaultWidth)    // overloaded constructor
        {
            this.typeLeft  = typeLeft;
            this.typeRight = typeRight;
            this.boxHeight = team.Count;
            this.boxWidth  = maxNameLength;

            //if (team.Count == 1 && String.IsNullOrEmpty(team.ElementAt(0)) && this.typeLeft == BoxTypeLeft.unconnected)
            if (String.IsNullOrEmpty(team.ElementAt(0)) && this.typeLeft == BoxTypeLeft.unconnected)
                this.box = makeEmptyBox();
            else
                this.box = makeBox(team);
        }


        private char[][] makeBox(List<string> team)
        {
            char[][] tmpBox = new char[this.realHeight][];
            StringBuilder sb = new StringBuilder(); // "sb" gets .Clear()ed after building every line of the tmpBox...

            if (typeLeft == BoxTypeLeft.connected) sb.Append("   ");
            sb.Append('┌').Append('─', this.boxWidth).Append('┐');
            if (typeRight == BoxTypeRight.connected_up) sb.Append("  │");
            tmpBox[0] = sb.ToString().ToCharArray();
            sb.Clear();

            int line = 1;                           // "line" and "mid" are used in the foreach loop
            int mid = (team.Count + 1) / 2;

            foreach (var athlete in team) {

                if (typeLeft == BoxTypeLeft.unconnected)
                    sb.Append('│');
                else {      // BoxTypeLeft is connected
                    if (line != mid)
                        sb.Append("   │");
                    else    // (line == mid)
                        sb.Append("├──┤");
                }

                sb.Append(Formatted(athlete));

                if (typeRight == BoxTypeRight.unconnected)
                    sb.Append('│');
                else {
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


            Debug.Assert(line == realHeight - 1);
            if (typeLeft == BoxTypeLeft.connected) sb.Append("   ");
            sb.Append('└').Append('─', this.boxWidth).Append('┘');
            if (typeRight == BoxTypeRight.connected_down) sb.Append("  │");
            tmpBox[realHeight - 1] = sb.ToString().ToCharArray();
            //sb.Clear();

            //foreach(var ln in tmpBox)
            //    Debug.Assert(ln.Length <= realWidth + 1); // ?!

            return tmpBox;
        }


        private char[][] makeEmptyBox() {
            char[][] tmpBox = new char[this.realHeight][];
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < realHeight; ++i)
            {
                sb.Append(spaceChar, this.realWidth - 2);
                tmpBox[i] = sb.ToString().ToCharArray();
                sb.Clear();
            }

            return tmpBox;
        }


        private StringBuilder Formatted(string athlete)
        {
            /* int len = Math.Min(athlete.Length, boxWidth);
             * string athName = athlete.Substring(0, len);
             * 
             * Cool trick, but I prefer the alternative implementation below this comment:
             */

            StringBuilder sb = new StringBuilder();

            if (athlete.Length > boxWidth) {

                sb.Append(athlete.Substring(0, boxWidth - 1)).Append('…');

            } else { // boxWidth >= athlete.Length

                sb.Append(' ', (boxWidth - athlete.Length) / 2);

                sb.Append(athlete);

                sb.Append(' ', (boxWidth - athlete.Length + 1) / 2);

            }

            Debug.Assert(sb.Length == boxWidth);

            return sb;
        }


        public char[][] get()       // returns the constructed LotteryBox
        {
            { //Debugging block
                StringBuilder sb = new StringBuilder();
                foreach (char[] line in box)
                    sb.Append(line).Append('\n');

                Debug.WriteLine("This LotteryBox object has the following contents (use a mono font!):\n"
                    + sb.ToString());
            }

            return box;
        }

    }
}
