namespace Husa.Quicklister.Abor.Application.Models
{
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;

    public class SaleListingDto : ListingDto
    {
        public ListingSaleStatusFieldsDto StatusFieldsInfo { get; set; }

        public SalePropertyDetailDto SaleProperty { get; set; }
    }
}
