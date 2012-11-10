using System; // also has System.Random, but that's low-quality randomness
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KarateGeek.databaseConnection;
using System.Diagnostics;   // has Debug.WriteLine()

/* high quality pseudo-random number generator, suitable for cryptographic purposes. Also see
 * http://msdn.microsoft.com/en-us/library/system.security.cryptography.rngcryptoserviceprovider(v=vs.100).aspx
 */
//using System.Security.Cryptography.RNGCryptoServiceProvider;
//using System.Windows.Forms.DataVisualization.Charting.Chart;


namespace KarateGeek.helpers
{
    /** 
     * Algorithm for the Lottery Generator described in project specs.
     * Simplified description (algorithm can be improved):
     * 
     * - Get list of tournament participants, in the form of List<long>,
     *   where the "long" is their index in the DB table "Athletes".
     * - Map that List to a List of tuples (athleteScoreList) which gets
     *   constructed using DB data. This applies a function to all list
     *   members which calculates their "score" (see project specs
     *   for the Factors taken into account)
     * - Randomise that score a little bit (configurable!)
     * - Sort the List (athleteScoreList) by score and return a new
     *   List<int>. That List will be used by the reporting tools.
     *
     * - The class LotteryGenerator will have a constructor that takes
     *   a tournament id and three publicly accessible methods:
     *   shuffle(), which produces a new randomisation, getLottery(),
     *   which returns it, and confirmLottery(), which writes to the DB.
     * - It must take into account team sports.
     * 
     * 
     * USING A (SIMPLIFIED) FACTORY-LIKE DESIGN PATTERN: LotteryGenerator
     * is an abstract base class providing the aforementioned three methods
     * and is extended by LotteryGen_Ind_Single, LotteryGen_Ind_Versus,
     * LotteryGen_Team_Single and LotteryGen_Team_Versus;
     * LotteryGeneratorFactory mainly has a "factory method" that chooses
     * which one of the above should be instatiated, based on DB data.
     * 
     * LotteryGenerator cannot be instatiated, but has a useful constructor
     * inherited by the others.
     * 
     * 
     * CURRENTLY ONLY IMPLEMENTED FOR "VERSUS"-TYPE TOURNAMENTS!
     */


    /* base class, abstract: */
    abstract class LotteryGenerator //TODO: Move some code up here.
    {

        /** Class fields/properties: **/

        public int randomisationFactor { get; set; }


        /** Class methods: **/

        public abstract void shuffle(int tries = 2);
        public abstract List<long> getLottery();
        public abstract void confirmLottery();

        /* Methods to display a chart of the randomised athlete list: */
        protected class Visualisation // maybe needed only for debugging/tweaking? Too hard to do anyway,
        {                           // because it needs optional .NET components! For now, leave it as a stub.

        }
    }



    /* Factory class: */
    class LotteryGeneratorFactory
    {
        public enum LotteryType { // enum not really needed anymore, replaced by class structure
            indiv_single,
            indiv_versus,
            team_single,
            team_versus
        };

        public static LotteryGenerator Create(int tournamentId)
        {
            LotteryGenerator lg;

            /* assignment of lottery type: the cases of the following switch are unfinished (and WRONG): */
            //switch (new LotteryGenConnection().getTournamentGameType(tournamentId)) {
            //    case Strings.indKata:    lg = new LotteryGen_Ind_Single(tournamentId);
            //                             break;

            //    case Strings.indKumite:
            //    case Strings.individual: lg = new LotteryGen_Ind_Versus(tournamentId);
            //                             break;

            //    case Strings.syncKata:
            //    case Strings.teamKata:   lg = new LotteryGen_Team_Single(tournamentId);
            //                             break;

            //    case Strings.teamKumite:
            //    case Strings.team:       lg = new LotteryGen_Team_Versus(tournamentId);
            //                             break;

            //    default:                 lg = null;
            //    /* for indKata and ?? we should also check the scoring system! */
            //}

            // for now only create LotteryGen_Ind_Versus objects...

            lg = new LotteryGen_Ind_Versus(tournamentId);

            Debug.Assert(lg != null);

            return lg;
        }

    }



    /* Implementation classes LotteryGen_Ind_Single, LotteryGen_Ind_Versus,
     * LotteryGen_Team_Single and LotteryGen_Team_Versus start here:           */


    class LotteryGen_Ind_Single : LotteryGenerator
    {
        public override void shuffle(int tries){}
        public override List<long> getLottery() { return null; }
        public override void confirmLottery() { }
    }



    class LotteryGen_Ind_Versus : LotteryGenerator
    {

        /** Class fields/properties: **/

        protected readonly List<long> athleteList; //TODO: private -> protected for all fields ??

