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
using FFXIVAPP.Plugin.Parse.Models;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class DamageOverTimeHelper
    {
        private static Dictionary<string, XOverTimeAction> _playerActions;
        private static Dictionary<string, XOverTimeAction> _monsterActions;
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

        public static Dictionary<string, XOverTimeAction> PlayerActions
        {
            get
            {
                if (_playerActions != null)
                {
                    return _playerActions;
                }
                _playerActions = new Dictionary<string, XOverTimeAction>();


                _playerActions.Add("circle of scorn", new XOverTimeAction
                {
                    ActionPotency = 100,
                    ActionOverTimePotency = 30,
                    Duration = 15,
                    HasNoInitialResult = false
                });
                _playerActions.Add("cercle du destin", _playerActions["circle of scorn"]);
                _playerActions.Add("kreis der verachtung", _playerActions["circle of scorn"]);
                _playerActions.Add("サークル・オブ・ドゥーム", _playerActions["circle of scorn"]);
                _playerActions.Add("厄运流转", _playerActions["circle of scorn"]);

                _playerActions.Add("demolish", new XOverTimeAction
                {
                    ActionPotency = 30,
                    ActionOverTimePotency = 40,
                    Duration = 21,
                    HasNoInitialResult = false
                });
                _playerActions.Add("démolition", _playerActions["demolish"]);
                _playerActions.Add("demolieren", _playerActions["demolish"]);
                _playerActions.Add("破砕拳", _playerActions["demolish"]);
                _playerActions.Add("破碎拳", _playerActions["demolish"]);

                _playerActions.Add("touch of death", new XOverTimeAction
                {
                    ActionPotency = 20,
                    ActionOverTimePotency = 25,
                    Duration = 30,
                    HasNoInitialResult = false
                });
                _playerActions.Add("toucher mortel", _playerActions["touch of death"]);
                _playerActions.Add("hauch des todes", _playerActions["touch of death"]);
                _playerActions.Add("秘孔拳", _playerActions["touch of death"]);

                _playerActions.Add("chaos thrust", new XOverTimeAction
                {
                    ActionPotency = 100,
                    ActionOverTimePotency = 30,
                    Duration = 30,
                    HasNoInitialResult = false
                });
                _playerActions.Add("percée chaotique", _playerActions["chaos thrust"]);
                _playerActions.Add("chaotischer tjost", _playerActions["chaos thrust"]);
                _playerActions.Add("桜華狂咲", _playerActions["chaos thrust"]);
                _playerActions.Add("樱花怒放", _playerActions["chaos thrust"]);

                _playerActions.Add("phlebotomize", new XOverTimeAction
                {
                    ActionPotency = 170,
                    ActionOverTimePotency = 30,
                    Duration = 24,
                    HasNoInitialResult = false
                });
                _playerActions.Add("double percée", _playerActions["phlebotomize"]);
                _playerActions.Add("phlebotomie", _playerActions["phlebotomize"]);
                _playerActions.Add("二段突き", _playerActions["phlebotomize"]);
                _playerActions.Add("二段突击", _playerActions["phlebotomize"]);

                _playerActions.Add("venomous bite", new XOverTimeAction
                {
                    ActionPotency = 100,
                    ActionOverTimePotency = 35,
                    Duration = 18,
                    HasNoInitialResult = false
                });
                _playerActions.Add("giftbiss", _playerActions["venomous bite"]);
                _playerActions.Add("morsure venimeuse", _playerActions["venomous bite"]);
                _playerActions.Add("ベノムバイト", _playerActions["venomous bite"]);
                _playerActions.Add("毒咬箭", _playerActions["venomous bite"]);

                _playerActions.Add("windbite", new XOverTimeAction
                {
                    ActionPotency = 60,
                    ActionOverTimePotency = 45,
                    Duration = 18,
                    HasNoInitialResult = false
                });
                _playerActions.Add("morsure du vent", _playerActions["windbite"]);
                _playerActions.Add("beißender wind", _playerActions["windbite"]);
                _playerActions.Add("ウィンドバイト", _playerActions["windbite"]);
                _playerActions.Add("风蚀箭", _playerActions["windbite"]);

                _playerActions.Add("aero", new XOverTimeAction
                {
                    ActionPotency = 50,
                    ActionOverTimePotency = 25,
                    Duration = 18,
                    HasNoInitialResult = false
                });
                _playerActions.Add("vent", _playerActions["aero"]);
                _playerActions.Add("wind", _playerActions["aero"]);
                _playerActions.Add("エアロ", _playerActions["aero"]);
                _playerActions.Add("疾风", _playerActions["aero"]);

                _playerActions.Add("aero ii", new XOverTimeAction
                {
                    ActionPotency = 50,
                    ActionOverTimePotency = 50,
                    Duration = 12,
                    HasNoInitialResult = false
                });
                _playerActions.Add("extra vent", _playerActions["aero ii"]);
                _playerActions.Add("windra", _playerActions["aero ii"]);
                _playerActions.Add("エアロラ", _playerActions["aero ii"]);
                _playerActions.Add("烈风", _playerActions["aero ii"]);

                _playerActions.Add("thunder", new XOverTimeAction
                {
                    ActionPotency = 30,
                    ActionOverTimePotency = 35,
                    Duration = 18,
                    HasNoInitialResult = false
                });
                _playerActions.Add("foudre", _playerActions["thunder"]);
                _playerActions.Add("blitz", _playerActions["thunder"]);
                _playerActions.Add("サンダー", _playerActions["thunder"]);
                _playerActions.Add("闪雷", _playerActions["thunder"]);

                _playerActions.Add("thunder ii", new XOverTimeAction
                {
                    ActionPotency = 50,
                    ActionOverTimePotency = 35,
                    Duration = 21,
                    HasNoInitialResult = false
                });
                _playerActions.Add("extra foudre", _playerActions["thunder ii"]);
                _playerActions.Add("blitzra", _playerActions["thunder ii"]);
                _playerActions.Add("サンダラ", _playerActions["thunder ii"]);
                _playerActions.Add("震雷", _playerActions["thunder ii"]);

                _playerActions.Add("thunder iii", new XOverTimeAction
                {
                    ActionPotency = 60,
                    ActionOverTimePotency = 35,
                    Duration = 24,
                    HasNoInitialResult = false
                });
                _playerActions.Add("méga foudre", _playerActions["thunder iii"]);
                _playerActions.Add("blitzga", _playerActions["thunder iii"]);
                _playerActions.Add("サンダガ", _playerActions["thunder iii"]);
                _playerActions.Add("暴雷", _playerActions["thunder iii"]);

                _playerActions.Add("bio", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 40,
                    Duration = 18,
                    HasNoInitialResult = true
                });
                _playerActions.Add("bactérie", _playerActions["bio"]);
                _playerActions.Add("バイオ", _playerActions["bio"]);
                _playerActions.Add("毒菌", _playerActions["bio"]);

                _playerActions.Add("miasma", new XOverTimeAction
                {
                    ActionPotency = 20,
                    ActionOverTimePotency = 35,
                    Duration = 24,
                    HasNoInitialResult = false
                });
                _playerActions.Add("miasmes", _playerActions["miasma"]);
                _playerActions.Add("ミアズマ", _playerActions["miasma"]);
                _playerActions.Add("瘴气", _playerActions["miasma"]);

                _playerActions.Add("miasma ii", new XOverTimeAction
                {
                    ActionPotency = 20,
                    ActionOverTimePotency = 10,
                    Duration = 15,
                    HasNoInitialResult = false
                });
                _playerActions.Add("extra miasmes", _playerActions["miasma ii"]);
                _playerActions.Add("miasra", _playerActions["miasma ii"]);
                _playerActions.Add("ミアズラ", _playerActions["miasma ii"]);
                _playerActions.Add("瘴疠", _playerActions["miasma ii"]);

                _playerActions.Add("bio ii", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 35,
                    Duration = 30,
                    HasNoInitialResult = true
                });
                _playerActions.Add("extra bactérie", _playerActions["bio ii"]);
                _playerActions.Add("biora", _playerActions["bio ii"]);
                _playerActions.Add("バイオラ", _playerActions["bio ii"]);
                _playerActions.Add("猛毒菌", _playerActions["bio ii"]);

                _playerActions.Add("inferno", new XOverTimeAction
                {
                    ActionPotency = 200,
                    ActionOverTimePotency = 20,
                    Duration = 15,
                    HasNoInitialResult = false
                });
                _playerActions.Add("flammes de l'enfer", _playerActions["inferno"]);
                _playerActions.Add("地獄の火炎", _playerActions["inferno"]);

                _playerActions.Add("mutilate", new XOverTimeAction
                {
                    ActionPotency = 60,
                    ActionOverTimePotency = 30,
                    Duration = 30,
                    HasNoInitialResult = false
                });
                _playerActions.Add("attaque mutilante", _playerActions["mutilate"]);
                _playerActions.Add("verstümmeln", _playerActions["mutilate"]);
                _playerActions.Add("無双旋", _playerActions["mutilate"]);

                _playerActions.Add("full swing", new XOverTimeAction
                {
                    ActionPotency = 150,
                    ActionOverTimePotency = 30,
                    Duration = 15,
                    HasNoInitialResult = false
                });
                _playerActions.Add("plein élan", _playerActions["full swing"]);
                _playerActions.Add("voller schwinger", _playerActions["full swing"]);
                _playerActions.Add("フルスイング", _playerActions["full swing"]);

                _playerActions.Add("fracture", new XOverTimeAction
                {
                    ActionPotency = 100,
                    ActionOverTimePotency = 20,
                    Duration = 30,
                    HasNoInitialResult = false
                });
                _playerActions.Add("knochenbrecher", _playerActions["fracture"]);
                _playerActions.Add("フラクチャー", _playerActions["fracture"]);
                _playerActions.Add("碎骨打", _playerActions["fracture"]);

                _playerActions.Add("shadow fang", new XOverTimeAction
                {
                    ActionPotency = 100,
                    ActionOverTimePotency = 40,
                    Duration = 18,
                    HasNoInitialResult = false
                });
                _playerActions.Add("croc d'ombre", _playerActions["shadow fang"]);
                _playerActions.Add("schattenfang", _playerActions["shadow fang"]);
                _playerActions.Add("影牙", _playerActions["shadow fang"]);

                _playerActions.Add("goring blade", new XOverTimeAction
                {
                    ActionPotency = 100,
                    ActionOverTimePotency = 220,
                    Duration = 24,
                    HasNoInitialResult = false
                });
                _playerActions.Add("lame étripante", _playerActions["goring blade"]);
                _playerActions.Add("ausweiden", _playerActions["goring blade"]);
                _playerActions.Add("ゴアブレード", _playerActions["goring blade"]);

                _playerActions.Add("aero iii", new XOverTimeAction
                {
                    ActionPotency = 50,
                    ActionOverTimePotency = 40,
                    Duration = 24,
                    HasNoInitialResult = false
                });
                _playerActions.Add("méga vent", _playerActions["aero iii"]);
                _playerActions.Add("windga", _playerActions["aero iii"]);
                _playerActions.Add("エアロガ", _playerActions["aero iii"]);

                _playerActions.Add("combust", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 40,
                    Duration = 18,
                    HasNoInitialResult = false
                });
                _playerActions.Add("conjonction supérieure", _playerActions["combust"]);
                _playerActions.Add("konjunktion", _playerActions["combust"]);
                _playerActions.Add("コンバス", _playerActions["combust"]);

                _playerActions.Add("combust ii", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 45,
                    Duration = 30,
                    HasNoInitialResult = false
                });
                _playerActions.Add("conjonction supérieure ii", _playerActions["combust ii"]);
                _playerActions.Add("konjunktion ii", _playerActions["combust ii"]);
                _playerActions.Add("コンバラ", _playerActions["combust ii"]);

                _playerActions.Add("choco beak", new XOverTimeAction
                {
                    ActionPotency = 130,
                    ActionOverTimePotency = 20,
                    Duration = 18,
                    HasNoInitialResult = false
                });
                _playerActions.Add("choco-bec", _playerActions["choco beak"]);
                _playerActions.Add("chocobo-schnabel", _playerActions["choco beak"]);
                _playerActions.Add("チョコビーク", _playerActions["choco beak"]);
                _playerActions.Add("陆行鸟猛啄", _playerActions["choco beak"]);

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
