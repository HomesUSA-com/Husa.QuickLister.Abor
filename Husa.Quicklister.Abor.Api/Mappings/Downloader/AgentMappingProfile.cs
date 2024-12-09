namespace Husa.Quicklister.Abor.Api.Mappings.Downloader
{
    using AutoMapper;
    using Husa.Downloader.CTX.ServiceBus.Contracts;
    using Husa.Quicklister.Abor.Application.Models.Agent;
    using Husa.Quicklister.Extensions.Domain.Entities.Agent;

    public class AgentMappingProfile : Profile
    {
        public AgentMappingProfile()
        {
            this.CreateMap<AgentMessage, AgentDto>()
                .ForMember(dto => dto.MarketUniqueId, am => am.MapFrom(x => x.EntityKey))
                .ForMember(dto => dto.MarketModified, am => am.MapFrom(x => x.ModificationTimestamp))
                .ForMember(dto => dto.CellPhone, am => am.MapFrom(x => x.MobilePhone))
                .ForMember(dto => dto.Fax, am => am.MapFrom(x => x.HomeFax))
                .ForMember(dto => dto.Status, am => am.MapFrom(x => x.MLSStatus))
                .ForMember(dto => dto.OfficeId, am => am.MapFrom(x => x.OfficeKey))
                .ForMember(dto => dto.WorkPhone, am => am.MapFrom(x => x.DirectWorkPhone))
                .ForMember(dto => dto.SysModified, am => am.Ignore());

            this.CreateMap<AgentDto, AgentValueObject>()
                .ForMember(dto => dto.OfficeName, am => am.Ignore());
        }
    }
}
