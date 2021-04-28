// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.Declarations.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Filter.Declarations.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Utilities {
    using FFXIVAPP.Plugin.Parse.Models.Events;

    public static partial class Filter {
        private static string _lastActionAllianceFrom = string.Empty;

        private static string _lastActionAllianceHealingFrom = string.Empty;

        private static bool _lastActionAllianceIsAttack;

        private static string _lastActionMonster = string.Empty;

        private static bool _lastActionMonsterIsAttack;

        private static string _lastActionOtherFrom = string.Empty;

        private static string _lastActionOtherHealingFrom = string.Empty;

        private static bool _lastActionOtherIsAttack;

        private static string _lastActionPartyFrom = string.Empty;

        private static string _lastActionPartyHealingFrom = string.Empty;

        private static bool _lastActionPartyIsAttack;

        private static string _lastActionPet = string.Empty;

        private static string _lastActionPetAllianceFrom = string.Empty;

        private static string _lastActionPetAllianceHealingFrom = string.Empty;

        private static bool _lastActionPetAllianceIsAttack;

        private static bool _lastActionPetIsAttack;

        private static string _lastActionPetOtherFrom = string.Empty;

        private static string _lastActionPetOtherHealingFrom = string.Empty;

        private static bool _lastActionPetOtherIsAttack;

        private static string _lastActionPetPartyFrom = string.Empty;

        private static string _lastActionPetPartyHealingFrom = string.Empty;

        private static bool _lastActionPetPartyIsAttack;

        private static string _lastActionYou = string.Empty;

        private static bool _lastActionYouIsAttack;

        // setup alliance info
        private static Event _lastEventAlliance;

        // setup other info
        private static Event _lastEventOther;

        // setup party info
        private static Event _lastEventParty;

        // setup you pet
        private static Event _lastEventPet;

        // setup alliancepet  info
        private static Event _lastEventPetAlliance;

        // setup otherpet  info
        private static Event _lastEventPetOther;

        // setup party pet info
        private static Event _lastEventPetParty;

        // setup you
        private static Event _lastEventYou;

        private static string _lastNameAllianceFrom = string.Empty;

        private static string _lastNameAllianceHealingFrom = string.Empty;

        private static string _lastNameAllianceTo = string.Empty;

        // setup monster info
        private static string _lastNameMonster = string.Empty;

        private static string _lastNameOtherFrom = string.Empty;

        private static string _lastNameOtherHealingFrom = string.Empty;

        private static string _lastNameOtherTo = string.Empty;

        private static string _lastNamePartyFrom = string.Empty;

        private static string _lastNamePartyHealingFrom = string.Empty;

        private static string _lastNamePartyTo = string.Empty;

        private static string _lastNamePet = string.Empty;

        private static string _lastNamePetAllianceFrom = string.Empty;

        private static string _lastNamePetAllianceHealingFrom = string.Empty;

        private static string _lastNamePetAllianceTo = string.Empty;

        private static string _lastNamePetOtherFrom = string.Empty;

        private static string _lastNamePetOtherHealingFrom = string.Empty;

        private static string _lastNamePetOtherTo = string.Empty;

        private static string _lastNamePetPartyFrom = string.Empty;

        private static string _lastNamePetPartyHealingFrom = string.Empty;

        private static string _lastNamePetPartyTo = string.Empty;
    }
}