﻿using System; // also has System.Random, but that's low-quality randomness
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

        public readonly int tournamentId;


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

            this.randomisationFactor = 800;
            this.tournamentId = tournamentId;
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

            /* OrderByDescending() instead of OrderBy(), because we want the highest-ranked athletes first: */
            foreach (var tuple in athleteScoreListShuffled.OrderByDescending(x => x.Item2)){
                L.Add(tuple.Item1);
                Debug.WriteLine("List item: " + tuple.Item1 + " with randomised score: " + tuple.Item2);
            }

            return L;
        }


        private List<Tuple<long, long, int, int>> getPairs(List<long> Participants) // Stub; see project specs for the algorithm
        {
            List<Tuple<long, long, int, int>> Pairs = new List<Tuple<long, long, int, int>>();

            /* Code that constructs athlete pairs goes here... */
            /**
             * ALGORITHM (please confirm correctness):
             * 
             * - copy sorted list to array (easier than using C#-style lists, I think)
             * - get array length (let's say "l")
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
             *   y*2 + (11-y) = 16  =>  y=5 and z=8-5=3 pairs in the 1st phase
             * 
             *         /           \                   /           \
             *        /             \                 /             \
             *       •               •               •               •
             *      / \             / \             / \             / \
             *     /   \           /   \           /   \           /   \
             *    /     \         /     \         /     \         /     \
             *   y1      y3      y5      •       •       •       y4      y2
             *                          / \     / \     / \
             *                         /   \   /   \   /   \
             *                        x1   x3 x5   x6 x4   x2
             * 
             * 
             *  [number of y-type athletes "above" x1:  n = (y+1) mod 2
             *   so x1 and x3 meet at the "4th position" of phase 1     ]
             * 
             * 
             *  ** There might be a smarter way to do it! (using Lists?!) **
             */

            /* Crude and untested first version: */

            int len = Participants.Count;
            int p = (int)Math.Pow(2, Math.Ceiling(Math.Log(len, 2))); // tested, works well for x>=1
            int y = p - len;
            int x = len - y;
            int z = p / 2 - y; // x must be >=2 (?)

            long[] Yarray = Participants.Take(y).ToArray();
            long[] Xarray = Participants.Skip(y).ToArray();

            /* Some assertions: */
            Debug.Assert(x == 2 * z);
            Debug.Assert(Xarray.Length == x);

            { //will be removed in the final version
                Debug.WriteLine(Participants);
                foreach (var i in Participants)
                    Debug.WriteLine(i);
                Debug.WriteLine(Yarray);
                foreach (var i in Yarray)
                    Debug.WriteLine(i);
                Debug.WriteLine(Xarray);
                foreach (var i in Xarray)
                    Debug.WriteLine(i);
            }

            //foreach (var p in Participants) ;

            return Pairs;

        }


        public void confirmLottery() // writes current lottery to the database, atomically!
        {

            if (this.confirmed)
                throw new Exception("Once \"confirmed\", a LotteryGenerator object cannot write to the database anymore");

            LotteryGenConnection conn = new LotteryGenConnection();
            List<long> L = this.getLottery();
            List<Tuple<long, long, int, int>> Pairs = getPairs(L);

            /* the flag will be changed to "true" after some testing */
            this.confirmed = conn.writeAllTournamentPairs(Pairs, tournamentId, false);
        }


        /* Methods to display a chart of the randomised athlete list: */
        private class Visualisation // maybe needed only for debugging/tweaking? Too hard to do anyway,
        {                           // because it needs optional .NET components! For now, leave it as a stub.


        }
    }
}
