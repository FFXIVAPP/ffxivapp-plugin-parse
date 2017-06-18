// FFXIVAPP.Plugin.Parse ~ HistoryGroup.cs
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

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FFXIVAPP.Plugin.Parse.Models.History
{
    public class HistoryGroup : HistoryGroupTypeDescriptor, ICollection<HistoryGroup>
    {
        public HistoryGroup(string name)
        {
            HistoryGroup = this;
            Name = name;
            Last20DamageActions = new List<LineHistory>();
            Last20DamageTakenActions = new List<LineHistory>();
            Last20HealingActions = new List<LineHistory>();
            Last20Items = new List<LineHistory>();
        }

        #region Property Bindings

        public string Name { get; set; }

        #endregion

        public List<LineHistory> Last20DamageActions { get; set; }
        public List<LineHistory> Last20DamageTakenActions { get; set; }
        public List<LineHistory> Last20HealingActions { get; set; }
        public List<LineHistory> Last20Items { get; set; }

        public HistoryGroup this[int i]
        {
            get { return Children[i]; }
            set { Children[i] = value; }
        }

        public bool HasGroup(string name)
        {
            return ChildContainer.ContainsKey(name);
        }

        public void AddGroup(HistoryGroup item)
        {
            ChildContainer.TryAdd(item.Name, item);
        }

        public HistoryGroup GetGroup(string name)
        {
            HistoryGroup result;
            TryGetGroup(name, out result);
            if (result == null)
            {
                AddGroup(new HistoryGroup(name));
            }
            return TryGetGroup(name, out result) ? result : null;
        }

        public bool TryGetGroup(string name, out HistoryGroup result)
        {
            HistoryGroup historyGroup;
            if (ChildContainer.TryGetValue(name, out historyGroup))
            {
                result = historyGroup;
                return true;
            }
            result = null;
            return false;
        }

        public object GetStatValue(string name)
        {
            if (name.ToLower() == "name")
            {
                return Name;
            }
            return Stats.GetStatValue(name);
        }

        #region Declarations

        private ConcurrentDictionary<string, HistoryGroup> _childContainer;
        private List<HistoryGroup> _children;
        private HistoryContainer _stats;

        private ConcurrentDictionary<string, HistoryGroup> ChildContainer
        {
            get { return _childContainer ?? (_childContainer = new ConcurrentDictionary<string, HistoryGroup>()); }
            set { _childContainer = value; }
        }

        public List<HistoryGroup> Children
        {
            get { return _children ?? (_children = new List<HistoryGroup>(ChildContainer.Values)); }
            set { _children = value; }
        }

        public HistoryContainer Stats
        {
            get { return _stats ?? (_stats = new HistoryContainer()); }
        }

        #endregion

        #region Implementation of ICollection<HistoryGroup>

        public IEnumerator<HistoryGroup> GetEnumerator()
        {
            var list = new List<HistoryGroup>();
            list.AddRange(ChildContainer.Values);
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(HistoryGroup item)
        {
            ChildContainer.TryAdd(item.Name, item);
        }

        public virtual void Clear()
        {
            ChildContainer.Clear();
        }

        public bool Contains(HistoryGroup item)
        {
            return ChildContainer.ContainsKey(item.Name);
        }

        public void CopyTo(HistoryGroup[] array, int arrayIndex)
        {
            ChildContainer.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(HistoryGroup item)
        {
            HistoryGroup result;
            return ChildContainer.TryRemove(item.Name, out result);
        }

        public int Count
        {
            get { return ChildContainer.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
    }
}