        protected readonly List<Tuple<long, int>> athleteScoreList;

        private List<Tuple<long, int>> athleteScoreListShuffled;

        protected bool confirmed = false; // once "confirmed", a LotteryGen_Ind_Versus object cannot write to the DB anymore!

        protected Int32 randomSeed = 134368;  // use a constant value with "new Random(randomSeed)",
                                              // or "new Random()" for a time-dependent value... (UNUSED, for now)
        protected Random rgen;

        // public int randomisationFactor { get; set; }

        public readonly int tournamentId;


        /** Class methods: **/

        public LotteryGen_Ind_Versus(int tournamentId) // constructor
        {
            /* NOTE: This will throw an exception if the list is empty. This must be caught by the GUI code! */
            athleteList = new LotteryGenConnection().tournamentParticipants(tournamentId);

            List<Tuple<long, int>> tmp = new List<Tuple<long,int>>();

            foreach (long athlete in athleteList)
                tmp.Add(new Tuple<long, int>(athlete, getAthleteScore(athlete)));

            athleteScoreList = tmp;

            /* NOTE: If we could "capture" REAL system randomness (like /dev/random on Linux)
             * it would be much, much better than this: */
            rgen = new Random(); // initialise pseudo-random number-generator with a time-dependent value.

            this.randomisationFactor = 650;
            this.tournamentId = tournamentId;
        }


        /* NOTE: Lots of hard-coded values here, but at least all of them are in the same place
         * and easy to edit (also set the "randomisationFactor" in the class constructor!)      */
        protected int getAthleteScore(long athleteId)
        {
            LotteryGenConnection conn = new LotteryGenConnection();

            const int beltFactor = 125;
            const int ageFactor  = 100;
            
            /* past achievements: */
            int score = conn.getNumOfGoodPlacements(athleteId, 1, true)  * 500 +  // first  place in   official event
                        conn.getNumOfGoodPlacements(athleteId, 2, true)  * 240 +  // second place in   official event
                        conn.getNumOfGoodPlacements(athleteId, 3, true)  * 120 +  // third  place in   official event
                        conn.getNumOfGoodPlacements(athleteId, 4, true)  *  80 +  // fourth place in   official event
                        conn.getNumOfGoodPlacements(athleteId, 1, false) * 250 +  // first  place in unofficial event
                        conn.getNumOfGoodPlacements(athleteId, 2, false) * 120 +  // second place in unofficial event
                        conn.getNumOfGoodPlacements(athleteId, 3, false) *  60 +  // third  place in unofficial event
                        conn.getNumOfGoodPlacements(athleteId, 4, false) *  40;   // fourth place in unofficial event

            /* belt color: */
            String belt = conn.getBeltColor(athleteId);
            for (int i = 0; i < Strings.rank.Length; ++i)
                if (Strings.rank[i] == belt)
                    score += i * beltFactor;

            //String belt = conn.getBeltColor(athleteId);  /* Both string comparison methods tested to work! */
            //for (int i = 0; i < Strings.rank.Length; ++i)
            //    if (Strings.rank[i].Equals(belt, StringComparison.Ordinal))
            //        score += i * beltFactor;

            /* age, only for children (<18): */
            int age = conn.getAge(athleteId);
            for (int i = 0; i < age && i < 18; ++i)
                score += ageFactor;


            return score;
        }


        /* produces a new randomization [It would be less ugly with more LINQ usage!] */
        public override void shuffle(int tries = 2) // a default value of 2 seems OK - we don't want infinite recursion!
        {
            List<Tuple<long, int>> L = new List<Tuple<long, int>>();

            foreach (var tuple in athleteScoreList) //TODO: Tweak randomisation factor
            {
                // L.Add(new Tuple<long, int>(tuple.Item1, tuple.Item2 + rgen.Next(0, randomisationFactor)));
                L.Add(new Tuple<long, int>(tuple.Item1, tuple.Item2
                    + rgen.Next(0, randomisationFactor * ( 1 + tuple.Item2 / 1000))));
                Debug.WriteLine("List item: " + tuple.Item1 + " with initial score: " + tuple.Item2);
            }

            athleteScoreListShuffled = L;

            /* EXPERIMENTAL and very computationally expensive way to check whether an athlete pair belongs
             * to the same club... Some refactoring would reduce the redundancy, but it should work as-is: */
            if ( tries > 0 && pairsClubConstraintActive(getPairs(this.getLottery())) ){
                Debug.WriteLine("\n ** AUTO-RESHUFFLING because same-club collisions were found... ** \n");
                shuffle(tries - 1);
            }
        }


