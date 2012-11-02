using System; // also has System.Random, but that's low-quality randomness
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KarateGeek.databaseConnection;

/* high quality pseudo-random number generator, suitable for cryptographic purposes. Also see
 * http://msdn.microsoft.com/en-us/library/system.security.cryptography.rngcryptoserviceprovider(v=vs.100).aspx
 */
using System.Security.Cryptography.RNGCryptoServiceProvider;
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
        private List<long> athleteList;
        private Int32 randomSeed = 134563;  // use a constant value with "new Random(randomSeed)" (for now),
                                            // or "new Random()" for a time-dependent value.
        Random rgen;


        public LotteryGenerator(int tournamentId) // constructor
        {
            /* NOTE: This will throw an exception if the list is empty. This must be caught by the GUI. */
            athleteList = new LotteryGenConnection().tournamentParticipants(tournamentId);

            /* NOTE: If we could "capture" REAL system randomness (like /dev/random on Linux)
             * it would be much, much better than this: */
            rgen = new Random(randomSeed);   // initialise pseudo-random number-generator
                                             // usage: "int i = rgen.next()"

            //AthleteRanking[] array = new AthleteRanking[10];

            //LinkedList<AthleteRanking> athlin = new LinkedList<AthleteRanking>();

            //athlin.AddLast( ( new AthleteRanking().athleteId = 2) );


            //athlin.ElementAt(1).athleteId = 23;
            //long score = athlin.ElementAt(1).score;

        }

        private class AthleteRanking // ?
        {

            /* We need SOMETHING to hold the weights for the calculation - maybe an array of tuples?
             * Or just an array? Or an Enumeration? */

            //static private Tuple<int,int>[,] weights = new Tuple<int,int> [belt, age, pastyear, prevyears]; //example

            //enum Factor
            //{
            //    belt,
            //    age,
            //    pastyear,
            //    prevyears
            //};

            //static private Tuple<Factor, int>[] weights = {
            //          new Tuple<Factor, int>(Factor.belt,      1000),
            //          new Tuple<Factor, int>(Factor.age,       1000),
            //          new Tuple<Factor, int>(Factor.pastyear,  1000),
            //          new Tuple<Factor, int>(Factor.prevyears, 1000)
            //        };


            private long athleteId;
            
            private Tuple<long, int> scoreTuple
            {
                get
                {
                    return new Tuple<long, int>(athleteId, getAthleteScore());
                }
                set {
                    ;
                }
            }

            //private int[] weights;

            public AthleteRanking(long athleteId)
            {
                this.athleteId = athleteId;
            }

            private int getAthleteScore()
            {
                LotteryGenConnection conn = new LotteryGenConnection();

                int score = conn.getNumOfGoodPlacements(athleteId, 1, true)  * 500 +  // first  place in   official event
                            conn.getNumOfGoodPlacements(athleteId, 2, true)  * 300 +  // second place in   official event
                            conn.getNumOfGoodPlacements(athleteId, 3, true)  * 100 +  // third  place in   official event
                            conn.getNumOfGoodPlacements(athleteId, 4, true)  *  80 +  // fourth place in   official event
                            conn.getNumOfGoodPlacements(athleteId, 1, false) * 250 +  // first  place in unofficial event
                            conn.getNumOfGoodPlacements(athleteId, 2, false) * 150 +  // second place in unofficial event
                            conn.getNumOfGoodPlacements(athleteId, 3, false) *  50 +  // third  place in unofficial event
                            conn.getNumOfGoodPlacements(athleteId, 4, false) *  40 ;  // fourth place in unofficial event

                return score;
            }

        }

        private class Visualisation // maybe needed only for debugging/tweaking? Too hard to do anyway,
        {                           // because it needs optional .NET components! For now, leave it as a stub.
            
        }

        public void shuffle (List<long> L) // produces a new randomization
        {
            
        }

        public List<long> getLottery()
        {
            return new List<long>();
        }
    }
}
