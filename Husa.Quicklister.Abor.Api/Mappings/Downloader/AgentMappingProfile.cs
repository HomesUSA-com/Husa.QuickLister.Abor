namespace Husa.Quicklister.Abor.Api.Mappings.Downloader
{
    using AutoMapper;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Quicklister.Abor.Application.Models.Agent;
    using Husa.Quicklister.Abor.Domain.ValueObjects;

    public class AgentMappingProfile : Profile
    {
        public AgentMappingProfile()
        {
            this.CreateMap<AgentMessage, AgentDto>()
                .ForMember(dto => dto.MarketUniqueId, am => am.MapFrom(x => x.AgentId))
                .ForMember(dto => dto.LoginName, am => am.MapFrom(x => x.LoginName))
                .ForMember(dto => dto.OfficeId, am => am.MapFrom(x => x.OfficeId))
                .ForMember(dto => dto.FirstName, am => am.MapFrom(x => x.FirstName))
                .ForMember(dto => dto.LastName, am => am.MapFrom(x => x.LastName))
                .ForMember(dto => dto.Status, am => am.MapFrom(x => x.Status))
                .ForMember(dto => dto.CellPhone, am => am.MapFrom(x => x.CellPhone))
                .ForMember(dto => dto.WorkPhone, am => am.MapFrom(x => x.DirectPhone))
                .ForMember(dto => dto.Email, am => am.MapFrom(x => x.Email))
                .ForMember(dto => dto.Fax, am => am.MapFrom(x => x.OfficePhone))
                .ForMember(dto => dto.OtherPhone, am => am.MapFrom(x => x.HomePhone))
                .ForMember(dto => dto.MarketModified, am => am.MapFrom(x => x.AgentUpdateDate))
                .ForMember(dto => dto.SysModified, am => am.Ignore());

            this.CreateMap<AgentDto, AgentValueObject>();
        }
    }
}
