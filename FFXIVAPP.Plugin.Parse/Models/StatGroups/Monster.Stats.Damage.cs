// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Monster.Stats.Damage.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Monster.Stats.Damage.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System.Linq;
    using System.Reflection;

    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public partial class Monster {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        /// <param name="isDamageOverTime"></param>
        public void SetDamage(Line line, bool isDamageOverTime = false) {
            // LineHistory.Add(new LineHistory(line));
            this.Last20DamageActions.Add(new LineHistory(line));
            if (this.Last20DamageActions.Count > 20) {
                this.Last20DamageActions.RemoveAt(0);
            }

            PropertyInfo[] fields = line.GetType().GetProperties();

            var currentDamage = line.Crit
                                    ? line.Amount > 0
                                          ? ParseHelper.GetOriginalAmount(line.Amount, .5)
                                          : 0
                                    : line.Amount;
            if (currentDamage > 0) {
                ParseHelper.LastAmountByAction.EnsureMonsterAction(line.Source, line.Action, currentDamage);
            }

            StatGroup abilityGroup = this.GetGroup("DamageByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.DamageStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup playerGroup = this.GetGroup("DamageToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup)) {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(this.DamageStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }

            StatGroup monsters = subPlayerGroup.GetGroup("DamageToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!monsters.TryGetGroup(line.Action, out subPlayerAbilityGroup)) {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(this.DamageStatList(subPlayerGroup, true));
                monsters.AddGroup(subPlayerAbilityGroup);
            }

            if (!isDamageOverTime) {
                this.Stats.IncrementStat("TotalDamageActionsUsed");
                subAbilityGroup.Stats.IncrementStat("TotalDamageActionsUsed");
                subPlayerGroup.Stats.IncrementStat("TotalDamageActionsUsed");
                subPlayerAbilityGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            }

            if (line.Hit) {
                this.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subPlayerGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                if (line.Crit) {
                    this.Stats.IncrementStat("DamageCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageCritHit");
                    subPlayerGroup.Stats.IncrementStat("DamageCritHit");
                    subPlayerAbilityGroup.Stats.IncrementStat("DamageCritHit");
                    this.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    if (line.Modifier != 0) {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageCritMod";
                        this.Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
                else {
                    this.Stats.IncrementStat("DamageRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageRegHit");
                    subPlayerGroup.Stats.IncrementStat("DamageRegHit");
                    subPlayerAbilityGroup.Stats.IncrementStat("DamageRegHit");
                    this.Stats.IncrementStat("RegularDamage", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    if (line.Modifier != 0) {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageRegMod";
                        this.Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
            }
            else {
                this.Stats.IncrementStat("DamageRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageRegMiss");
                subPlayerGroup.Stats.IncrementStat("DamageRegMiss");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageRegMiss");
            }

            foreach (PropertyInfo stat in fields.Where(stat => LD.Contains(stat.Name)).Where(stat => Equals(stat.GetValue(line), true))) {
                var regStat = $"Damage{stat.Name}";
                this.Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subPlayerGroup.Stats.IncrementStat(regStat);
                subPlayerAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0) {
                    continue;
                }

                var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                var modStat = $"Damage{stat.Name}Mod";
                this.Stats.IncrementStat(modStat, mod);
                subAbilityGroup.Stats.IncrementStat(modStat, mod);
                subPlayerGroup.Stats.IncrementStat(modStat, mod);
                subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
            }
        }
    }
}