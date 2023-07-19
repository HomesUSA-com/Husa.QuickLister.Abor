namespace Husa.Quicklister.Abor.Api.Mappings
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Data.Queries.Models.Alerts;

    public class AlertsMappingProfile : Profile
    {
        public AlertsMappingProfile()
        {
            this.CreateMap<UserQueryResult, UserResponse>();
            this.CreateMap<DetailAlertQueryResult, AlertDetailResponse>()
                .ForMember(target => target.Id, c => c.MapFrom(src => src.Id))
                .ForMember(target => target.MarketCode, c => c.MapFrom(src => src.MarketCode))
                .ForMember(target => target.MlsNumber, c => c.MapFrom(src => src.MlsNumber))
                .ForMember(target => target.MlsStatus, c => c.MapFrom(src => src.MlsStatus))
                .ForMember(target => target.Address, c => c.MapFrom(src => src.Address))
                .ForMember(target => target.Subdivision, c => c.MapFrom(src => src.Subdivision))
                .ForMember(target => target.OwnerName, c => c.MapFrom(src => src.OwnerName))
                .ForMember(target => target.EstimatedClosedDate, c => c.MapFrom(src => src.EstimatedClosedDate))
                .ForMember(target => target.ModifiedBy, c => c.MapFrom(src => src.ModifiedBy))
                .ForMember(target => target.ConstructionCompletionDate, c => c.MapFrom(src => src.ConstructionCompletionDate))
                .ForMember(target => target.ExpirationDate, c => c.MapFrom(src => src.ExpirationDate))
                .ForMember(target => target.BonusExpirationDate, c => c.MapFrom(src => src.BonusExpirationDate))
                .ForMember(target => target.BackOnMarketDate, c => c.MapFrom(src => src.BackOnMarketDate))
                .ForMember(target => target.SysModifiedOn, c => c.MapFrom(src => src.SysModifiedOn))
                .ForMember(target => target.DOM, c => c.MapFrom(src => src.DOM))
                .ForMember(target => target.PublicRemarks, c => c.MapFrom(src => src.PublicRemarks))
                .ForMember(target => target.SysModifiedBy, c => c.MapFrom(src => src.SysModifiedBy))
                .ForMember(target => target.SysCreatedBy, c => c.MapFrom(src => src.SysCreatedBy))
                .ForMember(target => target.CreatedBy, c => c.MapFrom(src => src.CreatedBy))
                .ForMember(target => target.LockedByUsername, c => c.MapFrom(src => src.LockedByUsername))
                .ForMember(target => target.LockedBy, c => c.MapFrom(src => src.LockedBy))
                .ForMember(target => target.Obtained, c => c.MapFrom(src => src.Obtained))
                .ForMember(target => target.MissingField, c => c.MapFrom(src => src.MissingField))
                .ForMember(target => target.OldPrice, c => c.MapFrom(src => src.OldPrice))
                .ForMember(target => target.NewPrice, c => c.MapFrom(src => src.NewPrice))
                .ForMember(target => target.OldStatus, c => c.MapFrom(src => src.OldStatus))
                .ForMember(target => target.NewStatus, c => c.MapFrom(src => src.NewStatus))
                .ForMember(target => target.MarketModifiedOn, c => c.MapFrom(src => src.MarketModifiedOn))
                .ForMember(target => target.UserId, c => c.MapFrom(src => src.UserId))
                .ForMember(target => target.Name, c => c.MapFrom(src => src.Name))
                .ForMember(target => target.Title, c => c.MapFrom(src => src.Title))
                .ForMember(target => target.CompanyId, c => c.MapFrom(src => src.CompanyId))
                .ForMember(target => target.CommunityId, c => c.MapFrom(src => src.CommunityId))
                .ForMember(target => target.CommunityName, c => c.MapFrom(src => src.CommunityName))
                .ForMember(target => target.ListingId, c => c.MapFrom(src => src.ListingId))
                .ForMember(target => target.PhotoRequestId, c => c.MapFrom(src => src.PhotoRequestId))
                .ForMember(target => target.AssignedOn, c => c.MapFrom(src => src.AssignedOn))
                .ForMember(target => target.AssignedTo, c => c.MapFrom(src => src.AssignedTo))
                .ForMember(target => target.ContactDate, c => c.MapFrom(src => src.ContactDate))
                .ForMember(target => target.ScheduleDate, c => c.MapFrom(src => src.ScheduleDate))
                .ForMember(target => target.PublishType, c => c.MapFrom(src => src.PublishType))
                .ForMember(target => target.PublishUser, c => c.MapFrom(src => src.PublishUser))
                .ForMember(target => target.PublishStatus, c => c.MapFrom(src => src.PublishStatus))
                .ForMember(target => target.PublishDate, c => c.MapFrom(src => src.PublishDate));
        }
    }
}
