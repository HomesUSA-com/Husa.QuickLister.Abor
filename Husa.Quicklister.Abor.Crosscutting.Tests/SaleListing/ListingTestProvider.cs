namespace Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Application.Models;

    public static class ListingTestProvider
    {
        public static SaleProperty GetSalePropertyEntity(
            Guid? listingId = null,
            Guid? companyId = null,
            Guid? communityId = null,
            Guid? planId = null,
            MarketStatuses? marketStatuses = null)
        {
            var listingCompanyId = companyId ?? Guid.NewGuid();
            var listingPlanId = planId ?? Guid.NewGuid();
            var saleProperty = new SaleProperty(
                Faker.Address.StreetName(),
                Faker.RandomNumber.Next(1, 100).ToString(),
                Faker.Enum.Random<Cities>(),
                Faker.Enum.Random<States>(),
                Faker.Address.ZipCode()[..5],
                Faker.Enum.Random<Counties>(),
                DateTime.Now,
                listingCompanyId,
                Faker.Company.Name(),
                communityId ?? Guid.NewGuid(),
                listingPlanId)
            {
                Id = listingId ?? Guid.NewGuid(),
            };
            saleProperty.Plan = new Plan(listingCompanyId, Faker.Lorem.GetFirstWord(), Faker.Lorem.GetFirstWord())
            {
                Id = listingPlanId,
            };

            return saleProperty;
        }

        public static SaleListing GetListingEntity(
            Guid? listingId = null,
            Guid? companyId = null,
            Guid? communityId = null,
            Guid? planId = null,
            MarketStatuses? marketStatuses = null)
        {
            var listingCompanyId = companyId ?? Guid.NewGuid();
            var listingPlanId = planId ?? Guid.NewGuid();
            var listing = new SaleListing(
                marketStatuses ?? MarketStatuses.Active,
                Faker.Address.StreetName(),
                Faker.RandomNumber.Next(1, 100).ToString(),
                Faker.Enum.Random<Cities>(),
                Faker.Enum.Random<States>(),
                Faker.Address.ZipCode()[..5],
                Faker.Enum.Random<Counties>(),
                DateTime.Now,
                listingCompanyId,
                Faker.Company.Name(),
                communityId ?? Guid.NewGuid(),
                listingPlanId,
                Faker.Boolean.Random())
            {
                Id = listingId ?? Guid.NewGuid(),
            };
            listing.SaleProperty.Plan = new Plan(listingCompanyId, Faker.Lorem.GetFirstWord(), Faker.Lorem.GetFirstWord())
            {
                Id = listingPlanId,
            };

            return listing;
        }

        public static ListingSaleDetailRequest GetListingSaleDetailRequest() => new()
        {
            StatusFieldsInfo = new(),
            SaleProperty = new()
            {
                AddressInfo = new()
                {
                    StreetName = Faker.Address.StreetName(),
                    StreetNumber = Faker.RandomNumber.Next(1, 100).ToString(),
                    City = Faker.Enum.Random<Cities>(),
                    ZipCode = Faker.RandomNumber.Next(10000, 99999).ToString(),
                    State = Faker.Enum.Random<States>(),
                    Block = "7",
                    County = Faker.Enum.Random<Counties>(),
                    LotNum = "6",
                    Subdivision = "subdivision name",
                },
                PropertyInfo = new(),
                ShowingInfo = new(),
                SchoolsInfo = new(),
                FeaturesInfo = new()
                {
                    NeighborhoodAmenities = new[] { Faker.Enum.Random<NeighborhoodAmenities>() },
                },
                FinancialInfo = new(),
                SpacesDimensionsInfo = new()
                {
                    StoriesTotal = Faker.Enum.Random<Stories>(),
                },
            },
        };

        public static SalePropertyDetailDto GetSalePropertyDetailDto() => new()
        {
            SpacesDimensionsInfo = new()
            {
                StoriesTotal = Stories.MultiLevel,
                SqFtTotal = 1235,
                DiningAreasTotal = 4,
                FullBathsTotal = 3,
                HalfBathsTotal = 1,
                LivingAreasTotal = 1,
            },
            FeaturesInfo = new(),
            FinancialInfo = new()
            {
                HOARequirement = HoaRequirement.Mandatory,
                BuyersAgentCommission = 5,
            },
            ShowingInfo = new()
            {
                ShowingInstructions = "Call salesperson or come to the model home at 1234 Sample Trail.",
                LockBoxType = LockBoxType.Combo,
                ShowingRequirements = ShowingRequirements.ShowingService,
            },
            SchoolsInfo = new()
            {
                SchoolDistrict = SchoolDistrict.Holland,
                MiddleSchool = MiddleSchool.Holland,
                HighSchool = HighSchool.Holland,
                ElementarySchool = ElementarySchool.Holland,
            },
            PropertyInfo = new(),
            AddressInfo = new()
            {
                County = Counties.Bee,
            },
            SalePropertyInfo = new(),
            Hoas = new HoaDto[] { new() },
            OpenHouses = new OpenHouseDto[] { new() },
            Rooms = new RoomDto[] { new() },
        };
    }
}
