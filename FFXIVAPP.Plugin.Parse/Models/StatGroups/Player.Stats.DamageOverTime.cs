// FFXIVAPP.Plugin.Parse
// FFXIVAPP & Related Plugins/Modules
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
        public void SetDamageOverTime(Line line)
        {
            if (Name == Constants.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            if ((LimitBreaks.IsLimit(line.Action)) && Settings.Default.IgnoreLimitBreaks)
            {
                return;
            }

            var currentDamage = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalAmount(line.Amount, .5) : 0 : line.Amount;
            if (currentDamage > 0)
            {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentDamage);
            }

            var abilityGroup = GetGroup("DamageOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(DamageOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var monsterGroup = GetGroup("DamageOverTimeToMonsters");
            StatGroup subMonsterGroup;
            if (!monsterGroup.TryGetGroup(line.Target, out subMonsterGroup))
            {
                subMonsterGroup = new StatGroup(line.Target);
                subMonsterGroup.Stats.AddStats(DamageOverTimeStatList(null));
                monsterGroup.AddGroup(subMonsterGroup);
            }
            var monsters = subMonsterGroup.GetGroup("DamageOverTimeToMonstersByAction");
            StatGroup subMonsterAbilityGroup;
            if (!monsters.TryGetGroup(line.Action, out subMonsterAbilityGroup))
            {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(DamageOverTimeStatList(subMonsterGroup, true));
                monsters.AddGroup(subMonsterAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subMonsterGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("DamageOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                subMonsterGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subMonsterGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
            }
            else
            {
                Stats.IncrementStat("DamageOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                subMonsterGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subMonsterGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
            }
        }
    }
}
