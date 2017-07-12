// FFXIVAPP.Plugin.Parse ~ ShellViewModel.cs
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using FFXIVAPP.Plugin.Parse.Interop;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Properties;
using FFXIVAPP.Plugin.Parse.Windows;

namespace FFXIVAPP.Plugin.Parse
{
    public sealed class ShellViewModel : INotifyPropertyChanged
    {
        public ShellViewModel()
        {
            Initializer.LoadSettings();
            Initializer.LoadPlayerRegEx();
            Initializer.LoadMonsterRegEx();
            Initializer.EnsureLogsDirectory();
            Initializer.SetupWidgetTopMost();
            Settings.Default.PropertyChanged += DefaultOnPropertyChanged;
        }

        internal static void Loaded(object sender, RoutedEventArgs e)
        {
            ShellView.View.Loaded -= Loaded;
        }

        private static void DefaultOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "ParseYou":
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.You);
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.Pet);
                    break;
                case "ParseParty":
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.Party);
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.PetParty);
                    break;
                case "ParseAlliance":
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.Alliance);
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.PetAlliance);
                    break;
                case "ParseOther":
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.Other);
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.PetOther);
                    break;
                case "WidgetClickThroughEnabled":
                    WinAPI.ToggleClickThrough(Widgets.Instance.DPSWidget);
                    WinAPI.ToggleClickThrough(Widgets.Instance.DTPSWidget);
                    WinAPI.ToggleClickThrough(Widgets.Instance.HPSWidget);
                    break;
                case "DPSWidgetUIScale":
                    try
                    {
                        Settings.Default.DPSWidgetWidth = (int) (250 * Double.Parse(Settings.Default.DPSWidgetUIScale));
                        Settings.Default.DPSWidgetHeight = (int) (450 * Double.Parse(Settings.Default.DPSWidgetUIScale));
                    }
                    catch (Exception)
                    {
                        Settings.Default.DPSWidgetWidth = 250;
                        Settings.Default.DPSWidgetHeight = 450;
                    }
                    break;
                case "HPSWidgetUIScale":
                    try
                    {
                        Settings.Default.HPSWidgetWidth = (int) (250 * Double.Parse(Settings.Default.HPSWidgetUIScale));
                        Settings.Default.HPSWidgetHeight = (int) (450 * Double.Parse(Settings.Default.HPSWidgetUIScale));
                    }
                    catch (Exception)
                    {
                        Settings.Default.HPSWidgetWidth = 250;
                        Settings.Default.HPSWidgetHeight = 450;
                    }
                    break;
                case "DTPSWidgetUIScale":
                    try
                    {
                        Settings.Default.DTPSWidgetWidth = (int) (250 * Double.Parse(Settings.Default.DTPSWidgetUIScale));
                        Settings.Default.DTPSWidgetHeight = (int) (450 * Double.Parse(Settings.Default.DTPSWidgetUIScale));
                    }
                    catch (Exception)
                    {
                        Settings.Default.DTPSWidgetWidth = 250;
                        Settings.Default.DTPSWidgetHeight = 450;
                    }
                    break;
                case "DPSWidgetSortDirection":
                case "DPSWidgetSortProperty":
                    try
                    {
                        var direction = Settings.Default.DPSWidgetSortDirection == "Descending" ? ListSortDirection.Descending : ListSortDirection.Ascending;
                        var sortDescription = new SortDescription(Settings.Default.DPSWidgetSortProperty, direction);
                        DPSWidget.View.Party.Items.SortDescriptions.Clear();
                        DPSWidget.View.Party.Items.SortDescriptions.Add(sortDescription);
                    }
                    catch (Exception)
                    {
                        DPSWidget.View.Party.Items.SortDescriptions.Clear();
                        DPSWidget.View.Party.Items.SortDescriptions.Add(new SortDescription("DPS", ListSortDirection.Descending));
                    }
                    DPSWidget.View.Party.Items.Refresh();
                    break;
                case "HPSWidgetSortDirection":
                case "HPSWidgetSortProperty":
                    try
                    {
                        var direction = Settings.Default.HPSWidgetSortDirection == "Descending" ? ListSortDirection.Descending : ListSortDirection.Ascending;
                        var sortDescription = new SortDescription(Settings.Default.HPSWidgetSortProperty, direction);
                        HPSWidget.View.Party.Items.SortDescriptions.Clear();
                        HPSWidget.View.Party.Items.SortDescriptions.Add(sortDescription);
                    }
                    catch (Exception)
                    {
                        HPSWidget.View.Party.Items.SortDescriptions.Clear();
                        HPSWidget.View.Party.Items.SortDescriptions.Add(new SortDescription("HPS", ListSortDirection.Descending));
                    }
                    HPSWidget.View.Party.Items.Refresh();
                    break;
                case "DTPSWidgetSortDirection":
                case "DTPSWidgetSortProperty":
                    try
                    {
                        var direction = Settings.Default.DTPSWidgetSortDirection == "Descending" ? ListSortDirection.Descending : ListSortDirection.Ascending;
                        var sortDescription = new SortDescription(Settings.Default.DTPSWidgetSortProperty, direction);
                        DTPSWidget.View.Party.Items.SortDescriptions.Clear();
                        DTPSWidget.View.Party.Items.SortDescriptions.Add(sortDescription);
                    }
                    catch (Exception)
                    {
                        DTPSWidget.View.Party.Items.SortDescriptions.Clear();
                        DTPSWidget.View.Party.Items.SortDescriptions.Add(new SortDescription("DTPS", ListSortDirection.Descending));
                    }
                    DTPSWidget.View.Party.Items.Refresh();
                    break;
            }
        }

        #region Property Bindings

        private static Lazy<ShellViewModel> _instance = new Lazy<ShellViewModel>(() => new ShellViewModel());

        public static ShellViewModel Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region Declarations

        #endregion

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

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
