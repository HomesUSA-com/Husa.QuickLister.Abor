namespace Husa.Quicklister.Abor.Domain.Comparers
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class ListingHoaComparer : IEqualityComparer<IProvideHoaInfo>
    {
        public bool Equals(IProvideHoaInfo x, IProvideHoaInfo y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.BillingFrequency == y.BillingFrequency &&
                   x.ContactPhone == y.ContactPhone &&
                   x.Fee == y.Fee &&
                   x.Name == y.Name &&
                   x.TransferFee == y.TransferFee &&
                   x.Website == y.Website &&
                   x.ContactPhone == y.ContactPhone;
        }

        public int GetHashCode(IProvideHoaInfo hoa)
        {
            if (ReferenceEquals(hoa, null))
            {
                return 0;
            }

            int hasHoaBillingFrequency = hoa.BillingFrequency.GetHashCode();
            int hasContactPhone = !string.IsNullOrEmpty(hoa.ContactPhone) ? hoa.ContactPhone.GetHashCode() : 0;
            int hasFee = hoa.Fee.GetHashCode();
            int hasName = hoa.Name.GetHashCode();
            int hasTransferFee = hoa.TransferFee.GetHashCode();
            int hasWebsite = !string.IsNullOrEmpty(hoa.Website) ? hoa.Website.GetHashCode() : 0;

            return hasHoaBillingFrequency ^
                   hasContactPhone ^
                   hasFee ^
                   hasName ^
                   hasTransferFee ^
                   hasWebsite;
        }
    }
}
