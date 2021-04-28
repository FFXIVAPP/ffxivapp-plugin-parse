// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryTimeline.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   HistoryTimeline.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.History {
    using NLog;

    public class HistoryTimeline {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public HistoryTimeline() {
            this.Overall = new HistoryGroup("Overall");
            this.Party = new HistoryGroup("Party");
            this.Monster = new HistoryGroup("Monster");
        }

        public HistoryGroup Monster { get; set; }

        public HistoryGroup Overall { get; set; }

        public HistoryGroup Party { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="monsterName"> </param>
        /// <returns> </returns>
        public HistoryGroup GetSetMonster(string monsterName) {
            if (!this.Monster.HasGroup(monsterName)) {
                this.Monster.AddGroup(new HistoryGroup(monsterName));
            }

            HistoryGroup monster = this.Monster.GetGroup(monsterName);
            return monster;
        }

        /// <summary>
        /// </summary>
        /// <param name="playerName"> </param>
        /// <returns> </returns>
        public HistoryGroup GetSetPlayer(string playerName) {
            if (!this.Party.HasGroup(playerName)) {
                this.Party.AddGroup(new HistoryGroup(playerName));
            }

            HistoryGroup player = this.Party.GetGroup(playerName);
            return player;
        }
    }
}