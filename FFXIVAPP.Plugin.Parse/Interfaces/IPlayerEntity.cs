// FFXIVAPP.Plugin.Parse
// IPlayerEntity.cs
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

using FFXIVAPP.Memory.Core.Enums;
using FFXIVAPP.Plugin.Parse.Enums;

namespace FFXIVAPP.Plugin.Parse.Interfaces
{
    public interface IPlayerEntity
    {
        PlayerType Type { get; set; }
        string Name { get; set; }
        Actor.Job Job { get; set; }
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
