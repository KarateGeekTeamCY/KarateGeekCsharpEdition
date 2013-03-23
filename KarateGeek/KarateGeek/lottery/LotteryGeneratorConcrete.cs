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


/** For an overview of the Lottery Generator component, see the comment in LotteryGeneratorBaseClass.cs **/

namespace KarateGeek.lottery
{

    /* Implementation of classes LotteryGen_Expo_Ind, LotteryGen_Versus_Ind,
     * LotteryGen_Expo_Sync, LotteryGen_Expo_Team and LotteryGen_Versus_Team
     * starts here:                                                          */


    #region concrete class LotteryGen_Expo_Ind : LotteryGen_Expo
    class LotteryGen_Expo_Ind : LotteryGen_Expo
    {

        /** Class fields/properties: **/

        protected override List<Tuple<long, int>> scoreList {   // here it's an athlete scoreList
            get { return athleteScoreList; }
        }


        /** Class methods: **/

        public LotteryGen_Expo_Ind(int tournamentId)
            : base(tournamentId, 1) // calling base constructor first
        {
            this.randomisationFactor = 1500; // overriding base constructor assignment and using a very large value
        }


        public override void shuffle(int tries) { base.shuffle(); }
        public override List<long> getLottery() { return base.getLottery(); }
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

        public LotteryGen_Versus_Ind(int tournamentId)
            : base(tournamentId, 1) // calling base constructor first
        {
            //this.randomisationFactor = 650;

        }

        /** All the code of the following region was moved to the base class (and changed there
         *  to become a bit more generic). It will be replaced by overridden/tweaked methods.   **/
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

        public override List<long> getLottery() { return base.getLottery(); }



        public override List<Tuple<List<long>, bool, int, int>> getPrintableLotterySets() // groups sets for ind.kumite / fugugo / in.kata (flag)
        {
            return LotteryPrinterTransformations.IndKumiteFugugoIndKataSetsToPrintableSets(buildTournamentGameSets(), this.athletesPerTeam, lotteryMode: true);
            
            #region code moved elsewhere
            //var Sets = buildTournamentGameSets();
            //var transformedSets = new List<Tuple<List<long>, bool, int, int>>();
            //int i;

            //foreach (var set in Sets) {
            //    Debug.Assert(set.Item1.Count <= 2); // implementation only for this particular lottery type

            //    int oldPhase = set.Item3;
            //    int oldPos = set.Item4;
            //    int middlePos = (int)Math.Pow(2, oldPhase - 1);     // approx.
            //                                                        // TODO: check WINNER LotteryBox 

            //    if (set.Item1.Count > 0) {
            //        i = (oldPos <= middlePos) ? 1 : 0;
            //        foreach (long id in set.Item1) {
            //            transformedSets.Add(new Tuple<List<long>, bool, int, int>(new List<long>() { id }, set.Item2, oldPhase + 1, 2 * oldPos - i));
            //            i = (oldPos <= middlePos) ? 0 : 1;
            //        }
            //    }
            //}

            //return transformedSets;
            #endregion
        }

    }
    #endregion



    #region concrete class LotteryGen_Expo_Sync : LotteryGen_Expo
    class LotteryGen_Expo_Sync : LotteryGen_Expo
    {

        /** Class fields/properties: **/

        protected override List<Tuple<long, int>> scoreList {   // here it's a team scoreList
            get { return new TeamHelper(tournamentId, athleteScoreList).getTeamScoreList(); }
        }


        /** Class methods: **/

         public LotteryGen_Expo_Sync(int tournamentId, int athletesPerTeam)
            : base(tournamentId, athletesPerTeam)   // calling base constructor first
        {
            this.randomisationFactor = 1000;     // overriding base constructor assignment and using a large value  EDIT: rethink about randomisationFactor
        }


        public override void shuffle(int tries)
        {
            base.shuffle(0);                    // disable club constraint checking
        }


