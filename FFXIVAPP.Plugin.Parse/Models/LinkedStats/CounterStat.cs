// FFXIVAPP.Plugin.Parse
// FFXIVAPP & Related Plugins/Modules
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

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public class CounterStat : NumericStat
    {
        public CounterStat(string name, double value) : base(name, 0)
        {
        }

        public CounterStat(string name) : base(name, 0)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="amount"> </param>
        /// <returns> </returns>
        private double Increment(double amount)
        {
            Value += amount;
            return Value;
        }
    }
}
