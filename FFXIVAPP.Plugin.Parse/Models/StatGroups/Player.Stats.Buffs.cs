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

using System;
using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetBuff(Line line)
        {
            if (Name == Constants.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            var abilityGroup = GetGroup("BuffByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(BuffStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("BuffToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(BuffStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("BuffToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(BuffStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalBuffTime");
            subAbilityGroup.Stats.IncrementStat("TotalBuffTime");
            subPlayerGroup.Stats.IncrementStat("TotalBuffTime");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalBuffTime");
            AdjustBuffTime(this);
            AdjustBuffTime(subAbilityGroup);
            AdjustBuffTime(subPlayerGroup);
            AdjustBuffTime(subPlayerAbilityGroup);
        }

        private void AdjustBuffTime(StatGroup statGroup)
        {
            var timeSpan = TimeSpan.FromSeconds(statGroup.Stats.GetStatValue("TotalBuffTime"));
            statGroup.Stats.GetStat("TotalBuffHours")
                     .Value = timeSpan.Hours;
            statGroup.Stats.GetStat("TotalBuffMinutes")
                     .Value = timeSpan.Minutes;
            statGroup.Stats.GetStat("TotalBuffSeconds")
                     .Value = timeSpan.Seconds;
        }
    }
}
