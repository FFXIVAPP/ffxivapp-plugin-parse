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

namespace FFXIVAPP.Plugin.Parse.Models.History
{
    public class HistoryGroupPropertyDescriptor : HistoryStatPropertyDescriptor
    {
        public HistoryGroupPropertyDescriptor(string name) : base(name)
        {
        }

        #region Overrides of StatPropertyDescriptor

        public override Type PropertyType
        {
            get { return Name.ToLower() == "name" ? typeof (string) : typeof (HistoryGroup); }
        }

        public override object GetValue(object component)
        {
            if (Name.ToLower() == "name")
            {
                return ((HistoryGroup) component).Name;
            }
            return ((HistoryGroup) component).GetGroup(Name);
        }

        #endregion
    }
}
