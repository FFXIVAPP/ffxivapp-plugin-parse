// FFXIVAPP.Plugin.Parse ~ NameToAvatarConverter.cs
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
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.ResourceFiles;
using HtmlAgilityPack;
using NLog;

namespace FFXIVAPP.Plugin.Parse.Converters
{
    public class NameToAvatarConverter : IMultiValueConverter
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        private readonly bool _cachingEnabled = true;

        /// <summary>
        /// </summary>
        public NameToAvatarConverter()
        {
            if (Directory.Exists(AvatarCache))
            {
                return;
            }
            try
            {
                Directory.CreateDirectory(AvatarCache);
            }
            catch
            {
                _cachingEnabled = false;
            }
        }

        private bool IsProcessing { get; set; }

        private string AvatarCache
        {
            get { return Path.Combine(Common.Constants.CachePath, "Avatars"); }
        }

        /// <summary>
        /// </summary>
        /// <param name="values"> </param>
        /// <param name="targetType"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] == null)
            {
                return null;
            }
            var image = values[0] as Image;
            var name = values[1] as String;
            if (image == null || name == null || Regex.IsMatch(name, @"\[[(A|[\?]+]\]"))
            {
                return Theme.DefaultAvatar;
            }
            name = Regex.Replace(name, @"\[[\w]+\]", string.Empty)
                        .Trim();
            var fileName = $"{Constants.ServerName}.{name.Replace(" ", string.Empty)}.{"png"}";
            var cachePath = Path.Combine(AvatarCache, fileName);
            if (_cachingEnabled && File.Exists(cachePath))
            {
                return ImageUtilities.LoadImageFromStream(cachePath);
            }
            var useAvatars = !String.IsNullOrWhiteSpace(Constants.ServerName);
            if (!useAvatars || IsProcessing)
            {
                return Theme.DefaultAvatar;
            }
            IsProcessing = true;
            Func<bool> downloadFunc = delegate
            {
                try
                {
                    var serverName = Constants.ServerName;
                    var url = "http://na.finalfantasyxiv.com/lodestone/character/?q={0}&worldname={1}";
                    var httpWebRequest = (HttpWebRequest) WebRequest.Create(String.Format(url, HttpUtility.UrlEncode(name), Uri.EscapeUriString(serverName)));
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                    httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    using (var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse())
                    {
                        using (var stream = httpWebResponse.GetResponseStream())
                        {
                            if (httpWebResponse.StatusCode == HttpStatusCode.OK && stream != null)
                            {
                                var doc = new HtmlDocument();
                                doc.Load(stream);
                                var htmlSource = doc.DocumentNode.SelectSingleNode("//html")
                                                    .OuterHtml;
                                var src = new Regex(@"<img src=string.Empty(?<image>.+)string.Empty width=string.Empty50string.Empty height=string.Empty50string.Empty alt=string.Emptystring.Empty>", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                                var imageUrl = src.Match(htmlSource)
                                                  .Groups["image"]
                                                  .Value;
                                imageUrl = imageUrl.Substring(0, imageUrl.IndexOf("?", Constants.InvariantComparer))
                                                   .Replace("50x50", "96x96");
                                image.Dispatcher.Invoke(DispatcherPriority.Background, (ThreadStart) delegate
                                {
                                    var imageUri = _cachingEnabled ? SaveToCache(fileName, new Uri(imageUrl)) : imageUrl;
                                    if (imageUri == null)
                                    {
                                        image.Source = Theme.DefaultAvatar;
                                    }
                                    else
                                    {
                                        image.Source = ImageUtilities.LoadImageFromStream(imageUri);
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
                IsProcessing = false;
                return true;
            };
            downloadFunc.BeginInvoke(delegate { }, downloadFunc);
            return Theme.DefaultAvatar;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="targetTypes"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"> </param>
        /// <param name="imageUri"> </param>
        /// <returns> </returns>
        private string SaveToCache(string fileName, Uri imageUri)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(imageUri);
                using (var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse())
                {
                    using (var response = httpResponse.GetResponseStream())
                    {
                        if (response != null)
                        {
                            if (httpResponse.ContentType == "image/jpeg" || httpResponse.ContentType == "image/png")
                            {
                                var imagePath = Path.Combine(AvatarCache, fileName);
                                using (var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write))
                                {
                                    response.CopyTo(fileStream);
                                    fileStream.Close();
                                }
                                return imagePath;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
            }
            return null;
        }
    }
}
