// FFXIVAPP.Plugin.Parse ~ DamageOverTimeHelper.cs
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

using System.Collections.Generic;
using Sharlayan.Helpers;
using Sharlayan.Models;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class DamageOverTimeHelper
    {
        private static Dictionary<string, List<string>> _thunderActions;
        private static Dictionary<string, List<string>> _bioActions;
        private static Dictionary<string, List<string>> _ruinActions;

        public static Dictionary<string, List<string>> ThunderActions
        {
            get
            {
                if (_thunderActions != null)
                {
                    return _thunderActions;
                }
                _thunderActions = new Dictionary<string, List<string>>();

                _thunderActions.Add("I", new List<string>
                {
                    "thunder",
                    "blitz",
                    "foudre",
                    "サンダー",
                    "闪雷"
                });
                _thunderActions.Add("II", new List<string>
                {
                    "thunder ii",
                    "blitzra",
                    "extra foudre",
                    "サンダラ",
                    "震雷"
                });
                _thunderActions.Add("III", new List<string>
                {
                    "thunder iii",
                    "blitzga",
                    "méga foudre",
                    "サンダガ",
                    "暴雷"
                });
                return _thunderActions;
            }
        }

        public static Dictionary<string, List<string>> BioActions
        {
            get
            {
                if (_bioActions != null)
                {
                    return _bioActions;
                }
                _bioActions = new Dictionary<string, List<string>>();

                _bioActions.Add("I", new List<string>
                {
                    "bio",
                    "bactérie",
                    "バイオ",
                    "毒菌"
                });
                _bioActions.Add("II", new List<string>
                {
                    "bio ii",
                    "biora",
                    "extra bactérie",
                    "バイオラ",
                    "猛毒菌"
                });
                return _bioActions;
            }
        }

        public static Dictionary<string, List<string>> RuinActions
        {
            get
            {
                if (_ruinActions != null)
                {
                    return _ruinActions;
                }
                _ruinActions = new Dictionary<string, List<string>>();

                _ruinActions.Add("I", new List<string>
                {
                    "ruin",
                    "ruine",
                    "ルイン",
                    "毁灭"
                });
                _ruinActions.Add("II", new List<string>
                {
                    "ruin ii",
                    "ruinra",
                    "extra ruine",
                    "ルインラ",
                    "毁坏"
                });
                return _ruinActions;
            }
        }

        #region Player Actions

        private static Dictionary<string, ActionItem> _playerActions;

        public static Dictionary<string, ActionItem> PlayerActions
        {
            get
            {
                if (_playerActions != null)
                {
                    return _playerActions;
                }
                _playerActions = new Dictionary<string, ActionItem>();

                foreach (var action in ActionHelper.DamageOverTimeActions())
                {
                    if (!string.IsNullOrWhiteSpace(action.Name.English))
                    {
                        _playerActions.Add(action.Name.English.ToLowerInvariant(), action);
                    }
                    if (!string.IsNullOrWhiteSpace(action.Name.Chinese))
                    {
                        _playerActions.Add(action.Name.Chinese.ToLowerInvariant(), action);
                    }
                    if (!string.IsNullOrWhiteSpace(action.Name.French))
                    {
                        _playerActions.Add(action.Name.French.ToLowerInvariant(), action);
                    }
                    if (!string.IsNullOrWhiteSpace(action.Name.German))
                    {
                        _playerActions.Add(action.Name.German.ToLowerInvariant(), action);
                    }
                    if (!string.IsNullOrWhiteSpace(action.Name.Japanese))
                    {
                        _playerActions.Add(action.Name.Japanese.ToLowerInvariant(), action);
                    }
                    if (!string.IsNullOrWhiteSpace(action.Name.Korean))
                    {
                        _playerActions.Add(action.Name.Korean.ToLowerInvariant(), action);
                    }
                }

                return _playerActions;
            }
            set { _playerActions = value; }
        }

        #endregion

        #region Monster Actions

        private static Dictionary<string, ActionItem> _monsterActions;

        public static Dictionary<string, ActionItem> MonsterActions
        {
            get { return _monsterActions ?? (_monsterActions = new Dictionary<string, ActionItem>()); }
            set { _monsterActions = value; }
        }

        #endregion
    }
}
