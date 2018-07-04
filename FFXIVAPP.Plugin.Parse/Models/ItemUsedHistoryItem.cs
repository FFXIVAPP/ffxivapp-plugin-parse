// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemUsedHistoryItem.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ItemUsedHistoryItem.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System;

    public class ItemUsedHistoryItem {
        public string Item { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}