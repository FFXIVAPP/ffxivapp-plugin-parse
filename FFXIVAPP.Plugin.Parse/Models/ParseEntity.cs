// FFXIVAPP.Plugin.Parse
// ParseEntity.cs
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

using System.Collections.Generic;
using FFXIVAPP.Plugin.Parse.Interfaces;

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class ParseEntity : IParseEntity
    {
        private List<PlayerEntity> _players;

        public List<PlayerEntity> Players
        {
            get { return _players ?? (_players = new List<PlayerEntity>()); }
            set { _players = value; }
        }

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
