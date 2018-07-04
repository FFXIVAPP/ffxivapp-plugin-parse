// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Filter.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Utilities {
    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.Events;
    using FFXIVAPP.Plugin.Parse.Properties;

    using NLog;

    public static partial class Filter {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static FilterType _type;

        private static string You {
            get {
                return string.IsNullOrWhiteSpace(Constants.CharacterName.Trim())
                           ? "You"
                           : Constants.CharacterName;
            }
        }

        public static void Process(Event e) {
            if (!ParseControl.Instance.FirstActionFound) {
                switch (Settings.Default.StoreHistoryEvent) {
                    case "Any":
                        ParseControl.Instance.Reset();
                        break;
                    case "Damage Only":
                        if (e.Type == EventType.Damage) {
                            ParseControl.Instance.Reset();
                        }

                        break;
                }
            }

            _lastEventYou = _lastEventYou ?? new Event();
            _lastEventPet = _lastEventPet ?? new Event();
            _lastEventParty = _lastEventParty ?? new Event();
            _lastEventPetParty = _lastEventPetParty ?? new Event();
            _lastEventAlliance = _lastEventAlliance ?? new Event();
            _lastEventPetAlliance = _lastEventPetAlliance ?? new Event();

            _type = FilterType.Unknown;

            var expressions = new Expressions(e);

            switch (Settings.Default.StoreHistoryEvent) {
                case "Any":
                    ParseControl.Instance.Timeline.StoreHistoryTimer.Stop();
                    break;
                case "Damage Only":
                    if (e.Type == EventType.Damage) {
                        ParseControl.Instance.Timeline.StoreHistoryTimer.Stop();
                    }

                    break;
            }

            switch (e.Type) {
                case EventType.Damage:
                case EventType.Cure:
                    ParseControl.Instance.Timeline.FightingRightNow = true;
                    ParseControl.Instance.Timeline.FightingTimer.Stop();
                    break;
            }

            switch (e.Type) {
                case EventType.Damage:
                    ProcessDamage(e, expressions);
                    break;
                case EventType.Failed:
                    ProcessFailed(e, expressions);
                    break;
                case EventType.Actions:
                    ProcessActions(e, expressions);
                    break;
                case EventType.Items:
                    ProcessItems(e, expressions);
                    break;
                case EventType.Cure:
                    ProcessCure(e, expressions);
                    break;
                case EventType.Beneficial:
                    ProcessBeneficial(e, expressions);
                    break;
                case EventType.Detrimental:
                    ProcessDetrimental(e, expressions);
                    break;
            }

            switch (_type) {
                case FilterType.You:
                    _lastEventYou = e;
                    break;
                case FilterType.Pet:
                    _lastEventPet = e;
                    break;
                case FilterType.Party:
                    _lastEventParty = e;
                    break;
                case FilterType.PetParty:
                    _lastEventPetParty = e;
                    break;
                case FilterType.Alliance:
                    _lastEventAlliance = e;
                    break;
                case FilterType.PetAlliance:
                    _lastEventPetAlliance = e;
                    break;
            }

            switch (Settings.Default.StoreHistoryEvent) {
                case "Any":
                    ParseControl.Instance.Timeline.StoreHistoryTimer.Start();
                    break;
                case "Damage Only":
                    if (e.Type == EventType.Damage) {
                        ParseControl.Instance.Timeline.StoreHistoryTimer.Start();
                    }

                    break;
            }

            switch (e.Type) {
                case EventType.Damage:
                case EventType.Cure:
                    ParseControl.Instance.Timeline.FightingTimer.Start();
                    break;
            }
        }
    }
}