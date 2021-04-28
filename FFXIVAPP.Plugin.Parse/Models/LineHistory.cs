// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineHistory.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   LineHistory.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.ViewModels;

    using NLog;

    using Sharlayan.Core;

    public class LineHistory {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public LineHistory(Line line) {
            this.TimeStamp = DateTime.Now;
            this.Line = line;
            this.SourceStatusEntries = new List<StatusItem>();
            this.TargetStatusEntries = new List<StatusItem>();
            uint PlayerID = 0;
            try {
                List<ActorItem> monsterEntries = XIVInfoViewModel.Instance.CurrentMonsters.Select(entity => entity.Value).ToList();
                List<ActorItem> pcEntries = XIVInfoViewModel.Instance.CurrentPCs.Select(entity => entity.Value).ToList();

                // process you => monster
                foreach (ActorItem actorEntity in pcEntries) {
                    if (!string.Equals(actorEntity.Name, line.Source, Constants.InvariantComparer)) {
                        continue;
                    }

                    PlayerID = actorEntity.ID;
                    foreach (StatusItem statusEntry in actorEntity.StatusItems) {
                        this.SourceStatusEntries.Add(statusEntry);
                    }
                }

                foreach (ActorItem actorEntity in monsterEntries) {
                    if (!string.Equals(actorEntity.Name, line.Target, Constants.InvariantComparer)) {
                        return;
                    }

                    foreach (StatusItem statusEntry in actorEntity.StatusItems) {
                        if (statusEntry.CasterID == PlayerID) {
                            this.TargetStatusEntries.Add(statusEntry);
                        }
                    }
                }

                // process monster => you
                foreach (ActorItem actorEntity in pcEntries) {
                    if (!string.Equals(actorEntity.Name, line.Target, Constants.InvariantComparer)) {
                        continue;
                    }

                    PlayerID = actorEntity.ID;
                    foreach (StatusItem statusEntry in actorEntity.StatusItems) {
                        this.TargetStatusEntries.Add(statusEntry);
                    }
                }

                foreach (ActorItem actorEntity in monsterEntries) {
                    if (!string.Equals(actorEntity.Name, line.Source, Constants.InvariantComparer)) {
                        return;
                    }

                    foreach (StatusItem statusEntry in actorEntity.StatusItems) {
                        if (statusEntry.CasterID == PlayerID) {
                            this.SourceStatusEntries.Add(statusEntry);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        public Line Line { get; set; }

        public List<StatusItem> SourceStatusEntries { get; set; }

        public List<StatusItem> TargetStatusEntries { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}