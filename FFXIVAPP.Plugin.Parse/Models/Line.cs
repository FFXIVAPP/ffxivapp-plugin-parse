// FFXIVAPP.Plugin.Parse ~ Line.cs
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
using System.Text.RegularExpressions;
using FFXIVAPP.Memory.Core;
using FFXIVAPP.Memory.Helpers;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models.Events;

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class Line
    {
        // battle data
        public Line(ChatLogEntry chatLogEntry = null)
        {
            if (chatLogEntry != null)
            {
                ChatLogEntry = chatLogEntry;
            }
        }

        public bool XOverTime { get; set; }
        public double Amount { get; set; }
        public double Modifier { get; set; }
        public string RecLossType { get; set; }
        public bool Hit { get; set; }
        public bool Miss { get; set; }
        public bool Crit { get; set; }
        public bool Counter { get; set; }
        public bool Block { get; set; }
        public bool Parry { get; set; }
        public bool Resist { get; set; }
        public bool Evade { get; set; }
        public ChatLogEntry ChatLogEntry { get; set; }

        public bool IsEmpty()
        {
            return String.IsNullOrWhiteSpace(Source) || String.IsNullOrWhiteSpace(Target) || String.IsNullOrWhiteSpace(Action);
        }

        private bool IsYou(string name)
        {
            return Regex.IsMatch(name, @"^(([Dd](ich|ie|u))|You|Vous)$") || String.Equals(name, Constants.CharacterName, Constants.InvariantComparer);
        }

        #region NPC ActorEntity Information

        public ActorEntity SourceEntity { get; set; }
        public ActorEntity TargetEntity { get; set; }

        public List<StatusEntry> SourceStatusEntries
        {
            get { return _sourceStatusEntries ?? (_sourceStatusEntries = new List<StatusEntry>()); }
            set { _sourceStatusEntries = value; }
        }

        public List<StatusEntry> TargetStatusEntries
        {
            get { return _targetStatusEntries ?? (_targetStatusEntries = new List<StatusEntry>()); }
            set { _targetStatusEntries = value; }
        }

        #endregion

        #region Event Information

        public EventDirection EventDirection { get; set; }
        public EventSubject EventSubject { get; set; }
        public EventType EventType { get; set; }

        #endregion

        #region Type Information

        public TimelineType SourceTimelineType { get; set; }
        public TimelineType TargetTimelineType { get; set; }

        #endregion

        #region Property Backings

        private string _action;
        private string _direction;
        private string _part;
        private string _source;
        private List<StatusEntry> _sourceStatusEntries;
        private string _target;
        private List<StatusEntry> _targetStatusEntries;

        public string Source
        {
            get { return _source ?? ""; }
            set
            {
                var name = StringHelper.TitleCase(value);
                _source = ParseHelper.GetTaggedName(name, new Expressions(new Event()), SourceTimelineType);
            }
        }

        public string Target
        {
            get { return _target ?? ""; }
            set
            {
                var name = StringHelper.TitleCase(value);
                _target = ParseHelper.GetTaggedName(name, new Expressions(new Event()), TargetTimelineType);
            }
        }

        public string Action
        {
            get { return _action ?? ""; }
            set { _action = StringHelper.TitleCase(value); }
        }

        public string Direction
        {
            get { return _direction ?? ""; }
            set { _direction = StringHelper.TitleCase(value); }
        }

        public string Part
        {
            get { return _part ?? ""; }
            set { _part = StringHelper.TitleCase(value); }
        }

        #endregion
    }
}
