// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogPublisher.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   LogPublisher.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Utilities {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.Events;

    using NLog;

    using Sharlayan.Core;

    public static class LogPublisher {
        public static bool IsPaused;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static List<string> _needGreedHistory;

        public static List<string> NeedGreedHistory {
            get {
                return _needGreedHistory ?? (_needGreedHistory = new List<string>());
            }

            set {
                if (_needGreedHistory == null) {
                    _needGreedHistory = new List<string>();
                }

                _needGreedHistory = value;
            }
        }

        public static bool Processing { get; set; }

        public static void HandleCommands(ChatLogItem chatLogItem) {
            // process commands
            if (chatLogItem.Code == "0038") {
                Match commandsRegEx = CommandBuilder.CommandsRegEx.Match(chatLogItem.Line.Trim());
                if (!commandsRegEx.Success) {
                    return;
                }

                var command = commandsRegEx.Groups["command"].Success
                                  ? commandsRegEx.Groups["command"].Value
                                  : string.Empty;
                switch (command) {
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

        public static void Process(ChatLogItem chatLogItem) {
            if (IsPaused) {
                return;
            }

            try {
                chatLogItem.Line = chatLogItem.Line.Replace("  ", " ");
                if (Constants.NeedGreed.Any(chatLogItem.Line.Contains)) {
                    NeedGreedHistory.Add(chatLogItem.Line);
                }

                DispatcherHelper.Invoke(() => EventParser.Instance.ParseAndPublish(chatLogItem));
                HandleCommands(chatLogItem);
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }
    }
}