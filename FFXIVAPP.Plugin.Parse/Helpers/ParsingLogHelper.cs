// FFXIVAPP.Plugin.Parse ~ ParsingLogHelper.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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
            var cleaned = String.Format("{0}:{1}", e.ChatLogEntry.Code, e.ChatLogEntry.Line);
            if (exp != null)
            {
                cleaned = String.Format("{0}:{1}", e.Code, exp.Cleaned);
            }
            var data = String.Format("Unknown {0} Line -> [Type:{1}][Subject:{2}][Direction:{3}] {4}", type, e.Type, e.Subject, e.Direction, cleaned);
            Logging.Log(logger, data);
        }

        public static void Error(Logger logger, string type, Event e, Exception ex)
        {
            var data = String.Format("{0} Error: [{1}] Line -> {2} StackTrace: \n{3}", type, ex.Message, e.ChatLogEntry.Line, ex.StackTrace);
            Logging.Log(logger, data, ex);
        }
    }
}
