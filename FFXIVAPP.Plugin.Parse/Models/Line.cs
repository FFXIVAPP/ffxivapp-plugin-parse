// FFXIVAPP.Plugin.Parse
// Line.cs
// 
// Copyright � 2007 - 2015 Ryan Wilson - All Rights Reserved
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
