// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccuracyStat.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   AccuracyStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats {
    using System;

    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public class AccuracyStat : LinkedStat {
        public AccuracyStat(string name, params Stat<double>[] dependencies)
            : base(name, 0) {
            this.UsedStat = dependencies[0];
            this.MissStat = dependencies[1];
            this.SetupDepends();
        }

        public AccuracyStat(string name, double value)
            : base(name, 0) { }

        public AccuracyStat(string name)
            : base(name, 0) { }

        private Stat<double> MissStat { get; }

        private Stat<double> UsedStat { get; }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue) {
            this.UpdateAccuracy();
        }

        /// <summary>
        /// </summary>
        private void SetupDepends() {
            this.AddDependency(this.UsedStat);
            this.AddDependency(this.MissStat);
            if (this.UsedStat.Value > 0 && this.MissStat.Value > 0) {
                this.UpdateAccuracy();
            }
        }

        /// <summary>
        /// </summary>
        private void UpdateAccuracy() {
            if (this.UsedStat.Value == 0 && this.MissStat.Value == 0) {
                this.Value = 0;
                return;
            }

            var totalHits = Convert.ToDouble(this.UsedStat.Value - this.MissStat.Value);
            if (totalHits > -1) {
                this.Value = totalHits / this.UsedStat.Value;
            }
        }
    }
}