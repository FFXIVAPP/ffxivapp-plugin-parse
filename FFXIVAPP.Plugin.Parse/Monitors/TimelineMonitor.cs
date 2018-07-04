// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimelineMonitor.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   TimelineMonitor.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Monitors {
    using System.Text.RegularExpressions;

    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.Events;
    using FFXIVAPP.Plugin.Parse.Models.Fights;
    using FFXIVAPP.Plugin.Parse.Models.StatGroups;
    using FFXIVAPP.Plugin.Parse.RegularExpressions;

    using NLog;

    using Sharlayan.Extensions;

    public class TimelineMonitor : EventMonitor {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="parseControl"> </param>
        public TimelineMonitor(ParseControl parseControl)
            : base("Timeline", parseControl) {
            this.Filter = EventParser.SubjectMask | EventParser.DirectionMask | (ulong) EventType.Loot | (ulong) EventType.Defeats;
        }

        private Expressions Expressions { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Event e) {
            this.Expressions = new Expressions(e);

            if (string.IsNullOrWhiteSpace(e.ChatLogItem.Line)) {
                return;
            }

            switch (e.Type) {
                case EventType.Defeats:
                    this.ProcessDefeated(e);
                    break;
                case EventType.Loot:
                    this.ProcessLoot(e);
                    break;
            }
        }

        protected override void HandleUnknownEvent(Event e) {
            ParsingLogHelper.Log(Logger, "UnknownEvent", e);
        }

        /// <summary>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        private void AddKillToPartyMonster(string target, string source) {
            var monsterName = target.Trim();
            this.ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyMonsterKilled, monsterName);
        }

        /// <summary>
        /// </summary>
        /// <param name="thing"> </param>
        /// <param name="e"></param>
        private void AttachDropToPartyMonster(string thing, Event e) {
            var monsterName = this.ParseControl.Timeline.FightingRightNow
                                  ? this.ParseControl.Timeline.LastEngaged
                                  : string.Empty;
            if (ParseControl.Instance.Timeline.FightingRightNow) {
                Fight fight;
                if (this.ParseControl.Timeline.Fights.TryGet(this.ParseControl.Timeline.LastEngaged, out fight)) {
                    monsterName = fight.MonsterName;
                    if (monsterName.Replace(" ", string.Empty) != string.Empty) {
                        Monster monsterGroup = this.ParseControl.Timeline.GetSetMonster(monsterName);
                        monsterGroup.SetDrop(thing);
                    }
                }
            }
            else {
                ParsingLogHelper.Log(Logger, "Loot.NoKillInLastThreeSeconds", e);
            }
        }

        /// <summary>
        /// </summary>
        private void ProcessDefeated(Event e) {
            Match matches;
            var you = Constants.CharacterName;
            switch (Constants.GameLanguage) {
                case "French":
                    matches = PlayerRegEx.DefeatsFr.Match(e.ChatLogItem.Line);
                    break;
                case "Japanese":
                    matches = PlayerRegEx.DefeatsJa.Match(e.ChatLogItem.Line);
                    break;
                case "German":
                    matches = PlayerRegEx.DefeatsDe.Match(e.ChatLogItem.Line);
                    break;
                case "Chinese":
                    matches = PlayerRegEx.DefeatsZh.Match(e.ChatLogItem.Line);
                    break;
                default:
                    matches = PlayerRegEx.DefeatsEn.Match(e.ChatLogItem.Line);
                    break;
            }

            if (!matches.Success) {
                this.ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyMonsterKilled, string.Empty);
                ParsingLogHelper.Log(Logger, "Defeat", e);
                return;
            }

            Group target = matches.Groups["target"];
            Group source = matches.Groups["source"];
            if (!target.Success) {
                return;
            }

            if (this.ParseControl.Timeline.Party.HasGroup(target.Value) || Regex.IsMatch(target.Value, this.Expressions.You) || target.Value == you) {
                return;
            }

            var targetName = target.Value.ToTitleCase();
            var sourceName = (source.Success
                                  ? source.Value
                                  : "Unknown").ToTitleCase();
            this.AddKillToPartyMonster(targetName, sourceName);
        }

        /// <summary>
        /// </summary>
        private void ProcessLoot(Event e) {
            Match matches;
            switch (Constants.GameLanguage) {
                case "French":
                    matches = PlayerRegEx.ObtainsFr.Match(e.ChatLogItem.Line);
                    break;
                case "Japanese":
                    matches = PlayerRegEx.ObtainsJa.Match(e.ChatLogItem.Line);
                    break;
                case "German":
                    matches = PlayerRegEx.ObtainsDe.Match(e.ChatLogItem.Line);
                    break;
                case "Chinese":
                    matches = PlayerRegEx.ObtainsZh.Match(e.ChatLogItem.Line);
                    break;
                default:
                    matches = PlayerRegEx.ObtainsEn.Match(e.ChatLogItem.Line);
                    break;
            }

            if (!matches.Success) {
                ParsingLogHelper.Log(Logger, "Loot", e);
                return;
            }

            var thing = matches.Groups["item"].Value.ToTitleCase();
            this.AttachDropToPartyMonster(thing, e);
        }
    }
}