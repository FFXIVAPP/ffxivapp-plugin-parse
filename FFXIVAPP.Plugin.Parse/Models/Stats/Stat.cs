// FFXIVAPP.Plugin.Parse ~ Stat.cs
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

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public abstract class Stat<T> : INotifyPropertyChanged
    {
        private string _name;
        private T _value;

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        protected Stat(string name = "", T value = default(T))
        {
            Name = name;
            Value = value;
        }

        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public T Value
        {
            get { return _value; }
            set
            {
                var previousValue = Value;
                _value = value;
                OnValueChanged(this, new StatChangedEvent(this, previousValue, Value));
                RaisePropertyChanged();
            }
        }

        #region Events

        public event EventHandler<StatChangedEvent> OnValueChanged = delegate { };

        #endregion

        /// <summary>
        /// </summary>
        public void Reset()
        {
            Value = default(T);
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
