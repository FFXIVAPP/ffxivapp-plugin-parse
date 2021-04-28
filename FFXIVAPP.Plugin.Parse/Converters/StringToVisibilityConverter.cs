// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringToVisibilityConverter.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   StringToVisibilityConverter.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Converters {
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Properties;

    using NLog;

    public class StringToVisibilityConverter : IValueConverter {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                var tag = value.ToString();
                var displayProperty = string.Empty;
                switch (tag) {
                    case "DPS":
                    case "TotalOverallDamage":
                    case "CombinedDPS":
                    case "CombinedTotalOverallDamage":
                        displayProperty = Settings.Default.DPSWidgetDisplayProperty;
                        switch (displayProperty) {
                            case "Individual":
                                return tag.Contains("Combined")
                                           ? Visibility.Collapsed
                                           : Visibility.Visible;
                            case "Combined":
                                return tag.Contains("Combined")
                                           ? Visibility.Visible
                                           : Visibility.Collapsed;
                        }

                        break;
                    case "HPS":
                    case "TotalOverallHealing":
                    case "CombinedHPS":
                    case "CombinedTotalOverallHealing":
                        displayProperty = Settings.Default.HPSWidgetDisplayProperty;
                        switch (displayProperty) {
                            case "Individual":
                                return tag.Contains("Combined")
                                           ? Visibility.Collapsed
                                           : Visibility.Visible;
                            case "Combined":
                                return tag.Contains("Combined")
                                           ? Visibility.Visible
                                           : Visibility.Collapsed;
                        }

                        break;
                    case "DTPS":
                    case "TotalOverallDamageTaken":
                    case "CombinedDTPS":
                    case "CombinedTotalOverallDamageTaken":
                        displayProperty = Settings.Default.DTPSWidgetDisplayProperty;
                        switch (displayProperty) {
                            case "Individual":
                                return tag.Contains("Combined")
                                           ? Visibility.Collapsed
                                           : Visibility.Visible;
                            case "Combined":
                                return tag.Contains("Combined")
                                           ? Visibility.Visible
                                           : Visibility.Collapsed;
                        }

                        break;
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}