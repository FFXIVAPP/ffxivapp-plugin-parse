// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericStat.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   NumericStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats {
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public class NumericStat : Stat<double> {
        public NumericStat(string name, double value)
            : base(name, 0) {
            this.Value = value;
        }

        public NumericStat(string name)
            : base(name, 0) { }
    }
}