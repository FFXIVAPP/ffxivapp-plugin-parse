// FFXIVAPP.Plugin.Parse ~ StatGroupTypeDescriptor.cs
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
using System.Linq;

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public abstract class StatGroupTypeDescriptor : CustomTypeDescriptor
    {
        protected StatGroup StatGroup;

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var descriptors = StatGroup.Stats.Select(stat => new StatPropertyDescriptor(stat.Name))
                                       .Cast<PropertyDescriptor>()
                                       .ToList();
            descriptors.Add(new StatPropertyDescriptor("Name"));
            descriptors.AddRange(StatGroup.Children.Select(p => new StatGroupPropertyDescriptor(p.Name)));
            return new PropertyDescriptorCollection(descriptors.ToArray());
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }
    }
}