        /* Builds and returns game sets, sorted by phase descending, position ascending: */
        protected override List<Tuple<List<long>, bool, int, int>> buildTournamentGameSets()
        {
            List<Tuple<List<long>, bool, int, int>> Sets = new List<Tuple<List<long>, bool, int, int>>();

            List<long> L = this.getLottery();

            //List<Tuple<long, long, int, int>> PairsToCommit = getPairsToCommit(L); //getPairs(L) instead of getPairsToCommit(L) ??

            List<Tuple<long, long, int, int>> fullPairs = getPairs(L);
            List<Tuple<long, long, int, int>> emptyPairs = getEmptyPairs(L.Count);

            foreach (var team in fullPairs)
                Sets.Add(new Tuple<List<long>, bool, int, int>(TeamHelper.getAthletesOfTeam(team.Item1, this.tournamentId), true, team.Item3, team.Item4));

            foreach (var team in emptyPairs)
                Sets.Add(new Tuple<List<long>, bool, int, int>(TeamHelper.getAthletesOfTeam(team.Item1, this.tournamentId), false, team.Item3, team.Item4));

            return Sets;
        }

    }
    #endregion



    #region concrete class LotteryGen_Expo_Team : LotteryGen_Expo
    class LotteryGen_Expo_Team : LotteryGen_Expo
    {

        /** Class fields/properties: **/

        protected override List<Tuple<long, int>> scoreList
        {   // here it's a team scoreList
            get { return new TeamHelper(tournamentId, athleteScoreList).getTeamScoreList(); }
        }


        /** Class methods: **/

        public LotteryGen_Expo_Team(int tournamentId, int athletesPerTeam)
            : base(tournamentId, athletesPerTeam)   // calling base constructor first
        {
            this.randomisationFactor = 1000;     // overriding base constructor assignment and using a large value
        }


        public override void shuffle(int tries)
        {
            base.shuffle(0);                    // disable club constraint checking
        }


        //public override List<long> getLottery() { return null; }
        public override List<long> getLottery() { return base.getLottery(); }


        protected override List<Tuple<long, long, int, int>> getPairs(List<long> Participants)
        {
            List<Tuple<long, long, int, int>> Pairs = new List<Tuple<long, long, int, int>>();

            int phase = (int)Math.Ceiling(Math.Log(Participants.Count, 2)) - 2;
            int pos = 1;

            /* With this double for-loop, the logic of list building differs a bit from other classes.
             * Revert this change and override confirmLottery() instead?                              */
            foreach (var teamId in this.getLottery())
                foreach (var athleteId in TeamHelper.getAthletesOfTeam(teamId, this.tournamentId)) {
                    Pairs.Add(new Tuple<long, long, int, int>(athleteId, -2, phase, pos));
                    ++pos;
                }

            return Pairs;
        }


        protected override List<Tuple<long, long, int, int>> getEmptyPairs(int numOfParticipants)  //TODO: Reduce code duplication with base class
        {
            List<Tuple<long, long, int, int>> emptyPairs = new List<Tuple<long, long, int, int>>();

            //int numOfPhases = (int)Math.Ceiling(Math.Log(numOfParticipants / athletesPerTeam, 2));
            int numOfPhases = (numOfParticipants <= 2) ? 1 : (int)Math.Ceiling(Math.Log(numOfParticipants, 2)) - 1;    // FIXED: numOfParticipants is the number of teams already

            for (int phase = numOfPhases - 1; phase > 0; --phase)
                for (int position = 1; position <= Math.Pow(2, phase + 1) * athletesPerTeam; ++position) // * athletesPerTeam
                    emptyPairs.Add(new Tuple<long, long, int, int>(-1, -1, phase - 1, position));

            return emptyPairs;
        }