        public override List<long> getLottery()
        {
            if (athleteScoreListShuffled == null) throw new Exception("Use method shuffle() first.");
            
            List<long> L = new List<long>();

            /* OrderByDescending() instead of OrderBy(), because we want the highest-ranked athletes first: */
            foreach (var tuple in athleteScoreListShuffled.OrderByDescending(x => x.Item2)){
                L.Add(tuple.Item1);
                Debug.WriteLine("List item: " + tuple.Item1 + " with randomised score: " + tuple.Item2);
            }

            return L;
        }


        protected bool pairsClubConstraintActive(List<Tuple<long, long, int, int>> Pairs)
        {
            LotteryGenConnection conn = new LotteryGenConnection();

            foreach (var pair in Pairs)
                if (conn.sameClubAthletePair(pair))
                    return true;

            return false;
        }


        /* CONVENTION: For semi-complete pairs, we provide a negative athlete id
         * to the writeTournamentPair() method of the LotteryGenConnection class.
         *
         * Specifically, we use -1 for !is_ready pairs and -2 for is_ready "pairs".
         * The latter are just single athlete participations (reusing the same code). */
        private List<Tuple<long, long, int, int>> getPairs(List<long> Participants)
        {
            List<Tuple<long, long, int, int>> Pairs = new List<Tuple<long, long, int, int>>();

            /* Code that constructs athlete pairs goes here... */
            /**
             * ALGORITHM (please confirm correctness, and cross-check with projec specs):
             * 
             * - copy sorted list to array (easier than using C#-style lists, I think)
             * - get array length (let's say "len")
             *   [ Actually we use LINQ and, instead of using System.ArraySegment<T> on
             *     Participants.ToArray() (and then Array.Copy()), we split the list
             *     into 2 arrays of lengths "x" and "y"... see below for the calculation
             *     of x and y.
             *   ]
             * 
             * - if "len" is a power of 2, OK; else, if "p" is the next-largest power of 2,
             *   "y" the number of athletes who auto-advance to the second round, "x" the
             *   number of athletes who do not and "z" the number of pairs in the first
             *   round (so that z == 2*x), solve the equations:
             * 
             * 
             *     2*y + (len - y) = p    (1)
             * 
             *     z = p/2 - y            (2)
             * 
             * 
             *   (1)  =>  y = p - len
             *   (2)  =>  2*z = p - 2*y  =>¹  z = (len - y)/2 = x/2  !
             * 
             * 
             * - first y athletes fill-in the "wings" of the lottery tree (going to phase 2
             *   directly) and the rest get paired in the first round (aka phase). Example
             *   for 11 athletes:
             * 
             *    y*2 + (11-y) = 16  =>  y=5 and z=8-5=3 pairs in the 1st phase
             * 
             * 
             *         /           \                   /           \
             *        /             \                 /             \
             *       •               •               •               •           ← phaseY
             *      / \             / \             / \             / \
             *     /   \           /   \           /   \           /   \
             *    /     \         /     \         /     \         /     \
             *   y1      y3      y5      •       •       •       y4      y2      ← phaseX
             *                          / \     / \     / \
             *                         /   \   /   \   /   \
             *                        x1   x3 x5   x6 x4   x2
             * 
             *  ├───── yleft ─────┤├─── z ────┤├─── z ────┤├── yright ──┤
             * 
             * 
             *  [number of y-type athletes "above" x1:  yleft = (y+1) / 2
             *   so x1 and x3 meet at the "4th position" of phase 1       ]
             * 
             * 
             *  ** There might be a smarter way to do it! (using Lists?!) **
             */

            /* Crude and untested first version: */

            int len = Participants.Count;
            int numOfPhases = (int) Math.Ceiling(Math.Log(len, 2));
            int p = (int) Math.Pow(2, numOfPhases);             // p has been tested, works well for len>=1
            int y = p - len;
            int x = len - y;
            int z = p / 2 - y; // len must be >=2 (?)

            int yleft  = (y + 1) / 2;  // What happens when y==3?! (y==5 is OK!) EDIT: I think the bug is fixed
            int yright = y - yleft;

            long[] Yarray = Participants.Take(y).ToArray();
            long[] Xarray = Participants.Skip(y).ToArray();

            /* Important assertions: */
            Debug.Assert(x == 2 * z);           // "x" is always divisible by 2!
            Debug.Assert(Xarray.Length == x);

            /**/

            /* The following parts might need more testing to eliminate off-by-one errors: */

            long[] Yleft  = new long[yleft];
            long[] Yright = new long[yright];
            long[] Xleft  = new long[z];
            long[] Xright = new long[z];

            for (int i = 0; i < x; i += 2) {
                Xleft[i / 2] = Xarray[i];
                Xright[z - (i / 2) - 1] = Xarray[i + 1]; // reverse order needed
            }

            for (int i = 0; i < yleft; ++i)
                Yleft[i] = Yarray[2 * i];

            for (int i = 0; i < yright; ++i)
                Yright[yright - i - 1] = Yarray[(2 * i) + 1]; // reverse order needed


            /* Now let's add the pairs to the list of pairs: */
            {
                /* "position" is 1-based; "phase" is 0-based and 0 is the final (1 is the semi-final etc.) */
                int phaseX = numOfPhases - 1;
                int phaseY = numOfPhases - 2;
                int phaseXposition = yleft + 1;
                int phaseYposition = 1;

                for (int i = 0; i < yleft; i += 2) {
                    Pairs.Add(new Tuple<long, long, int, int>(Yleft[i], Yleft[i + 1], phaseY, phaseYposition));
                    ++phaseYposition;
                }

                if (yleft % 2 != 0) {
                    Pairs.Add(new Tuple<long, long, int, int>(Yleft[yleft - 1], -1, phaseY, phaseYposition));
                    ++phaseYposition;
                }

                phaseYposition += z / 2; // ?!?! I think this line is OK.

                if (yright % 2 != 0) {
                    Pairs.Add(new Tuple<long, long, int, int>(-1, Yright[0], phaseY, phaseYposition));
                    ++phaseYposition;
                }

                for (int i = (yright % 2 != 0) ? 1 : 0; i < yright; i += 2) { // FIXME: check boundary conditions, especially here!
                    Pairs.Add(new Tuple<long, long, int, int>(Yright[i], Yright[i + 1], phaseY, phaseYposition));
                    ++phaseYposition;
                }

                for (int i = 0; i < z - 1; i += 2) {
                    Pairs.Add(new Tuple<long, long, int, int>(Xleft[i], Xleft[i + 1], phaseX, phaseXposition));
                    ++phaseXposition;
                }

                if (z % 2 != 0) {
                    Pairs.Add(new Tuple<long, long, int, int>(Xleft[z - 1], Xright[0], phaseX, phaseXposition));
                    ++phaseXposition;
                }

                for (int i = (z % 2 != 0) ? 1 : 0; i <  z - 1; i += 2) { // FIXME: check boundary conditions, especially here!
                    Pairs.Add(new Tuple<long, long, int, int>(Xright[i], Xright[i + 1], phaseX, phaseXposition));
                    ++phaseXposition;
                }
            }

            /**/

            { //Debug info, will be removed in the final version (auto-disabled in "release" builds anyway):
                Debug.WriteLine("Participants: " + Participants);
                foreach (var i in Participants)
                    Debug.WriteLine(i);

                Debug.WriteLine("Yarray: " + Yarray);
                foreach (var i in Yarray)
                    Debug.WriteLine(i);

                Debug.WriteLine("Xarray: " + Xarray);
                foreach (var i in Xarray)
                    Debug.WriteLine(i);

                Debug.WriteLine("Yleft array: " + Yleft);
                foreach (var i in Yleft)
                    Debug.WriteLine(i);

                Debug.WriteLine("Yright array: " + Yright);
                foreach (var i in Yright)
                    Debug.WriteLine(i);

                Debug.WriteLine("Xleft array: " + Xleft);
                foreach (var i in Xleft)
                    Debug.WriteLine(i);

                Debug.WriteLine("Xright array: " + Xright);
                foreach (var i in Xright)
                    Debug.WriteLine(i);

                Debug.WriteLine("Pairs and positions: " + Pairs);
                foreach (var i in Pairs)
                    Debug.WriteLine("athl.1:{0,4}  athl.2:{1,4}  phase:{2,4}  position:{3,4}",
                        i.Item1, i.Item2, i.Item3, i.Item4);
            }

            return Pairs;
        }


        public override void confirmLottery() // writes current lottery to the database, atomically!
        {

            if (this.confirmed)
                throw new Exception("Once \"confirmed\", a LotteryGenerator object cannot write to the database anymore.");

            LotteryGenConnection conn = new LotteryGenConnection();
            List<long> L = this.getLottery();
            List<Tuple<long, long, int, int>> Pairs = getPairs(L);

            /* the doCommit flag (named parameter) will be changed to "true" after some testing */
            this.confirmed = conn.writeAllTournamentPairs(Pairs, tournamentId, doCommit: false);
        }
    }



    class LotteryGen_Team_Single : LotteryGenerator
    {
        public override void shuffle(int tries) { }
        public override List<long> getLottery() { return null; }
        public override void confirmLottery() { }
    }



    class LotteryGen_Team_Versus : LotteryGenerator
    {
        public override void shuffle(int tries) { }
        public override List<long> getLottery() { return null; }
        public override void confirmLottery() { }
    }

}
