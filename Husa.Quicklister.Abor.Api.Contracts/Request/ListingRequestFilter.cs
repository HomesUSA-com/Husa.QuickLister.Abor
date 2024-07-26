namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using ExtensionsContract = Husa.Quicklister.Extensions.Api.Contracts.Request.Listing;

    public class ListingRequestFilter : BaseFilterRequest, ExtensionsContract.IListingRequestFilter
    {
        public IEnumerable<MarketStatuses> MlsStatus { get; set; }
        public ListedType? ListedType { get; set; }
        public string SearchBy { get; set; }
        public Guid? CommunityId { get; set; }
        public Guid? PlanId { get; set; }
        public int? SqftMin { get; set; }
        public int? SqftMax { get; set; }
        public DateTime? CreationStartDate { get; set; }
        public DateTime? CreationEndDate { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string MlsNumber { get; set; }
        public bool? IsCompleteHome { get; set; }
        public string ZipCode { get; set; }
    }
}
