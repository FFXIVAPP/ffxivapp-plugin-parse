// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseControl.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ParseControl.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
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
    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.Models.Timelines;
    using FFXIVAPP.Plugin.Parse.Monitors;
    using FFXIVAPP.Plugin.Parse.Properties;

    using Newtonsoft.Json;

    using NLog;

    using Sharlayan.Core.Enums;

    public class ParseControl : IParsingControl, INotifyPropertyChanged {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Lazy<ParseControl> _instance = new Lazy<ParseControl>(() => new ParseControl());

        private readonly Timer _parseEntityTimer = new Timer(100);

        private StatMonitor _statMonitor;

        private Timeline _timeline;

        private TimelineMonitor _timelineMonitor;

        public ParseControl() {
            this.Timeline = new Timeline(this);
            this.TimelineMonitor = new TimelineMonitor(this);
            this.StatMonitor = new StatMonitor(this);
            this.StartTime = DateTime.Now;
            this._parseEntityTimer.Elapsed += this.ParseEntityTimerOnElapsed;
            this._parseEntityTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static ParseControl Instance {
            get {
                return _instance.Value;
            }
        }

        public DateTime EndTime { get; set; }

        public bool FirstActionFound { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public StatMonitor StatMonitor {
            get {
                return this._statMonitor ?? (this._statMonitor = new StatMonitor(this));
            }

            set {
                this._statMonitor = value;
                this.RaisePropertyChanged();
            }
        }

        public Timeline Timeline {
            get {
                return this._timeline ?? (this._timeline = new Timeline(this));
            }

            set {
                this._timeline = value;
                this.RaisePropertyChanged();
            }
        }

        public TimelineMonitor TimelineMonitor {
            get {
                return this._timelineMonitor ?? (this._timelineMonitor = new TimelineMonitor(this));
            }

            set {
                this._timelineMonitor = value;
                this.RaisePropertyChanged();
            }
        }

        IParsingControl IParsingControl.Instance {
            get {
                return Instance;
            }
        }

        private ParseEntity LastParseEntity { get; set; }

        private bool ParseEntityTimerProcessing { get; set; }

        public void Initialize() { }

        public void Reset() {
            this._parseEntityTimer.Stop();
            this._parseEntityTimer.Elapsed -= this.ParseEntityTimerOnElapsed;
            this.FirstActionFound = !this.FirstActionFound;
            this.StatMonitor.Clear();
            this.Timeline.Clear();
            this.TimelineMonitor.Clear();
            var parseEntity = new ParseEntity {
                Players = new List<PlayerEntity>(),
            };
            this.StartTime = DateTime.Now;
            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.DPS);
            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.DTPS);
            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.HPS);
            this._parseEntityTimer.Elapsed += this.ParseEntityTimerOnElapsed;
            this._parseEntityTimer.Start();
        }

        private void ParseEntityTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs) {
            this.EndTime = DateTime.Now;
            if (this.ParseEntityTimerProcessing) {
                return;
            }

            this.ParseEntityTimerProcessing = true;

            Func<bool> processor = delegate {
                try {
                    var parseEntity = new ParseEntity {
                        Players = new List<PlayerEntity>(),
                    };
                    foreach (StatGroup statGroup in this.Timeline.Party) {
                        var player = (Player) statGroup;
                        try {
                            var type = Regex.Match(player.Name, @"\[(?<type>.+)\]", SharedRegEx.DefaultOptions).Groups["type"].Value;
                            var playerEntity = new PlayerEntity {
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
                                PercentOfTotalOverallDamageTakenOverTime = (double) player.GetStatValue("PercentOfTotalOverallDamageTakenOverTime"),
                            };
                            switch (type) {
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

                            if (player.NPCEntry != null) {
                                playerEntity.Job = player.NPCEntry.Job;
                            }

                            parseEntity.Players.Add(playerEntity);
                        }
                        catch (Exception ex) {
                            Logging.Log(Logger, new LogItem(ex, true));
                        }
                    }

                    parseEntity.CombinedDPS = (double) this.Timeline.Overall.GetStatValue("CombinedDPS");
                    parseEntity.DPS = (double) this.Timeline.Overall.GetStatValue("DPS");
                    parseEntity.DOTPS = (double) this.Timeline.Overall.GetStatValue("DOTPS");
                    parseEntity.CombinedHPS = (double) this.Timeline.Overall.GetStatValue("CombinedHPS");
                    parseEntity.HPS = (double) this.Timeline.Overall.GetStatValue("HPS");
                    parseEntity.HOTPS = (double) this.Timeline.Overall.GetStatValue("HOTPS");
                    parseEntity.HOHPS = (double) this.Timeline.Overall.GetStatValue("HOHPS");
                    parseEntity.HMPS = (double) this.Timeline.Overall.GetStatValue("HMPS");
                    parseEntity.CombinedDTPS = (double) this.Timeline.Overall.GetStatValue("CombinedDTPS");
                    parseEntity.DTPS = (double) this.Timeline.Overall.GetStatValue("DTPS");
                    parseEntity.DTOTPS = (double) this.Timeline.Overall.GetStatValue("DTOTPS");
                    parseEntity.CombinedTotalOverallDamage = (double) this.Timeline.Overall.GetStatValue("CombinedTotalOverallDamage");
                    parseEntity.TotalOverallDamage = (double) this.Timeline.Overall.GetStatValue("TotalOverallDamage");
                    parseEntity.TotalOverallDamageOverTime = (double) this.Timeline.Overall.GetStatValue("TotalOverallDamageOverTime");
                    parseEntity.CombinedTotalOverallHealing = (double) this.Timeline.Overall.GetStatValue("CombinedTotalOverallHealing");
                    parseEntity.TotalOverallHealing = (double) this.Timeline.Overall.GetStatValue("TotalOverallHealing");
                    parseEntity.TotalOverallHealingOverTime = (double) this.Timeline.Overall.GetStatValue("TotalOverallHealingOverTime");
                    parseEntity.TotalOverallHealingOverHealing = (double) this.Timeline.Overall.GetStatValue("TotalOverallHealingOverHealing");
                    parseEntity.TotalOverallHealingMitigated = (double) this.Timeline.Overall.GetStatValue("TotalOverallHealingMitigated");
                    parseEntity.CombinedTotalOverallDamageTaken = (double) this.Timeline.Overall.GetStatValue("CombinedTotalOverallDamageTaken");
                    parseEntity.TotalOverallDamageTaken = (double) this.Timeline.Overall.GetStatValue("TotalOverallDamageTaken");
                    parseEntity.TotalOverallDamageTakenOverTime = (double) this.Timeline.Overall.GetStatValue("TotalOverallDamageTakenOverTime");
                    parseEntity.PercentOfTotalOverallDamage = (double) this.Timeline.Overall.GetStatValue("PercentOfTotalOverallDamage");
                    parseEntity.PercentOfTotalOverallDamageOverTime = (double) this.Timeline.Overall.GetStatValue("PercentOfTotalOverallDamageOverTime");
                    parseEntity.PercentOfTotalOverallHealing = (double) this.Timeline.Overall.GetStatValue("PercentOfTotalOverallHealing");
                    parseEntity.PercentOfTotalOverallHealingOverTime = (double) this.Timeline.Overall.GetStatValue("PercentOfTotalOverallHealingOverTime");
                    parseEntity.PercentOfTotalOverallHealingOverHealing = (double) this.Timeline.Overall.GetStatValue("PercentOfTotalOverallHealingOverHealing");
                    parseEntity.PercentOfTotalOverallHealingMitigated = (double) this.Timeline.Overall.GetStatValue("PercentOfTotalOverallHealingMitigated");
                    parseEntity.PercentOfTotalOverallDamageTaken = (double) this.Timeline.Overall.GetStatValue("PercentOfTotalOverallDamageTaken");
                    parseEntity.PercentOfTotalOverallDamageTakenOverTime = (double) this.Timeline.Overall.GetStatValue("PercentOfTotalOverallDamageTakenOverTime");
                    var notify = false;
                    if (this.LastParseEntity == null) {
                        this.LastParseEntity = parseEntity;
                        notify = true;
                    }
                    else {
                        var hash1 = JsonConvert.SerializeObject(this.LastParseEntity).GetHashCode();
                        var hash2 = JsonConvert.SerializeObject(parseEntity).GetHashCode();
                        if (!hash1.Equals(hash2)) {
                            this.LastParseEntity = parseEntity;
                            notify = true;
                        }
                    }

                    if (notify) {
                        if (Settings.Default.ShowDPSWidgetOnLoad) {
                            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.DPS);
                        }

                        if (Settings.Default.ShowDTPSWidgetOnLoad) {
                            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.DTPS);
                        }

                        if (Settings.Default.ShowHPSWidgetOnLoad) {
                            EntityHelper.Parse.CleanAndCopy(parseEntity, EntityHelper.Parse.ParseType.HPS);
                        }
                    }
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }

                this.ParseEntityTimerProcessing = false;
                return true;
            };
            processor.BeginInvoke(delegate { }, processor);
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}