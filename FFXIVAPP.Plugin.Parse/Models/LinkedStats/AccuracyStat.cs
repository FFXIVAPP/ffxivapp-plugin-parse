// FFXIVAPP.Plugin.Parse
// AccuracyStat.cs
// 
// Copyright � 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public class AccuracyStat : LinkedStat
    {
        public AccuracyStat(string name, params Stat<double>[] dependencies) : base(name, 0)
        {
            UsedStat = dependencies[0];
            MissStat = dependencies[1];
            SetupDepends();
        }

        public AccuracyStat(string name, double value) : base(name, 0)
        {
        }

        public AccuracyStat(string name) : base(name, 0)
        {
        }

        private Stat<double> UsedStat { get; }
        private Stat<double> MissStat { get; }

        /// <summary>
        /// </summary>
        private void SetupDepends()
        {
            AddDependency(UsedStat);
            AddDependency(MissStat);
            if (UsedStat.Value > 0 && MissStat.Value > 0)
            {
                UpdateAccuracy();
            }
        }

        /// <summary>
        /// </summary>
        private void UpdateAccuracy()
        {
            if (UsedStat.Value == 0 && MissStat.Value == 0)
            {
                Value = 0;
                return;
            }
            var totalHits = Convert.ToDouble(UsedStat.Value - MissStat.Value);
            if (totalHits > -1)
            {
                Value = totalHits / UsedStat.Value;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            UpdateAccuracy();
        }
    }
}
