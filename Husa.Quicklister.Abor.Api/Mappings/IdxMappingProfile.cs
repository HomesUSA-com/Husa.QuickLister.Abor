namespace Husa.Quicklister.Abor.Api.Mappings
{
    using Husa.Quicklister.Abor.Api.Contracts.Response.ResidentialIdx;
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx;

    public class IdxMappingProfile : Husa.Quicklister.Extensions.Api.Mappings.IdxMappingProfile
    {
        public IdxMappingProfile()
            : base()
        {
            this.CreateMap<ResidentialIdxQueryResult, ResidentialIdxResponse>();
        }
    }
}
