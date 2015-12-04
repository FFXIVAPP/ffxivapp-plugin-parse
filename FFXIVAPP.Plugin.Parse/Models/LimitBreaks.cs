// FFXIVAPP.Plugin.Parse ~ LimitBreaks.cs
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace FFXIVAPP.Plugin.Parse.Models
{
    public static class LimitBreaks
    {
        private static List<string> _limitBreakSkills;

        private static List<string> LimitBreakSkills
        {
            get { return _limitBreakSkills ?? (_limitBreakSkills = GetLimitBreakList()); }
            set { _limitBreakSkills = value; }
        }

        public static bool IsLimit(string action)
        {
            return LimitBreakSkills.Any(lb => String.Equals(lb, action, Constants.InvariantComparer));
        }

        private static List<string> GetLimitBreakList()
        {
            var culture = Constants.CultureInfo.TwoLetterISOLanguageName;
            switch (culture)
            {
                case "ja":
                    return new List<string>
                    {
                        // limit break
                        "シールドウォール",
                        "マイティガード",
                        "ラストバスティオン",
                        "スカイシャード",
                        "プチメテオ",
                        "メテオ",
                        "癒しの風",
                        "大地の息吹",
                        "生命の鼓動",
                        "ブレイバー",
                        "ブレードダンス",
                        "ファイナルヘヴン"
                    };
                case "de":
                    return new List<string>
                    {
                        // limit break
                        "schutzschild",
                        "totalabwehr",
                        "letzte bastion",
                        "himmelsscherbe",
                        "sternensturm",
                        //"meteor",
                        "heilender wind",
                        "atem der erde",
                        "lebenspuls",
                        "mutangriff",
                        "schwertertanz",
                        "endgültiger himmel"
                    };
                case "fr":
                    return new List<string>
                    {
                        // limit break
                        "mur protecteur",
                        "garde puissante",
                        "dernier bastion",
                        "éclat de ciel",
                        "tempête d'étoiles",
                        "météore",
                        "vent curateur",
                        "souffle de la terre",
                        "pulsation vitale",
                        "ardeur courageuse",
                        "danse de la lame",
                        "paradis final"
                    };
            }
            return new List<string>
            {
                // limit break
                "shield wall",
                "might guard",
                "last bastion",
                "skyshard",
                "starstorm",
                "meteor",
                "healing wind",
                "breath of earth",
                "pulse of life",
                "braver",
                "bladedance",
                "final heaven"
            };
        }
    }
}
