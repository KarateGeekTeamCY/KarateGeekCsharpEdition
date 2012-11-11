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

    /* Factory class: */
    static class LotteryGeneratorFactory
    {
        /* enum not really needed anymore, replaced by class structure */
        //public enum LotteryType
        //{
        //    indiv_single,
        //    indiv_versus,
        //    team_single,
        //    team_versus
        //};

        public static LotteryGenerator Create(int tournamentId)
        {
            LotteryGenerator lg;

            /* Assignment of lottery type: the cases of the following switch might be WRONG.
             * 
             * For indKata and syncKata we should also check the scoring system! (For kids, it's
             * the flag system, so it's a "versus" type tournament.) I'm not sure about teamKata... */
            switch (new LotteryGenConnection().getTournamentGameType(tournamentId))
            {
                case Strings.indKata:    if (new LotteryGenConnection().getTournamentScoringType(tournamentId) == Strings.flag)
                                             lg = new LotteryGen_Versus_Ind(tournamentId);
                                         else // score system
                                             lg = new LotteryGen_Expo_Ind(tournamentId);
                                         break;

                case Strings.fugugo:
                case Strings.indKumite:
                case Strings.individual: lg = new LotteryGen_Versus_Ind(tournamentId);
                                         break;

                case Strings.enbu:
                case Strings.teamKata:   lg = new LotteryGen_Expo_Team(tournamentId);
                                         break;

                case Strings.syncKata:   if (new LotteryGenConnection().getTournamentScoringType(tournamentId) == Strings.flag)
                                             lg = new LotteryGen_Versus_Team(tournamentId);
                                         else // score system
                                             lg = new LotteryGen_Expo_Team(tournamentId);
                                         break;

                case Strings.teamKumite:
                case Strings.team:       lg = new LotteryGen_Versus_Team(tournamentId);
                                         break;

                default:                 lg = null;
                                         break;
            }

            // For now (!) only create LotteryGen_Versus_Ind objects...
            lg = new LotteryGen_Versus_Ind(tournamentId);

            Debug.Assert(lg != null);

            return lg;
        }

    }

}

