namespace Husa.Quicklister.Abor.Crosscutting.Tests
{
    using System;
    using System.Collections.Generic;
    using Husa.Downloader.CTX.Api.Contracts.Response;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Xml.Api.Contracts.Response;
    using ListingEntities = Husa.Quicklister.Abor.Domain.Entities.Listing;
    public static class DiscrepancyTestProvider
    {
        public static DataSet<XmlListingResponse> GenerateXmlListingResponse(
        IEnumerable<Tuple<Guid, Guid, string>> listingIds)
        {
            var listings = new List<XmlListingResponse>();
            foreach (var tuple in listingIds)
            {
                var xmlListing = new XmlListingResponse
                {
                    Id = tuple.Item2,
                    Name = "community Name",
                    ResidentialListingId = tuple.Item1,
                    Market = MarketCode.Houston,
                    Price = 100,
                };
                listings.Add(xmlListing);
            }

            return new DataSet<XmlListingResponse>(listings, listings.Count);
        }

        public static IEnumerable<MlsListingResponse> GenerateTrestleListingResponse(
        IEnumerable<Tuple<Guid, Guid, string>> listingIds)
        {
            var listings = new List<MlsListingResponse>();
            foreach (var tuple in listingIds)
            {
                var mlsListing = new MlsListingResponse
                {
                    StreetNumber = "StreetNumber",
                    StreetName = "StreetName",
                    City = "City",
                    Zip = "Zip",
                    Community = "Community",
                    ListPrice = 300,
                    MlsStatus = "Active",
                    MlsNumber = tuple.Item3,
                };
                listings.Add(mlsListing);
            }

            return listings;
        }

        public static IEnumerable<ListingEntities.SaleListing> GenerateListing(
        IEnumerable<Tuple<Guid, Guid, string>> listingIds)
        {
            var listings = new List<ListingEntities.SaleListing>();
            foreach (var tuple in listingIds)
            {
                var qlListing = new ListingEntities.SaleListing(
                    MarketStatuses.Active,
                    "streetName",
                    $"streetNum-{tuple.Item3}",
                    "unitNumber",
                    Cities.Cameron,
                    States.Arizona,
                    "12345",
                    Counties.Anderson,
                    DateTime.Today,
                    Guid.NewGuid(),
                    "ownerName",
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    true);
                qlListing.Id = tuple.Item1;
                qlListing.XmlListingId = tuple.Item2;
                qlListing.MlsNumber = tuple.Item3;
                listings.Add(qlListing);
            }

            return listings;
        }
    }
}
