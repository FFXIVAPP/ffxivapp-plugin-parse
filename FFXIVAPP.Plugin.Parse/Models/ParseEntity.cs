// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseEntity.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ParseEntity.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System.Collections.Generic;

    using FFXIVAPP.Plugin.Parse.Interfaces;

    public class ParseEntity : IParseEntity {
        private List<PlayerEntity> _players;

        public double CombinedDPS { get; set; }

        public double CombinedDTPS { get; set; }

        public double CombinedHPS { get; set; }

        public double CombinedTotalOverallDamage { get; set; }

        public double CombinedTotalOverallDamageTaken { get; set; }

        public double CombinedTotalOverallHealing { get; set; }

        public double DOTPS { get; set; }

        public double DPS { get; set; }

        public double DTOTPS { get; set; }

        public double DTPS { get; set; }

        public double HMPS { get; set; }

        public double HOHPS { get; set; }

        public double HOTPS { get; set; }

        public double HPS { get; set; }

        public double PercentOfTotalOverallDamage { get; set; }

        public double PercentOfTotalOverallDamageOverTime { get; set; }

        public double PercentOfTotalOverallDamageTaken { get; set; }

        public double PercentOfTotalOverallDamageTakenOverTime { get; set; }

        public double PercentOfTotalOverallHealing { get; set; }

        public double PercentOfTotalOverallHealingMitigated { get; set; }

        public double PercentOfTotalOverallHealingOverHealing { get; set; }

        public double PercentOfTotalOverallHealingOverTime { get; set; }

        public List<PlayerEntity> Players {
            get {
                return this._players ?? (this._players = new List<PlayerEntity>());
            }

            set {
                this._players = value;
            }
        }

        public double TotalOverallDamage { get; set; }

        public double TotalOverallDamageOverTime { get; set; }

        public double TotalOverallDamageTaken { get; set; }

        public double TotalOverallDamageTakenOverTime { get; set; }

        public double TotalOverallHealing { get; set; }

        public double TotalOverallHealingMitigated { get; set; }

        public double TotalOverallHealingOverHealing { get; set; }

        public double TotalOverallHealingOverTime { get; set; }
    }
}