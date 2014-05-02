// FFXIVAPP.Plugin.Parse
// EntityHelper.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Properties;
using FFXIVAPP.Plugin.Parse.Windows;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class EntityHelper
    {
        public static class Parse
        {
            public enum ParseType
            {
                DPS,
                DTPS,
                HPS
            }

            public static void CleanAndCopy(ParseEntity source, ParseType parseType)
            {
                try
                {
                    var target = new ParseEntity
                    {
                        CombinedDPS = source.CombinedDPS,
                        DPS = source.DPS,
                        DOTPS = source.DOTPS,
                        CombinedHPS = source.CombinedHPS,
                        HPS = source.HPS,
                        HOTPS = source.HOTPS,
                        HOHPS = source.HOHPS,
                        HMPS = source.HMPS,
                        CombinedDTPS = source.CombinedDTPS,
                        DTPS = source.DTPS,
                        DTOTPS = source.DTOTPS,
                        CombinedTotalOverallDamage = source.CombinedTotalOverallDamage,
                        TotalOverallDamage = source.TotalOverallDamage,
                        TotalOverallDamageOverTime = source.TotalOverallDamageOverTime,
                        CombinedTotalOverallHealing = source.CombinedTotalOverallHealing,
                        TotalOverallHealing = source.TotalOverallHealing,
                        TotalOverallHealingOverTime = source.TotalOverallHealingOverTime,
                        TotalOverallHealingOverHealing = source.TotalOverallHealingOverHealing,
                        TotalOverallHealingMitigated = source.TotalOverallHealingMitigated,
                        CombinedTotalOverallDamageTaken = source.CombinedTotalOverallDamageTaken,
                        TotalOverallDamageTaken = source.TotalOverallDamageTaken,
                        TotalOverallDamageTakenOverTime = source.TotalOverallDamageTakenOverTime,
                        PercentOfTotalOverallDamage = source.PercentOfTotalOverallDamage,
                        PercentOfTotalOverallDamageOverTime = source.PercentOfTotalOverallDamageOverTime,
                        PercentOfTotalOverallHealing = source.PercentOfTotalOverallHealing,
                        PercentOfTotalOverallHealingOverTime = source.PercentOfTotalOverallHealingOverTime,
                        PercentOfTotalOverallHealingOverHealing = source.PercentOfTotalOverallHealingOverHealing,
                        PercentOfTotalOverallHealingMitigated = source.PercentOfTotalOverallHealingMitigated,
                        PercentOfTotalOverallDamageTaken = source.PercentOfTotalOverallDamageTaken,
                        PercentOfTotalOverallDamageTakenOverTime = source.PercentOfTotalOverallDamageTakenOverTime,
                        Players = new List<PlayerEntity>()
                    };
                    foreach (var playerEntity in source.Players)
                    {
                        try
                        {
                            switch (parseType)
                            {
                                case ParseType.DPS:
                                    decimal dps;
                                    decimal.TryParse(Settings.Default.DPSVisibility, out dps);
                                    if (playerEntity.CombinedDPS <= dps)
                                    {
                                        continue;
                                    }
                                    break;
                                case ParseType.DTPS:
                                    decimal dtps;
                                    decimal.TryParse(Settings.Default.DTPSVisibility, out dtps);
                                    if (playerEntity.CombinedDTPS <= dtps)
                                    {
                                        continue;
                                    }
                                    break;
                                case ParseType.HPS:
                                    decimal hps;
                                    decimal.TryParse(Settings.Default.HPSVisibility, out hps);
                                    if (playerEntity.CombinedHPS <= hps)
                                    {
                                        continue;
                                    }
                                    break;
                            }
                            var entity = new PlayerEntity
                            {
                                Name = playerEntity.Name,
                                Job = playerEntity.Job,
                                CombinedDPS = playerEntity.CombinedDPS,
                                DPS = playerEntity.DPS,
                                DOTPS = playerEntity.DOTPS,
                                CombinedHPS = playerEntity.CombinedHPS,
                                HPS = playerEntity.HPS,
                                HOTPS = playerEntity.HOTPS,
                                HOHPS = playerEntity.HOHPS,
                                HMPS = playerEntity.HMPS,
                                CombinedDTPS = playerEntity.CombinedDTPS,
                                DTPS = playerEntity.DTPS,
                                DTOTPS = playerEntity.DTOTPS,
                                CombinedTotalOverallDamage = playerEntity.CombinedTotalOverallDamage,
                                TotalOverallDamage = playerEntity.TotalOverallDamage,
                                TotalOverallDamageOverTime = playerEntity.TotalOverallDamageOverTime,
                                CombinedTotalOverallHealing = playerEntity.CombinedTotalOverallHealing,
                                TotalOverallHealing = playerEntity.TotalOverallHealing,
                                TotalOverallHealingOverTime = playerEntity.TotalOverallHealingOverTime,
                                TotalOverallHealingOverHealing = playerEntity.TotalOverallHealingOverHealing,
                                TotalOverallHealingMitigated = playerEntity.TotalOverallHealingMitigated,
                                CombinedTotalOverallDamageTaken = playerEntity.CombinedTotalOverallDamageTaken,
                                TotalOverallDamageTaken = playerEntity.TotalOverallDamageTaken,
                                TotalOverallDamageTakenOverTime = playerEntity.TotalOverallDamageTakenOverTime,
                                PercentOfTotalOverallDamage = playerEntity.PercentOfTotalOverallDamage,
                                PercentOfTotalOverallDamageOverTime = playerEntity.PercentOfTotalOverallDamageOverTime,
                                PercentOfTotalOverallHealing = playerEntity.PercentOfTotalOverallHealing,
                                PercentOfTotalOverallHealingOverTime = playerEntity.PercentOfTotalOverallHealingOverTime,
                                PercentOfTotalOverallHealingOverHealing = playerEntity.PercentOfTotalOverallHealingOverHealing,
                                PercentOfTotalOverallHealingMitigated = playerEntity.PercentOfTotalOverallHealingMitigated,
                                PercentOfTotalOverallDamageTaken = playerEntity.PercentOfTotalOverallDamageTaken,
                                PercentOfTotalOverallDamageTakenOverTime = playerEntity.PercentOfTotalOverallDamageTakenOverTime,
                                Type = playerEntity.Type
                            };
                            target.Players.Add(entity);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    // sort entity based on settings
                    switch (parseType)
                    {
                        case ParseType.DPS:
                            if (target.Players.Any())
                            {
                                switch (Settings.Default.DPSWidgetSortDirection)
                                {
                                    case "Descending":
                                        switch (Settings.Default.DPSWidgetSortProperty)
                                        {
                                            case "Name":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.Name));
                                                break;
                                            case "Job":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.Job));
                                                break;
                                            case "DPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.DPS));
                                                break;
                                            case "CombinedDPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.CombinedDPS));
                                                break;
                                            case "TotalOverallDamage":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.TotalOverallDamage));
                                                break;
                                            case "CombinedTotalOverallDamage":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.CombinedTotalOverallDamage));
                                                break;
                                            case "PercentOfTotalOverallDamage":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.PercentOfTotalOverallDamage));
                                                break;
                                        }
                                        break;
                                    default:
                                        switch (Settings.Default.DPSWidgetSortProperty)
                                        {
                                            case "Name":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.Name));
                                                break;
                                            case "Job":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.Job));
                                                break;
                                            case "DPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.DPS));
                                                break;
                                            case "CombinedDPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.CombinedDPS));
                                                break;
                                            case "TotalOverallDamage":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.TotalOverallDamage));
                                                break;
                                            case "CombinedTotalOverallDamage":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.CombinedTotalOverallDamage));
                                                break;
                                            case "PercentOfTotalOverallDamage":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.PercentOfTotalOverallDamage));
                                                break;
                                        }
                                        break;
                                }
                            }
                            DPSWidgetViewModel.Instance.ParseEntity = target;
                            break;
                        case ParseType.DTPS:
                            if (target.Players.Any())
                            {
                                switch (Settings.Default.DTPSWidgetSortDirection)
                                {
                                    case "Descending":
                                        switch (Settings.Default.DTPSWidgetSortProperty)
                                        {
                                            case "Name":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.Name));
                                                break;
                                            case "Job":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.Job));
                                                break;
                                            case "DTPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.DTPS));
                                                break;
                                            case "CombinedDTPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.CombinedDTPS));
                                                break;
                                            case "TotalOverallDamageTaken":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.TotalOverallDamageTaken));
                                                break;
                                            case "CombinedTotalOverallDamageTaken":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.CombinedTotalOverallDamageTaken));
                                                break;
                                            case "PercentOfTotalOverallDamageTaken":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.PercentOfTotalOverallDamageTaken));
                                                break;
                                        }
                                        break;
                                    default:
                                        switch (Settings.Default.DTPSWidgetSortProperty)
                                        {
                                            case "Name":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.Name));
                                                break;
                                            case "Job":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.Job));
                                                break;
                                            case "DTPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.DTPS));
                                                break;
                                            case "CombinedDTPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.CombinedDTPS));
                                                break;
                                            case "TotalOverallDamageTaken":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.TotalOverallDamageTaken));
                                                break;
                                            case "CombinedTotalOverallDamageTaken":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.CombinedTotalOverallDamageTaken));
                                                break;
                                            case "PercentOfTotalOverallDamageTaken":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.PercentOfTotalOverallDamageTaken));
                                                break;
                                        }
                                        break;
                                }
                            }
                            DTPSWidgetViewModel.Instance.ParseEntity = target;
                            break;
                        case ParseType.HPS:
                            if (target.Players.Any())
                            {
                                switch (Settings.Default.HPSWidgetSortDirection)
                                {
                                    case "Descending":
                                        switch (Settings.Default.HPSWidgetSortProperty)
                                        {
                                            case "Name":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.Name));
                                                break;
                                            case "Job":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.Job));
                                                break;
                                            case "HPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.HPS));
                                                break;
                                            case "CombinedHPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.CombinedHPS));
                                                break;
                                            case "TotalOverallHealing":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.TotalOverallHealing));
                                                break;
                                            case "CombinedTotalOverallHealing":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.CombinedTotalOverallHealing));
                                                break;
                                            case "PercentOfTotalOverallHealing":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderByDescending(p => p.PercentOfTotalOverallHealing));
                                                break;
                                        }
                                        break;
                                    default:
                                        switch (Settings.Default.HPSWidgetSortProperty)
                                        {
                                            case "Name":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.Name));
                                                break;
                                            case "Job":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.Job));
                                                break;
                                            case "HPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.HPS));
                                                break;
                                            case "CombinedHPS":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.CombinedHPS));
                                                break;
                                            case "TotalOverallHealing":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.TotalOverallHealing));
                                                break;
                                            case "CombinedTotalOverallHealing":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.CombinedTotalOverallHealing));
                                                break;
                                            case "PercentOfTotalOverallHealing":
                                                target.Players = new List<PlayerEntity>(target.Players.OrderBy(p => p.PercentOfTotalOverallHealing));
                                                break;
                                        }
                                        break;
                                }
                            }
                            HPSWidgetViewModel.Instance.ParseEntity = target;
                            break;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static class Target
        {
        }
    }
}
