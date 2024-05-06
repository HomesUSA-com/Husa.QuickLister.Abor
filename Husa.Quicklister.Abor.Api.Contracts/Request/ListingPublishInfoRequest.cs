namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class ListingPublishInfoRequest
    {
        public ActionType? PublishType { get; set; }
    }
}
