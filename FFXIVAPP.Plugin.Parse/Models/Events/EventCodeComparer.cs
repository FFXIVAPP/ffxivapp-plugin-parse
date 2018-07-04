// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventCodeComparer.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   EventCodeComparer.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Events {
    using System.Collections.Generic;

    public class EventCodeComparer : IEqualityComparer<EventCode> {
        /// <summary>
        /// </summary>
        /// <param name="eventCode1"> </param>
        /// <param name="eventCode2"> </param>
        /// <returns> </returns>
        public bool Equals(EventCode eventCode1, EventCode eventCode2) {
            return eventCode1.Code == eventCode2.Code;
        }

        /// <summary>
        /// </summary>
        /// <param name="eventCode"> </param>
        /// <returns> </returns>
        public int GetHashCode(EventCode eventCode) {
            return eventCode.Code.GetHashCode();
        }
    }
}