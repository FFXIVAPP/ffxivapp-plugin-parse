// FFXIVAPP.Plugin.Parse
// FFXIVAPP & Related Plugins/Modules
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFXIVAPP.Common.ViewModelBase;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.ViewModels
{
    internal sealed class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            ResetDPSWidgetCommand = new DelegateCommand(ResetDPSWidget);
            OpenDPSWidgetCommand = new DelegateCommand(OpenDPSWidget);
            ResetDTPSWidgetCommand = new DelegateCommand(ResetDTPSWidget);
            OpenDTPSWidgetCommand = new DelegateCommand(OpenDTPSWidget);
            ResetHPSWidgetCommand = new DelegateCommand(ResetHPSWidget);
            OpenHPSWidgetCommand = new DelegateCommand(OpenHPSWidget);
        }

        #region Property Bindings

        private static SettingsViewModel _instance;

        public static SettingsViewModel Instance
        {
            get { return _instance ?? (_instance = new SettingsViewModel()); }
        }

        #endregion

        #region Declarations

        public ICommand ResetDPSWidgetCommand { get; private set; }
        public ICommand OpenDPSWidgetCommand { get; private set; }
        public ICommand ResetDTPSWidgetCommand { get; private set; }
        public ICommand OpenDTPSWidgetCommand { get; private set; }
        public ICommand ResetHPSWidgetCommand { get; private set; }
        public ICommand OpenHPSWidgetCommand { get; private set; }

        #endregion

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        public void ResetDPSWidget()
        {
            Settings.Default.DPSWidgetUIScale = Settings.Default.Properties["DPSWidgetUIScale"].DefaultValue.ToString();
            Settings.Default.DPSWidgetTop = Int32.Parse(Settings.Default.Properties["DPSWidgetTop"].DefaultValue.ToString());
            Settings.Default.DPSWidgetLeft = Int32.Parse(Settings.Default.Properties["DPSWidgetLeft"].DefaultValue.ToString());
            Settings.Default.DPSWidgetHeight = Int32.Parse(Settings.Default.Properties["DPSWidgetHeight"].DefaultValue.ToString());
            Settings.Default.DPSWidgetWidth = Int32.Parse(Settings.Default.Properties["DPSWidgetWidth"].DefaultValue.ToString());
            Settings.Default.DPSVisibility = Settings.Default.Properties["DPSVisibility"].DefaultValue.ToString();
            Settings.Default.DPSWidgetSortDirection = Settings.Default.Properties["DPSWidgetSortDirection"].DefaultValue.ToString();
            Settings.Default.DPSWidgetSortProperty = Settings.Default.Properties["DPSWidgetSortProperty"].DefaultValue.ToString();
        }

        public void OpenDPSWidget()
        {
            Settings.Default.ShowDPSWidgetOnLoad = true;
            Widgets.Instance.ShowDPSWidget();
        }

        public void ResetDTPSWidget()
        {
            Settings.Default.DTPSWidgetUIScale = Settings.Default.Properties["DTPSWidgetUIScale"].DefaultValue.ToString();
            Settings.Default.DTPSWidgetTop = Int32.Parse(Settings.Default.Properties["DTPSWidgetTop"].DefaultValue.ToString());
            Settings.Default.DTPSWidgetLeft = Int32.Parse(Settings.Default.Properties["DTPSWidgetLeft"].DefaultValue.ToString());
            Settings.Default.DTPSWidgetHeight = Int32.Parse(Settings.Default.Properties["DTPSWidgetHeight"].DefaultValue.ToString());
            Settings.Default.DTPSWidgetWidth = Int32.Parse(Settings.Default.Properties["DTPSWidgetWidth"].DefaultValue.ToString());
            Settings.Default.DTPSVisibility = Settings.Default.Properties["DTPSVisibility"].DefaultValue.ToString();
            Settings.Default.DTPSWidgetSortDirection = Settings.Default.Properties["DTPSWidgetSortDirection"].DefaultValue.ToString();
            Settings.Default.DTPSWidgetSortProperty = Settings.Default.Properties["DTPSWidgetSortProperty"].DefaultValue.ToString();
        }

        public void OpenDTPSWidget()
        {
            Settings.Default.ShowDTPSWidgetOnLoad = true;
            Widgets.Instance.ShowDTPSWidget();
        }

        public void ResetHPSWidget()
        {
            Settings.Default.HPSWidgetUIScale = Settings.Default.Properties["HPSWidgetUIScale"].DefaultValue.ToString();
            Settings.Default.HPSWidgetTop = Int32.Parse(Settings.Default.Properties["HPSWidgetTop"].DefaultValue.ToString());
            Settings.Default.HPSWidgetLeft = Int32.Parse(Settings.Default.Properties["HPSWidgetLeft"].DefaultValue.ToString());
            Settings.Default.HPSWidgetHeight = Int32.Parse(Settings.Default.Properties["HPSWidgetHeight"].DefaultValue.ToString());
            Settings.Default.HPSWidgetWidth = Int32.Parse(Settings.Default.Properties["HPSWidgetWidth"].DefaultValue.ToString());
            Settings.Default.HPSVisibility = Settings.Default.Properties["HPSVisibility"].DefaultValue.ToString();
            Settings.Default.HPSWidgetSortDirection = Settings.Default.Properties["HPSWidgetSortDirection"].DefaultValue.ToString();
            Settings.Default.HPSWidgetSortProperty = Settings.Default.Properties["HPSWidgetSortProperty"].DefaultValue.ToString();
        }

        public void OpenHPSWidget()
        {
            Settings.Default.ShowHPSWidgetOnLoad = true;
            Widgets.Instance.ShowHPSWidget();
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
