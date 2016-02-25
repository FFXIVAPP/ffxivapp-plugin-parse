// FFXIVAPP.Plugin.Parse ~ EventCode.cs
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
using FFXIVAPP.Plugin.Parse.Enums;

namespace FFXIVAPP.Plugin.Parse.Models.Events
{
    public class EventCode : INotifyPropertyChanged
    {
        private UInt64 _code;
        private string _description;
        private EventGroup _group;

        public EventCode()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="description"> </param>
        /// <param name="code"> </param>
        /// <param name="group"> </param>
        public EventCode(string description, UInt64 code, EventGroup group)
        {
            Description = description;
            Code = code;
            Group = group;
        }

        #region Property Bindings

        private string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }

        public UInt64 Code
        {
            get { return _code; }
            set
            {
                _code = value;
                RaisePropertyChanged();
            }
        }

        private EventGroup Group
        {
            get { return _group; }
            set
            {
                _group = value;
                RaisePropertyChanged();
            }
        }

        public UInt64 Flags
        {
            get { return (ushort) (Group == null ? 0x0 : Group.Flags); }
        }

        public EventDirection Direction
        {
            get { return Group == null ? EventDirection.Unknown : Group.Direction; }
        }

        public EventSubject Subject
        {
            get { return Group == null ? EventSubject.Unknown : Group.Subject; }
        }

        public EventType Type
        {
            get { return Group == null ? EventType.Unknown : Group.Type; }
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
