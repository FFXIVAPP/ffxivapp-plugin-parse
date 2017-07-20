// FFXIVAPP.Plugin.Parse ~ TimelineMonitor.cs
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
using System.Text.RegularExpressions;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Models.Fights;
using FFXIVAPP.Plugin.Parse.RegularExpressions;
using NLog;
using Sharlayan.Helpers;

namespace FFXIVAPP.Plugin.Parse.Monitors
{
    public class TimelineMonitor : EventMonitor
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="parseControl"> </param>
        public TimelineMonitor(ParseControl parseControl) : base("Timeline", parseControl)
        {
            Filter = EventParser.SubjectMask | EventParser.DirectionMask | (UInt64) EventType.Loot | (UInt64) EventType.Defeats;
        }

        private Expressions Expressions { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Event e)
        {
            Expressions = new Expressions(e);

            if (String.IsNullOrWhiteSpace(e.ChatLogEntry.Line))
            {
                return;
            }
            switch (e.Type)
            {
                case EventType.Defeats:
                    ProcessDefeated(e);
                    break;
                case EventType.Loot:
                    ProcessLoot(e);
                    break;
            }
        }

        protected override void HandleUnknownEvent(Event e)
        {
            ParsingLogHelper.Log(Logger, "UnknownEvent", e);
        }

        /// <summary>
        /// </summary>
        private void ProcessDefeated(Event e)
        {
            Match matches;
            var you = Constants.CharacterName;
            switch (Constants.GameLanguage)
            {
                case "French":
                    matches = PlayerRegEx.DefeatsFr.Match(e.ChatLogEntry.Line);
                    break;
                case "Japanese":
                    matches = PlayerRegEx.DefeatsJa.Match(e.ChatLogEntry.Line);
                    break;
                case "German":
                    matches = PlayerRegEx.DefeatsDe.Match(e.ChatLogEntry.Line);
                    break;
                case "Chinese":
                    matches = PlayerRegEx.DefeatsZh.Match(e.ChatLogEntry.Line);
                    break;
                default:
                    matches = PlayerRegEx.DefeatsEn.Match(e.ChatLogEntry.Line);
                    break;
            }
            if (!matches.Success)
            {
                ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyMonsterKilled, string.Empty);
                ParsingLogHelper.Log(Logger, "Defeat", e);
                return;
            }
            var target = matches.Groups["target"];
            var source = matches.Groups["source"];
            if (!target.Success)
            {
                return;
            }
            if (ParseControl.Timeline.Party.HasGroup(target.Value) || Regex.IsMatch(target.Value, Expressions.You) || target.Value == you)
            {
                return;
            }
            var targetName = StringHelper.TitleCase(target.Value);
            var sourceName = StringHelper.TitleCase(source.Success ? source.Value : "Unknown");
            AddKillToPartyMonster(targetName, sourceName);
        }

        /// <summary>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        private void AddKillToPartyMonster(string target, string source)
        {
            var monsterName = target.Trim();
            ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyMonsterKilled, monsterName);
        }

        /// <summary>
        /// </summary>
        private void ProcessLoot(Event e)
        {
            Match matches;
            switch (Constants.GameLanguage)
            {
                case "French":
                    matches = PlayerRegEx.ObtainsFr.Match(e.ChatLogEntry.Line);
                    break;
                case "Japanese":
                    matches = PlayerRegEx.ObtainsJa.Match(e.ChatLogEntry.Line);
                    break;
                case "German":
                    matches = PlayerRegEx.ObtainsDe.Match(e.ChatLogEntry.Line);
                    break;
                case "Chinese":
                    matches = PlayerRegEx.ObtainsZh.Match(e.ChatLogEntry.Line);
                    break;
                default:
                    matches = PlayerRegEx.ObtainsEn.Match(e.ChatLogEntry.Line);
                    break;
            }
            if (!matches.Success)
            {
                ParsingLogHelper.Log(Logger, "Loot", e);
                return;
            }
            var thing = StringHelper.TitleCase(matches.Groups["item"]
                                                      .Value);
            AttachDropToPartyMonster(thing, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="thing"> </param>
        /// <param name="e"></param>
        private void AttachDropToPartyMonster(string thing, Event e)
        {
            var monsterName = ParseControl.Timeline.FightingRightNow ? ParseControl.Timeline.LastEngaged : string.Empty;
            if (ParseControl.Instance.Timeline.FightingRightNow)
            {
                Fight fight;
                if (ParseControl.Timeline.Fights.TryGet(ParseControl.Timeline.LastEngaged, out fight))
                {
                    monsterName = fight.MonsterName;
                    if (monsterName.Replace(" ", string.Empty) != string.Empty)
                    {
                        var monsterGroup = ParseControl.Timeline.GetSetMonster(monsterName);
                        monsterGroup.SetDrop(thing);
                    }
                }
            }
            else
            {
                ParsingLogHelper.Log(Logger, "Loot.NoKillInLastThreeSeconds", e);
            }
        }
    }
}
