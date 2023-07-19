namespace Husa.Quicklister.Abor.Api.Mappings.Migration
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Husa.Extensions.Common;
    using Husa.Migration.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using MigrationRoomType = Husa.Migration.Crosscutting.Enums.RoomType;

    public class PlanMappingProfile : Profile
    {
        private const string NewConstructionValue = "New";

        public PlanMappingProfile()
        {
            this.CreateMap<RoomResponse, PlanRoom>()
                .ForMember(dto => dto.Features, pr => pr.MapFrom(x => x.Features.CsvToEnum<RoomFeatures>(true).ToArray()))
                .ForMember(dto => dto.IsDeleted, pr => pr.MapFrom(x => 0))
                .ForMember(dto => dto.RoomType, pr => pr.MapFrom(x => ToRoomType(x.RoomType)))
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
                .ForMember(dto => dto.NumBedrooms, pr => pr.MapFrom(x => x.Bedrooms))
                .ForMember(dto => dto.GarageDescription, c => c.MapFrom(dto => dto.ParkingDesc.CsvToEnum<GarageDescription>(true)))
                .ForMember(dto => dto.Stories, pr => pr.MapFrom(x => x.NumStories.ToStories()))
                .ForMember(dto => dto.OwnerName, pr => pr.MapFrom(x => x.OwnerName))
                .ForMember(dto => dto.IsNewConstruction, pr => pr.MapFrom(x => x.IsNewConstruction == NewConstructionValue))
                .ForMember(dto => dto.BathsHalf, pr => pr.MapFrom(x => x.HalfBath))
                .ForMember(dto => dto.BathsFull, pr => pr.MapFrom(x => x.FullBath));
        }

        public static RoomType ToRoomType(MigrationRoomType type) =>
            Enum.TryParse(type.ToString(), out RoomType roomType) ? roomType : RoomType.Bed;
    }
}
