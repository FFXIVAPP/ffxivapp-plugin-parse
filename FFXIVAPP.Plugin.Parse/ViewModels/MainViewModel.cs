// FFXIVAPP.Plugin.Parse ~ MainViewModel.cs
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

namespace FFXIVAPP.Plugin.Parse.ViewModels
{
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public MainViewModel()
        {
            IsCurrent = true;
            ShowLast20PlayerActionsCommand = new DelegateCommand<string>(ShowLast20PlayerActions);
            ShowLast20MonsterActionsCommand = new DelegateCommand<string>(ShowLast20MonsterActions);
            ShowLast20PlayerItemsUsedCommand = new DelegateCommand(ShowLast20PlayerItemsUsed);
            RefreshSelectedCommand = new DelegateCommand(RefreshSelected);
            ProcessSampleCommand = new DelegateCommand(ProcessSample);
            SwitchInfoViewSourceCommand = new DelegateCommand(SwitchInfoViewSource);
            SwitchInfoViewTypeCommand = new DelegateCommand(SwitchInfoViewType);
            ResetStatsCommand = new DelegateCommand(ResetStats);
            Convert2JsonCommand = new DelegateCommand(Convert2Json);
        }

        #region Property Bindings

        private static Lazy<MainViewModel> _instance = new Lazy<MainViewModel>(() => new MainViewModel());
        private bool _isCurrent;
        private bool _isHistory;
        private dynamic _monsterInfoSource;
        private dynamic _overallInfoSource;
        private ObservableCollection<ParseHistoryItem> _parseHistory;
        private dynamic _playerInfoSource;

        public static MainViewModel Instance
        {
            get { return _instance.Value; }
        }

        public ObservableCollection<ParseHistoryItem> ParseHistory
        {
            get
            {
                return _parseHistory ?? (_parseHistory = new ObservableCollection<ParseHistoryItem>
                {
                    new ParseHistoryItem
                    {
                        Name = "Current"
                    }
                });
            }
            set
            {
                if (_parseHistory == null)
                {
                    _parseHistory = new ObservableCollection<ParseHistoryItem>
                    {
                        new ParseHistoryItem
                        {
                            Name = "Current"
                        }
                    };
                }
                _parseHistory = value;
                RaisePropertyChanged();
            }
        }

        public dynamic PlayerInfoSource
        {
            get { return _playerInfoSource ?? ParseControl.Instance.Timeline.Party; }
            set
            {
                _playerInfoSource = value;
                RaisePropertyChanged();
            }
        }

        public dynamic MonsterInfoSource
        {
            get { return _monsterInfoSource ?? ParseControl.Instance.Timeline.Monster; }
            set
            {
                _monsterInfoSource = value;
                RaisePropertyChanged();
            }
        }

