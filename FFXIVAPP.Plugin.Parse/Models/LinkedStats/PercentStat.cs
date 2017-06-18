// FFXIVAPP.Plugin.Parse ~ PercentStat.cs
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

using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public class PercentStat : LinkedStat
    {
        private readonly Stat<double> _denominator;
        private readonly Stat<double> _numerator;

        public PercentStat(string name, params Stat<double>[] dependencies) : base(name, 0)
        {
            _numerator = dependencies[0];
            _denominator = dependencies[1];
            SetupDepends();
        }

        public PercentStat(string name, double value) : base(name, 0)
        {
        }

        public PercentStat(string name) : base(name, 0)
        {
        }

        /// <summary>
        /// </summary>
        private void SetupDepends()
        {
            AddDependency(_numerator);
            AddDependency(_denominator);
            if (_numerator.Value > 0 && _denominator.Value > 0)
            {
                UpdatePercent();
            }
        }

        /// <summary>
        /// </summary>
        private void UpdatePercent()
        {
            if (_numerator.Value == 0 || _denominator.Value == 0)
            {
                Value = 0;
                return;
            }
            Value = (_numerator.Value / _denominator.Value);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            UpdatePercent();
        }
    }
}
