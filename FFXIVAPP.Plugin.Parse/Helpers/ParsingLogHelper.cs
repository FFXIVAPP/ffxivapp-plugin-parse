// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParsingLogHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ParsingLogHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Helpers {
    using System;

    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.Events;

    using NLog;

    public static class ParsingLogHelper {
        public static void Error(Logger logger, string type, Event e, Exception ex) {
            var data = $"{type} Error: [{ex.Message}] Line -> {e.ChatLogItem.Line} StackTrace: \n{ex.StackTrace}";
            Logging.Log(logger, data, ex);
        }

        public static void Log(Logger logger, string type, Event e, Expressions exp = null) {
            var cleaned = $"{e.ChatLogItem.Code}:{e.ChatLogItem.Line}";
            if (exp != null) {
                cleaned = $"{e.Code}:{exp.Cleaned}";
            }

            var data = $"Unknown {type} Line -> [Type:{e.Type}][Subject:{e.Subject}][Direction:{e.Direction}] {cleaned}";
            Logging.Log(logger, data);
        }
    }
}