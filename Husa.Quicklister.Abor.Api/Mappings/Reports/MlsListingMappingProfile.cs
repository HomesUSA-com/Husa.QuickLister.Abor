namespace Husa.Quicklister.Abor.Api.Mappings.Reports
{
    using AutoMapper;
    using Husa.Quicklister.Extensions.Application.Models.Reports;
    using CtxContract = Husa.Downloader.CTX.Api.Contracts;
    using SaborContract = Husa.Downloader.Sabor.Api.Contracts;
    internal class MlsListingMappingProfile : Profile
    {
        public MlsListingMappingProfile()
        {
            this.CreateMap<CtxContract.Response.MlsListingResponse, MlsListing>();
            this.CreateMap<SaborContract.Response.MlsListingResponse, MlsListing>();
        }
    }
}
