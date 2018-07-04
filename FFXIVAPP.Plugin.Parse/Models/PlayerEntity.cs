// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerEntity.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PlayerEntity.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System;
    using System.Text.RegularExpressions;

    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Interfaces;

    using Sharlayan.Core.Enums;
    using Sharlayan.Extensions;

    public class PlayerEntity : IPlayerEntity {
        private string _name;

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

        public string FirstName {
            get {
                try {
                    return this.Name.Split(' ')[0];
                }
                catch (Exception) {
                    return this.Name;
                }
            }
        }

        public double HMPS { get; set; }

        public double HOHPS { get; set; }

        public double HOTPS { get; set; }

        public double HPS { get; set; }

        public Actor.Job Job { get; set; }

        public string LastName {
            get {
                try {
                    return this.Name.Split(' ')[1];
                }
                catch (Exception) {
                    return string.Empty;
                }
            }
        }

        public string Name {
            get {
                return this._name;
            }

            set {
                this._name = Regex.Replace(value, @"\[[\w]+\]", string.Empty).Trim().ToTitleCase();
            }
        }

        public string NameInitialsOnly {
            get {
                var missingLastName = string.IsNullOrWhiteSpace(this.LastName);
                try {
                    if (missingLastName) {
                        return $"{this.FirstName.Substring(0, 1)}.";
                    }

                    return $"{this.FirstName.Substring(0, 1)}.{this.LastName.Substring(0, 1)}.";
                }
                catch (Exception) {
                    return this.Name;
                }
            }
        }

        public double PercentOfTotalOverallDamage { get; set; }

        public double PercentOfTotalOverallDamageOverTime { get; set; }

        public double PercentOfTotalOverallDamageTaken { get; set; }

        public double PercentOfTotalOverallDamageTakenOverTime { get; set; }

        public double PercentOfTotalOverallHealing { get; set; }

        public double PercentOfTotalOverallHealingMitigated { get; set; }

        public double PercentOfTotalOverallHealingOverHealing { get; set; }

        public double PercentOfTotalOverallHealingOverTime { get; set; }

        public double TotalOverallDamage { get; set; }

        public double TotalOverallDamageOverTime { get; set; }

        public double TotalOverallDamageTaken { get; set; }

        public double TotalOverallDamageTakenOverTime { get; set; }

        public double TotalOverallHealing { get; set; }

        public double TotalOverallHealingMitigated { get; set; }

        public double TotalOverallHealingOverHealing { get; set; }

        public double TotalOverallHealingOverTime { get; set; }

        public PlayerType Type { get; set; }
    }
}