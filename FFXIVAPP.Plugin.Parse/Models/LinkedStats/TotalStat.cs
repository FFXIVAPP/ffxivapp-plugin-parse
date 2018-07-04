// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TotalStat.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   TotalStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats {
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public class TotalStat : LinkedStat {
        public TotalStat(string name, params Stat<double>[] dependencies)
            : base(name, 0) { }

        public TotalStat(string name, double value)
            : base(name, 0) { }

        public TotalStat(string name)
            : base(name, 0) { }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue) {
            this.Value += (double) newValue - (double) previousValue;
        }
    }
}