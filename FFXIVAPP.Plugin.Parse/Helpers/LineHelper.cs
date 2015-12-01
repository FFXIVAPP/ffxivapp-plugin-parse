// FFXIVAPP.Plugin.Parse
// FFXIVAPP & Related Plugins/Modules
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

using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Properties;

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class LineHelper
    {
        public static void SetTimelineTypes(ref Line line)
        {
            switch (line.EventSubject)
            {
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
            switch (line.EventDirection)
            {
                case EventDirection.Self:
                    switch (line.EventSubject)
                    {
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

        public static bool IsIgnored(Line line)
        {
            return IgnoreType(line.SourceTimelineType) || IgnoreType(line.TargetTimelineType);
        }

        private static bool IgnoreType(TimelineType timelineType)
        {
            switch (timelineType)
            {
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
