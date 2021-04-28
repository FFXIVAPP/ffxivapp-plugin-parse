// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   SettingsViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.ViewModels {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using FFXIVAPP.Common.ViewModelBase;
    using FFXIVAPP.Plugin.Parse.Properties;

    internal sealed class SettingsViewModel : INotifyPropertyChanged {
        private static Lazy<SettingsViewModel> _instance = new Lazy<SettingsViewModel>(() => new SettingsViewModel());

        public SettingsViewModel() {
            this.ResetDPSWidgetCommand = new DelegateCommand(this.ResetDPSWidget);
            this.OpenDPSWidgetCommand = new DelegateCommand(this.OpenDPSWidget);
            this.ResetDTPSWidgetCommand = new DelegateCommand(this.ResetDTPSWidget);
            this.OpenDTPSWidgetCommand = new DelegateCommand(this.OpenDTPSWidget);
            this.ResetHPSWidgetCommand = new DelegateCommand(this.ResetHPSWidget);
            this.OpenHPSWidgetCommand = new DelegateCommand(this.OpenHPSWidget);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static SettingsViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public ICommand OpenDPSWidgetCommand { get; private set; }

        public ICommand OpenDTPSWidgetCommand { get; private set; }

        public ICommand OpenHPSWidgetCommand { get; private set; }

        public ICommand ResetDPSWidgetCommand { get; private set; }

        public ICommand ResetDTPSWidgetCommand { get; private set; }

        public ICommand ResetHPSWidgetCommand { get; private set; }

        public void OpenDPSWidget() {
            Settings.Default.ShowDPSWidgetOnLoad = true;
            Widgets.Instance.ShowDPSWidget();
        }

        public void OpenDTPSWidget() {
            Settings.Default.ShowDTPSWidgetOnLoad = true;
            Widgets.Instance.ShowDTPSWidget();
        }

        public void OpenHPSWidget() {
            Settings.Default.ShowHPSWidgetOnLoad = true;
            Widgets.Instance.ShowHPSWidget();
        }

        public void ResetDPSWidget() {
            Settings.Default.DPSWidgetUIScale = Settings.Default.Properties["DPSWidgetUIScale"].DefaultValue.ToString();
            Settings.Default.DPSWidgetTop = int.Parse(Settings.Default.Properties["DPSWidgetTop"].DefaultValue.ToString());
            Settings.Default.DPSWidgetLeft = int.Parse(Settings.Default.Properties["DPSWidgetLeft"].DefaultValue.ToString());
            Settings.Default.DPSWidgetHeight = int.Parse(Settings.Default.Properties["DPSWidgetHeight"].DefaultValue.ToString());
            Settings.Default.DPSWidgetWidth = int.Parse(Settings.Default.Properties["DPSWidgetWidth"].DefaultValue.ToString());
            Settings.Default.DPSVisibility = Settings.Default.Properties["DPSVisibility"].DefaultValue.ToString();
            Settings.Default.DPSWidgetSortDirection = Settings.Default.Properties["DPSWidgetSortDirection"].DefaultValue.ToString();
            Settings.Default.DPSWidgetSortProperty = Settings.Default.Properties["DPSWidgetSortProperty"].DefaultValue.ToString();
        }

        public void ResetDTPSWidget() {
            Settings.Default.DTPSWidgetUIScale = Settings.Default.Properties["DTPSWidgetUIScale"].DefaultValue.ToString();
            Settings.Default.DTPSWidgetTop = int.Parse(Settings.Default.Properties["DTPSWidgetTop"].DefaultValue.ToString());
            Settings.Default.DTPSWidgetLeft = int.Parse(Settings.Default.Properties["DTPSWidgetLeft"].DefaultValue.ToString());
            Settings.Default.DTPSWidgetHeight = int.Parse(Settings.Default.Properties["DTPSWidgetHeight"].DefaultValue.ToString());
            Settings.Default.DTPSWidgetWidth = int.Parse(Settings.Default.Properties["DTPSWidgetWidth"].DefaultValue.ToString());
            Settings.Default.DTPSVisibility = Settings.Default.Properties["DTPSVisibility"].DefaultValue.ToString();
            Settings.Default.DTPSWidgetSortDirection = Settings.Default.Properties["DTPSWidgetSortDirection"].DefaultValue.ToString();
            Settings.Default.DTPSWidgetSortProperty = Settings.Default.Properties["DTPSWidgetSortProperty"].DefaultValue.ToString();
        }

        public void ResetHPSWidget() {
            Settings.Default.HPSWidgetUIScale = Settings.Default.Properties["HPSWidgetUIScale"].DefaultValue.ToString();
            Settings.Default.HPSWidgetTop = int.Parse(Settings.Default.Properties["HPSWidgetTop"].DefaultValue.ToString());
            Settings.Default.HPSWidgetLeft = int.Parse(Settings.Default.Properties["HPSWidgetLeft"].DefaultValue.ToString());
            Settings.Default.HPSWidgetHeight = int.Parse(Settings.Default.Properties["HPSWidgetHeight"].DefaultValue.ToString());
            Settings.Default.HPSWidgetWidth = int.Parse(Settings.Default.Properties["HPSWidgetWidth"].DefaultValue.ToString());
            Settings.Default.HPSVisibility = Settings.Default.Properties["HPSVisibility"].DefaultValue.ToString();
            Settings.Default.HPSWidgetSortDirection = Settings.Default.Properties["HPSWidgetSortDirection"].DefaultValue.ToString();
            Settings.Default.HPSWidgetSortProperty = Settings.Default.Properties["HPSWidgetSortProperty"].DefaultValue.ToString();
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}