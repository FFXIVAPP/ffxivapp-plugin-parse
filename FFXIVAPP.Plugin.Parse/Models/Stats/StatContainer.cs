// FFXIVAPP.Plugin.Parse
// StatContainer.cs
// 
// Copyright � 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

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
