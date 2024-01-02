namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Data.Queries.Models.Alerts;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;
    using Husa.Quicklister.Extensions.Data.Queries.Models.Alerts;

    public class AlertsMappingProfile : Profile
    {
        public AlertsMappingProfile()
        {
            this.CreateMap<UserQueryResult, UserResponse>();
            this.CreateMap<DetailAlertQueryResult, AlertDetailResponse>();
        }
    }
}
