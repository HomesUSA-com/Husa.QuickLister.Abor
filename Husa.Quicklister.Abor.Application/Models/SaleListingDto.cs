namespace Husa.Quicklister.Abor.Application.Models
{
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Extensions.Application.Models.ShowingTime;

    public class SaleListingDto : ListingDto
    {
        public ListingSaleStatusFieldsDto StatusFieldsInfo { get; set; }

        public SalePropertyDetailDto SaleProperty { get; set; }

        public ShowingTimeDto ShowingTime { get; set; }

        public virtual bool UseShowingTime { get; set; }
    }
}
