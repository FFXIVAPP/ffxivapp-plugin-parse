// FFXIVAPP.Plugin.Parse ~ StatMonitor.cs
// 
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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Memory.Helpers;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Models.History;
using FFXIVAPP.Plugin.Parse.Models.StatGroups;
using FFXIVAPP.Plugin.Parse.Models.Stats;
using FFXIVAPP.Plugin.Parse.Properties;
using FFXIVAPP.Plugin.Parse.ViewModels;
using NLog;

namespace FFXIVAPP.Plugin.Parse.Monitors
{
    public class StatMonitor : EventMonitor
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="parseControl"> </param>
        public StatMonitor(ParseControl parseControl) : base("StatMonitor", parseControl)
        {
            IncludeSelf = false;
            Filter = (EventParser.TypeMask | EventParser.Self | EventParser.Engaged | EventParser.UnEngaged);
            if (Settings.Default.ParseYou)
            {
                Filter = FilterHelper.Enable(Filter, EventParser.You);
                Filter = FilterHelper.Enable(Filter, EventParser.Pet);
            }
            if (Settings.Default.ParseParty)
            {
                Filter = FilterHelper.Enable(Filter, EventParser.Party);
                Filter = FilterHelper.Enable(Filter, EventParser.PetParty);
            }
            if (Settings.Default.ParseAlliance)
            {
                Filter = FilterHelper.Enable(Filter, EventParser.Alliance);
                Filter = FilterHelper.Enable(Filter, EventParser.PetAlliance);
            }
            if (Settings.Default.ParseOther)
            {
                Filter = FilterHelper.Enable(Filter, EventParser.Other);
                Filter = FilterHelper.Enable(Filter, EventParser.PetOther);
            }
        }

        public void ToggleFilter(UInt64 filter)
        {
            Filter = FilterHelper.Toggle(Filter, filter);
        }

        /// <summary>
        /// </summary>
        public override void Clear()
        {
            Logging.Log(Logger, String.Format("ClearEvent : Clearing {0} Party Member Totals.", Count));
            foreach (var player in ParseControl.Timeline.Party)
            {
                var playerInstance = ParseControl.Timeline.GetSetPlayer(player.Name);
                playerInstance.StatusUpdateTimer.Stop();
                playerInstance.IsActiveTimer.Stop();
            }
            foreach (var monster in ParseControl.Timeline.Monster)
            {
                var monsterInstance = ParseControl.Timeline.GetSetMonster(monster.Name);
                monsterInstance.StatusUpdateTimer.Stop();
                //monsterInstance.IsActiveTimer.Stop();
            }
            // save parse to log
            try
            {
                var parse = JsonHelper.ToJson.ConvertParse();
                if (parse.IsValid)
                {
                    var invalidCharacters = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
                    var invalidRegEx = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidCharacters);
                    var fileName = Path.Combine(Common.Constants.LogsPath, "Parser", Regex.Replace(parse.Name, invalidRegEx, "_") + ".json");
                    try
                    {
                        File.WriteAllText(fileName, parse.Parse, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }

            // move parse to history
            InitializeHistory();
            base.Clear();
        }

        private void InitializeHistory()
        {
            var hasDamage = ParseControl.Timeline.Overall.Stats.GetStatValue("TotalOverallDamage") > 0;
            var hasHealing = ParseControl.Timeline.Overall.Stats.GetStatValue("TotalOverallHealing") > 0;
            var hasDamageTaken = ParseControl.Timeline.Overall.Stats.GetStatValue("TotalOverallDamageTaken") > 0;
            if (hasDamage || hasHealing || hasDamageTaken)
            {
                var currentOverallStats = ParseControl.Timeline.Overall.Stats;
                var historyItem = new ParseHistoryItem();
                var historyController = historyItem.HistoryControl = new HistoryControl();
                foreach (var stat in currentOverallStats)
                {
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
                var playerList = ParseControl.Timeline.Party.ToArray();
                foreach (var player in playerList)
                {
                    var playerInstance = historyController.Timeline.GetSetPlayer(player.Name);
                    playerInstance.Last20DamageActions = ((Player) player).Last20DamageActions.ToList();
                    playerInstance.Last20DamageTakenActions = ((Player) player).Last20DamageTakenActions.ToList();
                    playerInstance.Last20HealingActions = ((Player) player).Last20HealingActions.ToList();
                    playerInstance.Last20Items = ((Player) player).Last20Items.ToList();
                    foreach (var stat in player.Stats)
                    {
                        playerInstance.Stats.EnsureStatValue(stat.Name, stat.Value);
                    }
                    RabbitHoleCopy(ref playerInstance, player);
                }
                var monsterList = ParseControl.Timeline.Monster.ToArray();
                foreach (var monster in monsterList)
                {
                    var monsterInstance = historyController.Timeline.GetSetMonster(monster.Name);
                    monsterInstance.Last20DamageActions = ((Monster) monster).Last20DamageActions.ToList();
                    monsterInstance.Last20DamageTakenActions = ((Monster) monster).Last20DamageTakenActions.ToList();
                    monsterInstance.Last20HealingActions = ((Monster) monster).Last20HealingActions.ToList();
                    monsterInstance.Last20Items = ((Monster) monster).Last20Items.ToList();
                    foreach (var stat in monster.Stats)
                    {
                        monsterInstance.Stats.EnsureStatValue(stat.Name, stat.Value);
                    }
                    RabbitHoleCopy(ref monsterInstance, monster);
                }
                historyItem.Start = ParseControl.StartTime;
                historyItem.End = DateTime.Now;
                historyItem.ParseLength = historyItem.End - historyItem.Start;
                var parseTimeDetails = String.Format("{0} -> {1} [{2}]", historyItem.Start, historyItem.End, historyItem.ParseLength);
                var zone = "Unknown";
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
                    foreach (var monster in ParseControl.Timeline.Monster)
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
                foreach (var oStat in currentOverallStats)
                {
                    historyController.Timeline.Overall.Stats.EnsureStatValue(oStat.Name, oStat.Value);
                }
                historyItem.Name = String.Format("{0} [{1}] {2}", zone, monsterName, parseTimeDetails);
                DispatcherHelper.Invoke(() => MainViewModel.Instance.ParseHistory.Insert(1, historyItem));
            }
        }

        private void RabbitHoleCopy(ref HistoryGroup parent, StatGroup statGroup)
        {
            if (statGroup.Stats != null)
            {
                foreach (var stat in statGroup.Stats)
                {
                    parent.Stats.EnsureStatValue(stat.Name, stat.Value);
                }
            }
            if (!statGroup.Children.Any())
            {
                return;
            }
            foreach (var group in statGroup.Children)
            {
                var newParent = parent.GetGroup(group.Name);
                foreach (var stat in group.Stats)
                {
                    newParent.Stats.EnsureStatValue(stat.Name, stat.Value);
                }
                RabbitHoleCopy(ref newParent, group);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Event e)
        {
            #region Clean Monster Names

            #endregion

            Utilities.Filter.Process(e);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleUnknownEvent(Event e)
        {
            ParsingLogHelper.Log(Logger, "UnknownEvent", e);
        }
    }
}
