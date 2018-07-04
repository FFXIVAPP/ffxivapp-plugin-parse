// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringToBrushConverter.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   StringToBrushConverter.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Converters {
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Properties;

    using NLog;

    public class StringToBrushConverter : IValueConverter {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var param = "DEFAULT";
            try {
                param = value.ToString().ToUpperInvariant();
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            SolidColorBrush brush = ColorStringToBrush(Settings.Default.DefaultProgressBarForeground);
            switch (param) {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        private static SolidColorBrush ColorStringToBrush(string color) {
            color = color.Replace("#", string.Empty);
            try {
                return (SolidColorBrush) new BrushConverter().ConvertFrom(color);
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            try {
                switch (color.Length) {
                    case 8:
                        return new SolidColorBrush(Color.FromArgb(byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(6, 2), NumberStyles.HexNumber)));
                    case 6:
                        return new SolidColorBrush(Color.FromRgb(byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber)));
                    default:
                        return Brushes.Green;
                }
            }
            catch (Exception) {
                return Brushes.Green;
            }
        }
    }
}