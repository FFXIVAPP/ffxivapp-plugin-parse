// FFXIVAPP.Plugin.Parse ~ MinStat.cs
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
    public sealed class MinStat : LinkedStat
    {
        public MinStat(string name, params Stat<double>[] dependencies) : base(name, 0)
        {
            AddDependency(dependencies[0]);
            GotValue = false;
        }

        public MinStat(string name, double value) : base(name, 0)
        {
        }

        public MinStat(string name) : base(name, 0)
        {
        }

        private bool GotValue { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            var ovalue = (double) previousValue;
            var nvalue = (double) newValue;
            var delta = Math.Max(ovalue, nvalue) - Math.Min(ovalue, nvalue);
            if (delta >= Value && GotValue)
            {
                return;
            }
            Value = delta;
            GotValue = true;
        }
    }
}
