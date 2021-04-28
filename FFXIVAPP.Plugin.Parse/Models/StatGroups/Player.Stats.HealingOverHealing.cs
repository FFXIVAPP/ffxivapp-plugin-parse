// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.Stats.HealingOverHealing.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Player.Stats.HealingOverHealing.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System.Text.RegularExpressions;

    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public partial class Player {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetHealingOverHealing(Line line) {
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

            StatGroup abilityGroup = this.GetGroup("HealingOverHealingByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.HealingOverHealingStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup playerGroup = this.GetGroup("HealingOverHealingToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup)) {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(this.HealingOverHealingStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }

            StatGroup abilities = subPlayerGroup.GetGroup("HealingOverHealingToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup)) {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(this.HealingOverHealingStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }

            this.Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            this.Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            if (line.Crit) {
                this.Stats.IncrementStat("HealingOverHealingCritHit");
                this.Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverHealingCritHit");
                subAbilityGroup.Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverHealingCritHit");
                subPlayerGroup.Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverHealingCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                if (line.Modifier != 0) {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverHealingCritMod";
                    this.Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else {
                this.Stats.IncrementStat("HealingOverHealingRegHit");
                this.Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverHealingRegHit");
                subAbilityGroup.Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverHealingRegHit");
                subPlayerGroup.Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverHealingRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                if (line.Modifier != 0) {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverHealingRegMod";
                    this.Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
        }

        private void SetupHealingOverHealing(Line line, HealingType healingType) {
            var cleanedAction = Regex.Replace(line.Action, @" \[.+\]", string.Empty);
            switch (healingType) {
                case HealingType.Normal:
                    line.Action = $"{cleanedAction} [∞]";
                    this.SetHealingOverHealing(line);
                    break;
                case HealingType.HealingOverTime:
                    line.Action = $"{cleanedAction} [•][∞]";
                    this.SetHealingOverHealing(line);
                    break;
            }
        }
    }
}