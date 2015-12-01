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
using System.Text.RegularExpressions;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetHealingOverTime(Line line)
        {
            if (Name == Constants.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            var currentHealing = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalAmount(line.Amount, .5) : 0 : line.Amount;
            if (currentHealing > 0)
            {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentHealing);
            }

            var unusedAmount = 0;
            var originalAmount = line.Amount;
            // get curable of target
            try
            {
                var cleanedName = Regex.Replace(line.Target, @"\[[\w]+\]", "")
                                       .Trim();
                var curable = Controller.Timeline.TryGetPlayerCurable(cleanedName);
                if (line.Amount > curable)
                {
                    unusedAmount = (int) (line.Amount - curable);
                    line.Amount = curable;
                }
            }
            catch (Exception ex)
            {
            }

            var abilityGroup = GetGroup("HealingOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(HealingOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("HealingOverTimeToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(HealingOverTimeStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("HealingOverTimeToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(HealingOverTimeStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("HealingOverTimeCritHit");
                Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverTimeCritHit");
                subPlayerGroup.Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverTimeCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverTimeCritMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else
            {
                Stats.IncrementStat("HealingOverTimeRegHit");
                Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverTimeRegHit");
                subPlayerGroup.Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverTimeRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverTimeRegMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }

            #region OverHealing Handler

            if (unusedAmount <= 0)
            {
                return;
            }

            line.Amount = originalAmount;
            SetupHealingOverHealing(line, HealingType.HealingOverTime);

            #endregion
        }
    }
}
