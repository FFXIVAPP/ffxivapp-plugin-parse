// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Monster.Stats.Kill.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Monster.Stats.Kill.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Models.Fights;

    public partial class Monster {
        /// <summary>
        /// </summary>
        /// <param name="fight"> </param>
        public void SetKill(Fight fight) {
            if (fight.MonsterName != this.Name) {
                Logging.Log(Logger, $"KillEvent : Got request to add kill stats for {fight.MonsterName}, but my name is {this.Name}!");
                return;
            }

            if (fight.MonsterName == string.Empty) {
                Logging.Log(Logger, $"KillEvent : Got request to add kill stats for {fight.MonsterName}, but no name!");
                return;
            }

            this.Stats.IncrementStat("TotalKilled");
            var avghp = this.Stats.GetStatValue("TotalOverallDamageTaken") / this.Stats.GetStatValue("TotalKilled");
            this.Stats.EnsureStatValue("AverageHP", avghp);
        }
    }
}