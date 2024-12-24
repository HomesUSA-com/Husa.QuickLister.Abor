namespace Husa.Quicklister.Abor.Application.Models
{
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using ExtensionsModel = Husa.Quicklister.Extensions.Application.Models.Listing;

    public class QuickCreateListingDto : ExtensionsModel.QuickCreateListingDto<MarketStatuses, Cities, Counties>, IProvideSaleAddress
    {
    }
}
