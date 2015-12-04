// FFXIVAPP.Plugin.Parse ~ HistoryControl.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using FFXIVAPP.Plugin.Parse.Models.History;

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class HistoryControl : INotifyPropertyChanged
    {
        private static HistoryControl _instance;
        private HistoryTimeline _timeline;

        public HistoryControl()
        {
            Timeline = new HistoryTimeline();
        }

        public static HistoryControl Instance
        {
            get { return _instance ?? (_instance = new HistoryControl()); }
            set { _instance = value; }
        }

        public HistoryTimeline Timeline
        {
            get { return _timeline ?? (_timeline = new HistoryTimeline()); }
            set { _timeline = value; }
        }

        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
