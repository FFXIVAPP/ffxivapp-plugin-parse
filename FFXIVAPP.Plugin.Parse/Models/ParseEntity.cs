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

        public decimal CombinedDPS { get; set; }
        public decimal DPS { get; set; }
        public decimal DOTPS { get; set; }
        public decimal CombinedHPS { get; set; }
        public decimal HPS { get; set; }
        public decimal HOTPS { get; set; }
        public decimal HOHPS { get; set; }
        public decimal HMPS { get; set; }
        public decimal CombinedDTPS { get; set; }
        public decimal DTPS { get; set; }
        public decimal DTOTPS { get; set; }
        public decimal CombinedTotalOverallDamage { get; set; }
        public decimal TotalOverallDamage { get; set; }
        public decimal TotalOverallDamageOverTime { get; set; }
        public decimal CombinedTotalOverallHealing { get; set; }
        public decimal TotalOverallHealing { get; set; }
        public decimal TotalOverallHealingOverTime { get; set; }
        public decimal TotalOverallHealingOverHealing { get; set; }
        public decimal TotalOverallHealingMitigated { get; set; }
        public decimal CombinedTotalOverallDamageTaken { get; set; }
        public decimal TotalOverallDamageTaken { get; set; }
        public decimal TotalOverallDamageTakenOverTime { get; set; }
        public decimal PercentOfTotalOverallDamage { get; set; }
        public decimal PercentOfTotalOverallDamageOverTime { get; set; }
        public decimal PercentOfTotalOverallHealing { get; set; }
        public decimal PercentOfTotalOverallHealingOverTime { get; set; }
        public decimal PercentOfTotalOverallHealingOverHealing { get; set; }
        public decimal PercentOfTotalOverallHealingMitigated { get; set; }
        public decimal PercentOfTotalOverallDamageTaken { get; set; }
        public decimal PercentOfTotalOverallDamageTakenOverTime { get; set; }
    }
}
