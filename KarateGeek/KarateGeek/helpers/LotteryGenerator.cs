using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Random; // for randomisation
using KarateGeek.databaseConnection;

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
        private Int32 randomSeed = 134563;  // use a constant value with "random(randomSeed)" (for now),
                                            // or "random()" for a time-dependent value.


        public LotteryGenerator(int tournamentId) // constructor
        {
            athleteList = new LotteryGenConnection().tournamentParticipants(tournamentId);

            /* NOTE: If we could "capture" REAL system randomness (like /dev/random on Linux)
             * it would be much, much better than this: */
            random(randomSeed); // initialise pseudo-random number-generator
        }


        /* We need SOMETHING to hold the weights for the calculation - maybe an array of tuples?
         * Or just an array? Or an Enumeration? */
        //static private Tuple<int,int>[,] weights = new Tuple<int,int> [belt, age, pastyear, prevyears]; //example
        enum Factor {
            belt,
            age,
            pastyear,
            prevyears
        };

        static private Tuple<Factor, int>[] weights = {
                      new Tuple<Factor, int>(Factor.belt,      1000),
                      new Tuple<Factor, int>(Factor.age,       1000),
                      new Tuple<Factor, int>(Factor.pastyear,  1000),
                      new Tuple<Factor, int>(Factor.prevyears, 1000)
                    };


        private class AthleteRanking
        {
            
        }

        private class Visualisation //maybe needed only for debugging/tweaking? Too hard to do anyway!
        {
        
        }

        //private 

        public void shuffle (List<long> L) // produces a new randomization
        {
        
        }

        public List<long> getLottery()
        {
        
        }

    }
}
