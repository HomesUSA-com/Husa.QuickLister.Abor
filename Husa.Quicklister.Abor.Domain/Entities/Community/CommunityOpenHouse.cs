namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Xml.Api.Contracts.Response;
    using OpenHouseTypeEnum = Husa.Quicklister.Extensions.Domain.Enums.OpenHouseType;

    public class CommunityOpenHouse : OpenHouse
    {
        public CommunityOpenHouse(Guid communityId, OpenHouseTypeEnum type, TimeSpan startTime, TimeSpan endTime, ICollection<Refreshments> refreshments)
            : base(type, startTime, endTime, refreshments)
        {
            this.CommunityId = communityId;
            this.OpenHouseType = EntityType.Community.ToString();
        }

        protected CommunityOpenHouse()
        {
            this.OpenHouseType = EntityType.Community.ToString();
        }

        public Guid CommunityId { get; set; }
        public virtual CommunitySale Community { get; set; }

        public static CommunityOpenHouse ImportFromXml(OpenHouseResponse openHouse, DayOfWeek day) => new()
        {
            Type = GetOpenHouseType(day),
            StartTime = TimeSpan.ParseExact(openHouse.StartTime, "hhmm", CultureInfo.InvariantCulture),
            EndTime = TimeSpan.ParseExact(openHouse.EndTime, "hhmm", CultureInfo.InvariantCulture),
        };
    }
}
