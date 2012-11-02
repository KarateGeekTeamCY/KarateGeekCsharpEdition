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
     * - Get List of tournament participants, in the form of List<int>,
     *   where the "int" is the their index in the DB table "Athletes".
     * - Map that List to a List<AthleteRanking> which get constructed
     *   using DB data.
     * - Apply a function to all list members which calculates their
     *   "score" (see project specs for the Factors taken into account)
     * - Randomise that score a little bit (configurable!)
     * - Sort the List<AthleteRanking> by score and return a new
     *   List<int>. That List will be used by the reporting tools.
     *   
     * - The class LotteryGenerator will have a constructor that takes
     *   a tournament id and two publicly accessible methods: shuffle(),
     *   which produces a new randomisation, and getLottery(), which
     *   returns it.
     * - It must take into account team sports (HOW?!!?)
     */

    class LotteryGenerator
    {
        private readonly List<long> athleteList;

        private readonly List<Tuple<long, int>> athleteScoreList;

        private List<Tuple<long, int>> athleteScoreListShuffled;

        private Int32 randomSeed = 134563;  // use a constant value with "new Random(randomSeed)" (for now),
                                            // or "new Random()" for a time-dependent value.
        Random rgen;


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
            rgen = new Random(randomSeed);   // initialise pseudo-random number-generator
                                             // usage: "int i = rgen.next()"

        }



        private class Visualisation // maybe needed only for debugging/tweaking? Too hard to do anyway,
        {                           // because it needs optional .NET components! For now, leave it as a stub.

        }


        private int getAthleteScore(long athleteId)
        {
            LotteryGenConnection conn = new LotteryGenConnection();

            int score = conn.getNumOfGoodPlacements(athleteId, 1, true)  * 400 +  // first  place in   official event
                        conn.getNumOfGoodPlacements(athleteId, 2, true)  * 240 +  // second place in   official event
                        conn.getNumOfGoodPlacements(athleteId, 3, true)  * 120 +  // third  place in   official event
                        conn.getNumOfGoodPlacements(athleteId, 4, true)  *  80 +  // fourth place in   official event
                        conn.getNumOfGoodPlacements(athleteId, 1, false) * 200 +  // first  place in unofficial event
                        conn.getNumOfGoodPlacements(athleteId, 2, false) * 120 +  // second place in unofficial event
                        conn.getNumOfGoodPlacements(athleteId, 3, false) *  60 +  // third  place in unofficial event
                        conn.getNumOfGoodPlacements(athleteId, 4, false) *  40;   // fourth place in unofficial event

            String belt = conn.getBeltColor(athleteId);

            for (int i = 0 ; i < Strings.rank.Length; ++i)
                if (Strings.rank[i] == belt)
                    score += i * 120;

            return score;
        }


        public void shuffle() // produces a new randomization  UNFINISHED
        {
            Debug.WriteLine("shuffling athlete list...");

            //athleteScoreListShuffled = athleteScoreList.CopyTo();

            //foreach (var tuple in athleteScoreListShuffled)
            //{
            //    L.Add(tuple.Item1);
            //    Debug.WriteLine("List item: " + tuple.Item1 + " with score: " + tuple.Item2);
            //}
            
        }


        public List<long> getLottery() // UNFINISHED (?)
        {
            if (athleteScoreListShuffled == null) throw new Exception("Use method shuffle() first.");
            
            List<long> L = new List<long>();

            foreach (var tuple in athleteScoreList.OrderBy(x => x.Item2)){
                L.Add(tuple.Item1);
                Debug.WriteLine("List item: " + tuple.Item1 + " with score: " + tuple.Item2);
            }

            return L;
        }
    }
}
