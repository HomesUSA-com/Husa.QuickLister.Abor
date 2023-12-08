namespace Husa.Quicklister.Abor.Api.Mappings.Migration
{
    using AutoMapper;
    using Husa.Extensions.Common;
    using Husa.Migration.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class PlanMappingProfile : Profile
    {
        public PlanMappingProfile()
        {
            this.CreateMap<RoomResponse, PlanRoom>()
                .ForMember(dto => dto.Features, pr => pr.MapFrom(x => x.Features.CsvToEnum<RoomFeatures>(true)))
                .ForMember(dto => dto.RoomType, pr => pr.MapFrom(x => x.RoomType.ToRoomType()))
                .ForMember(dto => dto.Level, pr => pr.MapFrom(x => x.Level.ToEnumFromEnumMember<RoomLevel>()))
                .ForMember(dto => dto.IsDeleted, pr => pr.MapFrom(x => 0))
                .ForMember(dto => dto.Id, am => am.Ignore())
                .ForMember(dto => dto.SysModifiedOn, am => am.Ignore())
                .ForMember(dto => dto.SysCreatedOn, am => am.Ignore())
                .ForMember(dto => dto.SysModifiedBy, am => am.Ignore())
                .ForMember(dto => dto.SysCreatedBy, am => am.Ignore())
                .ForMember(dto => dto.SysTimestamp, am => am.Ignore())
                .ForMember(dto => dto.CompanyId, am => am.Ignore())
                .ForMember(dto => dto.PlanId, am => am.Ignore())
                .ForMember(dto => dto.EntityOwnerType, am => am.Ignore());

            this.CreateMap<PlanResponse, BasePlan>()
                .ForMember(dto => dto.Name, pr => pr.MapFrom(x => x.PlanName))
                .ForMember(dto => dto.SqFtTotal, pr => pr.MapFrom(x => x.SqFt))
                .ForMember(dto => dto.MainLevelBedroomTotal, pr => pr.MapFrom(x => x.Bedrooms))
                .ForMember(dto => dto.StoriesTotal, pr => pr.MapFrom(x => x.NumStories.ToStories()))
                .ForMember(dto => dto.HalfBathsTotal, pr => pr.MapFrom(x => x.HalfBath))
                .ForMember(dto => dto.FullBathsTotal, pr => pr.MapFrom(x => x.FullBath))
                .ForMember(dto => dto.OwnerName, pr => pr.Ignore());
        }
    }
}
