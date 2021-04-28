// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventParser.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   EventParser.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Events {
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using FFXIVAPP.Plugin.Parse.Enums;

    using Sharlayan.Core;

    public class EventParser {
        public const ulong AllEvents = 0xFFFFFFFFFFF;

        public const ulong Alliance = (ulong) EventDirection.Alliance | (ulong) EventSubject.Alliance;

        public const ulong DirectionMask = 0x1FFF;

        public const ulong Engaged = (ulong) EventDirection.Engaged | (ulong) EventSubject.Engaged;

        public const ulong FriendlyNPC = (ulong) EventDirection.FriendlyNPC | (ulong) EventSubject.FriendlyNPC;

        public const ulong NPC = (ulong) EventDirection.NPC | (ulong) EventSubject.NPC;

        public const ulong Other = (ulong) EventDirection.Other | (ulong) EventSubject.Other;

        public const ulong Party = (ulong) EventDirection.Party | (ulong) EventSubject.Party;

        public const ulong Pet = (ulong) EventDirection.Pet | (ulong) EventSubject.Pet;

        public const ulong PetAlliance = (ulong) EventDirection.PetAlliance | (ulong) EventSubject.PetAlliance;

        public const ulong PetOther = (ulong) EventDirection.PetOther | (ulong) EventSubject.PetOther;

        public const ulong PetParty = (ulong) EventDirection.PetParty | (ulong) EventSubject.PetParty;

        public const ulong Self = (ulong) EventDirection.Self;

        public const ulong SubjectMask = 0x1FFE000;

        public const ulong TypeMask = 0x3FFFE000000;

        public const ulong UnEngaged = (ulong) EventDirection.UnEngaged | (ulong) EventSubject.UnEngaged;

        public const ulong Unknown = (ulong) EventDirection.Unknown | (ulong) EventSubject.Unknown;

        public const ulong UnknownEvent = 0x0;

        public const ulong You = (ulong) EventDirection.You | (ulong) EventSubject.You;

        private static Lazy<EventParser> _instance = new Lazy<EventParser>(() => new EventParser(Constants.ChatCodesXML));

        private readonly SortedDictionary<ulong, EventCode> _eventCodes = new SortedDictionary<ulong, EventCode>();

        private string LastKnownChatCodesXml = string.Empty;

        /// <summary>
        /// </summary>
        /// <param name="xml"> </param>
        private EventParser(string xml) {
            this.Initialize(xml);
        }

        public event EventHandler<Event> OnLogEvent = delegate { };

        public event EventHandler<Event> OnUnknownLogEvent = delegate { };

        /// <summary>
        /// </summary>
        public static EventParser Instance {
            get {
                return _instance.Value;
            }
        }

        private SortedDictionary<ulong, EventCode> EventCodes {
            get {
                return this._eventCodes;
            }
        }

        public void Initialize(string xml) {
            if (string.IsNullOrWhiteSpace(xml)) {
                return;
            }

            if (this.EventCodes.Count != 0 && this.LastKnownChatCodesXml == xml) {
                return;
            }

            this.EventCodes.Clear();
            this.LastKnownChatCodesXml = xml;
            this.LoadCodes(XElement.Parse(xml));
        }

        /// <summary>
        /// </summary>
        /// <param name="chatLogItem"></param>
        public void ParseAndPublish(ChatLogItem chatLogItem) {
            Event @event = this.Parse(chatLogItem);
            EventHandler<Event> eventHandler = @event.IsUnknown
                                                   ? this.OnUnknownLogEvent
                                                   : this.OnLogEvent;
            if (eventHandler == null) {
                return;
            }

            lock (eventHandler) {
                eventHandler(this, @event);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root"> </param>
        private void LoadCodes(XContainer root) {
            foreach (XElement group in root.Elements("Group")) {
                this.LoadGroups(group, new EventGroup("All"));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root"> </param>
        /// <param name="parent"> </param>
        private void LoadGroups(XElement root, EventGroup parent) {
            var thisGroup = new EventGroup((string) root.Attribute("Name"), parent);
            var type = (string) root.Attribute("Type");
            var subject = (string) root.Attribute("Subject");
            var direction = (string) root.Attribute("Direction");
            if (type != null) {
                switch (type) {
                    case "Damage":
                        thisGroup.Type = EventType.Damage;
                        break;
                    case "Failed":
                        thisGroup.Type = EventType.Failed;
                        break;
                    case "Actions":
                        thisGroup.Type = EventType.Actions;
                        break;
                    case "Items":
                        thisGroup.Type = EventType.Items;
                        break;
                    case "Cure":
                        thisGroup.Type = EventType.Cure;
                        break;
                    case "Beneficial":
                        thisGroup.Type = EventType.Beneficial;
                        break;
                    case "Detrimental":
                        thisGroup.Type = EventType.Detrimental;
                        break;
                    case "System":
                        thisGroup.Type = EventType.System;
                        break;
                    case "Battle":
                        thisGroup.Type = EventType.Battle;
                        break;
                    case "Synthesis":
                        thisGroup.Type = EventType.Synthesis;
                        break;
                    case "Gathering":
                        thisGroup.Type = EventType.Gathering;
                        break;
                    case "Error":
                        thisGroup.Type = EventType.Error;
                        break;
                    case "Echo":
                        thisGroup.Type = EventType.Echo;
                        break;
                    case "Dialogue":
                        thisGroup.Type = EventType.Dialogue;
                        break;
                    case "Loot":
                        thisGroup.Type = EventType.Loot;
                        break;
                    case "Progression":
                        thisGroup.Type = EventType.Progression;
                        break;
                    case "Defeats":
                        thisGroup.Type = EventType.Defeats;
                        break;
                }
            }

            if (subject != null) {
                switch (subject) {
                    case "You":
                        thisGroup.Subject = EventSubject.You;
                        break;
                    case "Party":
                        thisGroup.Subject = EventSubject.Party;
                        break;
                    case "Other":
                        thisGroup.Subject = EventSubject.Other;
                        break;
                    case "NPC":
                        thisGroup.Subject = EventSubject.NPC;
                        break;
                    case "Alliance":
                        thisGroup.Subject = EventSubject.Alliance;
                        break;
                    case "FriendlyNPC":
                        thisGroup.Subject = EventSubject.FriendlyNPC;
                        break;
                    case "Pet":
                        thisGroup.Subject = EventSubject.Pet;
                        break;
                    case "PetParty":
                        thisGroup.Subject = EventSubject.PetParty;
                        break;
                    case "PetAlliance":
                        thisGroup.Subject = EventSubject.PetAlliance;
                        break;
                    case "PetOther":
                        thisGroup.Subject = EventSubject.PetOther;
                        break;
                    case "Engaged":
                        thisGroup.Subject = EventSubject.Engaged;
                        break;
                    case "UnEngaged":
                        thisGroup.Subject = EventSubject.UnEngaged;
                        break;
                }
            }

            if (direction != null) {
                switch (direction) {
                    case "Self":
                        thisGroup.Direction = EventDirection.Self;
                        break;
                    case "You":
                        thisGroup.Direction = EventDirection.You;
                        break;
                    case "Party":
                        thisGroup.Direction = EventDirection.Party;
                        break;
                    case "Other":
                        thisGroup.Direction = EventDirection.Other;
                        break;
                    case "NPC":
                        thisGroup.Direction = EventDirection.NPC;
                        break;
                    case "Alliance":
                        thisGroup.Direction = EventDirection.Alliance;
                        break;
                    case "FriendlyNPC":
                        thisGroup.Direction = EventDirection.FriendlyNPC;
                        break;
                    case "Pet":
                        thisGroup.Direction = EventDirection.Pet;
                        break;
                    case "PetParty":
                        thisGroup.Direction = EventDirection.PetParty;
                        break;
                    case "PetAlliance":
                        thisGroup.Direction = EventDirection.PetAlliance;
                        break;
                    case "PetOther":
                        thisGroup.Direction = EventDirection.PetOther;
                        break;
                    case "Engaged":
                        thisGroup.Direction = EventDirection.Engaged;
                        break;
                    case "UnEngaged":
                        thisGroup.Direction = EventDirection.UnEngaged;
                        break;
                }
            }

            foreach (XElement group in root.Elements("Group")) {
                this.LoadGroups(group, thisGroup);
            }

            foreach (XElement xElement in root.Elements("Code")) {
                var xKey = Convert.ToUInt64((string) xElement.Attribute("Key"), 16);
                var xDescription = (string) xElement.Element("Description");
                this._eventCodes.Add(xKey, new EventCode(xDescription, xKey, thisGroup));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="chatLogItem"></param>
        /// <returns></returns>
        private Event Parse(ChatLogItem chatLogItem) {
            EventCode eventCode;
            var code = Convert.ToUInt64(chatLogItem.Code, 16);
            if (this.EventCodes.TryGetValue(code, out eventCode)) {
                return new Event(eventCode, chatLogItem);
            }

            var unknownEventCode = new EventCode {
                Code = code,
            };
            return new Event(unknownEventCode, chatLogItem);
        }
    }
}