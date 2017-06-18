// FFXIVAPP.Plugin.Parse ~ Monster.Stats.Drops.cs
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

using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public void SetDrop(string name)
        {
            var dropGroup = GetGroup("DropsByMonster");
            StatGroup subGroup;
            if (!dropGroup.TryGetGroup(name, out subGroup))
            {
                subGroup = new StatGroup(name);
                subGroup.Stats.AddStats(DropStatList());
                dropGroup.AddGroup(subGroup);
            }
            subGroup.Stats.IncrementStat("TotalDrops");
        }
    }
}
