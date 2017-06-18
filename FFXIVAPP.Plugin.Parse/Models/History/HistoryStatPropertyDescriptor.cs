// FFXIVAPP.Plugin.Parse ~ HistoryStatPropertyDescriptor.cs
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
using System.ComponentModel;

namespace FFXIVAPP.Plugin.Parse.Models.History
{
    public class HistoryStatPropertyDescriptor : PropertyDescriptor
    {
        public HistoryStatPropertyDescriptor(string name) : base(name, null)
        {
        }

        public override Type ComponentType
        {
            get { return typeof(HistoryGroup); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return Name.ToLower() == "name" ? typeof(string) : typeof(double); }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            var historyGroup = (HistoryGroup) component;
            return Name.ToLower() == "name" ? historyGroup.Name : historyGroup.GetStatValue(Name);
        }

        public override void ResetValue(object component)
        {
            var historyGroup = (HistoryGroup) component;
            if (historyGroup.Stats.HasStat(Name))
            {
                historyGroup.Stats.GetStat(Name)
                            .Value = 0;
            }
        }

        public override void SetValue(object component, object value)
        {
            var historyGroup = (HistoryGroup) component;
            historyGroup.Stats.EnsureStatValue(Name, (double) value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
