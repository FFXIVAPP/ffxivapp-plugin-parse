// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Monster.Stats.DamageTakenOverTime.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Monster.Stats.DamageTakenOverTime.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.Properties;

    public partial class Monster {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageTakenOverTime(Line line) {
            if (LimitBreaks.IsLimit(line.Action) && Settings.Default.IgnoreLimitBreaks) {
                return;
            }

            // LineHistory.Add(new LineHistory(line));
            StatGroup abilityGroup = this.GetGroup("DamageTakenOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.DamageTakenOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup damageGroup = this.GetGroup("DamageTakenOverTimeByPlayers");
            StatGroup subPlayerGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subPlayerGroup)) {
                subPlayerGroup = new StatGroup(line.Source);
                subPlayerGroup.Stats.AddStats(this.DamageTakenOverTimeStatList(null));
                damageGroup.AddGroup(subPlayerGroup);
            }

            StatGroup abilities = subPlayerGroup.GetGroup("DamageTakenOverTimeByPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup)) {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(this.DamageTakenOverTimeStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }

            this.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            this.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            if (line.Crit) {
                this.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subPlayerGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                this.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
            }
            else {
                this.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subPlayerGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                this.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
            }
        }
    }
}