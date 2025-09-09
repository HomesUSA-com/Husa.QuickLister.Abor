namespace Husa.Quicklister.Abor.Api.Filters
{
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Api.Filters;

    public class SavingListingValidationFilterAttribute
        : BaseSavingListingValidatorFilterAttribute<IListingSaleRepository, SaleListing>
    {
    }
}
