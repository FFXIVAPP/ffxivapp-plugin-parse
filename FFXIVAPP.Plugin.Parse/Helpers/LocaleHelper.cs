// FFXIVAPP.Plugin.Parse ~ LocaleHelper.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using FFXIVAPP.Plugin.Parse.Localization;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    internal static class LocaleHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="cultureInfo"> </param>
        public static Dictionary<string, string> Update(CultureInfo cultureInfo)
        {
            var culture = cultureInfo.TwoLetterISOLanguageName;
            ResourceDictionary dictionary;
            if (Constants.Supported.Contains(culture))
            {
                switch (culture)
                {
                    case "fr":
                        dictionary = French.Context();
                        break;
                    case "ja":
                        dictionary = Japanese.Context();
                        break;
                    case "de":
                        dictionary = German.Context();
                        break;
                    case "zh":
                        dictionary = Chinese.Context();
                        break;
                    case "ru":
                        dictionary = Russian.Context();
                        break;
                    default:
                        dictionary = English.Context();
                        break;
                }
            }
            else
            {
                dictionary = English.Context();
            }
            return dictionary.Cast<DictionaryEntry>()
                             .ToDictionary(item => (string) item.Key, item => (string) item.Value);
        }
    }
}
