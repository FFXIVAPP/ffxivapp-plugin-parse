// FFXIVAPP.Plugin.Parse ~ Player.Stats.HealingOverHealing.cs
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
        public void SetHealingOverHealing(Line line)
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

            var abilityGroup = GetGroup("HealingOverHealingByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(HealingOverHealingStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("HealingOverHealingToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(HealingOverHealingStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("HealingOverHealingToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(HealingOverHealingStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("HealingOverHealingCritHit");
                Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverHealingCritHit");
                subAbilityGroup.Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverHealingCritHit");
                subPlayerGroup.Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverHealingCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverHealingCritMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else
            {
                Stats.IncrementStat("HealingOverHealingRegHit");
                Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverHealingRegHit");
                subAbilityGroup.Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverHealingRegHit");
                subPlayerGroup.Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverHealingRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverHealingRegMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
        }

        private void SetupHealingOverHealing(Line line, HealingType healingType)
        {
            var cleanedAction = Regex.Replace(line.Action, @" \[.+\]", string.Empty);
            switch (healingType)
            {
                case HealingType.Normal:
                    line.Action = $"{cleanedAction} [∞]";
                    SetHealingOverHealing(line);
                    break;
                case HealingType.HealingOverTime:
                    line.Action = $"{cleanedAction} [•][∞]";
                    SetHealingOverHealing(line);
                    break;
            }
        }
    }
}
