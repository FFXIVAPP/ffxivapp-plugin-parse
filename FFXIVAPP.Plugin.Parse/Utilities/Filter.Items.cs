// FFXIVAPP.Plugin.Parse
// Filter.Items.cs
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
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;

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
                                _lastActionYou = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
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
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNamePet = line.Source;
                                _lastActionPet = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
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
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNamePartyFrom = line.Source;
                                _lastActionPartyFrom = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
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
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNamePetPartyFrom = line.Source;
                                _lastActionPet = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
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
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNameAllianceFrom = line.Source;
                                _lastActionAllianceFrom = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
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
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNamePetAllianceFrom = line.Source;
                                _lastActionPet = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
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
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNameOtherFrom = line.Source;
                                _lastActionOtherFrom = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
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
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNamePetOtherFrom = line.Source;
                                _lastActionPet = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
                            }
                            break;
                    }
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    break;
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
