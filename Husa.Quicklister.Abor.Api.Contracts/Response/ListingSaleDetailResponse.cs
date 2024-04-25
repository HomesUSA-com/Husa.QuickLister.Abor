namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail;

    public class ListingSaleDetailResponse : ListingResponse
    {
        public ListingSaleStatusFieldsResponse StatusFieldsInfo { get; set; }

        public PublishInfoResponse PublishInfo { get; set; }

        public SalePropertyDetailResponse SaleProperty { get; set; }

        public EmailLeadResponse EmailLead { get; set; }
    }
}
