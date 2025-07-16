namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.ShowingTime;

    public class ListingSaleDetailRequest : ListingRequest
    {
        public ListingSaleStatusFieldsRequest StatusFieldsInfo { get; set; }

        [Required]
        public SalePropertyDetailRequest SaleProperty { get; set; }

        public ShowingTimeRequest ShowingTime { get; set; }

        public virtual bool UseShowingTime { get; set; }
    }
}
