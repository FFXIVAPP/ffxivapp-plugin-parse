// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.Stats.Healing.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Player.Stats.Healing.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System;
    using System.Linq;
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
        public void SetHealing(Line line) {
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

            StatGroup abilityGroup = this.GetGroup("HealingByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.HealingStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup playerGroup = this.GetGroup("HealingToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup)) {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(this.HealingStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }

            StatGroup abilities = subPlayerGroup.GetGroup("HealingToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup)) {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(this.HealingStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }

            this.Stats.IncrementStat("TotalHealingActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            this.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            if (line.Crit) {
                this.Stats.IncrementStat("HealingCritHit");
                this.Stats.IncrementStat("CriticalHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingCritHit");
                subAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingCritHit");
                subPlayerGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                if (line.Modifier != 0) {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingCritMod";
                    this.Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else {
                this.Stats.IncrementStat("HealingRegHit");
                this.Stats.IncrementStat("RegularHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingRegHit");
                subAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingRegHit");
                subPlayerGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                if (line.Modifier != 0) {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingRegMod";
                    this.Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }

            if (MagicBarrierHelper.Adloquium.Any(action => string.Equals(line.Action, action, Constants.InvariantComparer))) {
                line.Amount = originalAmount;
                this.SetupHealingMitigated(line, "adloquium");
            }

            if (MagicBarrierHelper.Succor.Any(action => string.Equals(line.Action, action, Constants.InvariantComparer))) {
                line.Amount = originalAmount;
                this.SetupHealingMitigated(line, "succor");
            }

            #region OverHealing Handler

            if (unusedAmount <= 0) {
                return;
            }

            line.Amount = unusedAmount;
            this.SetupHealingOverHealing(line, HealingType.Normal);

            #endregion
        }
    }
}