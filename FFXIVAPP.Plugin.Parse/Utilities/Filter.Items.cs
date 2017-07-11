// FFXIVAPP.Plugin.Parse ~ Filter.Items.cs
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
using Sharlayan.Helpers;

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessItems(Event e, Expressions exp)
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
            var items = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success)
                            {
                                line.Source = You;
                                _lastActionYou = StringHelper.TitleCase(Convert.ToString(items.Groups["item"]
                                                                                              .Value));
                                UpdateItems(items, line, exp, FilterType.You);
                            }
                            break;
                    }
                    break;
                case EventSubject.Pet:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success)
                            {
                                line.Source = Convert.ToString(items.Groups["source"]
                                                                    .Value);
                                _lastNamePet = line.Source;
                                _lastActionPet = StringHelper.TitleCase(Convert.ToString(items.Groups["item"]
                                                                                              .Value));
                            }
                            break;
                    }
                    break;
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success)
                            {
                                line.Source = Convert.ToString(items.Groups["source"]
                                                                    .Value);
                                _lastNamePartyFrom = line.Source;
                                _lastActionPartyFrom = StringHelper.TitleCase(Convert.ToString(items.Groups["item"]
                                                                                                    .Value));
                                UpdateItems(items, line, exp, FilterType.Party);
                            }
                            break;
                    }
                    break;
                case EventSubject.PetParty:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success)
                            {
                                line.Source = Convert.ToString(items.Groups["source"]
                                                                    .Value);
                                _lastNamePetPartyFrom = line.Source;
                                _lastActionPet = StringHelper.TitleCase(Convert.ToString(items.Groups["item"]
                                                                                              .Value));
                            }
                            break;
                    }
                    break;
                case EventSubject.Alliance:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success)
                            {
                                line.Source = Convert.ToString(items.Groups["source"]
                                                                    .Value);
                                _lastNameAllianceFrom = line.Source;
                                _lastActionAllianceFrom = StringHelper.TitleCase(Convert.ToString(items.Groups["item"]
                                                                                                       .Value));
                                UpdateItems(items, line, exp, FilterType.Alliance);
                            }
                            break;
                    }
                    break;
                case EventSubject.PetAlliance:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success)
                            {
                                line.Source = Convert.ToString(items.Groups["source"]
                                                                    .Value);
                                _lastNamePetAllianceFrom = line.Source;
                                _lastActionPet = StringHelper.TitleCase(Convert.ToString(items.Groups["item"]
                                                                                              .Value));
                            }
                            break;
                    }
                    break;
                case EventSubject.Other:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success)
                            {
                                line.Source = Convert.ToString(items.Groups["source"]
                                                                    .Value);
                                _lastNameOtherFrom = line.Source;
                                _lastActionOtherFrom = StringHelper.TitleCase(Convert.ToString(items.Groups["item"]
                                                                                                    .Value));
                                UpdateItems(items, line, exp, FilterType.Other);
                            }
                            break;
                    }
                    break;
                case EventSubject.PetOther:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success)
                            {
                                line.Source = Convert.ToString(items.Groups["source"]
                                                                    .Value);
                                _lastNamePetOtherFrom = line.Source;
                                _lastActionPet = StringHelper.TitleCase(Convert.ToString(items.Groups["item"]
                                                                                              .Value));
                            }
                            break;
                    }
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged: break;
            }
            if (items.Success)
            {
                return;
            }
            ParsingLogHelper.Log(Logger, "Item", e, exp);
        }

        private static void UpdateItems(Match item, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
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
                switch (line.EventDirection)
                {
                    case EventDirection.You:
                        line.Target = You;
                        break;
                    default:
                        line.Target = line.Source;
                        break;
                }
                if (line.IsEmpty())
                {
                    return;
                }
                var playerInstance = ParseControl.Instance.Timeline.GetSetPlayer(line.Source);
                playerInstance.Last20Items.Add(new LineHistory(line));
                if (playerInstance.Last20Items.Count > 20)
                {
                    playerInstance.Last20Items.RemoveAt(0);
                }
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(Logger, "Item", exp.Event, ex);
            }
        }
    }
}
