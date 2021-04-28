// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinStat.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MinStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats {
    using System;

    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public sealed class MinStat : LinkedStat {
        public MinStat(string name, params Stat<double>[] dependencies) : base(name, 0) {
            this.AddDependency(dependencies[0]);
            this.GotValue = false;
        }

        public MinStat(string name, double value) : base(name, 0) { }

        public MinStat(string name) : base(name, 0) { }

        private bool GotValue { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue) {
            var ovalue = (double) previousValue;
            var nvalue = (double) newValue;
            var delta = Math.Max(ovalue, nvalue) - Math.Min(ovalue, nvalue);
            if (delta >= this.Value && this.GotValue) {
                return;
            }

            this.Value = delta;
            this.GotValue = true;
        }
    }
}