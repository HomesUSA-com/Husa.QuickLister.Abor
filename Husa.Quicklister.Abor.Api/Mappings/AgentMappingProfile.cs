namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Agent;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.Agent;
    using Husa.Quicklister.Extensions.Application.Models.Agents;
    using Husa.Quicklister.Extensions.Data.Queries.Models.Agent;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Entities.Agent;
    using TrestleDownloader = Husa.Downloader.CTX.Api.Contracts.Response.Agent;

    public class AgentMappingProfile : Profile
    {
        public AgentMappingProfile()
        {
            this.CreateMap<AgentRequestFilter, AgentQueryFilter>();
            this.CreateMap<AgentQueryResult, AgentResponse>();
            this.CreateMap<AgentRequestFilter, AgentFilter>();
            this.CreateMap<TrestleDownloader.FullAgentResponse, AgentValueObject>()
                .ForMember(dto => dto.MarketUniqueId, a => a.MapFrom(dto => dto.MlsId))
                .ForMember(dto => dto.OfficeId, a => a.MapFrom(dto => dto.OfficeKey))
                .ForMember(dto => dto.Status, a => a.MapFrom(dto => dto.MLSStatus))
                .ForMember(dto => dto.CellPhone, a => a.MapFrom(dto => dto.MobilePhone))
                .ForMember(dto => dto.WorkPhone, a => a.MapFrom(dto => dto.DirectWorkPhone))
                .ForMember(dto => dto.Fax, a => a.MapFrom(dto => dto.HomeFax))
                .ForMember(dto => dto.MarketModified, a => a.MapFrom(dto => dto.ModificationTimestamp));
            this.CreateMap<SimpleAgentDto, AgentResponse>();
        }
    }
}
