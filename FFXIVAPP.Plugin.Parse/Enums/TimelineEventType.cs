// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimelineEventType.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   TimelineEventType.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Enums {
    public enum TimelineEventType {
        PartyJoin,

        PartyLeave,

        PartyDisband,

        PartyMonsterFighting,

        PartyMonsterKilled,

        AllianceMonsterFighting,

        AllianceMonsterKilled,

        OtherMonsterFighting,

        OtherMonsterKilled,
    }
}