// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   FilterHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Helpers {
    public static class FilterHelper {
        public static ulong Disable(ulong filters, ulong filter) {
            if (IsEnabled(filters, filter)) {
                return filters & ~filter;
            }

            return filters;
        }

        public static ulong Enable(ulong filters, ulong filter) {
            if (IsEnabled(filters, filter)) {
                return filters;
            }

            return filters | filter;
        }

        public static ulong Toggle(ulong filters, ulong filter) {
            return IsEnabled(filters, filter)
                       ? Disable(filters, filter)
                       : Enable(filters, filter);
        }

        private static bool IsEnabled(ulong filters, ulong filter) {
            return (filters & filter) != 0;
        }
    }
}