// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.Items.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Filter.Items.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Utilities {
    using System;
    using System.Text.RegularExpressions;

    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.Events;
    using FFXIVAPP.Plugin.Parse.Models.StatGroups;

    using Sharlayan.Extensions;

    public static partial class Filter {
        private static void ProcessItems(Event e, Expressions exp) {
            var line = new Line(e.ChatLogItem) {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type
            };
            LineHelper.SetTimelineTypes(ref line);
            if (LineHelper.IsIgnored(line)) {
                return;
            }

            Match items = Regex.Match("ph", @"^\.$");
            switch (e.Subject) {
                case EventSubject.You:
                    switch (e.Direction) {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success) {
                                line.Source = You;
                                _lastActionYou = Convert.ToString(items.Groups["item"].Value).ToTitleCase();
                                UpdateItems(items, line, exp, FilterType.You);
                            }

                            break;
                    }

                    break;
                case EventSubject.Pet:
                    switch (e.Direction) {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success) {
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNamePet = line.Source;
                                _lastActionPet = Convert.ToString(items.Groups["item"].Value).ToTitleCase();
                            }

                            break;
                    }

                    break;
                case EventSubject.Party:
                    switch (e.Direction) {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success) {
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNamePartyFrom = line.Source;
                                _lastActionPartyFrom = Convert.ToString(items.Groups["item"].Value).ToTitleCase();
                                UpdateItems(items, line, exp, FilterType.Party);
                            }

                            break;
                    }

                    break;
                case EventSubject.PetParty:
                    switch (e.Direction) {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success) {
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNamePetPartyFrom = line.Source;
                                _lastActionPet = Convert.ToString(items.Groups["item"].Value).ToTitleCase();
                            }

                            break;
                    }

                    break;
                case EventSubject.Alliance:
                    switch (e.Direction) {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success) {
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNameAllianceFrom = line.Source;
                                _lastActionAllianceFrom = Convert.ToString(items.Groups["item"].Value).ToTitleCase();
                                UpdateItems(items, line, exp, FilterType.Alliance);
                            }

                            break;
                    }

                    break;
                case EventSubject.PetAlliance:
                    switch (e.Direction) {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success) {
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNamePetAllianceFrom = line.Source;
                                _lastActionPet = Convert.ToString(items.Groups["item"].Value).ToTitleCase();
                            }

                            break;
                    }

                    break;
                case EventSubject.Other:
                    switch (e.Direction) {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success) {
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNameOtherFrom = line.Source;
                                _lastActionOtherFrom = Convert.ToString(items.Groups["item"].Value).ToTitleCase();
                                UpdateItems(items, line, exp, FilterType.Other);
                            }

                            break;
                    }

                    break;
                case EventSubject.PetOther:
                    switch (e.Direction) {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success) {
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNamePetOtherFrom = line.Source;
                                _lastActionPet = Convert.ToString(items.Groups["item"].Value).ToTitleCase();
                            }

                            break;
                    }

                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    break;
            }

            if (items.Success) {
                return;
            }

            ParsingLogHelper.Log(Logger, "Item", e, exp);
        }

        private static void UpdateItems(Match item, Line line, Expressions exp, FilterType type) {
            _type = type;
            try {
                switch (type) {
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

                switch (line.EventDirection) {
                    case EventDirection.You:
                        line.Target = You;
                        break;
                    default:
                        line.Target = line.Source;
                        break;
                }

                if (line.IsEmpty()) {
                    return;
                }

                Player playerInstance = ParseControl.Instance.Timeline.GetSetPlayer(line.Source);
                playerInstance.Last20Items.Add(new LineHistory(line));
                if (playerInstance.Last20Items.Count > 20) {
                    playerInstance.Last20Items.RemoveAt(0);
                }
            }
            catch (Exception ex) {
                ParsingLogHelper.Error(Logger, "Item", exp.Event, ex);
            }
        }
    }
}