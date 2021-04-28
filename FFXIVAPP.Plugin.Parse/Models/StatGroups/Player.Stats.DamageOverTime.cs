// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.Stats.DamageOverTime.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Player.Stats.DamageOverTime.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.Properties;

    public partial class Player {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageOverTime(Line line) {
            if (this.Name == Constants.CharacterName) {
                // LineHistory.Add(new LineHistory(line));
            }

            if (LimitBreaks.IsLimit(line.Action) && Settings.Default.IgnoreLimitBreaks) {
                return;
            }

            var currentDamage = line.Crit
                                    ? line.Amount > 0
                                          ? ParseHelper.GetOriginalAmount(line.Amount, .5)
                                          : 0
                                    : line.Amount;
            if (currentDamage > 0) {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentDamage);
            }

            StatGroup abilityGroup = this.GetGroup("DamageOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.DamageOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup monsterGroup = this.GetGroup("DamageOverTimeToMonsters");
            StatGroup subMonsterGroup;
            if (!monsterGroup.TryGetGroup(line.Target, out subMonsterGroup)) {
                subMonsterGroup = new StatGroup(line.Target);
                subMonsterGroup.Stats.AddStats(this.DamageOverTimeStatList(null));
                monsterGroup.AddGroup(subMonsterGroup);
            }

            StatGroup monsters = subMonsterGroup.GetGroup("DamageOverTimeToMonstersByAction");
            StatGroup subMonsterAbilityGroup;
            if (!monsters.TryGetGroup(line.Action, out subMonsterAbilityGroup)) {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(this.DamageOverTimeStatList(subMonsterGroup, true));
                monsters.AddGroup(subMonsterAbilityGroup);
            }

            this.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            this.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subMonsterGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            if (line.Crit) {
                this.Stats.IncrementStat("DamageOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                subMonsterGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                this.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subMonsterGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
            }
            else {
                this.Stats.IncrementStat("DamageOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                subMonsterGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                this.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subMonsterGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
            }
        }
    }
}