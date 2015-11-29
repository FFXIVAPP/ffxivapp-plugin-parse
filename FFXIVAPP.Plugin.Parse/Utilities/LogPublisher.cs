// FFXIVAPP.Plugin.Parse
// LogPublisher.cs
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
