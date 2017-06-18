// FFXIVAPP.Plugin.Parse ~ FilterHelper.cs
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

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class FilterHelper
    {
        private static bool IsEnabled(UInt64 filters, UInt64 filter)
        {
            return (filters & filter) != 0;
        }

        public static UInt64 Toggle(UInt64 filters, UInt64 filter)
        {
            return IsEnabled(filters, filter) ? Disable(filters, filter) : Enable(filters, filter);
        }

        public static UInt64 Enable(UInt64 filters, UInt64 filter)
        {
            if (IsEnabled(filters, filter))
            {
                return filters;
            }
            return (filters | filter);
        }

        public static UInt64 Disable(UInt64 filters, UInt64 filter)
        {
            if (IsEnabled(filters, filter))
            {
                return (filters & (~filter));
            }
            return filters;
        }
    }
}