        public override List<Tuple<List<long>, bool, int, int>> getPrintableLotterySets() // groups sets for team kata
        {
            return LotteryPrinterTransformations.TeamKataSetsToPrintableSets(buildTournamentGameSets(), this.athletesPerTeam);

            #region code moved elsewhere
            ///** assumes correct ordering of input, please verify */

            //var Sets = buildTournamentGameSets();
            //var transformedSets = new List<Tuple<List<long>, bool, int, int>>();

            //List<long> team = new List<long>();
            //long id;
            //bool isReady;
            //int phase, pos;

            //foreach (var set in Sets) {
            //    Debug.Assert(set.Item1.Count <= 1); // implementation only for this particular lottery type

            //    if (set.Item1.Count == 0) {
            //        Debug.WriteLine("getPrintableLotterySets(): Set empty; skipped");
            //        break;
            //    }

            //    id = set.Item1.First();
            //    isReady = set.Item2;
            //    phase = set.Item3;
            //    pos = set.Item4;

            //    team.Add(id);

            //    if (team.Count == athletesPerTeam) {
            //        transformedSets.Add(new Tuple<List<long>, bool, int, int>(team, isReady, phase, (pos - 1) / athletesPerTeam + 1));

            //        // after Clear()ing the list, for some reason the Add() method stops working, so instead of calling
            //        // "team.Clear();" we re-initialize the list and let the garbage collector do its job:
            //        team = new List<long>();
            //    }
            //}

            //return transformedSets;
            #endregion
        }

    }
    #endregion



    #region concrete class LotteryGen_Versus_Team : LotteryGen_Versus
    class LotteryGen_Versus_Team : LotteryGen_Versus
    {

        /** Class fields/properties: **/

        protected override List<Tuple<long, int>> scoreList {   // here it's a team scoreList
            get { return new TeamHelper(tournamentId, athleteScoreList).getTeamScoreList(); }
        }


        /** Class methods: **/

        public LotteryGen_Versus_Team(int tournamentId, int athletesPerTeam)
            : base(tournamentId, athletesPerTeam)   // calling base constructor first
        {
            //this.randomisationFactor = 650;
        }


        public override void shuffle(int tries)
        {
            base.shuffle(0);                    // disable club constraint checking ;)
        }


        public override List<long> getLottery() { return base.getLottery(); }

        /* EXPERIMENTAL AND TOTALLY UNTESTED METHOD, inspired from the one in the LotteryGen_Expo_Team class: */
        /** TODO: FIXME: This (?) breaks for 5 teams and omits the last one! */ //EDIT: Fixed, I think? EDIT: Improved, but no!
        protected override List<Tuple<long, long, int, int>> getPairs(List<long> Participants)
        {
            List<Tuple<long, long, int, int>> teamPairs = base.getPairs(Participants);
            List<Tuple<long, long, int, int>> athletePairs = new List<Tuple<long, long, int, int>>();

            int phase = (int)Math.Ceiling(Math.Log(Participants.Count, 2)) - 3;

            /* Normally we would use (-1, -1) to write only empty !isReady pairs, but actually writing down the
             * real pairs is easier (!). The reason is that we cannot pass teamId to the database easily (as is)...
             *
             * So, we do it this way and then call a modified version of confirmLottery() to clean up athlete_id's.
             *
             * BTW, the order of athletes (which would decide the pairings) is the one of the DB query...           */
            foreach (var teamPair in teamPairs){
                if (teamPair.Item1 >= 0 && teamPair.Item2 >= 0) {
                    var t1 = TeamHelper.getAthletesOfTeam(teamPair.Item1, this.tournamentId);
                    var t2 = TeamHelper.getAthletesOfTeam(teamPair.Item2, this.tournamentId);
                    for (int i = 0; i < athletesPerTeam; ++i)
                        athletePairs.Add(new Tuple<long, long, int, int>(t1.ElementAt(i), t2.ElementAt(i), teamPair.Item3, (teamPair.Item4 - 1) * athletesPerTeam + i + 1)); // FIXME: position parameter might be wrong?
                }
                else if (teamPair.Item1 >= 0) {
                    var t1 = TeamHelper.getAthletesOfTeam(teamPair.Item1, this.tournamentId);
                    for (int i = 0; i < athletesPerTeam; ++i)
                        athletePairs.Add(new Tuple<long, long, int, int>(t1.ElementAt(i), -1, teamPair.Item3, (teamPair.Item4 - 1) * athletesPerTeam + i + 1));
                }
                else if (teamPair.Item2 >= 0) {
                    var t2 = TeamHelper.getAthletesOfTeam(teamPair.Item2, this.tournamentId);
                    for (int i = 0; i < athletesPerTeam; ++i)
                        athletePairs.Add(new Tuple<long, long, int, int>(-1, t2.ElementAt(i), teamPair.Item3, (teamPair.Item4 - 1) * athletesPerTeam + i + 1)); // ??
                }
            }

            return athletePairs;
        }


