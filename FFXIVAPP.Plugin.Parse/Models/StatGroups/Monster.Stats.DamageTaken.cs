// FFXIVAPP.Plugin.Parse ~ Monster.Stats.DamageTaken.cs
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

using System.Linq;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models.Stats;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageTaken(Line line)
        {
            if (LimitBreaks.IsLimit(line.Action) && Settings.Default.IgnoreLimitBreaks)
            {
                return;
            }

            Last20DamageTakenActions.Add(new LineHistory(line));
            if (Last20DamageTakenActions.Count > 20)
            {
                Last20DamageTakenActions.RemoveAt(0);
            }

            //LineHistory.Add(new LineHistory(line));

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
            var damageGroup = GetGroup("DamageTakenByPlayers");
            StatGroup subPlayerGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Source);
                subPlayerGroup.Stats.AddStats(DamageTakenStatList(null));
                damageGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("DamageTakenByPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(DamageTakenStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            if (line.Hit)
            {
                Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subPlayerGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                if (line.Crit)
                {
                    Stats.IncrementStat("DamageTakenCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subPlayerGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageTakenCritMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
                else
                {
                    Stats.IncrementStat("DamageTakenRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subPlayerGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                        var modStat = "DamageTakenRegMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
            }
            else
            {
                Stats.IncrementStat("DamageTakenRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subPlayerGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                var regStat = $"DamageTaken{stat.Name}";
                Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subPlayerGroup.Stats.IncrementStat(regStat);
                subPlayerAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0)
                {
                    continue;
                }
                var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                var modStat = $"DamageTaken{stat.Name}Mod";
                Stats.IncrementStat(modStat, mod);
                subAbilityGroup.Stats.IncrementStat(modStat, mod);
                subPlayerGroup.Stats.IncrementStat(modStat, mod);
                subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
            }
        }
    }
}
