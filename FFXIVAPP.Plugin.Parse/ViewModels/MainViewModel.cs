// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MainViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;

    using FFXIVAPP.Common;
    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.RegularExpressions;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Common.ViewModelBase;
    using FFXIVAPP.Plugin.Parse.Helpers;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Models.History;
    using FFXIVAPP.Plugin.Parse.Models.StatGroups;
    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.Views;
    using FFXIVAPP.Plugin.Parse.Windows;

    using Microsoft.Win32;

    using NLog;

    using Constants = FFXIVAPP.Plugin.Parse.Constants;

    internal sealed class MainViewModel : INotifyPropertyChanged {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Lazy<MainViewModel> _instance = new Lazy<MainViewModel>(() => new MainViewModel());

        private bool _isCurrent;

        private bool _isHistory;

        private dynamic _monsterInfoSource;

        private dynamic _overallInfoSource;

        private ObservableCollection<ParseHistoryItem> _parseHistory;

        private dynamic _playerInfoSource;

        public MainViewModel() {
            this.IsCurrent = true;
            this.ShowLast20PlayerActionsCommand = new DelegateCommand<string>(this.ShowLast20PlayerActions);
            this.ShowLast20MonsterActionsCommand = new DelegateCommand<string>(this.ShowLast20MonsterActions);
            this.ShowLast20PlayerItemsUsedCommand = new DelegateCommand(this.ShowLast20PlayerItemsUsed);
            this.RefreshSelectedCommand = new DelegateCommand(this.RefreshSelected);
            this.ProcessSampleCommand = new DelegateCommand(this.ProcessSample);
            this.SwitchInfoViewSourceCommand = new DelegateCommand(this.SwitchInfoViewSource);
            this.SwitchInfoViewTypeCommand = new DelegateCommand(this.SwitchInfoViewType);
            this.ResetStatsCommand = new DelegateCommand(this.ResetStats);
            this.Convert2JsonCommand = new DelegateCommand(this.Convert2Json);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static MainViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public ICommand Convert2JsonCommand { get; private set; }

        public bool IsCurrent {
            get {
                return this._isCurrent;
            }

            set {
                this._isCurrent = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsHistory {
            get {
                return this._isHistory;
            }

            set {
                this._isHistory = value;
                this.RaisePropertyChanged();
            }
        }

        public dynamic MonsterInfoSource {
            get {
                return this._monsterInfoSource ?? ParseControl.Instance.Timeline.Monster;
            }

            set {
                this._monsterInfoSource = value;
                this.RaisePropertyChanged();
            }
        }

        public dynamic OverallInfoSource {
            get {
                return this._overallInfoSource ?? ParseControl.Instance.Timeline.Overall;
            }

            set {
                this._overallInfoSource = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<ParseHistoryItem> ParseHistory {
            get {
                return this._parseHistory ?? (this._parseHistory = new ObservableCollection<ParseHistoryItem> {
                    new ParseHistoryItem {
                        Name = "Current"
                    }
                });
            }

            set {
                if (this._parseHistory == null) {
                    this._parseHistory = new ObservableCollection<ParseHistoryItem> {
                        new ParseHistoryItem {
                            Name = "Current"
                        }
                    };
                }

                this._parseHistory = value;
                this.RaisePropertyChanged();
            }
        }

        public dynamic PlayerInfoSource {
            get {
                return this._playerInfoSource ?? ParseControl.Instance.Timeline.Party;
            }

            set {
                this._playerInfoSource = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand ProcessSampleCommand { get; private set; }

        public ICommand RefreshSelectedCommand { get; private set; }

        public ICommand ResetStatsCommand { get; private set; }

        public ICommand ShowLast20MonsterActionsCommand { get; private set; }

        public ICommand ShowLast20PlayerActionsCommand { get; private set; }

        public ICommand ShowLast20PlayerItemsUsedCommand { get; private set; }

        public ICommand SwitchInfoViewSourceCommand { get; private set; }

        public ICommand SwitchInfoViewTypeCommand { get; private set; }

        private void Convert2Json() {
            // var results = JsonHelper.ToJson.ConvertParse();
            // DispatcherHelper.Invoke(() => Clipboard.SetText(results));
        }

        private void ProcessSample() {
            var openFileDialog = new OpenFileDialog {
                InitialDirectory = Path.Combine(Common.Constants.LogsPath, "Parser"),
                Multiselect = false,
                Filter = "JSON Files (*.json)|*.json"
            };
            openFileDialog.FileOk += delegate {
                try {
                    var parse = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                    JsonHelper.ToParseControl.ConvertJson(openFileDialog.FileName, parse);
                }
                catch (Exception ex) {
                    var popupContent = new PopupContent {
                        Title = PluginViewModel.Instance.Locale["app_WarningMessage"],
                        Message = ex.Message
                    };
                    Plugin.PHost.PopupMessage(Plugin.PName, popupContent);
                }
            };
            openFileDialog.ShowDialog();
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        private void RefreshSelected() {
            DispatcherHelper.Invoke(
                delegate {
                    try {
                        var index = MainView.View.PlayerInfoListView.SelectedIndex;
                        MainView.View.PlayerInfoListView.SelectedIndex = -1;
                        MainView.View.PlayerInfoListView.SelectedIndex = index;
                    }
                    catch (Exception ex) {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }

                    try {
                        var index = MainView.View.MonsterInfoListView.SelectedIndex;
                        MainView.View.MonsterInfoListView.SelectedIndex = -1;
                        MainView.View.MonsterInfoListView.SelectedIndex = index;
                    }
                    catch (Exception ex) {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }
                });
        }

        /// <summary>
        /// </summary>
        private void ResetStats() {
            ParseControl.Instance.Reset();

            // var title = PluginViewModel.Instance.Locale["app_WarningMessage"];
            // var message = PluginViewModel.Instance.Locale["parse_ResetStatsMessage"];
            // MessageBoxHelper.ShowMessageAsync(title, message, () => ParseControl.Instance.Reset(), delegate { });
        }

        private void ShowLast20MonsterActions(string actionType) {
            var title = "UNKNOWN";
            List<ActionHistoryItem> source = new List<ActionHistoryItem>();
            dynamic monsters = null;
            dynamic monster = null;
            if (this.IsHistory) {
                monsters = ((HistoryGroup) Instance.MonsterInfoSource).Children;
            }

            if (this.IsCurrent) {
                monsters = ((StatGroup) Instance.MonsterInfoSource).Children;
            }

            if (this.IsHistory) {
                monster = ((List<HistoryGroup>) monsters).Where(p => p != null).Where(p => string.Equals(p.Name, MainView.View.SelectedMonsterName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer)).Cast<HistoryGroup>().FirstOrDefault();
                if (monster == null) {
                    return;
                }

                title = monster.Name;
            }

            if (this.IsCurrent) {
                monster = ((List<StatGroup>) monsters).Where(p => p != null).Where(p => string.Equals(p.Name, MainView.View.SelectedMonsterName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer)).Cast<Monster>().FirstOrDefault();
                if (monster == null) {
                    return;
                }

                title = monster.Name;
            }

            switch (actionType) {
                case "Damage":
                    foreach (dynamic action in monster.Last20DamageActions) {
                        source.Add(
                            new ActionHistoryItem {
                                Action = action.Line.Action,
                                Amount = action.Line.Amount,
                                Critical = action.Line.Crit.ToString(),
                                Source = action.Line.Source,
                                Target = action.Line.Target,
                                TimeStamp = action.TimeStamp
                            });
                    }

                    break;
                case "DamageTaken":
                    foreach (dynamic action in monster.Last20DamageTakenActions) {
                        source.Add(
                            new ActionHistoryItem {
                                Action = action.Line.Action,
                                Amount = action.Line.Amount,
                                Critical = action.Line.Crit.ToString(),
                                Source = action.Line.Source,
                                Target = action.Line.Target,
                                TimeStamp = action.TimeStamp
                            });
                    }

                    break;
                case "Healing":
                    foreach (dynamic action in monster.Last20HealingActions) {
                        source.Add(
                            new ActionHistoryItem {
                                Action = action.Line.Action,
                                Amount = action.Line.Amount,
                                Critical = action.Line.Crit.ToString(),
                                Source = action.Line.Source,
                                Target = action.Line.Target,
                                TimeStamp = action.TimeStamp
                            });
                    }

                    break;
            }

            if (!source.Any()) {
                return;
            }

            var x = new xMetroWindowDataGrid {
                Title = title,
                xMetroWindowDG = {
                    ItemsSource = source
                }
            };
            x.Show();
        }

        private void ShowLast20PlayerActions(string actionType) {
            var title = "UNKNOWN";
            List<ActionHistoryItem> source = new List<ActionHistoryItem>();
            dynamic players = null;
            dynamic player = null;
            if (this.IsHistory) {
                players = ((HistoryGroup) Instance.PlayerInfoSource).Children;
            }

            if (this.IsCurrent) {
                players = ((StatGroup) Instance.PlayerInfoSource).Children;
            }

            if (this.IsHistory) {
                player = ((List<HistoryGroup>) players).Where(p => p != null).Where(p => string.Equals(p.Name, MainView.View.SelectedPlayerName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer)).Cast<HistoryGroup>().FirstOrDefault();
                if (player == null) {
                    return;
                }

                title = player.Name;
            }

            if (this.IsCurrent) {
                player = ((List<StatGroup>) players).Where(p => p != null).Where(p => string.Equals(p.Name, MainView.View.SelectedPlayerName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer)).Cast<Player>().FirstOrDefault();
                if (player == null) {
                    return;
                }

                title = player.Name;
            }

            switch (actionType) {
                case "Damage":
                    foreach (dynamic action in player.Last20DamageActions) {
                        source.Add(
                            new ActionHistoryItem {
                                Action = action.Line.Action,
                                Amount = action.Line.Amount,
                                Critical = action.Line.Crit.ToString(),
                                Source = action.Line.Source,
                                Target = action.Line.Target,
                                TimeStamp = action.TimeStamp
                            });
                    }

                    break;
                case "DamageTaken":
                    foreach (dynamic action in player.Last20DamageTakenActions) {
                        source.Add(
                            new ActionHistoryItem {
                                Action = action.Line.Action,
                                Amount = action.Line.Amount,
                                Critical = action.Line.Crit.ToString(),
                                Source = action.Line.Source,
                                Target = action.Line.Target,
                                TimeStamp = action.TimeStamp
                            });
                    }

                    break;
                case "Healing":
                    foreach (dynamic action in player.Last20HealingActions) {
                        source.Add(
                            new ActionHistoryItem {
                                Action = action.Line.Action,
                                Amount = action.Line.Amount,
                                Critical = action.Line.Crit.ToString(),
                                Source = action.Line.Source,
                                Target = action.Line.Target,
                                TimeStamp = action.TimeStamp
                            });
                    }

                    break;
            }

            if (!source.Any()) {
                return;
            }

            var x = new xMetroWindowDataGrid {
                Title = title,
                xMetroWindowDG = {
                    ItemsSource = source
                }
            };
            x.Show();
        }

        private void ShowLast20PlayerItemsUsed() {
            var title = "UNKNOWN";
            List<ItemUsedHistoryItem> source = new List<ItemUsedHistoryItem>();
            dynamic players = null;
            dynamic player = null;
            if (this.IsHistory) {
                players = ((HistoryGroup) Instance.PlayerInfoSource).Children;
            }

            if (this.IsCurrent) {
                players = ((StatGroup) Instance.PlayerInfoSource).Children;
            }

            if (this.IsHistory) {
                player = ((List<HistoryGroup>) players).Where(p => p != null).Where(p => string.Equals(p.Name, MainView.View.SelectedPlayerName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer)).Cast<HistoryGroup>().FirstOrDefault();
                if (player == null) {
                    return;
                }

                title = player.Name;
            }

            if (this.IsCurrent) {
                player = ((List<StatGroup>) players).Where(p => p != null).Where(p => string.Equals(p.Name, MainView.View.SelectedPlayerName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer)).Cast<Player>().FirstOrDefault();
                if (player == null) {
                    return;
                }

                title = player.Name;
            }

            foreach (dynamic item in player.Last20Items) {
                source.Add(
                    new ItemUsedHistoryItem {
                        Item = Regex.Replace(item.Line.Action, @"\[Hq\]", "[HQ]", SharedRegEx.DefaultOptions),
                        TimeStamp = item.TimeStamp
                    });
            }

            if (!source.Any()) {
                return;
            }

            var x = new xMetroWindowDataGrid {
                Title = title,
                xMetroWindowDG = {
                    ItemsSource = source
                }
            };
            x.Show();
        }

        private void SwitchInfoViewSource() {
            try {
                var index = MainView.View?.InfoViewSource.SelectedIndex ?? 0;
                switch (index) {
                    case 0:
                        this.PlayerInfoSource = null;
                        this.MonsterInfoSource = null;
                        this.OverallInfoSource = null;
                        this.IsCurrent = true;
                        this.IsHistory = false;
                        break;
                    default:
                        this.PlayerInfoSource = this.ParseHistory[index].HistoryControl.Timeline.Party;
                        this.MonsterInfoSource = this.ParseHistory[index].HistoryControl.Timeline.Monster;
                        this.OverallInfoSource = this.ParseHistory[index].HistoryControl.Timeline.Overall;
                        this.IsCurrent = false;
                        this.IsHistory = true;
                        break;
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        private void SwitchInfoViewType() {
            try {
                var index = MainView.View.InfoViewType.SelectedIndex;
                switch (MainView.View.InfoViewType.SelectedIndex) {
                    case 0:
                        MainView.View.PlayerInfoListView.Visibility = Visibility.Collapsed;
                        MainView.View.MonsterInfoListView.Visibility = Visibility.Collapsed;
                        MainView.View.RefreshSelectedButton.Visibility = Visibility.Collapsed;
                        break;
                    case 1:
                        MainView.View.PlayerInfoListView.Visibility = Visibility.Visible;
                        MainView.View.MonsterInfoListView.Visibility = Visibility.Collapsed;
                        MainView.View.RefreshSelectedButton.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        MainView.View.PlayerInfoListView.Visibility = Visibility.Collapsed;
                        MainView.View.MonsterInfoListView.Visibility = Visibility.Collapsed;
                        MainView.View.RefreshSelectedButton.Visibility = Visibility.Collapsed;
                        break;
                    case 3:
                        MainView.View.PlayerInfoListView.Visibility = Visibility.Collapsed;
                        MainView.View.MonsterInfoListView.Visibility = Visibility.Visible;
                        MainView.View.RefreshSelectedButton.Visibility = Visibility.Visible;
                        break;
                    case 4:
                        MainView.View.PlayerInfoListView.Visibility = Visibility.Collapsed;
                        MainView.View.MonsterInfoListView.Visibility = Visibility.Collapsed;
                        MainView.View.RefreshSelectedButton.Visibility = Visibility.Collapsed;
                        break;
                }

                MainView.View.InfoViewResults.SelectedIndex = index;
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }
    }
}