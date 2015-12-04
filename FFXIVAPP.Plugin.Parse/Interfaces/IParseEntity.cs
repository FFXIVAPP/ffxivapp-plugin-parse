// FFXIVAPP.Plugin.Parse ~ IParseEntity.cs
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

using System.Collections.Generic;
using FFXIVAPP.Plugin.Parse.Models;

namespace FFXIVAPP.Plugin.Parse.Interfaces
{
    public interface IParseEntity
    {
        List<PlayerEntity> Players { get; set; }
        double CombinedDPS { get; set; }
        double DPS { get; set; }
        double DOTPS { get; set; }
        double CombinedHPS { get; set; }
        double HPS { get; set; }
        double HOTPS { get; set; }
        double HOHPS { get; set; }
        double HMPS { get; set; }
        double CombinedDTPS { get; set; }
        double DTPS { get; set; }
        double DTOTPS { get; set; }
        double CombinedTotalOverallDamage { get; set; }
        double TotalOverallDamage { get; set; }
        double TotalOverallDamageOverTime { get; set; }
        double CombinedTotalOverallHealing { get; set; }
        double TotalOverallHealing { get; set; }
        double TotalOverallHealingOverTime { get; set; }
        double TotalOverallHealingOverHealing { get; set; }
        double TotalOverallHealingMitigated { get; set; }
        double CombinedTotalOverallDamageTaken { get; set; }
        double TotalOverallDamageTaken { get; set; }
        double TotalOverallDamageTakenOverTime { get; set; }
        double PercentOfTotalOverallDamage { get; set; }
        double PercentOfTotalOverallDamageOverTime { get; set; }
        double PercentOfTotalOverallHealing { get; set; }
        double PercentOfTotalOverallHealingOverTime { get; set; }
        double PercentOfTotalOverallHealingOverHealing { get; set; }
        double PercentOfTotalOverallHealingMitigated { get; set; }
        double PercentOfTotalOverallDamageTaken { get; set; }
        double PercentOfTotalOverallDamageTakenOverTime { get; set; }
    }
}
