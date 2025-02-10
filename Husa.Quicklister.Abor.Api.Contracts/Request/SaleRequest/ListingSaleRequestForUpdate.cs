namespace Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest
{
    using System;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Extensions.Api.Contracts.Models.ShowingTime;

    public class ListingSaleRequestForUpdate : ListingRequest
    {
        public Guid ListingSaleId { get; set; }

        public ListingSaleStatusFieldsRequest StatusFieldsInfo { get; set; }

        public ListingPublishInfoRequest PublishInfo { get; set; }

        public SalePropertyDetailRequest SaleProperty { get; set; }

        public ShowingTimeInfo ShowingTime { get; set; }
    }
}
