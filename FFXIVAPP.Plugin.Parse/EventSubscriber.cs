// FFXIVAPP.Plugin.Parse
// EventSubscriber.cs
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
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.IPluginInterface.Events;
using FFXIVAPP.Plugin.Parse.Delegates;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Utilities;

namespace FFXIVAPP.Plugin.Parse
{
    public static class EventSubscriber
    {
        public static void Subscribe()
        {
            Plugin.PHost.NewConstantsEntity += OnNewConstantsEntity;
            Plugin.PHost.NewChatLogEntry += OnNewChatLogEntry;
            Plugin.PHost.NewMonsterEntries += OnNewMonsterEntries;
            Plugin.PHost.NewNPCEntries += OnNewNPCEntries;
            Plugin.PHost.NewPCEntries += OnNewPCEntries;
            //Plugin.PHost.NewPlayerEntity += OnNewPlayerEntity;
            //Plugin.PHost.NewTargetEntity += OnNewTargetEntity;
            //Plugin.PHost.NewPartyEntries += OnNewPartyEntries;
        }

        public static void UnSubscribe()
        {
            Plugin.PHost.NewConstantsEntity -= OnNewConstantsEntity;
            Plugin.PHost.NewChatLogEntry -= OnNewChatLogEntry;
            Plugin.PHost.NewMonsterEntries -= OnNewMonsterEntries;
            Plugin.PHost.NewNPCEntries -= OnNewNPCEntries;
            Plugin.PHost.NewPCEntries -= OnNewPCEntries;
            //Plugin.PHost.NewPlayerEntity -= OnNewPlayerEntity;
            //Plugin.PHost.NewTargetEntity -= OnNewTargetEntity;
            //Plugin.PHost.NewPartyEntries -= OnNewPartyEntries;
        }

        #region Subscriptions

        private static void OnNewConstantsEntity(object sender, ConstantsEntityEvent constantsEntityEvent)
        {
            // delegate event from constants, not required to subsribe, but recommended as it gives you app settings
            if (sender == null)
            {
                return;
            }
            var constantsEntity = constantsEntityEvent.ConstantsEntity;
            Constants.AutoTranslate = constantsEntity.AutoTranslate;
            Constants.ChatCodes = constantsEntity.ChatCodes;
            Constants.ChatCodesXml = constantsEntity.ChatCodesXml;
            if (!String.IsNullOrWhiteSpace(Constants.ChatCodesXml))
            {
                EventParser.Instance.Initialize(Constants.ChatCodesXml);
            }
            Constants.Colors = constantsEntity.Colors;
            Constants.CultureInfo = constantsEntity.CultureInfo;
            Constants.CharacterName = constantsEntity.CharacterName;
            Constants.ServerName = constantsEntity.ServerName;
            Constants.GameLanguage = constantsEntity.GameLanguage;
            Constants.EnableHelpLabels = constantsEntity.EnableHelpLabels;
            Constants.Theme = constantsEntity.Theme;
            Constants.UIScale = constantsEntity.UIScale;
            PluginViewModel.Instance.EnableHelpLabels = Constants.EnableHelpLabels;
            PluginViewModel.Instance.UIScale = Constants.UIScale;
        }

        private static void OnNewChatLogEntry(object sender, ChatLogEntryEvent chatLogEntryEvent)
        {
            // delegate event from chat log, not required to subsribe
            // this updates 100 times a second and only sends a line when it gets a new one
            if (sender == null)
            {
                return;
            }
            var chatLogEntry = chatLogEntryEvent.ChatLogEntry;
            try
            {
                LogPublisher.Process(chatLogEntry);
            }
            catch (Exception ex)
            {
            }
        }

        private static void OnNewMonsterEntries(object sender, ActorEntitiesEvent actorEntitiesEvent)
        {
            // delegate event from monster entities from ram, not required to subsribe
            // this updates 10x a second and only sends data if the items are found in ram
            // currently there no change/new/removed event handling (looking into it)
            if (sender == null)
            {
                return;
            }
            var monsterEntities = actorEntitiesEvent.ActorEntities;
            if (!monsterEntities.Any())
            {
                return;
            }
            MonsterWorkerDelegate.ReplaceNPCEntities(new List<ActorEntity>(monsterEntities));
            Func<bool> saveToDictionary = delegate
            {
                try
                {
                    var enumerable = MonsterWorkerDelegate.GetUniqueNPCEntities();
                    foreach (var actor in monsterEntities)
                    {
                        var exists = enumerable.FirstOrDefault(n => n.ID == actor.ID);
                        if (exists != null)
                        {
                            continue;
                        }
                        MonsterWorkerDelegate.AddUniqueNPCEntity(actor);
                    }
                }
                catch (Exception ex)
                {
                }
                return true;
            };
            saveToDictionary.BeginInvoke(null, saveToDictionary);
        }

