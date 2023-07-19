namespace Husa.Quicklister.Abor.Crosscutting.Tests.Community
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums;

    public static class CommunityHoaTestProvider
    {
        public static CommunityHoa GetCommunityHoaEntity(Guid? communityId = null)
        {
            var name = Faker.Lorem.GetFirstWord();
            var transferFee = Faker.RandomNumber.Next(1, 20);
            var fee = Faker.RandomNumber.Next(1, 20);
            var type = Faker.Enum.Random<BillingFrequency>();
            var website = Faker.Internet.Url();
            var contactPhone = Faker.Phone.Number();

            return new CommunityHoa(communityId ?? Guid.NewGuid(), name, transferFee, fee, type, website, contactPhone);
        }

        public static ICollection<CommunityHoa> GetCommunityHoas(Guid? communityId = null, int? totalElements = 4)
        {
            var data = new List<CommunityHoa>();

            for (var i = 0; i < totalElements; i++)
            {
                data.Add(GetCommunityHoaEntity(communityId));
            }

            return data;
        }
    }
}
