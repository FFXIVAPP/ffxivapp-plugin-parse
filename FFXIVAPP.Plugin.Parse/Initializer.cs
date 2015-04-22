﻿// FFXIVAPP.Plugin.Parse
// Initializer.cs
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
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Properties;
using FFXIVAPP.Plugin.Parse.RegularExpressions;

namespace FFXIVAPP.Plugin.Parse
{
    internal static class Initializer
    {
        #region Declarations

        #endregion

        /// <summary>
        /// </summary>
        public static void LoadSettings()
        {
            if (Constants.XSettings != null)
            {
                foreach (var xElement in Constants.XSettings.Descendants()
                                                  .Elements("Setting"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element("Value");
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                    {
                        return;
                    }
                    Settings.SetValue(xKey, xValue, CultureInfo.InvariantCulture);
                    if (!Constants.Settings.Contains(xKey))
                    {
                        Constants.Settings.Add(xKey);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadPlayerRegEx()
        {
            if (Constants.XRegEx == null)
            {
                return;
            }
            foreach (var xElement in Constants.XRegEx.Descendants()
                                              .Elements("Player"))
            {
                var xKey = (string) xElement.Attribute("Key");
                var xLanguage = (string) xElement.Attribute("Language");
                var xValue = (string) xElement.Element("Value");
                if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                {
                    continue;
                }
                if (!SharedRegEx.IsValidRegex(xValue))
                {
                    continue;
                }
                var regex = new Regex(xValue, SharedRegEx.DefaultOptions);
                switch (xLanguage)
                {
                    case "EN":

                        #region Handle English Regular Expressions

                        switch (xKey)
                        {
                            case "Damage":
                                PlayerRegEx.DamageEn = regex;
                                break;
                            case "DamageAuto":
                                PlayerRegEx.DamageAutoEn = regex;
                                break;
                            case "Failed":
                                PlayerRegEx.FailedEn = regex;
                                break;
                            case "FailedAuto":
                                PlayerRegEx.FailedAutoEn = regex;
                                break;
                            case "Actions":
                                PlayerRegEx.ActionsEn = regex;
                                break;
                            case "Items":
                                PlayerRegEx.ItemsEn = regex;
                                break;
                            case "Cure":
                                PlayerRegEx.CureEn = regex;
                                break;
                            case "BeneficialGain":
                                PlayerRegEx.BeneficialGainEn = regex;
                                break;
                            case "BeneficialLose":
                                PlayerRegEx.BeneficialLoseEn = regex;
                                break;
                            case "DetrimentalGain":
                                PlayerRegEx.DetrimentalGainEn = regex;
                                break;
                            case "DetrimentalLose":
                                PlayerRegEx.DetrimentalLoseEn = regex;
                                break;
                        }

                        #endregion

                        break;
                    case "FR":

                        #region Handle French Regular Expressions

                        switch (xKey)
                        {
                            case "Damage":
                                PlayerRegEx.DamageFr = regex;
                                break;
                            case "DamageAuto":
                                PlayerRegEx.DamageAutoFr = regex;
                                break;
                            case "Failed":
                                PlayerRegEx.FailedFr = regex;
                                break;
                            case "FailedAuto":
                                PlayerRegEx.FailedAutoFr = regex;
                                break;
                            case "Actions":
                                PlayerRegEx.ActionsFr = regex;
                                break;
                            case "Items":
                                PlayerRegEx.ItemsFr = regex;
                                break;
                            case "Cure":
                                PlayerRegEx.CureFr = regex;
                                break;
                            case "BeneficialGain":
                                PlayerRegEx.BeneficialGainFr = regex;
                                break;
                            case "BeneficialLose":
                                PlayerRegEx.BeneficialLoseFr = regex;
                                break;
                            case "DetrimentalGain":
                                PlayerRegEx.DetrimentalGainFr = regex;
                                break;
                            case "DetrimentalLose":
                                PlayerRegEx.DetrimentalLoseFr = regex;
                                break;
                        }

                        #endregion

                        break;
                    case "JA":

                        #region Handle Japanese Regular Expressions

                        switch (xKey)
                        {
                            case "Damage":
                                PlayerRegEx.DamageJa = regex;
                                break;
                            case "DamageAuto":
                                PlayerRegEx.DamageAutoJa = regex;
                                break;
                            case "Failed":
                                PlayerRegEx.FailedJa = regex;
                                break;
                            case "FailedAuto":
                                PlayerRegEx.FailedAutoJa = regex;
                                break;
                            case "Actions":
                                PlayerRegEx.ActionsJa = regex;
                                break;
                            case "Items":
                                PlayerRegEx.ItemsJa = regex;
                                break;
                            case "Cure":
                                PlayerRegEx.CureJa = regex;
                                break;
                            case "BeneficialGain":
                                PlayerRegEx.BeneficialGainJa = regex;
                                break;
                            case "BeneficialLose":
                                PlayerRegEx.BeneficialLoseJa = regex;
                                break;
                            case "DetrimentalGain":
                                PlayerRegEx.DetrimentalGainJa = regex;
                                break;
                            case "DetrimentalLose":
                                PlayerRegEx.DetrimentalLoseJa = regex;
                                break;
                        }

                        #endregion

                        break;
                    case "DE":

                        #region Handle German Regular Expressions

                        switch (xKey)
                        {
                            case "Damage":
                                PlayerRegEx.DamageDe = regex;
                                break;
                            case "DamageAuto":
                                PlayerRegEx.DamageAutoDe = regex;
                                break;
                            case "Failed":
                                PlayerRegEx.FailedDe = regex;
                                break;
                            case "FailedAuto":
                                PlayerRegEx.FailedAutoDe = regex;
                                break;
                            case "Actions":
                                PlayerRegEx.ActionsDe = regex;
                                break;
                            case "Items":
                                PlayerRegEx.ItemsDe = regex;
                                break;
                            case "Cure":
                                PlayerRegEx.CureDe = regex;
                                break;
                            case "BeneficialGain":
                                PlayerRegEx.BeneficialGainDe = regex;
                                break;
                            case "BeneficialLose":
                                PlayerRegEx.BeneficialLoseDe = regex;
                                break;
                            case "DetrimentalGain":
                                PlayerRegEx.DetrimentalGainDe = regex;
                                break;
                            case "DetrimentalLose":
                                PlayerRegEx.DetrimentalLoseDe = regex;
                                break;
                        }

                        #endregion

                        break;
                    case "ZH":

                        #region Handle Chinese Regular Expressions

                        switch (xKey)
                        {
                            case "Damage":
                                PlayerRegEx.DamageZh = regex;
                                break;
                            case "DamageAuto":
                                PlayerRegEx.DamageAutoZh = regex;
                                break;
                            case "Failed":
                                PlayerRegEx.FailedZh = regex;
                                break;
                            case "FailedAuto":
                                PlayerRegEx.FailedAutoZh = regex;
                                break;
                            case "Actions":
                                PlayerRegEx.ActionsZh = regex;
                                break;
                            case "Items":
                                PlayerRegEx.ItemsZh = regex;
                                break;
                            case "Cure":
                                PlayerRegEx.CureZh = regex;
                                break;
                            case "BeneficialGain":
                                PlayerRegEx.BeneficialGainZh = regex;
                                break;
                            case "BeneficialLose":
                                PlayerRegEx.BeneficialLoseZh = regex;
                                break;
                            case "DetrimentalGain":
                                PlayerRegEx.DetrimentalGainZh = regex;
                                break;
                            case "DetrimentalLose":
                                PlayerRegEx.DetrimentalLoseZh = regex;
                                break;
                        }

                        #endregion

                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadMonsterRegEx()
        {
            if (Constants.XRegEx == null)
            {
                return;
            }
            foreach (var xElement in Constants.XRegEx.Descendants()
                                              .Elements("Monster"))
            {
                var xKey = (string) xElement.Attribute("Key");
                var xLanguage = (string) xElement.Attribute("Language");
                var xValue = (string) xElement.Element("Value");
                if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                {
                    continue;
                }
                if (!SharedRegEx.IsValidRegex(xValue))
                {
                    continue;
                }
                var regex = new Regex(xValue, SharedRegEx.DefaultOptions);
                switch (xLanguage)
                {
                    case "EN":

                        #region Handle English Regular Expressions

                        switch (xKey)
                        {
                            case "Damage":
                                MonsterRegEx.DamageEn = regex;
                                break;
                            case "DamageAuto":
                                MonsterRegEx.DamageAutoEn = regex;
                                break;
                            case "Failed":
                                MonsterRegEx.FailedEn = regex;
                                break;
                            case "FailedAuto":
                                MonsterRegEx.FailedAutoEn = regex;
                                break;
                            case "Actions":
                                MonsterRegEx.ActionsEn = regex;
                                break;
                            case "Items":
                                MonsterRegEx.ItemsEn = regex;
                                break;
                            case "Cure":
                                MonsterRegEx.CureEn = regex;
                                break;
                            case "BeneficialGain":
                                MonsterRegEx.BeneficialGainEn = regex;
                                break;
                            case "BeneficialLose":
                                MonsterRegEx.BeneficialLoseEn = regex;
                                break;
                            case "DetrimentalGain":
                                MonsterRegEx.DetrimentalGainEn = regex;
                                break;
                            case "DetrimentalLose":
                                MonsterRegEx.DetrimentalLoseEn = regex;
                                break;
                        }

                        #endregion

                        break;
                    case "FR":

                        #region Handle French Regular Expressions

                        switch (xKey)
                        {
                            case "Damage":
                                MonsterRegEx.DamageFr = regex;
                                break;
                            case "DamageAuto":
                                MonsterRegEx.DamageAutoFr = regex;
                                break;
                            case "Failed":
                                MonsterRegEx.FailedFr = regex;
                                break;
                            case "FailedAuto":
                                MonsterRegEx.FailedAutoFr = regex;
                                break;
                            case "Actions":
                                MonsterRegEx.ActionsFr = regex;
                                break;
                            case "Items":
                                MonsterRegEx.ItemsFr = regex;
                                break;
                            case "Cure":
                                MonsterRegEx.CureFr = regex;
                                break;
                            case "BeneficialGain":
                                MonsterRegEx.BeneficialGainFr = regex;
                                break;
                            case "BeneficialLose":
                                MonsterRegEx.BeneficialLoseFr = regex;
                                break;
                            case "DetrimentalGain":
                                MonsterRegEx.DetrimentalGainFr = regex;
                                break;
                            case "DetrimentalLose":
                                MonsterRegEx.DetrimentalLoseFr = regex;
                                break;
                        }

                        #endregion

                        break;
                    case "JA":

                        #region Handle Japanese Regular Expressions

                        switch (xKey)
                        {
                            case "Damage":
                                MonsterRegEx.DamageJa = regex;
                                break;
                            case "DamageAuto":
                                MonsterRegEx.DamageAutoJa = regex;
                                break;
                            case "Failed":
                                MonsterRegEx.FailedJa = regex;
                                break;
                            case "FailedAuto":
                                MonsterRegEx.FailedAutoJa = regex;
                                break;
                            case "Actions":
                                MonsterRegEx.ActionsJa = regex;
                                break;
                            case "Items":
                                MonsterRegEx.ItemsJa = regex;
                                break;
                            case "Cure":
                                MonsterRegEx.CureJa = regex;
                                break;
                            case "BeneficialGain":
                                MonsterRegEx.BeneficialGainJa = regex;
                                break;
                            case "BeneficialLose":
                                MonsterRegEx.BeneficialLoseJa = regex;
                                break;
                            case "DetrimentalGain":
                                MonsterRegEx.DetrimentalGainJa = regex;
                                break;
                            case "DetrimentalLose":
                                MonsterRegEx.DetrimentalLoseJa = regex;
                                break;
                        }

                        #endregion

                        break;
                    case "DE":

                        #region Handle German Regular Expressions

                        switch (xKey)
                        {
                            case "Damage":
                                MonsterRegEx.DamageDe = regex;
                                break;
                            case "DamageAuto":
                                MonsterRegEx.DamageAutoDe = regex;
                                break;
                            case "Failed":
                                MonsterRegEx.FailedDe = regex;
                                break;
                            case "FailedAuto":
                                MonsterRegEx.FailedAutoDe = regex;
                                break;
                            case "Actions":
                                MonsterRegEx.ActionsDe = regex;
                                break;
                            case "Items":
                                MonsterRegEx.ItemsDe = regex;
                                break;
                            case "Cure":
                                MonsterRegEx.CureDe = regex;
                                break;
                            case "BeneficialGain":
                                MonsterRegEx.BeneficialGainDe = regex;
                                break;
                            case "BeneficialLose":
                                MonsterRegEx.BeneficialLoseDe = regex;
                                break;
                            case "DetrimentalGain":
                                MonsterRegEx.DetrimentalGainDe = regex;
                                break;
                            case "DetrimentalLose":
                                MonsterRegEx.DetrimentalLoseDe = regex;
                                break;
                        }

                        #endregion

                        break;
                    case "ZH":

                        #region Handle Chinese Regular Expressions

                        switch (xKey)
                        {
                            case "Damage":
                                MonsterRegEx.DamageZh = regex;
                                break;
                            case "DamageAuto":
                                MonsterRegEx.DamageAutoZh = regex;
                                break;
                            case "Failed":
                                MonsterRegEx.FailedZh = regex;
                                break;
                            case "FailedAuto":
                                MonsterRegEx.FailedAutoZh = regex;
                                break;
                            case "Actions":
                                MonsterRegEx.ActionsZh = regex;
                                break;
                            case "Items":
                                MonsterRegEx.ItemsZh = regex;
                                break;
                            case "Cure":
                                MonsterRegEx.CureZh = regex;
                                break;
                            case "BeneficialGain":
                                MonsterRegEx.BeneficialGainZh = regex;
                                break;
                            case "BeneficialLose":
                                MonsterRegEx.BeneficialLoseZh = regex;
                                break;
                            case "DetrimentalGain":
                                MonsterRegEx.DetrimentalGainZh = regex;
                                break;
                            case "DetrimentalLose":
                                MonsterRegEx.DetrimentalLoseZh = regex;
                                break;
                        }

                        #endregion

                        break;
                }
            }
        }

        public static void EnsureLogsDirectory()
        {
            var path = Path.Combine(Common.Constants.LogsPath, "Parser");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void SetupWidgetTopMost()
        {
            WidgetTopMostHelper.HookWidgetTopMost();
        }
    }
}
