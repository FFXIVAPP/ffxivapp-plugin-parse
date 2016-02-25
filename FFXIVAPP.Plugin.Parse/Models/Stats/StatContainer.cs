// FFXIVAPP.Plugin.Parse ~ StatContainer.cs
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FFXIVAPP.Plugin.Parse.Models.LinkedStats;

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public sealed class StatContainer : IStatContainer
    {
        private readonly ConcurrentDictionary<string, Stat<double>> _statDict = new ConcurrentDictionary<string, Stat<double>>();

        #region Implementation of IEnumerable

        public IEnumerator<Stat<double>> GetEnumerator()
        {
            return _statDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<Stat<double>>

        public void Add(Stat<double> stat)
        {
            if (!_statDict.TryAdd(stat.Name, stat))
            {
                return;
            }
            stat.OnValueChanged += HandleStatValueChanged;
            DoCollectionChanged(NotifyCollectionChangedAction.Add, stat);
        }

        public void Clear()
        {
            foreach (var s in _statDict.Values)
            {
                s.OnValueChanged -= HandleStatValueChanged;
            }
            _statDict.Clear();
            DoCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        public bool Contains(Stat<double> stat)
        {
            return _statDict.ContainsKey(stat.Name);
        }

        public void CopyTo(Stat<double>[] array, int arrayIndex)
        {
            _statDict.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(Stat<double> stat)
        {
            Stat<double> removed;
            if (_statDict.TryRemove(stat.Name, out removed))
            {
                removed.OnValueChanged -= HandleStatValueChanged;
                DoCollectionChanged(NotifyCollectionChangedAction.Remove, removed);
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

        private void HandleStatValueChanged(object sender, StatChangedEvent e)
        {
            var stat = (Stat<double>) sender;
            RaisePropertyChanged(stat.Name);
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        private void DoCollectionChanged(NotifyCollectionChangedAction action, Stat<double> whichStat)
        {
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, whichStat));
        }

        #endregion

        #region Implementation of IStatContainer

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public bool HasStat(string name)
        {
            return _statDict.ContainsKey(name);
        }

        public Stat<double> GetStat(string name)
        {
            Stat<double> result;
            _statDict.TryGetValue(name, out result);
            return result;
        }

        public bool TryGetStat(string name, out object result)
        {
            if (HasStat(name))
            {
                result = GetStat(name);
                return true;
            }
            result = null;
            return false;
        }

        public Stat<double> EnsureStatValue(string name, double value)
        {
            Stat<double> stat;
            if (HasStat(name))
            {
                stat = GetStat(name);
                stat.Value = value;
            }
            else
            {
                stat = new NumericStat(name, value);
                Add(stat);
            }
            return stat;
        }

        public double GetStatValue(string name)
        {
            return HasStat(name) ? GetStat(name)
                .Value : 0;
        }

        public void IncrementStat(string name, double value = 1)
        {
            if (!HasStat(name))
            {
                return;
            }
            var result = GetStat(name);
            result.Value += value;
        }

        public void AddStats(IEnumerable<Stat<double>> stats)
        {
            foreach (var stat in stats)
            {
                Add(stat);
            }
        }

        public void ResetAll()
        {
            foreach (var s in _statDict.Values)
            {
                s.Reset();
            }
            DoCollectionChanged(NotifyCollectionChangedAction.Reset, _statDict.Values.First());
        }

        #endregion
    }
}
