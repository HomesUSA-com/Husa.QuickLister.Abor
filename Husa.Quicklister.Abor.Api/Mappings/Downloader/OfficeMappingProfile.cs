namespace Husa.Quicklister.Abor.Api.Mappings.Downloader
{
    using AutoMapper;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Quicklister.Abor.Application.Models.Office;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.ValueObjects;

    public class OfficeMappingProfile : Profile
    {
        public OfficeMappingProfile()
        {
            this.CreateMap<OfficeMessage, OfficeDto>()
                .ForMember(dto => dto.MarketUniqueId, ovm => ovm.MapFrom(x => x.OfficeId))
                .ForMember(dto => dto.Name, ovm => ovm.MapFrom(x => x.Name))
                .ForMember(dto => dto.Email, ovm => ovm.MapFrom(x => x.Email))
                .ForMember(dto => dto.Address, ovm => ovm.MapFrom(x => string.Concat(x.StreetNumber, " ", x.StreetName)))
                .ForMember(dto => dto.City, ovm => ovm.MapFrom(x => x.City.ToCity(false)))
                .ForMember(dto => dto.State, ovm => ovm.MapFrom(x => x.State.ToState(false)))
                .ForMember(dto => dto.Zip, ovm => ovm.MapFrom(x => x.ZipCode))
                .ForMember(dto => dto.Phone, ovm => ovm.MapFrom(x => x.PhoneNumber))
                .ForMember(dto => dto.Fax, ovm => ovm.MapFrom(x => x.FaxNumber))
                .ForMember(dto => dto.Status, ovm => ovm.MapFrom(x => x.Status))
                .ForMember(dto => dto.LicenseNumber, ovm => ovm.MapFrom(x => x.ResponsibleId))
                .ForMember(dto => dto.MarketModified, ovm => ovm.MapFrom(x => x.OfficeUpdateDate));

            this.CreateMap<OfficeDto, OfficeValueObject>();
        }
    }
}
