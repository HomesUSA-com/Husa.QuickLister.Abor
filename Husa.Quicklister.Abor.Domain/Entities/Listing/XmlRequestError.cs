namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using XmlExtension = Husa.Quicklister.Extensions.Domain.Entities.Listing.XmlRequestError;

    public class XmlRequestError : XmlExtension
    {
        public XmlRequestError(Guid listingId, string errorMessage)
        : base(listingId, errorMessage)
        {
        }

        public virtual SaleListing SaleListing { get; set; }
    }
}
