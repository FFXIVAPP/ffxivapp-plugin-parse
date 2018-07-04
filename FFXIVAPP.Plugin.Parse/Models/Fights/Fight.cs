// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fight.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Fight.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Fights {
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public sealed class Fight : INotifyPropertyChanged {
        private string _monsterName;

        public Fight(string monsterName = "") {
            this.MonsterName = monsterName;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string MonsterName {
            get {
                return this._monsterName;
            }

            private set {
                this._monsterName = value;
                this.RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}