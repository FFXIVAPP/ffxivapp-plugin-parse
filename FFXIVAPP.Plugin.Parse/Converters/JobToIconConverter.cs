// FFXIVAPP.Plugin.Parse ~ JobToIconConverter.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FFXIVAPP.Memory.Core.Enums;
using NLog;

namespace FFXIVAPP.Plugin.Parse.Converters
{
    public class JobToIconConverter : IValueConverter
    {
        private const string BasePath = Constants.LibraryPack + "Media/Images/Jobs/";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Converts a Actor.Job to the associated job icon as a BitmapImage.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return GetIcon((Actor.Job) value);
            }
            catch (InvalidCastException ex)
            {
                Logger.Error("Failed to convert job to job icon", ex);
                return GetIcon(Actor.Job.Unknown);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private BitmapImage GetIcon(Actor.Job job)
        {
            var path = new Uri(Path.Combine(BasePath, job + ".png"));
            return new BitmapImage(path);
        }
    }
}
