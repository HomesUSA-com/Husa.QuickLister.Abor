namespace Husa.Quicklister.Abor.Crosscutting.Tests
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Xml.Api.Contracts.Response;
    using Husa.Xml.Api.Contracts.Response.Amenities;
    using Husa.Xml.Crosscuting.Enums;
    using Husa.Xml.Domain.Enums;
    using Husa.Xml.Domain.Enums.Xml;

    public static class XmlTestProvider
    {
        public static SubdivisionResponse GetSubdivisionResponse(Guid subdivisionId, Guid? communityId = null) => new()
        {
            Id = subdivisionId,
            Name = "Rosewood Estates",
            CommunityProfileId = communityId,
            DrivingDirections = "From Fort Worth: Take 20 East to 199 and go North. Follow 199 to Southeast Pkwy in Azle. Go left to get onto Texas Loop 344/Main St/Stewart St. Turn left on Stewart Street and continue on Stewart Street for a mile. The community will be on your right after Stewart Bend Court.\r\n\r\nFrom Dallas: Take 30 West and take Exit 13B to Henderson Street. Keep right at the fork to go to 199 and continue North. Follow 199 to Southeast Pkwy in Azle. Go left to get onto Texas Loop 344/Main St/Stewart St. Turn left on Stewart Street and continue on Stewart Street for a mile. The community will be on your right after Stewart Bend Court.",
            TotalTaxRate = Faker.RandomNumber.Next(),
            Latitude = Faker.RandomNumber.Next(),
            Longitude = Faker.RandomNumber.Next(),
            City = Faker.Enum.Random<Cities>().GetEnumDescription(),
            SaleOffice = new()
            {
                StreetName = Faker.Address.StreetName(),
                StreetNum = Faker.RandomNumber.Next().ToString(),
                StreetSuffix = Faker.Address.StreetSuffix(),
                City = Faker.Enum.Random<Cities>().GetEnumDescription(),
                Zip = Faker.Address.ZipCode()[..5],
                Phone = Faker.Phone.Number(),
                Fax = Faker.Phone.Number(),
            },
            LeadsEmails = string.Join(';', Faker.Internet.Email(), Faker.Internet.Email(), Faker.Internet.Email()),
            Service = new List<ServiceResponse>
            {
                new()
                {
                    Name = Faker.Lorem.GetFirstWord(),
                    AssocFee = Faker.RandomNumber.Next(100, 1000),
                    MonthlyFee = Faker.RandomNumber.Next(100, 1000),
                },
            },
            SchoolDistrict = new List<SchoolDistrictResponse>
            {
                new()
                {
                    Name = SchoolDistrict.WaxahachieISD.ToStringFromEnumMember(),
                    School = new List<SchoolResponse>
                    {
                        new()
                        {
                            Name = ElementarySchool.MaryLouHartman.ToStringFromEnumMember(),
                            Type = SchoolType.Elementary,
                        },
                        new()
                        {
                            Name = MiddleSchool.WheatleyEmerson.ToStringFromEnumMember(),
                            Type = SchoolType.Middle,
                        },
                        new()
                        {
                            Name = HighSchool.Wagner.ToStringFromEnumMember(),
                            Type = SchoolType.High,
                        },
                    },
                },
            },
            Amenities = new List<SubAmenityResponse>
            {
                GetSubAmenityResponse(AmenityType.Basketball),
                GetSubAmenityResponse(AmenityType.VaultedCeilings),
                GetSubAmenityResponse(AmenityType.Volleyball),
                GetSubAmenityResponse(AmenityType.Playground),
                GetSubAmenityResponse(AmenityType.Tennis),
                GetSubAmenityResponse(AmenityType.GolfCourse),
                GetSubAmenityResponse(AmenityType.Trails),
                GetSubAmenityResponse(AmenityType.Clubhouse),
            },
            Utility = new List<UtilityResponse>
            {
                GetUtilityResponse(UtilitiesType.Water),
                GetUtilityResponse(UtilitiesType.Sewer),
                GetUtilityResponse(UtilitiesType.Gas),
                GetUtilityResponse(UtilitiesType.Electric),
            },
            OpenHouses = new List<OpenHouseResponse>
            {
                GetOpenHouseResponse(DayOfWeek.Monday),
                GetOpenHouseResponse(DayOfWeek.Saturday),
                GetOpenHouseResponse(DayOfWeek.Friday),
            },
        };
        private static SubAmenityResponse GetSubAmenityResponse(AmenityType amenityType) => new()
        {
            Type = amenityType,
            EntityType = Faker.Enum.Random<AmenityEntityType>(),
            SequencePosition = Faker.RandomNumber.Next(),
        };

        private static UtilityResponse GetUtilityResponse(UtilitiesType utilitiesType) => new()
        {
            Type = utilitiesType,
            Name = utilitiesType.ToString(),
        };

        private static OpenHouseResponse GetOpenHouseResponse(DayOfWeek day) => new()
        {
            Day = day,
            StartTime = "1030",
            EndTime = "2000",
        };
    }
}
