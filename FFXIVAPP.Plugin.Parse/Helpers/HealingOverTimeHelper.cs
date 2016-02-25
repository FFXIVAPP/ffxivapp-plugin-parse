// FFXIVAPP.Plugin.Parse ~ HealingOverTimeHelper.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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
using FFXIVAPP.Plugin.Parse.Models;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class HealingOverTimeHelper
    {
        private static Dictionary<string, XOverTimeAction> _playerActions;
        private static Dictionary<string, XOverTimeAction> _monsterActions;
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

        public static Dictionary<string, XOverTimeAction> PlayerActions
        {
            get
            {
                if (_playerActions != null)
                {
                    return _playerActions;
                }
                _playerActions = new Dictionary<string, XOverTimeAction>();

                _playerActions.Add("medica ii", new XOverTimeAction
                {
                    ActionPotency = 200,
                    ActionOverTimePotency = 50,
                    Duration = 30,
                    HasNoInitialResult = false
                });
                _playerActions.Add("extra médica", _playerActions["medica ii"]);
                _playerActions.Add("resedra", _playerActions["medica ii"]);
                _playerActions.Add("メディカラ", _playerActions["medica ii"]);
                _playerActions.Add("医济", _playerActions["medica ii"]);

                _playerActions.Add("regen", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 150,
                    Duration = 21,
                    HasNoInitialResult = true
                });
                _playerActions.Add("récup", _playerActions["regen"]);
                _playerActions.Add("regena", _playerActions["regen"]);
                _playerActions.Add("リジェネ", _playerActions["regen"]);
                _playerActions.Add("再生", _playerActions["regen"]);

                _playerActions.Add("choco regen", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 25,
                    Duration = 18,
                    HasNoInitialResult = true
                });
                _playerActions.Add("choco-récup", _playerActions["choco regen"]);
                _playerActions.Add("chocobo-regena", _playerActions["choco regen"]);
                _playerActions.Add("チョコリジェネ", _playerActions["choco regen"]);
                _playerActions.Add("陆行鸟再生", _playerActions["choco regen"]);

                _playerActions.Add("whispering dawn", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 100,
                    Duration = 21,
                    HasNoInitialResult = true
                });
                _playerActions.Add("erhebendes flüstern", _playerActions["whispering dawn"]);
                _playerActions.Add("murmure de l'aurore", _playerActions["whispering dawn"]);
                _playerActions.Add("光の囁き", _playerActions["whispering dawn"]);
                _playerActions.Add("日光的低语", _playerActions["whispering dawn"]);

                _playerActions.Add("sacred prism", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 70,
                    Duration = 24,
                    HasNoInitialResult = false
                });
                _playerActions.Add("prisme sacré", _playerActions["sacred prism"]);
                _playerActions.Add("barmherzigkeit", _playerActions["sacred prism"]);
                _playerActions.Add("女神の慈悲", _playerActions["sacred prism"]);
                _playerActions.Add("女神的慈悲", _playerActions["sacred prism"]);

                return _playerActions;
            }
            set { _playerActions = value; }
        }

        public static Dictionary<string, XOverTimeAction> MonsterActions
        {
            get { return _monsterActions ?? (_monsterActions = new Dictionary<string, XOverTimeAction>()); }
            set { _monsterActions = value; }
        }
    }
}
