// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ShellViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using FFXIVAPP.Plugin.Parse.Interop;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.Events;
    using FFXIVAPP.Plugin.Parse.Properties;
    using FFXIVAPP.Plugin.Parse.Windows;

    public sealed class ShellViewModel : INotifyPropertyChanged {
        private static Lazy<ShellViewModel> _instance = new Lazy<ShellViewModel>(() => new ShellViewModel());

        public ShellViewModel() {
            Initializer.LoadSettings();
            Initializer.LoadPlayerRegEx();
            Initializer.LoadMonsterRegEx();
            Initializer.EnsureLogsDirectory();
            Initializer.SetupWidgetTopMost();
            Settings.Default.PropertyChanged += DefaultOnPropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static ShellViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        internal static void Loaded(object sender, RoutedEventArgs e) {
            ShellView.View.Loaded -= Loaded;
        }

        private static void DefaultOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) {
            switch (propertyChangedEventArgs.PropertyName) {
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
                    try {
                        Settings.Default.DPSWidgetWidth = (int) (250 * double.Parse(Settings.Default.DPSWidgetUIScale));
                        Settings.Default.DPSWidgetHeight = (int) (450 * double.Parse(Settings.Default.DPSWidgetUIScale));
                    }
                    catch (Exception) {
                        Settings.Default.DPSWidgetWidth = 250;
                        Settings.Default.DPSWidgetHeight = 450;
                    }

                    break;
                case "HPSWidgetUIScale":
                    try {
                        Settings.Default.HPSWidgetWidth = (int) (250 * double.Parse(Settings.Default.HPSWidgetUIScale));
                        Settings.Default.HPSWidgetHeight = (int) (450 * double.Parse(Settings.Default.HPSWidgetUIScale));
                    }
                    catch (Exception) {
                        Settings.Default.HPSWidgetWidth = 250;
                        Settings.Default.HPSWidgetHeight = 450;
                    }

                    break;
                case "DTPSWidgetUIScale":
                    try {
                        Settings.Default.DTPSWidgetWidth = (int) (250 * double.Parse(Settings.Default.DTPSWidgetUIScale));
                        Settings.Default.DTPSWidgetHeight = (int) (450 * double.Parse(Settings.Default.DTPSWidgetUIScale));
                    }
                    catch (Exception) {
                        Settings.Default.DTPSWidgetWidth = 250;
                        Settings.Default.DTPSWidgetHeight = 450;
                    }

                    break;
                case "DPSWidgetSortDirection":
                case "DPSWidgetSortProperty":
                    try {
                        ListSortDirection direction = Settings.Default.DPSWidgetSortDirection == "Descending"
                                                          ? ListSortDirection.Descending
                                                          : ListSortDirection.Ascending;
                        var sortDescription = new SortDescription(Settings.Default.DPSWidgetSortProperty, direction);
                        DPSWidget.View.Party.Items.SortDescriptions.Clear();
                        DPSWidget.View.Party.Items.SortDescriptions.Add(sortDescription);
                    }
                    catch (Exception) {
                        DPSWidget.View.Party.Items.SortDescriptions.Clear();
                        DPSWidget.View.Party.Items.SortDescriptions.Add(new SortDescription("DPS", ListSortDirection.Descending));
                    }

                    DPSWidget.View.Party.Items.Refresh();
                    break;
                case "HPSWidgetSortDirection":
                case "HPSWidgetSortProperty":
                    try {
                        ListSortDirection direction = Settings.Default.HPSWidgetSortDirection == "Descending"
                                                          ? ListSortDirection.Descending
                                                          : ListSortDirection.Ascending;
                        var sortDescription = new SortDescription(Settings.Default.HPSWidgetSortProperty, direction);
                        HPSWidget.View.Party.Items.SortDescriptions.Clear();
                        HPSWidget.View.Party.Items.SortDescriptions.Add(sortDescription);
                    }
                    catch (Exception) {
                        HPSWidget.View.Party.Items.SortDescriptions.Clear();
                        HPSWidget.View.Party.Items.SortDescriptions.Add(new SortDescription("HPS", ListSortDirection.Descending));
                    }

                    HPSWidget.View.Party.Items.Refresh();
                    break;
                case "DTPSWidgetSortDirection":
                case "DTPSWidgetSortProperty":
                    try {
                        ListSortDirection direction = Settings.Default.DTPSWidgetSortDirection == "Descending"
                                                          ? ListSortDirection.Descending
                                                          : ListSortDirection.Ascending;
                        var sortDescription = new SortDescription(Settings.Default.DTPSWidgetSortProperty, direction);
                        DTPSWidget.View.Party.Items.SortDescriptions.Clear();
                        DTPSWidget.View.Party.Items.SortDescriptions.Add(sortDescription);
                    }
                    catch (Exception) {
                        DTPSWidget.View.Party.Items.SortDescriptions.Clear();
                        DTPSWidget.View.Party.Items.SortDescriptions.Add(new SortDescription("DTPS", ListSortDirection.Descending));
                    }

                    DTPSWidget.View.Party.Items.Refresh();
                    break;
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}