        private static void OnNewNPCEntries(object sender, ActorEntitiesEvent actorEntitiesEvent)
        {
            // delegate event from npc entities from ram, not required to subsribe
            // this list includes anything that is not a player or monster
            // this updates 10x a second and only sends data if the items are found in ram
            // currently there no change/new/removed event handling (looking into it)
            if (sender == null)
            {
                return;
            }
            var npcEntities = actorEntitiesEvent.ActorEntities;
            if (!npcEntities.Any())
            {
                return;
            }
            NPCWorkerDelegate.ReplaceNPCEntities(new List<ActorEntity>(npcEntities));
            Func<bool> saveToDictionary = delegate
            {
                try
                {
                    var enumerable = NPCWorkerDelegate.GetUniqueNPCEntities();
                    foreach (var actor in npcEntities)
                    {
                        var exists = enumerable.FirstOrDefault(n => n.NPCID2 == actor.NPCID2);
                        if (exists != null)
                        {
                            continue;
                        }
                        NPCWorkerDelegate.AddUniqueNPCEntity(actor);
                    }
                }
                catch (Exception ex)
                {
                }
                return true;
            };
            saveToDictionary.BeginInvoke(null, saveToDictionary);
        }

        private static void OnNewPCEntries(object sender, ActorEntitiesEvent actorEntitiesEvent)
        {
            // delegate event from player entities from ram, not required to subsribe
            // this updates 10x a second and only sends data if the items are found in ram
            // currently there no change/new/removed event handling (looking into it)
            if (sender == null)
            {
                return;
            }
            var pcEntities = actorEntitiesEvent.ActorEntities;
            if (!pcEntities.Any())
            {
                return;
            }
            PCWorkerDelegate.CurrentUser = pcEntities.First();
            PCWorkerDelegate.ReplaceNPCEntities(new List<ActorEntity>(pcEntities));
            Func<bool> saveToDictionary = delegate
            {
                try
                {
                    var enumerable = PCWorkerDelegate.GetUniqueNPCEntities();
                    foreach (var actor in pcEntities)
                    {
                        var exists = enumerable.FirstOrDefault(n => String.Equals(n.Name, actor.Name, Constants.InvariantComparer));
                        if (exists != null)
                        {
                            continue;
                        }
                        PCWorkerDelegate.AddUniqueNPCEntity(actor);
                    }
                }
                catch (Exception ex)
                {
                }
                return true;
            };
            saveToDictionary.BeginInvoke(null, saveToDictionary);
        }

        //private static void OnNewPlayerEntity(object sender, PlayerEntityEvent playerEntityEvent)
        //{
        //    // delegate event from player info from ram, not required to subsribe
        //    // this is for YOU and includes all your stats and your agro list with hate values as %
        //    // this updates 10x a second and only sends data when the newly read data is differen than what was previously sent
        //    if (sender == null)
        //    {
        //        return;
        //    }
        //    var playerEntity = playerEntityEvent.PlayerEntity;
        //}

        //private static void OnNewTargetEntity(object sender, TargetEntityEvent targetEntityEvent)
        //{
        //    // delegate event from target info from ram, not required to subsribe
        //    // this includes the full entities for current, previous, mouseover, focus targets (if 0+ are found)
        //    // it also includes a list of upto 16 targets that currently have hate on the currently targeted monster
        //    // these hate values are realtime and change based on the action used
        //    // this updates 10x a second and only sends data when the newly read data is differen than what was previously sent
        //    if (sender == null)
        //    {
        //        return;
        //    }
        //    var targetEntity = targetEntityEvent.TargetEntity;
        //}

        //private static void OnNewPartyEntries(object sender, PartyEntitiesEvent partyEntitiesEvent)
        //{
        //    // delegate event from party info worker that will give basic info on party members
        //    if (sender == null)
        //    {
        //        return;
        //    }
        //    var partyEntities = partyEntitiesEvent.PartyEntities;
        //}

        #endregion
    }
}
