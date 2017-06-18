// FFXIVAPP.Plugin.Parse ~ EventType.cs
// 
// Copyright � 2007 - 2017 Ryan Wilson - All Rights Reserved
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
    public enum EventType : long
    {
        Unknown = 0x0,
        Damage = 0x00002000000,
        Failed = 0x00004000000,
        Actions = 0x00008000000,
        Items = 0x00010000000,
        Cure = 0x00020000000,
        Beneficial = 0x00040000000,
        Detrimental = 0x00080000000,
        System = 0x00100000000,
        Battle = 0x00200000000,
        Synthesis = 0x00400000000,
        Gathering = 0x00800000000,
        Error = 0x01000000000,
        Echo = 0x02000000000,
        Dialogue = 0x04000000000,
        Loot = 0x08000000000,
        Progression = 0x10000000000,
        Defeats = 0x20000000000
    }
}
