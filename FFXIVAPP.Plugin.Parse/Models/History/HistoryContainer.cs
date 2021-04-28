// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryContainer.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   HistoryContainer.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.History {
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class HistoryContainer : IHistoryContainer {
        private readonly ConcurrentDictionary<string, HistoryStat<double>> _statDict = new ConcurrentDictionary<string, HistoryStat<double>>();

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

        public string Name { get; set; }

        public void Add(HistoryStat<double> stat) {
            this._statDict.TryAdd(stat.Name, stat);
        }

        public void Clear() {
            this._statDict.Clear();
        }

        public bool Contains(HistoryStat<double> stat) {
            return this._statDict.ContainsKey(stat.Name);
        }

        public void CopyTo(HistoryStat<double>[] array, int arrayIndex) {
            this._statDict.Values.CopyTo(array, arrayIndex);
        }

        public HistoryStat<double> EnsureStatValue(string name, double value) {
            HistoryStat<double> stat;
            if (this.HasStat(name)) {
                stat = this.GetStat(name);
                stat.Value = value;
            }
            else {
                stat = new NumericHistoryStat(name, value);
                this.Add(stat);
            }

            return stat;
        }

        public IEnumerator<HistoryStat<double>> GetEnumerator() {
            return this._statDict.Values.GetEnumerator();
        }

        public HistoryStat<double> GetStat(string name) {
            HistoryStat<double> result;
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

        public bool Remove(HistoryStat<double> stat) {
            HistoryStat<double> removed;
            if (this._statDict.TryRemove(stat.Name, out removed)) {
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}