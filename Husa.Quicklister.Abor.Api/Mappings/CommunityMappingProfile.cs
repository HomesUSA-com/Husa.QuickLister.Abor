namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Community;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Community.CommunityDetail;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Community;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Community.CommunityDetail;
    using Husa.Quicklister.Abor.Application.Models.Community;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.Community;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Application.Models;
    using Husa.Quicklister.Extensions.Application.Models.Community;
    using Husa.Quicklister.Extensions.Data.Queries.Models;

    public class CommunityMappingProfile : Profile
    {
        public CommunityMappingProfile()
        {
            this.CreateMap<CommunityRequestFilter, CommunityQueryFilter>();
            this.CreateMap<CommunityQueryResult, CommunityDataQueryResponse>();
            this.CreateMap<CommunitySale, CommunityDataQueryResponse>()
                .ForMember(dto => dto.OfficePhone, c => c.MapFrom(x => x.ProfileInfo.OfficePhone))
                .ForMember(dto => dto.BackupPhone, c => c.MapFrom(x => x.ProfileInfo.BackupPhone))
                .ForMember(dto => dto.Name, c => c.MapFrom(x => x.ProfileInfo.Name))
                .ForMember(dto => dto.Builder, c => c.MapFrom(x => x.ProfileInfo.OwnerName))
                .ForMember(dto => dto.City, c => c.MapFrom(x => x.Property.City))
                .ForMember(dto => dto.Subdivision, c => c.MapFrom(x => x.Property.Subdivision))
                .ForMember(dto => dto.ZipCode, c => c.MapFrom(x => x.Property.ZipCode))
                .ForMember(dto => dto.County, c => c.MapFrom(x => x.Property.County))
                .ForMember(dto => dto.Market, c => c.MapFrom(x => MarketCode.Austin))
                .ForMember(dto => dto.Directions, c => c.MapFrom(x => x.Showing.Directions))
                .ForMember(dto => dto.ModifiedBy, c => c.MapFrom(x => x.SysModifiedBy));
            this.CreateMap<CommunityEmployeeQueryResult, CommunityEmployeeDataQueryResponse>();

            this.CreateMap<CreateCommunityRequest, CommunitySaleCreateDto>();

            this.CreateMap<CommunityEmployeesRequest, CommunityEmployeesCreateDto>()
                .ForMember(dto => dto.CompanyId, c => c.Ignore())
                .ForMember(dto => dto.CommunityId, c => c.Ignore());
            this.CreateMap<CommunityEmployeesDeleteRequest, CommunityEmployeesDeleteDto>()
                .ForMember(dto => dto.CommunityId, c => c.Ignore());
            this.CreateMap<CommunitySaleRequest, CommunitySaleDto>();
            this.CreateMap<CommunityProfileRequest, CommunityProfileDto>();
            this.CreateMap<CommunityPropertyRequest, CommunityPropertyDto>();

            this.CreateMap<FeaturesAndUtilitiesRequest, FeaturesAndUtilitiesDto>()
                 .ForMember(dto => dto.NeighborhoodAmenities, c => c.MapFrom(dto => dto.NeighborhoodAmenities));
            this.CreateMap<CommunityFinancialRequest, CommunityFinancialDto>();
            this.CreateMap<CommunitySchoolsRequest, CommunitySchoolsDto>();
            this.CreateMap<CommunityShowingRequest, CommunityShowingDto>();
            this.CreateMap<OpenHouseRequest, OpenHouseDto>();
            this.CreateMap<CommunitySalesOfficeRequest, CommunitySalesOfficeDto>();
            this.CreateMap<EmailLeadRequest, EmailLeadDto>();
            this.CreateMap<CommunityHoaRequest, CommunityHoaDto>();

            this.CreateMap<CommunityProfileDto, ProfileInfo>();
            this.CreateMap<CommunityPropertyDto, Property>();
            this.CreateMap<CommunitySalesOfficeDto, CommunitySaleOffice>();
            this.CreateMap<FeaturesAndUtilitiesDto, Utilities>();
            this.CreateMap<CommunitySchoolsDto, SchoolsInfo>();
            this.CreateMap<CommunityFinancialDto, CommunityFinancialInfo>();
            this.CreateMap<CommunityShowingDto, CommunityShowingInfo>();

            this.CreateMap<CommunityHoaDto, CommunityHoa>()
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore())
                .ForMember(dto => dto.Id, c => c.Ignore())
                .ForMember(dto => dto.CommunitySaleId, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedBy, c => c.Ignore())
                .ForMember(dto => dto.IsDeleted, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedBy, c => c.Ignore())
                .ForMember(dto => dto.SysTimestamp, c => c.Ignore())
                .ForMember(dto => dto.CompanyId, c => c.Ignore());

            this.CreateMap<OpenHouseDto, CommunityOpenHouse>()
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore())
                .ForMember(dto => dto.Id, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedBy, c => c.Ignore())
                .ForMember(dto => dto.IsDeleted, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedBy, c => c.Ignore())
                .ForMember(dto => dto.SysTimestamp, c => c.Ignore())
                .ForMember(dto => dto.CompanyId, c => c.Ignore())
                .ForMember(dto => dto.CommunityId, c => c.Ignore())
                .ForMember(dto => dto.OpenHouseType, c => c.Ignore())
                .ForMember(dto => dto.Community, c => c.Ignore());

            this.CreateMap<EmailLeadDto, EmailLead>();
            this.CreateMap<ProfileQueryResult, CommunityProfileResponse>();
            this.CreateMap<PropertyQueryResult, CommunityPropertyResponse>()
                .ForMember(dto => dto.MlsArea, pr => pr.MapFrom(dto => dto.MlsArea.HasValue ? dto.MlsArea.Value.ToStringFromEnumMember(false) : null));
            this.CreateMap<UtilitiesQueryResult, FeaturesAndUtilitiesResponse>()
                .ForMember(dto => dto.NeighborhoodAmenities, c => c.MapFrom(dto => dto.NeighborhoodAmenities));
            this.CreateMap<FinancialSchoolsQueryResult, CommunityFinancialResponse>();
            this.CreateMap<SchoolsInfoQueryResult, CommunitySchoolsResponse>();
            this.CreateMap<ShowingQueryResult, CommunityShowingResponse>();
            this.CreateMap<OpenHousesQueryResult, OpenHouseResponse>();
            this.CreateMap<EmailLeadQueryResult, Api.Contracts.Response.EmailLeadResponse>();
            this.CreateMap<HoaQueryResult, CommunityHoaResponse>();
            this.CreateMap<CommunityDetailQueryResult, CommunitySaleResponse>()
                .ForMember(dto => dto.City, pr => pr.MapFrom(dto => dto.Property.City))
                .ForMember(dto => dto.ZipCode, pr => pr.MapFrom(dto => dto.Property.ZipCode))
                .ForMember(dto => dto.Name, pr => pr.MapFrom(dto => dto.Profile.Name));

            this.CreateMap<SalesOfficeQueryResult, CommunitySalesOfficeResponse>()
                .ForMember(dto => dto.SalesOfficeCity, c => c.MapFrom(dto => dto.SalesOfficeCity.HasValue ? dto.SalesOfficeCity.Value.ToStringFromEnumMember(false) : null));
        }
    }
}
