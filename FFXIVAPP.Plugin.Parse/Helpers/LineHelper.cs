// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   LineHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Parse.Helpers {
    using FFXIVAPP.Plugin.Parse.Enums;
    using FFXIVAPP.Plugin.Parse.Models;
    using FFXIVAPP.Plugin.Parse.Properties;

    public static class LineHelper {
        public static bool IsIgnored(Line line) {
            return IgnoreType(line.SourceTimelineType) || IgnoreType(line.TargetTimelineType);
        }

        public static void SetTimelineTypes(ref Line line) {
            switch (line.EventSubject) {
                case EventSubject.You:
                case EventSubject.Pet:
                    line.SourceTimelineType = TimelineType.You;
                    break;
                case EventSubject.Party:
                case EventSubject.PetParty:
                    line.SourceTimelineType = TimelineType.Party;
                    break;
                case EventSubject.Alliance:
                case EventSubject.PetAlliance:
                    line.SourceTimelineType = TimelineType.Alliance;
                    break;
                case EventSubject.Other:
                case EventSubject.PetOther:
                    line.SourceTimelineType = TimelineType.Other;
                    break;
            }

            switch (line.EventDirection) {
                case EventDirection.Self:
                    switch (line.EventSubject) {
                        case EventSubject.You:
                        case EventSubject.Pet:
                            line.TargetTimelineType = TimelineType.You;
                            break;
                        case EventSubject.Party:
                        case EventSubject.PetParty:
                            line.TargetTimelineType = TimelineType.Party;
                            break;
                        case EventSubject.Alliance:
                        case EventSubject.PetAlliance:
                            line.TargetTimelineType = TimelineType.Alliance;
                            break;
                        case EventSubject.Other:
                        case EventSubject.PetOther:
                            line.TargetTimelineType = TimelineType.Other;
                            break;
                    }

                    break;
                case EventDirection.You:
                case EventDirection.Pet:
                    line.TargetTimelineType = TimelineType.You;
                    break;
                case EventDirection.Party:
                case EventDirection.PetParty:
                    line.TargetTimelineType = TimelineType.Party;
                    break;
                case EventDirection.Alliance:
                case EventDirection.PetAlliance:
                    line.TargetTimelineType = TimelineType.Alliance;
                    break;
                case EventDirection.Other:
                case EventDirection.PetOther:
                    line.TargetTimelineType = TimelineType.Other;
                    break;
            }
        }

        private static bool IgnoreType(TimelineType timelineType) {
            switch (timelineType) {
                case TimelineType.You:
                    return !Settings.Default.ParseYou;
                case TimelineType.Party:
                    return !Settings.Default.ParseParty;
                case TimelineType.Alliance:
                    return !Settings.Default.ParseAlliance;
                case TimelineType.Other:
                    return !Settings.Default.ParseOther;
            }

            return false;
        }
    }
}