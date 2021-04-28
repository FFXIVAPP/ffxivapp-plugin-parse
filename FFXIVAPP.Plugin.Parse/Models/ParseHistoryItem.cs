// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseHistoryItem.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ParseHistoryItem.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System;

    public class ParseHistoryItem {
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        public ParseHistoryItem(string name = "UnknownEvent") {
            this.Name = name;
            this.HistoryControl = new HistoryControl();
        }

        public DateTime End { get; set; }

        public HistoryControl HistoryControl { get; set; }

        public string Name { get; set; }

        public TimeSpan ParseLength { get; set; }

        public DateTime Start { get; set; }
    }
}