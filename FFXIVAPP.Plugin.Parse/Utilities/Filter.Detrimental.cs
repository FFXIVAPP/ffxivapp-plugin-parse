// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.Detrimental.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Filter.Detrimental.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Utilities {
    using System.Text.RegularExpressions;

    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.Events;

    public static partial class Filter {
        private static void ProcessDetrimental(Event e, Expressions exp) {
            var line = new Line(e.ChatLogItem) {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type
            };
            Match detrimental = Regex.Match("ph", @"^\.$");
            if (detrimental.Success) {
                return;
            }

            ParsingLogHelper.Log(Logger, "Detrimental", e, exp);
        }
    }
}