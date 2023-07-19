namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Agent;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Agent;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;

    public class AgentMappingProfile : Profile
    {
        public AgentMappingProfile()
        {
            this.CreateMap<AgentRequestFilter, AgentQueryFilter>();
            this.CreateMap<AgentQueryResult, AgentQueryResponse>();
        }
    }
}
