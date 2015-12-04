// FFXIVAPP.Plugin.Parse ~ LogPublisher.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Memory.Core;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static class LogPublisher
    {
        public static bool IsPaused;
        public static bool Processing { get; set; }

        public static void Process(ChatLogEntry chatLogEntry)
        {
            if (IsPaused)
            {
                return;
            }
            try
            {
                chatLogEntry.Line = chatLogEntry.Line.Replace("  ", " ");
                if (Constants.NeedGreed.Any(chatLogEntry.Line.Contains))
                {
                    NeedGreedHistory.Add(chatLogEntry.Line);
                }
                DispatcherHelper.Invoke(() => EventParser.Instance.ParseAndPublish(chatLogEntry));
                HandleCommands(chatLogEntry);
            }
            catch (Exception ex)
            {
            }
        }

        public static void HandleCommands(ChatLogEntry chatLogEntry)
        {
            // process commands
            if (chatLogEntry.Code == "0038")
            {
                var commandsRegEx = CommandBuilder.CommandsRegEx.Match(chatLogEntry.Line.Trim());
                if (!commandsRegEx.Success)
                {
                    return;
                }
                var command = commandsRegEx.Groups["command"].Success ? commandsRegEx.Groups["command"].Value : "";
                switch (command)
                {
                    case "on":
                        IsPaused = false;
                        break;
                    case "off":
                        IsPaused = true;
                        break;
                    case "reset":
                        ParseControl.Instance.Reset();
                        break;
                }
            }
        }

        #region Property Bindings

        private static List<string> _needGreedHistory;

        public static List<string> NeedGreedHistory
        {
            get { return _needGreedHistory ?? (_needGreedHistory = new List<string>()); }
            set
            {
                if (_needGreedHistory == null)
                {
                    _needGreedHistory = new List<string>();
                }
                _needGreedHistory = value;
            }
        }

        #endregion
    }
}
