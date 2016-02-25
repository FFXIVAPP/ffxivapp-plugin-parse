// FFXIVAPP.Plugin.Parse ~ WidgetTopMostHelper.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Plugin.Parse.Interop;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class WidgetTopMostHelper
    {
        private static WinAPI.WinEventDelegate _delegate;
        private static IntPtr _mainHandleHook;
        private static WindowInteropHelper _dpsWidgetInteropHelper;
        private static WindowInteropHelper _dtpsWidgetInteropHelper;
        private static WindowInteropHelper _hpsWidgetInteropHelper;

        private static WindowInteropHelper DPSWidgetInteropHelper
        {
            get { return _dpsWidgetInteropHelper ?? (_dpsWidgetInteropHelper = new WindowInteropHelper(Widgets.Instance.DPSWidget)); }
        }

        private static WindowInteropHelper DTPSWidgetInteropHelper
        {
            get { return _dtpsWidgetInteropHelper ?? (_dtpsWidgetInteropHelper = new WindowInteropHelper(Widgets.Instance.DTPSWidget)); }
        }

        private static WindowInteropHelper HPSWidgetInteropHelper
        {
            get { return _hpsWidgetInteropHelper ?? (_hpsWidgetInteropHelper = new WindowInteropHelper(Widgets.Instance.HPSWidget)); }
        }

        private static Timer SetWindowTimer { get; set; }

        public static void HookWidgetTopMost()
        {
            try
            {
                _delegate = BringWidgetsIntoFocus;
                _mainHandleHook = WinAPI.SetWinEventHook(WinAPI.EVENT_SYSTEM_FOREGROUND, WinAPI.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, _delegate, 0, 0, WinAPI.WINEVENT_OUTOFCONTEXT);
            }
            catch (Exception e)
            {
            }
            SetWindowTimer = new Timer(1000);
            SetWindowTimer.Elapsed += SetWindowTimerOnElapsed;
            SetWindowTimer.Start();
        }

        private static void SetWindowTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            DispatcherHelper.Invoke(() => BringWidgetsIntoFocus(), DispatcherPriority.Normal);
        }

        private static void BringWidgetsIntoFocus(IntPtr hwineventhook, uint eventtype, IntPtr hwnd, int idobject, int idchild, uint dweventthread, uint dwmseventtime)
        {
            BringWidgetsIntoFocus(true);
        }

        private static void BringWidgetsIntoFocus(bool force = false)
        {
            try
            {
                var handle = WinAPI.GetForegroundWindow();
                var activeTitle = WinAPI.GetActiveWindowTitle();

                var stayOnTop = Application.Current.Windows.OfType<Window>()
                                           .Any(w => w.Title == activeTitle) || Regex.IsMatch(activeTitle.ToUpper(), @"^(FINAL FANTASY |最终幻想)XIV", SharedRegEx.DefaultOptions);

                // If any of the widgets are focused, don't try to hide any of them, or it'll prevent us from moving/closing them
                if (handle == DPSWidgetInteropHelper.Handle)
                {
                    return;
                }
                if (handle == DTPSWidgetInteropHelper.Handle)
                {
                    return;
                }
                if (handle == HPSWidgetInteropHelper.Handle)
                {
                    return;
                }
                if (Settings.Default.ShowDPSWidgetOnLoad)
                {
                    ToggleTopMost(Widgets.Instance.DPSWidget, stayOnTop, force);
                }
                if (Settings.Default.ShowDTPSWidgetOnLoad)
                {
                    ToggleTopMost(Widgets.Instance.DTPSWidget, stayOnTop, force);
                }
                if (Settings.Default.ShowHPSWidgetOnLoad)
                {
                    ToggleTopMost(Widgets.Instance.HPSWidget, stayOnTop, force);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="window"></param>
        /// <param name="stayOnTop"></param>
        /// <param name="force"></param>
        private static void ToggleTopMost(Window window, bool stayOnTop, bool force)
        {
            if (window.Topmost && stayOnTop && !force)
            {
                return;
            }
            window.Topmost = false;
            if (!stayOnTop)
            {
                if (window.IsVisible)
                {
                    window.Hide();
                }
                return;
            }
            window.Topmost = true;
            if (!window.IsVisible)
            {
                window.Show();
            }
        }
    }
}
