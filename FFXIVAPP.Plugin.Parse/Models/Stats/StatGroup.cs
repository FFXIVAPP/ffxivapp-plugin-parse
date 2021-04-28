// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatGroup.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   StatGroup.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Stats {
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Threading;

    public class StatGroup : StatGroupTypeDescriptor, ICollection<StatGroup>, INotifyCollectionChanged, INotifyPropertyChanged {
        public StatContainer Stats = new StatContainer();

        private readonly ConcurrentDictionary<string, StatGroup> ChildContainer = new ConcurrentDictionary<string, StatGroup>();

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public StatGroup(string name) {
            this.IncludeSelf = true;
            this.DoInit(name);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="children"> </param>
        protected StatGroup(string name, params StatGroup[] children) {
            this.IncludeSelf = true;
            IEnumerable<KeyValuePair<string, StatGroup>> valuePairs = children.Select(c => new KeyValuePair<string, StatGroup>(c.Name, c));
            this.ChildContainer = new ConcurrentDictionary<string, StatGroup>(valuePairs);
            this.DoInit(name);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public List<StatGroup> Children {
            get {
                return new List<StatGroup>(this.ChildContainer.Values);
            }
        }

        /// <summary>
        /// </summary>
        public int Count {
            get {
                var count = this.ChildContainer.Count();
                if (this.IncludeSelf) {
                    count++;
                }

                return count;
            }
        }

        public bool IncludeSelf { private get; set; }

        /// <summary>
        /// </summary>
        public bool IsReadOnly {
            get {
                return false;
            }
        }

        public string Name { get; set; }

        public StatGroup this[int i] {
            get {
                return this.Children[i];
            }

            set {
                this.Children[i] = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="item"> </param>
        public void Add(StatGroup item) {
            this.AddGroup(item);
        }

        /// <summary>
        /// </summary>
        /// <param name="child"> </param>
        public void AddGroup(StatGroup child) {
            if (this.ChildContainer.TryAdd(child.Name, child)) {
                this.DoCollectionChanged(NotifyCollectionChangedAction.Add, child);
            }
        }

        /// <summary>
        /// </summary>
        public virtual void Clear() {
            foreach (StatGroup statGroup in this.ChildContainer.Values) {
                statGroup.Stats.Clear();
                statGroup.Clear();
            }

            this.ChildContainer.Clear();
            this.Stats.Clear();
            foreach (Stat<double> stat in this.Stats) {
                stat.Reset();
            }

            this.DoCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="item"> </param>
        /// <returns> </returns>
        public bool Contains(StatGroup item) {
            return this.ChildContainer.ContainsKey(item.Name);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"> </param>
        /// <param name="arrayIndex"> </param>
        public void CopyTo(StatGroup[] array, int arrayIndex) {
            this.ChildContainer.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<StatGroup> GetEnumerator() {
            List<StatGroup> list = new List<StatGroup>();
            if (this.IncludeSelf) {
                list.Add(this);
            }

            list.AddRange(this.ChildContainer.Values);
            return list.GetEnumerator();
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public StatGroup GetGroup(string name) {
            StatGroup result;
            this.TryGetGroup(name, out result);
            if (result == null) {
                this.AddGroup(
                    new StatGroup(name) {
                        IncludeSelf = false,
                    });
            }

            return this.TryGetGroup(name, out result)
                       ? result
                       : null;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public object GetStatValue(string name) {
            if (name.ToLower() == "name") {
                return this.Name;
            }

            return this.Stats.GetStatValue(name);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public bool HasGroup(string name) {
            return this.ChildContainer.ContainsKey(name);
        }

        /// <summary>
        /// </summary>
        /// <param name="item"> </param>
        /// <returns> </returns>
        public bool Remove(StatGroup item) {
            StatGroup result;
            if (this.ChildContainer.TryRemove(item.Name, out result)) {
                this.DoCollectionChanged(NotifyCollectionChangedAction.Remove, result);
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="result"> </param>
        /// <returns> </returns>
        public bool TryGetGroup(string name, out StatGroup result) {
            StatGroup statGroup;
            if (this.ChildContainer.TryGetValue(name, out statGroup)) {
                result = statGroup;
                return true;
            }

            result = null;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        private void DoCollectionChanged(NotifyCollectionChangedAction action, StatGroup statGroup) {
            Dispatcher dispatcher = null;
            foreach (Delegate @delegate in this.CollectionChanged.GetInvocationList()) {
                var dispatcherObject = @delegate.Target as DispatcherObject;
                if (dispatcherObject == null) {
                    continue;
                }

                dispatcher = dispatcherObject.Dispatcher;
                break;
            }

            if (dispatcher != null && dispatcher.CheckAccess() == false) {
                dispatcher.Invoke(DispatcherPriority.DataBind, (Action) (() => this.DoCollectionChanged(action, statGroup)));
            }
            else {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, statGroup));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        private void DoInit(string name) {
            this.StatGroup = this;
            this.Name = name;
            this.Stats.PropertyChanged += (sender, e) => this.RaisePropertyChanged(e.PropertyName);
        }
    }
}