namespace Husa.Quicklister.Abor.Application.Models
{
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Application.Models.Listing;

    public class ListingSaleDto : QuickCreateListingDto<MarketStatuses, Cities, Counties>
    {
        public string UnitNumber { get; set; }
    }
}
