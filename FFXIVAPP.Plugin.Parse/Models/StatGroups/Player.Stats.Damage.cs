﻿// FFXIVAPP.Plugin.Parse
// Player.Stats.Damage.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Linq;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models.Stats;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamage(Line line)
        {
            if (Name == Constants.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            Last20DamageActions.Add(new LineHistory(line));
            if (Last20DamageActions.Count > 20)
            {
                Last20DamageActions.RemoveAt(0);
            }

            if ((LimitBreaks.IsLimit(line.Action)) && Settings.Default.IgnoreLimitBreaks)
            {
                return;
            }

            var fields = line.GetType()
                             .GetProperties();

            var currentDamage = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalAmount(line.Amount, .5) : 0 : line.Amount;
            if (currentDamage > 0)
            {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentDamage);
            }

            var abilityGroup = GetGroup("DamageByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(DamageStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var monsterGroup = GetGroup("DamageToMonsters");
            StatGroup subMonsterGroup;
            if (!monsterGroup.TryGetGroup(line.Target, out subMonsterGroup))
            {
                subMonsterGroup = new StatGroup(line.Target);
                subMonsterGroup.Stats.AddStats(DamageStatList(null));
                monsterGroup.AddGroup(subMonsterGroup);
            }
            var monsters = subMonsterGroup.GetGroup("DamageToMonstersByAction");
            StatGroup subMonsterAbilityGroup;
            if (!monsters.TryGetGroup(line.Action, out subMonsterAbilityGroup))
            {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(DamageStatList(subMonsterGroup, true));
                monsters.AddGroup(subMonsterAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            if (line.Hit)
            {
                Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subMonsterGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                if (line.Crit)
                {
                    Stats.IncrementStat("DamageCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageCritHit");
                    subMonsterGroup.Stats.IncrementStat("DamageCritHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageCritHit");
                    Stats.IncrementStat("CriticalDamage", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageCritMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
                else
                {
                    Stats.IncrementStat("DamageRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageRegHit");
                    subMonsterGroup.Stats.IncrementStat("DamageRegHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageRegHit");
                    Stats.IncrementStat("RegularDamage", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageRegMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
            }
            else
            {
                Stats.IncrementStat("DamageRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageRegMiss");
                subMonsterGroup.Stats.IncrementStat("DamageRegMiss");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageRegMiss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                var regStat = String.Format("Damage{0}", stat.Name);
                Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subMonsterGroup.Stats.IncrementStat(regStat);
                subMonsterAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0)
                {
                    continue;
                }
                var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                var modStat = String.Format("Damage{0}Mod", stat.Name);
                Stats.IncrementStat(modStat, mod);
                subAbilityGroup.Stats.IncrementStat(modStat, mod);
                subMonsterGroup.Stats.IncrementStat(modStat, mod);
                subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
            }
        }
    }
}
