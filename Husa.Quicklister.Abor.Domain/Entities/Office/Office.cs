namespace Husa.Quicklister.Abor.Domain.Entities.Office
{
    using Husa.Downloader.CTX.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Cities = Husa.Quicklister.Abor.Domain.Enums.Domain.Cities;
    using QLExtensionsOffice = Husa.Quicklister.Extensions.Domain.Entities.Office;

    public class Office : QLExtensionsOffice.Office<OfficeValueObject, Cities, StateOrProvince>
    {
        public Office(OfficeValueObject officeValue)
            : base(officeValue)
        {
        }

        public Office()
            : base()
        {
        }
    }
}
