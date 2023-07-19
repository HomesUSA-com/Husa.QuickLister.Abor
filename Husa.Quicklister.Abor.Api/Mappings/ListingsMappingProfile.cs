namespace Husa.Quicklister.Abor.Api.Mappings
{
    using System;
    using AutoMapper;
    using Husa.Downloader.Sabor.Api.Contracts.Response;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Notes.Api.Contracts.Response;
    using Husa.PhotoService.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Notes;
    using Husa.Quicklister.Abor.Api.Contracts.Response.PhotoRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Application.Models;
    using Husa.Quicklister.Extensions.Application.Models.Community;
    using Husa.Quicklister.Extensions.Application.Models.Media;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using HusaNotesTypes = Husa.Notes.Domain.Enums.NoteType;

    public class ListingsMappingProfile : Profile
    {
        public ListingsMappingProfile()
        {
            this.CreateMap<Api.Contracts.Request.ListingSaleRequestFilter, ListingQueryFilter>();
            this.CreateMap<Api.Contracts.Request.ListingSaleBillingRequestFilter, ListingSaleBillingQueryFilter>();
            this.CreateMap<SalePropertyQueryResult, SalePropertyResponse>();
            this.CreateMap<FeaturesQueryResult, FeaturesResponse>();
            this.CreateMap<FinancialQueryResult, FinancialResponse>();
            this.CreateMap<SchoolsInfoQueryResult, SchoolsResponse>();
            this.CreateMap<ListingShowingQueryResult, ShowingResponse>();
            this.CreateMap<AddressQueryResult, AddressInfoResponse>();
            this.CreateMap<PropertyInfoQueryResult, PropertyInfoResponse>()
                .ForMember(dto => dto.MlsArea, pr => pr.MapFrom(dto => dto.MlsArea.HasValue ? dto.MlsArea.Value.ToStringFromEnumMember(false) : null));
            this.CreateMap<RoomQueryResult, RoomResponse>();
            this.CreateMap<HoaQueryResult, HoaResponse>();
            this.CreateMap<ListingSaleQueryResult, ListingSaleResponse>()
               .ForMember(dest => dest.StreetNum, config => config.MapFrom(x => x.StreetNum))
               .ForMember(dest => dest.StreetName, config => config.MapFrom(x => x.StreetName))
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
               .ForMember(dest => dest.MarketCode, config => config.MapFrom(x => MarketCode.SanAntonio))
               .ForMember(dest => dest.CommunityId, config => config.MapFrom(x => x.CommunityId))
               .ForMember(dest => dest.PlanName, config => config.MapFrom(x => x.PlanName))
               .ForMember(dest => dest.IsCompleteHome, config => config.MapFrom(x => x.IsCompleteHome));

            this.CreateMap<SpacesDimensionsQueryResult, SpacesDimensionsResponse>();

            this.CreateMap<SalePropertyDetailQueryResult, SalePropertyDetailResponse>();
            this.CreateMap<ListingSalePublishInfoQueryResult, ListingSalePublishInfoResponse>();
            this.CreateMap<ListingSaleQueryDetailResult, ListingSaleDetailResponse>()
               .ForMember(dest => dest.StreetNum, config => config.MapFrom(x => x.SaleProperty.AddressInfo.StreetNumber))
               .ForMember(dest => dest.StreetName, config => config.MapFrom(x => x.SaleProperty.AddressInfo.StreetName))
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
               .ForMember(dest => dest.MarketCode, config => config.MapFrom(x => MarketCode.SanAntonio))
               .ForMember(dest => dest.IsCompleteHome, config => config.MapFrom(x => x.SaleProperty.PropertyInfo.ConstructionStage == ConstructionStage.Complete))
               .ForMember(dest => dest.PlanName, config => config.MapFrom(x => x.PlanName));

            this.CreateMap<SalePropertyRequest, SalePropertyDto>();
            this.CreateMap<FeaturesRequest, FeaturesDto>()
                .ForMember(dto => dto.IsNewConstruction, r => r.Ignore());
            this.CreateMap<FinancialRequest, FinancialDto>();
            this.CreateMap<SchoolsRequest, SchoolsDto>();
            this.CreateMap<ShowingRequest, ShowingDto>();
            this.CreateMap<AddressInfoRequest, AddressDto>();
            this.CreateMap<PropertyInfoRequest, PropertyDto>();
            this.CreateMap<RoomRequest, RoomDto>();
            this.CreateMap<HoaRequest, HoaDto>();
            this.CreateMap<ListingRequest, ListingDto>()
                .ForMember(dto => dto.LockedStatus, c => c.Ignore())
                .ForMember(dto => dto.LockedBy, c => c.Ignore())
                .ForMember(dto => dto.LockedOn, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore())
                .ForMember(dto => dto.CreatedBy, c => c.Ignore())
                .ForMember(dto => dto.ModifiedBy, c => c.Ignore());

            this.CreateMap<SpacesDimensionsRequest, SpacesDimensionsDto>();

            this.CreateMap<SalePropertyDetailRequest, SalePropertyDetailDto>()
                .ForMember(dto => dto.Id, c => c.Ignore());
            this.CreateMap<ListingSaleDetailRequest, SaleListingDto>()
                .ForMember(dto => dto.LockedStatus, c => c.Ignore())
                .ForMember(dto => dto.LockedBy, c => c.Ignore())
                .ForMember(dto => dto.LockedOn, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore())
                .ForMember(dto => dto.CreatedBy, c => c.Ignore())
                .ForMember(dto => dto.ModifiedBy, c => c.Ignore());

            this.CreateMap<ListingSaleRequest, ListingSaleDto>();

            this.CreateMap<ShowingDto, ShowingInfo>()
                .ForMember(dest => dest.Showing, config => config.MapFrom(dto => dto.Showing));
            this.CreateMap<SchoolsDto, SchoolsInfo>();

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

            this.CreateMap<HoaDto, SaleListingHoa>()
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore())
                .ForMember(dto => dto.SalePropertyId, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedBy, c => c.Ignore())
                .ForMember(dto => dto.IsDeleted, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedBy, c => c.Ignore())
                .ForMember(dto => dto.Id, c => c.Ignore())
                .ForMember(dto => dto.SaleProperty, c => c.Ignore())
                .ForMember(dto => dto.SysTimestamp, c => c.Ignore())
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

            this.CreateMap<SaleListingDto, Listing>()
                .ForMember(dto => dto.CDOM, c => c.Ignore())
                .ForMember(dto => dto.XmlListingId, c => c.Ignore())
                .ForMember(dto => dto.XmlDiscrepancyListingId, c => c.Ignore())
                .ForMember(dto => dto.DOM, c => c.Ignore())
                .ForMember(dto => dto.MarketUniqueId, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedBy, c => c.Ignore())
                .ForMember(dto => dto.IsDeleted, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedBy, c => c.Ignore())
                .ForMember(dto => dto.SysTimestamp, c => c.Ignore())
                .ForMember(dto => dto.CompanyId, c => c.Ignore())
                .ForMember(dto => dto.LastPhotoRequestCreationDate, c => c.Ignore())
                .ForMember(dto => dto.LastPhotoRequestId, c => c.Ignore())
                .ForMember(dto => dto.IsPhotosDeclined, c => c.Ignore())
                .ForMember(dto => dto.PhotosDeclinedBy, c => c.Ignore())
                .ForMember(dto => dto.PhotosDeclinedOn, c => c.Ignore());

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
                .ForMember(dto => dto.ListingSaleHoas, c => c.Ignore())
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
            this.CreateMap<AddressDto, AddressInfo>()
                .ForMember(dto => dto.City, c => c.MapFrom(src => src.City))
                .ForMember(dto => dto.State, c => c.MapFrom(src => src.State));

            this.CreateMap<PropertyDto, PropertyInfo>();

            this.CreateMap<ListingSalePublishInfoRequest, ListingSalePublishInfoDto>()
                .ForMember(dto => dto.PublishStatus, c => c.Ignore())
                .ForMember(dto => dto.PublishDate, c => c.Ignore())
                .ForMember(dto => dto.PublishUser, c => c.Ignore());

            this.CreateMap<ListingSaleRequestForUpdate, ListingSaleRequestDto>()
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore());
            this.CreateMap<ListingStatusFieldsRequest, ListingStatusFieldsDto>();
            this.CreateMap<ListingSaleStatusFieldsRequest, ListingSaleStatusFieldsDto>();
            this.CreateMap<ListingStatusFieldsDto, ListingStatusFieldsInfo>();
            this.CreateMap<ListingSaleStatusFieldsDto, ListingSaleStatusFieldsInfo>();

            this.CreateMap<Note, NotesResponse>()
                .ForMember(ln => ln.ModifiedBy, n => n.Ignore())
                .ForMember(ln => ln.SysModifiedBy, n => n.Ignore())
                .ForMember(ln => ln.CreatedBy, n => n.Ignore())
                .ForMember(ln => ln.SysModifiedOn, n => n.Ignore())
                .ForMember(ln => ln.LockedByUsername, sl => sl.Ignore())
                .ForMember(ln => ln.LockedBy, sl => sl.Ignore())
                .ForMember(ln => ln.Type, sl => sl.MapFrom(src => GetNoteType(src.Type)));

            this.CreateMap<NoteDetailResult, NotesResponse>();

            this.CreateMap<PublishInfo, ListingSalePublishInfoResponse>();

            this.CreateMap<MediaResponse, ListingSaleMediaDto>();

            this.CreateMap<Api.Contracts.Request.BaseAlertFilterRequest, BaseAlertQueryFilter>();
            this.CreateMap<Api.Contracts.Request.BaseFilterRequest, BaseQueryFilter>();
            this.CreateMap<Property, PhotoRequestPropertyResponse>()
                .ForMember(ln => ln.UnitNumber, sl => sl.Ignore());
        }

        private static NoteType GetNoteType(HusaNotesTypes noteType) => noteType switch
        {
            HusaNotesTypes.Residential => NoteType.Residential,
            HusaNotesTypes.CommunityProfile => NoteType.CommunityProfile,
            HusaNotesTypes.PlanProfile => NoteType.PlanProfile,
            HusaNotesTypes.Lot => NoteType.Lot,
            HusaNotesTypes.Lease => NoteType.Lease,
            HusaNotesTypes.ListingRequest => NoteType.ListingRequest,
            _ => throw new ArgumentOutOfRangeException(nameof(noteType)),
        };
    }
}
