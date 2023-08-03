namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Data.Documents.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Application.Models;
    using Husa.Quicklister.Extensions.Domain.Entities.Request;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;
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
                .ForMember(dest => dest.Hoas, config => config.MapFrom(dto => dto.Hoas))
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
                .ForMember(dest => dest.ListingSaleHoas, config => config.Ignore())
                .ForMember(dest => dest.SaleListings, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedBy, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedOn, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedBy, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedOn, config => config.Ignore())
                .ForMember(dest => dest.IsDeleted, config => config.Ignore())
                .ForMember(dest => dest.SaleListings, config => config.Ignore())
                .ForMember(dest => dest.SysTimestamp, config => config.Ignore());

            this.CreateMap<RequestBaseFilter, RequestBaseQueryFilter>();
            this.CreateMap<ListingSaleRequestFilter, ListingSaleRequestQueryFilter>();

            this.CreateMap<DocumentModels.SummarySectionQueryResult, SummarySectionContract>();
            this.CreateMap<DocumentModels.SummaryFieldQueryResult, SummaryFieldContract>();
            this.CreateMap<SaleListingRequest, ListingSaleRequestDetailResponse>()
                .ForMember(dest => dest.LockedByUsername, config => config.Ignore())
                .ForMember(dest => dest.LockedOn, config => config.Ignore())
                .ForMember(dest => dest.LockedBy, config => config.Ignore())
                .ForMember(dest => dest.LockedStatus, config => config.Ignore())
                .ForMember(dest => dest.CreatedBy, config => config.Ignore())
                .ForMember(dest => dest.ModifiedBy, config => config.Ignore())
                .ForMember(dest => dest.IsFirstRequest, config => config.Ignore());

            this.CreateMap<ListingSaleStatusFieldsDto, StatusFieldsRecord>();
            this.CreateMap<ListingSalePublishInfoDto, PublishFieldsRecord>();
            this.CreateMap<SpacesDimensionsDto, SpacesDimensionsRecord>();

            this.CreateMap<FeaturesDto, FeaturesRecord>();
            this.CreateMap<FinancialDto, FinancialRecord>();
            this.CreateMap<ShowingDto, ShowingRecord>();
            this.CreateMap<SchoolsDto, SchoolRecord>();
            this.CreateMap<AddressDto, AddressRecord>()
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

            this.CreateMap<HoaDto, HoaRecord>()
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
                .ForPath(dest => dest.ListingSaleHoas, config => config.MapFrom(dto => dto.Hoas))
                .ForPath(dest => dest.SalesOfficeInfo, config => config.Ignore())
                .ForMember(dest => dest.CompanyId, config => config.Ignore())
                .ForMember(dest => dest.PlanId, config => config.Ignore())
                .ForMember(dest => dest.CommunityId, config => config.Ignore())
                .ForMember(dest => dest.Address, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedBy, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedOn, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedBy, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedOn, config => config.Ignore())
                .ForMember(dest => dest.IsDeleted, config => config.Ignore())
                .ForMember(dest => dest.SysTimestamp, config => config.Ignore());

            this.CreateMap<DocumentModels.ListingRequest.ListingSaleRequestDetailQueryResult, ListingSaleRequestDetailResponse>();

            this.CreateMap<StatusFieldsRecord, ListingSaleStatusFieldsResponse>();
            this.CreateMap<PublishFieldsRecord, ListingSalePublishInfoResponse>();
            this.CreateMap<SpacesDimensionsRecord, SpacesDimensionsResponse>();
            this.CreateMap<AddressRecord, AddressInfoResponse>();
            this.CreateMap<FeaturesRecord, FeaturesResponse>();
            this.CreateMap<SchoolRecord, SchoolsResponse>();
            this.CreateMap<FinancialRecord, FinancialResponse>()
                .ForMember(dest => dest.BuyersAgentCommissionType, config => config.Ignore());
            this.CreateMap<PropertyRecord, PropertyInfoResponse>();
            this.CreateMap<ShowingRecord, ShowingResponse>();
            this.CreateMap<HoaRecord, HoaResponse>();
            this.CreateMap<RoomRecord, RoomResponse>();
            this.CreateMap<OpenHouseRecord, OpenHouseResponse>();
            this.CreateMap<SalePropertyRecord, SalePropertyDetailResponse>()
                .ForPath(dest => dest.Hoas, config => config.MapFrom(dto => dto.ListingSaleHoas))
                .ForPath(dest => dest.SalePropertyInfo.OwnerName, config => config.MapFrom(dto => dto.OwnerName))
                .ForPath(dest => dest.SalePropertyInfo.PlanId, config => config.MapFrom(dto => dto.PlanId))
                .ForPath(dest => dest.SalePropertyInfo.CommunityId, config => config.MapFrom(dto => dto.CommunityId))
                .ForPath(dest => dest.SalePropertyInfo.CompanyId, config => config.MapFrom(dto => dto.CompanyId));

            this.CreateMap<DocumentModels.ListingRequest.ListingRequestStatusFieldsQueryResult, ListingSaleStatusFieldsResponse>();
            this.CreateMap<DocumentModels.ListingRequest.ListingRequestSalePropertyQueryResult, SalePropertyDetailResponse>();
            this.CreateMap<DocumentModels.ListingRequest.SalePropertyQueryResult, SalePropertyResponse>();
            this.CreateMap<DocumentModels.ListingRequest.AddressInfoQueryResult, AddressInfoResponse>();
            this.CreateMap<DocumentModels.ListingRequest.SpacesDimensionsInfoQueryResult, SpacesDimensionsResponse>();
            this.CreateMap<DocumentModels.ListingRequest.FeaturesInfoQueryResult, FeaturesResponse>()
                .ForMember(dto => dto.NeighborhoodAmenities, c => c.MapFrom(dto => dto.NeighborhoodAmenities));
            this.CreateMap<DocumentModels.ListingRequest.FinancialInfoQueryResult, FinancialResponse>();
            this.CreateMap<DocumentModels.ListingRequest.ShowingInfoQueryResult, ShowingResponse>();
            this.CreateMap<DocumentModels.ListingRequest.SchoolsInfoQueryResult, SchoolsResponse>();

            this.CreateMap<SaleProperty, SalePropertyDetailResponse>()
                .ForMember(dest => dest.FeaturesInfo, config => config.MapFrom(dto => dto.FeaturesInfo))
                .ForMember(dest => dest.SpacesDimensionsInfo, config => config.MapFrom(dto => dto.SpacesDimensionsInfo))
                .ForMember(dest => dest.FinancialInfo, config => config.MapFrom(dto => dto.FinancialInfo))
                .ForMember(dest => dest.AddressInfo, config => config.MapFrom(dto => dto.AddressInfo))
                .ForMember(dest => dest.PropertyInfo, config => config.MapFrom(dto => dto.PropertyInfo))
                .ForMember(dest => dest.ShowingInfo, config => config.MapFrom(dto => dto.ShowingInfo))
                .ForMember(dest => dest.SchoolsInfo, config => config.MapFrom(dto => dto.SchoolsInfo))
                .ForMember(dest => dest.Rooms, config => config.MapFrom(dto => dto.Rooms))
                .ForMember(dest => dest.Hoas, config => config.MapFrom(dto => dto.ListingSaleHoas))
                .ForMember(dest => dest.OpenHouses, config => config.MapFrom(dto => dto.OpenHouses))
                .ForPath(dest => dest.SalePropertyInfo.OwnerName, config => config.MapFrom(dto => dto.OwnerName))
                .ForPath(dest => dest.SalePropertyInfo.PlanId, config => config.MapFrom(dto => dto.PlanId))
                .ForPath(dest => dest.SalePropertyInfo.CommunityId, config => config.MapFrom(dto => dto.CommunityId))
                .ForPath(dest => dest.SalePropertyInfo.CompanyId, config => config.MapFrom(dto => dto.CompanyId));

            this.CreateMap<SpacesDimensionsInfo, SpacesDimensionsResponse>();
            this.CreateMap<FeaturesInfo, FeaturesResponse>();
            this.CreateMap<FinancialInfo, FinancialResponse>();
            this.CreateMap<ShowingInfo, ShowingResponse>();
            this.CreateMap<SchoolsInfo, SchoolsResponse>();
            this.CreateMap<ListingSaleRoom, RoomResponse>();
            this.CreateMap<SaleListingHoa, HoaResponse>();
            this.CreateMap<AddressInfo, AddressInfoResponse>();
            this.CreateMap<PropertyInfo, PropertyInfoResponse>();
            this.CreateMap<SaleListingOpenHouse, OpenHouseResponse>();

            this.CreateMap<DocumentModels.ListingSaleRequestQueryResult, ListingSaleRequestQueryResponse>();

            this.CreateMap<SummaryField, SummaryFieldContract>();
            this.CreateMap<SummarySection, SummarySectionContract>();

            this.CreateMap<SaleListing, ListingSaleRequestDto>()
                .ForMember(dest => dest.ListingSaleId, config => config.Ignore())
                .ForMember(dest => dest.SaleProperty, config => config.MapFrom(dto => dto.SaleProperty));

            this.CreateMap<ListingSaleStatusFieldsInfo, ListingSaleStatusFieldsDto>().ReverseMap();

            this.CreateMap<SaleProperty, SalePropertyDetailDto>()
                .ForMember(dest => dest.FeaturesInfo, config => config.MapFrom(dto => dto.FeaturesInfo))
                .ForMember(dest => dest.FinancialInfo, config => config.MapFrom(dto => dto.FinancialInfo))
                .ForMember(dest => dest.SpacesDimensionsInfo, config => config.MapFrom(dto => dto.SpacesDimensionsInfo))
                .ForMember(dest => dest.ShowingInfo, config => config.MapFrom(dto => dto.ShowingInfo))
                .ForMember(dest => dest.SchoolsInfo, config => config.MapFrom(dto => dto.SchoolsInfo))
                .ForMember(dest => dest.AddressInfo, config => config.MapFrom(dto => dto.AddressInfo))
                .ForMember(dest => dest.PropertyInfo, config => config.MapFrom(dto => dto.PropertyInfo))
                .ForMember(dest => dest.Rooms, config => config.MapFrom(dto => dto.Rooms))
                .ForMember(dest => dest.Hoas, config => config.MapFrom(dto => dto.ListingSaleHoas))
                .ForMember(dest => dest.OpenHouses, config => config.MapFrom(dto => dto.OpenHouses))
                .ForPath(dest => dest.SalePropertyInfo.OwnerName, config => config.MapFrom(dto => dto.OwnerName))
                .ForPath(dest => dest.SalePropertyInfo.PlanId, config => config.MapFrom(dto => dto.PlanId))
                .ForPath(dest => dest.SalePropertyInfo.CommunityId, config => config.MapFrom(dto => dto.CommunityId))
                .ForPath(dest => dest.SalePropertyInfo.CompanyId, config => config.MapFrom(dto => dto.CompanyId));

            this.CreateMap<SpacesDimensionsInfo, SpacesDimensionsDto>();

            this.CreateMap<FeaturesInfo, FeaturesDto>();
            this.CreateMap<FinancialInfo, FinancialDto>();
            this.CreateMap<AddressInfo, AddressDto>();
            this.CreateMap<PropertyInfo, PropertyDto>();
            this.CreateMap<ShowingInfo, ShowingDto>();
            this.CreateMap<SchoolsInfo, SchoolsDto>();
            this.CreateMap<ListingSaleRoom, RoomDto>();
            this.CreateMap<SaleListingHoa, HoaDto>();
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
                .ForMember(dest => dest.CDOM, config => config.Ignore())
                .ForMember(dest => dest.DOM, config => config.Ignore())
                .ForMember(dest => dest.RequestState, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedOn, config => config.Ignore())
                .ForMember(dest => dest.SysCreatedBy, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedOn, config => config.Ignore())
                .ForMember(dest => dest.SysModifiedBy, config => config.Ignore())
                .ForMember(dest => dest.IsDeleted, config => config.Ignore())
                .ForMember(dest => dest.SysTimestamp, config => config.Ignore())
                .ForMember(dest => dest.CompanyId, config => config.Ignore());

            this.CreateMap<SaleListing, ListingSaleResponse>()
               .ForMember(dest => dest.LockedByUsername, config => config.Ignore())
               .ForMember(dest => dest.CreatedBy, config => config.Ignore())
               .ForMember(dest => dest.ModifiedBy, config => config.Ignore())
               .ForMember(dest => dest.StreetNum, config => config.MapFrom(x => x.SaleProperty.AddressInfo.StreetNumber))
               .ForMember(dest => dest.Directions, config => config.MapFrom(x => x.SaleProperty.ShowingInfo.Directions))
               .ForMember(dest => dest.StreetName, config => config.MapFrom(x => x.SaleProperty.AddressInfo.StreetName))
               .ForMember(dest => dest.City, config => config.MapFrom(x => x.SaleProperty.AddressInfo.City))
               .ForMember(dest => dest.County, config => config.MapFrom(x => x.SaleProperty.AddressInfo.County))
               .ForMember(dest => dest.Subdivision, config => config.MapFrom(x => x.SaleProperty.AddressInfo.Subdivision))
               .ForMember(dest => dest.ZipCode, config => config.MapFrom(x => x.SaleProperty.AddressInfo.ZipCode))
               .ForMember(dest => dest.State, config => config.MapFrom(x => x.SaleProperty.AddressInfo.State))
               .ForMember(dest => dest.ListPrice, config => config.MapFrom(x => x.ListPrice))
               .ForMember(dest => dest.OwnerName, config => config.MapFrom(x => x.SaleProperty.OwnerName))
               .ForMember(dest => dest.ListDate, config => config.MapFrom(x => x.ListDate))
               .ForMember(dest => dest.MlsNumber, config => config.MapFrom(x => x.MlsNumber))
               .ForMember(dest => dest.MlsStatus, config => config.MapFrom(x => x.MlsStatus))
               .ForMember(dest => dest.MarketModifiedOn, config => config.MapFrom(x => x.MarketModifiedOn))
               .ForMember(dest => dest.MarketCode, config => config.MapFrom(x => MarketCode.Austin))
               .ForMember(dest => dest.CommunityId, config => config.MapFrom(x => x.SaleProperty.CommunityId))
               .ForMember(dest => dest.IsCompleteHome, config => config.MapFrom(x => x.SaleProperty.PropertyInfo.ConstructionStage == ConstructionStage.Complete))
               .ForMember(dest => dest.PlanName, config => config.MapFrom(x => x.SaleProperty.Plan != null ? x.SaleProperty.Plan.BasePlan.Name : null));

            this.CreateMap<SaleListing, ListingSaleDetailResponse>()
                .ForMember(dest => dest.EmailLead, config => config.Ignore())
                .ForMember(dest => dest.LockedByUsername, config => config.Ignore())
                .ForMember(dest => dest.CreatedBy, config => config.Ignore())
                .ForMember(dest => dest.ModifiedBy, config => config.Ignore())
                .ForMember(dest => dest.StreetNum, config => config.MapFrom(x => x.SaleProperty.AddressInfo.StreetNumber))
                .ForMember(dest => dest.StreetName, config => config.MapFrom(x => x.SaleProperty.AddressInfo.StreetName))
                .ForMember(dest => dest.City, config => config.MapFrom(x => x.SaleProperty.AddressInfo.City))
                .ForMember(dest => dest.County, config => config.MapFrom(x => x.SaleProperty.AddressInfo.County))
                .ForMember(dest => dest.Subdivision, config => config.MapFrom(x => x.SaleProperty.AddressInfo.Subdivision))
                .ForMember(dest => dest.ZipCode, config => config.MapFrom(x => x.SaleProperty.AddressInfo.ZipCode))
                .ForMember(dest => dest.OwnerName, config => config.MapFrom(x => x.SaleProperty.OwnerName))
                .ForMember(dest => dest.CommunityId, config => config.MapFrom(x => x.SaleProperty.CommunityId))
                .ForMember(dest => dest.Directions, config => config.MapFrom(x => x.SaleProperty.ShowingInfo.Directions))
                .ForMember(dest => dest.MarketCode, config => config.MapFrom(x => MarketCode.Austin))
                .ForMember(dest => dest.IsCompleteHome, config => config.MapFrom(x => x.SaleProperty.PropertyInfo.ConstructionStage == ConstructionStage.Complete))
                .ForMember(dest => dest.State, config => config.MapFrom(x => x.SaleProperty.AddressInfo.State))
                .ForMember(dest => dest.PlanName, config => config.MapFrom(x => x.SaleProperty.Plan != null ? x.SaleProperty.Plan.BasePlan.Name : null));

            this.CreateMap<ListingStatusFieldsInfo, ListingStatusFieldsResponse>();
            this.CreateMap<ListingSaleStatusFieldsInfo, ListingSaleStatusFieldsResponse>();
            this.CreateMap<ListingSaleStatusFieldQueryResult, ListingSaleStatusFieldsResponse>();
            this.CreateMap<DocumentModels.ListingRequest.ListingRequestPublishInfoQueryResult, ListingSalePublishInfoResponse>();
        }
    }
}
