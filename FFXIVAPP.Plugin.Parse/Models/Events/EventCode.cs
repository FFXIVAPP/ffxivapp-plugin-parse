// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventCode.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   EventCode.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Events {
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using FFXIVAPP.Plugin.Parse.Enums;

    public class EventCode : INotifyPropertyChanged {
        private ulong _code;

        private string _description;

        private EventGroup _group;

        public EventCode() { }

        /// <summary>
        /// </summary>
        /// <param name="description"> </param>
        /// <param name="code"> </param>
        /// <param name="group"> </param>
        public EventCode(string description, ulong code, EventGroup group) {
            this.Description = description;
            this.Code = code;
            this.Group = group;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ulong Code {
            get {
                return this._code;
            }

            set {
                this._code = value;
                this.RaisePropertyChanged();
            }
        }

        public EventDirection Direction {
            get {
                return this.Group == null
                           ? EventDirection.Unknown
                           : this.Group.Direction;
            }
        }

        public ulong Flags {
            get {
                return (ushort) (this.Group == null
                                     ? 0x0
                                     : this.Group.Flags);
            }
        }

        public EventSubject Subject {
            get {
                return this.Group == null
                           ? EventSubject.Unknown
                           : this.Group.Subject;
            }
        }

        public EventType Type {
            get {
                return this.Group == null
                           ? EventType.Unknown
                           : this.Group.Type;
            }
        }

        private string Description {
            get {
                return this._description;
            }

            set {
                this._description = value;
                this.RaisePropertyChanged();
            }
        }

        private EventGroup Group {
            get {
                return this._group;
            }

            set {
                this._group = value;
                this.RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}