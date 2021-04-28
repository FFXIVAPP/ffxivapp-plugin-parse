// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatChangedEvent.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   StatChangedEvent.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Stats {
    using System;

    public class StatChangedEvent : EventArgs {
        /// <summary>
        /// </summary>
        /// <param name="sourceStat"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public StatChangedEvent(object sourceStat, object previousValue, object newValue) {
            this.SourceStat = (Stat<double>) sourceStat;
            this.PreviousValue = previousValue;
            this.NewValue = newValue;
        }

        public object NewValue { get; private set; }

        public object PreviousValue { get; private set; }

        private Stat<double> SourceStat { get; set; }
    }
}