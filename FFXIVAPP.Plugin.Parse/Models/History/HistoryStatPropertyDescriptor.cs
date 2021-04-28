// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryStatPropertyDescriptor.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   HistoryStatPropertyDescriptor.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models.History {
    using System;
    using System.ComponentModel;

    public class HistoryStatPropertyDescriptor : PropertyDescriptor {
        public HistoryStatPropertyDescriptor(string name) : base(name, null) { }

        public override Type ComponentType {
            get {
                return typeof(HistoryGroup);
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
            var historyGroup = (HistoryGroup) component;
            return this.Name.ToLower() == "name"
                       ? historyGroup.Name
                       : historyGroup.GetStatValue(this.Name);
        }

        public override void ResetValue(object component) {
            var historyGroup = (HistoryGroup) component;
            if (historyGroup.Stats.HasStat(this.Name)) {
                historyGroup.Stats.GetStat(this.Name).Value = 0;
            }
        }

        public override void SetValue(object component, object value) {
            var historyGroup = (HistoryGroup) component;
            historyGroup.Stats.EnsureStatValue(this.Name, (double) value);
        }

        public override bool ShouldSerializeValue(object component) {
            return false;
        }
    }
}