﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LimitBreaks.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   LimitBreaks.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System.Collections.Generic;
    using System.Linq;

    public static class LimitBreaks {
        private static List<string> _limitBreakSkills;

        private static List<string> LimitBreakSkills {
            get {
                return _limitBreakSkills ?? (_limitBreakSkills = GetLimitBreakList());
            }

            set {
                _limitBreakSkills = value;
            }
        }

        public static bool IsLimit(string action) {
            return LimitBreakSkills.Any(lb => string.Equals(lb, action, Constants.InvariantComparer));
        }

        private static List<string> GetLimitBreakList() {
            var culture = Constants.CultureInfo.TwoLetterISOLanguageName;
            switch (culture) {
                case "ja":
                    return new List<string> {
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
                        "ファイナルヘヴン",
                    };
                case "de":
                    return new List<string> {
                        // limit break
                        "schutzschild",
                        "totalabwehr",
                        "letzte bastion",
                        "himmelsscherbe",
                        "sternensturm",

                        // "meteor",
                        "heilender wind",
                        "atem der erde",
                        "lebenspuls",
                        "mutangriff",
                        "schwertertanz",
                        "endgültiger himmel",
                    };
                case "fr":
                    return new List<string> {
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
                        "paradis final",
                    };
            }

            return new List<string> {
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
                "final heaven",
            };
        }
    }
}