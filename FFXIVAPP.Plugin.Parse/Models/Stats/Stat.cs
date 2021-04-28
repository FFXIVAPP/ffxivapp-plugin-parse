// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Stat.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Stat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Stats {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class Stat<T> : INotifyPropertyChanged {
        private string _name;

        private T _value;

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        protected Stat(string name = "", T value = default) {
            this.Name = name;
            this.Value = value;
        }

        public event EventHandler<StatChangedEvent> OnValueChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string Name {
            get {
                return this._name;
            }

            private set {
                this._name = value;
                this.RaisePropertyChanged();
            }
        }

        public T Value {
            get {
                return this._value;
            }

            set {
                T previousValue = this.Value;
                this._value = value;
                this.OnValueChanged(this, new StatChangedEvent(this, previousValue, this.Value));
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        public void Reset() {
            this.Value = default;
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}