// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Expressions.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Expressions.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Models {
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    using FFXIVAPP.Plugin.Parse.Models.Events;
    using FFXIVAPP.Plugin.Parse.RegularExpressions;

    public class Expressions : INotifyPropertyChanged {
        private Match _mActions;

        private Match _mBeneficialGain;

        private Match _mBeneficialLose;

        private Match _mCure;

        private Match _mDamage;

        private Match _mDamageAuto;

        private Match _mDetrimentalGain;

        private Match _mDetrimentalLose;

        private Match _mFailed;

        private Match _mFailedAuto;

        private Match _mItems;

        private Match _pActions;

        private Match _pBeneficialGain;

        private Match _pBeneficialLose;

        private Match _pCure;

        private Match _pDamage;

        private Match _pDamageAuto;

        private Match _pDetrimentalGain;

        private Match _pDetrimentalLose;

        private Match _pFailed;

        private Match _pFailedAuto;

        private Match _pItems;

        public Expressions(Event e) {
            this.Event = e;
            this.Cleaned = e.ChatLogItem == null
                               ? string.Empty
                               : e.ChatLogItem.Line;
            this.Initialize();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string Added { get; private set; }

        public string Attack { get; private set; }

        public string Cleaned { get; set; }

        public string Counter { get; private set; }

        public string DAttack { get; private set; }

        public Event Event { get; set; }

        public string HealingType { get; private set; }

        public Match mActions {
            get {
                return this._mActions ?? (this._mActions = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mActions = value;
                this.RaisePropertyChanged();
            }
        }

        public Match mBeneficialGain {
            get {
                return this._mBeneficialGain ?? (this._mBeneficialGain = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mBeneficialGain = value;
                this.RaisePropertyChanged();
            }
        }

        public Match mBeneficialLose {
            get {
                return this._mBeneficialLose ?? (this._mBeneficialLose = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mBeneficialLose = value;
                this.RaisePropertyChanged();
            }
        }

        public Match mCure {
            get {
                return this._mCure ?? (this._mCure = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mCure = value;
                this.RaisePropertyChanged();
            }
        }

        public Match mDamage {
            get {
                return this._mDamage ?? (this._mDamage = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mDamage = value;
                this.RaisePropertyChanged();
            }
        }

        public Match mDamageAuto {
            get {
                return this._mDamageAuto ?? (this._mDamageAuto = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mDamageAuto = value;
                this.RaisePropertyChanged();
            }
        }

        public Match mDetrimentalGain {
            get {
                return this._mDetrimentalGain ?? (this._mDetrimentalGain = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mDetrimentalGain = value;
                this.RaisePropertyChanged();
            }
        }

        public Match mDetrimentalLose {
            get {
                return this._mDetrimentalLose ?? (this._mDetrimentalLose = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mDetrimentalLose = value;
                this.RaisePropertyChanged();
            }
        }

        public Match mFailed {
            get {
                return this._mFailed ?? (this._mFailed = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mFailed = value;
                this.RaisePropertyChanged();
            }
        }

        public Match mFailedAuto {
            get {
                return this._mFailedAuto ?? (this._mFailedAuto = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mFailedAuto = value;
                this.RaisePropertyChanged();
            }
        }

        public Match mItems {
            get {
                return this._mItems ?? (this._mItems = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._mItems = value;
                this.RaisePropertyChanged();
            }
        }

        public string Mitigated { get; private set; }

        public Match pActions {
            get {
                return this._pActions ?? (this._pActions = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pActions = value;
                this.RaisePropertyChanged();
            }
        }

        public Match pBeneficialGain {
            get {
                return this._pBeneficialGain ?? (this._pBeneficialGain = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pBeneficialGain = value;
                this.RaisePropertyChanged();
            }
        }

        public Match pBeneficialLose {
            get {
                return this._pBeneficialLose ?? (this._pBeneficialLose = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pBeneficialLose = value;
                this.RaisePropertyChanged();
            }
        }

        public Match pCure {
            get {
                return this._pCure ?? (this._pCure = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pCure = value;
                this.RaisePropertyChanged();
            }
        }

        public Match pDamage {
            get {
                return this._pDamage ?? (this._pDamage = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pDamage = value;
                this.RaisePropertyChanged();
            }
        }

        public Match pDamageAuto {
            get {
                return this._pDamageAuto ?? (this._pDamageAuto = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pDamageAuto = value;
                this.RaisePropertyChanged();
            }
        }

        public Match pDetrimentalGain {
            get {
                return this._pDetrimentalGain ?? (this._pDetrimentalGain = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pDetrimentalGain = value;
                this.RaisePropertyChanged();
            }
        }

        public Match pDetrimentalLose {
            get {
                return this._pDetrimentalLose ?? (this._pDetrimentalLose = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pDetrimentalLose = value;
                this.RaisePropertyChanged();
            }
        }

        public Match pFailed {
            get {
                return this._pFailed ?? (this._pFailed = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pFailed = value;
                this.RaisePropertyChanged();
            }
        }

        public Match pFailedAuto {
            get {
                return this._pFailedAuto ?? (this._pFailedAuto = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pFailedAuto = value;
                this.RaisePropertyChanged();
            }
        }

        public Match pItems {
            get {
                return this._pItems ?? (this._pItems = Regex.Match("ph", @"^\.$"));
            }

            private set {
                this._pItems = value;
                this.RaisePropertyChanged();
            }
        }

        public string RAttack { get; private set; }

        public string You { get; private set; }

        public string YouString { get; private set; }

        private void Initialize() {
            switch (Constants.GameLanguage) {
                case "French":
                    this.pDamage = PlayerRegEx.DamageFr.Match(this.Cleaned);
                    this.pDamageAuto = PlayerRegEx.DamageAutoFr.Match(this.Cleaned);
                    this.pFailed = PlayerRegEx.FailedFr.Match(this.Cleaned);
                    this.pFailedAuto = PlayerRegEx.FailedAutoFr.Match(this.Cleaned);
                    this.pActions = PlayerRegEx.ActionsFr.Match(this.Cleaned);
                    this.pItems = PlayerRegEx.ItemsFr.Match(this.Cleaned);
                    this.pCure = PlayerRegEx.CureFr.Match(this.Cleaned);
                    this.pBeneficialGain = PlayerRegEx.BeneficialGainFr.Match(this.Cleaned);
                    this.pBeneficialLose = PlayerRegEx.BeneficialLoseFr.Match(this.Cleaned);
                    this.pDetrimentalGain = PlayerRegEx.DetrimentalGainFr.Match(this.Cleaned);
                    this.pDetrimentalLose = PlayerRegEx.DetrimentalLoseFr.Match(this.Cleaned);
                    this.mDamage = MonsterRegEx.DamageFr.Match(this.Cleaned);
                    this.mDamageAuto = MonsterRegEx.DamageAutoFr.Match(this.Cleaned);
                    this.mFailed = MonsterRegEx.FailedFr.Match(this.Cleaned);
                    this.mFailedAuto = MonsterRegEx.FailedAutoFr.Match(this.Cleaned);
                    this.mActions = MonsterRegEx.ActionsFr.Match(this.Cleaned);
                    this.mItems = MonsterRegEx.ItemsFr.Match(this.Cleaned);
                    this.mCure = MonsterRegEx.CureFr.Match(this.Cleaned);
                    this.mBeneficialGain = MonsterRegEx.BeneficialGainFr.Match(this.Cleaned);
                    this.mBeneficialLose = MonsterRegEx.BeneficialLoseFr.Match(this.Cleaned);
                    this.mDetrimentalGain = MonsterRegEx.DetrimentalGainFr.Match(this.Cleaned);
                    this.mDetrimentalLose = MonsterRegEx.DetrimentalLoseFr.Match(this.Cleaned);
                    this.Counter = "Contre";
                    this.Added = "Effet Supplémentaire";
                    this.HealingType = "PV";
                    this.RAttack = "D'Attaque À Distance";
                    this.DAttack = "Direct Attack";
                    this.Attack = "Attaque";
                    this.You = @"^[Vv]ous$";
                    this.YouString = "Vous";
                    this.Mitigated = "Dommages Atténué (Bouclier Magique)";
                    break;
                case "Japanese":
                    this.pDamage = PlayerRegEx.DamageJa.Match(this.Cleaned);
                    this.pDamageAuto = PlayerRegEx.DamageAutoJa.Match(this.Cleaned);
                    this.pFailed = PlayerRegEx.FailedJa.Match(this.Cleaned);
                    this.pFailedAuto = PlayerRegEx.FailedAutoJa.Match(this.Cleaned);
                    this.pActions = PlayerRegEx.ActionsJa.Match(this.Cleaned);
                    this.pItems = PlayerRegEx.ItemsJa.Match(this.Cleaned);
                    this.pCure = PlayerRegEx.CureJa.Match(this.Cleaned);
                    this.pBeneficialGain = PlayerRegEx.BeneficialGainJa.Match(this.Cleaned);
                    this.pBeneficialLose = PlayerRegEx.BeneficialLoseJa.Match(this.Cleaned);
                    this.pDetrimentalGain = PlayerRegEx.DetrimentalGainJa.Match(this.Cleaned);
                    this.pDetrimentalLose = PlayerRegEx.DetrimentalLoseJa.Match(this.Cleaned);
                    this.mDamage = MonsterRegEx.DamageJa.Match(this.Cleaned);
                    this.mDamageAuto = MonsterRegEx.DamageAutoJa.Match(this.Cleaned);
                    this.mFailed = MonsterRegEx.FailedJa.Match(this.Cleaned);
                    this.mFailedAuto = MonsterRegEx.FailedAutoJa.Match(this.Cleaned);
                    this.mActions = MonsterRegEx.ActionsJa.Match(this.Cleaned);
                    this.mItems = MonsterRegEx.ItemsJa.Match(this.Cleaned);
                    this.mCure = MonsterRegEx.CureJa.Match(this.Cleaned);
                    this.mBeneficialGain = MonsterRegEx.BeneficialGainJa.Match(this.Cleaned);
                    this.mBeneficialLose = MonsterRegEx.BeneficialLoseJa.Match(this.Cleaned);
                    this.mDetrimentalGain = MonsterRegEx.DetrimentalGainJa.Match(this.Cleaned);
                    this.mDetrimentalLose = MonsterRegEx.DetrimentalLoseJa.Match(this.Cleaned);
                    this.Counter = "カウンター";
                    this.Added = "追加効果";
                    this.HealingType = "ＨＰ";
                    this.RAttack = "Ranged Attack";
                    this.Attack = "Attack";
                    this.DAttack = "Direct Attack";
                    this.You = @"^\.$";
                    this.YouString = "君";
                    this.Mitigated = "軽減ダメージ（魔法の盾）";
                    break;
                case "German":
                    this.pDamage = PlayerRegEx.DamageDe.Match(this.Cleaned);
                    this.pDamageAuto = PlayerRegEx.DamageAutoDe.Match(this.Cleaned);
                    this.pFailed = PlayerRegEx.FailedDe.Match(this.Cleaned);
                    this.pFailedAuto = PlayerRegEx.FailedAutoDe.Match(this.Cleaned);
                    this.pActions = PlayerRegEx.ActionsDe.Match(this.Cleaned);
                    this.pItems = PlayerRegEx.ItemsDe.Match(this.Cleaned);
                    this.pCure = PlayerRegEx.CureDe.Match(this.Cleaned);
                    this.pBeneficialGain = PlayerRegEx.BeneficialGainDe.Match(this.Cleaned);
                    this.pBeneficialLose = PlayerRegEx.BeneficialLoseDe.Match(this.Cleaned);
                    this.pDetrimentalGain = PlayerRegEx.DetrimentalGainDe.Match(this.Cleaned);
                    this.pDetrimentalLose = PlayerRegEx.DetrimentalLoseDe.Match(this.Cleaned);
                    this.mDamage = MonsterRegEx.DamageDe.Match(this.Cleaned);
                    this.mDamageAuto = MonsterRegEx.DamageAutoDe.Match(this.Cleaned);
                    this.mFailed = MonsterRegEx.FailedDe.Match(this.Cleaned);
                    this.mFailedAuto = MonsterRegEx.FailedAutoDe.Match(this.Cleaned);
                    this.mActions = MonsterRegEx.ActionsDe.Match(this.Cleaned);
                    this.mItems = MonsterRegEx.ItemsDe.Match(this.Cleaned);
                    this.mCure = MonsterRegEx.CureDe.Match(this.Cleaned);
                    this.mBeneficialGain = MonsterRegEx.BeneficialGainDe.Match(this.Cleaned);
                    this.mBeneficialLose = MonsterRegEx.BeneficialLoseDe.Match(this.Cleaned);
                    this.mDetrimentalGain = MonsterRegEx.DetrimentalGainDe.Match(this.Cleaned);
                    this.mDetrimentalLose = MonsterRegEx.DetrimentalLoseDe.Match(this.Cleaned);
                    this.Counter = "Counter";
                    this.Added = "Zusatzefeckt";
                    this.HealingType = "LP";
                    this.RAttack = "Ranged Attack";
                    this.Attack = "Attack";
                    this.DAttack = "Direct Attack";
                    this.You = @"^[Dd](ich|ie|u)$";
                    this.YouString = "Du";
                    this.Mitigated = "Schäden Gemildert (Zauberschild)";
                    break;
                case "Chinese":
                    this.pDamage = PlayerRegEx.DamageZh.Match(this.Cleaned);
                    this.pDamageAuto = PlayerRegEx.DamageAutoZh.Match(this.Cleaned);
                    this.pFailed = PlayerRegEx.FailedZh.Match(this.Cleaned);
                    this.pFailedAuto = PlayerRegEx.FailedAutoZh.Match(this.Cleaned);
                    this.pActions = PlayerRegEx.ActionsZh.Match(this.Cleaned);
                    this.pItems = PlayerRegEx.ItemsZh.Match(this.Cleaned);
                    this.pCure = PlayerRegEx.CureZh.Match(this.Cleaned);
                    this.pBeneficialGain = PlayerRegEx.BeneficialGainZh.Match(this.Cleaned);
                    this.pBeneficialLose = PlayerRegEx.BeneficialLoseZh.Match(this.Cleaned);
                    this.pDetrimentalGain = PlayerRegEx.DetrimentalGainZh.Match(this.Cleaned);
                    this.pDetrimentalLose = PlayerRegEx.DetrimentalLoseZh.Match(this.Cleaned);
                    this.mDamage = MonsterRegEx.DamageZh.Match(this.Cleaned);
                    this.mDamageAuto = MonsterRegEx.DamageAutoZh.Match(this.Cleaned);
                    this.mFailed = MonsterRegEx.FailedZh.Match(this.Cleaned);
                    this.mFailedAuto = MonsterRegEx.FailedAutoZh.Match(this.Cleaned);
                    this.mActions = MonsterRegEx.ActionsZh.Match(this.Cleaned);
                    this.mItems = MonsterRegEx.ItemsZh.Match(this.Cleaned);
                    this.mCure = MonsterRegEx.CureZh.Match(this.Cleaned);
                    this.mBeneficialGain = MonsterRegEx.BeneficialGainZh.Match(this.Cleaned);
                    this.mBeneficialLose = MonsterRegEx.BeneficialLoseZh.Match(this.Cleaned);
                    this.mDetrimentalGain = MonsterRegEx.DetrimentalGainZh.Match(this.Cleaned);
                    this.mDetrimentalLose = MonsterRegEx.DetrimentalLoseZh.Match(this.Cleaned);
                    this.Counter = "Counter";
                    this.Added = "附加效果";
                    this.HealingType = "体力";
                    this.RAttack = "Ranged Attack";
                    this.Attack = "Attack";
                    this.DAttack = "Direct Attack";
                    this.You = @"^[Yy]ou?$";
                    this.YouString = "You";
                    this.Mitigated = "Mitigated Damage (Magic Shield)";
                    break;
                default:
                    this.pDamage = PlayerRegEx.DamageEn.Match(this.Cleaned);
                    this.pDamageAuto = PlayerRegEx.DamageAutoEn.Match(this.Cleaned);
                    this.pFailed = PlayerRegEx.FailedEn.Match(this.Cleaned);
                    this.pFailedAuto = PlayerRegEx.FailedAutoEn.Match(this.Cleaned);
                    this.pActions = PlayerRegEx.ActionsEn.Match(this.Cleaned);
                    this.pItems = PlayerRegEx.ItemsEn.Match(this.Cleaned);
                    this.pCure = PlayerRegEx.CureEn.Match(this.Cleaned);
                    this.pBeneficialGain = PlayerRegEx.BeneficialGainEn.Match(this.Cleaned);
                    this.pBeneficialLose = PlayerRegEx.BeneficialLoseEn.Match(this.Cleaned);
                    this.pDetrimentalGain = PlayerRegEx.DetrimentalGainEn.Match(this.Cleaned);
                    this.pDetrimentalLose = PlayerRegEx.DetrimentalLoseEn.Match(this.Cleaned);
                    this.mDamage = MonsterRegEx.DamageEn.Match(this.Cleaned);
                    this.mDamageAuto = MonsterRegEx.DamageAutoEn.Match(this.Cleaned);
                    this.mFailed = MonsterRegEx.FailedEn.Match(this.Cleaned);
                    this.mFailedAuto = MonsterRegEx.FailedAutoEn.Match(this.Cleaned);
                    this.mActions = MonsterRegEx.ActionsEn.Match(this.Cleaned);
                    this.mItems = MonsterRegEx.ItemsEn.Match(this.Cleaned);
                    this.mCure = MonsterRegEx.CureEn.Match(this.Cleaned);
                    this.mBeneficialGain = MonsterRegEx.BeneficialGainEn.Match(this.Cleaned);
                    this.mBeneficialLose = MonsterRegEx.BeneficialLoseEn.Match(this.Cleaned);
                    this.mDetrimentalGain = MonsterRegEx.DetrimentalGainEn.Match(this.Cleaned);
                    this.mDetrimentalLose = MonsterRegEx.DetrimentalLoseEn.Match(this.Cleaned);
                    this.Counter = "Counter";
                    this.Added = "Additional Effect";
                    this.HealingType = "HP";
                    this.RAttack = "Ranged Attack";
                    this.Attack = "Attack";
                    this.DAttack = "Direct Attack";
                    this.You = @"^[Yy]ou?$";
                    this.YouString = "You";
                    this.Mitigated = "Mitigated Damage (Magic Shield)";
                    break;
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}