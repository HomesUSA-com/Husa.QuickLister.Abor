namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System;
    using System.Globalization;
    using Husa.Xml.Api.Contracts.Response;
    using ExtensionsOpenHouse = Husa.Quicklister.Extensions.Domain.Entities.Community.CommunityOpenHouse;
    using OpenHouseTypeEnum = Husa.Quicklister.Extensions.Domain.Enums.OpenHouseType;

    public class CommunityOpenHouse : ExtensionsOpenHouse
    {
        public CommunityOpenHouse(Guid communityId, OpenHouseTypeEnum type, TimeSpan startTime, TimeSpan endTime, bool refreshments, bool lunch)
            : base(communityId, type, startTime, endTime, refreshments, lunch)
        {
        }

        protected CommunityOpenHouse()
        {
        }

        public virtual CommunitySale Community { get; set; }

        public static CommunityOpenHouse ImportFromXml(OpenHouseResponse openHouse, DayOfWeek day) => new()
        {
            Type = GetOpenHouseType(day),
            StartTime = TimeSpan.ParseExact(openHouse.StartTime, "hhmm", CultureInfo.InvariantCulture),
            EndTime = TimeSpan.ParseExact(openHouse.EndTime, "hhmm", CultureInfo.InvariantCulture),
        };
    }
}
