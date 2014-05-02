// FFXIVAPP.Plugin.Parse
// IParseEntity.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
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
using FFXIVAPP.Plugin.Parse.Models;

namespace FFXIVAPP.Plugin.Parse.Interfaces
{
    public interface IParseEntity
    {
        List<PlayerEntity> Players { get; set; }
        decimal CombinedDPS { get; set; }
        decimal DPS { get; set; }
        decimal DOTPS { get; set; }
        decimal CombinedHPS { get; set; }
        decimal HPS { get; set; }
        decimal HOTPS { get; set; }
        decimal HOHPS { get; set; }
        decimal HMPS { get; set; }
        decimal CombinedDTPS { get; set; }
        decimal DTPS { get; set; }
        decimal DTOTPS { get; set; }
        decimal CombinedTotalOverallDamage { get; set; }
        decimal TotalOverallDamage { get; set; }
        decimal TotalOverallDamageOverTime { get; set; }
        decimal CombinedTotalOverallHealing { get; set; }
        decimal TotalOverallHealing { get; set; }
        decimal TotalOverallHealingOverTime { get; set; }
        decimal TotalOverallHealingOverHealing { get; set; }
        decimal TotalOverallHealingMitigated { get; set; }
        decimal CombinedTotalOverallDamageTaken { get; set; }
        decimal TotalOverallDamageTaken { get; set; }
        decimal TotalOverallDamageTakenOverTime { get; set; }
        decimal PercentOfTotalOverallDamage { get; set; }
        decimal PercentOfTotalOverallDamageOverTime { get; set; }
        decimal PercentOfTotalOverallHealing { get; set; }
        decimal PercentOfTotalOverallHealingOverTime { get; set; }
        decimal PercentOfTotalOverallHealingOverHealing { get; set; }
        decimal PercentOfTotalOverallHealingMitigated { get; set; }
        decimal PercentOfTotalOverallDamageTaken { get; set; }
        decimal PercentOfTotalOverallDamageTakenOverTime { get; set; }
    }
}
