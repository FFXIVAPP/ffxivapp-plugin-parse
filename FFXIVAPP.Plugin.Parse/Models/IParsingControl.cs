// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IParsingControl.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   IParsingControl.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System;

    using FFXIVAPP.Plugin.Parse.Models.Timelines;
    using FFXIVAPP.Plugin.Parse.Monitors;

    public interface IParsingControl {
        DateTime EndTime { get; set; }

        IParsingControl Instance { get; }

        string Name { get; set; }

        DateTime StartTime { get; set; }

        StatMonitor StatMonitor { get; set; }

        Timeline Timeline { get; set; }

        TimelineMonitor TimelineMonitor { get; set; }

        void Initialize();

        void Reset();
    }
}