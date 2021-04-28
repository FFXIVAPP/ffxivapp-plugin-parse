// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PercentStat.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PercentStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats {
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public class PercentStat : LinkedStat {
        private readonly Stat<double> _denominator;

        private readonly Stat<double> _numerator;

        public PercentStat(string name, params Stat<double>[] dependencies) : base(name, 0) {
            this._numerator = dependencies[0];
            this._denominator = dependencies[1];
            this.SetupDepends();
        }

        public PercentStat(string name, double value) : base(name, 0) { }

        public PercentStat(string name) : base(name, 0) { }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue) {
            this.UpdatePercent();
        }

        /// <summary>
        /// </summary>
        private void SetupDepends() {
            this.AddDependency(this._numerator);
            this.AddDependency(this._denominator);
            if (this._numerator.Value > 0 && this._denominator.Value > 0) {
                this.UpdatePercent();
            }
        }

        /// <summary>
        /// </summary>
        private void UpdatePercent() {
            if (this._numerator.Value == 0 || this._denominator.Value == 0) {
                this.Value = 0;
                return;
            }

            this.Value = this._numerator.Value / this._denominator.Value;
        }
    }
}