// FFXIVAPP.Plugin.Parse ~ StatPropertyDescriptor.cs
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

using System;
using System.ComponentModel;

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public class StatPropertyDescriptor : PropertyDescriptor
    {
        public StatPropertyDescriptor(string name) : base(name, null)
        {
        }

        public override Type ComponentType
        {
            get { return typeof (StatGroup); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return Name.ToLower() == "name" ? typeof (string) : typeof (double); }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            var statGroup = (StatGroup) component;
            return Name.ToLower() == "name" ? statGroup.Name : statGroup.GetStatValue(Name);
        }

        public override void ResetValue(object component)
        {
            var statGroup = (StatGroup) component;
            if (statGroup.Stats.HasStat(Name))
            {
                statGroup.Stats.GetStat(Name)
                         .Value = 0;
            }
        }

        public override void SetValue(object component, object value)
        {
            var statGroup = (StatGroup) component;
            statGroup.Stats.EnsureStatValue(Name, (double) value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
