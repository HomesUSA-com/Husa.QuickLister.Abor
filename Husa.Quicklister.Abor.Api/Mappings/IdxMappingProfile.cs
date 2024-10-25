namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ResidentialIdx;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ResidentialIdx.Media;
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx;
    using Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx.Media;

    public class IdxMappingProfile : Profile
    {
        public IdxMappingProfile()
        {
            this.CreateMap<ResidentialIdxQueryResult, ResidentialIdxResponse>();
            this.CreateMap<ImageIdxQueryResult, ImageIdxResponse>();
            this.CreateMap<ItemIdxQueryResult, ItemIdxResponse>();
            this.CreateMap<MediaIdxQueryResult, MediaIdxResponse>();
        }
    }
}
