// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   JsonHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Helpers {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.RegularExpressions;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.History;
    using FFXIVAPP.Plugin.Parse.Models.StatGroups;
    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.ViewModels;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NLog;

    using Sharlayan.Models.XIVDatabase;
    using Sharlayan.Utilities;

    public static class JsonHelper {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static class ToJson {
            public static JsonParse ConvertParse() {
                var hasDamage = ParseControl.Instance.Timeline.Overall.Stats.GetStatValue("TotalOverallDamage") > 0;
                var hasHealing = ParseControl.Instance.Timeline.Overall.Stats.GetStatValue("TotalOverallHealing") > 0;
                var hasDamageTaken = ParseControl.Instance.Timeline.Overall.Stats.GetStatValue("TotalOverallDamageTaken") > 0;
                if (!hasDamage && !hasHealing && !hasDamageTaken) {
                    return new JsonParse();
                }

                StatContainer currentOverallStats = ParseControl.Instance.Timeline.Overall.Stats;
                Dictionary<string, object> historyItem = new Dictionary<string, object>();
                Dictionary<string, object> timeline = new Dictionary<string, object>();
                Dictionary<string, object> overall = new Dictionary<string, object> {
                    {
                        "Stats", new Dictionary<string, object>()
                    },
                };
                foreach (Stat<double> stat in currentOverallStats) {
                    ((Dictionary<string, object>) overall["Stats"]).Add(stat.Name, stat.Value);
                }

                timeline.Add("Overall", overall);
                StatGroup[] playerList = ParseControl.Instance.Timeline.Party.ToArray();
                Dictionary<string, object> players = new Dictionary<string, object>();
                foreach (StatGroup player in playerList) {
                    Dictionary<string, object> playerItem = new Dictionary<string, object> {
                        {
                            "Stats", new Dictionary<string, object>()
                        }, {
                            "Last20DamageActions", ((Player) player).Last20DamageActions.ToList()
                        }, {
                            "Last20DamageTakenActions", ((Player) player).Last20DamageTakenActions.ToList()
                        }, {
                            "Last20HealingActions", ((Player) player).Last20HealingActions.ToList()
                        }, {
                            "Last20Items", ((Player) player).Last20Items.ToList()
                        },
                    };
                    foreach (Stat<double> stat in player.Stats) {
                        ((Dictionary<string, object>) playerItem["Stats"]).Add(stat.Name, stat.Value);
                    }

                    players.Add(player.Name, playerItem);
                    RabbitHoleCopy(ref playerItem, player);
                }

                timeline.Add("Party", players);
                StatGroup[] monsterList = ParseControl.Instance.Timeline.Monster.ToArray();
                Dictionary<string, object> monsters = new Dictionary<string, object>();
                foreach (StatGroup monster in monsterList) {
                    Dictionary<string, object> monsterItem = new Dictionary<string, object> {
                        {
                            "Stats", new Dictionary<string, object>()
                        }, {
                            "Last20DamageActions", ((Monster) monster).Last20DamageActions.ToList()
                        }, {
                            "Last20DamageTakenActions", ((Monster) monster).Last20DamageTakenActions.ToList()
                        }, {
                            "Last20HealingActions", ((Monster) monster).Last20HealingActions.ToList()
                        }, {
                            "Last20Items", ((Monster) monster).Last20Items.ToList()
                        },
                    };
                    foreach (Stat<double> stat in monster.Stats) {
                        ((Dictionary<string, object>) monsterItem["Stats"]).Add(stat.Name, stat.Value);
                    }

                    monsters.Add(monster.Name, monsterItem);
                    RabbitHoleCopy(ref monsterItem, monster);
                }

                timeline.Add("Monster", monsters);
                historyItem.Add("Timeline", timeline);

                DateTime start = ParseControl.Instance.StartTime;
                DateTime end = DateTime.Now;
                TimeSpan parseLength = end - start;
                var parseTimeDetails = $"{start} -> {end} [{parseLength}]";
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
                    foreach (StatGroup monster in ParseControl.Instance.Timeline.Monster) {
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

                return new JsonParse {
                    Name = $"{zone} [{monsterName}] {parseTimeDetails}",
                    Parse = JsonConvert.SerializeObject(
                        historyItem, new JsonSerializerSettings {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        }),
                };
            }

            private static void RabbitHoleCopy(ref Dictionary<string, object> parent, StatGroup statGroup) {
                if (statGroup.Stats != null) {
                    foreach (Stat<double> stat in statGroup.Stats) {
                        if (((Dictionary<string, object>) parent["Stats"]).ContainsKey(stat.Name)) {
                            ((Dictionary<string, object>) parent["Stats"])[stat.Name] = stat.Value;
                        }
                        else {
                            ((Dictionary<string, object>) parent["Stats"]).Add(stat.Name, stat.Value);
                        }
                    }
                }

                if (!statGroup.Children.Any()) {
                    return;
                }

                foreach (StatGroup group in statGroup.Children) {
                    Dictionary<string, object> newParent = new Dictionary<string, object> {
                        {
                            "Stats", new Dictionary<string, object>()
                        },
                    };
                    foreach (Stat<double> stat in group.Stats) {
                        ((Dictionary<string, object>) newParent["Stats"]).Add(stat.Name, stat.Value);
                    }

                    parent.Add(group.Name, newParent);
                    RabbitHoleCopy(ref newParent, group);
                }
            }
        }

        public static class ToParseControl {
            private static Regex FileNameRegEx = new Regex(@"^(?<zone>.+) \[(?<monsterName>.+)\] (?<startTime>\d{4}-\d{2}-\d{2} \d{2}_\d{2}_\d{2} \w{2}) -_ (?<endTime>\d{4}-\d{2}-\d{2} \d{2}_\d{2}_\d{2} \w{2}) \[.+\]$", SharedRegEx.DefaultOptions);

            public static void ConvertJson(string fileName, string json) {
                JObject jsonHistoryItem = JObject.Parse(json);

                Dictionary<string, JToken> timeline = GetDictionary(jsonHistoryItem["Timeline"]);
                Dictionary<string, JToken> overall = GetDictionary(timeline["Overall"]);
                Dictionary<string, JToken> overallStats = GetDictionary(overall["Stats"]);

                var historyItem = new ParseHistoryItem();
                HistoryControl historyController = historyItem.HistoryControl = new HistoryControl();

                foreach (KeyValuePair<string, JToken> stat in overallStats) {
                    historyController.Timeline.Overall.Stats.EnsureStatValue(stat.Key, (double) stat.Value);
                }

                Dictionary<string, JToken> players = GetDictionary(timeline["Party"]);
                foreach (KeyValuePair<string, JToken> player in players) {
                    Dictionary<string, JToken> children = GetDictionary(player.Value);
                    HistoryGroup playerInstance = historyController.Timeline.GetSetPlayer(player.Key);
                    playerInstance.Last20DamageActions = children["Last20DamageActions"].ToObject<List<LineHistory>>();
                    playerInstance.Last20DamageTakenActions = children["Last20DamageTakenActions"].ToObject<List<LineHistory>>();
                    playerInstance.Last20HealingActions = children["Last20HealingActions"].ToObject<List<LineHistory>>();
                    playerInstance.Last20Items = children["Last20Items"].ToObject<List<LineHistory>>();
                    Dictionary<string, JToken> stats = GetDictionary(player.Value["Stats"]);
                    foreach (KeyValuePair<string, JToken> stat in stats) {
                        playerInstance.Stats.EnsureStatValue(stat.Key, (double) stat.Value);
                    }

                    RabbitHoleCopy(ref playerInstance, player);
                }

                Dictionary<string, JToken> monsters = GetDictionary(timeline["Monster"]);
                foreach (KeyValuePair<string, JToken> monster in monsters) {
                    Dictionary<string, JToken> children = GetDictionary(monster.Value);
                    HistoryGroup monsterInstance = historyController.Timeline.GetSetMonster(monster.Key);
                    monsterInstance.Last20DamageActions = children["Last20DamageActions"].ToObject<List<LineHistory>>();
                    monsterInstance.Last20DamageTakenActions = children["Last20DamageTakenActions"].ToObject<List<LineHistory>>();
                    monsterInstance.Last20HealingActions = children["Last20HealingActions"].ToObject<List<LineHistory>>();
                    monsterInstance.Last20Items = children["Last20Items"].ToObject<List<LineHistory>>();
                    Dictionary<string, JToken> stats = GetDictionary(monster.Value["Stats"]);
                    foreach (KeyValuePair<string, JToken> stat in stats) {
                        monsterInstance.Stats.EnsureStatValue(stat.Key, (double) stat.Value);
                    }

                    RabbitHoleCopy(ref monsterInstance, monster);
                }

                Match fileNameParts = FileNameRegEx.Match(Path.GetFileNameWithoutExtension(fileName));

                historyItem.Start = DateTime.Now;
                historyItem.End = DateTime.Now;
                var zone = "Unknown";
                var monsterName = "NULL";
                try {
                    historyItem.Start = DateTime.Parse(fileNameParts.Groups["startTime"].ToString().Replace("_", ":"));
                    historyItem.End = DateTime.Parse(fileNameParts.Groups["endTime"].ToString().Replace("_", ":"));
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }

                try {
                    zone = fileNameParts.Groups["zone"].ToString();
                    monsterName = fileNameParts.Groups["monsterName"].ToString();
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }

                historyItem.ParseLength = historyItem.End - historyItem.Start;
                var parseTimeDetails = $"{historyItem.Start} -> {historyItem.End} [{historyItem.ParseLength}]";
                historyItem.Name = $"{zone} [{monsterName}] {parseTimeDetails}";
                DispatcherHelper.Invoke(() => MainViewModel.Instance.ParseHistory.Insert(1, historyItem));
            }

            private static Dictionary<string, JToken> GetDictionary(object value) {
                IDictionary<string, JToken> obj = (JObject) value;
                return obj.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }

            private static void RabbitHoleCopy(ref HistoryGroup parent, KeyValuePair<string, JToken> statGroup) {
                Dictionary<string, JToken> stats = GetDictionary(statGroup.Value["Stats"]);
                if (stats != null) {
                    foreach (KeyValuePair<string, JToken> stat in stats) {
                        parent.Stats.EnsureStatValue(stat.Key, (double) stat.Value);
                    }
                }

                Dictionary<string, JToken> groups = GetDictionary(statGroup.Value);
                if (!groups.Any()) {
                    return;
                }

                foreach (KeyValuePair<string, JToken> group in groups.Where(kvp => kvp.Key != "Stats" && !kvp.Key.StartsWith("Last20"))) {
                    HistoryGroup newParent = parent.GetGroup(group.Key);
                    try {
                        Dictionary<string, JToken> groupStats = GetDictionary(group.Value["Stats"]);
                        foreach (KeyValuePair<string, JToken> stat in groupStats) {
                            newParent.Stats.EnsureStatValue(stat.Key, (double) stat.Value);
                        }
                    }
                    catch (Exception ex) {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }

                    RabbitHoleCopy(ref newParent, group);
                }
            }
        }

        public class JsonParse {
            public bool IsValid {
                get {
                    return !string.IsNullOrWhiteSpace(this.Name) && !string.IsNullOrWhiteSpace(this.Parse);
                }
            }

            public string Name { get; set; }

            public string Parse { get; set; }
        }
    }
}