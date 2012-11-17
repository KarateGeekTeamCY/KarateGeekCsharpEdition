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

        public static LotteryGenerator Create(int tournamentId)
        {
            LotteryGenerator lg;

            /* Assignment of lottery type: the cases of the following switch might be WRONG.
             * 
             * For indKata and syncKata we should also check the scoring system! (For kids, it's
             * the flag system, so it's a "versus" type tournament.) I'm not sure about teamKata... */
            switch (new LotteryGenConnection().getTournamentGameType(tournamentId))
            {
                case Strings.indKata: if (new LotteryGenConnection().getTournamentScoringType(tournamentId).Equals(Strings.flag, StringComparison.Ordinal))
                                             lg = new LotteryGen_Versus_Ind(tournamentId);
                                         else // score system
                                             lg = new LotteryGen_Expo_Ind(tournamentId);
                                         break;

                case Strings.teamKata:   lg = new LotteryGen_Expo_Ind(tournamentId);    // !!
                                         break;

                case Strings.indKumite:
                case Strings.fugugo:     lg = new LotteryGen_Versus_Ind(tournamentId);
                                         break;

                /* case Strings.teamKata: // was here */
                case Strings.enbu:
                case Strings.syncKata:   lg = new LotteryGen_Expo_Team(tournamentId);
                                         break;

                case Strings.teamKumite: lg = new LotteryGen_Versus_Team(tournamentId);
                                         break;

                case Strings.individual: // not enough info
                case Strings.team:       // not enough info
                default:                 lg = null;
                                         break;
            }

            /* For now (!) only create LotteryGen_Versus_Ind objects... */
            //lg = new LotteryGen_Versus_Ind(tournamentId);

            Debug.Assert(lg != null);

            return lg;
        }

    }

}

