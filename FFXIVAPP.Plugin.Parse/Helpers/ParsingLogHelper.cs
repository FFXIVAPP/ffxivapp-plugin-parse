// FFXIVAPP.Plugin.Parse ~ ParsingLogHelper.cs
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
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using NLog;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class ParsingLogHelper
    {
        public static void Log(Logger logger, string type, Event e, Expressions exp = null)
        {
            var cleaned = $"{e.ChatLogEntry.Code}:{e.ChatLogEntry.Line}";
            if (exp != null)
            {
                cleaned = $"{e.Code}:{exp.Cleaned}";
            }
            var data = $"Unknown {type} Line -> [Type:{e.Type}][Subject:{e.Subject}][Direction:{e.Direction}] {cleaned}";
            Logging.Log(logger, data);
        }

        public static void Error(Logger logger, string type, Event e, Exception ex)
        {
            var data = $"{type} Error: [{ex.Message}] Line -> {e.ChatLogEntry.Line} StackTrace: \n{ex.StackTrace}";
            Logging.Log(logger, data, ex);
        }
    }
}
