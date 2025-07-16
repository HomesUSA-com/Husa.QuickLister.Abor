namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Extensions.Application.Models.ShowingTime;
    using Husa.Quicklister.Extensions.Domain.Entities.ShowingTime;

    public class ShowingTimeMappingProfile : Profile
    {
        public ShowingTimeMappingProfile()
        {
            this.CreateMap<ShowingTime, ShowingTimeDto>();
            this.CreateMap<ContactDto, ShowingTimeContact>()
                .ForMember(dst => dst.Id, ops => ops.Ignore())
                .ForMember(dst => dst.Communities, ops => ops.Ignore())
                .ForMember(dst => dst.Listings, ops => ops.Ignore())
                .ForMember(dst => dst.SysCreatedOn, ops => ops.Ignore())
                .ForMember(dst => dst.SysModifiedOn, ops => ops.Ignore())
                .ForMember(dst => dst.IsDeleted, ops => ops.Ignore())
                .ForMember(dst => dst.SysModifiedBy, ops => ops.Ignore())
                .ForMember(dst => dst.SysCreatedBy, ops => ops.Ignore())
                .ForMember(dst => dst.SysTimestamp, ops => ops.Ignore());
        }
    }
}
