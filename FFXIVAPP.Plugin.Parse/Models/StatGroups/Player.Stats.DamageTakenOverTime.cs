// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.Stats.DamageTakenOverTime.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Player.Stats.DamageTakenOverTime.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    public partial class Player {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageTakenOverTime(Line line) {
            if (this.Name == Constants.CharacterName) {
                // LineHistory.Add(new LineHistory(line));
            }

            // stubbed
        }
    }
}