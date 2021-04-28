// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatContainer.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   StatContainer.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Stats {
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using FFXIVAPP.Plugin.Parse.Models.LinkedStats;

    public sealed class StatContainer : IStatContainer {
        private readonly ConcurrentDictionary<string, Stat<double>> _statDict = new ConcurrentDictionary<string, Stat<double>>();

        private string _name;

        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public int Count {
            get {
                return this._statDict.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return false;
            }
        }

        public string Name {
            get {
                return this._name;
            }

            set {
                this._name = value;
                this.RaisePropertyChanged();
            }
        }

        public void Add(Stat<double> stat) {
            if (!this._statDict.TryAdd(stat.Name, stat)) {
                return;
            }

            stat.OnValueChanged += this.HandleStatValueChanged;
            this.DoCollectionChanged(NotifyCollectionChangedAction.Add, stat);
        }

        public void AddStats(IEnumerable<Stat<double>> stats) {
            foreach (Stat<double> stat in stats) {
                this.Add(stat);
            }
        }

        public void Clear() {
            foreach (Stat<double> s in this._statDict.Values) {
                s.OnValueChanged -= this.HandleStatValueChanged;
            }

            this._statDict.Clear();
            this.DoCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        public bool Contains(Stat<double> stat) {
            return this._statDict.ContainsKey(stat.Name);
        }

        public void CopyTo(Stat<double>[] array, int arrayIndex) {
            this._statDict.Values.CopyTo(array, arrayIndex);
        }

        public Stat<double> EnsureStatValue(string name, double value) {
            Stat<double> stat;
            if (this.HasStat(name)) {
                stat = this.GetStat(name);
                stat.Value = value;
            }
            else {
                stat = new NumericStat(name, value);
                this.Add(stat);
            }

            return stat;
        }

        public IEnumerator<Stat<double>> GetEnumerator() {
            return this._statDict.Values.GetEnumerator();
        }

        public Stat<double> GetStat(string name) {
            Stat<double> result;
            this._statDict.TryGetValue(name, out result);
            return result;
        }

        public double GetStatValue(string name) {
            return this.HasStat(name)
                       ? this.GetStat(name).Value
                       : 0;
        }

        public bool HasStat(string name) {
            return this._statDict.ContainsKey(name);
        }

        public void IncrementStat(string name, double value = 1) {
            if (!this.HasStat(name)) {
                return;
            }

            Stat<double> result = this.GetStat(name);
            result.Value += value;
        }

        public bool Remove(Stat<double> stat) {
            Stat<double> removed;
            if (this._statDict.TryRemove(stat.Name, out removed)) {
                removed.OnValueChanged -= this.HandleStatValueChanged;
                this.DoCollectionChanged(NotifyCollectionChangedAction.Remove, removed);
                return true;
            }

            return false;
        }

        public void ResetAll() {
            foreach (Stat<double> s in this._statDict.Values) {
                s.Reset();
            }

            this.DoCollectionChanged(NotifyCollectionChangedAction.Reset, this._statDict.Values.First());
        }

        public bool TryGetStat(string name, out object result) {
            if (this.HasStat(name)) {
                result = this.GetStat(name);
                return true;
            }

            result = null;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        private void DoCollectionChanged(NotifyCollectionChangedAction action, Stat<double> whichStat) {
            this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, whichStat));
        }

        private void HandleStatValueChanged(object sender, StatChangedEvent e) {
            Stat<double> stat = (Stat<double>) sender;
            this.RaisePropertyChanged(stat.Name);
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}