// FFXIVAPP.Plugin.Parse ~ EventGroup.cs
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Plugin.Parse.Enums;

namespace FFXIVAPP.Plugin.Parse.Models.Events
{
    public class EventGroup : INotifyPropertyChanged
    {
        /// <summary>
        /// </summary>
        public EventGroup()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parent"> </param>
        public EventGroup(string name, EventGroup parent = null)
        {
            Init(name, parent);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parent"> </param>
        private void Init(string name, EventGroup parent)
        {
            Name = name;
            Parent = parent;
        }

        /// <summary>
        /// </summary>
        /// <param name="kid"> </param>
        /// <returns> </returns>
        public EventGroup AddChild(EventGroup kid)
        {
            kid.Parent = this;
            return this;
        }

        #region Property Bindings

        private List<EventCode> _codes;
        private string _name;

        private string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public List<EventCode> Codes
        {
            get { return _codes ?? (_codes = new List<EventCode>()); }
            private set
            {
                _codes = value;
                RaisePropertyChanged();
            }
        }

        public UInt64 Flags
        {
            get
            {
                if (Parent == null)
                {
                    return _flags;
                }
                UInt64 combinedFlags = 0x0;
                if ((_flags & EventParser.DirectionMask) != 0)
                {
                    combinedFlags |= _flags & EventParser.DirectionMask;
                }
                else
                {
                    combinedFlags |= (UInt32) Parent.Direction;
                }
                if ((_flags & EventParser.SubjectMask) != 0)
                {
                    combinedFlags |= _flags & EventParser.SubjectMask;
                }
                else
                {
                    combinedFlags |= (UInt32) Parent.Subject;
                }
                if ((_flags & EventParser.TypeMask) != 0)
                {
                    combinedFlags |= _flags & EventParser.TypeMask;
                }
                else
                {
                    combinedFlags |= (UInt32) Parent.Type;
                }
                return combinedFlags;
            }
        }

        public EventDirection Direction
        {
            get { return (EventDirection) (Flags & EventParser.DirectionMask); }
            set
            {
                _flags = (_flags & ~EventParser.DirectionMask) | (UInt64) value;
                RaisePropertyChanged();
            }
        }

        public EventSubject Subject
        {
            get { return (EventSubject) (Flags & EventParser.SubjectMask); }
            set
            {
                _flags = (_flags & ~EventParser.SubjectMask) | (UInt64) value;
                RaisePropertyChanged();
            }
        }

        public EventType Type
        {
            get { return (EventType) (Flags & EventParser.TypeMask); }
            set
            {
                _flags = (_flags & ~EventParser.TypeMask) | (UInt32) value;
                RaisePropertyChanged();
            }
        }

        private EventGroup Parent
        {
            get { return _parent; }
            set
            {
                if (_parent != null && value != null)
                {
                    _parent._children.Remove(this);
                }
                if (value == null)
                {
                    return;
                }
                _parent = value;
                value._children.Add(this);
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        private readonly List<EventGroup> _children = new List<EventGroup>();
        private UInt64 _flags;
        private EventGroup _parent;

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
