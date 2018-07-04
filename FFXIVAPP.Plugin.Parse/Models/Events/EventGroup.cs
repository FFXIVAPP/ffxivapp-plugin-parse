// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventGroup.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   EventGroup.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Events {
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using FFXIVAPP.Plugin.Parse.Enums;

    public class EventGroup : INotifyPropertyChanged {
        private readonly List<EventGroup> _children = new List<EventGroup>();

        private List<EventCode> _codes;

        private ulong _flags;

        private string _name;

        private EventGroup _parent;

        /// <summary>
        /// </summary>
        public EventGroup() { }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parent"> </param>
        public EventGroup(string name, EventGroup parent = null) {
            this.Init(name, parent);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public List<EventCode> Codes {
            get {
                return this._codes ?? (this._codes = new List<EventCode>());
            }

            private set {
                this._codes = value;
                this.RaisePropertyChanged();
            }
        }

        public EventDirection Direction {
            get {
                return (EventDirection) (this.Flags & EventParser.DirectionMask);
            }

            set {
                this._flags = (this._flags & ~EventParser.DirectionMask) | (ulong) value;
                this.RaisePropertyChanged();
            }
        }

        public ulong Flags {
            get {
                if (this.Parent == null) {
                    return this._flags;
                }

                ulong combinedFlags = 0x0;
                if ((this._flags & EventParser.DirectionMask) != 0) {
                    combinedFlags |= this._flags & EventParser.DirectionMask;
                }
                else {
                    combinedFlags |= (uint) this.Parent.Direction;
                }

                if ((this._flags & EventParser.SubjectMask) != 0) {
                    combinedFlags |= this._flags & EventParser.SubjectMask;
                }
                else {
                    combinedFlags |= (uint) this.Parent.Subject;
                }

                if ((this._flags & EventParser.TypeMask) != 0) {
                    combinedFlags |= this._flags & EventParser.TypeMask;
                }
                else {
                    combinedFlags |= (uint) this.Parent.Type;
                }

                return combinedFlags;
            }
        }

        public EventSubject Subject {
            get {
                return (EventSubject) (this.Flags & EventParser.SubjectMask);
            }

            set {
                this._flags = (this._flags & ~EventParser.SubjectMask) | (ulong) value;
                this.RaisePropertyChanged();
            }
        }

        public EventType Type {
            get {
                return (EventType) (this.Flags & EventParser.TypeMask);
            }

            set {
                this._flags = (this._flags & ~EventParser.TypeMask) | (uint) value;
                this.RaisePropertyChanged();
            }
        }

        private string Name {
            get {
                return this._name;
            }

            set {
                this._name = value;
                this.RaisePropertyChanged();
            }
        }

        private EventGroup Parent {
            get {
                return this._parent;
            }

            set {
                if (this._parent != null && value != null) {
                    this._parent._children.Remove(this);
                }

                if (value == null) {
                    return;
                }

                this._parent = value;
                value._children.Add(this);
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="kid"> </param>
        /// <returns> </returns>
        public EventGroup AddChild(EventGroup kid) {
            kid.Parent = this;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parent"> </param>
        private void Init(string name, EventGroup parent) {
            this.Name = name;
            this.Parent = parent;
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}