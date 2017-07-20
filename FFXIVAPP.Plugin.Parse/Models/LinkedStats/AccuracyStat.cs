// FFXIVAPP.Plugin.Parse ~ AccuracyStat.cs
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
using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public class AccuracyStat : LinkedStat
    {
        public AccuracyStat(string name, params Stat<double>[] dependencies) : base(name, 0)
        {
            UsedStat = dependencies[0];
            MissStat = dependencies[1];
            SetupDepends();
        }

        public AccuracyStat(string name, double value) : base(name, 0)
        {
        }

        public AccuracyStat(string name) : base(name, 0)
        {
        }

        private Stat<double> UsedStat { get; }
        private Stat<double> MissStat { get; }

        /// <summary>
        /// </summary>
        private void SetupDepends()
        {
            AddDependency(UsedStat);
            AddDependency(MissStat);
            if (UsedStat.Value > 0 && MissStat.Value > 0)
            {
                UpdateAccuracy();
            }
        }

        /// <summary>
        /// </summary>
        private void UpdateAccuracy()
        {
            if (UsedStat.Value == 0 && MissStat.Value == 0)
            {
                Value = 0;
                return;
            }
            var totalHits = Convert.ToDouble(UsedStat.Value - MissStat.Value);
            if (totalHits > -1)
            {
                Value = totalHits / UsedStat.Value;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            UpdateAccuracy();
        }
    }
}