        public dynamic OverallInfoSource
        {
            get { return _overallInfoSource ?? ParseControl.Instance.Timeline.Overall; }
            set
            {
                _overallInfoSource = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCurrent
        {
            get { return _isCurrent; }
            set
            {
                _isCurrent = value;
                RaisePropertyChanged();
            }
        }

        public bool IsHistory
        {
            get { return _isHistory; }
            set
            {
                _isHistory = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        public ICommand ShowLast20PlayerActionsCommand { get; private set; }
        public ICommand ShowLast20MonsterActionsCommand { get; private set; }
        public ICommand ShowLast20PlayerItemsUsedCommand { get; private set; }
        public ICommand RefreshSelectedCommand { get; private set; }
        public ICommand ProcessSampleCommand { get; private set; }
        public ICommand SwitchInfoViewSourceCommand { get; private set; }
        public ICommand SwitchInfoViewTypeCommand { get; private set; }
        public ICommand ResetStatsCommand { get; private set; }
        public ICommand Convert2JsonCommand { get; private set; }

        #endregion

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        private void ShowLast20PlayerActions(string actionType)
        {
            var title = "UNKNOWN";
            var source = new List<ActionHistoryItem>();
            dynamic players = null;
            dynamic player = null;
            if (IsHistory)
            {
                players = ((HistoryGroup) Instance.PlayerInfoSource).Children;
            }
            if (IsCurrent)
            {
                players = ((StatGroup) Instance.PlayerInfoSource).Children;
            }
            if (IsHistory)
            {
                player = ((List<HistoryGroup>) players).Where(p => p != null)
                                                       .Where(p => String.Equals(p.Name, MainView.View.SelectedPlayerName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer))
                                                       .Cast<HistoryGroup>()
                                                       .FirstOrDefault();
                if (player == null)
                {
                    return;
                }
                title = player.Name;
            }
            if (IsCurrent)
            {
                player = ((List<StatGroup>) players).Where(p => p != null)
                                                    .Where(p => String.Equals(p.Name, MainView.View.SelectedPlayerName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer))
                                                    .Cast<Player>()
                                                    .FirstOrDefault();
                if (player == null)
                {
                    return;
                }
                title = player.Name;
            }
            switch (actionType)
            {
                case "Damage":
                    foreach (var action in player.Last20DamageActions)
                    {
                        source.Add(new ActionHistoryItem
                        {
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
                    foreach (var action in player.Last20DamageTakenActions)
                    {
                        source.Add(new ActionHistoryItem
                        {
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
                    foreach (var action in player.Last20HealingActions)
                    {
                        source.Add(new ActionHistoryItem
                        {
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
            if (!source.Any())
            {
                return;
            }
            var x = new xMetroWindowDataGrid
            {
                Title = title,
                xMetroWindowDG =
                {
                    ItemsSource = source
                }
            };
            x.Show();
        }

        private void ShowLast20MonsterActions(string actionType)
        {
            var title = "UNKNOWN";
            var source = new List<ActionHistoryItem>();
            dynamic monsters = null;
            dynamic monster = null;
            if (IsHistory)
            {
                monsters = ((HistoryGroup) Instance.MonsterInfoSource).Children;
            }
            if (IsCurrent)
            {
                monsters = ((StatGroup) Instance.MonsterInfoSource).Children;
            }
            if (IsHistory)
            {
                monster = ((List<HistoryGroup>) monsters).Where(p => p != null)
                                                         .Where(p => String.Equals(p.Name, MainView.View.SelectedMonsterName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer))
                                                         .Cast<HistoryGroup>()
                                                         .FirstOrDefault();
                if (monster == null)
                {
                    return;
                }
                title = monster.Name;
            }
            if (IsCurrent)
            {
                monster = ((List<StatGroup>) monsters).Where(p => p != null)
                                                      .Where(p => String.Equals(p.Name, MainView.View.SelectedMonsterName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer))
                                                      .Cast<Monster>()
                                                      .FirstOrDefault();
                if (monster == null)
                {
                    return;
                }
                title = monster.Name;
            }
            switch (actionType)
            {
                case "Damage":
                    foreach (var action in monster.Last20DamageActions)
                    {
                        source.Add(new ActionHistoryItem
                        {
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
                    foreach (var action in monster.Last20DamageTakenActions)
                    {
                        source.Add(new ActionHistoryItem
                        {
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
                    foreach (var action in monster.Last20HealingActions)
                    {
                        source.Add(new ActionHistoryItem
                        {
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
            if (!source.Any())
            {
                return;
            }
            var x = new xMetroWindowDataGrid
            {
                Title = title,
                xMetroWindowDG =
                {
                    ItemsSource = source
                }
            };
            x.Show();
        }

        private void ShowLast20PlayerItemsUsed()
        {
            var title = "UNKNOWN";
            var source = new List<ItemUsedHistoryItem>();
            dynamic players = null;
            dynamic player = null;
            if (IsHistory)
            {
                players = ((HistoryGroup) Instance.PlayerInfoSource).Children;
            }
            if (IsCurrent)
            {
                players = ((StatGroup) Instance.PlayerInfoSource).Children;
            }
            if (IsHistory)
            {
                player = ((List<HistoryGroup>) players).Where(p => p != null)
                                                       .Where(p => String.Equals(p.Name, MainView.View.SelectedPlayerName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer))
                                                       .Cast<HistoryGroup>()
                                                       .FirstOrDefault();
                if (player == null)
                {
                    return;
                }
                title = player.Name;
            }
            if (IsCurrent)
            {
                player = ((List<StatGroup>) players).Where(p => p != null)
                                                    .Where(p => String.Equals(p.Name, MainView.View.SelectedPlayerName.Text.ToString(CultureInfo.InvariantCulture), Constants.InvariantComparer))
                                                    .Cast<Player>()
                                                    .FirstOrDefault();
                if (player == null)
                {
                    return;
                }
                title = player.Name;
            }
            foreach (var item in player.Last20Items)
            {
                source.Add(new ItemUsedHistoryItem
                {
                    Item = Regex.Replace(item.Line.Action, @"\[Hq\]", "[HQ]", SharedRegEx.DefaultOptions),
                    TimeStamp = item.TimeStamp
                });
            }
            if (!source.Any())
            {
                return;
            }
            var x = new xMetroWindowDataGrid
            {
                Title = title,
                xMetroWindowDG =
                {
                    ItemsSource = source
                }
            };
            x.Show();
        }

        private void RefreshSelected()
        {
            DispatcherHelper.Invoke(delegate
            {
                try
                {
                    var index = MainView.View.PlayerInfoListView.SelectedIndex;
                    MainView.View.PlayerInfoListView.SelectedIndex = -1;
                    MainView.View.PlayerInfoListView.SelectedIndex = index;
                }
                catch (Exception ex)
                {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
                try
                {
                    var index = MainView.View.MonsterInfoListView.SelectedIndex;
                    MainView.View.MonsterInfoListView.SelectedIndex = -1;
                    MainView.View.MonsterInfoListView.SelectedIndex = index;
                }
                catch (Exception ex)
                {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
            });
        }

        private void ProcessSample()
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Path.Combine(Common.Constants.LogsPath, "Parser"),
                Multiselect = false,
                Filter = "JSON Files (*.json)|*.json"
            };
            openFileDialog.FileOk += delegate
            {
                try
                {
                    var parse = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                    JsonHelper.ToParseControl.ConvertJson(openFileDialog.FileName, parse);
                }
                catch (Exception ex)
                {
                    var popupContent = new PopupContent
                    {
                        Title = PluginViewModel.Instance.Locale["app_WarningMessage"],
                        Message = ex.Message
                    };
                    Plugin.PHost.PopupMessage(Plugin.PName, popupContent);
                }
            };
            openFileDialog.ShowDialog();
        }

        private void SwitchInfoViewSource()
        {
            try
            {
                var index = MainView.View.InfoViewSource.SelectedIndex;
                switch (index)
                {
                    case 0:
                        PlayerInfoSource = null;
                        MonsterInfoSource = null;
                        OverallInfoSource = null;
                        IsCurrent = true;
                        IsHistory = false;
                        break;
                    default:
                        PlayerInfoSource = ParseHistory[index]
                            .HistoryControl.Timeline.Party;
                        MonsterInfoSource = ParseHistory[index]
                            .HistoryControl.Timeline.Monster;
                        OverallInfoSource = ParseHistory[index]
                            .HistoryControl.Timeline.Overall;
                        IsCurrent = false;
                        IsHistory = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        private void SwitchInfoViewType()
        {
            try
            {
                var index = MainView.View.InfoViewType.SelectedIndex;
                switch (MainView.View.InfoViewType.SelectedIndex)
                {
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
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        /// <summary>
        /// </summary>
        private void ResetStats()
        {
            ParseControl.Instance.Reset();
            //var title = PluginViewModel.Instance.Locale["app_WarningMessage"];
            //var message = PluginViewModel.Instance.Locale["parse_ResetStatsMessage"];
            //MessageBoxHelper.ShowMessageAsync(title, message, () => ParseControl.Instance.Reset(), delegate { });
        }

        private void Convert2Json()
        {
            //var results = JsonHelper.ToJson.ConvertParse();
            //DispatcherHelper.Invoke(() => Clipboard.SetText(results));
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
