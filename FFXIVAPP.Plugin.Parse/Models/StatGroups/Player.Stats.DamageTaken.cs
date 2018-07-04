// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.Stats.DamageTaken.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Player.Stats.DamageTaken.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System.Linq;
    using System.Reflection;

    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public partial class Player {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageTaken(Line line) {
            if (this.Name == Constants.CharacterName) {
                // LineHistory.Add(new LineHistory(line));
            }

            this.Last20DamageTakenActions.Add(new LineHistory(line));
            if (this.Last20DamageTakenActions.Count > 20) {
                this.Last20DamageTakenActions.RemoveAt(0);
            }

            PropertyInfo[] fields = line.GetType().GetProperties();

            StatGroup abilityGroup = this.GetGroup("DamageTakenByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.DamageTakenStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup damageGroup = this.GetGroup("DamageTakenByMonsters");
            StatGroup subMonsterGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subMonsterGroup)) {
                subMonsterGroup = new StatGroup(line.Source);
                subMonsterGroup.Stats.AddStats(this.DamageTakenStatList(null));
                damageGroup.AddGroup(subMonsterGroup);
            }

            StatGroup abilities = subMonsterGroup.GetGroup("DamageTakenByMonstersByAction");
            StatGroup subMonsterAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subMonsterAbilityGroup)) {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(this.DamageTakenStatList(subMonsterGroup, true));
                abilities.AddGroup(subMonsterAbilityGroup);
            }

            this.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            if (line.Hit) {
                this.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subMonsterGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                if (line.Crit) {
                    this.Stats.IncrementStat("DamageTakenCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subMonsterGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    this.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    if (line.Modifier != 0) {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageTakenCritMod";
                        this.Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
                else {
                    this.Stats.IncrementStat("DamageTakenRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subMonsterGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    this.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    if (line.Modifier != 0) {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageTakenRegMod";
                        this.Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
            }
            else {
                this.Stats.IncrementStat("DamageTakenRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subMonsterGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
            }

            foreach (PropertyInfo stat in fields.Where(stat => LD.Contains(stat.Name)).Where(stat => Equals(stat.GetValue(line), true))) {
                var regStat = $"DamageTaken{stat.Name}";
                this.Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subMonsterGroup.Stats.IncrementStat(regStat);
                subMonsterAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0) {
                    continue;
                }

                var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                var modStat = $"DamageTaken{stat.Name}Mod";
                this.Stats.IncrementStat(modStat, mod);
                subAbilityGroup.Stats.IncrementStat(modStat, mod);
                subMonsterGroup.Stats.IncrementStat(modStat, mod);
                subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
            }
        }
    }
}