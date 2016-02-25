// FFXIVAPP.Plugin.Parse ~ Filter.Declarations.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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
        private static string _lastActionYou = "";
        private static bool _lastActionYouIsAttack;
        // setup you pet
        private static Event _lastEventPet;
        private static string _lastNamePet = "";
        private static string _lastActionPet = "";
        private static bool _lastActionPetIsAttack;
        // setup party info
        private static Event _lastEventParty;
        private static string _lastNamePartyFrom = "";
        private static string _lastActionPartyFrom = "";
        private static string _lastNamePartyHealingFrom = "";
        private static string _lastActionPartyHealingFrom = "";
        private static string _lastNamePartyTo = "";
        private static bool _lastActionPartyIsAttack;
        // setup party pet info
        private static Event _lastEventPetParty;
        private static string _lastNamePetPartyFrom = "";
        private static string _lastActionPetPartyFrom = "";
        private static string _lastNamePetPartyHealingFrom = "";
        private static string _lastActionPetPartyHealingFrom = "";
        private static string _lastNamePetPartyTo = "";
        private static bool _lastActionPetPartyIsAttack;
        // setup alliance info
        private static Event _lastEventAlliance;
        private static string _lastNameAllianceFrom = "";
        private static string _lastActionAllianceFrom = "";
        private static string _lastNameAllianceHealingFrom = "";
        private static string _lastActionAllianceHealingFrom = "";
        private static string _lastNameAllianceTo = "";
        private static bool _lastActionAllianceIsAttack;
        // setup alliancepet  info
        private static Event _lastEventPetAlliance;
        private static string _lastNamePetAllianceFrom = "";
        private static string _lastActionPetAllianceFrom = "";
        private static string _lastNamePetAllianceHealingFrom = "";
        private static string _lastActionPetAllianceHealingFrom = "";
        private static string _lastNamePetAllianceTo = "";
        private static bool _lastActionPetAllianceIsAttack;
        // setup other info
        private static Event _lastEventOther;
        private static string _lastNameOtherFrom = "";
        private static string _lastActionOtherFrom = "";
        private static string _lastNameOtherHealingFrom = "";
        private static string _lastActionOtherHealingFrom = "";
        private static string _lastNameOtherTo = "";
        private static bool _lastActionOtherIsAttack;
        // setup otherpet  info
        private static Event _lastEventPetOther;
        private static string _lastNamePetOtherFrom = "";
        private static string _lastActionPetOtherFrom = "";
        private static string _lastNamePetOtherHealingFrom = "";
        private static string _lastActionPetOtherHealingFrom = "";
        private static string _lastNamePetOtherTo = "";
        private static bool _lastActionPetOtherIsAttack;
        // setup monster info
        private static string _lastNameMonster = "";
        private static string _lastActionMonster = "";
        private static bool _lastActionMonsterIsAttack;
    }
}
