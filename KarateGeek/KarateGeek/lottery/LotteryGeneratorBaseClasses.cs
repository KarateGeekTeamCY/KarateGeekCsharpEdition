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


namespace KarateGeek.lottery
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
     * - The class LotteryGenerator has a constructor that takes a
     *   tournament id and three publicly accessible methods:
     *   shuffle(), which produces a new randomisation, getLottery(),
     *   which returns it, and confirmLottery(), which writes to the DB.
     *   (A fourth public method might be added, printLottery(), using
     *    getLottery() to correctly initialise the LotteryPrinter class and
     *    pretty-print the lottery to the screen...)
     * - It must take into account team sports.
     * 
     * 
     * USING A (SIMPLIFIED) FACTORY-LIKE DESIGN PATTERN: LotteryGenerator
     * is an abstract base class providing the aforementioned three methods
     * and is extended by LotteryGen_Expo_Ind, LotteryGen_Versus_Ind,
     * LotteryGen_Expo_Sync, LotteryGen_Expo_Team and LotteryGen_Versus_Team;
     * LotteryGeneratorFactory mainly has a "factory method" that chooses
     * which one of the above should be instatiated, based on DB data.
     * 
     * LotteryGenerator cannot be instatiated, but has a useful constructor
     * inherited by the others. Also, there are 2 more abstract classes
     * between LotteryGenerator and the aforementioned 5, LotteryGen_Versus
     * and LotteryGen_Expo. They provide different getPairs() implem/tions.
     */

    #region abstract base class LotteryGenerator
    abstract class LotteryGenerator //TODO: Move more code up here.
    {

        /** Class fields/properties: **/

        protected readonly List<long> athleteList;

        protected readonly List<Tuple<long, int>> athleteScoreList; // an athlete scoreList

        protected abstract List<Tuple<long, int>> scoreList { get; } // an athlete/team scoreList (property)

        protected List<Tuple<long, int>> scoreListShuffled; // again, might be an athlete or a team scoreList!

        protected bool confirmed;   // once "confirmed", a LotteryGen_Versus_Ind object cannot write to the DB anymore!

        protected Int32 randomSeed = 134368;  // use a constant value with "new Random(randomSeed)",
                                              // or "new Random()" for a time-dependent value... (UNUSED, for now)
        protected Random rgen;

        public readonly int tournamentId;

        public int randomisationFactor { get; set; }


        /** Class methods: **/

        //public abstract void shuffle(int tries = 2);
        //public abstract List<long> getLottery();


        protected LotteryGenerator(int tournamentId) // constructor of the abstract class
        {
            this.tournamentId = tournamentId;
            this.confirmed = new LotteryGenConnection().getTournamentLotteryStateReady(tournamentId);

            /* NOTE: This will throw an exception if the list is empty. This must be caught by the GUI code! */
            this.athleteList = new LotteryGenConnection().tournamentParticipatingAthletes(tournamentId);

            List<Tuple<long, int>> tmp = new List<Tuple<long,int>>();

            foreach (long athlete in athleteList) // this.tournamentId must be already set in order to get the scores!
                tmp.Add(new Tuple<long, int>(athlete, getAthleteScore(athlete)));

            this.athleteScoreList = tmp;

            /* NOTE: If we could "capture" REAL system randomness (like /dev/random on Linux)
             * it would be much, much better than this: */
            rgen = new Random(); // initialise pseudo-random number-generator with a time-dependent value.

            this.randomisationFactor = 750;
        }


        /* NOTE: Lots of hard-coded values here, but at least all of them are in the
         * same place and easy to edit (also set the "randomisationFactor" in the class
         * constructor or the subclass constructors!)                                   */
        protected int getAthleteScore(long athleteId)
        {
            LotteryGenConnection conn = new LotteryGenConnection();

            String generalGameTypeSQL; // will be used with the PostgreSQL "SIMILAR TO" feature, so it must be formatted accordingly

            int[] rankingWeight = {0, 500, 240, 120, 80, 40, 40, 40, 40}; // index 0 is unused
            const double unofficialMultiplier = 0.5;
            const double otherTypeMultiplier  = 0.4;
            const int beltFactor = 125;
            const int ageFactor  = 100;

            int scoreAccu = 0;  // score accumulator

            /* initialisation: */
            switch (conn.getTournamentGameType(this.tournamentId)) // we could use .Split('|')[1] and be done with it, but we would lose the special handling of fugu-go
            {
                /* kata skills needed: */
                case Strings.indKata:
                case Strings.teamKata:
                case Strings.syncKata:
                case Strings.enbu:       generalGameTypeSQL = "'%(" + Strings.suffixKata   + "|"
                                                                    + Strings.suffixEnbu   + "|"
                                                                    + Strings.suffixFugugo +   ")'";
                                         break;

                /* kumite skills needed: */
                case Strings.indKumite:
                case Strings.teamKumite: generalGameTypeSQL = "'%(" + Strings.suffixKumite + "|"
                                                                    + Strings.suffixFugugo +   ")'";
                                         break;

                /* fugu-go needs both kata and kumite skills: */
                case Strings.fugugo:     generalGameTypeSQL = "'%(" + Strings.suffixKata   + "|"
                                                                    + Strings.suffixEnbu   + "|"
                                                                    + Strings.suffixKumite + "|"
                                                                    + Strings.suffixFugugo +   ")'";
                                         break;

                /* default case (we assert it won't happen): */
                default:                 generalGameTypeSQL = null;
                                         break;
            }
            Debug.Assert(generalGameTypeSQL != null);
            Debug.Assert(unofficialMultiplier < 1);
            Debug.Assert(otherTypeMultiplier < 1);

          //  /* past achievements: */
          //  scoreAccu = conn.getNumOfGoodPlacements(athleteId, 1, true, generalGameTypesSQL) * 500 +  // first  place in   official event of the same type
          //              conn.getNumOfGoodPlacements(athleteId, 2, true, generalGameTypesSQL) * 240 +  // second place in   official event of the same type
          //              conn.getNumOfGoodPlacements(athleteId, 3, true, generalGameTypesSQL) * 120 +  // third  place in   official event of the same type
          //              conn.getNumOfGoodPlacements(athleteId, 4, true, generalGameTypesSQL) *  80    // fourth place in   official event of the same type

          ///* x 1/2 */ + conn.getNumOfGoodPlacements(athleteId, 1, false, generalGameTypesSQL) * 250 +  // first  place in unofficial event of the same type
          //              conn.getNumOfGoodPlacements(athleteId, 2, false, generalGameTypesSQL) * 120 +  // second place in unofficial event of the same type
          //              conn.getNumOfGoodPlacements(athleteId, 3, false, generalGameTypesSQL) *  60 +  // third  place in unofficial event of the same type
          //              conn.getNumOfGoodPlacements(athleteId, 4, false, generalGameTypesSQL) *  40    // fourth place in unofficial event of the same type

          ///* x 2/5 */ + conn.getNumOfGoodPlacements(athleteId, 1, true)  * 200 +  // first  place in   official event (any)
          //              conn.getNumOfGoodPlacements(athleteId, 2, true)  *  96 +  // second place in   official event (any)
          //              conn.getNumOfGoodPlacements(athleteId, 3, true)  *  48 +  // third  place in   official event (any)
          //              conn.getNumOfGoodPlacements(athleteId, 4, true)  *  32    // fourth place in   official event (any)

          ///* x 1/5 */ + conn.getNumOfGoodPlacements(athleteId, 1, false) * 100 +  // first  place in unofficial event (any)
          //              conn.getNumOfGoodPlacements(athleteId, 2, false) *  48 +  // second place in unofficial event (any)
          //              conn.getNumOfGoodPlacements(athleteId, 3, false) *  24 +  // third  place in unofficial event (any)
          //              conn.getNumOfGoodPlacements(athleteId, 4, false) *  16    // fourth place in unofficial event (any)
          //            ;

            /* past achievements: */
            for (int pos = 1; pos <= 4; ++pos)
                scoreAccu += (int) ( // 16 SQL queries for each athlete, perhaps a candidate for optimisation (stored procedures?)
                                       conn.getNumOfGoodPlacements(athleteId, pos, generalGameTypeSQL, official: true,  sameType: true)  * rankingWeight[pos]
                                     + conn.getNumOfGoodPlacements(athleteId, pos, generalGameTypeSQL, official: false, sameType: true)  * rankingWeight[pos] * unofficialMultiplier
                                     + conn.getNumOfGoodPlacements(athleteId, pos, generalGameTypeSQL, official: true,  sameType: false) * rankingWeight[pos] * otherTypeMultiplier
                                     + conn.getNumOfGoodPlacements(athleteId, pos, generalGameTypeSQL, official: false, sameType: false) * rankingWeight[pos] * otherTypeMultiplier * unofficialMultiplier
                                   );


            /* belt color: */
            String belt = conn.getBeltColor(athleteId);
            for (int i = 0; i < Strings.rank.Length; ++i)
                if (Strings.rank[i].Equals(belt, StringComparison.Ordinal))
                    scoreAccu += i * beltFactor;

            /* age, only for children (<18): */
            int age = conn.getAge(athleteId);
            for (int i = 0; i < age && i < 18; ++i)
                scoreAccu += ageFactor;

            Debug.WriteLine("Score of athlete{0,6} is{1,6}:", athleteId , scoreAccu);


            return scoreAccu;
        }


        /* Produces a new randomization [It would be less ugly with more LINQ usage!] */
        /* Declared as "virtual" means that it can be overridden by child classes. */
        public virtual void shuffle(int tries = 2) // a default value of 2 seems OK - we don't want infinite recursion!
        {
            List<Tuple<long, int>> L = new List<Tuple<long, int>>();

            Debug.Assert(scoreList != null);

            foreach (var tuple in scoreList) //TODO: Tweak randomisation factor
            {
                // L.Add(new Tuple<long, int>(tuple.Item1, tuple.Item2 + rgen.Next(0, randomisationFactor)));
                L.Add(new Tuple<long, int>(tuple.Item1, tuple.Item2
                    + rgen.Next(0, randomisationFactor * ( 1 + tuple.Item2 / 1000))));
                Debug.WriteLine("List item: " + tuple.Item1 + " with initial score: " + tuple.Item2);
            }

            scoreListShuffled = L;

            /* EXPERIMENTAL and computationally expensive way to check whether an athlete pair belongs to the
             * same club... Some refactoring would reduce the redundancy, but it should work as-is (and it does): */
            if ( tries > 0 && pairsClubConstraintActive(getPairs(this.getLottery())) ){
                Debug.WriteLine("\n ** AUTO-RESHUFFLING because same-club collisions were found... ** \n");
                shuffle(tries - 1);
            }
        }


        private bool pairsClubConstraintActive(List<Tuple<long, long, int, int>> Pairs)
        {
            LotteryGenConnection conn = new LotteryGenConnection();

            foreach (var pair in Pairs)
                if (conn.sameClubAthletePair(pair))
                    return true;

            return false;
        }


        public virtual List<long> getLottery()
        {
            if (scoreListShuffled == null) throw new Exception("Use method shuffle() first.");
            
            List<long> L = new List<long>();

            /* OrderByDescending() instead of OrderBy(), because we want the highest-ranked athletes first: */
            foreach (var tuple in scoreListShuffled.OrderByDescending(x => x.Item2)){
                L.Add(tuple.Item1);
                Debug.WriteLine("List item: " + tuple.Item1 + " with randomised score: " + tuple.Item2);
            }

            return L;
        }


        protected abstract List<Tuple<long, long, int, int>> getPairs(List<long> Participants);


        protected abstract List<Tuple<long, long, int, int>> getEmptyPairs(int numOfParticipants); // ALWAYS RETURNS GAMES!! Participants might be teams.


        /* The following method is designed (?) to be overriden, if needed, by child classes (team cases?).
         * I should probably change the structure used by getPairsToCommit(), writeAllTournamentPairs() etc.
         * to something like List<Tuple<List<long>, int, int>>, though, because not all cases are "pairs"...
         */
        protected List<Tuple<long, long, int, int>> getPairsToCommit(List<long> Lott)
        {
            return getPairs(Lott).Concat(getEmptyPairs(Lott.Count)).ToList();
        }


        public virtual void confirmLottery(bool doCommit = false) // writes current lottery to the database, atomically!
        {
            if (this.confirmed)
                throw new Exception("Once \"confirmed\", a LotteryGenerator object cannot write to the database anymore.");

            LotteryGenConnection conn = new LotteryGenConnection();
            List<long> L = this.getLottery();
            List<Tuple<long, long, int, int>> pairsToCommit = getPairsToCommit(L);

            { // Debug info
                Debug.WriteLine("Ready to commit the following pairs (including empty ones) to the DB: " + pairsToCommit);
                foreach (var i in pairsToCommit)
                    Debug.WriteLine("athl.1:{0,4}  athl.2:{1,4}  phase:{2,4}  position:{3,4}",
                        i.Item1, i.Item2, i.Item3, i.Item4);
            }

            /**/

            /* Converting pairs to sets... */

            /* CONVENTION: For semi-complete pairs, the caller provides a negative athlete id.
             *
             * Specifically, we use -1 for !is_ready pairs and -2 for is_ready "pairs".
             * The latter are just single athlete participations, or single team
             * participations (reusing the same code for the DB transaction).                 */

            List<Tuple<List<long>, bool, int, int>> setsToCommit = new List<Tuple<List<long>, bool, int, int>>(); ;
            bool isReady;
            long id1, id2;

            foreach (var pair in pairsToCommit) {

                id1 = pair.Item1;
                id2 = pair.Item2;
                isReady = (id1 >= 0 && id2 >= 0) || (id1 >= 0 && id2 == -2);

                List<long> set = new List<long>();
                if (id1 >= 0) set.Add(id1);
                if (id2 >= 0) set.Add(id2);

                setsToCommit.Add(new Tuple<List<long>, bool, int, int>(set, isReady, pair.Item3, pair.Item4));
            }

            /**/

            //this.confirmed = conn.writeAllTournamentPairs(PairsToCommit, tournamentId, doCommit: doCommit);
            this.confirmed = conn.writeAllTournamentGameSets(setsToCommit, tournamentId, doCommit: doCommit);
            conn.setTournamentLotteryStateReady(tournamentId, this.confirmed);
            // we could make "confirmed" a property, so that the setter does this automatically :)
        }

        #region helper nested classes
        ///* Methods to display a chart of the randomised athlete list: */
        //protected class Visualisation // maybe needed only for debugging/tweaking? Too hard to do anyway,
        //{                           // because it needs optional .NET components! For now, leave it as a stub.

        //}

        /* Helper methods to handle teams and team lists (class to be renamed?): */
        protected sealed class TeamHelper // Is this even a good idea? (C# lacks multiple inheritance)
        {

            /** Class fields/properties: **/

            private readonly List<Tuple<long, int>> athleteScoreList;
            private readonly long tournamentId;

            /** Class methods: **/

            public TeamHelper(long tournamentId, List<Tuple<long, int>> athleteScoreList) // constructor of the abstract class
            {
                this.tournamentId = tournamentId;
                this.athleteScoreList = athleteScoreList;
            }

            //public List<Tuple<long, int>> getTeamScoreList(List<Tuple<long, int>> athleteScoreList) /* was static! */
            public List<Tuple<long, int>> getTeamScoreList()
            {
                LotteryGenConnection conn = new LotteryGenConnection();
                /** pseudocode:
                 *
                 * for each athlete find his team;
                 *     if the team is not in the list
                 *         add its id and athlete's score as a new list node;
                 *     else
                 *         find the id and add the athlete's score (careful, tuples are immutable!)
                 *
                 * return the constructed list
                 */

                List<Tuple<long, int>> L = new List<Tuple<long, int>>();

                foreach (var ath in athleteScoreList) {
                    long teamId = conn.getTeamOfAthlete(tournamentId, ath.Item1);
                    Tuple<long, int> old;

                    try {
                        old = L.First(t => t.Item1 == teamId);
                    } catch (InvalidOperationException e) {
                        old = null;
                    }

                    if (old == null) // team not yet in the list
                        L.Add(new Tuple<long, int>(teamId, ath.Item2));
                    else {
                        L.Remove(old);
                        L.Add(new Tuple<long, int>(teamId, old.Item2 + ath.Item2));
                    }
                }

                return L;
            }


            public List<long> getAthletesOfTeam(long teamId)
            {
                return getAthletesOfTeam(teamId, this.tournamentId);
            }


            public static List<long> getAthletesOfTeam(long teamId, long tournamentId)
            {
                return new LotteryGenConnection().tournamentParticipatingAthletesOfTeam(tournamentId, teamId);
            }

        }
        #endregion
    }
    #endregion



    #region abstract base class LotteryGen_Expo : LotteryGenerator
    abstract class LotteryGen_Expo : LotteryGenerator
    {

        public LotteryGen_Expo(int tournamentId) : base(tournamentId) { } //calling base constructor


        protected override List<Tuple<long, long, int, int>> getPairs(List<long> Participants)
        {
            List<Tuple<long, long, int, int>> Pairs = new List<Tuple<long, long, int, int>>();

            /**
             * ALGORITHM: Simply use the given (shuffled) list of athletes!
             *            Phase is always 0.
             *
             *  WARNING:  This might be wrong according to the project specs. Some Exposition-type
             *            tournaments still have phases (usually 16 -> 8 -> 4 -> winner). In that
             *            case we can re-use the getPairs() implementation from LotteryGen_Versus,
             *            but the numOfPhases calculation needs to be corrected.
             *             Or we might declare them as Versus-type tournaments (still correcting
             *            the calculation of numOfPhases, decreasing by 1).
             *
             *  UPDATE:   Using the given (shuffled) list of athletes, but calculating phase
             *            similarly to LotteryGen_Versus and then subtracting 1:
             *            int numOfPhases = (int) Math.Ceiling(Math.Log(Participants.Count, 2)) - 1;
             *            int phase = numOfPhases - 1;
             */

            int phase = (int) Math.Ceiling(Math.Log(Participants.Count, 2)) - 2;
            int pos = 1;

            foreach (var id in this.getLottery()){
                Pairs.Add(new Tuple<long, long, int, int>(id, -2, phase, pos));
                ++pos;
            }

            return Pairs;
        }


        protected override List<Tuple<long, long, int, int>> getEmptyPairs(int numOfParticipants)
        {
            List<Tuple<long, long, int, int>> emptyPairs = new List<Tuple<long, long, int, int>>();

            int numOfPhases = (int) Math.Ceiling(Math.Log(numOfParticipants, 2));

            //for (int phase = numOfPhases - 2; phase >= 0; --phase)  // numOfPhases - 2 ?
            //    for (int position = 1; position <= Math.Pow(2, phase); ++position)
            //        emptyPairs.Add(new Tuple<long, long, int, int>(-1, -1, phase, position));

            //for (int phase = numOfPhases - 1; phase >= 0; --phase)
            //    for (int position = 1; position <= Math.Pow(2, phase + 2); ++position)
            //        emptyPairs.Add(new Tuple<long, long, int, int>(-1, -1, phase, position));

            for (int phase = numOfPhases - 3; phase >= 0; --phase)
                for (int position = 1; position <= Math.Pow(2, phase + 2); ++position)
                    emptyPairs.Add(new Tuple<long, long, int, int>(-1, -1, phase, position));

            return emptyPairs;
        }
    }
    #endregion



    #region abstract base class LotteryGen_Versus : LotteryGenerator
    abstract class LotteryGen_Versus : LotteryGenerator
    {

        public LotteryGen_Versus(int tournamentId) : base(tournamentId) { } //calling base constructor


        /* CONVENTION: For semi-complete pairs, we provide a negative athlete id
         * to the writeTournamentPair() method of the LotteryGenConnection class.
         *
         * Specifically, we use -1 for !is_ready pairs and -2 for is_ready "pairs".
         * The latter are just single athlete participations, or single team
         * participations (reusing the same code for the DB transaction). */
        protected override List<Tuple<long, long, int, int>> getPairs(List<long> Participants)
        {
            List<Tuple<long, long, int, int>> Pairs = new List<Tuple<long, long, int, int>>();

            /**
             * ALGORITHM (please confirm correctness, and cross-check with project specs):
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
             * - first y athletes fill-in the "wings" of the lottery tree (going to phase Y
             *   directly) and the rest get paired in the first round (aka phase X). Example
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

                for (int i = 0; i < yleft - 1; i += 2) {
                    Pairs.Add(new Tuple<long, long, int, int>(Yleft[i], Yleft[i + 1], phaseY, phaseYposition));
                    ++phaseYposition;
                }

                if (yleft % 2 != 0) {
                    Pairs.Add(new Tuple<long, long, int, int>(Yleft[yleft - 1], -1, phaseY, phaseYposition));
                    ++phaseYposition;
                }

                //phaseYposition += z / 2; // ?!?! I think this line is OK. EDIT: NO, IT IS NOT!
                phaseYposition += (z - (yleft % 2)) / 2;  // Check correctness!

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

            { // Debug info, might be removed in the final version (auto-disabled in "release" builds anyway):
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


        protected override List<Tuple<long, long, int, int>> getEmptyPairs(int numOfParticipants)
        {
            List<Tuple<long, long, int, int>> emptyPairs = new List<Tuple<long, long, int, int>>();

            int numOfPhases = (int)Math.Ceiling(Math.Log(numOfParticipants, 2));

            int p = (int)Math.Pow(2, numOfPhases);
            int y = p - numOfParticipants;
            int yleft = (y + 1) / 2;
            int yright = y - yleft;

            // FIXME: check boundary conditions, especially here! (according to a few tests, probably OK)
            for (int position = 1 + (yleft + 1) / 2; position <= Math.Pow(2, numOfPhases - 2) - (yright + 1) / 2; ++position)  // phase Y (missing games)
                emptyPairs.Add(new Tuple<long, long, int, int>(-1, -1, numOfPhases - 2, position));

            for (int phase = numOfPhases - 3; phase >= 0; --phase)  // phase (Y - 1) to phase 0
                for (int position = 1; position <= Math.Pow(2, phase); ++position)
                    emptyPairs.Add(new Tuple<long, long, int, int>(-1, -1, phase, position));

            { // Debug info
                Debug.WriteLine("EMPTY Pairs and positions: " + emptyPairs);
                foreach (var i in emptyPairs)
                    Debug.WriteLine("athl.1:{0,4}  athl.2:{1,4}  phase:{2,4}  position:{3,4}",
                        i.Item1, i.Item2, i.Item3, i.Item4);
            }

            return emptyPairs;
        }

    }
    #endregion

}

