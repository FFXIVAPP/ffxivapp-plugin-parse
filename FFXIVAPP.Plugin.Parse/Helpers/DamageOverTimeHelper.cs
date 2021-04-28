// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DamageOverTimeHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   DamageOverTimeHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Helpers {
    using System.Collections.Generic;

    using Sharlayan.Models.XIVDatabase;
    using Sharlayan.Utilities;

    public static class DamageOverTimeHelper {
        private static Dictionary<string, List<string>> _bioActions;

        private static Dictionary<string, ActionItem> _monsterActions;

        private static Dictionary<string, ActionItem> _playerActions;

        private static Dictionary<string, List<string>> _ruinActions;

        private static Dictionary<string, List<string>> _thunderActions;

        public static Dictionary<string, List<string>> BioActions {
            get {
                if (_bioActions != null) {
                    return _bioActions;
                }

                _bioActions = new Dictionary<string, List<string>>();

                _bioActions.Add(
                    "I", new List<string> {
                        "bio",
                        "bactérie",
                        "バイオ",
                        "毒菌",
                    });
                _bioActions.Add(
                    "II", new List<string> {
                        "bio ii",
                        "biora",
                        "extra bactérie",
                        "バイオラ",
                        "猛毒菌",
                    });
                return _bioActions;
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

                foreach (ActionItem action in ActionLookup.DamageOverTimeActions()) {
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

        public static Dictionary<string, List<string>> RuinActions {
            get {
                if (_ruinActions != null) {
                    return _ruinActions;
                }

                _ruinActions = new Dictionary<string, List<string>>();

                _ruinActions.Add(
                    "I", new List<string> {
                        "ruin",
                        "ruine",
                        "ルイン",
                        "毁灭",
                    });
                _ruinActions.Add(
                    "II", new List<string> {
                        "ruin ii",
                        "ruinra",
                        "extra ruine",
                        "ルインラ",
                        "毁坏",
                    });
                return _ruinActions;
            }
        }

        public static Dictionary<string, List<string>> ThunderActions {
            get {
                if (_thunderActions != null) {
                    return _thunderActions;
                }

                _thunderActions = new Dictionary<string, List<string>>();

                _thunderActions.Add(
                    "I", new List<string> {
                        "thunder",
                        "blitz",
                        "foudre",
                        "サンダー",
                        "闪雷",
                    });
                _thunderActions.Add(
                    "II", new List<string> {
                        "thunder ii",
                        "blitzra",
                        "extra foudre",
                        "サンダラ",
                        "震雷",
                    });
                _thunderActions.Add(
                    "III", new List<string> {
                        "thunder iii",
                        "blitzga",
                        "méga foudre",
                        "サンダガ",
                        "暴雷",
                    });
                return _thunderActions;
            }
        }
    }
}