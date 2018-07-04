// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameToAvatarConverter.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   NameToAvatarConverter.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Converters {
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
    using System.Windows.Threading;

    using FFXIVAPP.Common;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.ResourceFiles;

    using HtmlAgilityPack;

    using NLog;

    using Constants = FFXIVAPP.Plugin.Parse.Constants;

    public class NameToAvatarConverter : IMultiValueConverter {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly bool _cachingEnabled = true;

        /// <summary>
        /// </summary>
        public NameToAvatarConverter() {
            if (Directory.Exists(this.AvatarCache)) {
                return;
            }

            try {
                Directory.CreateDirectory(this.AvatarCache);
            }
            catch {
                this._cachingEnabled = false;
            }
        }

        private string AvatarCache {
            get {
                return Path.Combine(Common.Constants.CachePath, "Avatars");
            }
        }

        private bool IsProcessing { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="values"> </param>
        /// <param name="targetType"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values[1] == null) {
                return null;
            }

            var image = values[0] as Image;
            var name = values[1] as string;
            if (image == null || name == null || Regex.IsMatch(name, @"\[[(A|[\?]+]\]")) {
                return Theme.DefaultAvatar;
            }

            name = Regex.Replace(name, @"\[[\w]+\]", string.Empty).Trim();
            var fileName = $"{Constants.ServerName}.{name.Replace(" ", string.Empty)}.{"png"}";
            var cachePath = Path.Combine(this.AvatarCache, fileName);
            if (this._cachingEnabled && File.Exists(cachePath)) {
                return ImageUtilities.LoadImageFromStream(cachePath);
            }

            var useAvatars = !string.IsNullOrWhiteSpace(Constants.ServerName);
            if (!useAvatars || this.IsProcessing) {
                return Theme.DefaultAvatar;
            }

            this.IsProcessing = true;

            Func<bool> downloadAvatar = delegate {
                try {
                    var serverName = Constants.ServerName;
                    var url = "http://na.finalfantasyxiv.com/lodestone/character/?q={0}&worldname={1}";
                    var httpWebRequest = (HttpWebRequest) WebRequest.Create(string.Format(url, HttpUtility.UrlEncode(name), Uri.EscapeUriString(serverName)));
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                    httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    using (var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse()) {
                        using (Stream stream = httpWebResponse.GetResponseStream()) {
                            if (httpWebResponse.StatusCode == HttpStatusCode.OK && stream != null) {
                                var doc = new HtmlDocument();
                                doc.Load(stream);
                                var htmlSource = doc.DocumentNode.SelectSingleNode("//html").OuterHtml;
                                var src = new Regex(@"<img src=string.Empty(?<image>.+)string.Empty width=string.Empty50string.Empty height=string.Empty50string.Empty alt=string.Emptystring.Empty>", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                                var imageUrl = src.Match(htmlSource).Groups["image"].Value;
                                imageUrl = imageUrl.Substring(0, imageUrl.IndexOf("?", Constants.InvariantComparer)).Replace("50x50", "96x96");
                                image.Dispatcher.Invoke(
                                    DispatcherPriority.Background,
                                    (ThreadStart) delegate {
                                        var imageUri = this._cachingEnabled
                                                           ? this.SaveToCache(fileName, new Uri(imageUrl))
                                                           : imageUrl;
                                        if (imageUri == null) {
                                            image.Source = Theme.DefaultAvatar;
                                        }
                                        else {
                                            image.Source = ImageUtilities.LoadImageFromStream(imageUri);
                                        }
                                    });
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }

                this.IsProcessing = false;
                return true;
            };
            downloadAvatar.BeginInvoke(delegate { }, downloadAvatar);
            return Theme.DefaultAvatar;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="targetTypes"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"> </param>
        /// <param name="imageUri"> </param>
        /// <returns> </returns>
        private string SaveToCache(string fileName, Uri imageUri) {
            try {
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(imageUri);
                using (var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse()) {
                    using (Stream response = httpResponse.GetResponseStream()) {
                        if (response != null) {
                            if (httpResponse.ContentType == "image/jpeg" || httpResponse.ContentType == "image/png") {
                                var imagePath = Path.Combine(this.AvatarCache, fileName);
                                using (var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write)) {
                                    response.CopyTo(fileStream);
                                    fileStream.Close();
                                }

                                return imagePath;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            return null;
        }
    }
}