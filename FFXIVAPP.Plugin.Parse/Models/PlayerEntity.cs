// FFXIVAPP.Plugin.Parse ~ PlayerEntity.cs
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
using System.Text.RegularExpressions;
using FFXIVAPP.Memory.Core.Enums;
using FFXIVAPP.Memory.Helpers;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Interfaces;

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class PlayerEntity : IPlayerEntity
    {
        private string _name;

        public string FirstName
        {
            get
            {
                try
                {
                    return Name.Split(' ')[0];
                }
                catch (Exception ex)
                {
                    return Name;
                }
            }
        }

        public string LastName
        {
            get
            {
                try
                {
                    return Name.Split(' ')[1];
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public string NameInitialsOnly
        {
            get
            {
                var missingLastName = String.IsNullOrWhiteSpace(LastName);
                try
                {
                    if (missingLastName)
                    {
                        return String.Format("{0}.", FirstName.Substring(0, 1));
                    }
                    return String.Format("{0}.{1}.", FirstName.Substring(0, 1), LastName.Substring(0, 1));
                }
                catch (Exception ex)
                {
                    return Name;
                }
            }
        }

        public PlayerType Type { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = StringHelper.TitleCase(Regex.Replace(value, @"\[[\w]+\]", "")
                                                    .Trim());
            }
        }

        public Actor.Job Job { get; set; }
        public double CombinedDPS { get; set; }
        public double DPS { get; set; }
        public double DOTPS { get; set; }
        public double CombinedHPS { get; set; }
        public double HPS { get; set; }
        public double HOTPS { get; set; }
        public double HOHPS { get; set; }
        public double HMPS { get; set; }
        public double CombinedDTPS { get; set; }
        public double DTPS { get; set; }
        public double DTOTPS { get; set; }
        public double CombinedTotalOverallDamage { get; set; }
        public double TotalOverallDamage { get; set; }
        public double TotalOverallDamageOverTime { get; set; }
        public double CombinedTotalOverallHealing { get; set; }
        public double TotalOverallHealing { get; set; }
        public double TotalOverallHealingOverTime { get; set; }
        public double TotalOverallHealingOverHealing { get; set; }
        public double TotalOverallHealingMitigated { get; set; }
        public double CombinedTotalOverallDamageTaken { get; set; }
        public double TotalOverallDamageTaken { get; set; }
        public double TotalOverallDamageTakenOverTime { get; set; }
        public double PercentOfTotalOverallDamage { get; set; }
        public double PercentOfTotalOverallDamageOverTime { get; set; }
        public double PercentOfTotalOverallHealing { get; set; }
        public double PercentOfTotalOverallHealingOverTime { get; set; }
        public double PercentOfTotalOverallHealingOverHealing { get; set; }
        public double PercentOfTotalOverallHealingMitigated { get; set; }
        public double PercentOfTotalOverallDamageTaken { get; set; }
        public double PercentOfTotalOverallDamageTakenOverTime { get; set; }
    }
}
