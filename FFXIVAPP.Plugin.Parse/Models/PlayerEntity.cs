// FFXIVAPP.Plugin.Parse
// PlayerEntity.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

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
