﻿// FFXIVAPP.Plugin.Parse
// Player.Stats.Healing.cs
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
