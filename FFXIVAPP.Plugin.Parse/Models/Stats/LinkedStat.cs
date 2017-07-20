// FFXIVAPP.Plugin.Parse ~ LinkedStat.cs
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public abstract class LinkedStat : Stat<double>, ILinkedStat
    {
        private List<Stat<double>> _dependencies;

        protected LinkedStat(string name, params Stat<double>[] dependencies) : base(name, 0)
        {
            SetupStats(dependencies);
        }

        protected LinkedStat(string name, double value) : base(name, 0)
        {
        }

        protected LinkedStat(string name) : base(name, 0)
        {
        }

        #region Declarations

        private List<Stat<double>> Dependencies
        {
            get { return _dependencies ?? (_dependencies = new List<Stat<double>>()); }
            set { _dependencies = value; }
        }

        #endregion

        #region Events

        public event EventHandler<StatChangedEvent> OnDependencyValueChanged = delegate { };

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        public virtual void AddDependency(Stat<double> dependency)
        {
            dependency.OnValueChanged += DependencyValueChanged;
            Dependencies.Add(dependency);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public IEnumerable<Stat<double>> GetDependencies()
        {
            return Dependencies.AsReadOnly();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public virtual void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            Value = (double) newValue;
        }

        /// <summary>
        /// </summary>
        public void ClearDependencies()
        {
            if (Dependencies.Any())
            {
                foreach (var dependency in Dependencies)
                {
                    dependency.OnValueChanged -= DependencyValueChanged;
                }
            }
            Dependencies.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        public void RemoveDependency(Stat<double> dependency)
        {
            if (!Dependencies.Any())
            {
                return;
            }
            dependency.OnValueChanged -= DependencyValueChanged;
            Dependencies.Remove(dependency);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public IEnumerable<Stat<double>> CloneDependentStats()
        {
            return GetDependencies();
        }

        /// <summary>
        /// </summary>
        /// <param name="dependencies"> </param>
        private void SetupStats(IEnumerable<Stat<double>> dependencies)
        {
            foreach (var dependency in dependencies)
            {
                AddDependency(dependency);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void DependencyValueChanged(object sender, StatChangedEvent e)
        {
            OnDependencyValueChanged(this, new StatChangedEvent(sender, e.PreviousValue, e.NewValue));
            DoDependencyValueChanged(sender, e.PreviousValue, e.NewValue);
        }
    }
}
