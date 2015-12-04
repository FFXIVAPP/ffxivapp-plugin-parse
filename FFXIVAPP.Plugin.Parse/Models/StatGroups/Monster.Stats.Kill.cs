// FFXIVAPP.Plugin.Parse ~ Monster.Stats.Kill.cs
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

using System;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Models.Fights;

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="fight"> </param>
        public void SetKill(Fight fight)
        {
            if (fight.MonsterName != Name)
            {
                Logging.Log(Logger, String.Format("KillEvent : Got request to add kill stats for {0}, but my name is {1}!", fight.MonsterName, Name));
                return;
            }
            if (fight.MonsterName == "")
            {
                Logging.Log(Logger, String.Format("KillEvent : Got request to add kill stats for {0}, but no name!", fight.MonsterName));
                return;
            }
            Stats.IncrementStat("TotalKilled");
            var avghp = Stats.GetStatValue("TotalOverallDamageTaken") / Stats.GetStatValue("TotalKilled");
            Stats.EnsureStatValue("AverageHP", avghp);
        }
    }
}
