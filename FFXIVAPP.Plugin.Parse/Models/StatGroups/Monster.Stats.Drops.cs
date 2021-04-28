// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Monster.Stats.Drops.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Monster.Stats.Drops.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups {
    using FFXIVAPP.Plugin.Parse.Models.Stats;

    public partial class Monster {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public void SetDrop(string name) {
            StatGroup dropGroup = this.GetGroup("DropsByMonster");
            StatGroup subGroup;
            if (!dropGroup.TryGetGroup(name, out subGroup)) {
                subGroup = new StatGroup(name);
                subGroup.Stats.AddStats(this.DropStatList());
                dropGroup.AddGroup(subGroup);
            }

            subGroup.Stats.IncrementStat("TotalDrops");
        }
    }
}