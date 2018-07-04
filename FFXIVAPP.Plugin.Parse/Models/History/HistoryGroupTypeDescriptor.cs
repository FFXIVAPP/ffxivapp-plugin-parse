// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryGroupTypeDescriptor.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   HistoryGroupTypeDescriptor.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.History {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public abstract class HistoryGroupTypeDescriptor : CustomTypeDescriptor {
        protected HistoryGroup HistoryGroup;

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes) {
            List<PropertyDescriptor> descriptors = this.HistoryGroup.Stats.Select(stat => new HistoryStatPropertyDescriptor(stat.Name)).Cast<PropertyDescriptor>().ToList();
            descriptors.Add(new HistoryStatPropertyDescriptor("Name"));
            descriptors.AddRange(this.HistoryGroup.Children.Select(p => new HistoryGroupPropertyDescriptor(p.Name)));
            return new PropertyDescriptorCollection(descriptors.ToArray());
        }

        public override PropertyDescriptorCollection GetProperties() {
            return this.GetProperties(null);
        }
    }
}