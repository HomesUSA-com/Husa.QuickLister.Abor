namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using Husa.Downloader.CTX.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Cities = Husa.Quicklister.Abor.Domain.Enums.Domain.Cities;
    using QLExtensions = Husa.Quicklister.Extensions.Domain.Repositories;

    public interface IOfficeRepository : QLExtensions.IOfficeRepository<Office, OfficeValueObject, Cities, StateOrProvince>
    {
    }
}
