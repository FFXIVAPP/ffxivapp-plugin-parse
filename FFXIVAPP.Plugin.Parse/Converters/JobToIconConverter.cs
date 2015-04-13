using FFXIVAPP.Common.Core.Memory.Enums;
using NLog;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace FFXIVAPP.Plugin.Parse.Converters
{
    public class JobToIconConverter : IValueConverter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string BasePath = FFXIVAPP.Plugin.Parse.Constants.LibraryPack + "Media/Images/Jobs/";

        /// <summary>
        /// Converts a Actor.Job to the associated job icon as a BitmapImage.
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
                return GetIcon((Actor.Job)value);
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
            var path = new Uri(Path.Combine(BasePath, job.ToString() + ".png"));
            return new BitmapImage(path);
        }
    }
}
