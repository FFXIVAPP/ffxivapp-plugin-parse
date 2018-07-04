// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatGroupTypeDescriptor.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   StatGroupTypeDescriptor.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Stats {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public abstract class StatGroupTypeDescriptor : CustomTypeDescriptor {
        protected StatGroup StatGroup;

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes) {
            List<PropertyDescriptor> descriptors = this.StatGroup.Stats.Select(stat => new StatPropertyDescriptor(stat.Name)).Cast<PropertyDescriptor>().ToList();
            descriptors.Add(new StatPropertyDescriptor("Name"));
            descriptors.AddRange(this.StatGroup.Children.Select(p => new StatGroupPropertyDescriptor(p.Name)));
            return new PropertyDescriptorCollection(descriptors.ToArray());
        }

        public override PropertyDescriptorCollection GetProperties() {
            return this.GetProperties(null);
        }
    }
}