// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Event.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Event.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Events {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using FFXIVAPP.Plugin.Parse.Enums;

    using Sharlayan.Core;

    public class Event : EventArgs, INotifyPropertyChanged {
        private ChatLogItem _chatLogItem;

        private EventCode _eventCode;

        private DateTime _timestamp;

        public Event(EventCode eventCode = null, ChatLogItem chatLogItem = null) {
            this.Initialize(DateTime.Now, eventCode, chatLogItem);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ChatLogItem ChatLogItem {
            get {
                return this._chatLogItem;
            }

            set {
                this._chatLogItem = value;
                this.RaisePropertyChanged();
            }
        }

        public ulong Code {
            get {
                return this.EventCode != null
                           ? this.EventCode.Code
                           : 0x0;
            }
        }

        public EventDirection Direction {
            get {
                return this.EventCode != null
                           ? this.EventCode.Direction
                           : EventDirection.Unknown;
            }
        }

        public bool IsUnknown {
            get {
                return this.EventCode == null || this.EventCode.Flags == EventParser.UnknownEvent;
            }
        }

        public EventSubject Subject {
            get {
                return this.EventCode != null
                           ? this.EventCode.Subject
                           : EventSubject.Unknown;
            }
        }

        public DateTime Timestamp {
            get {
                return this._timestamp;
            }

            private set {
                this._timestamp = value;
                this.RaisePropertyChanged();
            }
        }

        public EventType Type {
            get {
                return this.EventCode != null
                           ? this.EventCode.Type
                           : EventType.Unknown;
            }
        }

        private EventCode EventCode {
            get {
                return this._eventCode;
            }

            set {
                this._eventCode = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="event1"> </param>
        /// <param name="event2"> </param>
        /// <returns> </returns>
        public static bool operator ==(Event event1, Event event2) {
            return event2 != null && event1 != null && event1.Timestamp == event2.Timestamp && new EventCodeComparer().Equals(event1.EventCode, event2.EventCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="event1"> </param>
        /// <param name="event2"> </param>
        /// <returns> </returns>
        public static bool operator !=(Event event1, Event event2) {
            return !(event1 == event2);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public override bool Equals(object source) {
            return source is Event
                       ? this == (Event) source
                       : base.Equals(source);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public override int GetHashCode() {
            return this.Timestamp.GetHashCode() ^ this.Subject.GetHashCode() ^ this.Type.GetHashCode() ^ this.Direction.GetHashCode();
        }

        /// <summary>
        /// </summary>
        /// <param name="filter"> </param>
        /// <returns> </returns>
        public bool MatchesFilter(ulong filter, Event e) {
            return ((ulong) this.Subject & filter) != 0 && ((ulong) this.Type & filter) != 0 && ((ulong) this.Direction & filter) != 0;
        }

        private void Initialize(DateTime timeStamp, EventCode eventCode, ChatLogItem chatLogItem) {
            this.Timestamp = timeStamp;
            this.EventCode = eventCode;
            this.ChatLogItem = chatLogItem;
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}