// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidgetTopMostHelper.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   WidgetTopMostHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Helpers {
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Timers;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Threading;

    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.RegularExpressions;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Parse.Interop;
    using FFXIVAPP.Plugin.Parse.Properties;

    using NLog;

    public static class WidgetTopMostHelper {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static WinAPI.WinEventDelegate _delegate;

        private static WindowInteropHelper _dpsWidgetInteropHelper;

        private static WindowInteropHelper _dtpsWidgetInteropHelper;

        private static WindowInteropHelper _hpsWidgetInteropHelper;

        private static IntPtr _mainHandleHook;

        private static WindowInteropHelper DPSWidgetInteropHelper {
            get {
                return _dpsWidgetInteropHelper ?? (_dpsWidgetInteropHelper = new WindowInteropHelper(Widgets.Instance.DPSWidget));
            }
        }

        private static WindowInteropHelper DTPSWidgetInteropHelper {
            get {
                return _dtpsWidgetInteropHelper ?? (_dtpsWidgetInteropHelper = new WindowInteropHelper(Widgets.Instance.DTPSWidget));
            }
        }

        private static WindowInteropHelper HPSWidgetInteropHelper {
            get {
                return _hpsWidgetInteropHelper ?? (_hpsWidgetInteropHelper = new WindowInteropHelper(Widgets.Instance.HPSWidget));
            }
        }

        private static Timer SetWindowTimer { get; set; }

        public static void HookWidgetTopMost() {
            try {
                _delegate = BringWidgetsIntoFocus;
                _mainHandleHook = WinAPI.SetWinEventHook(WinAPI.EVENT_SYSTEM_FOREGROUND, WinAPI.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, _delegate, 0, 0, WinAPI.WINEVENT_OUTOFCONTEXT);
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            SetWindowTimer = new Timer(1000);
            SetWindowTimer.Elapsed += SetWindowTimerOnElapsed;
            SetWindowTimer.Start();
        }

        private static void BringWidgetsIntoFocus(IntPtr hwineventhook, uint eventtype, IntPtr hwnd, int idobject, int idchild, uint dweventthread, uint dwmseventtime) {
            BringWidgetsIntoFocus(true);
        }

        private static void BringWidgetsIntoFocus(bool force = false) {
            try {
                IntPtr handle = WinAPI.GetForegroundWindow();
                var activeTitle = WinAPI.GetActiveWindowTitle();

                var stayOnTop = Application.Current.Windows.OfType<Window>().Any(w => w.Title == activeTitle) || Regex.IsMatch(activeTitle.ToUpper(), @"^(FINAL FANTASY |最终幻想)XIV", SharedRegEx.DefaultOptions);

                // If any of the widgets are focused, don't try to hide any of them, or it'll prevent us from moving/closing them
                if (handle == DPSWidgetInteropHelper.Handle) {
                    return;
                }

                if (handle == DTPSWidgetInteropHelper.Handle) {
                    return;
                }

                if (handle == HPSWidgetInteropHelper.Handle) {
                    return;
                }

                if (Settings.Default.ShowDPSWidgetOnLoad) {
                    ToggleTopMost(Widgets.Instance.DPSWidget, stayOnTop, force);
                }

                if (Settings.Default.ShowDTPSWidgetOnLoad) {
                    ToggleTopMost(Widgets.Instance.DTPSWidget, stayOnTop, force);
                }

                if (Settings.Default.ShowHPSWidgetOnLoad) {
                    ToggleTopMost(Widgets.Instance.HPSWidget, stayOnTop, force);
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        private static void SetWindowTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs) {
            DispatcherHelper.Invoke(() => BringWidgetsIntoFocus(), DispatcherPriority.Normal);
        }

        /// <summary>
        /// </summary>
        /// <param name="window"></param>
        /// <param name="stayOnTop"></param>
        /// <param name="force"></param>
        private static void ToggleTopMost(Window window, bool stayOnTop, bool force) {
            if (window.Topmost && stayOnTop && !force) {
                return;
            }

            window.Topmost = false;
            if (!stayOnTop) {
                if (window.IsVisible) {
                    window.Hide();
                }

                return;
            }

            window.Topmost = true;
            if (!window.IsVisible) {
                window.Show();
            }
        }
    }
}