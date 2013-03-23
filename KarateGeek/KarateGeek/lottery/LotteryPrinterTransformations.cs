using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace KarateGeek.lottery
{
    static class LotteryPrinterTransformations
    {

        /** groups sets for ind.kumite / fugugo / in.kata (flag)
         *  → lotteryMode allows printing of single (waiting) athletes (disabled by default because the printing order cannot be controlled during tournaments) */
        public static List<Tuple<List<long>, bool, int, int>> IndKumiteFugugoIndKataSetsToPrintableSets(List<Tuple<List<long>, bool, int, int>> initialSets, int athletesPerTeam, bool lotteryMode = false)
        {
            var transformedSets = new List<Tuple<List<long>, bool, int, int>>();
            int i;

            foreach (var set in initialSets)
            {
                Debug.Assert(set.Item1.Count <= 2); // implementation only for this particular lottery type (LotteryGen_Versus_Ind)

                int oldPhase = set.Item3;
                int oldPos = set.Item4;
                int middlePos = (int)Math.Pow(2, oldPhase - 1);     // approx.

                //if (set.Item1.Count > 0)
                //{
                //    i = (oldPos <= middlePos) ? 1 : 0;
                //    foreach (long id in set.Item1)
                //    {
                //        transformedSets.Add(new Tuple<List<long>, bool, int, int>(new List<long>() { id }, set.Item2, oldPhase + 1, 2 * oldPos - i));
                //        i = (oldPos <= middlePos) ? 0 : 1;
                //    }
                //}
                /**/
                if (set.Item1.Count == 1 && lotteryMode) {
                    i = (oldPos <= middlePos) ? 1 : 0;
                    transformedSets.Add(new Tuple<List<long>, bool, int, int>(new List<long>() { set.Item1.ElementAt(0) }, set.Item2, oldPhase + 1, 2 * oldPos - i));
                }
                if (set.Item1.Count == 2) {
                    transformedSets.Add(new Tuple<List<long>, bool, int, int>(new List<long>() { set.Item1.ElementAt(0) }, set.Item2, oldPhase + 1, 2 * oldPos - 1));
                    transformedSets.Add(new Tuple<List<long>, bool, int, int>(new List<long>() { set.Item1.ElementAt(1) }, set.Item2, oldPhase + 1, 2 * oldPos));
                }
                /**/
            }

            return transformedSets;
        }


        /** groups sets for team kata */
        public static List<Tuple<List<long>, bool, int, int>> TeamKataSetsToPrintableSets(List<Tuple<List<long>, bool, int, int>> initialSets, int athletesPerTeam)
        {
            /** assumes correct ordering of input, please verify */

            var transformedSets = new List<Tuple<List<long>, bool, int, int>>();

            List<long> team = new List<long>();
            long id;
            bool isReady;
            int phase, pos;

            foreach (var set in initialSets)
            {
                Debug.Assert(set.Item1.Count <= 1); // implementation only for this particular lottery type (LotteryGen_Expo_Team)

                if (set.Item1.Count == 0)
                {
                    Debug.WriteLine("getPrintableLotterySets(): Set empty; skipped");
                    break;
                }

                id = set.Item1.First();
                isReady = set.Item2;
                phase = set.Item3;
                pos = set.Item4;

                team.Add(id);

                if (team.Count == athletesPerTeam)
                {
                    transformedSets.Add(new Tuple<List<long>, bool, int, int>(team, isReady, phase, (pos - 1) / athletesPerTeam + 1));

                    // after Clear()ing the list, for some reason the Add() method stops working, so instead of calling
                    // "team.Clear();" we re-initialize the list and let the garbage collector do its job:
                    team = new List<long>();
                }
            }

            return transformedSets;
        }


        /** groups sets for team kumite
         *  → lotteryMode allows printing of single (waiting) teams (disabled by default because the printing order cannot be controlled during tournaments) */
        public static List<Tuple<List<long>, bool, int, int>> TeamKumiteSetsToPrintableSets(List<Tuple<List<long>, bool, int, int>> initialSets, int athletesPerTeam, bool lotteryMode = false)
        {
            /** assumes correct ordering of input, please verify */
            var transformedSets = new List<Tuple<List<long>, bool, int, int>>();

            List<long> team1 = new List<long>();
            List<long> team2 = new List<long>();
            long id1, id2;
            bool isReady;
            int newPhase, oldPos, newPos, middlePos;

            foreach (var set in initialSets)
            {
                Debug.Assert(set.Item1.Count <= 2); // implementation only for this particular lottery type (LotteryGen_Versus_Team)

                if (set.Item1.Count == 0)
                {
                    Debug.WriteLine("getPrintableLotterySets(): Set empty; skipped");
                    break;
                }

                isReady = set.Item2;
                newPhase = set.Item3 + 1;
                oldPos = set.Item4;
                newPos = 2 * ((oldPos - 1) / athletesPerTeam) + 1;
                middlePos = (int)Math.Pow(2, set.Item3);    // approx.

                {
                    Debug.WriteLine("getPrintableLotterySets(): newPhase is:{0,3}, newPos is:{1,3}, middlePos is:{2,3}", newPhase, newPos, middlePos);
                }

                id1 = set.Item1.First();
                team1.Add(id1);

                if (set.Item1.Count == 2) {
                    id2 = set.Item1.Skip(1).First();
                    team2.Add(id2);
                }

                //if (team1.Count == athletesPerTeam) {
                //    transformedSets.Add(new Tuple<List<long>, bool, int, int>(team1, isReady, newPhase, newPos + ((newPos <= middlePos) ? 0 : 1)));

                //    if (team2.Count > 0)            // adding an empty set shouldn't (?) be a problem, but we check anyway (it's computationally cheaper)
                //        transformedSets.Add(new Tuple<List<long>, bool, int, int>(team2, isReady, newPhase, newPos + ((newPos <= middlePos) ? 1 : 0)));

                //    // after Clear()ing the lists, for some reason the Add() method stops working, so instead of calling
                //    // "team1.Clear();" or "team2.Clear();" we re-initialize the lists and let the garbage collector do its job:
                //    team1 = new List<long>();
                //    team2 = new List<long>();
                //}
                /**/
                if (team1.Count == athletesPerTeam) {
                    if (team2.Count == 0 && lotteryMode)
                        transformedSets.Add(new Tuple<List<long>, bool, int, int>(team1, isReady, newPhase, newPos + ((newPos <= middlePos) ? 0 : 1)));
                    else {
                        transformedSets.Add(new Tuple<List<long>, bool, int, int>(team1, isReady, newPhase, newPos));
                        transformedSets.Add(new Tuple<List<long>, bool, int, int>(team2, isReady, newPhase, newPos + 1));
                    }

                    // after Clear()ing the lists, for some reason the Add() method stops working, so instead of calling
                    // "team1.Clear();" or "team2.Clear();" we re-initialize the lists and let the garbage collector do its job:
                    team1 = new List<long>();
                    team2 = new List<long>();
                }
                /**/
            }

            return transformedSets;
        }

    }
}
