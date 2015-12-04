// FFXIVAPP.Plugin.Parse ~ FightList.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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

using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace FFXIVAPP.Plugin.Parse.Models.Fights
{
    public sealed class FightList : ConcurrentStack<Fight>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        /// <summary>
        /// </summary>
        /// <param name="fights"> </param>
        public FightList(params Fight[] fights) : base(fights)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="fight"> </param>
        public void Add(Fight fight)
        {
            Push(fight);
            DoCollectionChanged(NotifyCollectionChangedAction.Add, fight);
        }

        /// <summary>
        /// </summary>
        /// <param name="monsterName"> </param>
        /// <param name="result"> </param>
        /// <returns> </returns>
        public bool TryGet(string monsterName, out Fight result)
        {
            foreach (var fight in this.Where(fight => fight.MonsterName.ToLower() == monsterName.ToLower()))
            {
                result = fight;
                return true;
            }
            result = null;
            return false;
        }

        #region Implementation of ConcurrentStack

        /// <summary>
        /// </summary>
        /// <param name="fight"> </param>
        private new void Push(Fight fight)
        {
            base.Push(fight);
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

        private void DoCollectionChanged(NotifyCollectionChangedAction action, params Fight[] fights)
        {
            Dispatcher dispatcher = null;
            foreach (var @delegate in CollectionChanged.GetInvocationList())
            {
                var dispatcherObject = @delegate.Target as DispatcherObject;
                if (dispatcherObject == null)
                {
                    continue;
                }
                dispatcher = dispatcherObject.Dispatcher;
                break;
            }
            if (dispatcher != null && dispatcher.CheckAccess() == false)
            {
                dispatcher.Invoke(DispatcherPriority.DataBind, (Action) (() => DoCollectionChanged(action, fights)));
            }
            else
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, fights));
            }
        }

        #endregion
    }
}
