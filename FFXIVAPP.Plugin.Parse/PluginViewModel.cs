// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PluginViewModel.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PluginViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using FFXIVAPP.Common.Events;
    using FFXIVAPP.Common.Helpers;

    internal sealed class PluginViewModel : INotifyPropertyChanged {
        private static Lazy<PluginViewModel> _instance = new Lazy<PluginViewModel>(() => new PluginViewModel());

        private bool _enableHelpLabels;

        private Dictionary<string, string> _locale;

        private string _uiScale;

        // used for global static properties
        public event EventHandler<PopupResultEvent> PopupResultChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static PluginViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public static Dictionary<string, string> PluginInfo {
            get {
                Dictionary<string, string> pluginInfo = new Dictionary<string, string>();
                pluginInfo.Add("Icon", "Logo.png");
                pluginInfo.Add("Name", AssemblyHelper.Name);
                pluginInfo.Add("Description", AssemblyHelper.Description);
                pluginInfo.Add("Copyright", AssemblyHelper.Copyright);
                pluginInfo.Add("Version", AssemblyHelper.Version.ToString());
                return pluginInfo;
            }
        }

        public bool EnableHelpLabels {
            get {
                return this._enableHelpLabels;
            }

            set {
                this._enableHelpLabels = value;
                this.RaisePropertyChanged();
            }
        }

        public Dictionary<string, string> Locale {
            get {
                return this._locale ?? (this._locale = new Dictionary<string, string>());
            }

            set {
                this._locale = value;
                this.RaisePropertyChanged();
            }
        }

        public string UIScale {
            get {
                return this._uiScale;
            }

            set {
                this._uiScale = value;
                this.RaisePropertyChanged();
            }
        }

        public void OnPopupResultChanged(PopupResultEvent e) {
            this.PopupResultChanged(this, e);
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}