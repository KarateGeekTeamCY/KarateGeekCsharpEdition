using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KarateGeek.databaseConnection;

namespace KarateGeek.helpers
{
    /* Algorithm described in project specs.
     * Simplified description (algorithm can be improved):
     * 
     * - Get List of tournament participants, in the form of List<int>,
     *   where the "int" is the their index in the DB table "Athletes".
     * - Map that List to a List<AthleteRanking> which get constructed
     *   using DB data.
     * - Apply a function to all list members which calculates their
     *   "score" (see project specs for the factors taken into account)
     * - Randomise that score a little bit (configurable!)
     * - Sort the List<AthleteRanking> by score and return a new
     *   List<int>. That List will be used by the reporting tools.
     *   
     * - The class LotteryGenerator will have a constructor that takes
     *   a tournament id and two publicly accessible methods: try(),
     *   which produces a new randomisation, and getLottery(), which
     *   returns it.
     * - It must take into account team sports (HOW?!!?)
     */

    class LotteryGenerator
    {
        private List<long> athleteList;

        public LotteryGenerator(int tournamentId) // constructor
        {
            athleteList = new LotteryGenConnection().tournamentParticipants(tournamentId);
        }

        private class AthleteRanking
        {
            
        }

        private class Visualisation //maybe needed only for debugging/tweaking? Too hard to do anyway!
        {
        
        }

        //private 

        //public 
    }
}
