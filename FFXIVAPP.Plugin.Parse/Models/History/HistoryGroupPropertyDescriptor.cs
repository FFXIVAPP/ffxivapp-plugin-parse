// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryGroupPropertyDescriptor.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   HistoryGroupPropertyDescriptor.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.History {
    using System;

    public class HistoryGroupPropertyDescriptor : HistoryStatPropertyDescriptor {
        public HistoryGroupPropertyDescriptor(string name) : base(name) { }

        public override Type PropertyType {
            get {
                return this.Name.ToLower() == "name"
                           ? typeof(string)
                           : typeof(HistoryGroup);
            }
        }

        public override object GetValue(object component) {
            if (this.Name.ToLower() == "name") {
                return ((HistoryGroup) component).Name;
            }

            return ((HistoryGroup) component).GetGroup(this.Name);
        }
    }
}