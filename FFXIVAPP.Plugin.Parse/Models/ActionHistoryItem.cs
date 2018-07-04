// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionHistoryItem.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ActionHistoryItem.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System;

    public class ActionHistoryItem {
        public string Action { get; set; }

        public double Amount { get; set; }

        public string Critical { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}