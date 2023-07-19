namespace Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;

    public static class HoaTestProvider
    {
        public static SaleListingHoa GetListingSaleHoaEntity(Guid? salePropertyId = null)
        {
            var name = Faker.Lorem.GetFirstWord();
            var transferFee = Faker.RandomNumber.Next(1, 20);
            var fee = Faker.RandomNumber.Next(1, 20);
            var type = Faker.Enum.Random<BillingFrequency>();
            var website = Faker.Internet.Url();
            var contactPhone = Faker.Phone.Number();

            return new SaleListingHoa(salePropertyId ?? Guid.NewGuid(), name, transferFee, fee, type, website, contactPhone);
        }

        public static ICollection<SaleListingHoa> GetListingSaleHoas(Guid? salePropertyId = null, int? totalElements = 4)
        {
            var data = new List<SaleListingHoa>();

            for (var i = 0; i < totalElements; i++)
            {
                data.Add(GetListingSaleHoaEntity(salePropertyId));
            }

            return data;
        }
    }
}
