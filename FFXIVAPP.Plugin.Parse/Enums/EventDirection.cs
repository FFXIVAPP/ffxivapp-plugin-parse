// FFXIVAPP.Plugin.Parse ~ EventDirection.cs
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

namespace FFXIVAPP.Plugin.Parse.Enums
{
    public enum EventDirection : long
    {
        Unknown = 0x0,
        Self = 0x00000000001,
        You = 0x00000000002,
        Party = 0x00000000004,
        Other = 0x00000000008,
        NPC = 0x00000000010,
        Alliance = 0x00000000020,
        FriendlyNPC = 0x00000000040,
        Pet = 0x00000000080,
        PetParty = 0x00000000100,
        PetAlliance = 0x00000000200,
        PetOther = 0x00000000400,
        Engaged = 0x00000000800,
        UnEngaged = 0x00000001000
    }
}
