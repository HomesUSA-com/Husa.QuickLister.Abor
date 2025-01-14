namespace Husa.Quicklister.Abor.Api.Contracts.Request.Community
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Community.CommunityDetail;
    using Husa.Quicklister.Extensions.Api.Contracts.Models.ShowingTime;

    public class CommunitySaleRequest
    {
        public Guid CompanyId { get; set; }
        public CommunityProfileRequest Profile { get; set; }
        public CommunityPropertyRequest Property { get; set; }
        public FeaturesAndUtilitiesRequest Utilities { get; set; }
        public CommunityFinancialRequest FinancialSchools { get; set; }
        public CommunityShowingRequest Showing { get; set; }
        public ShowingTimeInfo ShowingTime { get; set; }
        public IEnumerable<OpenHouseRequest> OpenHouses { get; set; }
    }
}
