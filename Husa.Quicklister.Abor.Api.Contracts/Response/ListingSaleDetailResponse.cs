namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;
    using Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;

    public class ListingSaleDetailResponse : ListingResponse
    {
        public ListingSaleStatusFieldsResponse StatusFieldsInfo { get; set; }

        public PublishInfoResponse PublishInfo { get; set; }

        public SalePropertyDetailResponse SaleProperty { get; set; }

        public EmailLeadResponse EmailLead { get; set; }
        public bool LockedByLegacy { get; set; }
        public Guid? UnlockedFromLegacyBy { get; set; }
        public string UnlockedFromLegacyByFullName { get; set; }
        public DateTime? UnlockedFromLegacyOn { get; set; }
    }
}
