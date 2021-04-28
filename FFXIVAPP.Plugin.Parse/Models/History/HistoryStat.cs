// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryStat.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   HistoryStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.History {
    public abstract class HistoryStat<T> {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        protected HistoryStat(string name = "", T value = default) {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; private set; }

        public T Value { get; set; }

        /// <summary>
        /// </summary>
        public void Reset() {
            this.Value = default;
        }
    }
}