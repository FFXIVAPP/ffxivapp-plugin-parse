// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.Stats.HealingMitigated.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Player.Stats.HealingMitigated.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System.Text.RegularExpressions;

    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public partial class Player {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetHealingMitigated(Line line) {
            if (this.Name == Constants.CharacterName) {
                // LineHistory.Add(new LineHistory(line));
            }

            this.Last20HealingActions.Add(new LineHistory(line));
            if (this.Last20HealingActions.Count > 20) {
                this.Last20HealingActions.RemoveAt(0);
            }

            var currentHealing = line.Crit
                                     ? line.Amount > 0
                                           ? ParseHelper.GetOriginalAmount(line.Amount, .5)
                                           : 0
                                     : line.Amount;
            if (currentHealing > 0) {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentHealing);
            }

            StatGroup abilityGroup = this.GetGroup("HealingMitigatedByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.HealingMitigatedStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup playerGroup = this.GetGroup("HealingMitigatedToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup)) {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(this.HealingMitigatedStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }

            StatGroup abilities = subPlayerGroup.GetGroup("HealingMitigatedToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup)) {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(this.HealingMitigatedStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }

            this.Stats.IncrementStat("TotalHealingMitigatedActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingMitigatedActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingMitigatedActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingMitigatedActionsUsed");
            this.Stats.IncrementStat("TotalOverallHealingMitigated", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealingMitigated", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealingMitigated", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealingMitigated", line.Amount);
            if (line.Crit) {
                this.Stats.IncrementStat("HealingMitigatedCritHit");
                this.Stats.IncrementStat("CriticalHealingMitigated", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingMitigatedCritHit");
                subAbilityGroup.Stats.IncrementStat("CriticalHealingMitigated", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingMitigatedCritHit");
                subPlayerGroup.Stats.IncrementStat("CriticalHealingMitigated", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingMitigatedCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealingMitigated", line.Amount);
                if (line.Modifier != 0) {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingMitigatedCritMod";
                    this.Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else {
                this.Stats.IncrementStat("HealingMitigatedRegHit");
                this.Stats.IncrementStat("RegularHealingMitigated", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingMitigatedRegHit");
                subAbilityGroup.Stats.IncrementStat("RegularHealingMitigated", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingMitigatedRegHit");
                subPlayerGroup.Stats.IncrementStat("RegularHealingMitigated", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingMitigatedRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealingMitigated", line.Amount);
                if (line.Modifier != 0) {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingMitigatedRegMod";
                    this.Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
        }

        public void SetupHealingMitigated(Line line, string type = "") {
            var cleanedAction = Regex.Replace(line.Action, @" \[.+\]", string.Empty);
            line.Action = $"{cleanedAction} [☯]";
            switch (type) {
                case "adloquium":
                    if (line.Crit) {
                        line.Amount = line.Amount * 2;
                    }

                    break;
                case "succor":
                    break;
                default:
                    break;
            }

            this.SetHealingMitigated(line);
        }
    }
}