namespace Husa.Quicklister.Abor.Application.Models.Community
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Extensions.Application.Models.ShowingTime;
    using ExtensionsDtos = Husa.Quicklister.Extensions.Application.Interfaces.Community.Dtos;

    public class CommunitySaleDto : ExtensionsDtos.ICommunity
    {
        public Guid CompanyId { get; set; }
        public CommunityProfileDto Profile { get; set; }
        public CommunityPropertyDto Property { get; set; }
        public FeaturesAndUtilitiesDto Utilities { get; set; }
        public CommunityFinancialDto FinancialSchools { get; set; }
        public CommunityShowingDto Showing { get; set; }
        public ShowingTimeDto ShowingTime { get; set; }
        public IEnumerable<OpenHouseDto> OpenHouses { get; set; }
    }
}
