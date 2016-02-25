// FFXIVAPP.Plugin.Parse ~ IStatContainer.cs
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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public interface IStatContainer : ICollection<Stat<double>>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        string Name { get; set; }
        bool HasStat(string name);
        Stat<double> GetStat(string name);
        bool TryGetStat(string name, out object result);
        Stat<double> EnsureStatValue(string name, double value);
        double GetStatValue(string name);
        void IncrementStat(string name, double value);
        void AddStats(IEnumerable<Stat<double>> stats);
    }
}
