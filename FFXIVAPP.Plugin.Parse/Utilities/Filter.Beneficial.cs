// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.Beneficial.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Filter.Beneficial.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Utilities {
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.Events;
    using FFXIVAPP.Plugin.Parse.ViewModels;

    using Sharlayan.Core;
    using Sharlayan.Core.Enums;

    public static partial class Filter {
        private static void ProcessBeneficial(Event e, Expressions exp) {
            var line = new Line(e.ChatLogItem) {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type,
            };
            LineHelper.SetTimelineTypes(ref line);
            if (LineHelper.IsIgnored(line)) {
                return;
            }

            Match beneficial = Regex.Match("ph", @"^\.$");
            switch (e.Subject) {
                case EventSubject.You:
                    switch (e.Direction) {
                        case EventDirection.Self:
                            line.Target = You;
                            break;
                    }

                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success) {
                        line.Source = You;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.You);
                    }

                    break;
                case EventSubject.Pet:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success) {
                        line.Source = _lastNamePet;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.Pet);
                    }

                    break;
                case EventSubject.Party:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success) {
                        line.Source = _lastNamePartyFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.Party);
                    }

                    break;
                case EventSubject.PetParty:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success) {
                        line.Source = _lastNamePetPartyFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.PetParty);
                    }

                    break;
                case EventSubject.Alliance:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success) {
                        line.Source = _lastNameAllianceFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.Alliance);
                    }

                    break;
                case EventSubject.PetAlliance:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success) {
                        line.Source = _lastNamePetAllianceFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.PetAlliance);
                    }

                    break;
                case EventSubject.Other:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success) {
                        line.Source = _lastNameOtherFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.Other);
                    }

                    break;
                case EventSubject.PetOther:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success) {
                        line.Source = _lastNamePetOtherFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.PetOther);
                    }

                    break;
            }

            if (beneficial.Success) {
                return;
            }

            ParsingLogHelper.Log(Logger, "Beneficial", e, exp);
        }

        private static void UpdateBeneficialGain(Match beneficial, Line line, Expressions exp, FilterType type) {
            _type = type;
            try {
                if (string.IsNullOrWhiteSpace(line.Source)) {
                    line.Source = Convert.ToString(beneficial.Groups["source"].Value);
                }

                if (string.IsNullOrWhiteSpace(line.Target)) {
                    line.Target = Convert.ToString(beneficial.Groups["target"].Value);
                }

                line.Action = Convert.ToString(beneficial.Groups["status"].Value);
                var isStoneSkin = false;
                foreach (var stoneSkin in MagicBarrierHelper.StoneSkin.Where(stoneSkin => string.Equals(stoneSkin, line.Action, Constants.InvariantComparer))) {
                    isStoneSkin = true;
                }

                switch (line.EventDirection) {
                    case EventDirection.You:
                        line.Target = You;
                        break;
                }

                if (line.IsEmpty()) {
                    return;
                }

                if (isStoneSkin) {
                    var multiplier = 0.1m;
                    try {
                        var cleanedName = Regex.Replace(line.Source, @"\[[\w]+\]", string.Empty).Trim();
                        ActorItem source = XIVInfoViewModel.Instance.CurrentPCs.FirstOrDefault(kvp => kvp.Value.Name.Equals(cleanedName, Constants.InvariantComparer)).Value;
                        if (source != null) {
                            multiplier = source.Job == Actor.Job.WHM
                                             ? 0.18m
                                             : multiplier;
                        }
                    }
                    catch (Exception ex) {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }

                    try {
                        var cleanedName = Regex.Replace(line.Target, @"\[[\w]+\]", string.Empty).Trim();
                        ActorItem target = XIVInfoViewModel.Instance.CurrentPCs.FirstOrDefault(kvp => kvp.Value.Name.Equals(cleanedName, Constants.InvariantComparer)).Value;
                        if (target != null) {
                            line.Amount = (double) (target.HPMax * multiplier);
                            ParseControl.Instance.Timeline.GetSetPlayer(line.Source).SetupHealingMitigated(line, "stoneskin");
                        }
                    }
                    catch (Exception ex) {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }
                }
            }
            catch (Exception ex) {
                ParsingLogHelper.Error(Logger, "Cure", exp.Event, ex);
            }
        }
    }
}