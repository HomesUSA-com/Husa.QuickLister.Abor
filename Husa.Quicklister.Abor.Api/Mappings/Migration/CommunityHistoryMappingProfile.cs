namespace Husa.Quicklister.Abor.Api.Mappings.Migration
{
    using AutoMapper;
    using Husa.Migration.Api.Contracts.Response.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class CommunityHistoryMappingProfile : Profile
    {
        public CommunityHistoryMappingProfile()
        {
            this.CreateMap<CommunityHistoryResponse, CommunityHistory>()
                .ForMember(dto => dto.Property, cr => cr.MapFrom(x => x.PropertyInfo))
                .ForMember(dto => dto.Utilities, cr => cr.MapFrom(x => x.UtilitiesInfo))
                .ForMember(dto => dto.Financial, cr => cr.MapFrom(x => x.FinancialInfo))
                .ForMember(dto => dto.Showing, cr => cr.MapFrom(x => x.ShowingInfo))
                .ForMember(dto => dto.EmailLead, cr => cr.MapFrom(x => x.EmailLeads))
                .ForMember(dto => dto.LegacyId, cr => cr.MapFrom(x => x.LegacyCommunityHistoryId))
                .ForMember(dto => dto.IsDeleted, cr => cr.MapFrom(x => false))
                .ForMember(dto => dto.CommunityType, cr => cr.MapFrom(x => CommunityType.SaleCommunity))
                .ForMember(dto => dto.Id, cr => cr.Ignore())
                .ForMember(dto => dto.SysModifiedBy, cr => cr.Ignore())
                .ForMember(dto => dto.EntityId, cr => cr.Ignore())
                .ForMember(dto => dto.OpenHouses, cr => cr.Ignore())
                .ForMember(dto => dto.CompanyId, cr => cr.Ignore())
                .ForMember(dto => dto.SysCreatedBy, cr => cr.Ignore())
                .ForMember(dto => dto.XmlStatus, cr => cr.Ignore())
                .ForMember(dto => dto.LastPhotoRequestCreationDate, cr => cr.Ignore())
                .ForMember(dto => dto.LastPhotoRequestId, cr => cr.Ignore());
        }
    }
}
