// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerSecondAverageStat.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PerSecondAverageStat.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats {
    using System;
    using System.Timers;

    using FFXIVAPP.Plugin.Parse.Models.Stats;
    using FFXIVAPP.Plugin.Parse.Properties;

    public class PerSecondAverageStat : LinkedStat {
        public PerSecondAverageStat(string name, params Stat<double>[] dependencies)
            : base(name, 0) {
            this.SetupDepends(dependencies[0]);
        }

        public PerSecondAverageStat(string name, double value)
            : base(name, 0) { }

        public PerSecondAverageStat(string name)
            : base(name, 0) { }

        public Timer UpdateTimer { get; set; }

        private DateTime? FirstEventReceived { get; set; }

        private DateTime LastEventReceived { get; set; }

        private double LastTimeDifference { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue) {
            this.UpdateDPSTick((double) newValue);
        }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        private void SetupDepends(Stat<double> dependency) {
            this.AddDependency(dependency);
            this.UpdateTimer = new Timer(1000);
            this.UpdateTimer.Elapsed += this.UpdateTimerOnElapsed;
            this.UpdateTimer.Start();
        }

        private void UpdateDPSTick(double value) {
            if (this.FirstEventReceived == default(DateTime) || this.FirstEventReceived == null) {
                this.FirstEventReceived = Settings.Default.TrackXPSFromParseStartEvent
                                              ? ParseControl.Instance.StartTime
                                              : DateTime.Now;
            }

            this.LastEventReceived = DateTime.Now;
            var timeDifference = Convert.ToDouble(this.LastEventReceived.Subtract((DateTime) this.FirstEventReceived).TotalSeconds);
            if (value == 0 || timeDifference == 0) {
                return;
            }

            this.LastTimeDifference = timeDifference;
            this.Value = value / timeDifference;
        }

        private void UpdateTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs) {
            if (!ParseControl.Instance.Timeline.FightingRightNow) {
                return;
            }

            var originalValue = this.Value * this.LastTimeDifference;
            this.UpdateDPSTick(originalValue);
        }
    }
}