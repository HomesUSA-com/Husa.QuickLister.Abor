namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using Husa.Downloader.CTX.Domain.Enums;
    using Cities = Husa.Quicklister.Abor.Domain.Enums.Domain.Cities;
    using QLExtensionsOffice = Husa.Quicklister.Extensions.Domain.Entities.Office;

    public class OfficeValueObject : QLExtensionsOffice.OfficeValueObject<Cities, StateOrProvince>
    {
        public OfficeValueObject()
        {
        }
    }
}
