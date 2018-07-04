// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILinkedStat.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ILinkedStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Stats {
    using System;
    using System.Collections.Generic;

    public interface ILinkedStat {
        event EventHandler<StatChangedEvent> OnDependencyValueChanged;

        void AddDependency(Stat<double> dependency);

        void ClearDependencies();

        IEnumerable<Stat<double>> CloneDependentStats();

        void DoDependencyValueChanged(object sender, object previousValue, object newValue);

        IEnumerable<Stat<double>> GetDependencies();

        void RemoveDependency(Stat<double> dependency);
    }
}