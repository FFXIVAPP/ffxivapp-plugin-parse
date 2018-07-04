// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Timeline.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Timeline.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Timelines {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Timers;

    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Models.Fights;
    using FFXIVAPP.Plugin.Parse.Models.LinkedStats;
    using FFXIVAPP.Plugin.Parse.Models.StatGroups;
    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.Properties;

    using NLog;

    public sealed class Timeline : INotifyPropertyChanged {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public readonly Timer FightingTimer = new Timer(2500);

        public readonly Timer StoreHistoryTimer = new Timer(5000);

        private ParseControl _controller;

        private bool _deathFound;

        private bool _fightingRightNow;

        private FightList _fights;

        private string _lastEngaged;

        private string _lastKilled;

        private StatGroup _monster;

        private StatGroup _overall;

        private StatGroup _party;

        private Dictionary<string, int> _playerCurables;

        /// <summary>
        /// </summary>
        public Timeline(ParseControl parseControl) {
            this.Controller = parseControl;
            this.FightingRightNow = false;
            this.DeathFound = false;
            this.Fights = new FightList();

            // setup party
            this.Overall = new StatGroup("Overall");
            this.Party = new StatGroup("Party") {
                IncludeSelf = false
            };
            this.Monster = new StatGroup("Monster") {
                IncludeSelf = false
            };
            this.PlayerCurables = new Dictionary<string, int>();
            this.SetStoreHistoryInterval();
            this.InitStats();
            this.StoreHistoryTimer.Elapsed += this.StoreHistoryTimerOnElapsed;
            this.FightingTimer.Elapsed += this.FightingTimerOnElapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event EventHandler<TimelineChangedEvent> TimelineChangedEvent = delegate { };

        public bool DeathFound {
            get {
                return this._deathFound;
            }

            private set {
                this._deathFound = value;
                this.RaisePropertyChanged();
            }
        }

        public bool FightingRightNow {
            get {
                return this._fightingRightNow;
            }

            set {
                this._fightingRightNow = value;
                this.RaisePropertyChanged();
            }
        }

        public FightList Fights {
            get {
                return this._fights;
            }

            internal set {
                this._fights = value;
                this.RaisePropertyChanged();
            }
        }

        public string LastEngaged {
            get {
                return this._lastEngaged;
            }

            set {
                this._lastEngaged = value;
                this.RaisePropertyChanged();
            }
        }

        public string LastKilled {
            get {
                return this._lastKilled;
            }

            set {
                this._lastKilled = value;
                this.RaisePropertyChanged();
            }
        }

        public StatGroup Monster {
            get {
                return this._monster;
            }

            internal set {
                this._monster = value;
                this.RaisePropertyChanged();
            }
        }

        public StatGroup Overall {
            get {
                return this._overall;
            }

            internal set {
                this._overall = value;
                this.RaisePropertyChanged();
            }
        }

        public StatGroup Party {
            get {
                return this._party;
            }

            internal set {
                this._party = value;
                this.RaisePropertyChanged();
            }
        }

        private ParseControl Controller {
            get {
                return this._controller;
            }

            set {
                this._controller = value;
                this.RaisePropertyChanged();
            }
        }

        private Dictionary<string, int> PlayerCurables {
            get {
                return this._playerCurables;
            }

            set {
                this._playerCurables = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        public void Clear() {
            this.Fights.Clear();
            this.Overall.Clear();
            this.Party.Clear();
            this.Monster.Clear();
            this.InitStats();
        }

        public void ClearPlayerCurables() {
            lock (this.PlayerCurables) {
                this.PlayerCurables.Clear();
            }
        }

        public Dictionary<string, int> GetPlayerCurables() {
            lock (this.PlayerCurables) {
                return this.PlayerCurables.ToDictionary(playerCurable => playerCurable.Key, playerCurable => playerCurable.Value);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="monsterName"> </param>
        /// <returns> </returns>
        public Monster GetSetMonster(string monsterName) {
            if (!this.Monster.HasGroup(monsterName)) {
                Logging.Log(Logger, $"StatEvent : Adding new stat group for monster : {monsterName}");
                this.Monster.AddGroup(new Monster(monsterName, this.Controller));
            }

            var monster = (Monster) this.Monster.GetGroup(monsterName);
            return monster;
        }

        /// <summary>
        /// </summary>
        /// <param name="playerName"> </param>
        /// <returns> </returns>
        public Player GetSetPlayer(string playerName) {
            if (!this.Party.HasGroup(playerName)) {
                Logging.Log(Logger, $"StatEvent : Adding new stat group for player : {playerName}");
                this.Party.AddGroup(new Player(playerName, this.Controller));
            }

            var player = (Player) this.Party.GetGroup(playerName);
            return player;
        }

        /// <summary>
        /// </summary>
        /// <param name="eventType"> </param>
        /// <param name="eventArgs"> </param>
        public void PublishTimelineEvent(TimelineEventType eventType, params object[] eventArgs) {
            object args = eventArgs != null && eventArgs.Any()
                              ? eventArgs[0]
                              : "(no args)";
            Logging.Log(Logger, $"TimelineEvent : {eventType} {args}");
            if (eventArgs != null) {
                var monsterName = eventArgs.First() as string;
                switch (eventType) {
                    case TimelineEventType.PartyJoin:
                    case TimelineEventType.PartyDisband:
                    case TimelineEventType.PartyLeave:
                        break;
                    case TimelineEventType.PartyMonsterFighting:
                    case TimelineEventType.AllianceMonsterFighting:
                    case TimelineEventType.OtherMonsterFighting:
                        this.DeathFound = false;
                        if (monsterName != null && (monsterName.ToLower().Contains("target") || monsterName == string.Empty)) {
                            break;
                        }

                        Fight fighting;
                        if (!this.Fights.TryGet(monsterName, out fighting)) {
                            fighting = new Fight(monsterName);
                            this.Fights.Add(fighting);
                        }

                        this.Controller.Timeline.LastEngaged = monsterName;
                        break;
                    case TimelineEventType.PartyMonsterKilled:
                    case TimelineEventType.AllianceMonsterKilled:
                    case TimelineEventType.OtherMonsterKilled:
                        this.DeathFound = true;
                        if (monsterName != null && (monsterName.ToLower().Contains("target") || monsterName == string.Empty)) {
                            break;
                        }

                        Fight killed;
                        if (!this.Fights.TryGet(monsterName, out killed)) {
                            killed = new Fight(monsterName);
                            this.Fights.Add(killed);
                        }

                        switch (eventType) {
                            case TimelineEventType.PartyMonsterKilled:
                                this.GetSetMonster(monsterName).SetKill(killed);
                                break;
                            case TimelineEventType.AllianceMonsterKilled:
                                this.GetSetMonster(monsterName).SetKill(killed);
                                break;
                            case TimelineEventType.OtherMonsterKilled:
                                this.GetSetMonster(monsterName).SetKill(killed);
                                break;
                        }

                        this.Controller.Timeline.LastKilled = monsterName;
                        break;
                }
            }

            this.RaiseTimelineChangedEvent(this, new TimelineChangedEvent(eventType, eventArgs));
        }

        public int TryGetPlayerCurable(string key) {
            lock (this.PlayerCurables) {
                return this.PlayerCurables.ContainsKey(key)
                           ? this.PlayerCurables[key]
                           : 0;
            }
        }

        public void TrySetPlayerCurable(string key, int value) {
            lock (this.PlayerCurables) {
                if (this.PlayerCurables.ContainsKey(key)) {
                    this.PlayerCurables[key] = value;
                }
                else {
                    this.PlayerCurables.Add(key, value);
                }
            }
        }

        private void FightingTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs) {
            this.FightingRightNow = false;
            this.FightingTimer.Stop();
        }

        private void InitStats() {
            this.Overall.Stats.Clear();
            List<Stat<double>> overallStats = this.OverallStats().Select(s => s.Value).ToList();
            this.Overall.Stats.AddStats(overallStats);
        }

        private Dictionary<string, Stat<double>> OverallStats() {
            Dictionary<string, Stat<double>> stats = new Dictionary<string, Stat<double>>();

            

            #region Combined

            stats.Add("CombinedTotalOverallDamage", new TotalStat("CombinedTotalOverallDamage"));
            stats.Add("CombinedDPS", new PerSecondAverageStat("CombinedDPS", stats["CombinedTotalOverallDamage"]));
            stats.Add("CombinedStaticPlayerDPS", new TotalStat("CombinedStaticPlayerDPS"));
            stats.Add("CombinedRegularDamage", new TotalStat("CombinedRegularDamage"));
            stats.Add("CombinedCriticalDamage", new TotalStat("CombinedCriticalDamage"));

            stats.Add("CombinedTotalOverallHealing", new TotalStat("CombinedTotalOverallHealing"));
            stats.Add("CombinedHPS", new PerSecondAverageStat("CombinedHPS", stats["CombinedTotalOverallHealing"]));
            stats.Add("CombinedStaticPlayerHPS", new TotalStat("CombinedStaticPlayerHPS"));
            stats.Add("CombinedRegularHealing", new TotalStat("CombinedRegularHealing"));
            stats.Add("CombinedCriticalHealing", new TotalStat("CombinedCriticalHealing"));

            stats.Add("CombinedTotalOverallDamageTaken", new TotalStat("CombinedTotalOverallDamageTaken"));
            stats.Add("CombinedDTPS", new PerSecondAverageStat("CombinedDTPS", stats["CombinedTotalOverallDamageTaken"]));
            stats.Add("CombinedStaticPlayerDTPS", new TotalStat("CombinedStaticPlayerDTPS"));
            stats.Add("CombinedRegularDamageTaken", new TotalStat("CombinedRegularDamageTaken"));
            stats.Add("CombinedCriticalDamageTaken", new TotalStat("CombinedCriticalDamageTaken"));

            #endregion

            // damage
            stats.Add("TotalOverallDamage", new TotalStat("TotalOverallDamage"));
            stats.Add("DPS", new PerSecondAverageStat("DPS", stats["TotalOverallDamage"]));
            stats.Add("StaticPlayerDPS", new TotalStat("StaticPlayerDPS"));
            stats.Add("RegularDamage", new TotalStat("RegularDamage"));
            stats.Add("CriticalDamage", new TotalStat("CriticalDamage"));

            stats.Add("TotalOverallDamageOverTime", new TotalStat("TotalOverallDamageOverTime"));
            stats.Add("DOTPS", new PerSecondAverageStat("DOTPS", stats["TotalOverallDamageOverTime"]));
            stats.Add("StaticPlayerDOTPS", new TotalStat("StaticPlayerDOTPS"));
            stats.Add("RegularDamageOverTime", new TotalStat("RegularDamageOverTime"));
            stats.Add("CriticalDamageOverTime", new TotalStat("CriticalDamageOverTime"));

            // healing
            stats.Add("TotalOverallHealing", new TotalStat("TotalOverallHealing"));
            stats.Add("HPS", new PerSecondAverageStat("HPS", stats["TotalOverallHealing"]));
            stats.Add("StaticPlayerHPS", new TotalStat("StaticPlayerHPS"));
            stats.Add("RegularHealing", new TotalStat("RegularHealing"));
            stats.Add("CriticalHealing", new TotalStat("CriticalHealing"));

            stats.Add("TotalOverallHealingOverHealing", new TotalStat("TotalOverallHealingOverHealing"));
            stats.Add("HOHPS", new PerSecondAverageStat("HOHPS", stats["TotalOverallHealingOverHealing"]));
            stats.Add("StaticPlayerHOHPS", new TotalStat("StaticPlayerHOHPS"));
            stats.Add("RegularHealingOverHealing", new TotalStat("RegularHealingOverHealing"));
            stats.Add("CriticalHealingOverHealing", new TotalStat("CriticalHealingOverHealing"));

            stats.Add("TotalOverallHealingOverTime", new TotalStat("TotalOverallHealingOverTime"));
            stats.Add("HOTPS", new PerSecondAverageStat("HOTPS", stats["TotalOverallHealingOverTime"]));
            stats.Add("StaticPlayerHOTPS", new TotalStat("StaticPlayerHOTPS"));
            stats.Add("RegularHealingOverTime", new TotalStat("RegularHealingOverTime"));
            stats.Add("CriticalHealingOverTime", new TotalStat("CriticalHealingOverTime"));

            stats.Add("TotalOverallHealingMitigated", new TotalStat("TotalOverallHealingMitigated"));
            stats.Add("HMPS", new PerSecondAverageStat("HMPS", stats["TotalOverallHealingMitigated"]));
            stats.Add("StaticPlayerHMPS", new TotalStat("StaticPlayerHMPS"));
            stats.Add("RegularHealingMitigated", new TotalStat("RegularHealingMitigated"));
            stats.Add("CriticalHealingMitigated", new TotalStat("CriticalHealingMitigated"));

            // damage taken
            stats.Add("TotalOverallDamageTaken", new TotalStat("TotalOverallDamageTaken"));
            stats.Add("DTPS", new PerSecondAverageStat("DTPS", stats["TotalOverallDamageTaken"]));
            stats.Add("StaticPlayerDTPS", new TotalStat("StaticPlayerDTPS"));
            stats.Add("RegularDamageTaken", new TotalStat("RegularDamageTaken"));
            stats.Add("CriticalDamageTaken", new TotalStat("CriticalDamageTaken"));

            stats.Add("TotalOverallDamageTakenOverTime", new TotalStat("TotalOverallDamageTakenOverTime"));
            stats.Add("DTOTPS", new PerSecondAverageStat("DTOTPS", stats["TotalOverallDamageTakenOverTime"]));
            stats.Add("StaticPlayerDTOTPS", new TotalStat("StaticPlayerDTOTPS"));
            stats.Add("RegularDamageTakenOverTime", new TotalStat("RegularDamageTakenOverTime"));
            stats.Add("CriticalDamageTakenOverTime", new TotalStat("CriticalDamageTakenOverTime"));

            // other
            stats.Add("TotalOverallActiveTime", new TotalStat("TotalOverallActiveTime"));
            stats.Add("TotalOverallTP", new TotalStat("TotalOverallTP"));
            stats.Add("TotalOverallMP", new TotalStat("TotalOverallMP"));

            

            #region Monster

            #region Combined

            stats.Add("CombinedTotalOverallDamageMonster", new TotalStat("CombinedTotalOverallDamageMonster"));
            stats.Add("CombinedDPSMonster", new PerSecondAverageStat("CombinedDPSMonster", stats["CombinedTotalOverallDamageMonster"]));
            stats.Add("CombinedStaticMonsterDPSMonster", new TotalStat("CombinedStaticMonsterDPSMonster"));
            stats.Add("CombinedRegularDamageMonster", new TotalStat("CombinedRegularDamageMonster"));
            stats.Add("CombinedCriticalDamageMonster", new TotalStat("CombinedCriticalDamageMonster"));

            stats.Add("CombinedTotalOverallHealingMonster", new TotalStat("CombinedTotalOverallHealingMonster"));
            stats.Add("CombinedHPSMonster", new PerSecondAverageStat("CombinedHPSMonster", stats["CombinedTotalOverallHealingMonster"]));
            stats.Add("CombinedStaticMonsterHPSMonster", new TotalStat("CombinedStaticMonsterHPSMonster"));
            stats.Add("CombinedRegularHealingMonster", new TotalStat("CombinedRegularHealingMonster"));
            stats.Add("CombinedCriticalHealingMonster", new TotalStat("CombinedCriticalHealingMonster"));

            stats.Add("CombinedTotalOverallDamageTakenMonster", new TotalStat("CombinedTotalOverallDamageTakenMonster"));
            stats.Add("CombinedDTPSMonster", new PerSecondAverageStat("CombinedDTPSMonster", stats["CombinedTotalOverallDamageTakenMonster"]));
            stats.Add("CombinedStaticMonsterDTPSMonster", new TotalStat("CombinedStaticMonsterDTPSMonster"));
            stats.Add("CombinedRegularDamageTakenMonster", new TotalStat("CombinedRegularDamageTakenMonster"));
            stats.Add("CombinedCriticalDamageTakenMonster", new TotalStat("CombinedCriticalDamageTakenMonster"));

            #endregion

            // damage
            stats.Add("TotalOverallDamageMonster", new TotalStat("TotalOverallDamageMonster"));
            stats.Add("DPSMonster", new PerSecondAverageStat("DPSMonster", stats["TotalOverallDamageMonster"]));
            stats.Add("StaticMonsterDPSMonster", new TotalStat("StaticMonsterDPSMonster"));
            stats.Add("RegularDamageMonster", new TotalStat("RegularDamageMonster"));
            stats.Add("CriticalDamageMonster", new TotalStat("CriticalDamageMonster"));

            stats.Add("TotalOverallDamageOverTimeMonster", new TotalStat("TotalOverallDamageOverTimeMonster"));
            stats.Add("DOTPSMonster", new PerSecondAverageStat("DOTPSMonster", stats["TotalOverallDamageOverTimeMonster"]));
            stats.Add("StaticMonsterDOTPSMonster", new TotalStat("StaticMonsterDOTPSMonster"));
            stats.Add("RegularDamageOverTimeMonster", new TotalStat("RegularDamageOverTimeMonster"));
            stats.Add("CriticalDamageOverTimeMonster", new TotalStat("CriticalDamageOverTimeMonster"));

            // healing
            stats.Add("TotalOverallHealingMonster", new TotalStat("TotalOverallHealingMonster"));
            stats.Add("HPSMonster", new PerSecondAverageStat("HPSMonster", stats["TotalOverallHealingMonster"]));
            stats.Add("StaticMonsterHPSMonster", new TotalStat("StaticMonsterHPSMonster"));
            stats.Add("RegularHealingMonster", new TotalStat("RegularHealingMonster"));
            stats.Add("CriticalHealingMonster", new TotalStat("CriticalHealingMonster"));

            stats.Add("TotalOverallHealingOverHealingMonster", new TotalStat("TotalOverallHealingOverHealingMonster"));
            stats.Add("HOHPSMonster", new PerSecondAverageStat("HOHPSMonster", stats["TotalOverallHealingOverHealingMonster"]));
            stats.Add("StaticMonsterHOHPSMonster", new TotalStat("StaticMonsterHOHPSMonster"));
            stats.Add("RegularHealingOverHealingMonster", new TotalStat("RegularHealingOverHealingMonster"));
            stats.Add("CriticalHealingOverHealingMonster", new TotalStat("CriticalHealingOverHealingMonster"));

            stats.Add("TotalOverallHealingOverTimeMonster", new TotalStat("TotalOverallHealingOverTimeMonster"));
            stats.Add("HOTPSMonster", new PerSecondAverageStat("HOTPSMonster", stats["TotalOverallHealingOverTimeMonster"]));
            stats.Add("StaticMonsterHOTPSMonster", new TotalStat("StaticMonsterHOTPSMonster"));
            stats.Add("RegularHealingOverTimeMonster", new TotalStat("RegularHealingOverTimeMonster"));
            stats.Add("CriticalHealingOverTimeMonster", new TotalStat("CriticalHealingOverTimeMonster"));

            stats.Add("TotalOverallHealingMitigatedMonster", new TotalStat("TotalOverallHealingMitigatedMonster"));
            stats.Add("HMPSMonster", new PerSecondAverageStat("HMPSMonster", stats["TotalOverallHealingMitigatedMonster"]));
            stats.Add("StaticMonsterHMPSMonster", new TotalStat("StaticMonsterHMPSMonster"));
            stats.Add("RegularHealingMitigatedMonster", new TotalStat("RegularHealingMitigatedMonster"));
            stats.Add("CriticalHealingMitigatedMonster", new TotalStat("CriticalHealingMitigatedMonster"));

            // damage taken
            stats.Add("TotalOverallDamageTakenMonster", new TotalStat("TotalOverallDamageTakenMonster"));
            stats.Add("DTPSMonster", new PerSecondAverageStat("DTPSMonster", stats["TotalOverallDamageTakenMonster"]));
            stats.Add("StaticMonsterDTPSMonster", new TotalStat("StaticMonsterDTPSMonster"));
            stats.Add("RegularDamageTakenMonster", new TotalStat("RegularDamageTakenMonster"));
            stats.Add("CriticalDamageTakenMonster", new TotalStat("CriticalDamageTakenMonster"));

            stats.Add("TotalOverallDamageTakenOverTimeMonster", new TotalStat("TotalOverallDamageTakenOverTimeMonster"));
            stats.Add("DTOTPSMonster", new PerSecondAverageStat("DTOTPSMonster", stats["TotalOverallDamageTakenOverTimeMonster"]));
            stats.Add("StaticMonsterDTOTPSMonster", new TotalStat("StaticMonsterDTOTPSMonster"));
            stats.Add("RegularDamageTakenOverTimeMonster", new TotalStat("RegularDamageTakenOverTimeMonster"));
            stats.Add("CriticalDamageTakenOverTimeMonster", new TotalStat("CriticalDamageTakenOverTimeMonster"));

            #endregion

            return stats;
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        private void RaiseTimelineChangedEvent(object sender, TimelineChangedEvent e) {
            this.TimelineChangedEvent(sender, e);
        }

        private void SetStoreHistoryInterval() {
            try {
                double interval;
                double.TryParse(Settings.Default.StoreHistoryInterval, out interval);
                this.StoreHistoryTimer.Interval = interval;
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        private void StoreHistoryTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs) {
            this.SetStoreHistoryInterval();
            if (Settings.Default.EnableStoreHistoryReset) {
                if (!this.FightingRightNow) {
                    this.Controller.Reset();
                }
            }

            this.StoreHistoryTimer.Stop();
        }
    }
}