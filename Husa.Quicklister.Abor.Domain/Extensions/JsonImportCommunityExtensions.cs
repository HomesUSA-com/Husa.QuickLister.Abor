namespace Husa.Quicklister.Abor.Domain.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Common;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Dfw.Domain.Extensions;
    using JsonEnums = Husa.JsonImport.Domain.Enums;

    public static class JsonImportCommunityExtensions
    {
        public static void Import(this CommunitySale community, CommunityResponse jsonCommunity)
        {
            community.Showing.Directions = jsonCommunity.Directions;
            community.Financial.TaxRate = jsonCommunity.TaxRate;
            community.Utilities.NeighborhoodAmenities = jsonCommunity.Amenities.ToMarket();
            community.SaleOffice.Import(jsonCommunity);
            community.ProfileInfo.Import(jsonCommunity);
            community.SchoolsInfo.Import(jsonCommunity);
            community.Property.Import(jsonCommunity);
            community.ImportOpenHouse<OpenHouse, CommunityOpenHouse, CommunitySale>(jsonCommunity.OpenHouses.ToOpenHouses());
        }

        private static void Import(this Property fields, CommunityResponse jsonCommunity)
        {
            fields.City = jsonCommunity.Location.City.ToMarket<Cities>();
            fields.County = jsonCommunity.Location.County.ToMarket<Counties>();
            fields.ZipCode = jsonCommunity.Location.ZipCode;
        }

        private static void Import(this ProfileInfo fields, CommunityResponse jsonCommunity)
        {
            fields.OfficePhone = jsonCommunity.Phone;
            if (jsonCommunity.Location.Latitude.HasValue && jsonCommunity.Location.Longitude.HasValue)
            {
                fields.UseLatLong = true;
                fields.Latitude = jsonCommunity.Location.Latitude;
                fields.Longitude = jsonCommunity.Location.Longitude;
            }
        }

        private static void Import(this CommunitySaleOffice fields, CommunityResponse jsonCommunity)
        {
            fields.StreetName = jsonCommunity.SalesOfficeLocation.StreetName;
            fields.StreetNumber = jsonCommunity.SalesOfficeLocation.StreetNum;
            fields.SalesOfficeCity = jsonCommunity.SalesOfficeLocation.City.ToMarket<Cities>();
            fields.SalesOfficeZip = jsonCommunity.SalesOfficeLocation.ZipCode;
        }

        private static void Import(this SchoolsInfo fields, CommunityResponse jsonCommunity)
        {
            fields.SchoolDistrict = jsonCommunity.SchoolInformation.SchoolDistrict.ToMarket<SchoolDistrict>();
            fields.ElementarySchool = jsonCommunity.SchoolInformation.ElementarySchool.ToMarket<ElementarySchool>();
            fields.MiddleSchool = jsonCommunity.SchoolInformation.MiddleSchool.ToMarket<MiddleSchool>();
            fields.HighSchool = jsonCommunity.SchoolInformation.HighSchool.ToMarket<HighSchool>();
        }

        private static T? ToMarket<T>(this string value)
            where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value.ToEnumOrNullFromEnumMember<T>()
                ?? value.ToEnumOrNullFromDescription<T>()
                ?? value.GetEnumFromText<T>();
        }

        private static ICollection<NeighborhoodAmenities> ToMarket(this ICollection<JsonEnums.Amenities> values)
        {
            if (values == null || !values.Any())
            {
                return Array.Empty<NeighborhoodAmenities>();
            }

            return values.Select(x => x.ToMarket()).Where(x => x != null).Select(x => x.Value).ToList();
        }

        private static NeighborhoodAmenities? ToMarket(this JsonEnums.Amenities value)
            => value switch
            {
                JsonEnums.Amenities.ClubHouse => NeighborhoodAmenities.Clubhouse,
                JsonEnums.Amenities.CommunityPool => NeighborhoodAmenities.Pool,
                JsonEnums.Amenities.FitnessCenter => NeighborhoodAmenities.FitnessCenter,
                JsonEnums.Amenities.Golf => NeighborhoodAmenities.GolfCourse,
                JsonEnums.Amenities.JoggingOrBikePath => NeighborhoodAmenities.WalkBikeHikeJogTrails,
                JsonEnums.Amenities.Lake => NeighborhoodAmenities.Lake,
                JsonEnums.Amenities.Park => NeighborhoodAmenities.Park,
                JsonEnums.Amenities.Playground => NeighborhoodAmenities.Playground,
                JsonEnums.Amenities.TennisCourts => NeighborhoodAmenities.TennisCourt,
                _ => null,
            };
    }
}
