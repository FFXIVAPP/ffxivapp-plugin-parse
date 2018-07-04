// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericHistoryStat.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   NumericHistoryStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.History {
    public class NumericHistoryStat : HistoryStat<double> {
        public NumericHistoryStat(string name, double value)
            : base(name, 0) {
            this.Value = value;
        }

        public NumericHistoryStat(string name)
            : base(name, 0) { }
    }
}