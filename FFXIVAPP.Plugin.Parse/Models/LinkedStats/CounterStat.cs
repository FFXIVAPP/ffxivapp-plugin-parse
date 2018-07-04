// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CounterStat.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   CounterStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats {
    public class CounterStat : NumericStat {
        public CounterStat(string name, double value)
            : base(name, 0) { }

        public CounterStat(string name)
            : base(name, 0) { }

        /// <summary>
        /// </summary>
        /// <param name="amount"> </param>
        /// <returns> </returns>
        private double Increment(double amount) {
            this.Value += amount;
            return this.Value;
        }
    }
}