// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimelineChangedEvent.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   TimelineChangedEvent.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Timelines {
    using System;

    using FFXIVAPP.Plugin.Parse.Enums;

    public class TimelineChangedEvent : EventArgs {
        /// <summary>
        /// </summary>
        /// <param name="eventType"> </param>
        /// <param name="eventArgs"> </param>
        public TimelineChangedEvent(TimelineEventType eventType, params object[] eventArgs) {
            this.EventType = eventType;
            this.EventArgs = eventArgs;
        }

        private object[] EventArgs { get; set; }

        private TimelineEventType EventType { get; set; }
    }
}