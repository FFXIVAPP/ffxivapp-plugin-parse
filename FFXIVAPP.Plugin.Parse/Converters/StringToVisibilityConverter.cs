// FFXIVAPP.Plugin.Parse
// StringToVisibilityConverter.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
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
using System.Windows;
using System.Windows.Data;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tag = value.ToString();
                var displayProperty = "";
                switch (tag)
                {
                    case "DPS":
                    case "TotalOverallDamage":
                    case "CombinedDPS":
                    case "CombinedTotalOverallDamage":
                        displayProperty = Settings.Default.DPSWidgetDisplayProperty;
                        switch (displayProperty)
                        {
                            case "Individual":
                                return tag.Contains("Combined") ? Visibility.Collapsed : Visibility.Visible;
                            case "Combined":
                                return tag.Contains("Combined") ? Visibility.Visible : Visibility.Collapsed;
                        }
                        break;
                    case "HPS":
                    case "TotalOverallHealing":
                    case "CombinedHPS":
                    case "CombinedTotalOverallHealing":
                        displayProperty = Settings.Default.HPSWidgetDisplayProperty;
                        switch (displayProperty)
                        {
                            case "Individual":
                                return tag.Contains("Combined") ? Visibility.Collapsed : Visibility.Visible;
                            case "Combined":
                                return tag.Contains("Combined") ? Visibility.Visible : Visibility.Collapsed;
                        }
                        break;
                    case "DTPS":
                    case "TotalOverallDamageTaken":
                    case "CombinedDTPS":
                    case "CombinedTotalOverallDamageTaken":
                        displayProperty = Settings.Default.DTPSWidgetDisplayProperty;
                        switch (displayProperty)
                        {
                            case "Individual":
                                return tag.Contains("Combined") ? Visibility.Collapsed : Visibility.Visible;
                            case "Combined":
                                return tag.Contains("Combined") ? Visibility.Visible : Visibility.Collapsed;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
