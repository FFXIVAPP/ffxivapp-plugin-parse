// FFXIVAPP.Plugin.Parse ~ HistoryContainer.cs
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

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FFXIVAPP.Plugin.Parse.Models.History
{
    public class HistoryContainer : IHistoryContainer
    {
        private readonly ConcurrentDictionary<string, HistoryStat<double>> _statDict = new ConcurrentDictionary<string, HistoryStat<double>>();
        public string Name { get; set; }

        public HistoryStat<double> GetStat(string name)
        {
            HistoryStat<double> result;
            _statDict.TryGetValue(name, out result);
            return result;
        }

        public HistoryStat<double> EnsureStatValue(string name, double value)
        {
            HistoryStat<double> stat;
            if (HasStat(name))
            {
                stat = GetStat(name);
                stat.Value = value;
            }
            else
            {
                stat = new NumericHistoryStat(name, value);
                Add(stat);
            }
            return stat;
        }

        public double GetStatValue(string name)
        {
            return HasStat(name) ? GetStat(name)
                .Value : 0;
        }

        public bool HasStat(string name)
        {
            return _statDict.ContainsKey(name);
        }

        #region Implementation of IEnumerable

        public IEnumerator<HistoryStat<double>> GetEnumerator()
        {
            return _statDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<Stat<double>>

        public void Add(HistoryStat<double> stat)
        {
            _statDict.TryAdd(stat.Name, stat);
        }

        public void Clear()
        {
            _statDict.Clear();
        }

        public bool Contains(HistoryStat<double> stat)
        {
            return _statDict.ContainsKey(stat.Name);
        }

        public void CopyTo(HistoryStat<double>[] array, int arrayIndex)
        {
            _statDict.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(HistoryStat<double> stat)
        {
            HistoryStat<double> removed;
            if (_statDict.TryRemove(stat.Name, out removed))
            {
                return true;
            }
            return false;
        }

        public int Count
        {
            get { return _statDict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
    }
}
