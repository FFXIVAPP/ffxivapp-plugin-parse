// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatContainer.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   IStatContainer.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Stats {
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public interface IStatContainer : ICollection<Stat<double>>, INotifyPropertyChanged, INotifyCollectionChanged {
        string Name { get; set; }

        void AddStats(IEnumerable<Stat<double>> stats);

        Stat<double> EnsureStatValue(string name, double value);

        Stat<double> GetStat(string name);

        double GetStatValue(string name);

        bool HasStat(string name);

        void IncrementStat(string name, double value);

        bool TryGetStat(string name, out object result);
    }
}