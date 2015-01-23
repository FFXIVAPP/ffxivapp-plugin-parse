// FFXIVAPP.Plugin.Parse
// SettingsViewModel.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFXIVAPP.Common.ViewModelBase;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.ViewModels
{
    internal sealed class SettingsViewModel : INotifyPropertyChanged
    {
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

        public SettingsViewModel()
        {
            ResetDPSWidgetCommand = new DelegateCommand(ResetDPSWidget);
            OpenDPSWidgetCommand = new DelegateCommand(OpenDPSWidget);
            ResetDTPSWidgetCommand = new DelegateCommand(ResetDTPSWidget);
            OpenDTPSWidgetCommand = new DelegateCommand(OpenDTPSWidget);
            ResetHPSWidgetCommand = new DelegateCommand(ResetHPSWidget);
            OpenHPSWidgetCommand = new DelegateCommand(OpenHPSWidget);
        }

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
