// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatGroupPropertyDescriptor.cs" company="SyndicatedLife">
//   Copyrightę 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   StatGroupPropertyDescriptor.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Stats {
    using System;

    public class StatGroupPropertyDescriptor : StatPropertyDescriptor {
        public StatGroupPropertyDescriptor(string name) : base(name) { }

        public override Type PropertyType {
            get {
                return this.Name.ToLower() == "name"
                           ? typeof(string)
                           : typeof(StatGroup);
            }
        }

        public override object GetValue(object component) {
            if (this.Name.ToLower() == "name") {
                return ((StatGroup) component).Name;
            }

            return ((StatGroup) component).GetGroup(this.Name);
        }
    }
}