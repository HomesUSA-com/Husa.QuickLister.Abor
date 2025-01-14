namespace Husa.Quicklister.Abor.Domain.Extensions.JsonImport
{
    using Husa.JsonImport.Api.Contracts.Response.Listing;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.Interfaces.SaleListing;

    public static class JsonImportListingPropertiesExtensions
    {
        public static void ImportRootInfo(this IProvideListingInfo listing, SpecDetailResponse spec)
        {
            if (string.IsNullOrWhiteSpace(listing.MlsNumber))
            {
                listing.MlsNumber = spec.MlsNumber;
            }

            listing.ListPrice = spec.Price;
            listing.MlsStatus = spec.ListStatus.ToMarket();
        }

        public static void Import(this IProvideSpacesDimensions fields, SpecDetailResponse spec)
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

        public static void Import(this IProvideSaleProperty fields, SpecDetailResponse spec)
        {
            fields.ConstructionStage = spec.ConstructionStage.ToMarket();
            fields.LotSize = spec.Lot ?? fields.LotSize;
            fields.ConstructionStartYear = spec.YearBuilt;
        }

        public static void Import(this IProvideSaleFeature fields, SpecDetailResponse spec)
        {
            fields.PropertyDescription = spec.Description ?? fields.PropertyDescription;
            fields.GarageSpaces = spec.GarageCapacity ?? fields.GarageSpaces;
        }

        public static void Import(this IProvideStatusFields fields, SpecStatusResponse spec)
        {
            fields.EstimatedClosedDate = spec.EstimatedCloseDate;
            fields.PendingDate = spec.ContractDate;
            fields.ClosedDate = spec.CloseDate;
            fields.ClosePrice = spec.SalesPrice;
            fields.BackOnMarketDate = spec.MoveInDate;
            fields.SaleTerms = spec.Financing.ToMarket();
        }
    }
}
