// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Line.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Line.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models.Events;

    using Sharlayan.Core;
    using Sharlayan.Extensions;

    public class Line {
        private string _action;

        private string _direction;

        private string _part;

        private string _source;

        private List<StatusItem> _sourceStatusEntries;

        private string _target;

        private List<StatusItem> _targetStatusEntries;

        // battle data
        public Line(ChatLogItem chatLogItem = null) {
            if (chatLogItem != null) {
                this.ChatLogItem = chatLogItem;
            }
        }

        public string Action {
            get {
                return this._action ?? string.Empty;
            }

            set {
                this._action = value.ToTitleCase();
            }
        }

        public double Amount { get; set; }

        public bool Block { get; set; }

        public ChatLogItem ChatLogItem { get; set; }

        public bool Counter { get; set; }

        public bool Crit { get; set; }

        public bool DirectHit { get; set; }

        public string Direction {
            get {
                return this._direction ?? string.Empty;
            }

            set {
                this._direction = value.ToTitleCase();
            }
        }

        public bool Evade { get; set; }

        public EventDirection EventDirection { get; set; }

        public EventSubject EventSubject { get; set; }

        public EventType EventType { get; set; }

        public bool Hit { get; set; }

        public bool Miss { get; set; }

        public double Modifier { get; set; }

        public bool Parry { get; set; }

        public string Part {
            get {
                return this._part ?? string.Empty;
            }

            set {
                this._part = value.ToTitleCase();
            }
        }

        public string RecLossType { get; set; }

        public bool Resist { get; set; }

        public string Source {
            get {
                return this._source ?? string.Empty;
            }

            set {
                var name = value.ToTitleCase();
                this._source = ParseHelper.GetTaggedName(name, new Expressions(new Event()), this.SourceTimelineType);
            }
        }

        public ActorItem SourceEntity { get; set; }

        public List<StatusItem> SourceStatusEntries {
            get {
                return this._sourceStatusEntries ?? (this._sourceStatusEntries = new List<StatusItem>());
            }

            set {
                this._sourceStatusEntries = value;
            }
        }

        public TimelineType SourceTimelineType { get; set; }

        public string Target {
            get {
                return this._target ?? string.Empty;
            }

            set {
                var name = value.ToTitleCase();
                this._target = ParseHelper.GetTaggedName(name, new Expressions(new Event()), this.TargetTimelineType);
            }
        }

        public ActorItem TargetEntity { get; set; }

        public List<StatusItem> TargetStatusEntries {
            get {
                return this._targetStatusEntries ?? (this._targetStatusEntries = new List<StatusItem>());
            }

            set {
                this._targetStatusEntries = value;
            }
        }

        public TimelineType TargetTimelineType { get; set; }

        public bool XOverTime { get; set; }

        public bool IsEmpty() {
            return string.IsNullOrWhiteSpace(this.Source) || string.IsNullOrWhiteSpace(this.Target) || string.IsNullOrWhiteSpace(this.Action);
        }

        private bool IsYou(string name) {
            return Regex.IsMatch(name, @"^(([Dd](ich|ie|u))|You|Vous)$") || string.Equals(name, Constants.CharacterName, Constants.InvariantComparer);
        }
    }
}