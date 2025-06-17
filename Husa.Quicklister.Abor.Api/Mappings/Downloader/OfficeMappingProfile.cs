namespace Husa.Quicklister.Abor.Api.Mappings.Downloader
{
    using AutoMapper;
    using Husa.Downloader.CTX.ServiceBus.Contracts;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Application.Models.Office;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Data.Configuration;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.ValueObjects;

    public class OfficeMappingProfile : Profile
    {
        public OfficeMappingProfile()
        {
            this.CreateMap<OfficeMessage, OfficeDto>()
                .ForMember(dto => dto.Status, om => om.MapFrom(x => x.Status))
                .ForMember(dto => dto.StateOrProvince, om => om.MapFrom(x => x.StateOrProvince))
                .ForMember(dto => dto.Address, om => om.MapFrom(x => x.Address))
                .ForMember(dto => dto.City, om => om.MapFrom(x => x.City.ToEnumFromEnumMember<Cities>()))
                .ForMember(dto => dto.Zip, om => om.MapFrom(x => x.ZipCode.GetSubstring(OfficeConfiguration.ZipCodeLength)))
                .ForMember(dto => dto.ZipExt, om => om.MapFrom(x => x.ZipCodeExt))
                .ForMember(dto => dto.Phone, om => om.MapFrom(x => x.PhoneNumber))
                .ForMember(dto => dto.Name, om => om.MapFrom(x => x.Name))
                .ForMember(dto => dto.MarketUniqueId, om => om.MapFrom(x => x.OfficeKey))
                .ForMember(dto => dto.MarketModified, om => om.MapFrom(x => x.OfficeUpdateDate));
            this.CreateMap<OfficeDto, OfficeValueObject>();
        }
    }
}
