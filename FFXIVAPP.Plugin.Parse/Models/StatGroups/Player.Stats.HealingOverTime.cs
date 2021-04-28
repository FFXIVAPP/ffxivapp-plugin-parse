// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.Stats.HealingOverTime.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Player.Stats.HealingOverTime.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System;
    using System.Text.RegularExpressions;

    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public partial class Player {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetHealingOverTime(Line line) {
            if (this.Name == Constants.CharacterName) {
                // LineHistory.Add(new LineHistory(line));
            }

            var currentHealing = line.Crit
                                     ? line.Amount > 0
                                           ? ParseHelper.GetOriginalAmount(line.Amount, .5)
                                           : 0
                                     : line.Amount;
            if (currentHealing > 0) {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentHealing);
            }

            var unusedAmount = 0;
            var originalAmount = line.Amount;

            // get curable of target
            try {
                var cleanedName = Regex.Replace(line.Target, @"\[[\w]+\]", string.Empty).Trim();
                var curable = Controller.Timeline.TryGetPlayerCurable(cleanedName);
                if (line.Amount > curable) {
                    unusedAmount = (int) (line.Amount - curable);
                    line.Amount = curable;
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            StatGroup abilityGroup = this.GetGroup("HealingOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.HealingOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup playerGroup = this.GetGroup("HealingOverTimeToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup)) {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(this.HealingOverTimeStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }

            StatGroup abilities = subPlayerGroup.GetGroup("HealingOverTimeToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup)) {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(this.HealingOverTimeStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }

            this.Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            this.Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            if (line.Crit) {
                this.Stats.IncrementStat("HealingOverTimeCritHit");
                this.Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverTimeCritHit");
                subPlayerGroup.Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverTimeCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                if (line.Modifier != 0) {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverTimeCritMod";
                    this.Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else {
                this.Stats.IncrementStat("HealingOverTimeRegHit");
                this.Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverTimeRegHit");
                subPlayerGroup.Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverTimeRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                if (line.Modifier != 0) {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverTimeRegMod";
                    this.Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }

            if (unusedAmount <= 0) {
                return;
            }

            line.Amount = originalAmount;
            this.SetupHealingOverHealing(line, HealingType.HealingOverTime);
        }
    }
}