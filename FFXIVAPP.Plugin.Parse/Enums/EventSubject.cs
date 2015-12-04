// FFXIVAPP.Plugin.Parse ~ EventSubject.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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

namespace FFXIVAPP.Plugin.Parse.Enums
{
    public enum EventSubject : long
    {
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
