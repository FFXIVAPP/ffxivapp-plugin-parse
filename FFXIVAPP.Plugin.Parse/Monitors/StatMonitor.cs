// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatMonitor.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   StatMonitor.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Monitors {
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.Events;
    using FFXIVAPP.Plugin.Parse.Models.History;
    using FFXIVAPP.Plugin.Parse.Models.StatGroups;
    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.Properties;
    using FFXIVAPP.Plugin.Parse.ViewModels;

    using NLog;

    using Sharlayan.Models.XIVDatabase;
    using Sharlayan.Utilities;

    public class StatMonitor : EventMonitor {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="parseControl"> </param>
        public StatMonitor(ParseControl parseControl) : base("StatMonitor", parseControl) {
            this.IncludeSelf = false;
            this.Filter = EventParser.TypeMask | EventParser.Self | EventParser.Engaged | EventParser.UnEngaged;
            if (Settings.Default.ParseYou) {
                this.Filter = FilterHelper.Enable(this.Filter, EventParser.You);
                this.Filter = FilterHelper.Enable(this.Filter, EventParser.Pet);
            }

            if (Settings.Default.ParseParty) {
                this.Filter = FilterHelper.Enable(this.Filter, EventParser.Party);
                this.Filter = FilterHelper.Enable(this.Filter, EventParser.PetParty);
            }

            if (Settings.Default.ParseAlliance) {
                this.Filter = FilterHelper.Enable(this.Filter, EventParser.Alliance);
                this.Filter = FilterHelper.Enable(this.Filter, EventParser.PetAlliance);
            }

            if (Settings.Default.ParseOther) {
                this.Filter = FilterHelper.Enable(this.Filter, EventParser.Other);
                this.Filter = FilterHelper.Enable(this.Filter, EventParser.PetOther);
            }
        }

        /// <summary>
        /// </summary>
        public override void Clear() {
            Logging.Log(Logger, $"ClearEvent : Clearing {this.Count} Party Member Totals.");
            foreach (StatGroup player in this.ParseControl.Timeline.Party) {
                Player playerInstance = this.ParseControl.Timeline.GetSetPlayer(player.Name);
                playerInstance.StatusUpdateTimer.Stop();
                playerInstance.IsActiveTimer.Stop();
            }

            foreach (StatGroup monster in this.ParseControl.Timeline.Monster) {
                Monster monsterInstance = this.ParseControl.Timeline.GetSetMonster(monster.Name);
                monsterInstance.StatusUpdateTimer.Stop();

                // monsterInstance.IsActiveTimer.Stop();
            }

            // save parse to log
            try {
                JsonHelper.JsonParse parse = JsonHelper.ToJson.ConvertParse();
                if (parse.IsValid) {
                    var invalidCharacters = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
                    var invalidRegEx = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidCharacters);
                    var fileName = Path.Combine(Common.Constants.LogsPath, "Parser", Regex.Replace(parse.Name, invalidRegEx, "_") + ".json");
                    try {
                        File.WriteAllText(fileName, parse.Parse, Encoding.UTF8);
                    }
                    catch (Exception ex) {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            // move parse to history
            this.InitializeHistory();
            base.Clear();
        }

        public void ToggleFilter(ulong filter) {
            this.Filter = FilterHelper.Toggle(this.Filter, filter);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Event e) {
            Utilities.Filter.Process(e);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleUnknownEvent(Event e) {
            ParsingLogHelper.Log(Logger, "UnknownEvent", e);
        }

        private void InitializeHistory() {
            var hasDamage = this.ParseControl.Timeline.Overall.Stats.GetStatValue("TotalOverallDamage") > 0;
            var hasHealing = this.ParseControl.Timeline.Overall.Stats.GetStatValue("TotalOverallHealing") > 0;
            var hasDamageTaken = this.ParseControl.Timeline.Overall.Stats.GetStatValue("TotalOverallDamageTaken") > 0;
            if (hasDamage || hasHealing || hasDamageTaken) {
                StatContainer currentOverallStats = this.ParseControl.Timeline.Overall.Stats;
                var historyItem = new ParseHistoryItem();
                HistoryControl historyController = historyItem.HistoryControl = new HistoryControl();
                foreach (Stat<double> stat in currentOverallStats) {
                    historyController.Timeline.Overall.Stats.EnsureStatValue(stat.Name, stat.Value);
                }

                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerDPS", currentOverallStats.GetStatValue("DPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerDOTPS", currentOverallStats.GetStatValue("DOTPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerHPS", currentOverallStats.GetStatValue("HPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerHOHPS", currentOverallStats.GetStatValue("HOHPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerHOTPS", currentOverallStats.GetStatValue("HOTPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerHMPS", currentOverallStats.GetStatValue("HMPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerDTPS", currentOverallStats.GetStatValue("DTPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerDTOTPS", currentOverallStats.GetStatValue("DTOTPS"));
                StatGroup[] playerList = this.ParseControl.Timeline.Party.ToArray();
                foreach (StatGroup player in playerList) {
                    HistoryGroup playerInstance = historyController.Timeline.GetSetPlayer(player.Name);
                    playerInstance.Last20DamageActions = ((Player) player).Last20DamageActions.ToList();
                    playerInstance.Last20DamageTakenActions = ((Player) player).Last20DamageTakenActions.ToList();
                    playerInstance.Last20HealingActions = ((Player) player).Last20HealingActions.ToList();
                    playerInstance.Last20Items = ((Player) player).Last20Items.ToList();
                    foreach (Stat<double> stat in player.Stats) {
                        playerInstance.Stats.EnsureStatValue(stat.Name, stat.Value);
                    }

                    this.RabbitHoleCopy(ref playerInstance, player);
                }

                StatGroup[] monsterList = this.ParseControl.Timeline.Monster.ToArray();
                foreach (StatGroup monster in monsterList) {
                    HistoryGroup monsterInstance = historyController.Timeline.GetSetMonster(monster.Name);
                    monsterInstance.Last20DamageActions = ((Monster) monster).Last20DamageActions.ToList();
                    monsterInstance.Last20DamageTakenActions = ((Monster) monster).Last20DamageTakenActions.ToList();
                    monsterInstance.Last20HealingActions = ((Monster) monster).Last20HealingActions.ToList();
                    monsterInstance.Last20Items = ((Monster) monster).Last20Items.ToList();
                    foreach (Stat<double> stat in monster.Stats) {
                        monsterInstance.Stats.EnsureStatValue(stat.Name, stat.Value);
                    }

                    this.RabbitHoleCopy(ref monsterInstance, monster);
                }

                historyItem.Start = this.ParseControl.StartTime;
                historyItem.End = DateTime.Now;
                historyItem.ParseLength = historyItem.End - historyItem.Start;
                var parseTimeDetails = $"{historyItem.Start} -> {historyItem.End} [{historyItem.ParseLength}]";
                var zone = "UNKNOWN";
                if (XIVInfoViewModel.Instance.CurrentUser != null) {
                    var mapIndex = XIVInfoViewModel.Instance.CurrentUser.MapIndex;
                    MapItem mapItem = ZoneLookup.GetZoneInfo(mapIndex);
                    switch (Constants.GameLanguage) {
                        case "French":
                            zone = mapItem.Name.French;
                            break;
                        case "Japanese":
                            zone = mapItem.Name.Japanese;
                            break;
                        case "German":
                            zone = mapItem.Name.German;
                            break;
                        case "Chinese":
                            zone = mapItem.Name.Chinese;
                            break;
                        case "Korean":
                            zone = mapItem.Name.Korean;
                            break;
                        default:
                            zone = mapItem.Name.English;
                            break;
                    }
                }

                var monsterName = "NULL";
                try {
                    StatGroup biggestMonster = null;
                    foreach (StatGroup monster in this.ParseControl.Timeline.Monster) {
                        if (biggestMonster == null) {
                            biggestMonster = monster;
                        }
                        else {
                            if (monster.Stats.GetStatValue("TotalOverallDamage") > biggestMonster.Stats.GetStatValue("TotalOverallDamage")) {
                                biggestMonster = monster;
                            }
                        }
                    }

                    if (biggestMonster != null) {
                        monsterName = biggestMonster.Name;
                    }
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }

                foreach (Stat<double> oStat in currentOverallStats) {
                    historyController.Timeline.Overall.Stats.EnsureStatValue(oStat.Name, oStat.Value);
                }

                historyItem.Name = $"{zone} [{monsterName}] {parseTimeDetails}";
                DispatcherHelper.Invoke(() => MainViewModel.Instance.ParseHistory.Insert(1, historyItem));
            }
        }

        private void RabbitHoleCopy(ref HistoryGroup parent, StatGroup statGroup) {
            if (statGroup.Stats != null) {
                foreach (Stat<double> stat in statGroup.Stats) {
                    parent.Stats.EnsureStatValue(stat.Name, stat.Value);
                }
            }

            if (!statGroup.Children.Any()) {
                return;
            }

            foreach (StatGroup group in statGroup.Children) {
                HistoryGroup newParent = parent.GetGroup(group.Name);
                foreach (Stat<double> stat in group.Stats) {
                    newParent.Stats.EnsureStatValue(stat.Name, stat.Value);
                }

                this.RabbitHoleCopy(ref newParent, group);
            }
        }
    }
}