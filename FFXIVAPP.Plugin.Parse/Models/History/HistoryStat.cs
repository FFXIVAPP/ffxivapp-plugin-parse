// FFXIVAPP.Plugin.Parse ~ HistoryStat.cs
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

namespace FFXIVAPP.Plugin.Parse.Models.History
{
    public abstract class HistoryStat<T>
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        protected HistoryStat(string name = "", T value = default(T))
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public T Value { get; set; }

        /// <summary>
        /// </summary>
        public void Reset()
        {
            Value = default(T);
        }
    }
}
