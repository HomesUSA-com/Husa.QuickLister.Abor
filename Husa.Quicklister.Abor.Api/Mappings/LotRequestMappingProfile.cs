namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Request.LotRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.LotRequest;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Models.LotRequest;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using ListingRequestFilter = Husa.Quicklister.Extensions.Api.Contracts.Request.ListingRequest.ListingRequestFilter;

    public class LotRequestMappingProfile : Profile
    {
        public LotRequestMappingProfile()
        {
            this.CreateMap<ListingRequestFilter, ListingRequestQueryFilter>();
            this.CreateMap<ListingRequestQueryResult, ListingRequestQueryResponse>();
            this.CreateMap<LotListingRequestDetailQueryResult, LotListingRequestDetailResponse>();
            this.CreateMap<LotListingRequestForUpdate, LotListingDto>();
            this.CreateMap<LotListingRequestForUpdate, LotListingRequestDto>();
            this.CreateMap<LotListingRequestDto, ListingRequestValueObject>();
            this.CreateMap<LotListingRequestDto, LotPropertyValueObject>();
        }
    }
}
