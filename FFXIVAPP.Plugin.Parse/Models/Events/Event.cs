// FFXIVAPP.Plugin.Parse ~ Event.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Memory.Core;
using FFXIVAPP.Plugin.Parse.Enums;

namespace FFXIVAPP.Plugin.Parse.Models.Events
{
    public class Event : EventArgs, INotifyPropertyChanged
    {
        public Event(EventCode eventCode = null, ChatLogEntry chatLogEntry = null)
        {
            Initialize(DateTime.Now, eventCode, chatLogEntry);
        }

        private void Initialize(DateTime timeStamp, EventCode eventCode, ChatLogEntry chatLogEntry)
        {
            Timestamp = timeStamp;
            EventCode = eventCode;
            ChatLogEntry = chatLogEntry;
        }

        #region Utility Functions

        /// <summary>
        /// </summary>
        /// <param name="filter"> </param>
        /// <returns> </returns>
        public bool MatchesFilter(UInt64 filter, Event e)
        {
            return (((UInt64) Subject & filter) != 0 && ((UInt64) Type & filter) != 0 && ((UInt64) Direction & filter) != 0);
        }

        #endregion

        #region Property Bindings

        private ChatLogEntry _chatLogEntry;
        private EventCode _eventCode;
        private DateTime _timestamp;

        public DateTime Timestamp
        {
            get { return _timestamp; }
            private set
            {
                _timestamp = value;
                RaisePropertyChanged();
            }
        }

        private EventCode EventCode
        {
            get { return _eventCode; }
            set
            {
                _eventCode = value;
                RaisePropertyChanged();
            }
        }

        public ChatLogEntry ChatLogEntry
        {
            get { return _chatLogEntry; }
            set
            {
                _chatLogEntry = value;
                RaisePropertyChanged();
            }
        }

        public EventSubject Subject
        {
            get { return EventCode != null ? EventCode.Subject : EventSubject.Unknown; }
        }

        public EventType Type
        {
            get { return EventCode != null ? EventCode.Type : EventType.Unknown; }
        }

        public EventDirection Direction
        {
            get { return EventCode != null ? EventCode.Direction : EventDirection.Unknown; }
        }

        public ulong Code
        {
            get { return (EventCode != null ? EventCode.Code : 0x0); }
        }

        public bool IsUnknown
        {
            get { return (EventCode == null) || (EventCode.Flags == EventParser.UnknownEvent); }
        }

        #endregion

        #region Equality Methods

        /// <summary>
        /// </summary>
        /// <param name="event1"> </param>
        /// <param name="event2"> </param>
        /// <returns> </returns>
        public static bool operator ==(Event event1, Event event2)
        {
            return event2 != null && (event1 != null && ((event1.Timestamp == event2.Timestamp) && new EventCodeComparer().Equals(event1.EventCode, event2.EventCode)));
        }

        /// <summary>
        /// </summary>
        /// <param name="event1"> </param>
        /// <param name="event2"> </param>
        /// <returns> </returns>
        public static bool operator !=(Event event1, Event event2)
        {
            return !(event1 == event2);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public override bool Equals(object source)
        {
            return source is Event ? this == (Event) source : base.Equals(source);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public override int GetHashCode()
        {
            return (Timestamp.GetHashCode() ^ Subject.GetHashCode() ^ Type.GetHashCode() ^ Direction.GetHashCode());
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
