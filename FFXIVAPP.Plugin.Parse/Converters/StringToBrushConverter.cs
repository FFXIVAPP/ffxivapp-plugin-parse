// FFXIVAPP.Plugin.Parse ~ StringToBrushConverter.cs
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
using System.Windows.Data;
using System.Windows.Media;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Properties;
using NLog;

namespace FFXIVAPP.Plugin.Parse.Converters
{
    public class StringToBrushConverter : IValueConverter
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = "DEFAULT";
            try
            {
                param = value.ToString()
                             .ToUpperInvariant();
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
            }
            var brush = ColorStringToBrush(Settings.Default.DefaultProgressBarForeground);
            switch (param)
            {
                case "PLD":
                    brush = ColorStringToBrush(Settings.Default.PLDProgressBarForeground);
                    break;
                case "DRG":
                    brush = ColorStringToBrush(Settings.Default.DRGProgressBarForeground);
                    break;
                case "BLM":
                    brush = ColorStringToBrush(Settings.Default.BLMProgressBarForeground);
                    break;
                case "WAR":
                    brush = ColorStringToBrush(Settings.Default.WARProgressBarForeground);
                    break;
                case "WHM":
                    brush = ColorStringToBrush(Settings.Default.WHMProgressBarForeground);
                    break;
                case "SCH":
                    brush = ColorStringToBrush(Settings.Default.SCHProgressBarForeground);
                    break;
                case "MNK":
                    brush = ColorStringToBrush(Settings.Default.MNKProgressBarForeground);
                    break;
                case "BRD":
                    brush = ColorStringToBrush(Settings.Default.BRDProgressBarForeground);
                    break;
                case "SMN":
                    brush = ColorStringToBrush(Settings.Default.SMNProgressBarForeground);
                    break;
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static SolidColorBrush ColorStringToBrush(string color)
        {
            color = color.Replace("#", string.Empty);
            try
            {
                return (SolidColorBrush) new BrushConverter().ConvertFrom(color);
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
            }
            try
            {
                switch (color.Length)
                {
                    case 8: return new SolidColorBrush(Color.FromArgb(Byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber), Byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber), Byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber), Byte.Parse(color.Substring(6, 2), NumberStyles.HexNumber)));
                    case 6: return new SolidColorBrush(Color.FromRgb(Byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber), Byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber), Byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber)));
                    default: return Brushes.Green;
                }
            }
            catch (Exception)
            {
                return Brushes.Green;
            }
        }
    }
}
