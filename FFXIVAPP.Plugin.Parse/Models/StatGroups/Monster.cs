﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Monster.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Monster.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    using FFXIVAPP.Plugin.Parse.Models.LinkedStats;
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    using NLog;

    using Sharlayan.Core;

    public partial class Monster : StatGroup {
        private static readonly IList<string> LD = new[] {
            "Counter",
            "Block",
            "Parry",
            "Resist",
            "Evade",
        };

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public readonly Timer StatusUpdateTimer = new Timer(1000);

        public List<StatusItem> StatusEntriesMonsters = new List<StatusItem>();

        public List<StatusItem> StatusEntriesPlayers = new List<StatusItem>();

        public List<StatusItem> StatusEntriesSelf = new List<StatusItem>();

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parseControl"></param>
        public Monster(string name, ParseControl parseControl) : base(name) {
            Controller = parseControl;
            this.ID = 0;
            this.LineHistory = new List<LineHistory>();
            this.Last20DamageActions = new List<LineHistory>();
            this.Last20DamageTakenActions = new List<LineHistory>();
            this.Last20HealingActions = new List<LineHistory>();
            this.Last20Items = new List<LineHistory>();
            this.InitStats();
            this.StatusUpdateTimer.Elapsed += this.StatusUpdateTimerOnElapsed;
            this.StatusUpdateTimer.Start();
        }

        public uint ID { get; set; }

        public List<LineHistory> Last20DamageActions { get; set; }

        public List<LineHistory> Last20DamageTakenActions { get; set; }

        public List<LineHistory> Last20HealingActions { get; set; }

        public List<LineHistory> Last20Items { get; set; }

        public List<LineHistory> LineHistory { get; set; }

        public ActorItem NPCEntry { get; set; }

        private static ParseControl Controller { get; set; }

        private CounterStat TotalKilled { get; set; }

        private TotalStat TotalOverallDrops { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="sub"> </param>
        /// <param name="useSub"></param>
        /// <returns> </returns>
        private IEnumerable<Stat<double>> DamageOverTimeStatList(StatGroup sub, bool useSub = false) {
            Dictionary<string, Stat<double>> stats = StatGeneration.DamageOverTimeStats();

            // setup per ability "percent of" stats
            switch (useSub) {
                case true:
                    stats.Add("PercentOfTotalOverallDamageOverTime", new PercentStat("PercentOfTotalOverallDamageOverTime", stats["TotalOverallDamageOverTime"], sub.Stats.GetStat("TotalOverallDamageOverTime")));
                    stats.Add("PercentOfRegularDamageOverTime", new PercentStat("PercentOfRegularDamageOverTime", stats["RegularDamageOverTime"], sub.Stats.GetStat("RegularDamageOverTime")));
                    stats.Add("PercentOfCriticalDamageOverTime", new PercentStat("PercentOfCriticalDamageOverTime", stats["CriticalDamageOverTime"], sub.Stats.GetStat("CriticalDamageOverTime")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallDamageOverTime", new PercentStat("PercentOfTotalOverallDamageOverTime", stats["TotalOverallDamageOverTime"], this.Stats.GetStat("TotalOverallDamageOverTime")));
                    stats.Add("PercentOfRegularDamageOverTime", new PercentStat("PercentOfRegularDamageOverTime", stats["RegularDamageOverTime"], this.Stats.GetStat("RegularDamageOverTime")));
                    stats.Add("PercentOfCriticalDamageOverTime", new PercentStat("PercentOfCriticalDamageOverTime", stats["CriticalDamageOverTime"], this.Stats.GetStat("CriticalDamageOverTime")));
                    break;
            }

            return stats.Select(s => s.Value).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"> </param>
        /// <param name="useSub"></param>
        /// <returns> </returns>
        private IEnumerable<Stat<double>> DamageStatList(StatGroup sub, bool useSub = false) {
            Dictionary<string, Stat<double>> stats = StatGeneration.DamageStats();

            // setup per ability "percent of" stats
            switch (useSub) {
                case true:
                    stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], sub.Stats.GetStat("TotalOverallDamage")));
                    stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], sub.Stats.GetStat("RegularDamage")));
                    stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], sub.Stats.GetStat("CriticalDamage")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], this.Stats.GetStat("TotalOverallDamage")));
                    stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], this.Stats.GetStat("RegularDamage")));
                    stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], this.Stats.GetStat("CriticalDamage")));
                    break;
            }

            return stats.Select(s => s.Value).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="useSub"></param>
        /// <returns></returns>
        private IEnumerable<Stat<double>> DamageTakenOverTimeStatList(StatGroup sub, bool useSub = false) {
            Dictionary<string, Stat<double>> stats = StatGeneration.DamageTakenOverTimeStats();

            // setup per damage taken "percent of" stats
            switch (useSub) {
                case true:
                    stats.Add("PercentOfTotalOverallDamageTakenOverTime", new PercentStat("PercentOfTotalOverallDamageTakenOverTime", stats["TotalOverallDamageTakenOverTime"], sub.Stats.GetStat("TotalOverallDamageTakenOverTime")));
                    stats.Add("PercentOfRegularDamageTakenOverTime", new PercentStat("PercentOfRegularDamageTakenOverTime", stats["RegularDamageTakenOverTime"], sub.Stats.GetStat("RegularDamageTakenOverTime")));
                    stats.Add("PercentOfCriticalDamageTakenOverTime", new PercentStat("PercentOfCriticalDamageTakenOverTime", stats["CriticalDamageTakenOverTime"], sub.Stats.GetStat("CriticalDamageTakenOverTime")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallDamageTakenOverTime", new PercentStat("PercentOfTotalOverallDamageTakenOverTime", stats["TotalOverallDamageTakenOverTime"], this.Stats.GetStat("TotalOverallDamageTakenOverTime")));
                    stats.Add("PercentOfRegularDamageTakenOverTime", new PercentStat("PercentOfRegularDamageTakenOverTime", stats["RegularDamageTakenOverTime"], this.Stats.GetStat("RegularDamageTakenOverTime")));
                    stats.Add("PercentOfCriticalDamageTakenOverTime", new PercentStat("PercentOfCriticalDamageTakenOverTime", stats["CriticalDamageTakenOverTime"], this.Stats.GetStat("CriticalDamageTakenOverTime")));
                    break;
            }

            return stats.Select(s => s.Value).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="useSub"></param>
        /// <returns></returns>
        private IEnumerable<Stat<double>> DamageTakenStatList(StatGroup sub, bool useSub = false) {
            Dictionary<string, Stat<double>> stats = StatGeneration.DamageTakenStats();

            // setup per damage taken "percent of" stats
            switch (useSub) {
                case true:
                    stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], sub.Stats.GetStat("TotalOverallDamageTaken")));
                    stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], sub.Stats.GetStat("RegularDamageTaken")));
                    stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], sub.Stats.GetStat("CriticalDamageTaken")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], this.Stats.GetStat("TotalOverallDamageTaken")));
                    stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], this.Stats.GetStat("RegularDamageTaken")));
                    stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], this.Stats.GetStat("CriticalDamageTaken")));
                    break;
            }

            return stats.Select(s => s.Value).ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private IEnumerable<Stat<double>> DropStatList() {
            Dictionary<string, Stat<double>> stats = new Dictionary<string, Stat<double>>();

            stats.Add("TotalDrops", new CounterStat("TotalDrops"));
            stats.Add("DropPercent", new PercentStat("DropPercent", stats["TotalDrops"], this.TotalKilled));

            this.TotalOverallDrops.AddDependency(stats["TotalDrops"]);

            return stats.Select(s => s.Value).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="useSub"></param>
        /// <returns></returns>
        private IEnumerable<Stat<double>> HealingMitigatedStatList(StatGroup sub, bool useSub = false) {
            Dictionary<string, Stat<double>> stats = StatGeneration.HealingMitigatedStats();

            // setup per HealingMitigated "percent of" stats
            switch (useSub) {
                case true:
                    stats.Add("PercentOfTotalOverallHealingMitigated", new PercentStat("PercentOfTotalOverallHealingMitigated", stats["TotalOverallHealingMitigated"], sub.Stats.GetStat("TotalOverallHealingMitigated")));
                    stats.Add("PercentOfRegularHealingMitigated", new PercentStat("PercentOfRegularHealingMitigated", stats["RegularHealingMitigated"], sub.Stats.GetStat("RegularHealingMitigated")));
                    stats.Add("PercentOfCriticalHealingMitigated", new PercentStat("PercentOfCriticalHealingMitigated", stats["CriticalHealingMitigated"], sub.Stats.GetStat("CriticalHealingMitigated")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallHealingMitigated", new PercentStat("PercentOfTotalOverallHealingMitigated", stats["TotalOverallHealingMitigated"], this.Stats.GetStat("TotalOverallHealingMitigated")));
                    stats.Add("PercentOfRegularHealingMitigated", new PercentStat("PercentOfRegularHealingMitigated", stats["RegularHealingMitigated"], this.Stats.GetStat("RegularHealingMitigated")));
                    stats.Add("PercentOfCriticalHealingMitigated", new PercentStat("PercentOfCriticalHealingMitigated", stats["CriticalHealingMitigated"], this.Stats.GetStat("CriticalHealingMitigated")));
                    break;
            }

            return stats.Select(s => s.Value).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="useSub"></param>
        /// <returns></returns>
        private IEnumerable<Stat<double>> HealingOverHealingStatList(StatGroup sub, bool useSub = false) {
            Dictionary<string, Stat<double>> stats = StatGeneration.HealingOverHealingStats();

            // setup per HealingOverHealing "percent of" stats
            switch (useSub) {
                case true:
                    stats.Add("PercentOfTotalOverallHealingOverHealing", new PercentStat("PercentOfTotalOverallHealingOverHealing", stats["TotalOverallHealingOverHealing"], sub.Stats.GetStat("TotalOverallHealingOverHealing")));
                    stats.Add("PercentOfRegularHealingOverHealing", new PercentStat("PercentOfRegularHealingOverHealing", stats["RegularHealingOverHealing"], sub.Stats.GetStat("RegularHealingOverHealing")));
                    stats.Add("PercentOfCriticalHealingOverHealing", new PercentStat("PercentOfCriticalHealingOverHealing", stats["CriticalHealingOverHealing"], sub.Stats.GetStat("CriticalHealingOverHealing")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallHealingOverHealing", new PercentStat("PercentOfTotalOverallHealingOverHealing", stats["TotalOverallHealingOverHealing"], this.Stats.GetStat("TotalOverallHealingOverHealing")));
                    stats.Add("PercentOfRegularHealingOverHealing", new PercentStat("PercentOfRegularHealingOverHealing", stats["RegularHealingOverHealing"], this.Stats.GetStat("RegularHealingOverHealing")));
                    stats.Add("PercentOfCriticalHealingOverHealing", new PercentStat("PercentOfCriticalHealingOverHealing", stats["CriticalHealingOverHealing"], this.Stats.GetStat("CriticalHealingOverHealing")));
                    break;
            }

            return stats.Select(s => s.Value).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="useSub"></param>
        /// <returns></returns>
        private IEnumerable<Stat<double>> HealingOverTimeStatList(StatGroup sub, bool useSub = false) {
            Dictionary<string, Stat<double>> stats = StatGeneration.HealingOverTimeStats();

            // setup per HealingOverTime "percent of" stats
            switch (useSub) {
                case true:
                    stats.Add("PercentOfTotalOverallHealingOverTime", new PercentStat("PercentOfTotalOverallHealingOverTime", stats["TotalOverallHealingOverTime"], sub.Stats.GetStat("TotalOverallHealingOverTime")));
                    stats.Add("PercentOfRegularHealingOverTime", new PercentStat("PercentOfRegularHealingOverTime", stats["RegularHealingOverTime"], sub.Stats.GetStat("RegularHealingOverTime")));
                    stats.Add("PercentOfCriticalHealingOverTime", new PercentStat("PercentOfCriticalHealingOverTime", stats["CriticalHealingOverTime"], sub.Stats.GetStat("CriticalHealingOverTime")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallHealingOverTime", new PercentStat("PercentOfTotalOverallHealingOverTime", stats["TotalOverallHealingOverTime"], this.Stats.GetStat("TotalOverallHealingOverTime")));
                    stats.Add("PercentOfRegularHealingOverTime", new PercentStat("PercentOfRegularHealingOverTime", stats["RegularHealingOverTime"], this.Stats.GetStat("RegularHealingOverTime")));
                    stats.Add("PercentOfCriticalHealingOverTime", new PercentStat("PercentOfCriticalHealingOverTime", stats["CriticalHealingOverTime"], this.Stats.GetStat("CriticalHealingOverTime")));
                    break;
            }

            return stats.Select(s => s.Value).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="useSub"></param>
        /// <returns></returns>
        private IEnumerable<Stat<double>> HealingStatList(StatGroup sub, bool useSub = false) {
            Dictionary<string, Stat<double>> stats = StatGeneration.HealingStats();

            // setup per healing "percent of" stats
            switch (useSub) {
                case true:
                    stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], sub.Stats.GetStat("TotalOverallHealing")));
                    stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], sub.Stats.GetStat("RegularHealing")));
                    stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], sub.Stats.GetStat("CriticalHealing")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], this.Stats.GetStat("TotalOverallHealing")));
                    stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], this.Stats.GetStat("RegularHealing")));
                    stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], this.Stats.GetStat("CriticalHealing")));
                    break;
            }

            return stats.Select(s => s.Value).ToList();
        }

        /// <summary>
        /// </summary>
        private void InitStats() {
            this.Stats.AddStats(this.TotalStatList());
        }

        private void StatusUpdateTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs) {
            this.StatusEntriesSelf.Clear();
            this.StatusEntriesPlayers.Clear();
            this.StatusEntriesMonsters.Clear();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private IEnumerable<Stat<double>> TotalStatList() {
            Dictionary<string, Stat<double>> stats = new Dictionary<string, Stat<double>>();

            this.TotalOverallDrops = new TotalStat("TotalOverallDrops");
            this.TotalKilled = new CounterStat("TotalKilled");

            stats.Add("TotalOverallDrops", this.TotalOverallDrops);
            stats.Add("TotalKilled", this.TotalKilled);
            stats.Add("AverageHP", new NumericStat("AverageHP"));

            // setup monster ability stats
            foreach (KeyValuePair<string, Stat<double>> damageStat in StatGeneration.DamageStats()) {
                stats.Add(damageStat.Key, damageStat.Value);
            }

            foreach (KeyValuePair<string, Stat<double>> damageStat in StatGeneration.DamageOverTimeStats()) {
                stats.Add(damageStat.Key, damageStat.Value);
            }

            // setup monster healing stats
            foreach (KeyValuePair<string, Stat<double>> healingStat in StatGeneration.HealingStats()) {
                stats.Add(healingStat.Key, healingStat.Value);
            }

            foreach (KeyValuePair<string, Stat<double>> healingStat in StatGeneration.HealingOverHealingStats()) {
                stats.Add(healingStat.Key, healingStat.Value);
            }

            foreach (KeyValuePair<string, Stat<double>> healingStat in StatGeneration.HealingOverTimeStats()) {
                stats.Add(healingStat.Key, healingStat.Value);
            }

            foreach (KeyValuePair<string, Stat<double>> healingStat in StatGeneration.HealingMitigatedStats()) {
                stats.Add(healingStat.Key, healingStat.Value);
            }

            // setup monster damage taken stats
            foreach (KeyValuePair<string, Stat<double>> damageTakenStat in StatGeneration.DamageTakenStats()) {
                stats.Add(damageTakenStat.Key, damageTakenStat.Value);
            }

            foreach (KeyValuePair<string, Stat<double>> damageTakenStat in StatGeneration.DamageTakenOverTimeStats()) {
                stats.Add(damageTakenStat.Key, damageTakenStat.Value);
            }

            // setup combined stats
            foreach (KeyValuePair<string, Stat<double>> combinedStat in StatGeneration.CombinedStats()) {
                stats.Add(combinedStat.Key, combinedStat.Value);
            }

            // link to main monster stats
            Dictionary<string, Stat<double>> oStats = Controller.Timeline.Overall.Stats.ToDictionary(o => o.Name);

            ((TotalStat) oStats["TotalOverallDamageMonster"]).AddDependency(stats["TotalOverallDamage"]);
            ((TotalStat) oStats["RegularDamageMonster"]).AddDependency(stats["RegularDamage"]);
            ((TotalStat) oStats["CriticalDamageMonster"]).AddDependency(stats["CriticalDamage"]);

            ((TotalStat) oStats["TotalOverallDamageOverTimeMonster"]).AddDependency(stats["TotalOverallDamageOverTime"]);
            ((TotalStat) oStats["RegularDamageOverTimeMonster"]).AddDependency(stats["RegularDamageOverTime"]);
            ((TotalStat) oStats["CriticalDamageOverTimeMonster"]).AddDependency(stats["CriticalDamageOverTime"]);

            #region Healing

            ((TotalStat) oStats["TotalOverallHealingMonster"]).AddDependency(stats["TotalOverallHealing"]);
            ((TotalStat) oStats["RegularHealingMonster"]).AddDependency(stats["RegularHealing"]);
            ((TotalStat) oStats["CriticalHealingMonster"]).AddDependency(stats["CriticalHealing"]);

            ((TotalStat) oStats["TotalOverallHealingOverHealingMonster"]).AddDependency(stats["TotalOverallHealingOverHealing"]);
            ((TotalStat) oStats["RegularHealingOverHealingMonster"]).AddDependency(stats["RegularHealingOverHealing"]);
            ((TotalStat) oStats["CriticalHealingOverHealingMonster"]).AddDependency(stats["CriticalHealingOverHealing"]);

            ((TotalStat) oStats["TotalOverallHealingOverTimeMonster"]).AddDependency(stats["TotalOverallHealingOverTime"]);
            ((TotalStat) oStats["RegularHealingOverTimeMonster"]).AddDependency(stats["RegularHealingOverTime"]);
            ((TotalStat) oStats["CriticalHealingOverTimeMonster"]).AddDependency(stats["CriticalHealingOverTime"]);

            ((TotalStat) oStats["TotalOverallHealingMitigatedMonster"]).AddDependency(stats["TotalOverallHealingMitigated"]);
            ((TotalStat) oStats["RegularHealingMitigatedMonster"]).AddDependency(stats["RegularHealingMitigated"]);
            ((TotalStat) oStats["CriticalHealingMitigatedMonster"]).AddDependency(stats["CriticalHealingMitigated"]);

            #endregion

            #region Damage Taken

            ((TotalStat) oStats["TotalOverallDamageTakenMonster"]).AddDependency(stats["TotalOverallDamageTaken"]);
            ((TotalStat) oStats["RegularDamageTakenMonster"]).AddDependency(stats["RegularDamageTaken"]);
            ((TotalStat) oStats["CriticalDamageTakenMonster"]).AddDependency(stats["CriticalDamageTaken"]);

            ((TotalStat) oStats["TotalOverallDamageTakenOverTimeMonster"]).AddDependency(stats["TotalOverallDamageTakenOverTime"]);
            ((TotalStat) oStats["RegularDamageTakenOverTimeMonster"]).AddDependency(stats["RegularDamageTakenOverTime"]);
            ((TotalStat) oStats["CriticalDamageTakenOverTimeMonster"]).AddDependency(stats["CriticalDamageTakenOverTime"]);

            #endregion

            #region Global Percent Of Total Stats

            stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], (TotalStat) oStats["TotalOverallDamageMonster"]));
            stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], (TotalStat) oStats["RegularDamageMonster"]));
            stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], (TotalStat) oStats["CriticalDamageMonster"]));

            stats.Add("PercentOfTotalOverallDamageOverTime", new PercentStat("PercentOfTotalOverallDamageOverTime", stats["TotalOverallDamageOverTime"], (TotalStat) oStats["TotalOverallDamageOverTimeMonster"]));
            stats.Add("PercentOfRegularDamageOverTime", new PercentStat("PercentOfRegularDamageOverTime", stats["RegularDamageOverTime"], (TotalStat) oStats["RegularDamageOverTimeMonster"]));
            stats.Add("PercentOfCriticalDamageOverTime", new PercentStat("PercentOfCriticalDamageOverTime", stats["CriticalDamageOverTime"], (TotalStat) oStats["CriticalDamageOverTimeMonster"]));

            // healing
            stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], (TotalStat) oStats["TotalOverallHealingMonster"]));
            stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], (TotalStat) oStats["RegularHealingMonster"]));
            stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], (TotalStat) oStats["CriticalHealingMonster"]));

            stats.Add("PercentOfTotalOverallHealingOverHealing", new PercentStat("PercentOfTotalOverallHealingOverHealing", stats["TotalOverallHealingOverHealing"], (TotalStat) oStats["TotalOverallHealingOverHealingMonster"]));
            stats.Add("PercentOfRegularHealingOverHealing", new PercentStat("PercentOfRegularHealingOverHealing", stats["RegularHealingOverHealing"], (TotalStat) oStats["RegularHealingOverHealingMonster"]));
            stats.Add("PercentOfCriticalHealingOverHealing", new PercentStat("PercentOfCriticalHealingOverHealing", stats["CriticalHealingOverHealing"], (TotalStat) oStats["CriticalHealingOverHealingMonster"]));

            stats.Add("PercentOfTotalOverallHealingOverTime", new PercentStat("PercentOfTotalOverallHealingOverTime", stats["TotalOverallHealingOverTime"], (TotalStat) oStats["TotalOverallHealingOverTimeMonster"]));
            stats.Add("PercentOfRegularHealingOverTime", new PercentStat("PercentOfRegularHealingOverTime", stats["RegularHealingOverTime"], (TotalStat) oStats["RegularHealingOverTimeMonster"]));
            stats.Add("PercentOfCriticalHealingOverTime", new PercentStat("PercentOfCriticalHealingOverTime", stats["CriticalHealingOverTime"], (TotalStat) oStats["CriticalHealingOverTimeMonster"]));

            stats.Add("PercentOfTotalOverallHealingMitigated", new PercentStat("PercentOfTotalOverallHealingMitigated", stats["TotalOverallHealingMitigated"], (TotalStat) oStats["TotalOverallHealingMitigatedMonster"]));
            stats.Add("PercentOfRegularHealingMitigated", new PercentStat("PercentOfRegularHealingMitigated", stats["RegularHealingMitigated"], (TotalStat) oStats["RegularHealingMitigatedMonster"]));
            stats.Add("PercentOfCriticalHealingMitigated", new PercentStat("PercentOfCriticalHealingMitigated", stats["CriticalHealingMitigated"], (TotalStat) oStats["CriticalHealingMitigatedMonster"]));

            // damage taken
            stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], (TotalStat) oStats["TotalOverallDamageTakenMonster"]));
            stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], (TotalStat) oStats["RegularDamageTakenMonster"]));
            stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], (TotalStat) oStats["CriticalDamageTakenMonster"]));

            stats.Add("PercentOfTotalOverallDamageTakenOverTime", new PercentStat("PercentOfTotalOverallDamageTakenOverTime", stats["TotalOverallDamageTakenOverTime"], (TotalStat) oStats["TotalOverallDamageTakenOverTimeMonster"]));
            stats.Add("PercentOfRegularDamageTakenOverTime", new PercentStat("PercentOfRegularDamageTakenOverTime", stats["RegularDamageTakenOverTime"], (TotalStat) oStats["RegularDamageTakenOverTimeMonster"]));
            stats.Add("PercentOfCriticalDamageTakenOverTime", new PercentStat("PercentOfCriticalDamageTakenOverTime", stats["CriticalDamageTakenOverTime"], (TotalStat) oStats["CriticalDamageTakenOverTimeMonster"]));

            #endregion

            #region Monster Combined

            // ((TotalStat) stats["CombinedTotalOverallDamage"]).AddDependency(stats["TotalOverallDamage"]);
            // ((TotalStat) stats["CombinedTotalOverallDamage"]).AddDependency(stats["TotalOverallDamageOverTime"]);
            // ((TotalStat) stats["CombinedCriticalDamage"]).AddDependency(stats["CriticalDamage"]);
            // ((TotalStat) stats["CombinedRegularDamage"]).AddDependency(stats["RegularDamage"]);

            // ((TotalStat) stats["CombinedTotalOverallHealing"]).AddDependency(stats["TotalOverallHealing"]);
            // ((TotalStat) stats["CombinedTotalOverallHealing"]).AddDependency(stats["TotalOverallHealingOverTime"]);
            // ((TotalStat) stats["CombinedTotalOverallHealing"]).AddDependency(stats["TotalOverallHealingMitigated"]);
            // ((TotalStat) stats["CombinedCriticalHealing"]).AddDependency(stats["CriticalHealing"]);
            // ((TotalStat) stats["CombinedRegularHealing"]).AddDependency(stats["RegularHealing"]);

            // ((TotalStat) stats["CombinedTotalOverallDamageTaken"]).AddDependency(stats["TotalOverallDamageTaken"]);
            // ((TotalStat) stats["CombinedTotalOverallDamageTaken"]).AddDependency(stats["TotalOverallDamageTakenOverTime"]);
            // ((TotalStat) stats["CombinedCriticalDamageTaken"]).AddDependency(stats["CriticalDamageTaken"]);
            // ((TotalStat) stats["CombinedRegularDamageTaken"]).AddDependency(stats["RegularDamageTaken"]);
            ((TotalStat) stats["CombinedTotalOverallDamage"]).AddDependency(stats["TotalOverallDamage"]);
            ((TotalStat) stats["CombinedRegularDamage"]).AddDependency(stats["RegularDamage"]);
            ((TotalStat) stats["CombinedCriticalDamage"]).AddDependency(stats["CriticalDamage"]);
            ((MinStat) stats["CombinedDamageRegLow"]).AddDependency(stats["DamageRegLow"]);
            ((MaxStat) stats["CombinedDamageRegHigh"]).AddDependency(stats["DamageRegHigh"]);
            ((AverageStat) stats["CombinedDamageRegAverage"]).AddDependency(stats["DamageRegAverage"]);
            ((TotalStat) stats["CombinedDamageRegMod"]).AddDependency(stats["DamageRegMod"]);
            ((AverageStat) stats["CombinedDamageRegModAverage"]).AddDependency(stats["DamageRegModAverage"]);
            ((MinStat) stats["CombinedDamageCritLow"]).AddDependency(stats["DamageCritLow"]);
            ((MaxStat) stats["CombinedDamageCritHigh"]).AddDependency(stats["DamageCritHigh"]);
            ((AverageStat) stats["CombinedDamageCritAverage"]).AddDependency(stats["DamageCritAverage"]);
            ((TotalStat) stats["CombinedDamageCritMod"]).AddDependency(stats["DamageCritMod"]);
            ((AverageStat) stats["CombinedDamageCritModAverage"]).AddDependency(stats["DamageCritModAverage"]);

            ((TotalStat) stats["CombinedTotalOverallDamage"]).AddDependency(stats["TotalOverallDamageOverTime"]);
            ((TotalStat) stats["CombinedRegularDamage"]).AddDependency(stats["RegularDamageOverTime"]);
            ((TotalStat) stats["CombinedCriticalDamage"]).AddDependency(stats["CriticalDamageOverTime"]);
            ((MinStat) stats["CombinedDamageRegLow"]).AddDependency(stats["DamageOverTimeRegLow"]);
            ((MaxStat) stats["CombinedDamageRegHigh"]).AddDependency(stats["DamageOverTimeRegHigh"]);
            ((AverageStat) stats["CombinedDamageRegAverage"]).AddDependency(stats["DamageOverTimeRegAverage"]);
            ((TotalStat) stats["CombinedDamageRegMod"]).AddDependency(stats["DamageOverTimeRegMod"]);
            ((AverageStat) stats["CombinedDamageRegModAverage"]).AddDependency(stats["DamageOverTimeRegModAverage"]);
            ((MinStat) stats["CombinedDamageCritLow"]).AddDependency(stats["DamageOverTimeCritLow"]);
            ((MaxStat) stats["CombinedDamageCritHigh"]).AddDependency(stats["DamageOverTimeCritHigh"]);
            ((AverageStat) stats["CombinedDamageCritAverage"]).AddDependency(stats["DamageOverTimeCritAverage"]);
            ((TotalStat) stats["CombinedDamageCritMod"]).AddDependency(stats["DamageOverTimeCritMod"]);
            ((AverageStat) stats["CombinedDamageCritModAverage"]).AddDependency(stats["DamageOverTimeCritModAverage"]);

            ((TotalStat) stats["CombinedTotalOverallHealing"]).AddDependency(stats["TotalOverallHealing"]);
            ((TotalStat) stats["CombinedRegularHealing"]).AddDependency(stats["RegularHealing"]);
            ((TotalStat) stats["CombinedCriticalHealing"]).AddDependency(stats["CriticalHealing"]);
            ((MinStat) stats["CombinedHealingRegLow"]).AddDependency(stats["HealingRegLow"]);
            ((MaxStat) stats["CombinedHealingRegHigh"]).AddDependency(stats["HealingRegHigh"]);
            ((AverageStat) stats["CombinedHealingRegAverage"]).AddDependency(stats["HealingRegAverage"]);
            ((TotalStat) stats["CombinedHealingRegMod"]).AddDependency(stats["HealingRegMod"]);
            ((AverageStat) stats["CombinedHealingRegModAverage"]).AddDependency(stats["HealingRegModAverage"]);
            ((MinStat) stats["CombinedHealingCritLow"]).AddDependency(stats["HealingCritLow"]);
            ((MaxStat) stats["CombinedHealingCritHigh"]).AddDependency(stats["HealingCritHigh"]);
            ((AverageStat) stats["CombinedHealingCritAverage"]).AddDependency(stats["HealingCritAverage"]);
            ((TotalStat) stats["CombinedHealingCritMod"]).AddDependency(stats["HealingCritMod"]);
            ((AverageStat) stats["CombinedHealingCritModAverage"]).AddDependency(stats["HealingCritModAverage"]);

            ((TotalStat) stats["CombinedTotalOverallHealing"]).AddDependency(stats["TotalOverallHealingOverTime"]);
            ((TotalStat) stats["CombinedRegularHealing"]).AddDependency(stats["RegularHealingOverTime"]);
            ((TotalStat) stats["CombinedCriticalHealing"]).AddDependency(stats["CriticalHealingOverTime"]);
            ((MinStat) stats["CombinedHealingRegLow"]).AddDependency(stats["HealingOverTimeRegLow"]);
            ((MaxStat) stats["CombinedHealingRegHigh"]).AddDependency(stats["HealingOverTimeRegHigh"]);
            ((AverageStat) stats["CombinedHealingRegAverage"]).AddDependency(stats["HealingOverTimeRegAverage"]);
            ((TotalStat) stats["CombinedHealingRegMod"]).AddDependency(stats["HealingOverTimeRegMod"]);
            ((AverageStat) stats["CombinedHealingRegModAverage"]).AddDependency(stats["HealingOverTimeRegModAverage"]);
            ((MinStat) stats["CombinedHealingCritLow"]).AddDependency(stats["HealingOverTimeCritLow"]);
            ((MaxStat) stats["CombinedHealingCritHigh"]).AddDependency(stats["HealingOverTimeCritHigh"]);
            ((AverageStat) stats["CombinedHealingCritAverage"]).AddDependency(stats["HealingOverTimeCritAverage"]);
            ((TotalStat) stats["CombinedHealingCritMod"]).AddDependency(stats["HealingOverTimeCritMod"]);
            ((AverageStat) stats["CombinedHealingCritModAverage"]).AddDependency(stats["HealingOverTimeCritModAverage"]);

            ((TotalStat) stats["CombinedTotalOverallHealing"]).AddDependency(stats["TotalOverallHealingMitigated"]);
            ((TotalStat) stats["CombinedRegularHealing"]).AddDependency(stats["RegularHealingMitigated"]);
            ((TotalStat) stats["CombinedCriticalHealing"]).AddDependency(stats["CriticalHealingMitigated"]);
            ((MinStat) stats["CombinedHealingRegLow"]).AddDependency(stats["HealingMitigatedRegLow"]);
            ((MaxStat) stats["CombinedHealingRegHigh"]).AddDependency(stats["HealingMitigatedRegHigh"]);
            ((AverageStat) stats["CombinedHealingRegAverage"]).AddDependency(stats["HealingMitigatedRegAverage"]);
            ((TotalStat) stats["CombinedHealingRegMod"]).AddDependency(stats["HealingMitigatedRegMod"]);
            ((AverageStat) stats["CombinedHealingRegModAverage"]).AddDependency(stats["HealingMitigatedRegModAverage"]);
            ((MinStat) stats["CombinedHealingCritLow"]).AddDependency(stats["HealingMitigatedCritLow"]);
            ((MaxStat) stats["CombinedHealingCritHigh"]).AddDependency(stats["HealingMitigatedCritHigh"]);
            ((AverageStat) stats["CombinedHealingCritAverage"]).AddDependency(stats["HealingMitigatedCritAverage"]);
            ((TotalStat) stats["CombinedHealingCritMod"]).AddDependency(stats["HealingMitigatedCritMod"]);
            ((AverageStat) stats["CombinedHealingCritModAverage"]).AddDependency(stats["HealingMitigatedCritModAverage"]);

            ((TotalStat) stats["CombinedTotalOverallDamageTaken"]).AddDependency(stats["TotalOverallDamageTaken"]);
            ((TotalStat) stats["CombinedRegularDamageTaken"]).AddDependency(stats["RegularDamageTaken"]);
            ((TotalStat) stats["CombinedCriticalDamageTaken"]).AddDependency(stats["CriticalDamageTaken"]);
            ((MinStat) stats["CombinedDamageTakenRegLow"]).AddDependency(stats["DamageTakenRegLow"]);
            ((MaxStat) stats["CombinedDamageTakenRegHigh"]).AddDependency(stats["DamageTakenRegHigh"]);
            ((AverageStat) stats["CombinedDamageTakenRegAverage"]).AddDependency(stats["DamageTakenRegAverage"]);
            ((TotalStat) stats["CombinedDamageTakenRegMod"]).AddDependency(stats["DamageTakenRegMod"]);
            ((AverageStat) stats["CombinedDamageTakenRegModAverage"]).AddDependency(stats["DamageTakenRegModAverage"]);
            ((MinStat) stats["CombinedDamageTakenCritLow"]).AddDependency(stats["DamageTakenCritLow"]);
            ((MaxStat) stats["CombinedDamageTakenCritHigh"]).AddDependency(stats["DamageTakenCritHigh"]);
            ((AverageStat) stats["CombinedDamageTakenCritAverage"]).AddDependency(stats["DamageTakenCritAverage"]);
            ((TotalStat) stats["CombinedDamageTakenCritMod"]).AddDependency(stats["DamageTakenCritMod"]);
            ((AverageStat) stats["CombinedDamageTakenCritModAverage"]).AddDependency(stats["DamageTakenCritModAverage"]);

            ((TotalStat) stats["CombinedTotalOverallDamageTaken"]).AddDependency(stats["TotalOverallDamageTakenOverTime"]);
            ((TotalStat) stats["CombinedRegularDamageTaken"]).AddDependency(stats["RegularDamageTakenOverTime"]);
            ((TotalStat) stats["CombinedCriticalDamageTaken"]).AddDependency(stats["CriticalDamageTakenOverTime"]);
            ((MinStat) stats["CombinedDamageTakenRegLow"]).AddDependency(stats["DamageTakenOverTimeRegLow"]);
            ((MaxStat) stats["CombinedDamageTakenRegHigh"]).AddDependency(stats["DamageTakenOverTimeRegHigh"]);
            ((AverageStat) stats["CombinedDamageTakenRegAverage"]).AddDependency(stats["DamageTakenOverTimeRegAverage"]);
            ((TotalStat) stats["CombinedDamageTakenRegMod"]).AddDependency(stats["DamageTakenOverTimeRegMod"]);
            ((AverageStat) stats["CombinedDamageTakenRegModAverage"]).AddDependency(stats["DamageTakenOverTimeRegModAverage"]);
            ((MinStat) stats["CombinedDamageTakenCritLow"]).AddDependency(stats["DamageTakenOverTimeCritLow"]);
            ((MaxStat) stats["CombinedDamageTakenCritHigh"]).AddDependency(stats["DamageTakenOverTimeCritHigh"]);
            ((AverageStat) stats["CombinedDamageTakenCritAverage"]).AddDependency(stats["DamageTakenOverTimeCritAverage"]);
            ((TotalStat) stats["CombinedDamageTakenCritMod"]).AddDependency(stats["DamageTakenOverTimeCritMod"]);
            ((AverageStat) stats["CombinedDamageTakenCritModAverage"]).AddDependency(stats["DamageTakenOverTimeCritModAverage"]);

            ((PerSecondAverageStat) stats["CombinedDPS"]).AddDependency(stats["CombinedTotalOverallDamage"]);
            ((PerSecondAverageStat) stats["CombinedHPS"]).AddDependency(stats["CombinedTotalOverallHealing"]);
            ((PerSecondAverageStat) stats["CombinedDTPS"]).AddDependency(stats["CombinedTotalOverallDamageTaken"]);

            #endregion

            #region Global Combined

            ((TotalStat) oStats["CombinedTotalOverallDamageMonster"]).AddDependency(stats["CombinedTotalOverallDamage"]);
            ((TotalStat) oStats["CombinedRegularDamageMonster"]).AddDependency(stats["CombinedRegularDamage"]);
            ((TotalStat) oStats["CombinedCriticalDamageMonster"]).AddDependency(stats["CombinedCriticalDamage"]);

            ((TotalStat) oStats["CombinedTotalOverallHealingMonster"]).AddDependency(stats["CombinedTotalOverallHealing"]);
            ((TotalStat) oStats["CombinedRegularHealingMonster"]).AddDependency(stats["CombinedRegularHealing"]);
            ((TotalStat) oStats["CombinedCriticalHealingMonster"]).AddDependency(stats["CombinedCriticalHealing"]);

            ((TotalStat) oStats["CombinedTotalOverallDamageTakenMonster"]).AddDependency(stats["CombinedTotalOverallDamageTaken"]);
            ((TotalStat) oStats["CombinedRegularDamageTakenMonster"]).AddDependency(stats["CombinedRegularDamageTaken"]);
            ((TotalStat) oStats["CombinedCriticalDamageTakenMonster"]).AddDependency(stats["CombinedCriticalDamageTaken"]);

            #endregion

            return stats.Select(s => s.Value).ToList();
        }
    }
}