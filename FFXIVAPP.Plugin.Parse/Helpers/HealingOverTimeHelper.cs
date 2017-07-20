// FFXIVAPP.Plugin.Parse ~ HealingOverTimeHelper.cs
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
    public static class HealingOverTimeHelper
    {
        private static Dictionary<string, List<string>> _cureActions;
        private static Dictionary<string, List<string>> _medicaActions;

        public static Dictionary<string, List<string>> CureActions
        {
            get
            {
                if (_cureActions != null)
                {
                    return _cureActions;
                }
                _cureActions = new Dictionary<string, List<string>>();

                _cureActions.Add("I", new List<string>
                {
                    "cure",
                    "vita",
                    "soin",
                    "ケアル",
                    "治疗"
                });
                _cureActions.Add("II", new List<string>
                {
                    "cure ii",
                    "vitra",
                    "extra soin",
                    "ケアルラ",
                    "救疗"
                });
                _cureActions.Add("III", new List<string>
                {
                    "cure iii",
                    "vitaga",
                    "méga soin",
                    "ケアルガ",
                    "愈疗"
                });
                return _cureActions;
            }
        }

        public static Dictionary<string, List<string>> MedicaActions
        {
            get
            {
                if (_medicaActions != null)
                {
                    return _medicaActions;
                }
                _medicaActions = new Dictionary<string, List<string>>();

                _medicaActions.Add("I", new List<string>
                {
                    "reseda",
                    "médica",
                    "メディカ",
                    "医治"
                });
                _medicaActions.Add("II", new List<string>
                {
                    "medica ii",
                    "resedra",
                    "extra médica",
                    "メディカラ",
                    "医济"
                });
                return _medicaActions;
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

                foreach (var action in ActionHelper.HealingOverTimeActions())
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