        protected override List<Tuple<long, long, int, int>> getEmptyPairs(int numOfParticipants)
        {
            return getEmptyPairs(numOfParticipants, athletesPerTeam);
        }


        public override List<Tuple<List<long>, bool, int, int>> getPrintableLotterySets() // groups sets for team kumite
        {
            return LotteryPrinterTransformations.TeamKumiteSetsToPrintableSets(buildTournamentGameSets(), this.athletesPerTeam, lotteryMode: true);

            #region code moved elsewhere
            ///** assumes correct ordering of input, please verify */

            //var Sets = buildTournamentGameSets();
            //var transformedSets = new List<Tuple<List<long>, bool, int, int>>();

            //List<long> team1 = new List<long>();
            //List<long> team2 = new List<long>();
            //long id1, id2;
            //bool isReady;
            //int newPhase, oldPos, newPos, middlePos;

            //foreach (var set in Sets)
            //{
            //    Debug.Assert(set.Item1.Count <= 2); // implementation only for this particular lottery type

            //    if (set.Item1.Count == 0)
            //    {
            //        Debug.WriteLine("getPrintableLotterySets(): Set empty; skipped");
            //        break;
            //    }

            //    isReady = set.Item2;
            //    newPhase = set.Item3 + 1;
            //    oldPos = set.Item4;
            //    newPos = 2 * ((oldPos - 1) / athletesPerTeam) + 1;
            //    middlePos = (int)Math.Pow(2, set.Item3);    // approx.

            //    {
            //        Debug.WriteLine("getPrintableLotterySets(): newPhase is:{0,3}, newPos is:{1,3}, middlePos is:{2,3}", newPhase, newPos, middlePos);
            //    }

            //    id1 = set.Item1.First();
            //    team1.Add(id1);

            //    if (set.Item1.Count == 2) {
            //        id2 = set.Item1.Skip(1).First();
            //        team2.Add(id2);
            //    }

            //    if (team1.Count == athletesPerTeam) {
            //        transformedSets.Add(new Tuple<List<long>, bool, int, int>(team1, isReady, newPhase, newPos + ((newPos <= middlePos) ? 0 : 1)));

            //        if (team2.Count > 0)            // adding an empty set shouldn't (?) be a problem, but we check anyway (it's computationally cheaper)
            //            transformedSets.Add(new Tuple<List<long>, bool, int, int>(team2, isReady, newPhase, newPos + ((newPos <= middlePos) ? 1 : 0)));

            //        // after Clear()ing the lists, for some reason the Add() method stops working, so instead of calling
            //        // "team1.Clear();" or "team2.Clear();" we re-initialize the lists and let the garbage collector do its job:
            //        team1 = new List<long>();
            //        team2 = new List<long>();
            //    }
            //}

            //return transformedSets;
            #endregion
        }


        public override void confirmLottery(bool doCommit = false)
        {
            base.confirmLottery(doCommit);

            /* Important fix for the special case of Team Kumite...
             * (because the order of the athletes in a team is not known in advance)
             *
             * Unfortunately it's even more messy to implement in the main DB transaction
             * (but it gets called immediately after and executes instantaneously).
             */
            new LotteryGenConnection().removeAthleteIdsFromTournamentParticipations(this.tournamentId);
        }

    }
    #endregion

}

