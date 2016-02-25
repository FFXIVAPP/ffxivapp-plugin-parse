// FFXIVAPP.Plugin.Parse ~ Monster.Stats.DamageTakenOverTime.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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

using FFXIVAPP.Plugin.Parse.Models.Stats;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageTakenOverTime(Line line)
        {
            if ((LimitBreaks.IsLimit(line.Action)) && Settings.Default.IgnoreLimitBreaks)
            {
                return;
            }

            //LineHistory.Add(new LineHistory(line));

            var abilityGroup = GetGroup("DamageTakenOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(DamageTakenOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var damageGroup = GetGroup("DamageTakenOverTimeByPlayers");
            StatGroup subPlayerGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Source);
                subPlayerGroup.Stats.AddStats(DamageTakenOverTimeStatList(null));
                damageGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("DamageTakenOverTimeByPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(DamageTakenOverTimeStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subPlayerGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
            }
            else
            {
                Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subPlayerGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
            }
        }
    }
}
