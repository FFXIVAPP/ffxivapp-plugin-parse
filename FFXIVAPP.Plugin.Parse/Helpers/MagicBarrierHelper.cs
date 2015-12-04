// FFXIVAPP.Plugin.Parse ~ MagicBarrierHelper.cs
// 
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

using System.Collections.Generic;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class MagicBarrierHelper
    {
        private static List<string> _stoneSkin;
        private static List<string> _succor;
        private static List<string> _adloquium;

        public static List<string> StoneSkin
        {
            get
            {
                if (_stoneSkin != null)
                {
                    return _stoneSkin;
                }
                _stoneSkin = new List<string>
                {
                    "stoneskin",
                    "steinhaut",
                    "cuirasse",
                    "ストンスキン"
                };
                return _stoneSkin;
            }
        }

        public static List<string> Succor
        {
            get
            {
                if (_succor != null)
                {
                    return _succor;
                }
                _succor = new List<string>
                {
                    "succor",
                    "kurieren",
                    "traité du soulagement",
                    "士気高揚の策"
                };
                return _succor;
            }
        }

        public static List<string> Adloquium
        {
            get
            {
                if (_adloquium != null)
                {
                    return _adloquium;
                }
                _adloquium = new List<string>
                {
                    "adloquium",
                    "traité du réconfort",
                    "鼓舞激励の策"
                };
                return _adloquium;
            }
        }
    }
}
