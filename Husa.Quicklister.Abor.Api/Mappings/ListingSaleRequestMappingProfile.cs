namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Extensions.Common;
    using Husa.Extensions.Document.Models;
    using Husa.Extensions.Document.QueryFilters;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Extensions.Application.Models.ShowingTime;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using DocumentModels = Husa.Quicklister.Abor.Data.Documents.Models;

    public class ListingSaleRequestMappingProfile : Profile
    {
        public ListingSaleRequestMappingProfile()
        {
            this.CreateMap<SalePropertyDetailDto, SalePropertyValueObject>()
                .ForMember(dest => dest.OwnerName, config => config.MapFrom(dto => dto.SalePropertyInfo.OwnerName))
                .ForMember(dest => dest.CommunityId, config => config.MapFrom(dto => dto.SalePropertyInfo.CommunityId))
                .ForMember(dest => dest.PlanId, config => config.MapFrom(dto => dto.SalePropertyInfo.PlanId))
                .ForMember(dest => dest.SpacesDimensionsInfo, config => config.MapFrom(dto => dto.SpacesDimensionsInfo))
                .ForMember(dest => dest.FeaturesInfo, config => config.MapFrom(dto => dto.FeaturesInfo))
                .ForMember(dest => dest.FinancialInfo, config => config.MapFrom(dto => dto.FinancialInfo))
                .ForMember(dest => dest.ShowingInfo, config => config.MapFrom(dto => dto.ShowingInfo))
                .ForMember(dest => dest.SchoolsInfo, config => config.MapFrom(dto => dto.SchoolsInfo))
                .ForMember(dest => dest.AddressInfo, config => config.MapFrom(dto => dto.AddressInfo))
                .ForMember(dest => dest.PropertyInfo, config => config.MapFrom(dto => dto.PropertyInfo))
                .ForMember(dest => dest.Rooms, config => config.MapFrom(dto => dto.Rooms))
                .ForMember(dest => dest.OpenHouses, config => config.MapFrom(dto => dto.OpenHouses));

            this.CreateMap<SalePropertyDetailDto, SaleProperty>()
                .ForMember(dest => dest.OwnerName, config => config.MapFrom(dto => dto.SalePropertyInfo.OwnerName))
                .ForMember(dest => dest.SpacesDimensionsInfo, config => config.MapFrom(dto => dto.SpacesDimensionsInfo))
                .ForMember(dest => dest.FeaturesInfo, config => config.MapFrom(dto => dto.FeaturesInfo))
                .ForMember(dest => dest.FinancialInfo, config => config.MapFrom(dto => dto.FinancialInfo))
                .ForMember(dest => dest.ShowingInfo, config => config.MapFrom(dto => dto.ShowingInfo))
                .ForMember(dest => dest.SchoolsInfo, config => config.MapFrom(dto => dto.SchoolsInfo))
                .ForMember(dest => dest.AddressInfo, config => config.MapFrom(dto => dto.AddressInfo))
                .ForMember(dest => dest.PropertyInfo, config => config.MapFrom(dto => dto.PropertyInfo))
                .ForMember(dest => dest.SalesOfficeInfo, config => config.Ignore())
                .ForMember(dest => dest.CompanyId, config => config.Ignore())
                .ForMember(dest => dest.PlanId, config => config.Ignore())
                .ForMember(dest => dest.Plan, config => config.Ignore())
                .ForMember(dest => dest.CommunityId, config => config.Ignore())
                .ForMember(dest => dest.Community, config => config.Ignore())
                .ForMember(dest => dest.SaleListings, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedBy, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedOn, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedBy, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedOn, config => config.Ignore())
                .ForMember(dest => dest.IsDeleted, config => config.Ignore())
                .ForMember(dest => dest.SaleListings, config => config.Ignore())
                .ForMember(dest => dest.SysTimestamp, config => config.Ignore());

            this.CreateMap<RequestBaseFilter, RequestBaseQueryFilter>();
            this.CreateMap<SaleListingRequestFilter, SaleListingRequestQueryFilter>()
                .ForMember(dest => dest.ListingId, config => config.MapFrom(dto => dto.SaleListingId));

            this.CreateMap<SummarySectionQueryResult, SummarySectionContract>();
            this.CreateMap<SummaryFieldQueryResult, SummaryFieldContract>();

            this.CreateMap<ListingSaleStatusFieldsDto, SaleStatusFieldsRecord>();
            this.CreateMap<ListingSalePublishInfoDto, PublishFieldsRecord>();
            this.CreateMap<SpacesDimensionsDto, SpacesDimensionsRecord>();

            this.CreateMap<FeaturesDto, FeaturesRecord>();
            this.CreateMap<FinancialDto, FinancialRecord>()
                .ForMember(dest => dest.ReadableAgentBonusAmount, config => config.MapFrom(dto => dto.AgentBonusAmount.GetCommissionAmount(dto.AgentBonusAmountType)))
                .ForMember(dest => dest.ReadableBuyersAgentCommission, config => config.MapFrom(dto => dto.BuyersAgentCommission.GetCommissionAmount(dto.BuyersAgentCommissionType)));
            this.CreateMap<ShowingDto, ShowingRecord>();
            this.CreateMap<SchoolsDto, SchoolRecord>();
            this.CreateMap<SaleAddressDto, AddressRecord>()
                 .ForMember(dest => dest.FormalAddress, config => config.Ignore())
                .ForMember(dest => dest.ReadableCity, config => config.Ignore());

            this.CreateMap<PropertyDto, PropertyRecord>();
            this.CreateMap<RoomDto, RoomRecord>()
                .ForMember(dest => dest.SysCreatedBy, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedOn, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedBy, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedOn, config => config.Ignore())
                .ForMember(dest => dest.IsDeleted, config => config.Ignore())
                .ForMember(dest => dest.SysTimestamp, config => config.Ignore())
                .ForMember(dest => dest.Id, config => config.Ignore())
                .ForMember(dest => dest.SalePropertyId, config => config.Ignore())
                .ForMember(dest => dest.FieldType, config => config.Ignore())
                .ForMember(dest => dest.CompanyId, config => config.Ignore());

            this.CreateMap<OpenHouseDto, OpenHouseRecord>()
                .ForMember(dest => dest.IsDeleted, config => config.Ignore())
                .ForMember(dest => dest.Id, config => config.Ignore());

            this.CreateMap<SalePropertyDetailDto, SalePropertyRecord>()
                .ForMember(dest => dest.OwnerName, config => config.MapFrom(dto => dto.SalePropertyInfo.OwnerName))
                .ForPath(dest => dest.SpacesDimensionsInfo, config => config.MapFrom(dto => dto.SpacesDimensionsInfo))
                .ForPath(dest => dest.FeaturesInfo, config => config.MapFrom(dto => dto.FeaturesInfo))
                .ForPath(dest => dest.FinancialInfo, config => config.MapFrom(dto => dto.FinancialInfo))
                .ForPath(dest => dest.ShowingInfo, config => config.MapFrom(dto => dto.ShowingInfo))
                .ForPath(dest => dest.SchoolsInfo, config => config.MapFrom(dto => dto.SchoolsInfo))
                .ForPath(dest => dest.AddressInfo, config => config.MapFrom(dto => dto.AddressInfo))
                .ForPath(dest => dest.PropertyInfo, config => config.MapFrom(dto => dto.PropertyInfo))
                .ForPath(dest => dest.Rooms, config => config.MapFrom(dto => dto.Rooms))
                .ForPath(dest => dest.OpenHouses, config => config.MapFrom(dto => dto.OpenHouses))
                .ForPath(dest => dest.SalesOfficeInfo, config => config.Ignore())
                .ForMember(dest => dest.CompanyId, config => config.Ignore())
                .ForMember(dest => dest.PlanId, config => config.Ignore())
                .ForMember(dest => dest.CommunityId, config => config.Ignore())
                .ForMember(dest => dest.Address, config => config.Ignore())
                .ForMember(dest => dest.PlanName, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedBy, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedOn, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedBy, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedOn, config => config.Ignore())
                .ForMember(dest => dest.IsDeleted, config => config.Ignore())
                .ForMember(dest => dest.PlanName, config => config.Ignore())
                .ForMember(dest => dest.SysTimestamp, config => config.Ignore());

            this.CreateMap<DocumentModels.ListingRequest.ListingSaleRequestDetailQueryResult, ListingSaleRequestDetailResponse>()
                .ForPath(dest => dest.ListingId, config => config.MapFrom(dto => dto.ListingSaleId));

            this.CreateMap<DocumentModels.ListingRequest.ListingRequestStatusFieldsQueryResult, ListingSaleStatusFieldsResponse>();
            this.CreateMap<DocumentModels.ListingRequest.ListingRequestSalePropertyQueryResult, SalePropertyDetailResponse>();
            this.CreateMap<DocumentModels.ListingRequest.SalePropertyQueryResult, SalePropertyResponse>();
            this.CreateMap<DocumentModels.ListingRequest.AddressInfoQueryResult, SaleAddressResponse>();
            this.CreateMap<DocumentModels.ListingRequest.SpacesDimensionsInfoQueryResult, SpacesDimensionsResponse>();
            this.CreateMap<DocumentModels.ListingRequest.FeaturesInfoQueryResult, FeaturesResponse>()
                .ForMember(dto => dto.NeighborhoodAmenities, c => c.MapFrom(dto => dto.NeighborhoodAmenities));
            this.CreateMap<DocumentModels.ListingRequest.FinancialInfoQueryResult, FinancialResponse>();
            this.CreateMap<DocumentModels.ListingRequest.ShowingInfoQueryResult, ShowingResponse>();
            this.CreateMap<DocumentModels.ListingRequest.SchoolsInfoQueryResult, SchoolsResponse>();

            this.CreateMap<DocumentModels.ListingSaleRequestQueryResult, ListingSaleRequestQueryResponse>()
                .ForMember(dest => dest.StreetType, config => config.MapFrom(dto => dto.StreetType.GetValueOrDefault().ToStringFromEnumMember(false)));

            this.CreateMap<SummaryField, SummaryFieldContract>();
            this.CreateMap<SummarySection, SummarySectionContract>();

            this.CreateMap<SaleListing, ShowingTimeDto>();

            this.CreateMap<SaleListing, ListingSaleRequestDto>()
                .ForMember(dest => dest.ListingSaleId, config => config.Ignore())
                .ForMember(dest => dest.SaleProperty, config => config.MapFrom(dto => dto.SaleProperty))
                .ForMember(dest => dest.ShowingTime, config => config.MapFrom(dto => dto));

            this.CreateMap<ListingStatusFieldsInfo, ListingSaleStatusFieldsDto>().ReverseMap();

            this.CreateMap<SaleProperty, SalePropertyDetailDto>()
                .ForMember(dest => dest.FeaturesInfo, config => config.MapFrom(dto => dto.FeaturesInfo))
                .ForMember(dest => dest.FinancialInfo, config => config.MapFrom(dto => dto.FinancialInfo))
                .ForMember(dest => dest.SpacesDimensionsInfo, config => config.MapFrom(dto => dto.SpacesDimensionsInfo))
                .ForMember(dest => dest.ShowingInfo, config => config.MapFrom(dto => dto.ShowingInfo))
                .ForMember(dest => dest.SchoolsInfo, config => config.MapFrom(dto => dto.SchoolsInfo))
                .ForMember(dest => dest.AddressInfo, config => config.MapFrom(dto => dto.AddressInfo))
                .ForMember(dest => dest.PropertyInfo, config => config.MapFrom(dto => dto.PropertyInfo))
                .ForMember(dest => dest.Rooms, config => config.MapFrom(dto => dto.Rooms))
                .ForMember(dest => dest.OpenHouses, config => config.MapFrom(dto => dto.OpenHouses))
                .ForPath(dest => dest.SalePropertyInfo.OwnerName, config => config.MapFrom(dto => dto.OwnerName))
                .ForPath(dest => dest.SalePropertyInfo.PlanId, config => config.MapFrom(dto => dto.PlanId))
                .ForPath(dest => dest.SalePropertyInfo.CommunityId, config => config.MapFrom(dto => dto.CommunityId))
                .ForPath(dest => dest.SalePropertyInfo.CompanyId, config => config.MapFrom(dto => dto.CompanyId));

            this.CreateMap<SpacesDimensionsInfo, SpacesDimensionsDto>();

            this.CreateMap<FeaturesInfo, FeaturesDto>();
            this.CreateMap<FinancialInfo, FinancialDto>();
            this.CreateMap<SaleAddressInfo, SaleAddressDto>();
            this.CreateMap<PropertyInfo, PropertyDto>();
            this.CreateMap<ShowingInfo, ShowingDto>();
            this.CreateMap<SchoolsInfo, SchoolsDto>();
            this.CreateMap<ListingSaleRoom, RoomDto>();
            this.CreateMap<SaleListingOpenHouse, OpenHouseDto>();
            this.CreateMap<PublishInfo, ListingSalePublishInfoDto>().ReverseMap();

            this.CreateMap<ListingSaleRequestDto, ListingRequestValueObject>()
                .ForMember(dest => dest.ExpirationDate, config => config.MapFrom(dto => dto.ExpirationDate))
                .ForMember(dest => dest.ListDate, config => config.MapFrom(dto => dto.ListDate))
                .ForMember(dest => dest.ListPrice, config => config.MapFrom(dto => dto.ListPrice))
                .ForMember(dest => dest.MlsNumber, config => config.MapFrom(dto => dto.MlsNumber))
                .ForMember(dest => dest.MlsStatus, config => config.MapFrom(dto => dto.MlsStatus));

            this.CreateMap<ListingSaleRequestDto, SaleListingRequest>()
                .ForMember(dest => dest.SaleProperty, config => config.Ignore())
                .ForMember(dest => dest.ShowingTimeInfo, config => config.Ignore())
                .ForMember(dest => dest.EntityId, config => config.MapFrom(dto => dto.ListingSaleId))
                .ForMember(dest => dest.CDOM, config => config.Ignore())
                .ForMember(dest => dest.DOM, config => config.Ignore())
                .ForMember(dest => dest.RequestState, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedOn, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedBy, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedOn, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedBy, config => config.Ignore())
                .ForMember(dest => dest.IsDeleted, config => config.Ignore())
                .ForMember(dest => dest.SysTimestamp, config => config.Ignore())
                .ForMember(dest => dest.LegacyId, config => config.Ignore())
                .ForMember(dest => dest.CompanyId, config => config.Ignore());

            this.CreateMap<ListingSaleStatusFieldQueryResult, ListingSaleStatusFieldsResponse>();
            this.CreateMap<SaleListingOpenHouseQueryResult, ListingSaleOpenHouseResponse>();

            this.CreateMap<PropertyInfo, Property>()
                .ForMember(dst => dst.ZipCode, ops => ops.Ignore())
                .ForMember(dst => dst.County, ops => ops.Ignore())
                .ForMember(dst => dst.City, ops => ops.Ignore())
                .ForMember(dst => dst.Subdivision, ops => ops.Ignore());
            this.CreateMap<FinancialInfo, CommunityFinancialInfo>();
            this.CreateMap<ShowingInfo, CommunityShowingInfo>();
            this.CreateMap<FeaturesInfo, Utilities>();
            this.CreateMap<SaleListingOpenHouse, CommunityOpenHouse>()
                .ForMember(dst => dst.CommunityId, ops => ops.Ignore())
                .ForMember(dst => dst.Community, ops => ops.Ignore());
            this.CreateMap<SaleListing, CommunitySale>()
                .ForMember(dst => dst.ProfileInfo, ops => ops.Ignore())
                .ForMember(dst => dst.Financial, ops => ops.MapFrom(src => src.SaleProperty.FinancialInfo))
                .ForMember(dst => dst.Showing, ops => ops.MapFrom(src => src.SaleProperty.ShowingInfo))
                .ForMember(dst => dst.Utilities, ops => ops.MapFrom(src => src.SaleProperty.FeaturesInfo))
                .ForMember(dst => dst.SchoolsInfo, ops => ops.MapFrom(src => src.SaleProperty.SchoolsInfo))
                .ForMember(dst => dst.Property, ops => ops.MapFrom(src => src.SaleProperty.PropertyInfo))
                .ForMember(dst => dst.OpenHouses, ops => ops.MapFrom(src => src.SaleProperty.OpenHouses))
                .ForMember(dst => dst.SaleOffice, ops => ops.Ignore())
                .ForMember(dst => dst.EmailLead, ops => ops.Ignore())
                .ForMember(dst => dst.SaleProperties, ops => ops.Ignore())
                .ForMember(dst => dst.Employees, ops => ops.Ignore())
                .ForMember(dst => dst.CommunityType, ops => ops.Ignore())
                .ForMember(dst => dst.LegacyProfileId, ops => ops.Ignore())
                .ForMember(dst => dst.XmlStatus, ops => ops.Ignore())
                .ForMember(dst => dst.Changes, ops => ops.Ignore())
                .ForMember(dst => dst.LotListings, ops => ops.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.Property.ZipCode = src.SaleProperty.AddressInfo.ZipCode;
                    dst.Property.County = src.SaleProperty.AddressInfo.County;
                    dst.Property.City = src.SaleProperty.AddressInfo.City;
                    dst.Property.Subdivision = src.SaleProperty.AddressInfo.Subdivision;
                    foreach (var openHouse in dst.OpenHouses)
                    {
                        openHouse.CommunityId = src.SaleProperty.CommunityId.Value;
                        openHouse.Community = src.SaleProperty.Community;
                    }
                });
        }
    }
}
