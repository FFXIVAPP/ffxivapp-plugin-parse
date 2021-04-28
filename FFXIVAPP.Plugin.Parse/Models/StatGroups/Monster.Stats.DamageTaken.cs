// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Monster.Stats.DamageTaken.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Monster.Stats.DamageTaken.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System.Linq;
    using System.Reflection;

    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.Properties;

    public partial class Monster {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageTaken(Line line) {
            if (LimitBreaks.IsLimit(line.Action) && Settings.Default.IgnoreLimitBreaks) {
                return;
            }

            this.Last20DamageTakenActions.Add(new LineHistory(line));
            if (this.Last20DamageTakenActions.Count > 20) {
                this.Last20DamageTakenActions.RemoveAt(0);
            }

            // LineHistory.Add(new LineHistory(line));
            PropertyInfo[] fields = line.GetType().GetProperties();

            StatGroup abilityGroup = this.GetGroup("DamageTakenByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.DamageTakenStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup damageGroup = this.GetGroup("DamageTakenByPlayers");
            StatGroup subPlayerGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subPlayerGroup)) {
                subPlayerGroup = new StatGroup(line.Source);
                subPlayerGroup.Stats.AddStats(this.DamageTakenStatList(null));
                damageGroup.AddGroup(subPlayerGroup);
            }

            StatGroup abilities = subPlayerGroup.GetGroup("DamageTakenByPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup)) {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(this.DamageTakenStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }

            this.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            if (line.Hit) {
                this.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subPlayerGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                if (line.Crit) {
                    this.Stats.IncrementStat("DamageTakenCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subPlayerGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    this.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    if (line.Modifier != 0) {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageTakenCritMod";
                        this.Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
                else {
                    this.Stats.IncrementStat("DamageTakenRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subPlayerGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    this.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    if (line.Modifier != 0) {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageTakenRegMod";
                        this.Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
            }
            else {
                this.Stats.IncrementStat("DamageTakenRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subPlayerGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
            }

            foreach (PropertyInfo stat in fields.Where(stat => LD.Contains(stat.Name)).Where(stat => Equals(stat.GetValue(line), true))) {
                var regStat = $"DamageTaken{stat.Name}";
                this.Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subPlayerGroup.Stats.IncrementStat(regStat);
                subPlayerAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0) {
                    continue;
                }

                var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                var modStat = $"DamageTaken{stat.Name}Mod";
                this.Stats.IncrementStat(modStat, mod);
                subAbilityGroup.Stats.IncrementStat(modStat, mod);
                subPlayerGroup.Stats.IncrementStat(modStat, mod);
                subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
            }
        }
    }
}