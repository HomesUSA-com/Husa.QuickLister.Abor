namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;

    public class LotMappingProfile : Profile
    {
        public LotMappingProfile()
        {
            this.CreateMap<LotPropertyDto, LotPropertyInfo>();
            this.CreateMap<LotFeaturesDto, LotFeaturesInfo>();
            this.CreateMap<LotFinancialDto, LotFinancialInfo>();
            this.CreateMap<LotSchoolsDto, LotSchoolsInfo>();
            this.CreateMap<LotShowingDto, LotShowingInfo>();
        }
    }
}
