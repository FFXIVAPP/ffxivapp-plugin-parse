// FFXIVAPP.Plugin.Parse
// FFXIVAPP & Related Plugins/Modules
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

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class ParseHistoryItem
    {
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        public ParseHistoryItem(string name = "UnknownEvent")
        {
            Name = name;
            HistoryControl = new HistoryControl();
        }

        #region "Auto Properties"

        public string Name { get; set; }
        public HistoryControl HistoryControl { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan ParseLength { get; set; }

        #endregion
    }
}
