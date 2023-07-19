namespace Husa.Quicklister.Abor.Crosscutting.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Xunit;
    using LocalEnumExtensions = Husa.Quicklister.Abor.Crosscutting.Extensions.EnumExtensions;

    public class EnumExtensionsTest
    {
        [Theory]
        [InlineData(BillingFrequency.Annually, "Annually")]
        [InlineData(BillingFrequency.Monthly, "Monthly")]
        [InlineData(BillingFrequency.Quarterly, "Quarterly")]
        [InlineData(BillingFrequency.SemiAnnually, "Semi-Annually")]
        public void GetEnumDescription_ForBillingType_Success(BillingFrequency billingType, string description)
        {
            // Act
            var result = billingType.GetEnumDescription();

            // Assert
            Assert.Equal(description, result);
        }

        [Theory]
        [InlineData(BillingFrequency.Annually, "Annually")]
        [InlineData(BillingFrequency.Monthly, "Monthly")]
        [InlineData(BillingFrequency.Quarterly, "Quarterly")]
        [InlineData(BillingFrequency.SemiAnnually, "Semi-Annually")]
        public void GetEnumValue_ForBillingType_Success(BillingFrequency billingType, string description)
        {
            // Act
            var result = description.GetEnumValueFromDescription<BillingFrequency>();

            // Assert
            Assert.Equal(billingType, result);
        }

        [Theory]
        [InlineData(typeof(Accessibility))]
        [InlineData(typeof(Cities))]
        [InlineData(typeof(CoolingSystem))]
        [InlineData(typeof(Counties))]
        [InlineData(typeof(ElementarySchool))]
        [InlineData(typeof(EnergyFeatures))]
        [InlineData(typeof(Exterior))]
        [InlineData(typeof(ExteriorFeatures))]
        [InlineData(typeof(RoomLevel))]
        [InlineData(typeof(RoomType))]
        [InlineData(typeof(RoomFeatures))]
        [InlineData(typeof(FireplaceDescription))]
        [InlineData(typeof(Floors))]
        [InlineData(typeof(Foundation))]
        [InlineData(typeof(GarageDescription))]
        [InlineData(typeof(GreenCertification))]
        [InlineData(typeof(GreenFeatures))]
        [InlineData(typeof(HeatingFuel))]
        [InlineData(typeof(HeatingSystem))]
        [InlineData(typeof(HighSchool))]
        [InlineData(typeof(HoaRequirement))]
        [InlineData(typeof(HomeFaces))]
        [InlineData(typeof(HousingStyle))]
        [InlineData(typeof(HowSold))]
        [InlineData(typeof(Inclusions))]
        [InlineData(typeof(LotDescription))]
        [InlineData(typeof(LotImprovements))]
        [InlineData(typeof(MiddleSchool))]
        [InlineData(typeof(MlsArea))]
        [InlineData(typeof(NeighborhoodAmenities))]
        [InlineData(typeof(Occupancy))]
        [InlineData(typeof(OtherParking))]
        [InlineData(typeof(PrivatePool))]
        [InlineData(typeof(ProposedTerms))]
        [InlineData(typeof(RoofDescription))]
        [InlineData(typeof(SchoolDistrict))]
        [InlineData(typeof(SellerConcessionDescription))]
        [InlineData(typeof(Showing))]
        [InlineData(typeof(SpecialtyRooms))]
        [InlineData(typeof(SqFtSource))]
        [InlineData(typeof(States))]
        [InlineData(typeof(Stories))]
        [InlineData(typeof(WaterSewer))]
        [InlineData(typeof(WindowCoverings))]
        public void EnumHasNoDuplicates(Type enumType)
        {
            // Arrange
            var enumValues = Enum.GetValues(enumType);
            var toStringCollectionFromEnumMembers = typeof(LocalEnumExtensions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == nameof(LocalEnumExtensions.ToStringCollectionFromEnumMembers))
                .MakeGenericMethod(enumType);

            // Act
            var members = (IEnumerable<string>)toStringCollectionFromEnumMembers.Invoke(obj: null, new[] { enumValues });

            // Assert
            Assert.All(members, member => Assert.Single(members, m => m == member));
        }
    }
}
