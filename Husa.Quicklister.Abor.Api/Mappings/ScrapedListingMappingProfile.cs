namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Reports;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;

    public class ScrapedListingMappingProfile : Profile
    {
        public ScrapedListingMappingProfile()
        {
            this.CreateMap<ScrapedListingRequestFilter, ScrapedListingQueryFilter>();
            this.CreateMap<ScrapedListingQueryResult, ScrapedListingQueryResponse>();
        }
    }
}
