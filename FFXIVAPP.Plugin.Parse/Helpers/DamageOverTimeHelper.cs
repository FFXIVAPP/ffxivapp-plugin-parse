// FFXIVAPP.Plugin.Parse
// DamageOverTimeHelper.cs
// 
// Created by Ryan Wilson.
// 
// Copyright © 2014 - 2014 Ryan Wilson - All Rights Reserved

using System.Collections.Generic;
using System.Windows;
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
                    "サンダー"
                });
                _thunderActions.Add("II", new List<string>
                {
                    "thunder ii",
                    "blitzra",
                    "extra foudre",
                    "サンダラ"
                });
                _thunderActions.Add("III", new List<string>
                {
                    "thunder iii",
                    "blitzga",
                    "méga foudre",
                    "サンダガ"
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
                    "バイオ"
                });
                _bioActions.Add("II", new List<string>
                {
                    "bio ii",
                    "biora",
                    "extra bactérie",
                    "バイオラ"
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
                    "ルイン"
                });
                _ruinActions.Add("II", new List<string>
                {
                    "ruin ii",
                    "ruinra",
                    "extra ruine",
                    "ルインラ"
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

                _playerActions.Add("storm's path", new XOverTimeAction
                {
                    ActionPotency = 100,
                    ActionOverTimePotency = 250,
                    Duration = 20,
                    HasNoInitialResult = false
                });
                _playerActions.Add("couperet de justice", _playerActions["storm's path"]);
                _playerActions.Add("sturmkeil", _playerActions["storm's path"]);
                _playerActions.Add("シュトルムヴィント", _playerActions["storm's path"]);

                _playerActions.Add("storm's eye", new XOverTimeAction
                {
                    ActionPotency = 100,
                    ActionOverTimePotency = 270,
                    Duration = 20,
                    HasNoInitialResult = false
                });
                _playerActions.Add("œil de la tempête", _playerActions["storm's eye"]);
                _playerActions.Add("sturmbrecher", _playerActions["storm's eye"]);
                _playerActions.Add("シュトルムブレハ", _playerActions["storm's eye"]);

                _playerActions.Add("demolish", new XOverTimeAction
                {
                    ActionPotency = 30,
                    ActionOverTimePotency = 40,
                    Duration = 18,
                    HasNoInitialResult = false
                });
                _playerActions.Add("démolition", _playerActions["demolish"]);
                _playerActions.Add("demolieren", _playerActions["demolish"]);
                _playerActions.Add("破砕拳", _playerActions["demolish"]);

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

                _playerActions.Add("phlebotomize", new XOverTimeAction
                {
                    ActionPotency = 170,
                    ActionOverTimePotency = 25,
                    Duration = 18,
                    HasNoInitialResult = false
                });
                _playerActions.Add("double percée", _playerActions["phlebotomize"]);
                _playerActions.Add("phlebotomie", _playerActions["phlebotomize"]);
                _playerActions.Add("二段突き", _playerActions["phlebotomize"]);

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

                _playerActions.Add("aero ii", new XOverTimeAction
                {
                    ActionPotency = 50,
                    ActionOverTimePotency = 40,
                    Duration = 12,
                    HasNoInitialResult = false
                });
                _playerActions.Add("extra vent", _playerActions["aero ii"]);
                _playerActions.Add("windra", _playerActions["aero ii"]);
                _playerActions.Add("エアロラ", _playerActions["aero ii"]);

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

                _playerActions.Add("bio", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 40,
                    Duration = 18,
                    HasNoInitialResult = true
                });
                _playerActions.Add("bactérie", _playerActions["bio"]);
                _playerActions.Add("バイオ", _playerActions["bio"]);

                _playerActions.Add("miasma", new XOverTimeAction
                {
                    ActionPotency = 20,
                    ActionOverTimePotency = 35,
                    Duration = 24,
                    HasNoInitialResult = false
                });
                _playerActions.Add("miasmes", _playerActions["miasma"]);
                _playerActions.Add("ミアズマ", _playerActions["miasma"]);

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

                _playerActions.Add("inferno", new XOverTimeAction
                {
                    ActionPotency = 200,
                    ActionOverTimePotency = 20,
                    Duration = 15,
                    HasNoInitialResult = false
                });
                _playerActions.Add("flammes de l'enfer", _playerActions["inferno"]);
                _playerActions.Add("地獄の火炎", _playerActions["inferno"]);

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
