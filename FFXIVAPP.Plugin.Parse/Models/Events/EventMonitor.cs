// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMonitor.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   EventMonitor.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Events {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public class EventMonitor : StatGroup {
        private ulong _filter;

        private DateTime _lastEventReceived;

        private ParseControl _parseControl;

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parseControl"> </param>
        protected EventMonitor(string name, ParseControl parseControl) : base(name) {
            this.Initialize(parseControl);
            EventParser.Instance.OnLogEvent += this.FilterEvent;
            EventParser.Instance.OnUnknownLogEvent += this.FilterUnknownEvent;
        }

        public event EventHandler<StatChangedEvent> OnStatChanged = delegate { };

        public new event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected internal ulong Filter {
            get {
                return this._filter;
            }

            set {
                this._filter = value;
                this.RaisePropertyChanged();
            }
        }

        protected ParseControl ParseControl {
            get {
                return this._parseControl;
            }

            private set {
                this._parseControl = value;
                this.RaisePropertyChanged();
            }
        }

        private DateTime LastEventReceived {
            get {
                return this._lastEventReceived;
            }

            set {
                this._lastEventReceived = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected virtual void HandleEvent(Event e) { }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected virtual void HandleUnknownEvent(Event e) { }

        /// <summary>
        /// </summary>
        protected virtual void InitStats() {
            foreach (Stat<double> stat in this.Stats) {
                stat.OnValueChanged += this.DoStatChanged;
            }
        }

        private void DoStatChanged(object source, StatChangedEvent e) {
            this.OnStatChanged(this, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="e"> </param>
        private void FilterEvent(object source, Event e) {
            if (!e.MatchesFilter(this.Filter, e)) {
                return;
            }

            this.LastEventReceived = e.Timestamp;
            this.HandleEvent(e);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="e"> </param>
        private void FilterUnknownEvent(object source, Event e) {
            this.LastEventReceived = e.Timestamp;
            this.HandleUnknownEvent(e);
        }

        /// <summary>
        /// </summary>
        /// <param name="instance"> </param>
        private void Initialize(ParseControl instance) {
            this.ParseControl = instance;
            this.InitStats();
        }

        private new void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}