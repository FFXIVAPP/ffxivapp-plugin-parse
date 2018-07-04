// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Constants.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    using FFXIVAPP.Common.Helpers;

    public static class Constants {
        public const string LibraryPack = "pack://application:,,,/FFXIVAPP.Plugin.Parse;component/";

        public static readonly List<string> Abilities = new List<string> {
            "142B",
            "14AB",
            "152B",
            "15AB",
            "162B",
            "16AB",
            "172B",
            "17AB",
            "182B",
            "18AB",
            "192B",
            "19AB",
            "1A2B",
            "1AAB",
            "1B2B",
            "1BAB"
        };

        public static readonly List<string> NeedGreed = new List<string> {
            "rolls Need",
            "rolls Greed",
            "dés Besoin",
            "dés Cupidité"
        };

        public static readonly string[] Supported = {
            "en",
            "ja",
            "fr",
            "de",
            "ru"
        };

        public static StringComparison CultureComparer = StringComparison.CurrentCultureIgnoreCase;

        public static StringComparison InvariantComparer = StringComparison.InvariantCultureIgnoreCase;

        private static Dictionary<string, string> _autoTranslate;

        private static Dictionary<string, string> _chatCodes;

        private static Dictionary<string, string[]> _colors;

        private static CultureInfo _cultureInfo;

        private static List<string> _settings;

        private static XDocument _xRegEx;

        private static XDocument _xSettings;

        public static Dictionary<string, string> AutoTranslate {
            get {
                return _autoTranslate ?? (_autoTranslate = new Dictionary<string, string>());
            }

            set {
                _autoTranslate = value;
            }
        }

        public static string BaseDirectory {
            get {
                var appDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
                return Path.Combine(appDirectory, "Plugins", Plugin.PName);
            }
        }

        public static string CharacterName { get; set; }

        public static Dictionary<string, string> ChatCodes {
            get {
                return _chatCodes ?? (_chatCodes = new Dictionary<string, string>());
            }

            set {
                _chatCodes = value;
            }
        }

        public static string ChatCodesXML { get; set; }

        public static Dictionary<string, string[]> Colors {
            get {
                return _colors ?? (_colors = new Dictionary<string, string[]>());
            }

            set {
                _colors = value;
            }
        }

        public static CultureInfo CultureInfo {
            get {
                return _cultureInfo ?? (_cultureInfo = new CultureInfo("en"));
            }

            set {
                _cultureInfo = value;
            }
        }

        public static bool EnableHelpLabels { get; set; }

        public static bool EnableNetworkReading { get; set; }

        public static string GameLanguage { get; set; }

        public static string ServerName { get; set; }

        public static List<string> Settings {
            get {
                return _settings ?? (_settings = new List<string>());
            }

            set {
                _settings = value;
            }
        }

        public static string Theme { get; set; }

        public static string UIScale { get; set; }

        public static XDocument XRegEx {
            get {
                var file = Path.Combine(Common.Constants.PluginsSettingsPath, "RegularExpressions.xml");
                if (_xRegEx != null) {
                    return _xRegEx;
                }

                try {
                    var found = File.Exists(file);
                    _xRegEx = found
                                  ? XDocument.Load(file)
                                  : ResourceHelper.XDocResource(LibraryPack + "/Defaults/RegularExpressions.xml");
                }
                catch (Exception) {
                    _xRegEx = ResourceHelper.XDocResource(LibraryPack + "/Defaults/RegularExpressions.xml");
                }

                return _xRegEx;
            }

            set {
                _xRegEx = value;
            }
        }

        public static XDocument XSettings {
            get {
                var file = Path.Combine(Common.Constants.PluginsSettingsPath, "FFXIVAPP.Plugin.Parse.xml");
                var legacyFile = "./Plugins/FFXIVAPP.Plugin.Parse/Settings.xml";
                if (_xSettings != null) {
                    return _xSettings;
                }

                try {
                    var found = File.Exists(file);
                    if (found) {
                        _xSettings = XDocument.Load(file);
                    }
                    else {
                        found = File.Exists(legacyFile);
                        _xSettings = found
                                         ? XDocument.Load(legacyFile)
                                         : ResourceHelper.XDocResource(LibraryPack + "/Defaults/Settings.xml");
                    }
                }
                catch (Exception) {
                    _xSettings = ResourceHelper.XDocResource(LibraryPack + "/Defaults/Settings.xml");
                }

                return _xSettings;
            }

            set {
                _xSettings = value;
            }
        }
    }
}