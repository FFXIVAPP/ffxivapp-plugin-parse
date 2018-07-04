// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryControl.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   HistoryControl.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using FFXIVAPP.Plugin.Parse.Models.History;

    public class HistoryControl : INotifyPropertyChanged {
        private static Lazy<HistoryControl> _instance = new Lazy<HistoryControl>(() => new HistoryControl());

        private HistoryTimeline _timeline;

        public HistoryControl() {
            this.Timeline = new HistoryTimeline();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static HistoryControl Instance {
            get {
                return _instance.Value;
            }
        }

        public DateTime EndTime { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public HistoryTimeline Timeline {
            get {
                return this._timeline ?? (this._timeline = new HistoryTimeline());
            }

            set {
                this._timeline = value;
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}