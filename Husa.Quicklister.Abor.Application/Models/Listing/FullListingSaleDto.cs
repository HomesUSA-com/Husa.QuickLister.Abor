namespace Husa.Quicklister.Abor.Application.Models.Listing
{
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;

    public class FullListingSaleDto : ListingDto
    {
        public string SellingAgentId { get; set; }

        public ListingSaleStatusFieldsDto StatusFieldsInfo { get; set; }

        public SalePropertyDetailDto SaleProperty { get; set; }
    }
}
