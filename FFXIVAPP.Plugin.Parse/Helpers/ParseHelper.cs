// FFXIVAPP.Plugin.Parse
// ParseHelper.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Models;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class ParseHelper
    {
        // setup pet info that comes through as "YOU"
        private static List<string> _pets = new List<string>
        {
            "eos",
            "selene",
            "topaz carbuncle",
            "emerald carbuncle",
            "ifrit-egi",
            "titan-egi",
            "garuda-egi",
            "amber carbuncle",
            "carbuncle topaze",
            "carbuncle émeraude",
            "carbuncle ambre",
            "topas-karfunkel",
            "smaragd-karfunkel",
            "bernstein-karfunkel",
            "フェアリー・エオス",
            "フェアリー・セレネ",
            "カーバンクル・トパーズ",
            "カーバンクル・エメラルド",
            "イフリート・エギ",
            "タイタン・エギ",
            "ガルーダ・エギ",
            "カーバンクル・アンバー"
        };

        private static List<string> _healingActions;

        public static List<string> HealingActions
        {
            get
            {
                if (_healingActions == null)
                {
                    _healingActions = new List<string>
                    {
                        "内丹",
                        "Second Wind",
                        "Second souffle",
                        "Chakra",
                        "ケアル",
                        "Cure",
                        "Soin",
                        "Vita",
                        "メディカ",
                        "Medica",
                        "Médica",
                        "Reseda",
                        "ケアルガ",
                        "Cure III",
                        "Méga Soin",
                        "Vitaga",
                        "メディカラ",
                        "Medica II",
                        "Extra Médica",
                        "Resedra",
                        "ケアルラ",
                        "Cure II",
                        "Extra Soin",
                        "Vitra",
                        "鼓舞激励の策",
                        "Adloquium",
                        "Traité du réconfort",
                        "Adloquium",
                        "士気高揚の策",
                        "Succor",
                        "Traité du soulagement",
                        "Kurieren",
                        "フィジク",
                        "Physick",
                        "Médecine",
                        "Physick",
                        "光の癒し",
                        "Embrace",
                        "Embrassement",
                        "Sanfte Umarmung",
                        "光の囁き",
                        "Whispering Dawn",
                        "Murmure de l'aurore",
                        "Erhebendes Flüstern",
                        "光の癒し",
                        "Embrace",
                        "Embrassement",
                        "Sanfte Umarmung",
                        "生命活性法",
                        "Lustrate",
                        "Loi de revivification",
                        "Revitalisierung",
                        "チョコメディカ",
                        "Choco Medica",
                        "Choco-médica",
                        "Chocobo-Reseda",
                        "チョコケアル",
                        "Choco Cure",
                        "Choco-soin",
                        "Chocobo-Vita"
                    };
                }
                return _healingActions;
            }
            set { _healingActions = value; }
        }

        /// <summary>
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public static double GetBonusAmount(double amount, double modifier)
        {
            return Math.Abs((amount / (modifier + 1)) - amount);
        }

        /// <summary>
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public static double GetOriginalAmount(double amount, double modifier)
        {
            return Math.Abs(amount - GetBonusAmount(amount, modifier));
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="exp"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTaggedName(string name, Expressions exp, TimelineType type)
        {
            if (type == TimelineType.Unknown)
            {
                return name;
            }
            var tag = "???";
            name = name.Trim();
            if (String.IsNullOrWhiteSpace(name))
            {
                return "";
            }
            name = Regex.Replace(name, @"\[[\w]+\]", "")
                        .Trim();
            var petFound = false;
            foreach (var pet in _pets.Where(pet => String.Equals(pet, name, Constants.InvariantComparer)))
            {
                petFound = true;
            }
            switch (type)
            {
                case TimelineType.You:
                    tag = exp.YouString;
                    break;
                case TimelineType.Party:
                    tag = "P";
                    break;
                case TimelineType.Alliance:
                    tag = "A";
                    break;
                case TimelineType.Other:
                    tag = "O";
                    break;
            }
            return String.Format("[{0}] {1}", tag, name);
        }

        #region LastAction Helper Dictionaries

        public static class LastAmountByAction
        {
            private static Dictionary<string, List<Tuple<string, double>>> Monster = new Dictionary<string, List<Tuple<string, double>>>();
            private static Dictionary<string, List<Tuple<string, double>>> Player = new Dictionary<string, List<Tuple<string, double>>>();

            public static Dictionary<string, double> GetMonster(string name)
            {
                var results = new Dictionary<string, double>();
                EnsureMonster(name);
                lock (Monster)
                {
                    var actionList = new List<string>();
                    var actionUpdateCount = new Dictionary<string, int>();
                    foreach (var actionTuple in Monster[name])
                    {
                        if (!results.ContainsKey(actionTuple.Item1))
                        {
                            results.Add(actionTuple.Item1, 0);
                        }
                        //results[actionTuple.Item1] += actionTuple.Item2;
                        results[actionTuple.Item1] = actionTuple.Item2;
                        if (!actionList.Contains(actionTuple.Item1))
                        {
                            actionList.Add(actionTuple.Item1);
                        }
                        if (!actionUpdateCount.ContainsKey(actionTuple.Item1))
                        {
                            actionUpdateCount.Add(actionTuple.Item1, 0);
                        }
                        actionUpdateCount[actionTuple.Item1]++;
                    }
                    foreach (var action in actionList)
                    {
                        //results[action] = results[action] / actionUpdateCount[action];
                    }
                }
                return results;
            }

            private static void EnsureMonster(string name)
            {
                lock (Monster)
                {
                    if (!Monster.ContainsKey(name))
                    {
                        Monster.Add(name, new List<Tuple<string, double>>());
                    }
                }
            }

            public static void EnsureMonsterAction(string name, string action, double amount)
            {
                EnsureMonster(name);
                lock (Monster)
                {
                    Monster[name].Add(new Tuple<string, double>(action, amount));
                }
            }

            public static Dictionary<string, double> GetPlayer(string name)
            {
                var results = new Dictionary<string, double>();
                EnsurePlayer(name);
                lock (Player)
                {
                    var actionList = new List<string>();
                    var actionUpdateCount = new Dictionary<string, int>();
                    foreach (var actionTuple in Player[name])
                    {
                        if (!results.ContainsKey(actionTuple.Item1))
                        {
                            results.Add(actionTuple.Item1, 0);
                        }
                        //results[actionTuple.Item1] += actionTuple.Item2;
                        results[actionTuple.Item1] = actionTuple.Item2;
                        if (!actionList.Contains(actionTuple.Item1))
                        {
                            actionList.Add(actionTuple.Item1);
                        }
                        if (!actionUpdateCount.ContainsKey(actionTuple.Item1))
                        {
                            actionUpdateCount.Add(actionTuple.Item1, 0);
                        }
                        actionUpdateCount[actionTuple.Item1]++;
                    }
                    foreach (var action in actionList)
                    {
                        //results[action] = results[action] / actionUpdateCount[action];
                    }
                }
                return results;
            }

            private static void EnsurePlayer(string name)
            {
                lock (Player)
                {
                    if (!Player.ContainsKey(name))
                    {
                        Player.Add(name, new List<Tuple<string, double>>());
                    }
                }
            }

            public static void EnsurePlayerAction(string name, string action, double amount)
            {
                EnsurePlayer(name);
                lock (Player)
                {
                    Player[name].Add(new Tuple<string, double>(action, amount));
                }
            }
        }

        #endregion
    }
}
