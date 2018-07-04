// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.Stats.Damage.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Player.Stats.Damage.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System.Linq;
    using System.Reflection;

    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.Properties;

    public partial class Player {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamage(Line line) {
            if (this.Name == Constants.CharacterName) {
                // LineHistory.Add(new LineHistory(line));
            }

            this.Last20DamageActions.Add(new LineHistory(line));
            if (this.Last20DamageActions.Count > 20) {
                this.Last20DamageActions.RemoveAt(0);
            }

            if (LimitBreaks.IsLimit(line.Action) && Settings.Default.IgnoreLimitBreaks) {
                return;
            }

            PropertyInfo[] fields = line.GetType().GetProperties();

            var currentDamage = line.Crit
                                    ? line.Amount > 0
                                          ? ParseHelper.GetOriginalAmount(line.Amount, .5)
                                          : 0
                                    : line.Amount;
            if (currentDamage > 0) {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentDamage);
            }

            StatGroup abilityGroup = this.GetGroup("DamageByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup)) {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(this.DamageStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }

            StatGroup monsterGroup = this.GetGroup("DamageToMonsters");
            StatGroup subMonsterGroup;
            if (!monsterGroup.TryGetGroup(line.Target, out subMonsterGroup)) {
                subMonsterGroup = new StatGroup(line.Target);
                subMonsterGroup.Stats.AddStats(this.DamageStatList(null));
                monsterGroup.AddGroup(subMonsterGroup);
            }

            StatGroup monsters = subMonsterGroup.GetGroup("DamageToMonstersByAction");
            StatGroup subMonsterAbilityGroup;
            if (!monsters.TryGetGroup(line.Action, out subMonsterAbilityGroup)) {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(this.DamageStatList(subMonsterGroup, true));
                monsters.AddGroup(subMonsterAbilityGroup);
            }

            this.Stats.IncrementStat("TotalDamageActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            if (line.Hit) {
                this.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subMonsterGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                if (line.Crit) {
                    this.Stats.IncrementStat("DamageCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageCritHit");
                    subMonsterGroup.Stats.IncrementStat("DamageCritHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageCritHit");
                    this.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    if (line.Modifier != 0) {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageCritMod";
                        this.Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
                else {
                    this.Stats.IncrementStat("DamageRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageRegHit");
                    subMonsterGroup.Stats.IncrementStat("DamageRegHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageRegHit");
                    this.Stats.IncrementStat("RegularDamage", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    if (line.Modifier != 0) {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageRegMod";
                        this.Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
            }
            else {
                this.Stats.IncrementStat("DamageRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageRegMiss");
                subMonsterGroup.Stats.IncrementStat("DamageRegMiss");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageRegMiss");
            }

            foreach (PropertyInfo stat in fields.Where(stat => LD.Contains(stat.Name)).Where(stat => Equals(stat.GetValue(line), true))) {
                var regStat = $"Damage{stat.Name}";
                this.Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subMonsterGroup.Stats.IncrementStat(regStat);
                subMonsterAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0) {
                    continue;
                }

                var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                var modStat = $"Damage{stat.Name}Mod";
                this.Stats.IncrementStat(modStat, mod);
                subAbilityGroup.Stats.IncrementStat(modStat, mod);
                subMonsterGroup.Stats.IncrementStat(modStat, mod);
                subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
            }
        }
    }
}