// FFXIVAPP.Plugin.Parse ~ Initializer.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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

        #region Declarations

        #endregion
    }
}
