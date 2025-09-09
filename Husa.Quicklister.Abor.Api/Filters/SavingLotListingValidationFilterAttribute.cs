namespace Husa.Quicklister.Abor.Api.Filters
{
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Api.Filters;

    public class SavingLotListingValidationFilterAttribute
        : BaseSavingListingValidatorFilterAttribute<ILotListingRepository, LotListing>
    {
    }
}
