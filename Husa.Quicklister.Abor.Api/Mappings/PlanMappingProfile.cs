namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Plan;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Plan;
    using Husa.Quicklister.Abor.Application.Models.Plan;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Data.Queries.Models.Plan;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class PlanMappingProfile : Profile
    {
        public PlanMappingProfile()
        {
            this.CreateMap<PlanRequestFilter, PlanQueryFilter>();
            this.CreateMap<PlanQueryResult, PlanDataQueryResponse>();
            this.CreateMap<PlanDetailQueryResult, PlanDetailResponse>();

            this.CreateMap<CreatePlanRequest, PlanCreateDto>();
            this.CreateMap<UpdatePlanRequest, UpdatePlanDto>();
            this.CreateMap<UpdatePlanDto, BasePlan>();
            this.CreateMap<RoomDto, PlanRoom>()
                .ForMember(dto => dto.SysCreatedOn, c => c.Ignore())
                .ForMember(dto => dto.Id, c => c.Ignore())
                .ForMember(dto => dto.PlanId, c => c.Ignore())
                .ForMember(dto => dto.SysCreatedBy, c => c.Ignore())
                .ForMember(dto => dto.IsDeleted, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedOn, c => c.Ignore())
                .ForMember(dto => dto.SysModifiedBy, c => c.Ignore())
                .ForMember(dto => dto.SysTimestamp, c => c.Ignore())
                .ForMember(dto => dto.EntityOwnerType, c => c.MapFrom(x => EntityType.Plan.ToString()))
                .ForMember(dto => dto.CompanyId, c => c.Ignore());
        }
    }
}
