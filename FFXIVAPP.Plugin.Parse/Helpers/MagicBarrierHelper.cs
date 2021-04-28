// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagicBarrierHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MagicBarrierHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Helpers {
    using System.Collections.Generic;

    public static class MagicBarrierHelper {
        private static List<string> _adloquium;

        private static List<string> _stoneSkin;

        private static List<string> _succor;

        public static List<string> Adloquium {
            get {
                if (_adloquium != null) {
                    return _adloquium;
                }

                _adloquium = new List<string> {
                    "adloquium",
                    "traité du réconfort",
                    "鼓舞激励の策",
                };
                return _adloquium;
            }
        }

        public static List<string> StoneSkin {
            get {
                if (_stoneSkin != null) {
                    return _stoneSkin;
                }

                _stoneSkin = new List<string> {
                    "stoneskin",
                    "steinhaut",
                    "cuirasse",
                    "ストンスキン",
                };
                return _stoneSkin;
            }
        }

        public static List<string> Succor {
            get {
                if (_succor != null) {
                    return _succor;
                }

                _succor = new List<string> {
                    "succor",
                    "kurieren",
                    "traité du soulagement",
                    "士気高揚の策",
                };
                return _succor;
            }
        }
    }
}