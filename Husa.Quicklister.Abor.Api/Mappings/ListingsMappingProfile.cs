namespace Husa.Quicklister.Abor.Api.Mappings
{
    using System;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Quickbooks.Models.Invoice;
    using Husa.PhotoService.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Reports;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.PhotoRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ReverseProspect;
    using Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Models.ReverseProspect;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Models.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Alert;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.Reports;
    using Husa.Quicklister.Extensions.Application.Models.Community;
    using Husa.Quicklister.Extensions.Application.Models.Media;
    using Husa.Quicklister.Extensions.Application.Models.Reports;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.ReverseProspect.Api.Contracts.Response;
    using DownloaderCtxResponse = Husa.Downloader.CTX.Api.Contracts.Response;
    using ExtensionMapping = Husa.Quicklister.Extensions.Api.Mappings.ListingMappingProfile;

    public class ListingsMappingProfile : ExtensionMapping
    {
        public ListingsMappingProfile()
        {
            this.CreateMap<Api.Contracts.Request.ListingRequestFilter, ListingQueryFilter>();
            this.CreateMap<InvoiceRequest, InvoiceDto>();
            this.CreateMap<SalePropertyQueryResult, SalePropertyResponse>();
            this.CreateMap<FeaturesQueryResult, FeaturesResponse>();
            this.CreateMap<FinancialQueryResult, FinancialResponse>();
            this.CreateMap<ListingShowingQueryResult, ShowingResponse>();
            this.CreateMap<SaleAddressQueryResult, SaleAddressResponse>();
            this.CreateMap<PropertyInfoQueryResult, PropertyInfoResponse>()
                .ForMember(dto => dto.MlsArea, pr => pr.MapFrom(dto => dto.MlsArea.HasValue ? dto.MlsArea.Value.ToStringFromEnumMember(false) : null));
            this.CreateMap<RoomQueryResult, RoomResponse>();
            this.CreateMap<ListingSaleQueryResult, ListingResponse>()
               .ForMember(dest => dest.StreetNum, config => config.MapFrom(x => x.StreetNum))
               .ForMember(dest => dest.StreetName, config => config.MapFrom(x => x.StreetName))
               .ForMember(dest => dest.StreetType, config => config.MapFrom(x => x.StreetType.GetValueOrDefault().ToStringFromEnumMember(false)))
               .ForMember(dest => dest.City, config => config.MapFrom(x => x.City))
               .ForMember(dest => dest.County, config => config.MapFrom(x => x.County))
               .ForMember(dest => dest.Subdivision, config => config.MapFrom(x => x.Subdivision))
               .ForMember(dest => dest.ZipCode, config => config.MapFrom(x => x.ZipCode))
               .ForMember(dest => dest.State, config => config.MapFrom(x => x.State))
               .ForMember(dest => dest.ListPrice, config => config.MapFrom(x => x.ListPrice))
               .ForMember(dest => dest.OwnerName, config => config.MapFrom(x => x.OwnerName))
               .ForMember(dest => dest.ListDate, config => config.MapFrom(x => x.ListDate))
               .ForMember(dest => dest.Directions, config => config.MapFrom(x => x.Directions))
               .ForMember(dest => dest.MlsNumber, config => config.MapFrom(x => x.MlsNumber))
               .ForMember(dest => dest.MlsStatus, config => config.MapFrom(x => x.MlsStatus))
               .ForMember(dest => dest.MarketModifiedOn, config => config.MapFrom(x => x.MarketModifiedOn))
               .ForMember(dest => dest.MarketCode, config => config.MapFrom(x => MarketCode.Austin))
               .ForMember(dest => dest.CommunityId, config => config.MapFrom(x => x.CommunityId))
               .ForMember(dest => dest.PlanName, config => config.MapFrom(x => x.PlanName))
               .ForMember(dest => dest.IsCompleteHome, config => config.MapFrom(x => x.IsCompleteHome));

            this.CreateMap<SpacesDimensionsQueryResult, SpacesDimensionsResponse>();

            this.CreateMap<SalePropertyDetailQueryResult, SalePropertyDetailResponse>();
            this.CreateMap<PublishInfoQueryResult, PublishInfoResponse>();
            this.CreateMap<ListingSaleQueryDetailResult, ListingSaleDetailResponse>()
               .ForMember(dest => dest.StreetNum, config => config.MapFrom(x => x.SaleProperty.AddressInfo.StreetNumber))
               .ForMember(dest => dest.StreetName, config => config.MapFrom(x => x.SaleProperty.AddressInfo.StreetName))
               .ForMember(dest => dest.StreetType, config => config.MapFrom(x => x.SaleProperty.AddressInfo.StreetType))
               .ForMember(dest => dest.City, config => config.MapFrom(x => x.SaleProperty.AddressInfo.City))
               .ForMember(dest => dest.County, config => config.MapFrom(x => x.SaleProperty.AddressInfo.County))
               .ForMember(dest => dest.Subdivision, config => config.MapFrom(x => x.SaleProperty.AddressInfo.Subdivision))
               .ForMember(dest => dest.ZipCode, config => config.MapFrom(x => x.SaleProperty.AddressInfo.ZipCode))
               .ForMember(dest => dest.CommunityId, config => config.MapFrom(x => x.SaleProperty.SalePropertyInfo.CommunityId))
               .ForMember(dest => dest.State, config => config.MapFrom(x => x.SaleProperty.AddressInfo.State))
               .ForMember(dest => dest.ListPrice, config => config.MapFrom(x => x.ListPrice))
               .ForMember(dest => dest.OwnerName, config => config.MapFrom(x => x.OwnerName))
               .ForMember(dest => dest.ListDate, config => config.MapFrom(x => x.ListDate))
               .ForMember(dest => dest.MlsNumber, config => config.MapFrom(x => x.MlsNumber))
               .ForMember(dest => dest.MlsStatus, config => config.MapFrom(x => x.MlsStatus))
               .ForMember(dest => dest.MarketModifiedOn, config => config.MapFrom(x => x.MarketModifiedOn))
               .ForMember(dest => dest.MarketCode, config => config.MapFrom(x => MarketCode.Austin))
               .ForMember(dest => dest.IsCompleteHome, config => config.MapFrom(x => x.SaleProperty.PropertyInfo.ConstructionStage == ConstructionStage.Complete))
               .ForMember(dest => dest.PlanName, config => config.MapFrom(x => x.PlanName))
               .ForMember(dest => dest.XmlListingId, config => config.MapFrom(x => x.XmlListingId == null || x.XmlListingId == Guid.Empty ? x.XmlDiscrepancyListingId : x.XmlListingId));

            this.CreateMap<SalePropertyRequest, SalePropertyDto>();
            this.CreateMap<FeaturesRequest, FeaturesDto>()
                .ForMember(dto => dto.IsNewConstruction, r => r.Ignore());
            this.CreateMap<FinancialRequest, FinancialDto>();
            this.CreateMap<ShowingRequest, ShowingDto>();

            this.CreateMap<SaleAddressRequest, SaleAddressDto>();
            this.CreateMap<PropertyInfoRequest, PropertyDto>();
            this.CreateMap<RoomRequest, RoomDto>();
            this.CreateMap<ListingRequest, ListingDto>();

            this.CreateMap<ReverseProspectInformationDto, ReverseProspectInformationResponse>();
            this.CreateMap<ReverseProspectDataDto, ReverseProspectDataResponse>();
            this.CreateMap<ReverseProspectData, ReverseProspectDataDto>();

            this.CreateMap<SpacesDimensionsRequest, SpacesDimensionsDto>();

            this.CreateMap<SalePropertyDetailRequest, SalePropertyDetailDto>()
                .ForMember(dto => dto.Id, c => c.Ignore());
            this.CreateMap<ListingSaleDetailRequest, SaleListingDto>();

            this.CreateMap<QuickCreateListingRequest, QuickCreateListingDto>()
                .ForMember(dto => dto.LegacyId, c => c.Ignore());

            this.CreateMap<ShowingDto, ShowingInfo>();

            this.CreateMap<RoomDto, ListingSaleRoom>()
                .ForMember(dto => dto.Id, c => c.MapFrom(x => x.Id))
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore())
                .ForMember(dto => dto.SalePropertyId, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedBy, c => c.Ignore())
                .ForMember(dto => dto.IsDeleted, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedBy, c => c.Ignore())
                .ForMember(dto => dto.SysTimestamp, c => c.Ignore())
                .ForMember(dto => dto.EntityOwnerType, c => c.MapFrom(x => EntityType.SaleProperty.ToString()))
                .ForMember(dto => dto.CompanyId, c => c.Ignore());

            this.CreateMap<OpenHouseDto, SaleListingOpenHouse>()
                .ForMember(dto => dto.OpenHouseType, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore())
                .ForMember(dto => dto.Id, c => c.Ignore())
                .ForMember(dto => dto.SalePropertyId, c => c.Ignore())
                .ForMember(dto => dto.SaleProperty, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedBy, c => c.Ignore())
                .ForMember(dto => dto.IsDeleted, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedBy, c => c.Ignore())
                .ForMember(dto => dto.SysTimestamp, c => c.Ignore())
                .ForMember(dto => dto.CompanyId, c => c.Ignore());

            this.CreateMap<SpacesDimensionsDto, SpacesDimensionsInfo>();

            this.CreateMap<SalePropertyDto, SaleProperty>()
                .ForMember(dto => dto.SalesOfficeInfo, c => c.Ignore())
                .ForMember(dto => dto.FinancialInfo, c => c.Ignore())
                .ForMember(dto => dto.FeaturesInfo, c => c.Ignore())
                .ForMember(dto => dto.SpacesDimensionsInfo, c => c.Ignore())
                .ForMember(dto => dto.SchoolsInfo, c => c.Ignore())
                .ForMember(dto => dto.ShowingInfo, c => c.Ignore())
                .ForMember(dto => dto.AddressInfo, c => c.Ignore())
                .ForMember(dto => dto.PropertyInfo, c => c.Ignore())
                .ForMember(dto => dto.OpenHouses, c => c.Ignore())
                .ForMember(dto => dto.Rooms, c => c.Ignore())
                .ForMember(dto => dto.CompanyId, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedBy, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedBy, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysTimestamp, c => c.Ignore())
                .ForMember(dto => dto.IsDeleted, c => c.Ignore())
                .ForMember(dto => dto.Id, c => c.Ignore())
                .ForMember(dto => dto.Community, c => c.Ignore())
                .ForMember(dto => dto.Plan, c => c.Ignore())
                .ForMember(dto => dto.SaleListings, c => c.Ignore());

            this.CreateMap<SaleListing, LockedListingResponse>()
                .ForMember(dto => dto.LockedByUsername, c => c.Ignore())
                .ForMember(dto => dto.StreetNumber, c => c.MapFrom(src => src.SaleProperty.AddressInfo.StreetNumber))
                .ForMember(dto => dto.CreatedBy, c => c.MapFrom(src => src.SysCreatedBy))
                .ForMember(dto => dto.ModifiedBy, c => c.MapFrom(src => src.SysModifiedBy))
                .ForMember(dto => dto.StreetName, c => c.MapFrom(src => src.SaleProperty.AddressInfo.StreetName));

            this.CreateMap<FinancialDto, FinancialInfo>();
            this.CreateMap<FeaturesDto, FeaturesInfo>();
            this.CreateMap<SaleAddressDto, SaleAddressInfo>()
                .ForMember(dto => dto.City, c => c.MapFrom(src => src.City))
                .ForMember(dto => dto.State, c => c.MapFrom(src => src.State));

            this.CreateMap<PropertyDto, PropertyInfo>();

            this.CreateMap<ListingPublishInfoRequest, ListingSalePublishInfoDto>()
                .ForMember(dto => dto.PublishStatus, c => c.Ignore())
                .ForMember(dto => dto.PublishDate, c => c.Ignore())
                .ForMember(dto => dto.PublishUser, c => c.Ignore());

            this.CreateMap<ListingSaleRequestForUpdate, ListingSaleRequestDto>();
            this.CreateMap<ListingStatusFieldsRequest, ListingStatusFieldsDto>();
            this.CreateMap<ListingSaleStatusFieldsRequest, ListingSaleStatusFieldsDto>();
            this.CreateMap<ListingStatusFieldsDto, ListingStatusFieldsInfo>();
            this.CreateMap<ListingSaleStatusFieldsDto, ListingStatusFieldsInfo>();

            this.CreateMap<PublishInfo, PublishInfoResponse>();

            this.CreateMap<DownloaderCtxResponse.MediaDetailResponse, ListingSaleMediaDto>()
                .ForMember(ls => ls.MediaId, mr => mr.MapFrom(x => x.EntityKey))
                .ForMember(ls => ls.UploadKey, mr => mr.Ignore());

            this.CreateMap<BaseAlertFilterRequest, BaseAlertQueryFilter>();
            this.CreateMap<BaseFilterRequest, BaseQueryFilter>();
            this.CreateMap<Property, PhotoRequestPropertyResponse>()
                .ForMember(ln => ln.UnitNumber, sl => sl.Ignore());

            this.CreateMap<DiscrepancyAnalysisResult, DiscrepancyAnalysisResponse>();
            this.CreateMap<DiscrepancyDetailResult, DiscrepancyDetailResponse>();
        }
    }
}
