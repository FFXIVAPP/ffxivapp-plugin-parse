// FFXIVAPP.Plugin.Parse ~ Player.Stats.Healing.cs
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
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
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
        public void SetHealing(Line line)
        {
            if (Name == Constants.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            Last20HealingActions.Add(new LineHistory(line));
            if (Last20HealingActions.Count > 20)
            {
                Last20HealingActions.RemoveAt(0);
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
                var cleanedName = Regex.Replace(line.Target, @"\[[\w]+\]", string.Empty)
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
                Logging.Log(Logger, new LogItem(ex, true));
            }

            var abilityGroup = GetGroup("HealingByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(HealingStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("HealingToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(HealingStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("HealingToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(HealingStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalHealingActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("HealingCritHit");
                Stats.IncrementStat("CriticalHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingCritHit");
                subAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingCritHit");
                subPlayerGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingCritMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else
            {
                Stats.IncrementStat("HealingRegHit");
                Stats.IncrementStat("RegularHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingRegHit");
                subAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingRegHit");
                subPlayerGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingRegMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }

            #region Handle Mitigaged (With Initial Healing)

            if (MagicBarrierHelper.Adloquium.Any(action => String.Equals(line.Action, action, Constants.InvariantComparer)))
            {
                line.Amount = originalAmount;
                SetupHealingMitigated(line, "adloquium");
            }
            if (MagicBarrierHelper.Succor.Any(action => String.Equals(line.Action, action, Constants.InvariantComparer)))
            {
                line.Amount = originalAmount;
                SetupHealingMitigated(line, "succor");
            }

            #endregion

            #region OverHealing Handler

            if (unusedAmount <= 0)
            {
                return;
            }

            line.Amount = unusedAmount;
            SetupHealingOverHealing(line, HealingType.Normal);

            #endregion
        }
    }
}
