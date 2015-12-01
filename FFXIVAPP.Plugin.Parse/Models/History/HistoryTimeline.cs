// FFXIVAPP.Plugin.Parse
// FFXIVAPP & Related Plugins/Modules
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

using NLog;

namespace FFXIVAPP.Plugin.Parse.Models.History
{
    public class HistoryTimeline
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public HistoryTimeline()
        {
            Overall = new HistoryGroup("Overall");
            Party = new HistoryGroup("Party");
            Monster = new HistoryGroup("Monster");
        }

        public HistoryGroup Overall { get; set; }
        public HistoryGroup Party { get; set; }
        public HistoryGroup Monster { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="monsterName"> </param>
        /// <returns> </returns>
        public HistoryGroup GetSetMonster(string monsterName)
        {
            if (!Monster.HasGroup(monsterName))
            {
                Monster.AddGroup(new HistoryGroup(monsterName));
            }
            var monster = Monster.GetGroup(monsterName);
            return monster;
        }

        /// <summary>
        /// </summary>
        /// <param name="playerName"> </param>
        /// <returns> </returns>
        public HistoryGroup GetSetPlayer(string playerName)
        {
            if (!Party.HasGroup(playerName))
            {
                Party.AddGroup(new HistoryGroup(playerName));
            }
            var player = Party.GetGroup(playerName);
            return player;
        }
    }
}
