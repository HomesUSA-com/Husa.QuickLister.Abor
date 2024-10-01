namespace Husa.Quicklister.Abor.Api.Mappings
{
    using System;
    using AutoMapper;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Request.LotListing;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.LotListing;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.ValueObjects;

    public class LotMappingProfile : Profile
    {
        public LotMappingProfile()
        {
            this.CreateMap<LotListingDto, LotValueObject>();
            this.CreateMap<LotPropertyDto, LotPropertyInfo>();
            this.CreateMap<LotFeaturesDto, LotFeaturesInfo>();
            this.CreateMap<LotFinancialDto, LotFinancialInfo>();
            this.CreateMap<LotSchoolsDto, LotSchoolsInfo>();
            this.CreateMap<LotShowingDto, LotShowingInfo>();
            this.CreateMap<LotAddressDto, LotAddressInfo>();

            this.CreateMap<AddressQueryResult, AddressInfoResponse>();
            this.CreateMap<LotAddressQueryResult, LotAddressResponse>();
            this.CreateMap<LotListingQueryResult, ListingResponse>()
               .ForMember(dest => dest.IsCompleteHome, config => config.MapFrom(x => false));
            this.CreateMap<LotPropertyQueryResult, LotPropertyResponse>();
            this.CreateMap<LotFeaturesQueryResult, LotFeaturesResponse>();
            this.CreateMap<LotFinancialQueryResult, LotFinancialResponse>();
            this.CreateMap<LotShowingQueryResult, LotShowingResponse>();
            this.CreateMap<SchoolsInfoQueryResult, LotSchoolsResponse>();
            this.CreateMap<ListingStatusFieldsQueryResult, ListingStatusFieldsResponse>();

            this.CreateMap<LotListingQueryDetailResult, LotListingDetailResponse>()
               .ForMember(dest => dest.StreetNum, config => config.MapFrom(x => x.AddressInfo.StreetNumber))
               .ForMember(dest => dest.StreetName, config => config.MapFrom(x => x.AddressInfo.StreetName))
               .ForMember(dest => dest.StreetType, config => config.MapFrom(x => x.AddressInfo.StreetType))
               .ForMember(dest => dest.City, config => config.MapFrom(x => x.AddressInfo.City))
               .ForMember(dest => dest.County, config => config.MapFrom(x => x.AddressInfo.County))
               .ForMember(dest => dest.Subdivision, config => config.MapFrom(x => x.AddressInfo.Subdivision))
               .ForMember(dest => dest.ZipCode, config => config.MapFrom(x => x.AddressInfo.ZipCode))
               .ForMember(dest => dest.State, config => config.MapFrom(x => x.AddressInfo.State))
               .ForMember(dest => dest.ListPrice, config => config.MapFrom(x => x.ListPrice))
               .ForMember(dest => dest.OwnerName, config => config.MapFrom(x => x.OwnerName))
               .ForMember(dest => dest.ListDate, config => config.MapFrom(x => x.ListDate))
               .ForMember(dest => dest.MlsNumber, config => config.MapFrom(x => x.MlsNumber))
               .ForMember(dest => dest.MlsStatus, config => config.MapFrom(x => x.MlsStatus))
               .ForMember(dest => dest.MarketModifiedOn, config => config.MapFrom(x => x.MarketModifiedOn))
               .ForMember(dest => dest.MarketCode, config => config.MapFrom(x => MarketCode.Austin))
               .ForMember(dest => dest.IsCompleteHome, config => config.MapFrom(x => false))
               .ForMember(dest => dest.PlanName, config => config.MapFrom(x => x.PlanName))
               .ForMember(dest => dest.XmlListingId, config => config.MapFrom(x => x.XmlListingId == null || x.XmlListingId == Guid.Empty ? x.XmlDiscrepancyListingId : x.XmlListingId))
               .ForMember(dest => dest.UnitNumber, config => config.MapFrom(x => x.AddressInfo.UnitNumber));

            this.CreateMap<LotListingDetailRequest, LotListingDto>();
            this.CreateMap<LotPropertyRequest, LotPropertyDto>();
            this.CreateMap<LotFeaturesRequest, LotFeaturesDto>();
            this.CreateMap<LotFinancialRequest, LotFinancialDto>();
            this.CreateMap<LotSchoolsRequest, LotSchoolsDto>();
            this.CreateMap<LotShowingRequest, LotShowingDto>();
            this.CreateMap<LotAdressRequest, LotAddressDto>();
        }
    }
}
