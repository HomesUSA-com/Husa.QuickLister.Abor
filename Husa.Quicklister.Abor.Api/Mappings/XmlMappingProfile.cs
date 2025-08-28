namespace Husa.Quicklister.Abor.Api.Mappings
{
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Xml;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using ExtensionsMapping = Husa.Quicklister.Extensions.Api.Mappings;
    using XmlResponse = Husa.Xml.Api.Contracts.Response;

    public class XmlMappingProfile : ExtensionsMapping.XmlMappingProfile
    {
        public XmlMappingProfile()
            : base()
        {
            this.CreateMap<XmlResponse.XmlListingResponse, XmlListingResponse>()
                .ForMember(dto => dto.City, c => c.MapFrom(x => x.City.ToCity(false) ?? Cities.NotApplicable))
                .ForMember(dto => dto.County, c => c.MapFrom(x => x.County.ToCounty(false)));

            this.CreateMap<XmlResponse.XmlListingDetailResponse, QuickCreateListingDto>()
                .ForMember(dto => dto.MlsStatus, c => c.MapFrom(x => x.Status))
                .ForMember(dto => dto.StreetNumber, c => c.MapFrom(x => x.StreetNum))
                .ForMember(dto => dto.ListPrice, c => c.MapFrom(x => x.Price))
                .ForMember(dto => dto.ZipCode, c => c.MapFrom(x => x.Zip))
                .ForMember(dto => dto.City, c => c.MapFrom(x => x.City.ToCity(false) ?? Cities.NotApplicable))
                .ForMember(dto => dto.StreetType, c => c.MapFrom(x => x.StreetSuffix.ToStreetType(false) ?? StreetType.None))
                .ForMember(dto => dto.County, c => c.MapFrom(x => x.County.ToCounty(false)))
                .ForMember(dto => dto.ConstructionCompletionDate, c => c.Ignore())
                .ForMember(dto => dto.ListingIdToImport, c => c.Ignore())
                .ForMember(dto => dto.IsManuallyManaged, c => c.Ignore())
                .ForMember(dto => dto.StreetType, c => c.Ignore())
                .ForMember(dto => dto.UnitNumber, c => c.MapFrom(x => x.UnitIndicator))
                .ForMember(dto => dto.LegacyId, c => c.Ignore());

            this.CreateMap<ManagementTraceQueryResult, XmlManagementResponse>();
        }
    }
}
