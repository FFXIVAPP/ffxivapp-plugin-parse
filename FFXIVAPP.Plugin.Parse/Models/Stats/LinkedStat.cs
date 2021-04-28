// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkedStat.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   LinkedStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Stats {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class LinkedStat : Stat<double>, ILinkedStat {
        private List<Stat<double>> _dependencies;

        protected LinkedStat(string name, params Stat<double>[] dependencies) : base(name) {
            this.SetupStats(dependencies);
        }

        protected LinkedStat(string name, double value) : base(name) { }

        protected LinkedStat(string name) : base(name) { }

        public event EventHandler<StatChangedEvent> OnDependencyValueChanged = delegate { };

        private List<Stat<double>> Dependencies {
            get {
                return this._dependencies ?? (this._dependencies = new List<Stat<double>>());
            }

            set {
                this._dependencies = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        public virtual void AddDependency(Stat<double> dependency) {
            dependency.OnValueChanged += this.DependencyValueChanged;
            this.Dependencies.Add(dependency);
        }

        /// <summary>
        /// </summary>
        public void ClearDependencies() {
            if (this.Dependencies.Any()) {
                foreach (Stat<double> dependency in this.Dependencies) {
                    dependency.OnValueChanged -= this.DependencyValueChanged;
                }
            }

            this.Dependencies.Clear();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public IEnumerable<Stat<double>> CloneDependentStats() {
            return this.GetDependencies();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public virtual void DoDependencyValueChanged(object sender, object previousValue, object newValue) {
            this.Value = (double) newValue;
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public IEnumerable<Stat<double>> GetDependencies() {
            return this.Dependencies.AsReadOnly();
        }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        public void RemoveDependency(Stat<double> dependency) {
            if (!this.Dependencies.Any()) {
                return;
            }

            dependency.OnValueChanged -= this.DependencyValueChanged;
            this.Dependencies.Remove(dependency);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void DependencyValueChanged(object sender, StatChangedEvent e) {
            this.OnDependencyValueChanged(this, new StatChangedEvent(sender, e.PreviousValue, e.NewValue));
            this.DoDependencyValueChanged(sender, e.PreviousValue, e.NewValue);
        }

        /// <summary>
        /// </summary>
        /// <param name="dependencies"> </param>
        private void SetupStats(IEnumerable<Stat<double>> dependencies) {
            foreach (Stat<double> dependency in dependencies) {
                this.AddDependency(dependency);
            }
        }
    }
}