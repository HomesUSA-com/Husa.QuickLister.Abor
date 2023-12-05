namespace Husa.Quicklister.Abor.Api.Mappings
{
    using System;
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Xml;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Xml;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.Xml;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Enums.Xml;
    using XmlImportStatus = Husa.Xml.Domain.Enums.ImportStatus;
    using XmlRequest = Husa.Xml.Api.Contracts.Request;
    using XmlResponse = Husa.Xml.Api.Contracts.Response;

    public class XmlMappingProfile : Profile
    {
        public XmlMappingProfile()
        {
            this.CreateMap<XmlListingFilterRequest, XmlListingQueryFilter>();
            this.CreateMap<XmlListingQueryFilter, XmlRequest.ListingRequestFilter>()
                .ForMember(dto => dto.ImportStatus, c => c.MapFrom(x => GetXmlImportStatus(x.ImportStatus)))
                .ForMember(dto => dto.SortBy, c => c.Ignore())
                .ForMember(dto => dto.CommunityIds, c => c.Ignore())
                .ForMember(dto => dto.MarketCode, c => c.Ignore())
                .ForMember(dto => dto.IsDiscrepancyReport, c => c.Ignore())
                .ForMember(dto => dto.CompanyName, c => c.Ignore());
            this.CreateMap<XmlResponse.XmlListingResponse, XmlListingResponse>()
                .ForMember(dto => dto.City, c => c.MapFrom(x => x.City.ToCity(false) ?? Cities.NotApplicable))
                .ForMember(dto => dto.County, c => c.MapFrom(x => x.County.ToCounty(false)));

            this.CreateMap<XmlResponse.XmlListingDetailResponse, ListingSaleDto>()
                .ForMember(dto => dto.MlsStatus, c => c.MapFrom(x => x.Status))
                .ForMember(dto => dto.StreetNumber, c => c.MapFrom(x => x.StreetNum))
                .ForMember(dto => dto.ListPrice, c => c.MapFrom(x => x.Price))
                .ForMember(dto => dto.ZipCode, c => c.MapFrom(x => x.Zip))
                .ForMember(dto => dto.City, c => c.MapFrom(x => x.City.ToCity(false) ?? Cities.NotApplicable))
                .ForMember(dto => dto.County, c => c.MapFrom(x => x.County.ToCounty(false)))
                .ForMember(dto => dto.ConstructionCompletionDate, c => c.Ignore())
                .ForMember(dto => dto.ListingIdToImport, c => c.Ignore())
                .ForMember(dto => dto.IsManuallyManaged, c => c.Ignore())
                .ForMember(dto => dto.UnitNumber, c => c.Ignore())
                .ForMember(dto => dto.LegacyId, c => c.Ignore());

            this.CreateMap<ManagementTraceQueryResult, XmlManagementResponse>();
        }

        private static XmlImportStatus GetXmlImportStatus(ImportStatus status) => status switch
        {
            ImportStatus.Imported => XmlImportStatus.Imported,
            ImportStatus.Deleted => XmlImportStatus.Deleted,
            ImportStatus.Available => XmlImportStatus.Available,
            _ => throw new ArgumentOutOfRangeException(nameof(status)),
        };
    }
}
