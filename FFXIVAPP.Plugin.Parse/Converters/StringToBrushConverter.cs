// FFXIVAPP.Plugin.Parse
// StringToBrushConverter.cs
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
using System.Windows.Data;
using System.Windows.Media;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.Converters
{
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = "DEFAULT";
            try
            {
                param = ((Actor.Job) value).ToString()
                                           .ToUpperInvariant();
            }
            catch (Exception ex)
            {
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
            color = color.Replace("#", "");
            try
            {
                return (SolidColorBrush) (new BrushConverter().ConvertFrom(color));
            }
            catch (Exception ex)
            {
            }
            try
            {
                switch (color.Length)
                {
                    case 8:
                        return new SolidColorBrush(Color.FromArgb(Byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber), Byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber), Byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber), Byte.Parse(color.Substring(6, 2), NumberStyles.HexNumber)));
                    case 6:
                        return new SolidColorBrush(Color.FromRgb(Byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber), Byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber), Byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber)));
                    default:
                        return Brushes.Green;
                }
            }
            catch (Exception ex)
            {
                return Brushes.Green;
            }
        }
    }
}
