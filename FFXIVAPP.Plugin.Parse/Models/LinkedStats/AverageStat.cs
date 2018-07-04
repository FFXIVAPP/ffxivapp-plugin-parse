// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AverageStat.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   AverageStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats {
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public class AverageStat : LinkedStat {
        private int _numUpdates;

        public AverageStat(string name, params Stat<double>[] dependencies)
            : base(name, 0) {
            this.SetupDepends(dependencies[0]);
        }

        public AverageStat(string name, double value)
            : base(name, 0) { }

        public AverageStat(string name)
            : base(name, 0) { }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue) {
            var value = (double) newValue;
            this.Value = value / ++this._numUpdates;
        }

        /// <summary>
        /// </summary>
        /// <param name="total"> </param>
        private void SetupDepends(Stat<double> total) {
            this.AddDependency(total);
        }
    }
}