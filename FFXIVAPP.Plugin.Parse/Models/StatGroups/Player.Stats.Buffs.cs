// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.Stats.Buffs.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Player.Stats.Buffs.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System;

    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public partial class Player {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetBuff(Line line) {
            if (this.Name == Constants.CharacterName) {
                // LineHistory.Add(new LineHistory(line));
            }

            StatGroup abilityGroup = this.GetGroup("BuffByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.BuffStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup playerGroup = this.GetGroup("BuffToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup)) {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(this.BuffStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }

            StatGroup abilities = subPlayerGroup.GetGroup("BuffToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup)) {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(this.BuffStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }

            this.Stats.IncrementStat("TotalBuffTime");
            subAbilityGroup.Stats.IncrementStat("TotalBuffTime");
            subPlayerGroup.Stats.IncrementStat("TotalBuffTime");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalBuffTime");
            this.AdjustBuffTime(this);
            this.AdjustBuffTime(subAbilityGroup);
            this.AdjustBuffTime(subPlayerGroup);
            this.AdjustBuffTime(subPlayerAbilityGroup);
        }

        private void AdjustBuffTime(StatGroup statGroup) {
            TimeSpan timeSpan = TimeSpan.FromSeconds(statGroup.Stats.GetStatValue("TotalBuffTime"));
            statGroup.Stats.GetStat("TotalBuffHours").Value = timeSpan.Hours;
            statGroup.Stats.GetStat("TotalBuffMinutes").Value = timeSpan.Minutes;
            statGroup.Stats.GetStat("TotalBuffSeconds").Value = timeSpan.Seconds;
        }
    }
}