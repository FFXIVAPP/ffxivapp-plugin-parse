// FFXIVAPP.Plugin.Parse ~ Player.Stats.DamageTaken.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageTaken(Line line)
        {
            if (Name == Constants.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            Last20DamageTakenActions.Add(new LineHistory(line));
            if (Last20DamageTakenActions.Count > 20)
            {
                Last20DamageTakenActions.RemoveAt(0);
            }

            var fields = line.GetType()
                             .GetProperties();

            var abilityGroup = GetGroup("DamageTakenByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(DamageTakenStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var damageGroup = GetGroup("DamageTakenByMonsters");
            StatGroup subMonsterGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subMonsterGroup))
            {
                subMonsterGroup = new StatGroup(line.Source);
                subMonsterGroup.Stats.AddStats(DamageTakenStatList(null));
                damageGroup.AddGroup(subMonsterGroup);
            }
            var abilities = subMonsterGroup.GetGroup("DamageTakenByMonstersByAction");
            StatGroup subMonsterAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subMonsterAbilityGroup))
            {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(DamageTakenStatList(subMonsterGroup, true));
                abilities.AddGroup(subMonsterAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            if (line.Hit)
            {
                Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subMonsterGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                if (line.Crit)
                {
                    Stats.IncrementStat("DamageTakenCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subMonsterGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageTakenCritMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
                else
                {
                    Stats.IncrementStat("DamageTakenRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subMonsterGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageTakenRegMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
            }
            else
            {
                Stats.IncrementStat("DamageTakenRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subMonsterGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                var regStat = String.Format("DamageTaken{0}", stat.Name);
                Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subMonsterGroup.Stats.IncrementStat(regStat);
                subMonsterAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0)
                {
                    continue;
                }
                var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                var modStat = String.Format("DamageTaken{0}Mod", stat.Name);
                Stats.IncrementStat(modStat, mod);
                subAbilityGroup.Stats.IncrementStat(modStat, mod);
                subMonsterGroup.Stats.IncrementStat(modStat, mod);
                subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
            }
        }
    }
}
