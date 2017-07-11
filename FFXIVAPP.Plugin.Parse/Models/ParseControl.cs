// FFXIVAPP.Plugin.Parse ~ ParseControl.cs
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Timers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models.StatGroups;
using FFXIVAPP.Plugin.Parse.Models.Timelines;
using FFXIVAPP.Plugin.Parse.Monitors;
using FFXIVAPP.Plugin.Parse.Properties;
using Newtonsoft.Json;
using NLog;
using Sharlayan.Core.Enums;

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class ParseControl : IParsingControl, INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public ParseControl()
        {
            Timeline = new Timeline(this);
            TimelineMonitor = new TimelineMonitor(this);
            StatMonitor = new StatMonitor(this);
            StartTime = DateTime.Now;
            _parseEntityTimer.Elapsed += ParseEntityTimerOnElapsed;
            _parseEntityTimer.Start();
        }

        private bool ParseEntityTimerProcessing { get; set; }

        private void ParseEntityTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            EndTime = DateTime.Now;
            if (ParseEntityTimerProcessing)
            {
                return;
            }
            ParseEntityTimerProcessing = true;
            Func<bool> parseEntityProcessor = delegate
            {
                try
                {
                    var parseEntity = new ParseEntity
                    {
                        Players = new List<PlayerEntity>()
                    };
                    foreach (Player player in Timeline.Party)
                    {
                        try
                        {
                            var type = Regex.Match(player.Name, @"\[(?<type>.+)\]", SharedRegEx.DefaultOptions)
                                            .Groups["type"]
                                            .Value;
                            var playerEntity = new PlayerEntity
                            {
                                Name = player.Name,
                                Job = Actor.Job.Unknown,
                                CombinedDPS = (double) player.GetStatValue("CombinedDPS"),
                                DPS = (double) player.GetStatValue("DPS"),
                                DOTPS = (double) player.GetStatValue("DOTPS"),
                                CombinedHPS = (double) player.GetStatValue("CombinedHPS"),
                                HPS = (double) player.GetStatValue("HPS"),
                                HOTPS = (double) player.GetStatValue("HOTPS"),
                                HOHPS = (double) player.GetStatValue("HOHPS"),
                                HMPS = (double) player.GetStatValue("HMPS"),
                                CombinedDTPS = (double) player.GetStatValue("CombinedDTPS"),
                                DTPS = (double) player.GetStatValue("DTPS"),
                                DTOTPS = (double) player.GetStatValue("DTOTPS"),
                                CombinedTotalOverallDamage = (double) player.GetStatValue("CombinedTotalOverallDamage"),
                                TotalOverallDamage = (double) player.GetStatValue("TotalOverallDamage"),
                                TotalOverallDamageOverTime = (double) player.GetStatValue("TotalOverallDamageOverTime"),
                                CombinedTotalOverallHealing = (double) player.GetStatValue("CombinedTotalOverallHealing"),
                                TotalOverallHealing = (double) player.GetStatValue("TotalOverallHealing"),
                                TotalOverallHealingOverTime = (double) player.GetStatValue("TotalOverallHealingOverTime"),
                                TotalOverallHealingOverHealing = (double) player.GetStatValue("TotalOverallHealingOverHealing"),
                                TotalOverallHealingMitigated = (double) player.GetStatValue("TotalOverallHealingMitigated"),
                                CombinedTotalOverallDamageTaken = (double) player.GetStatValue("CombinedTotalOverallDamageTaken"),
                                TotalOverallDamageTaken = (double) player.GetStatValue("TotalOverallDamageTaken"),
                                TotalOverallDamageTakenOverTime = (double) player.GetStatValue("TotalOverallDamageTakenOverTime"),
                                PercentOfTotalOverallDamage = (double) player.GetStatValue("PercentOfTotalOverallDamage"),
                                PercentOfTotalOverallDamageOverTime = (double) player.GetStatValue("PercentOfTotalOverallDamageOverTime"),
                                PercentOfTotalOverallHealing = (double) player.GetStatValue("PercentOfTotalOverallHealing"),
                                PercentOfTotalOverallHealingOverTime = (double) player.GetStatValue("PercentOfTotalOverallHealingOverTime"),
                                PercentOfTotalOverallHealingOverHealing = (double) player.GetStatValue("PercentOfTotalOverallHealingOverHealing"),
                                PercentOfTotalOverallHealingMitigated = (double) player.GetStatValue("PercentOfTotalOverallHealingMitigated"),
                                PercentOfTotalOverallDamageTaken = (double) player.GetStatValue("PercentOfTotalOverallDamageTaken"),
                                PercentOfTotalOverallDamageTakenOverTime = (double) player.GetStatValue("PercentOfTotalOverallDamageTakenOverTime")
                            };
                            switch (type)
                            {
                                case "P":
                                    playerEntity.Type = PlayerType.Party;
                                    break;
                                case "O":
                                    playerEntity.Type = PlayerType.Other;
                                    break;
                                case "A":
                                    playerEntity.Type = PlayerType.Alliance;
                                    break;
                                case "???":
                                    playerEntity.Type = PlayerType.Unknown;
                                    break;
                                default:
                                    playerEntity.Type = PlayerType.You;
                                    break;
                            }
                            if (player.NPCEntry != null)
                            {
                                playerEntity.Job = player.NPCEntry.Job;
                            }
                            parseEntity.Players.Add(playerEntity);
                        }
                        catch (Exception ex)
                        {
                            Logging.Log(Logger, new LogItem(ex, true));
                        }
                    }
                    parseEntity.CombinedDPS = (double) Timeline.Overall.GetStatValue("CombinedDPS");
                    parseEntity.DPS = (double) Timeline.Overall.GetStatValue("DPS");
                    parseEntity.DOTPS = (double) Timeline.Overall.GetStatValue("DOTPS");
                    parseEntity.CombinedHPS = (double) Timeline.Overall.GetStatValue("CombinedHPS");
                    parseEntity.HPS = (double) Timeline.Overall.GetStatValue("HPS");
                    parseEntity.HOTPS = (double) Timeline.Overall.GetStatValue("HOTPS");
                    parseEntity.HOHPS = (double) Timeline.Overall.GetStatValue("HOHPS");
                    parseEntity.HMPS = (double) Timeline.Overall.GetStatValue("HMPS");
                    parseEntity.CombinedDTPS = (double) Timeline.Overall.GetStatValue("CombinedDTPS");
                    parseEntity.DTPS = (double) Timeline.Overall.GetStatValue("DTPS");
                    parseEntity.DTOTPS = (double) Timeline.Overall.GetStatValue("DTOTPS");
                    parseEntity.CombinedTotalOverallDamage = (double) Timeline.Overall.GetStatValue("CombinedTotalOverallDamage");
                    parseEntity.TotalOverallDamage = (double) Timeline.Overall.GetStatValue("TotalOverallDamage");
                    parseEntity.TotalOverallDamageOverTime = (double) Timeline.Overall.GetStatValue("TotalOverallDamageOverTime");
                    parseEntity.CombinedTotalOverallHealing = (double) Timeline.Overall.GetStatValue("CombinedTotalOverallHealing");
                    parseEntity.TotalOverallHealing = (double) Timeline.Overall.GetStatValue("TotalOverallHealing");
                    parseEntity.TotalOverallHealingOverTime = (double) Timeline.Overall.GetStatValue("TotalOverallHealingOverTime");
                    parseEntity.TotalOverallHealingOverHealing = (double) Timeline.Overall.GetStatValue("TotalOverallHealingOverHealing");
                    parseEntity.TotalOverallHealingMitigated = (double) Timeline.Overall.GetStatValue("TotalOverallHealingMitigated");
                    parseEntity.CombinedTotalOverallDamageTaken = (double) Timeline.Overall.GetStatValue("CombinedTotalOverallDamageTaken");
                    parseEntity.TotalOverallDamageTaken = (double) Timeline.Overall.GetStatValue("TotalOverallDamageTaken");
                    parseEntity.TotalOverallDamageTakenOverTime = (double) Timeline.Overall.GetStatValue("TotalOverallDamageTakenOverTime");
                    parseEntity.PercentOfTotalOverallDamage = (double) Timeline.Overall.GetStatValue("PercentOfTotalOverallDamage");
                    parseEntity.PercentOfTotalOverallDamageOverTime = (double) Timeline.Overall.GetStatValue("PercentOfTotalOverallDamageOverTime");
                    parseEntity.PercentOfTotalOverallHealing = (double) Timeline.Overall.GetStatValue("PercentOfTotalOverallHealing");
                    parseEntity.PercentOfTotalOverallHealingOverTime = (double) Timeline.Overall.GetStatValue("PercentOfTotalOverallHealingOverTime");
                    parseEntity.PercentOfTotalOverallHealingOverHealing = (double) Timeline.Overall.GetStatValue("PercentOfTotalOverallHealingOverHealing");
                    parseEntity.PercentOfTotalOverallHealingMitigated = (double) Timeline.Overall.GetStatValue("PercentOfTotalOverallHealingMitigated");
                    parseEntity.PercentOfTotalOverallDamageTaken = (double) Timeline.Overall.GetStatValue("PercentOfTotalOverallDamageTaken");
                    parseEntity.PercentOfTotalOverallDamageTakenOverTime = (double) Timeline.Overall.GetStatValue("PercentOfTotalOverallDamageTakenOverTime");
                    var notify = false;
                    if (LastParseEntity == null)
                    {
                        LastParseEntity = parseEntity;
                        notify = true;
                    }
                    else
                    {
                        var hash1 = JsonConvert.SerializeObject(LastParseEntity)
                                               .GetHashCode();
                        var hash2 = JsonConvert.SerializeObject(parseEntity)
                                               .GetHashCode();
                        if (!hash1.Equals(hash2))
                        {
                            LastParseEntity = parseEntity;
                            notify = true;
                        }
                    }
                    if (notify)
                    {
                        if (Settings.Default.ShowDPSWidgetOnLoad)
                        {
                            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.DPS);
                        }
                        if (Settings.Default.ShowDTPSWidgetOnLoad)
                        {
                            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.DTPS);
                        }
                        if (Settings.Default.ShowHPSWidgetOnLoad)
                        {
                            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.HPS);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
                ParseEntityTimerProcessing = false;
                return true;
            };
            parseEntityProcessor.BeginInvoke(delegate { }, parseEntityProcessor);
        }

        #region Auto Properties

        public bool FirstActionFound { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        #endregion

        #region Declarations

        private readonly Timer _parseEntityTimer = new Timer(100);
        private ParseEntity LastParseEntity { get; set; }

        #endregion

        #region Implementation of IParsingControl

        private static ParseControl _instance;
        private StatMonitor _statMonitor;
        private Timeline _timeline;
        private TimelineMonitor _timelineMonitor;

        public static ParseControl Instance
        {
            get { return _instance ?? (_instance = new ParseControl()); }
            set { _instance = value; }
        }

        IParsingControl IParsingControl.Instance
        {
            get { return Instance; }
        }

        public Timeline Timeline
        {
            get { return _timeline ?? (_timeline = new Timeline(this)); }
            set
            {
                _timeline = value;
                RaisePropertyChanged();
            }
        }

        public StatMonitor StatMonitor
        {
            get { return _statMonitor ?? (_statMonitor = new StatMonitor(this)); }
            set
            {
                _statMonitor = value;
                RaisePropertyChanged();
            }
        }

        public TimelineMonitor TimelineMonitor
        {
            get { return _timelineMonitor ?? (_timelineMonitor = new TimelineMonitor(this)); }
            set
            {
                _timelineMonitor = value;
                RaisePropertyChanged();
            }
        }

        public void Initialize()
        {
        }

        public void Reset()
        {
            _parseEntityTimer.Stop();
            _parseEntityTimer.Elapsed -= ParseEntityTimerOnElapsed;
            FirstActionFound = !FirstActionFound;
            StatMonitor.Clear();
            Timeline.Clear();
            TimelineMonitor.Clear();
            var parseEntity = new ParseEntity
            {
                Players = new List<PlayerEntity>()
            };
            StartTime = DateTime.Now;
            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.DPS);
            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.DTPS);
            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.HPS);
            _parseEntityTimer.Elapsed += ParseEntityTimerOnElapsed;
            _parseEntityTimer.Start();
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
