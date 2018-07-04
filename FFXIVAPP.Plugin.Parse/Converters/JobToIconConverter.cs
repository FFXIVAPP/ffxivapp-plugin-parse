// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobToIconConverter.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   JobToIconConverter.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Converters {
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using FFXIVAPP.ResourceFiles;

    using NLog;

    public class JobToIconConverter : IValueConverter {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Converts a Actor.Job to the associated job icon as a BitmapImage.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return Game.GetIconByName(value?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}