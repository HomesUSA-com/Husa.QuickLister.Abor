namespace Husa.Quicklister.Abor.Domain.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.JsonImport.Api.Contracts.Response.Listing;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using JsonEnums = Husa.JsonImport.Domain.Enums;

    public static class JsonImportListingExtensions
    {
        public static void Import(this SaleListing listing, SpecDetailResponse spec)
        {
            ArgumentNullException.ThrowIfNull(spec);
            if (string.IsNullOrWhiteSpace(listing.MlsNumber))
            {
                listing.MlsNumber = spec.MlsNumber;
            }

            listing.ListPrice = spec.Price;
            listing.MlsStatus = spec.ListStatus.ToMarket();
            listing.SaleProperty.SpacesDimensionsInfo.Import(spec);
            listing.SaleProperty.PropertyInfo.Import(spec);
            listing.SaleProperty.FeaturesInfo.Import(spec);

            if (listing.MlsStatus != MarketStatuses.Active)
            {
                listing.StatusFieldsInfo.Import(spec.StatusFields);
            }
        }

        public static MarketStatuses ToMarket(this JsonEnums.ListStatus value)
            => value switch
            {
                JsonEnums.ListStatus.Active => MarketStatuses.Active,
                JsonEnums.ListStatus.Pending => MarketStatuses.Pending,
                JsonEnums.ListStatus.Cancelled => MarketStatuses.Canceled,
                JsonEnums.ListStatus.Sold => MarketStatuses.Closed,
                _ => throw new NotImplementedException(),
            };

        public static SaleTerms? ToMarket(this JsonEnums.AcceptableFinancing value)
            => value switch
            {
                JsonEnums.AcceptableFinancing.Cash => SaleTerms.Cash,
                JsonEnums.AcceptableFinancing.FHA => SaleTerms.FHA,
                JsonEnums.AcceptableFinancing.TexasVet => SaleTerms.TexasVet,
                JsonEnums.AcceptableFinancing.Conventional => SaleTerms.Conventional,
                JsonEnums.AcceptableFinancing.USDA => SaleTerms.UsdaEligible,
                JsonEnums.AcceptableFinancing.VA => SaleTerms.VA,
                _ => null,
            };

        public static ICollection<SaleTerms> ToMarket(this ICollection<JsonEnums.AcceptableFinancing> value)
            => value.Select(x => x.ToMarket()).Where(x => x != null).Select(x => x.Value).ToList();

        public static ConstructionStage? ToMarket(this JsonEnums.ConstructionStage value)
            => value switch
            {
                JsonEnums.ConstructionStage.Incomplete => ConstructionStage.Incomplete,
                JsonEnums.ConstructionStage.Complete => ConstructionStage.Complete,
                _ => null,
            };

        private static void Import(this SpacesDimensionsInfo fields, SpecDetailResponse spec)
        {
            fields.FullBathsTotal = spec.Bathrooms ?? fields.FullBathsTotal;
            fields.HalfBathsTotal = spec.HalfBaths ?? fields.HalfBathsTotal;
            fields.StoriesTotal = spec.Stories.ToStories() ?? fields.StoriesTotal;
            fields.MainLevelBedroomTotal = spec.Bedrooms ?? fields.MainLevelBedroomTotal;
            fields.LivingAreasTotal = spec.LivingAreas ?? fields.LivingAreasTotal;
            fields.DiningAreasTotal = spec.DinningAreas ?? fields.DiningAreasTotal;
            if (spec.SquareFeet.HasValue)
            {
                fields.SqFtTotal = (int)spec.SquareFeet.Value;
            }
        }

        private static void Import(this PropertyInfo fields, SpecDetailResponse spec)
        {
            fields.ConstructionStage = spec.ConstructionStage.ToMarket();
            fields.LotSize = spec.Lot ?? fields.LotSize;
            fields.ConstructionStartYear = spec.YearBuilt;
        }

        private static void Import(this FeaturesInfo fields, SpecDetailResponse spec)
        {
            fields.PropertyDescription = spec.Description ?? fields.PropertyDescription;
            fields.GarageSpaces = spec.GarageCapacity ?? fields.GarageSpaces;
        }

        private static void Import(this ListingStatusFieldsInfo fields, SpecStatusResponse spec)
        {
            fields.EstimatedClosedDate = spec.EstimatedCloseDate;
            fields.PendingDate = spec.ContractDate;
            fields.ClosedDate = spec.CloseDate;
            fields.ClosePrice = spec.SalesPrice;
            fields.BackOnMarketDate = spec.MoveInDate;
            fields.SaleTerms = spec.AcceptableFinancing.ToMarket();
        }
    }
}
