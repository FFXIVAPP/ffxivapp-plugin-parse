// FFXIVAPP.Plugin.Parse ~ PlayerRegEx.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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

using System.Text.RegularExpressions;
using FFXIVAPP.Common.RegularExpressions;

namespace FFXIVAPP.Plugin.Parse.RegularExpressions
{
    internal static class PlayerRegEx
    {
        //ENGLISH REGULAR EXPRESSIONS
        public static Regex DamageEn = new Regex(@"^ ⇒ (?<block>Blocked! )?(?<parry>Parried! )?(?<crit>Critical! )?((T|t)he )?(?<target>.+) takes? (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);
        public static Regex DamageAutoEn = new Regex(@"^(?! ⇒)(?<block>Blocked! )?(?<parry>Parried! )?(?<crit>Critical! )?(?<source>You|.+) hits? ((T|t)he )?(?<target>.+) for (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);
        public static Regex ResistEn = new Regex(@"^(?! ⇒)(?<resist>(Full|Partial|Half) resist! )(?<source>You|.+) takes? ((?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?|no )damage\.$", SharedRegEx.DefaultOptions);
        public static Regex FailedEn = new Regex(@"^( ⇒ )?The attack miss(es)?( ((T|t)he )?(?<target>.+))?\.$", SharedRegEx.DefaultOptions);
        public static Regex FailedAutoEn = new Regex(@"^(?! ⇒)(?<source>You|.+) miss(es)? ((T|t)he )?(?<target>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex ActionsEn = new Regex(@"^(?<source>You|.+) (use|cast)s? (?<action>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex ItemsEn = new Regex(@"^(?<source>You|.+) uses? (an? |the )?(?<item>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex CureEn = new Regex(@"( ⇒ )?(?<crit>Critical! )?((T|t)he )?(?<target>You|.+) recovers? (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?(?<type>\w+)\.$", SharedRegEx.DefaultOptions);
        public static Regex BeneficialGainEn = new Regex(@"^( ⇒ )?(?<target>You|.+) gains? the effect of (?<status>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex BeneficialLoseEn = new Regex(@"^( ⇒ )?(?<target>You|.+) loses? the effect of (?<status>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex DetrimentalGainEn = new Regex(@"^( ⇒ )?(?<target>You|.+) suffers? the effect of (?<status>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex DetrimentalLoseEn = new Regex(@"^( ⇒ )?(?<target>You|.+) recovers? from the effect of (?<status>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex ObtainsEn = new Regex(@"^(?<source>You|.+) obtains? (an? |the )?(?<item>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex DefeatsEn = new Regex(@"^((?<source>You|.+) defeats? ((T|t)he )?(?<target>.+)|((T|t)he )?(?<target>.+) is defeated)\.$", SharedRegEx.DefaultOptions);
        //FRENCH REGULAR EXPRESSIONS
        public static Regex DamageFr = new Regex(@"^ ⇒ (?<parry>Parade ?! )?(?<block>Blocage ?! )?(?<crit>Critique ?! )?(L[aes] |[LEAD]')?(?<target>.+) subit (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?points? de dégâts?\.$", SharedRegEx.DefaultOptions);
        public static Regex DamageAutoFr = new Regex(@"^(?! ⇒)(?<parry>Parade ?! )?(?<block>Blocage ?! )?(?<crit>Critique ?! )?(?<source>Vous|.+) infligez? \w+ (l[aes] |[lead]')?(?<target>.+) (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?points? de dégâts?\.$", SharedRegEx.DefaultOptions);
        public static Regex ResistFr = new Regex(@"^(?! ⇒)(?<resist>(Résistance totale|Résistance partielle|Semi résistance)! )(L[aes] |[LEAD]')?(?<target>.+) (subi(t|ssez?)? (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?points? de |ne subi(t|ssez?)? aucun )dégâts?\.$", SharedRegEx.DefaultOptions);
        public static Regex FailedFr = new Regex(@"^ ⇒ L'attaque manquez? (l[aes] |[lead]')?(?<target>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex FailedAutoFr = new Regex(@"^(?! ⇒)(?<source>Vous|.+) manquez? (l[aes] |[lead]')?(?<target>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex ActionsFr = new Regex(@"^(?<source>Vous|.+) (utilise|lance)z? (?<action>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex ItemsFr = new Regex(@"^(?<source>Vous|.+) utilisez? une? (?<item>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex CureFr = new Regex(@"^( ⇒ )?(?<crit>Critique ?! )?(?<target>Vous|.+) récup(é|è)rez? (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?(?<type>\w+)\.$", SharedRegEx.DefaultOptions);
        public static Regex BeneficialGainFr = new Regex(@"^( ⇒ )?(?<target>Vous|.+) bénéficiez? de l'effet (?<status>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex BeneficialLoseFr = new Regex(@"^( ⇒ )?(?<target>Vous|.+) perd(ez?)? l'effet (?<status>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex DetrimentalGainFr = new Regex(@"^( ⇒ )?(?<target>Vous|.+) subi(t|ssez?) l'effet (?<status>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex DetrimentalLoseFr = new Regex(@"^( ⇒ )?(?<target>Vous|.+) (perd(ez?)?|ne subi(t|ssez?)) plus l'effet (?<status>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex ObtainsFr = new Regex(@"^((?<source>Vous) obtenez|(?<source>.+) obtient)( une?| de la| l[aes])? (?<item>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex DefeatsFr = new Regex(@"^(((?<source>Vous) avez|(?<source>.+) a) vaincu (l[aes] |[lead]')?(?<target>.+)|(L[aes] |[LEAD]')?(?<target>.+) s'effondre)\.$", SharedRegEx.DefaultOptions);
        //JAPANESE REGULAR EXPRESSIONS
        public static Regex DamageJa = new Regex(@"^ ⇒ (?<crit>クリティカル！ )?(?<target>.+)((に|は)、?)(?<block>ブロックした！ )?(?<parry>受け流した！ )?(?<amount>\d+) ?(\((?<modifier>.\d+)%\) ?)?ダメージ。$", SharedRegEx.DefaultOptions);
        public static Regex DamageAutoJa = new Regex(@"^(?<source>.+)の攻撃( ⇒ )?(?<crit>クリティカル！ )?(?<target>.+)((に|は)、?)(?<block>ブロックした！ )?(?<parry>受け流した！ )?(?<amount>\d+) ?(\((?<modifier>.\d+)%\) ?)?ダメージ。$", SharedRegEx.DefaultOptions);
        public static Regex ResistJa = new Regex(@"^ ⇒ (?<target>.+)((に|は)、?)((?<resist>レジストした！)|(?<resist>クォータレジストした！ )?(?<resist>ハーフレジストした！ )?(?<amount>\d+) ?(\((?<modifier>.\d+)%\) ?)?ダメージ。)$", SharedRegEx.DefaultOptions);
        public static Regex FailedJa = new Regex(@"^ ⇒ (?<target>.+)((に|は)、?)ミス！$", SharedRegEx.DefaultOptions);
        public static Regex FailedAutoJa = new Regex(@"^(?<source>.+)の攻撃( ⇒ )?(?<target>.+)((に|は)、?)ミス！$", SharedRegEx.DefaultOptions);
        public static Regex ActionsJa = new Regex(@"^(?<source>.+)の「(?<action>.+)」$", SharedRegEx.DefaultOptions);
        public static Regex ItemsJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);
        public static Regex CureJa = new Regex(@"^( ⇒ )?(?<crit>クリティカル！ )?(?<target>.+)((に|は)、?)(?<amount>\d+) ?(\((?<modifier>.\d+)%\) ?)?(?<type>\w+)回復。$", SharedRegEx.DefaultOptions);
        public static Regex BeneficialGainJa = new Regex(@"^( ⇒ )?(?<target>.+)((に|は)、?)「(?<status>.+)」の効果。$", SharedRegEx.DefaultOptions);
        public static Regex BeneficialLoseJa = new Regex(@"^( ⇒ )?(?<target>.+)((に|は)、?)「(?<status>.+)」が切れた。$", SharedRegEx.DefaultOptions);
        public static Regex DetrimentalGainJa = new Regex(@"^( ⇒ )?(?<target>.+)((に|は)、?)「(?<status>.+)」の効果。$", SharedRegEx.DefaultOptions);
        public static Regex DetrimentalLoseJa = new Regex(@"^( ⇒ )?(?<target>.+)((に|は)、?)「(?<status>.+)」が切れた。$", SharedRegEx.DefaultOptions);
        public static Regex ObtainsJa = new Regex(@"^(?<source>.+)((に|は)、?)「(?<item>.+)」(?<amount>×.+)?を入手した。'$", SharedRegEx.DefaultOptions);
        public static Regex DefeatsJa = new Regex(@"^((?<source>.+)((に|は)、?)(?<target>.+)を倒した。|(?<target>.+)((に|は)、?)力尽きた。)$", SharedRegEx.DefaultOptions);
        //GERMAN REGULAR EXPRESSIONS
        public static Regex DamageDe = new Regex(@"^ ⇒ (?<block>Geblockt! ?)?(?<parry>Pariert! ?)?(?<crit>Kritischer Treffer! ?)?(D(u|einer|(i|e)r|ich|as|ie|en) )?(?<target>.+) erleides?t (nur )?(?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?Punkte? (Schaden|reduziert)\.$", SharedRegEx.DefaultOptions);
        public static Regex DamageAutoDe = new Regex(@"^(?! ⇒)(?<block>Geblockt! ?)?(?<parry>Pariert! ?)?(?<crit>Kritischer Treffer! ?)?(?<source>Du|.+) triffs?t (d(u|einer|(i|e)r|ich|as|ie|en) )?(?<target>.+) und verursachs?t (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?Punkte? (Schaden|reduziert)\.$", SharedRegEx.DefaultOptions);
        public static Regex ResistDe = new Regex(@"^(?! ⇒)(?<resist>(Vollkommen widerstanden|Teilweise widerstanden|Halb widerstanden!)! )(D(u|einer|(i|e)r|ich|as|ie|en) )?(?<target>.+) erleides?t (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?Punkte? Schaden\.$", SharedRegEx.DefaultOptions);
        public static Regex FailedDe = new Regex(@"^ ⇒ Die Attacke verfehlt?( (d(u|einer|(i|e)r|ich|as|ie|en) )?(?<target>.+))?\.$", SharedRegEx.DefaultOptions);
        public static Regex FailedAutoDe = new Regex(@"^(?! ⇒)(?<source>Du|.+) verfehls?t (d(u|einer|(i|e)r|ich|as|ie|en) )?(?<target>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex ActionsDe = new Regex(@"^(?<source>Du|.+) (setzt (?<action>.+) ein|wirks?t (?<action>.+))\.$", SharedRegEx.DefaultOptions);
        public static Regex ItemsDe = new Regex(@"^(?<source>Du|.+) verwendes?t (?<item>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex CureDe = new Regex(@"^( ⇒ )?(?<crit>Kritischer Treffer ?! )?(D(u|einer|(i|e)r|ich|as|ie|en) )?(?<target>.+) regeneriers?t (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?(?<type>\w+)\.$", SharedRegEx.DefaultOptions);
        public static Regex BeneficialGainDe = new Regex(@"^( ⇒ )?(D(u|einer|(i|e)r|ich|as|ie|en) )?(?<target>.+) erh lt(st| den) Effekt von (?<status>.+)\.$", SharedRegEx.DefaultOptions);
        public static Regex BeneficialLoseDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);
        public static Regex DetrimentalGainDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);
        public static Regex DetrimentalLoseDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);
        public static Regex ObtainsDe = new Regex(@"^(D(u|einer|(i|e)r|ich|as|ie|en) )?(?<source>.+) has?t (?<item>.+) erhalten\.$", SharedRegEx.DefaultOptions);
        public static Regex DefeatsDe = new Regex(@"^((?<source>Du|.+) has?t (d(u|einer|(i|e)r|ich|as|ie|en) )?(?<target>.+) besiegt|(D(u|einer|(i|e)r|ich|as|ie|en) )?(?<target>.+) bricht bewusstlos zusammen)\.$", SharedRegEx.DefaultOptions);
        //CHINESE REGULAR EXPRESSIONS
        public static Regex DamageZh = new Regex(@"^: ⇒ (?<crit>暴击！ )?(?<target>.+?)(?<block>招架住了！ )?(?<parry>格挡住了！ )?(受到(了)?)(?<amount>\d+) ?(\((?<modifier>.\d+)%\) ?)?点伤害。$", SharedRegEx.DefaultOptions);
        public static Regex DamageAutoZh = new Regex(@"^:(?<source>.+)发动攻击( ⇒ )?(?<crit>暴击！ )?(?<target>.+?)(?<block>招架住了！ )?(?<parry>格挡住了！ )?(受到(了)?)(?<amount>\d+) ?(\((?<modifier>.\d+)%\) ?)?点伤害。$", SharedRegEx.DefaultOptions);
        public static Regex ResistZh = new Regex(@"^: ⇒ (?<resist>无效！)(?<target>.+)((?<resist>没有受到伤害！)|(?<resist>クォータレジストした！ )?(?<resist>ハーフレジストした！ )?(?<amount>\d+) ?(\((?<modifier>.\d+)%\) ?)?ダメージ。)$", SharedRegEx.DefaultOptions);
        public static Regex FailedZh = new Regex(@"^: ⇒ 失误(了)?！$", SharedRegEx.DefaultOptions);
        public static Regex FailedAutoZh = new Regex(@"^:(?<source>.+)发动攻击( ⇒ )?((失误了！)|(对(?<target>.+)无效。))$", SharedRegEx.DefaultOptions);
        public static Regex ActionsZh = new Regex(@"^:(?<source>You|.+)(发动了|咏唱了|正在咏唱|正在发动)“(?<action>.+)”。$", SharedRegEx.DefaultOptions);
        public static Regex ItemsZh = new Regex(@"^:(?<source>You|.+)使用了“(?<item>.+)”。$", SharedRegEx.DefaultOptions);
        public static Regex CureZh = new Regex(@"^:( ⇒ )?(?<crit>暴击！ )?(?<target>You|.+)恢复了?(?<amount>\d+)?(\((?<modifier>.\d+)%\))?点(?<type>\w+)。$", SharedRegEx.DefaultOptions);
        public static Regex BeneficialGainZh = new Regex(@"^:( ⇒ )?对(?<target>You|.+)附加了“?(?<status>.+)”的效果。$", SharedRegEx.DefaultOptions);
        public static Regex BeneficialLoseZh = new Regex(@"^:( ⇒ )?(?<target>You|.+)的“(?<status>.+)”状态效果消失了。$", SharedRegEx.DefaultOptions);
        public static Regex DetrimentalGainZh = new Regex(@"^:( ⇒ )?对(?<target>.+)附加了“?(?<status>.+)”的效果。$", SharedRegEx.DefaultOptions);
        public static Regex DetrimentalLoseZh = new Regex(@"^:( ⇒ )?(?<target>.+)的“(?<status>.+)”状态效果消失了。$", SharedRegEx.DefaultOptions);
        public static Regex ObtainsZh = new Regex(@"^:(?<source>You|.+)获得了(?<item>.+)。$", SharedRegEx.DefaultOptions);
        public static Regex DefeatsZh = new Regex(@"^:(?<source>You|.+)将?(?<target>.+)打倒了。$", SharedRegEx.DefaultOptions);
    }
}
