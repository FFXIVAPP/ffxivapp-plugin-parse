// FFXIVAPP.Plugin.Parse ~ DPSWidget.xaml.cs
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

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using FFXIVAPP.Plugin.Parse.Interop;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.Windows
{
    /// <summary>
    ///     Interaction logic for DPSWidget.xaml
    /// </summary>
    public partial class DPSWidget
    {
        public static DPSWidget View;

        public DPSWidget()
        {
            View = this;
            InitializeComponent();
            View.SourceInitialized += delegate { WinAPI.ToggleClickThrough(this); };
        }

        private void TitleBar_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void WidgetClose_OnClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.ShowDPSWidgetOnLoad = false;
            Close();
        }

        private void Widget_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
