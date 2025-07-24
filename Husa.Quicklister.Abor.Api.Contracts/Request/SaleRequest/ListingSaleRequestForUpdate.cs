namespace Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest
{
    using System;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.ShowingTime;

    public class ListingSaleRequestForUpdate : ListingRequest
    {
        public Guid ListingSaleId { get; set; }

        public ListingSaleStatusFieldsRequest StatusFieldsInfo { get; set; }

        public ListingPublishInfoRequest PublishInfo { get; set; }

        public SalePropertyDetailRequest SaleProperty { get; set; }

        public ShowingTimeRequest ShowingTime { get; set; }

        public virtual bool UseShowingTime { get; set; }
    }
}
