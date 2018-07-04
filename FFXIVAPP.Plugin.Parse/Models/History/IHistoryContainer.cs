// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHistoryContainer.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   IHistoryContainer.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.History {
    using System.Collections.Generic;

    public interface IHistoryContainer : ICollection<HistoryStat<double>> {
        string Name { get; set; }

        HistoryStat<double> EnsureStatValue(string name, double value);

        HistoryStat<double> GetStat(string name);

        double GetStatValue(string name);
    }
}