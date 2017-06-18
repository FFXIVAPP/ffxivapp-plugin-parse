// FFXIVAPP.Plugin.Parse ~ Filter.Cure.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
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
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessCure(Event e, Expressions exp)
        {
            var line = new Line(e.ChatLogEntry)
            {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type
            };
            LineHelper.SetTimelineTypes(ref line);
            if (LineHelper.IsIgnored(line))
            {
                return;
            }
            var cure = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            line.Target = You;
                            break;
                    }
                    cure = exp.pCure;
                    if (cure.Success)
                    {
                        line.Source = You;
                        UpdateHealing(cure, line, exp, FilterType.You);
                    }
                    break;
                case EventSubject.Pet:
                    cure = exp.pCure;
                    if (cure.Success)
                    {
                        line.Source = _lastNamePet;
                        UpdateHealing(cure, line, exp, FilterType.Pet);
                    }
                    break;
                case EventSubject.Party:
                    cure = exp.pCure;
                    if (cure.Success)
                    {
                        line.Source = _lastNamePartyHealingFrom;
                        UpdateHealing(cure, line, exp, FilterType.Party);
                    }
                    break;
                case EventSubject.PetParty:
                    cure = exp.pCure;
                    if (cure.Success)
                    {
                        line.Source = _lastNamePetPartyHealingFrom;
                        UpdateHealing(cure, line, exp, FilterType.PetParty);
                    }
                    break;
                case EventSubject.Alliance:
                    cure = exp.pCure;
                    if (cure.Success)
                    {
                        line.Source = _lastNameAllianceHealingFrom;
                        UpdateHealing(cure, line, exp, FilterType.Alliance);
                    }
                    break;
                case EventSubject.PetAlliance:
                    cure = exp.pCure;
                    if (cure.Success)
                    {
                        line.Source = _lastNamePetAllianceHealingFrom;
                        UpdateHealing(cure, line, exp, FilterType.PetAlliance);
                    }
                    break;
                case EventSubject.Other:
                    cure = exp.pCure;
                    if (cure.Success)
                    {
                        line.Source = _lastNameOtherHealingFrom;
                        UpdateHealing(cure, line, exp, FilterType.Other);
                    }
                    break;
                case EventSubject.PetOther:
                    cure = exp.pCure;
                    if (cure.Success)
                    {
                        line.Source = _lastNamePetOtherHealingFrom;
                        UpdateHealing(cure, line, exp, FilterType.PetOther);
                    }
                    break;
            }
            if (cure.Success)
            {
                return;
            }
            ParsingLogHelper.Log(Logger, "Cure", e, exp);
        }

        private static void UpdateHealing(Match cure, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                if (String.IsNullOrWhiteSpace(line.Source))
                {
                    line.Source = Convert.ToString(cure.Groups["source"]
                                                       .Value);
                }
                if (String.IsNullOrWhiteSpace(line.Target))
                {
                    line.Target = Convert.ToString(cure.Groups["target"]
                                                       .Value);
                }
                switch (type)
                {
                    case FilterType.You:
                        line.Action = _lastActionYou;
                        break;
                    case FilterType.Pet:
                        line.Action = _lastActionPet;
                        break;
                    case FilterType.Party:
                        line.Action = _lastActionPartyHealingFrom;
                        break;
                    case FilterType.PetParty:
                        line.Action = _lastActionPetPartyHealingFrom;
                        break;
                    case FilterType.Alliance:
                        line.Action = _lastActionAllianceHealingFrom;
                        break;
                    case FilterType.PetAlliance:
                        line.Action = _lastActionPetAllianceHealingFrom;
                        break;
                    case FilterType.Other:
                        line.Action = _lastActionOtherHealingFrom;
                        break;
                    case FilterType.PetOther:
                        line.Action = _lastActionPetOtherHealingFrom;
                        break;
                }
                line.Amount = cure.Groups["amount"]
                                  .Success ? Convert.ToDouble(cure.Groups["amount"]
                                                                  .Value) : 0;
                line.Crit = cure.Groups["crit"]
                                .Success;
                line.Modifier = cure.Groups["modifier"]
                                    .Success ? Convert.ToDouble(cure.Groups["modifier"]
                                                                    .Value) / 100 : 0;
                switch (line.EventDirection)
                {
                    case EventDirection.You:
                        line.Target = You;
                        break;
                }
                line.RecLossType = Convert.ToString(cure.Groups["type"]
                                                        .Value.ToUpperInvariant());
                if (line.IsEmpty())
                {
                    return;
                }
                if (line.RecLossType == exp.HealingType)
                {
                    ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                                .SetHealing(line);
                }
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(Logger, "Cure", exp.Event, ex);
            }
        }
    }
}
