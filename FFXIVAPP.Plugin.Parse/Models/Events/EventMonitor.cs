// FFXIVAPP.Plugin.Parse ~ EventMonitor.cs
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.Events
{
    public class EventMonitor : StatGroup
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parseControl"> </param>
        protected EventMonitor(string name, ParseControl parseControl) : base(name)
        {
            Initialize(parseControl);
            EventParser.Instance.OnLogEvent += FilterEvent;
            EventParser.Instance.OnUnknownLogEvent += FilterUnknownEvent;
        }

        /// <summary>
        /// </summary>
        /// <param name="instance"> </param>
        private void Initialize(ParseControl instance)
        {
            ParseControl = instance;
            InitStats();
        }

        /// <summary>
        /// </summary>
        protected virtual void InitStats()
        {
            foreach (var stat in Stats)
            {
                stat.OnValueChanged += DoStatChanged;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="e"> </param>
        private void FilterEvent(object source, Event e)
        {
            if (!e.MatchesFilter(Filter, e))
            {
                return;
            }
            LastEventReceived = e.Timestamp;
            HandleEvent(e);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="e"> </param>
        private void FilterUnknownEvent(object source, Event e)
        {
            LastEventReceived = e.Timestamp;
            HandleUnknownEvent(e);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected virtual void HandleEvent(Event e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected virtual void HandleUnknownEvent(Event e)
        {
        }

        public event EventHandler<StatChangedEvent> OnStatChanged = delegate { };

        private void DoStatChanged(object source, StatChangedEvent e)
        {
            OnStatChanged(this, e);
        }

        #region Property Bindings

        private UInt64 _filter;
        private DateTime _lastEventReceived;
        private ParseControl _parseControl;

        private DateTime LastEventReceived
        {
            get { return _lastEventReceived; }
            set
            {
                _lastEventReceived = value;
                RaisePropertyChanged();
            }
        }

        protected internal UInt64 Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                RaisePropertyChanged();
            }
        }

        protected ParseControl ParseControl
        {
            get { return _parseControl; }
            private set
            {
                _parseControl = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged = delegate { };

        private new void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
