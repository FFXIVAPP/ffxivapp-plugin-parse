// FFXIVAPP.Plugin.Parse
// JsonHelper.cs
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Memory.Helpers;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.History;
using FFXIVAPP.Plugin.Parse.Models.StatGroups;
using FFXIVAPP.Plugin.Parse.Models.Stats;
using FFXIVAPP.Plugin.Parse.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class JsonHelper
    {
        public class JsonParse
        {
            public string Name { get; set; }
            public string Parse { get; set; }

            public bool IsValid
            {
                get { return (!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(Parse)); }
            }
        }

        public static class ToJson
        {
            public static JsonParse ConvertParse()
            {
                var hasDamage = ParseControl.Instance.Timeline.Overall.Stats.GetStatValue("TotalOverallDamage") > 0;
                var hasHealing = ParseControl.Instance.Timeline.Overall.Stats.GetStatValue("TotalOverallHealing") > 0;
                var hasDamageTaken = ParseControl.Instance.Timeline.Overall.Stats.GetStatValue("TotalOverallDamageTaken") > 0;
                if (!hasDamage && !hasHealing && !hasDamageTaken)
                {
                    return new JsonParse();
                }
                var currentOverallStats = ParseControl.Instance.Timeline.Overall.Stats;
                var historyItem = new Dictionary<string, object>();
                var timeline = new Dictionary<string, object>();
                var overall = new Dictionary<string, object>
                {
                    {
                        "Stats", new Dictionary<string, object>()
                    }
                };
                foreach (var stat in currentOverallStats)
                {
                    ((Dictionary<string, object>) overall["Stats"]).Add(stat.Name, stat.Value);
                }
                timeline.Add("Overall", overall);
                var playerList = ParseControl.Instance.Timeline.Party.ToArray();
                var players = new Dictionary<string, object>();
                foreach (var player in playerList)
                {
                    var playerItem = new Dictionary<string, object>
                    {
                        {
                            "Stats", new Dictionary<string, object>()
                        },
                        {
                            "Last20DamageActions", ((Player) player).Last20DamageActions.ToList()
                        },
                        {
                            "Last20DamageTakenActions", ((Player) player).Last20DamageTakenActions.ToList()
                        },
                        {
                            "Last20HealingActions", ((Player) player).Last20HealingActions.ToList()
                        },
                        {
                            "Last20Items", ((Player) player).Last20Items.ToList()
                        }
                    };
                    foreach (var stat in player.Stats)
                    {
                        ((Dictionary<string, object>) playerItem["Stats"]).Add(stat.Name, stat.Value);
                    }
                    players.Add(player.Name, playerItem);
                    RabbitHoleCopy(ref playerItem, player);
                }
                timeline.Add("Party", players);
                var monsterList = ParseControl.Instance.Timeline.Monster.ToArray();
                var monsters = new Dictionary<string, object>();
                foreach (var monster in monsterList)
                {
                    var monsterItem = new Dictionary<string, object>
                    {
                        {
                            "Stats", new Dictionary<string, object>()
                        },
                        {
                            "Last20DamageActions", ((Monster) monster).Last20DamageActions.ToList()
                        },
                        {
                            "Last20DamageTakenActions", ((Monster) monster).Last20DamageTakenActions.ToList()
                        },
                        {
                            "Last20HealingActions", ((Monster) monster).Last20HealingActions.ToList()
                        },
                        {
                            "Last20Items", ((Monster) monster).Last20Items.ToList()
                        }
                    };
                    foreach (var stat in monster.Stats)
                    {
                        ((Dictionary<string, object>) monsterItem["Stats"]).Add(stat.Name, stat.Value);
                    }
                    monsters.Add(monster.Name, monsterItem);
                    RabbitHoleCopy(ref monsterItem, monster);
                }
                timeline.Add("Monster", monsters);
                historyItem.Add("Timeline", timeline);

                #region Resolve FileName Details

                var start = ParseControl.Instance.StartTime;
                var end = DateTime.Now;
                var parseLength = end - start;
                var parseTimeDetails = String.Format("{0} -> {1} [{2}]", start, end, parseLength);
                var zone = "UNKNOWN";
                if (XIVInfoViewModel.Instance.CurrentUser != null)
                {
                    var mapIndex = XIVInfoViewModel.Instance.CurrentUser.MapIndex;
                    zone = ZoneHelper.GetMapInfo(mapIndex)
                                     .English;
                    switch (Constants.GameLanguage)
                    {
                        case "French":
                            zone = ZoneHelper.GetMapInfo(mapIndex)
                                             .French;
                            break;
                        case "Japanese":
                            zone = ZoneHelper.GetMapInfo(mapIndex)
                                             .Japanese;
                            break;
                        case "German":
                            zone = ZoneHelper.GetMapInfo(mapIndex)
                                             .German;
                            break;
                        case "Chinese":
                            zone = ZoneHelper.GetMapInfo(mapIndex)
                                             .Chinese;
                            break;
                    }
                }
                var monsterName = "NULL";
                try
                {
                    StatGroup biggestMonster = null;
                    foreach (var monster in ParseControl.Instance.Timeline.Monster)
                    {
                        if (biggestMonster == null)
                        {
                            biggestMonster = monster;
                        }
                        else
                        {
                            if (monster.Stats.GetStatValue("TotalOverallDamage") > biggestMonster.Stats.GetStatValue("TotalOverallDamage"))
                            {
                                biggestMonster = monster;
                            }
                        }
                    }
                    if (biggestMonster != null)
                    {
                        monsterName = biggestMonster.Name;
                    }
                }
                catch (Exception ex)
                {
                }

                #endregion

                return new JsonParse
                {
                    Name = String.Format("{0} [{1}] {2}", zone, monsterName, parseTimeDetails),
                    Parse = JsonConvert.SerializeObject(historyItem, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
                };
            }

            private static void RabbitHoleCopy(ref Dictionary<string, object> parent, StatGroup statGroup)
            {
                if (statGroup.Stats != null)
                {
                    foreach (var stat in statGroup.Stats)
                    {
                        if (((Dictionary<string, object>) parent["Stats"]).ContainsKey(stat.Name))
                        {
                            ((Dictionary<string, object>) parent["Stats"])[stat.Name] = stat.Value;
                        }
                        else
                        {
                            ((Dictionary<string, object>) parent["Stats"]).Add(stat.Name, stat.Value);
                        }
                    }
                }
                if (!statGroup.Children.Any())
                {
                    return;
                }
                foreach (var group in statGroup.Children)
                {
                    var newParent = new Dictionary<string, object>
                    {
                        {
                            "Stats", new Dictionary<string, object>()
                        }
                    };
                    foreach (var stat in group.Stats)
                    {
                        ((Dictionary<string, object>) newParent["Stats"]).Add(stat.Name, stat.Value);
                    }
                    parent.Add(group.Name, newParent);
                    RabbitHoleCopy(ref newParent, group);
                }
            }
        }

        public static class ToParseControl
        {
            private static Regex FileNameRegEx = new Regex(@"^(?<zone>.+) \[(?<monsterName>.+)\] (?<startTime>\d{4}-\d{2}-\d{2} \d{2}_\d{2}_\d{2} \w{2}) -_ (?<endTime>\d{4}-\d{2}-\d{2} \d{2}_\d{2}_\d{2} \w{2}) \[.+\]$", SharedRegEx.DefaultOptions);

            public static void ConvertJson(string fileName, string json)
            {
                var jsonHistoryItem = JObject.Parse(json);

                var timeline = GetDictionary(jsonHistoryItem["Timeline"]);
                var overall = GetDictionary(timeline["Overall"]);
                var overallStats = GetDictionary(overall["Stats"]);

                var historyItem = new ParseHistoryItem();
                var historyController = historyItem.HistoryControl = new HistoryControl();

                foreach (var stat in overallStats)
                {
                    historyController.Timeline.Overall.Stats.EnsureStatValue(stat.Key, (double) stat.Value);
                }
                var players = GetDictionary(timeline["Party"]);
                foreach (var player in players)
                {
                    var children = GetDictionary(player.Value);
                    var playerInstance = historyController.Timeline.GetSetPlayer(player.Key);
                    playerInstance.Last20DamageActions = children["Last20DamageActions"].ToObject<List<LineHistory>>();
                    playerInstance.Last20DamageTakenActions = children["Last20DamageTakenActions"].ToObject<List<LineHistory>>();
                    playerInstance.Last20HealingActions = children["Last20HealingActions"].ToObject<List<LineHistory>>();
                    playerInstance.Last20Items = children["Last20Items"].ToObject<List<LineHistory>>();
                    var stats = GetDictionary(player.Value["Stats"]);
                    foreach (var stat in stats)
                    {
                        playerInstance.Stats.EnsureStatValue(stat.Key, (double) stat.Value);
                    }
                    RabbitHoleCopy(ref playerInstance, player);
                }
                var monsters = GetDictionary(timeline["Monster"]);
                foreach (var monster in monsters)
                {
                    var children = GetDictionary(monster.Value);
                    var monsterInstance = historyController.Timeline.GetSetMonster(monster.Key);
                    monsterInstance.Last20DamageActions = children["Last20DamageActions"].ToObject<List<LineHistory>>();
                    monsterInstance.Last20DamageTakenActions = children["Last20DamageTakenActions"].ToObject<List<LineHistory>>();
                    monsterInstance.Last20HealingActions = children["Last20HealingActions"].ToObject<List<LineHistory>>();
                    monsterInstance.Last20Items = children["Last20Items"].ToObject<List<LineHistory>>();
                    var stats = GetDictionary(monster.Value["Stats"]);
                    foreach (var stat in stats)
                    {
                        monsterInstance.Stats.EnsureStatValue(stat.Key, (double) stat.Value);
                    }
                    RabbitHoleCopy(ref monsterInstance, monster);
                }

                var fileNameParts = FileNameRegEx.Match(Path.GetFileNameWithoutExtension(fileName));

                historyItem.Start = DateTime.Now;
                historyItem.End = DateTime.Now;
                var zone = "Unknown";
                var monsterName = "NULL";
                try
                {
                    historyItem.Start = DateTime.Parse(fileNameParts.Groups["startTime"].ToString()
                                                                                        .Replace("_", ":"));
                    historyItem.End = DateTime.Parse(fileNameParts.Groups["endTime"].ToString()
                                                                                    .Replace("_", ":"));
                }
                catch (Exception ex)
                {
                }
                try
                {
                    zone = fileNameParts.Groups["zone"].ToString();
                    monsterName = fileNameParts.Groups["monsterName"].ToString();
                }
                catch (Exception ex)
                {
                }
                historyItem.ParseLength = historyItem.End - historyItem.Start;
                var parseTimeDetails = String.Format("{0} -> {1} [{2}]", historyItem.Start, historyItem.End, historyItem.ParseLength);
                historyItem.Name = String.Format("{0} [{1}] {2}", zone, monsterName, parseTimeDetails);
                DispatcherHelper.Invoke(() => MainViewModel.Instance.ParseHistory.Insert(1, historyItem));
            }

            private static void RabbitHoleCopy(ref HistoryGroup parent, KeyValuePair<string, JToken> statGroup)
            {
                var stats = GetDictionary(statGroup.Value["Stats"]);
                if (stats != null)
                {
                    foreach (var stat in stats)
                    {
                        parent.Stats.EnsureStatValue(stat.Key, (double) stat.Value);
                    }
                }
                var groups = GetDictionary(statGroup.Value);
                if (!groups.Any())
                {
                    return;
                }
                foreach (var group in groups.Where(kvp => kvp.Key != "Stats" && !kvp.Key.StartsWith("Last20")))
                {
                    var newParent = parent.GetGroup(group.Key);
                    try
                    {
                        var groupStats = GetDictionary(group.Value["Stats"]);
                        foreach (var stat in groupStats)
                        {
                            newParent.Stats.EnsureStatValue(stat.Key, (double) stat.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    RabbitHoleCopy(ref newParent, group);
                }
            }

            private static Dictionary<string, JToken> GetDictionary(object value)
            {
                IDictionary<string, JToken> obj = (JObject) value;
                return obj.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
        }
    }
}
