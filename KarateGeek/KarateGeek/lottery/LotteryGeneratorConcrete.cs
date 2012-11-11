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


/** For an overview of the Lottery Generator component, see the comment in LotteryGeneratorBaseClass.cs **/

namespace KarateGeek.lottery
{

    /* Implementation of classes LotteryGen_Expo_Ind, LotteryGen_Versus_Ind,
     * LotteryGen_Expo_Team and LotteryGen_Versus_Team starts here:          */


    #region concrete class LotteryGen_Expo_Ind : LotteryGen_Expo
    class LotteryGen_Expo_Ind : LotteryGen_Expo
    {

        /** Class fields/properties: **/

        protected override List<Tuple<long, int>> scoreList {   // here it's an athlete scoreList
            get { return athleteScoreList; }
        }


        /** Class methods: **/

        public LotteryGen_Expo_Ind(int tournamentId)
            : base(tournamentId)// calling base constructor first
        {
            this.randomisationFactor = 1200; // overriding base constructor assignment and using a very large value
        }

        public override void shuffle(int tries){}
        public override List<long> getLottery() { return null; }
    }
    #endregion



    #region concrete class LotteryGen_Versus_Ind : LotteryGen_Versus
    class LotteryGen_Versus_Ind : LotteryGen_Versus
    {

        /** Class fields/properties: **/

        protected override List<Tuple<long, int>> scoreList {   // here it's an athlete scoreList
            get { return athleteScoreList; }
        }


        /** Class methods: **/

        public LotteryGen_Versus_Ind(int tournamentId) : base(tournamentId)// calling base constructor first
        {
            //this.randomisationFactor = 650;
            
        }

        /** All the following code was moved to the base class (and changed there to become a
         *  bit more generic). It will be replaced by overridden/tweaked methods.             **/
        #region deprecated code (moved to the base class LotteryGenerator and slightly modified there)
        ///* produces a new randomization [It would be less ugly with more LINQ usage!] */
        //public override void shuffle(int tries = 2) // a default value of 2 seems OK - we don't want infinite recursion!
        //{
        //    List<Tuple<long, int>> L = new List<Tuple<long, int>>();

        //    foreach (var tuple in athleteScoreList) //TODO: Tweak randomisation factor
        //    {
        //        // L.Add(new Tuple<long, int>(tuple.Item1, tuple.Item2 + rgen.Next(0, randomisationFactor)));
        //        L.Add(new Tuple<long, int>(tuple.Item1, tuple.Item2
        //            + rgen.Next(0, randomisationFactor * ( 1 + tuple.Item2 / 1000))));
        //        Debug.WriteLine("List item: " + tuple.Item1 + " with initial score: " + tuple.Item2);
        //    }

        //    scoreListShuffled = L;

        //    /* EXPERIMENTAL and very computationally expensive way to check whether an athlete pair belongs
        //     * to the same club... Some refactoring would reduce the redundancy, but it should work as-is: */
        //    if ( tries > 0 && pairsClubConstraintActive(getPairs(this.getLottery())) ){
        //        Debug.WriteLine("\n ** AUTO-RESHUFFLING because same-club collisions were found... ** \n");
        //        shuffle(tries - 1);
        //    }
        //}


        //public override List<long> getLottery()
        //{
        //    if (scoreListShuffled == null) throw new Exception("Use method shuffle() first.");
            
        //    List<long> L = new List<long>();

        //    /* OrderByDescending() instead of OrderBy(), because we want the highest-ranked athletes first: */
        //    foreach (var tuple in scoreListShuffled.OrderByDescending(x => x.Item2)){
        //        L.Add(tuple.Item1);
        //        Debug.WriteLine("List item: " + tuple.Item1 + " with randomised score: " + tuple.Item2);
        //    }

        //    return L;
        //}


        //protected bool pairsClubConstraintActive(List<Tuple<long, long, int, int>> Pairs)
        //{
        //    LotteryGenConnection conn = new LotteryGenConnection();

        //    foreach (var pair in Pairs)
        //        if (conn.sameClubAthletePair(pair))
        //            return true;

        //    return false;
        //}
        #endregion

        public override List<long> getLottery()
        {
            return base.getLottery();
        }

    }
    #endregion



    #region concrete class LotteryGen_Expo_Team : LotteryGen_Expo
    class LotteryGen_Expo_Team : LotteryGen_Expo
    {

        /** Class fields/properties: **/

        protected override List<Tuple<long, int>> scoreList {   // here it's a team scoreList
            get { return new TeamHelper(tournamentId).getTeamScoreList(athleteScoreList); }
        }


        /** Class methods: **/

         public LotteryGen_Expo_Team(int tournamentId)
            : base(tournamentId)// calling base constructor first
        {
            this.randomisationFactor = 2000; // overriding base constructor assignment and using a very large value
        }

        
        public override void shuffle(int tries)
        {
            base.shuffle(0);                    // disable club constraint checking
        }


        public override List<long> getLottery() { return null; }
    }
    #endregion



    #region concrete class LotteryGen_Versus_Team : LotteryGen_Versus
    class LotteryGen_Versus_Team : LotteryGen_Versus
    {

        /** Class fields/properties: **/

        protected override List<Tuple<long, int>> scoreList {   // here it's a team scoreList
            get { return new TeamHelper(tournamentId).getTeamScoreList(athleteScoreList); }
        }


        /** Class methods: **/

        public LotteryGen_Versus_Team(int tournamentId)
            : base(tournamentId)// calling base constructor first
        {
            //this.randomisationFactor = 650;
        }


        public override void shuffle(int tries)
        {
            base.shuffle(0);                    // disable club constraint checking
        }


        public override List<long> getLottery() { return null; }
    }
    #endregion

}

