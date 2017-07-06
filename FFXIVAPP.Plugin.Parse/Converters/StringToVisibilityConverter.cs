// FFXIVAPP.Plugin.Parse ~ StringToVisibilityConverter.cs
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

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Properties;
using NLog;

namespace FFXIVAPP.Plugin.Parse.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tag = value.ToString();
                var displayProperty = string.Empty;
                switch (tag)
                {
                    case "DPS":
                    case "TotalOverallDamage":
                    case "CombinedDPS":
                    case "CombinedTotalOverallDamage":
                        displayProperty = Settings.Default.DPSWidgetDisplayProperty;
                        switch (displayProperty)
                        {
                            case "Individual": return tag.Contains("Combined") ? Visibility.Collapsed : Visibility.Visible;
                            case "Combined": return tag.Contains("Combined") ? Visibility.Visible : Visibility.Collapsed;
                        }
                        break;
                    case "HPS":
                    case "TotalOverallHealing":
                    case "CombinedHPS":
                    case "CombinedTotalOverallHealing":
                        displayProperty = Settings.Default.HPSWidgetDisplayProperty;
                        switch (displayProperty)
                        {
                            case "Individual": return tag.Contains("Combined") ? Visibility.Collapsed : Visibility.Visible;
                            case "Combined": return tag.Contains("Combined") ? Visibility.Visible : Visibility.Collapsed;
                        }
                        break;
                    case "DTPS":
                    case "TotalOverallDamageTaken":
                    case "CombinedDTPS":
                    case "CombinedTotalOverallDamageTaken":
                        displayProperty = Settings.Default.DTPSWidgetDisplayProperty;
                        switch (displayProperty)
                        {
                            case "Individual": return tag.Contains("Combined") ? Visibility.Collapsed : Visibility.Visible;
                            case "Combined": return tag.Contains("Combined") ? Visibility.Visible : Visibility.Collapsed;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
