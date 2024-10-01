namespace Husa.Quicklister.Abor.Domain.Extensions.Lot
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public static class MarketUpdateExtensions
    {
        public static void ApplyMarkeyUpdate(
            this LotListing listing,
            LotValueObject lotInfo)
        {
            listing.ListPrice = lotInfo.ListPrice;
            listing.MlsStatus = lotInfo.MlsStatus;
            listing.OwnerName = lotInfo.OwnerName;
            listing.ListDate = lotInfo.ListDate;
            listing.ExpirationDate = lotInfo.ExpirationDate;
            listing.LockedStatus = LockedStatus.NoLocked;
            listing.LockedOn = null;
            listing.LockedBy = null;
            listing.MarketModifiedOn = lotInfo.MarketModifiedOn;
            listing.MarketUpdateStatusFields(lotInfo.StatusFieldsInfo);
            listing.MarketUpdateFeatures(lotInfo.FeaturesInfo);
            listing.MarketUpdateFinancial(lotInfo.FinancialInfo);
            listing.MarketUpdatePropertyInfo(lotInfo.PropertyInfo);
            listing.MarketUpdateAddressInfo(lotInfo.AddressInfo);
            listing.MarketUpdateShowing(lotInfo.ShowingInfo);
            listing.MarketUpdateSchools(lotInfo.SchoolsInfo);
        }

        private static void MarketUpdateFeatures(this LotListing listing, LotFeaturesInfo features)
        {
            ArgumentNullException.ThrowIfNull(features);

            listing.FeaturesInfo.Fencing = features.Fencing;
            listing.FeaturesInfo.WaterSource = features.WaterSource;
            listing.FeaturesInfo.WaterBodyName = features.WaterBodyName;
            listing.FeaturesInfo.View = features.View;
            listing.FeaturesInfo.ExteriorFeatures = features.ExteriorFeatures;
            listing.FeaturesInfo.NeighborhoodAmenities = features.NeighborhoodAmenities;
            listing.FeaturesInfo.WaterSewer = features.WaterSewer;
            listing.FeaturesInfo.UtilitiesDescription = features.UtilitiesDescription;
            listing.FeaturesInfo.WaterfrontFeatures = features.WaterfrontFeatures;
        }

        private static void MarketUpdateFinancial(this LotListing listing, LotFinancialInfo financial)
        {
            ArgumentNullException.ThrowIfNull(financial);

            listing.FinancialInfo.AcceptableFinancing = financial.AcceptableFinancing;
            listing.FinancialInfo.TaxYear = financial.TaxYear;
            listing.FinancialInfo.HoaName = financial.HoaName;
            listing.FinancialInfo.HasHoa = financial.HasHoa;
            listing.FinancialInfo.HoaIncludes = financial.HoaIncludes;
            listing.FinancialInfo.HoaFee = financial.HoaFee;
            listing.FinancialInfo.BillingFrequency = financial.BillingFrequency;
        }

        private static void MarketUpdatePropertyInfo(this LotListing listing, LotPropertyInfo propertyInfo)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo);

            listing.PropertyInfo.LegalDescription = propertyInfo.LegalDescription;
            listing.PropertyInfo.TaxId = propertyInfo.TaxId;
            listing.PropertyInfo.PropertyType = propertyInfo.PropertyType;
            listing.PropertyInfo.MlsArea = propertyInfo.MlsArea;
            listing.PropertyInfo.Latitude = propertyInfo.Latitude;
            listing.PropertyInfo.Longitude = propertyInfo.Longitude;
            listing.PropertyInfo.LotDescription = propertyInfo.LotDescription;
            listing.PropertyInfo.LotSize = propertyInfo.LotSize;
            listing.PropertyInfo.LotDimension = propertyInfo.LotDimension;
            listing.PropertyInfo.TaxLot = propertyInfo.TaxLot;
        }

        private static void MarketUpdateAddressInfo(this LotListing listing, LotAddressInfo addressInfo)
        {
            ArgumentNullException.ThrowIfNull(addressInfo);

            listing.AddressInfo.StreetNumber = addressInfo.StreetNumber;
            listing.AddressInfo.StreetName = addressInfo.StreetName;
            listing.AddressInfo.StreetType = addressInfo.StreetType;
            listing.AddressInfo.StreetDirPrefix = addressInfo.StreetDirPrefix;
            listing.AddressInfo.StreetDirSuffix = addressInfo.StreetDirSuffix;
            listing.AddressInfo.UnitNumber = addressInfo.UnitNumber;
            listing.AddressInfo.City = addressInfo.City;
            listing.AddressInfo.State = addressInfo.State;
            listing.AddressInfo.ZipCode = addressInfo.ZipCode;
            listing.AddressInfo.County = addressInfo.County;
            listing.AddressInfo.Subdivision = addressInfo.Subdivision;
        }

        private static void MarketUpdateShowing(this LotListing listing, LotShowingInfo showing)
        {
            ArgumentNullException.ThrowIfNull(showing);

            listing.ShowingInfo.ShowingRequirements = showing.ShowingRequirements;
            listing.ShowingInfo.Directions = showing.Directions;
            listing.ShowingInfo.ShowingInstructions = showing.ShowingInstructions;
            listing.ShowingInfo.OwnerName = showing.OwnerName;
        }

        private static void MarketUpdateSchools(this LotListing listing, LotSchoolsInfo schools)
        {
            ArgumentNullException.ThrowIfNull(schools);

            listing.SchoolsInfo.SchoolDistrict = schools.SchoolDistrict;
            listing.SchoolsInfo.MiddleSchool = schools.MiddleSchool;
            listing.SchoolsInfo.ElementarySchool = schools.ElementarySchool;
            listing.SchoolsInfo.HighSchool = schools.HighSchool;
        }

        private static void MarketUpdateStatusFields(this LotListing listing, ListingStatusFieldsInfo statusFieldsInfo)
        {
            ArgumentNullException.ThrowIfNull(statusFieldsInfo);

            listing.StatusFieldsInfo.SellConcess = statusFieldsInfo.SellConcess;
            listing.StatusFieldsInfo.ClosePrice = statusFieldsInfo.ClosePrice;
            listing.StatusFieldsInfo.EstimatedClosedDate = statusFieldsInfo.EstimatedClosedDate;
            listing.StatusFieldsInfo.HasBuyerAgent = statusFieldsInfo.HasBuyerAgent;
            listing.StatusFieldsInfo.ClosedDate = statusFieldsInfo.ClosedDate;
            listing.StatusFieldsInfo.BackOnMarketDate = statusFieldsInfo.BackOnMarketDate;
            listing.StatusFieldsInfo.OffMarketDate = statusFieldsInfo.OffMarketDate;
            listing.StatusFieldsInfo.SaleTerms = statusFieldsInfo.SaleTerms;
            listing.StatusFieldsInfo.PendingDate = statusFieldsInfo.PendingDate;
        }
    }
}
