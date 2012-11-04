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
    /** Algorithm described in project specs.
      * Simplified description (algorithm can be improved):
      * 
      * - Get list of tournament participants, in the form of List<long>,
      *   where the "long" is their index in the DB table "Athletes".
      * - Map that List to a List of tuples (athleteScoreList) which gets
      *   constructed using DB data. This applies a function to all list
      *   members which calculates their "score" (see project specs
      *   for the Factors taken into account)
      * - Randomise that score a little bit (configurable!)
      * - Sort the List<AthleteRanking> by score and return a new
      *   List<int>. That List will be used by the reporting tools.
      *
      * - The class LotteryGenerator will have a constructor that takes
      *   a tournament id and three publicly accessible methods:
      *   shuffle(), which produces a new randomisation, getLottery(),
      *   which returns it, and confirmLottery(), which writes to the DB.
      * - It must take into account team sports (HOW?!!?)
      * 
      * CURRENTLY ONLY IMPLEMENTED FOR "VERSUS"-TYPE TOURNAMENTS!
      */

    class LotteryGenerator
    {

        /** Class fields/properties: */


        private readonly List<long> athleteList;

        private readonly List<Tuple<long, int>> athleteScoreList;


        private List<Tuple<long, int>> athleteScoreListShuffled;

        private bool confirmed = false; // once "confirmed", a LotteryGenerator object cannot write to the DB anymore!

        private Int32 randomSeed = 134368;  // use a constant value with "new Random(randomSeed)",
                                            // or "new Random()" for a time-dependent value... (UNUSED, for now)
        private Random rgen;

        public int randomisationFactor { get; set; }



        /** Class methods: */

        public LotteryGenerator(int tournamentId) // constructor
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
            randomisationFactor = 800;
        }


        /* NOTE: Lots of hard-coded values here, but at least all of them are in the same place
         * and easy to edit (also set the "randomisationFactor" in the class constructor!)      */
        private int getAthleteScore(long athleteId)
        {
            LotteryGenConnection conn = new LotteryGenConnection();
            
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
            for (int i = 0 ; i < Strings.rank.Length; ++i)
                if (Strings.rank[i] == belt)
                    score += i * 125;

            /* age, only for children (<18): */
            int age = conn.getAge(athleteId);
            for (int i = 0; i < age && i < 18; ++i)
                score += 100;

            return score;
        }


        public void shuffle() // produces a new randomization [It would be less ugly with more LINQ usage!]
        {
            List<Tuple<long, int>> L = new List<Tuple<long, int>>();

            foreach (var tuple in athleteScoreList) //TODO: Tweak randomisation factor
            {
                L.Add(new Tuple<long, int> (tuple.Item1, tuple.Item2 + rgen.Next(0, randomisationFactor)));
                Debug.WriteLine("List item: " + tuple.Item1 + " with initial score: " + tuple.Item2);
            }

            athleteScoreListShuffled = L;
        }


        public List<long> getLottery()
        {
            if (athleteScoreListShuffled == null) throw new Exception("Use method shuffle() first.");
            
            List<long> L = new List<long>();

            foreach (var tuple in athleteScoreListShuffled.OrderBy(x => x.Item2)){
                L.Add(tuple.Item1);
                Debug.WriteLine("List item: " + tuple.Item1 + " with randomised score: " + tuple.Item2);
            }

            return L;
        }


        private List<Tuple<long, long, int, int>> getPairsHelper(List<long> Participants) // recursive
        {
            /* Oops! C#, despite LINQ, doesn't have functional-style lists! */
            //if (Participants.Count() == 0)
            //    return new List<Tuple<long, long, int, int>>();
            //else
            //    //return getPairsHelper(...).Add(new Tuple<long, long, int, int>(Participants.head, .tail));
            return null;
        }

        private List<Tuple<long, long, int, int>> getPairs(List<long> Participants) // Stub; see project specs for the algorithm
        {
            List<Tuple<long, long, int, int>> Pairs = new List<Tuple<long,long,int,int>>();

            /* Code that constructs athlete pairs goes here... */
            
            /* Crude and untested first version (the algorithm might be incorrect!): */
            foreach (var p in Participants) ;

            return Pairs;

        }


        public void confirmLottery() // writes current lottery to the database, atomically!
        {

            if (this.confirmed)
                throw new Exception("Once \"confirmed\", a LotteryGenerator object cannot write to the database anymore");

            LotteryGenConnection conn = new LotteryGenConnection();
            List<long> L = this.getLottery();
            List<Tuple<long, long, int, int>> Pairs = getPairs(L);
            this.confirmed = conn.writeAllTournamentPairs(Pairs, false); // the flag will be changed to "true" after some testing
        }


        /* Methods to display a chart of the randomised athlete list: */
        private class Visualisation // maybe needed only for debugging/tweaking? Too hard to do anyway,
        {                           // because it needs optional .NET components! For now, leave it as a stub.


        }
    }
}
