// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FightList.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   FightList.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Fights {
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Threading;

    public sealed class FightList : ConcurrentStack<Fight>, INotifyPropertyChanged, INotifyCollectionChanged {
        /// <summary>
        /// </summary>
        /// <param name="fights"> </param>
        public FightList(params Fight[] fights)
            : base(fights) { }

        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// </summary>
        /// <param name="fight"> </param>
        public void Add(Fight fight) {
            this.Push(fight);
            this.DoCollectionChanged(NotifyCollectionChangedAction.Add, fight);
        }

        /// <summary>
        /// </summary>
        /// <param name="monsterName"> </param>
        /// <param name="result"> </param>
        /// <returns> </returns>
        public bool TryGet(string monsterName, out Fight result) {
            foreach (Fight fight in this.Where(fight => fight.MonsterName.ToLower() == monsterName.ToLower())) {
                result = fight;
                return true;
            }

            result = null;
            return false;
        }

        private void DoCollectionChanged(NotifyCollectionChangedAction action, params Fight[] fights) {
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
                dispatcher.Invoke(DispatcherPriority.DataBind, (Action) (() => this.DoCollectionChanged(action, fights)));
            }
            else {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, fights));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fight"> </param>
        private new void Push(Fight fight) {
            base.Push(fight);
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}