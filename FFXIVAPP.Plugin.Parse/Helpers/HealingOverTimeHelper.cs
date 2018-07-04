// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HealingOverTimeHelper.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   HealingOverTimeHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Helpers {
    using System.Collections.Generic;

    using Sharlayan.Models.XIVDatabase;
    using Sharlayan.Utilities;

    public static class HealingOverTimeHelper {
        private static Dictionary<string, List<string>> _cureActions;

        private static Dictionary<string, List<string>> _medicaActions;

        private static Dictionary<string, ActionItem> _monsterActions;

        private static Dictionary<string, ActionItem> _playerActions;

        public static Dictionary<string, List<string>> CureActions {
            get {
                if (_cureActions != null) {
                    return _cureActions;
                }

                _cureActions = new Dictionary<string, List<string>>();

                _cureActions.Add(
                    "I",
                    new List<string> {
                        "cure",
                        "vita",
                        "soin",
                        "ケアル",
                        "治疗"
                    });
                _cureActions.Add(
                    "II",
                    new List<string> {
                        "cure ii",
                        "vitra",
                        "extra soin",
                        "ケアルラ",
                        "救疗"
                    });
                _cureActions.Add(
                    "III",
                    new List<string> {
                        "cure iii",
                        "vitaga",
                        "méga soin",
                        "ケアルガ",
                        "愈疗"
                    });
                return _cureActions;
            }
        }

        public static Dictionary<string, List<string>> MedicaActions {
            get {
                if (_medicaActions != null) {
                    return _medicaActions;
                }

                _medicaActions = new Dictionary<string, List<string>>();

                _medicaActions.Add(
                    "I",
                    new List<string> {
                        "reseda",
                        "médica",
                        "メディカ",
                        "医治"
                    });
                _medicaActions.Add(
                    "II",
                    new List<string> {
                        "medica ii",
                        "resedra",
                        "extra médica",
                        "メディカラ",
                        "医济"
                    });
                return _medicaActions;
            }
        }

        public static Dictionary<string, ActionItem> MonsterActions {
            get {
                return _monsterActions ?? (_monsterActions = new Dictionary<string, ActionItem>());
            }

            set {
                _monsterActions = value;
            }
        }

        public static Dictionary<string, ActionItem> PlayerActions {
            get {
                if (_playerActions != null) {
                    return _playerActions;
                }

                _playerActions = new Dictionary<string, ActionItem>();

                foreach (ActionItem action in ActionLookup.HealingOverTimeActions()) {
                    if (!string.IsNullOrWhiteSpace(action.Name.English)) {
                        _playerActions.Add(action.Name.English.ToLowerInvariant(), action);
                    }

                    if (!string.IsNullOrWhiteSpace(action.Name.Chinese)) {
                        _playerActions.Add(action.Name.Chinese.ToLowerInvariant(), action);
                    }

                    if (!string.IsNullOrWhiteSpace(action.Name.French)) {
                        _playerActions.Add(action.Name.French.ToLowerInvariant(), action);
                    }

                    if (!string.IsNullOrWhiteSpace(action.Name.German)) {
                        _playerActions.Add(action.Name.German.ToLowerInvariant(), action);
                    }

                    if (!string.IsNullOrWhiteSpace(action.Name.Japanese)) {
                        _playerActions.Add(action.Name.Japanese.ToLowerInvariant(), action);
                    }

                    if (!string.IsNullOrWhiteSpace(action.Name.Korean)) {
                        _playerActions.Add(action.Name.Korean.ToLowerInvariant(), action);
                    }
                }

                return _playerActions;
            }

            set {
                _playerActions = value;
            }
        }
    }
}