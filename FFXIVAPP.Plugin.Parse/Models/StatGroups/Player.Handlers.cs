// FFXIVAPP.Plugin.Parse ~ Player.Handlers.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Memory.Core;
using FFXIVAPP.Memory.Helpers;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Properties;
using FFXIVAPP.Plugin.Parse.ViewModels;

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Player
    {
        #region Damage Over Time

        /// <summary>
        /// </summary>
        /// <param name="statusEntriesMonsters"></param>
        private void ProcessDamageOverTime(IEnumerable<StatusEntry> statusEntriesMonsters)
        {
            foreach (var statusEntry in statusEntriesMonsters)
            {
                try
                {
                    var statusInfo = StatusEffectHelper.StatusInfo((uint) statusEntry.StatusID);
                    var statusKey = statusInfo.Name.English;
                    switch (Constants.GameLanguage)
                    {
                        case "French":
                            statusKey = statusInfo.Name.French;
                            break;
                        case "Japanese":
                            statusKey = statusInfo.Name.Japanese;
                            break;
                        case "German":
                            statusKey = statusInfo.Name.German;
                            break;
                        case "Chinese":
                            statusKey = statusInfo.Name.Chinese;
                            break;
                    }
                    if (String.IsNullOrWhiteSpace(statusKey))
                    {
                        continue;
                    }
                    var difference = (60 - NPCEntry.Level);
                    if (difference <= 0)
                    {
                        difference = 10;
                    }
                    var amount = NPCEntry.Level / (difference * .025);
                    var key = statusKey;
                    XOverTimeAction actionData = null;
                    foreach (var damageOverTimeAction in DamageOverTimeHelper.PlayerActions.ToList()
                                                                             .Where(d => String.Equals(d.Key, key, Constants.InvariantComparer)))
                    {
                        actionData = damageOverTimeAction.Value;
                    }
                    if (actionData == null)
                    {
                        continue;
                    }
                    var zeroFoundInList = false;
                    var bio = Regex.IsMatch(key, @"(バイオ|bactérie|bio|毒菌)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    var thunder = Regex.IsMatch(key, @"(サンダ|foudre|blitz|thunder|闪雷)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    var lastDamageAmountByActions = ParseHelper.LastAmountByAction.GetPlayer(Name)
                                                               .ToList();
                    var resolvedPotency = 80;
                    var thunderDuration = 24;
                    double originalThunderDamage = 0;
                    foreach (var lastDamageAmountByAction in lastDamageAmountByActions)
                    {
                        if (thunder)
                        {
                            var found = false;
                            var thunderActions = DamageOverTimeHelper.ThunderActions;
                            var action = lastDamageAmountByAction;
                            if (thunderActions["III"]
                                .Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                found = true;
                                thunderDuration = DamageOverTimeHelper.PlayerActions["thunder iii"]
                                                                      .Duration;
                                originalThunderDamage = action.Value;
                                amount = (action.Value / DamageOverTimeHelper.PlayerActions["thunder iii"]
                                                                             .ActionPotency) * 30;
                            }
                            if (thunderActions["II"]
                                .Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                found = true;
                                thunderDuration = DamageOverTimeHelper.PlayerActions["thunder ii"]
                                                                      .Duration;
                                originalThunderDamage = action.Value;
                                amount = (action.Value / DamageOverTimeHelper.PlayerActions["thunder ii"]
                                                                             .ActionPotency) * 30;
                            }
                            if (thunderActions["I"]
                                .Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                found = true;
                                thunderDuration = DamageOverTimeHelper.PlayerActions["thunder"]
                                                                      .Duration;
                                originalThunderDamage = action.Value;
                                amount = action.Value;
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                        if (bio)
                        {
                            var found = false;
                            var ruinActions = DamageOverTimeHelper.RuinActions;
                            var action = lastDamageAmountByAction;
                            if (ruinActions["II"]
                                .Any(ruinAction => String.Equals(action.Key, ruinAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                amount = action.Value;
                            }
                            if (ruinActions["I"]
                                .Any(ruinAction => String.Equals(action.Key, ruinAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                amount = action.Value;
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                        if (String.Equals(lastDamageAmountByAction.Key, key, Constants.InvariantComparer))
                        {
                            amount = lastDamageAmountByAction.Value;
                            break;
                        }
                    }
                    statusKey = String.Format("{0} [•]", statusKey);
                    if (amount == 0)
                    {
                        amount = 75;
                    }
                    resolvedPotency = zeroFoundInList ? resolvedPotency : bio ? resolvedPotency : actionData.ActionPotency;
                    var tickDamage = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                    if (actionData.HasNoInitialResult && !zeroFoundInList)
                    {
                        var nonZeroActions = lastDamageAmountByActions.Where(d => !d.Key.Contains("•"));
                        var keyValuePairs = nonZeroActions as IList<KeyValuePair<string, double>> ?? nonZeroActions.ToList();
                        double damage = 0;
                        switch (bio)
                        {
                            case true:
                                damage = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                                break;
                            case false:
                                if (keyValuePairs.Any())
                                {
                                    amount = keyValuePairs.Sum(action => action.Value);
                                    amount = amount / keyValuePairs.Count();
                                }
                                damage = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                                break;
                        }
                        tickDamage = damage > 0 ? damage : tickDamage;
                    }
                    if (originalThunderDamage > 300 && thunder)
                    {
                        tickDamage = Math.Ceiling(originalThunderDamage / (thunderDuration + 3));
                    }
                    var line = new Line
                    {
                        Action = statusKey,
                        Amount = tickDamage,
                        EventDirection = EventDirection.Unknown,
                        EventType = EventType.Damage,
                        EventSubject = EventSubject.Unknown,
                        Source = Name,
                        SourceEntity = NPCEntry,
                        Target = statusEntry.TargetName,
                        XOverTime = true
                    };
                    Controller.Timeline.FightingRightNow = true;
                    Controller.Timeline.FightingTimer.Stop();
                    Controller.Timeline.StoreHistoryTimer.Stop();
                    DispatcherHelper.Invoke(delegate
                    {
                        line.Hit = true;
                        var currentCritPercent = Stats.GetStatValue("DamageCritPercent");
                        if (new Random().NextDouble() * 3 < currentCritPercent)
                        {
                            line.Crit = true;
                            line.Amount = line.Amount * 1.5;
                        }
                        Controller.Timeline.GetSetPlayer(line.Source)
                                  .SetDamageOverTime(line);
                        Controller.Timeline.GetSetMonster(line.Target)
                                  .SetDamageTakenOverTime(line);
                    });
                }
                catch (Exception ex)
                {
                }
            }
            Controller.Timeline.FightingTimer.Start();
            Controller.Timeline.StoreHistoryTimer.Start();
        }

        #endregion

        #region Healing Over Time

        /// <summary>
        /// </summary>
        /// <param name="statusEntriesPlayers"></param>
        private void ProcessHealingOverTime(IEnumerable<StatusEntry> statusEntriesPlayers)
        {
            foreach (var statusEntry in statusEntriesPlayers)
            {
                try
                {
                    var statusInfo = StatusEffectHelper.StatusInfo((uint) statusEntry.StatusID);
                    var statusKey = statusInfo.Name.English;
                    switch (Constants.GameLanguage)
                    {
                        case "French":
                            statusKey = statusInfo.Name.French;
                            break;
                        case "Japanese":
                            statusKey = statusInfo.Name.Japanese;
                            break;
                        case "German":
                            statusKey = statusInfo.Name.German;
                            break;
                        case "Chinese":
                            statusKey = statusInfo.Name.Chinese;
                            break;
                    }
                    if (String.IsNullOrWhiteSpace(statusKey))
                    {
                        continue;
                    }
                    var amount = NPCEntry.Level / ((60 - NPCEntry.Level) * .025);
                    var key = statusKey;
                    XOverTimeAction actionData = null;
                    foreach (var healingOverTimeAction in HealingOverTimeHelper.PlayerActions.ToList()
                                                                               .Where(d => String.Equals(d.Key, key, Constants.InvariantComparer)))
                    {
                        actionData = healingOverTimeAction.Value;
                    }
                    if (actionData == null)
                    {
                        continue;
                    }
                    var zeroFoundInList = false;
                    var regen = Regex.IsMatch(key, @"(リジェネ|récup|regen|再生|whispering|murmure|erhebendes|光の囁き|日光的低语)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    var healingHistoryList = ParseHelper.LastAmountByAction.GetPlayer(Name)
                                                        .ToList();
                    var resolvedPotency = 350;
                    foreach (var healingAction in healingHistoryList)
                    {
                        if (regen)
                        {
                            var found = false;
                            var cureActions = HealingOverTimeHelper.CureActions;
                            var medicaActions = HealingOverTimeHelper.MedicaActions;
                            var action = healingAction;
                            if (cureActions["III"]
                                .Any(cureAction => String.Equals(action.Key, cureAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                resolvedPotency = 550;
                            }
                            if (cureActions["II"]
                                .Any(cureAction => String.Equals(action.Key, cureAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                resolvedPotency = 650;
                            }
                            if (cureActions["I"]
                                .Any(cureAction => String.Equals(action.Key, cureAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                resolvedPotency = 400;
                            }
                            if (medicaActions["II"]
                                .Any(medicaAction => String.Equals(action.Key, medicaAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                resolvedPotency = 200;
                            }
                            if (medicaActions["I"]
                                .Any(medicaAction => String.Equals(action.Key, medicaAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                resolvedPotency = 300;
                            }
                            if (found)
                            {
                                if (action.Value > 0)
                                {
                                    amount = action.Value;
                                }
                                break;
                            }
                        }
                        if (String.Equals(healingAction.Key, key, Constants.InvariantComparer))
                        {
                            amount = healingAction.Value;
                            break;
                        }
                    }
                    statusKey = String.Format("{0} [•]", statusKey);
                    if (amount == 0)
                    {
                        amount = 75;
                    }
                    resolvedPotency = zeroFoundInList ? resolvedPotency : regen ? resolvedPotency : actionData.ActionPotency;
                    var tickHealing = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                    if (actionData.HasNoInitialResult && !zeroFoundInList)
                    {
                        var nonZeroActions = healingHistoryList.Where(d => !d.Key.Contains("•"));
                        var keyValuePairs = nonZeroActions as IList<KeyValuePair<string, double>> ?? nonZeroActions.ToList();
                        double healing = 0;
                        switch (regen)
                        {
                            case true:
                                healing = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                                break;
                            case false:
                                if (keyValuePairs.Any())
                                {
                                    amount = keyValuePairs.Sum(action => action.Value);
                                    amount = amount / keyValuePairs.Count();
                                }
                                healing = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                                break;
                        }
                        tickHealing = healing > 0 ? healing : tickHealing;
                    }
                    var line = new Line
                    {
                        Action = statusKey,
                        Amount = tickHealing,
                        EventDirection = EventDirection.Unknown,
                        EventType = EventType.Cure,
                        EventSubject = EventSubject.Unknown,
                        Source = Name,
                        SourceEntity = NPCEntry,
                        XOverTime = true
                    };
                    try
                    {
                        var players = Controller.Timeline.Party.ToList();
                        var entry = statusEntry;
                        foreach (var player in players.Where(player => player.Name.Contains(entry.TargetName)))
                        {
                            line.Target = player.Name;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (String.IsNullOrWhiteSpace(line.Target))
                    {
                        line.Target = String.Format("[???] {0}", statusEntry.TargetName);
                    }
                    Controller.Timeline.FightingRightNow = true;
                    Controller.Timeline.FightingTimer.Stop();
                    switch (Settings.Default.StoreHistoryEvent)
                    {
                        case "Any":
                            Controller.Timeline.StoreHistoryTimer.Stop();
                            break;
                    }
                    DispatcherHelper.Invoke(delegate
                    {
                        line.Hit = true;
                        // resolve player hp each tick to ensure they are not at max
                        try
                        {
                            var players = XIVInfoViewModel.Instance.CurrentPCs.Select(entity => entity.Value)
                                                          .ToList();
                            if (!players.Any())
                            {
                                return;
                            }
                            foreach (var actorEntity in players)
                            {
                                var playerName = actorEntity.Name;
                                Controller.Timeline.TrySetPlayerCurable(playerName, actorEntity.HPMax - actorEntity.HPCurrent);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        var currentCritPercent = Stats.GetStatValue("HealingCritPercent");
                        if (new Random().NextDouble() * 3 < currentCritPercent)
                        {
                            line.Crit = true;
                            line.Amount = line.Amount * 1.5;
                        }
                        Controller.Timeline.GetSetPlayer(line.Source)
                                  .SetHealingOverTime(line);
                    });
                }
                catch (Exception ex)
                {
                }
            }
            Controller.Timeline.FightingTimer.Start();
            switch (Settings.Default.StoreHistoryEvent)
            {
                case "Any":
                    Controller.Timeline.StoreHistoryTimer.Start();
                    break;
            }
        }

        #endregion

        #region Buff Tracker

        private void ProcessBuffs(IEnumerable<StatusEntry> statusEntriesPlayers)
        {
            foreach (var statusEntry in statusEntriesPlayers)
            {
                try
                {
                    var statusInfo = StatusEffectHelper.StatusInfo((uint) statusEntry.StatusID);
                    var statusKey = statusInfo.Name.English;
                    switch (Constants.GameLanguage)
                    {
                        case "French":
                            statusKey = statusInfo.Name.French;
                            break;
                        case "Japanese":
                            statusKey = statusInfo.Name.Japanese;
                            break;
                        case "German":
                            statusKey = statusInfo.Name.German;
                            break;
                        case "Chinese":
                            statusKey = statusInfo.Name.Chinese;
                            break;
                    }
                    if (String.IsNullOrWhiteSpace(statusKey))
                    {
                        continue;
                    }
                    var line = new Line
                    {
                        Action = statusKey,
                        Amount = 0,
                        EventDirection = EventDirection.Unknown,
                        EventType = EventType.Unknown,
                        EventSubject = EventSubject.Unknown,
                        Source = Name,
                        SourceEntity = NPCEntry
                    };
                    try
                    {
                        var players = Controller.Timeline.Party.ToList();
                        var entry = statusEntry;
                        foreach (var player in players.Where(player => player.Name.Contains(entry.TargetName)))
                        {
                            line.Target = player.Name;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (String.IsNullOrWhiteSpace(line.Target))
                    {
                        line.Target = String.Format("[???] {0}", statusEntry.TargetName);
                    }
                    DispatcherHelper.Invoke(() => Controller.Timeline.GetSetPlayer(line.Source)
                                                            .SetBuff(line));
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion
    }
}
