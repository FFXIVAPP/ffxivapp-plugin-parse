// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IParseEntity.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   IParseEntity.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Interfaces {
    using System.Collections.Generic;

    using FFXIVAPP.Plugin.Parse.Models;

    public interface IParseEntity {
        double CombinedDPS { get; set; }

        double CombinedDTPS { get; set; }

        double CombinedHPS { get; set; }

        double CombinedTotalOverallDamage { get; set; }

        double CombinedTotalOverallDamageTaken { get; set; }

        double CombinedTotalOverallHealing { get; set; }

        double DOTPS { get; set; }

        double DPS { get; set; }

        double DTOTPS { get; set; }

        double DTPS { get; set; }

        double HMPS { get; set; }

        double HOHPS { get; set; }

        double HOTPS { get; set; }

        double HPS { get; set; }

        double PercentOfTotalOverallDamage { get; set; }

        double PercentOfTotalOverallDamageOverTime { get; set; }

        double PercentOfTotalOverallDamageTaken { get; set; }

        double PercentOfTotalOverallDamageTakenOverTime { get; set; }

        double PercentOfTotalOverallHealing { get; set; }

        double PercentOfTotalOverallHealingMitigated { get; set; }

        double PercentOfTotalOverallHealingOverHealing { get; set; }

        double PercentOfTotalOverallHealingOverTime { get; set; }

        List<PlayerEntity> Players { get; set; }

        double TotalOverallDamage { get; set; }

        double TotalOverallDamageOverTime { get; set; }

        double TotalOverallDamageTaken { get; set; }

        double TotalOverallDamageTakenOverTime { get; set; }

        double TotalOverallHealing { get; set; }

        double TotalOverallHealingMitigated { get; set; }

        double TotalOverallHealingOverHealing { get; set; }

        double TotalOverallHealingOverTime { get; set; }
    }
}