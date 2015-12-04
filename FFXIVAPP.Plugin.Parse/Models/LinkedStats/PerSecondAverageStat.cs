// FFXIVAPP.Plugin.Parse ~ PerSecondAverageStat.cs
// 
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
using System.Timers;
using FFXIVAPP.Plugin.Parse.Models.Stats;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public class PerSecondAverageStat : LinkedStat
    {
        public PerSecondAverageStat(string name, params Stat<double>[] dependencies) : base(name, 0)
        {
            SetupDepends(dependencies[0]);
        }

        public PerSecondAverageStat(string name, double value) : base(name, 0)
        {
        }

        public PerSecondAverageStat(string name) : base(name, 0)
        {
        }

        public Timer UpdateTimer { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        private void SetupDepends(Stat<double> dependency)
        {
            AddDependency(dependency);
            UpdateTimer = new Timer(1000);
            UpdateTimer.Elapsed += UpdateTimerOnElapsed;
            UpdateTimer.Start();
        }

        private void UpdateTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (!ParseControl.Instance.Timeline.FightingRightNow)
            {
                return;
            }
            var originalValue = Value * LastTimeDifference;
            UpdateDPSTick(originalValue);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            UpdateDPSTick((double) newValue);
        }

        private void UpdateDPSTick(double value)
        {
            if (FirstEventReceived == default(DateTime) || FirstEventReceived == null)
            {
                FirstEventReceived = Settings.Default.TrackXPSFromParseStartEvent ? ParseControl.Instance.StartTime : DateTime.Now;
            }
            LastEventReceived = DateTime.Now;
            var timeDifference = Convert.ToDouble(LastEventReceived.Subtract((DateTime) FirstEventReceived)
                                                                   .TotalSeconds);
            if (value == 0 || timeDifference == 0)
            {
                return;
            }
            LastTimeDifference = timeDifference;
            Value = value / timeDifference;
        }

        #region Time Tracking

        private DateTime? FirstEventReceived { get; set; }
        private DateTime LastEventReceived { get; set; }
        private double LastTimeDifference { get; set; }

        #endregion
    }
}
