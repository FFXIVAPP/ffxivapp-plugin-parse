// FFXIVAPP.Plugin.Parse ~ Filter.Declarations.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using FFXIVAPP.Plugin.Parse.Models.Events;

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        // setup you
        private static Event _lastEventYou;

        private static string _lastActionYou = string.Empty;

        private static bool _lastActionYouIsAttack;

        // setup you pet
        private static Event _lastEventPet;

        private static string _lastNamePet = string.Empty;
        private static string _lastActionPet = string.Empty;

        private static bool _lastActionPetIsAttack;

        // setup party info
        private static Event _lastEventParty;

        private static string _lastNamePartyFrom = string.Empty;
        private static string _lastActionPartyFrom = string.Empty;
        private static string _lastNamePartyHealingFrom = string.Empty;
        private static string _lastActionPartyHealingFrom = string.Empty;
        private static string _lastNamePartyTo = string.Empty;

        private static bool _lastActionPartyIsAttack;

        // setup party pet info
        private static Event _lastEventPetParty;

        private static string _lastNamePetPartyFrom = string.Empty;
        private static string _lastActionPetPartyFrom = string.Empty;
        private static string _lastNamePetPartyHealingFrom = string.Empty;
        private static string _lastActionPetPartyHealingFrom = string.Empty;
        private static string _lastNamePetPartyTo = string.Empty;

        private static bool _lastActionPetPartyIsAttack;

        // setup alliance info
        private static Event _lastEventAlliance;

        private static string _lastNameAllianceFrom = string.Empty;
        private static string _lastActionAllianceFrom = string.Empty;
        private static string _lastNameAllianceHealingFrom = string.Empty;
        private static string _lastActionAllianceHealingFrom = string.Empty;
        private static string _lastNameAllianceTo = string.Empty;

        private static bool _lastActionAllianceIsAttack;

        // setup alliancepet  info
        private static Event _lastEventPetAlliance;

        private static string _lastNamePetAllianceFrom = string.Empty;
        private static string _lastActionPetAllianceFrom = string.Empty;
        private static string _lastNamePetAllianceHealingFrom = string.Empty;
        private static string _lastActionPetAllianceHealingFrom = string.Empty;
        private static string _lastNamePetAllianceTo = string.Empty;

        private static bool _lastActionPetAllianceIsAttack;

        // setup other info
        private static Event _lastEventOther;

        private static string _lastNameOtherFrom = string.Empty;
        private static string _lastActionOtherFrom = string.Empty;
        private static string _lastNameOtherHealingFrom = string.Empty;
        private static string _lastActionOtherHealingFrom = string.Empty;
        private static string _lastNameOtherTo = string.Empty;

        private static bool _lastActionOtherIsAttack;

        // setup otherpet  info
        private static Event _lastEventPetOther;

        private static string _lastNamePetOtherFrom = string.Empty;
        private static string _lastActionPetOtherFrom = string.Empty;
        private static string _lastNamePetOtherHealingFrom = string.Empty;
        private static string _lastActionPetOtherHealingFrom = string.Empty;
        private static string _lastNamePetOtherTo = string.Empty;

        private static bool _lastActionPetOtherIsAttack;

        // setup monster info
        private static string _lastNameMonster = string.Empty;

        private static string _lastActionMonster = string.Empty;
        private static bool _lastActionMonsterIsAttack;
    }
}
