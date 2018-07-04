// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.Handlers.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Player.Handlers.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.Properties;
    using FFXIVAPP.Plugin.Parse.ViewModels;

    using Sharlayan.Core;
    using Sharlayan.Utilities;

    using ActionItem = Sharlayan.Models.XIVDatabase.ActionItem;

    public partial class Player {
        private void ProcessBuffs(IEnumerable<StatusItem> statusEntriesPlayers) {
            foreach (StatusItem statusEntry in statusEntriesPlayers) {
                try {
                    Sharlayan.Models.XIVDatabase.StatusItem statusInfo = StatusEffectLookup.GetStatusInfo((uint) statusEntry.StatusID);
                    var statusKey = statusInfo.Name.English;
                    switch (Constants.GameLanguage) {
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
                        case "Korean":
                            statusKey = statusInfo.Name.Korean;
                            break;
                    }

                    if (string.IsNullOrWhiteSpace(statusKey)) {
                        continue;
                    }

                    var line = new Line {
                        Action = statusKey,
                        Amount = 0,
                        EventDirection = EventDirection.Unknown,
                        EventType = EventType.Unknown,
                        EventSubject = EventSubject.Unknown,
                        Source = this.Name,
                        SourceEntity = this.NPCEntry
                    };
                    try {
                        List<StatGroup> players = Controller.Timeline.Party.ToList();
                        StatusItem entry = statusEntry;
                        foreach (StatGroup player in players.Where(player => player.Name.Contains(entry.TargetName))) {
                            line.Target = player.Name;
                            break;
                        }
                    }
                    catch (Exception ex) {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }

                    if (string.IsNullOrWhiteSpace(line.Target)) {
                        line.Target = $"[???] {statusEntry.TargetName}";
                    }

                    DispatcherHelper.Invoke(() => Controller.Timeline.GetSetPlayer(line.Source).SetBuff(line));
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="statusEntriesMonsters"></param>
        private void ProcessDamageOverTime(IEnumerable<StatusItem> statusEntriesMonsters) {
            foreach (StatusItem statusEntry in statusEntriesMonsters) {
                try {
                    Sharlayan.Models.XIVDatabase.StatusItem statusInfo = StatusEffectLookup.GetStatusInfo((uint) statusEntry.StatusID);
                    var statusKey = statusInfo.Name.English;
                    switch (Constants.GameLanguage) {
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
                        case "Korean":
                            statusKey = statusInfo.Name.Korean;
                            break;
                    }

                    if (string.IsNullOrWhiteSpace(statusKey)) {
                        continue;
                    }

                    var difference = 60 - this.NPCEntry.Level;
                    if (difference <= 0) {
                        difference = 10;
                    }

                    var amount = this.NPCEntry.Level / (difference * .025);
                    var key = statusKey;
                    ActionItem actionData = null;
                    foreach (KeyValuePair<string, ActionItem> damageOverTimeAction in DamageOverTimeHelper.PlayerActions.ToList().Where(d => string.Equals(d.Key, key, Constants.InvariantComparer))) {
                        actionData = damageOverTimeAction.Value;
                    }

                    if (actionData == null) {
                        continue;
                    }

                    var zeroFoundInList = false;
                    var bio = Regex.IsMatch(key, @"(バイオ|bactérie|bio|毒菌)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    var thunder = Regex.IsMatch(key, @"(サンダ|foudre|blitz|thunder|闪雷)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    List<KeyValuePair<string, double>> lastDamageAmountByActions = ParseHelper.LastAmountByAction.GetPlayer(this.Name).ToList();
                    var resolvedPotency = 80;
                    var thunderDuration = 24.0;
                    double originalThunderDamage = 0;
                    foreach (KeyValuePair<string, double> lastDamageAmountByAction in lastDamageAmountByActions) {
                        if (thunder) {
                            var found = false;
                            Dictionary<string, List<string>> thunderActions = DamageOverTimeHelper.ThunderActions;
                            KeyValuePair<string, double> action = lastDamageAmountByAction;
                            if (thunderActions["III"].Any(thunderAction => string.Equals(action.Key, thunderAction, Constants.InvariantComparer))) {
                                found = true;
                                thunderDuration = (double) DamageOverTimeHelper.PlayerActions["thunder iii"].Duration;
                                originalThunderDamage = action.Value;
                                amount = action.Value / DamageOverTimeHelper.PlayerActions["thunder iii"].Potency * 30;
                            }

                            if (thunderActions["II"].Any(thunderAction => string.Equals(action.Key, thunderAction, Constants.InvariantComparer))) {
                                found = true;
                                thunderDuration = (double) DamageOverTimeHelper.PlayerActions["thunder ii"].Duration;
                                originalThunderDamage = action.Value;
                                amount = action.Value / DamageOverTimeHelper.PlayerActions["thunder ii"].Potency * 30;
                            }

                            if (thunderActions["I"].Any(thunderAction => string.Equals(action.Key, thunderAction, Constants.InvariantComparer))) {
                                found = true;
                                thunderDuration = (double) DamageOverTimeHelper.PlayerActions["thunder"].Duration;
                                originalThunderDamage = action.Value;
                                amount = action.Value;
                            }

                            if (found) {
                                break;
                            }
                        }

                        if (bio) {
                            var found = false;
                            Dictionary<string, List<string>> ruinActions = DamageOverTimeHelper.RuinActions;
                            KeyValuePair<string, double> action = lastDamageAmountByAction;
                            if (ruinActions["II"].Any(ruinAction => string.Equals(action.Key, ruinAction, Constants.InvariantComparer))) {
                                found = zeroFoundInList = true;
                                amount = action.Value;
                            }

                            if (ruinActions["I"].Any(ruinAction => string.Equals(action.Key, ruinAction, Constants.InvariantComparer))) {
                                found = zeroFoundInList = true;
                                amount = action.Value;
                            }

                            if (found) {
                                break;
                            }
                        }

                        if (string.Equals(lastDamageAmountByAction.Key, key, Constants.InvariantComparer)) {
                            amount = lastDamageAmountByAction.Value;
                            break;
                        }
                    }

                    statusKey = $"{statusKey} [•]";
                    if (amount == 0) {
                        amount = 75;
                    }

                    resolvedPotency = zeroFoundInList
                                          ? resolvedPotency
                                          : bio
                                              ? resolvedPotency
                                              : actionData.Potency;
                    var tickDamage = Math.Ceiling(amount / resolvedPotency * actionData.OverTimePotency / 3);
                    if (actionData.HasNoInitialResult && !zeroFoundInList) {
                        IEnumerable<KeyValuePair<string, double>> nonZeroActions = lastDamageAmountByActions.Where(d => !d.Key.Contains("•"));
                        IList<KeyValuePair<string, double>> keyValuePairs = nonZeroActions as IList<KeyValuePair<string, double>> ?? nonZeroActions.ToList();
                        double damage = 0;
                        switch (bio) {
                            case true:
                                damage = Math.Ceiling(amount / resolvedPotency * actionData.OverTimePotency / 3);
                                break;
                            case false:
                                if (keyValuePairs.Any()) {
                                    amount = keyValuePairs.Sum(action => action.Value);
                                    amount = amount / keyValuePairs.Count();
                                }

                                damage = Math.Ceiling(amount / resolvedPotency * actionData.OverTimePotency / 3);
                                break;
                        }

                        tickDamage = damage > 0
                                         ? damage
                                         : tickDamage;
                    }

                    if (originalThunderDamage > 300 && thunder) {
                        tickDamage = Math.Ceiling(originalThunderDamage / (thunderDuration + 3));
                    }

                    var line = new Line {
                        Action = statusKey,
                        Amount = tickDamage,
                        EventDirection = EventDirection.Unknown,
                        EventType = EventType.Damage,
                        EventSubject = EventSubject.Unknown,
                        Source = this.Name,
                        SourceEntity = this.NPCEntry,
                        Target = statusEntry.TargetName,
                        XOverTime = true
                    };
                    Controller.Timeline.FightingRightNow = true;
                    Controller.Timeline.FightingTimer.Stop();
                    Controller.Timeline.StoreHistoryTimer.Stop();
                    DispatcherHelper.Invoke(
                        delegate {
                            line.Hit = true;
                            var currentCritPercent = this.Stats.GetStatValue("DamageCritPercent");
                            if (new Random().NextDouble() * 3 < currentCritPercent) {
                                line.Crit = true;
                                line.Amount = line.Amount * 1.5;
                            }

                            Controller.Timeline.GetSetPlayer(line.Source).SetDamageOverTime(line);
                            Controller.Timeline.GetSetMonster(line.Target).SetDamageTakenOverTime(line);
                        });
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
            }

            Controller.Timeline.FightingTimer.Start();
            Controller.Timeline.StoreHistoryTimer.Start();
        }

        /// <summary>
        /// </summary>
        /// <param name="statusEntriesPlayers"></param>
        private void ProcessHealingOverTime(IEnumerable<StatusItem> statusEntriesPlayers) {
            foreach (StatusItem statusEntry in statusEntriesPlayers) {
                try {
                    Sharlayan.Models.XIVDatabase.StatusItem statusInfo = StatusEffectLookup.GetStatusInfo((uint) statusEntry.StatusID);
                    var statusKey = statusInfo.Name.English;
                    switch (Constants.GameLanguage) {
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
                        case "Korean":
                            statusKey = statusInfo.Name.Korean;
                            break;
                    }

                    if (string.IsNullOrWhiteSpace(statusKey)) {
                        continue;
                    }

                    var amount = this.NPCEntry.Level / ((60 - this.NPCEntry.Level) * .025);
                    var key = statusKey;
                    ActionItem actionData = null;
                    foreach (KeyValuePair<string, ActionItem> healingOverTimeAction in HealingOverTimeHelper.PlayerActions.ToList().Where(d => string.Equals(d.Key, key, Constants.InvariantComparer))) {
                        actionData = healingOverTimeAction.Value;
                    }

                    if (actionData == null) {
                        continue;
                    }

                    var zeroFoundInList = false;
                    var regen = Regex.IsMatch(key, @"(リジェネ|récup|regen|再生|whispering|murmure|erhebendes|光の囁き|日光的低语)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    List<KeyValuePair<string, double>> healingHistoryList = ParseHelper.LastAmountByAction.GetPlayer(this.Name).ToList();
                    var resolvedPotency = 350;
                    foreach (KeyValuePair<string, double> healingAction in healingHistoryList) {
                        if (regen) {
                            var found = false;
                            Dictionary<string, List<string>> cureActions = HealingOverTimeHelper.CureActions;
                            Dictionary<string, List<string>> medicaActions = HealingOverTimeHelper.MedicaActions;
                            KeyValuePair<string, double> action = healingAction;
                            if (cureActions["III"].Any(cureAction => string.Equals(action.Key, cureAction, Constants.InvariantComparer))) {
                                found = zeroFoundInList = true;
                                resolvedPotency = 550;
                            }

                            if (cureActions["II"].Any(cureAction => string.Equals(action.Key, cureAction, Constants.InvariantComparer))) {
                                found = zeroFoundInList = true;
                                resolvedPotency = 650;
                            }

                            if (cureActions["I"].Any(cureAction => string.Equals(action.Key, cureAction, Constants.InvariantComparer))) {
                                found = zeroFoundInList = true;
                                resolvedPotency = 400;
                            }

                            if (medicaActions["II"].Any(medicaAction => string.Equals(action.Key, medicaAction, Constants.InvariantComparer))) {
                                found = zeroFoundInList = true;
                                resolvedPotency = 200;
                            }

                            if (medicaActions["I"].Any(medicaAction => string.Equals(action.Key, medicaAction, Constants.InvariantComparer))) {
                                found = zeroFoundInList = true;
                                resolvedPotency = 300;
                            }

                            if (found) {
                                if (action.Value > 0) {
                                    amount = action.Value;
                                }

                                break;
                            }
                        }

                        if (string.Equals(healingAction.Key, key, Constants.InvariantComparer)) {
                            amount = healingAction.Value;
                            break;
                        }
                    }

                    statusKey = $"{statusKey} [•]";
                    if (amount == 0) {
                        amount = 75;
                    }

                    resolvedPotency = zeroFoundInList
                                          ? resolvedPotency
                                          : regen
                                              ? resolvedPotency
                                              : actionData.Potency;
                    var tickHealing = Math.Ceiling(amount / resolvedPotency * actionData.OverTimePotency / 3);
                    if (actionData.HasNoInitialResult && !zeroFoundInList) {
                        IEnumerable<KeyValuePair<string, double>> nonZeroActions = healingHistoryList.Where(d => !d.Key.Contains("•"));
                        IList<KeyValuePair<string, double>> keyValuePairs = nonZeroActions as IList<KeyValuePair<string, double>> ?? nonZeroActions.ToList();
                        double healing = 0;
                        switch (regen) {
                            case true:
                                healing = Math.Ceiling(amount / resolvedPotency * actionData.OverTimePotency / 3);
                                break;
                            case false:
                                if (keyValuePairs.Any()) {
                                    amount = keyValuePairs.Sum(action => action.Value);
                                    amount = amount / keyValuePairs.Count();
                                }

                                healing = Math.Ceiling(amount / resolvedPotency * actionData.OverTimePotency / 3);
                                break;
                        }

                        tickHealing = healing > 0
                                          ? healing
                                          : tickHealing;
                    }

                    var line = new Line {
                        Action = statusKey,
                        Amount = tickHealing,
                        EventDirection = EventDirection.Unknown,
                        EventType = EventType.Cure,
                        EventSubject = EventSubject.Unknown,
                        Source = this.Name,
                        SourceEntity = this.NPCEntry,
                        XOverTime = true
                    };
                    try {
                        List<StatGroup> players = Controller.Timeline.Party.ToList();
                        StatusItem entry = statusEntry;
                        foreach (StatGroup player in players.Where(player => player.Name.Contains(entry.TargetName))) {
                            line.Target = player.Name;
                            break;
                        }
                    }
                    catch (Exception ex) {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }

                    if (string.IsNullOrWhiteSpace(line.Target)) {
                        line.Target = $"[???] {statusEntry.TargetName}";
                    }

                    Controller.Timeline.FightingRightNow = true;
                    Controller.Timeline.FightingTimer.Stop();
                    switch (Settings.Default.StoreHistoryEvent) {
                        case "Any":
                            Controller.Timeline.StoreHistoryTimer.Stop();
                            break;
                    }

                    DispatcherHelper.Invoke(
                        delegate {
                            line.Hit = true;

                            // resolve player hp each tick to ensure they are not at max
                            try {
                                List<ActorItem> players = XIVInfoViewModel.Instance.CurrentPCs.Select(entity => entity.Value).ToList();
                                if (!players.Any()) {
                                    return;
                                }

                                foreach (ActorItem actorEntity in players) {
                                    var playerName = actorEntity.Name;
                                    Controller.Timeline.TrySetPlayerCurable(playerName, actorEntity.HPMax - actorEntity.HPCurrent);
                                }
                            }
                            catch (Exception ex) {
                                Logging.Log(Logger, new LogItem(ex, true));
                            }

                            var currentCritPercent = this.Stats.GetStatValue("HealingCritPercent");
                            if (new Random().NextDouble() * 3 < currentCritPercent) {
                                line.Crit = true;
                                line.Amount = line.Amount * 1.5;
                            }

                            Controller.Timeline.GetSetPlayer(line.Source).SetHealingOverTime(line);
                        });
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
            }

            Controller.Timeline.FightingTimer.Start();
            switch (Settings.Default.StoreHistoryEvent) {
                case "Any":
                    Controller.Timeline.StoreHistoryTimer.Start();
                    break;
            }
        }
    }
}