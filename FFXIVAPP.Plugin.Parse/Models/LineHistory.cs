// FFXIVAPP.Plugin.Parse ~ LineHistory.cs
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
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Memory.Core;
using FFXIVAPP.Plugin.Parse.ViewModels;
using NLog;

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class LineHistory
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public LineHistory(Line line)
        {
            TimeStamp = DateTime.Now;
            Line = line;
            SourceStatusEntries = new List<StatusEntry>();
            TargetStatusEntries = new List<StatusEntry>();
            uint PlayerID = 0;
            try
            {
                var monsterEntries = XIVInfoViewModel.Instance.CurrentMonsters.Select(entity => entity.Value)
                                                     .ToList();
                var pcEntries = XIVInfoViewModel.Instance.CurrentPCs.Select(entity => entity.Value)
                                                .ToList();
                // process you => monster
                foreach (var actorEntity in pcEntries)
                {
                    if (!String.Equals(actorEntity.Name, line.Source, Constants.InvariantComparer))
                    {
                        continue;
                    }
                    PlayerID = actorEntity.ID;
                    foreach (var statusEntry in actorEntity.StatusEntries)
                    {
                        SourceStatusEntries.Add(statusEntry);
                    }
                }
                foreach (var actorEntity in monsterEntries)
                {
                    if (!String.Equals(actorEntity.Name, line.Target, Constants.InvariantComparer))
                    {
                        return;
                    }
                    foreach (var statusEntry in actorEntity.StatusEntries)
                    {
                        if (statusEntry.CasterID == PlayerID)
                        {
                            TargetStatusEntries.Add(statusEntry);
                        }
                    }
                }
                // process monster => you
                foreach (var actorEntity in pcEntries)
                {
                    if (!String.Equals(actorEntity.Name, line.Target, Constants.InvariantComparer))
                    {
                        continue;
                    }
                    PlayerID = actorEntity.ID;
                    foreach (var statusEntry in actorEntity.StatusEntries)
                    {
                        TargetStatusEntries.Add(statusEntry);
                    }
                }
                foreach (var actorEntity in monsterEntries)
                {
                    if (!String.Equals(actorEntity.Name, line.Source, Constants.InvariantComparer))
                    {
                        return;
                    }
                    foreach (var statusEntry in actorEntity.StatusEntries)
                    {
                        if (statusEntry.CasterID == PlayerID)
                        {
                            SourceStatusEntries.Add(statusEntry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public DateTime TimeStamp { get; set; }
        public Line Line { get; set; }

        #region StatEnties [Source|Target]

        public List<StatusEntry> SourceStatusEntries { get; set; }
        public List<StatusEntry> TargetStatusEntries { get; set; }

        #endregion
    }
}
