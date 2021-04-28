// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryGroup.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   HistoryGroup.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.History {
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class HistoryGroup : HistoryGroupTypeDescriptor, ICollection<HistoryGroup> {
        private ConcurrentDictionary<string, HistoryGroup> _childContainer;

        private List<HistoryGroup> _children;

        private HistoryContainer _stats;

        public HistoryGroup(string name) {
            this.HistoryGroup = this;
            this.Name = name;
            this.Last20DamageActions = new List<LineHistory>();
            this.Last20DamageTakenActions = new List<LineHistory>();
            this.Last20HealingActions = new List<LineHistory>();
            this.Last20Items = new List<LineHistory>();
        }

        public List<HistoryGroup> Children {
            get {
                return this._children ?? (this._children = new List<HistoryGroup>(this.ChildContainer.Values));
            }

            set {
                this._children = value;
            }
        }

        public int Count {
            get {
                return this.ChildContainer.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return false;
            }
        }

        public List<LineHistory> Last20DamageActions { get; set; }

        public List<LineHistory> Last20DamageTakenActions { get; set; }

        public List<LineHistory> Last20HealingActions { get; set; }

        public List<LineHistory> Last20Items { get; set; }

        public string Name { get; set; }

        public HistoryContainer Stats {
            get {
                return this._stats ?? (this._stats = new HistoryContainer());
            }
        }

        private ConcurrentDictionary<string, HistoryGroup> ChildContainer {
            get {
                return this._childContainer ?? (this._childContainer = new ConcurrentDictionary<string, HistoryGroup>());
            }

            set {
                this._childContainer = value;
            }
        }

        public HistoryGroup this[int i] {
            get {
                return this.Children[i];
            }

            set {
                this.Children[i] = value;
            }
        }

        public void Add(HistoryGroup item) {
            this.ChildContainer.TryAdd(item.Name, item);
        }

        public void AddGroup(HistoryGroup item) {
            this.ChildContainer.TryAdd(item.Name, item);
        }

        public virtual void Clear() {
            this.ChildContainer.Clear();
        }

        public bool Contains(HistoryGroup item) {
            return this.ChildContainer.ContainsKey(item.Name);
        }

        public void CopyTo(HistoryGroup[] array, int arrayIndex) {
            this.ChildContainer.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<HistoryGroup> GetEnumerator() {
            List<HistoryGroup> list = new List<HistoryGroup>();
            list.AddRange(this.ChildContainer.Values);
            return list.GetEnumerator();
        }

        public HistoryGroup GetGroup(string name) {
            HistoryGroup result;
            this.TryGetGroup(name, out result);
            if (result == null) {
                this.AddGroup(new HistoryGroup(name));
            }

            return this.TryGetGroup(name, out result)
                       ? result
                       : null;
        }

        public object GetStatValue(string name) {
            if (name.ToLower() == "name") {
                return this.Name;
            }

            return this.Stats.GetStatValue(name);
        }

        public bool HasGroup(string name) {
            return this.ChildContainer.ContainsKey(name);
        }

        public bool Remove(HistoryGroup item) {
            HistoryGroup result;
            return this.ChildContainer.TryRemove(item.Name, out result);
        }

        public bool TryGetGroup(string name, out HistoryGroup result) {
            HistoryGroup historyGroup;
            if (this.ChildContainer.TryGetValue(name, out historyGroup)) {
                result = historyGroup;
                return true;
            }

            result = null;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}