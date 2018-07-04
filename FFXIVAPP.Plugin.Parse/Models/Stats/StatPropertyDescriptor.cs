// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatPropertyDescriptor.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   StatPropertyDescriptor.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.Stats {
    using System;
    using System.ComponentModel;

    public class StatPropertyDescriptor : PropertyDescriptor {
        public StatPropertyDescriptor(string name)
            : base(name, null) { }

        public override Type ComponentType {
            get {
                return typeof(StatGroup);
            }
        }

        public override bool IsReadOnly {
            get {
                return false;
            }
        }

        public override Type PropertyType {
            get {
                return this.Name.ToLower() == "name"
                           ? typeof(string)
                           : typeof(double);
            }
        }

        public override bool CanResetValue(object component) {
            return true;
        }

        public override object GetValue(object component) {
            var statGroup = (StatGroup) component;
            return this.Name.ToLower() == "name"
                       ? statGroup.Name
                       : statGroup.GetStatValue(this.Name);
        }

        public override void ResetValue(object component) {
            var statGroup = (StatGroup) component;
            if (statGroup.Stats.HasStat(this.Name)) {
                statGroup.Stats.GetStat(this.Name).Value = 0;
            }
        }

        public override void SetValue(object component, object value) {
            var statGroup = (StatGroup) component;
            statGroup.Stats.EnsureStatValue(this.Name, (double) value);
        }

        public override bool ShouldSerializeValue(object component) {
            return false;
        }
    }
}