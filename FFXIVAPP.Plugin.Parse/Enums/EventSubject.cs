// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSubject.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   EventSubject.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Enums {
    public enum EventSubject : long {
        Unknown = 0x0,

        You = 0x00000002000,

        Party = 0x00000004000,

        Other = 0x00000008000,

        NPC = 0x00000010000,

        Alliance = 0x00000020000,

        FriendlyNPC = 0x00000040000,

        Pet = 0x00000080000,

        PetParty = 0x00000100000,

        PetAlliance = 0x00000200000,

        PetOther = 0x00000400000,

        Engaged = 0x00000800000,

        UnEngaged = 0x00001000000
    }
}