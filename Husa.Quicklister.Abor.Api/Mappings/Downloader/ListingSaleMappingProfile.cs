namespace Husa.Quicklister.Abor.Api.Mappings.Downloader
{
    using System;
    using System.Globalization;
    using AutoMapper;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Api.Mappings.Downloader.Converters;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Application.Models;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class ListingSaleMappingProfile : Profile
    {
        public ListingSaleMappingProfile()
        {
            this.MapResidentialDownloaderToListingDto();
            this.MapResidentialDownloaderToFullListingDto();
            this.MapFullListingDtoToListingValueObject();
            this.MapWithConverters();

            this.CreateMap<ListingSalePublishInfoDto, PublishInfo>();
        }

        private static CommissionType GetBuyersAgentCommissionType(string buyerAgentComission)
        {
            if (string.IsNullOrEmpty(buyerAgentComission))
            {
                return CommissionType.Percent;
            }

            return buyerAgentComission.Contains('$') ? CommissionType.Amount : CommissionType.Percent;
        }

        private static decimal? GetBuyersAgentCommission(string buyerAgentComission)
        {
            if (string.IsNullOrEmpty(buyerAgentComission))
            {
                return null;
            }

            return buyerAgentComission.Contains('$') ?
                decimal.Parse(buyerAgentComission.Replace("$", string.Empty), CultureInfo.InvariantCulture) :
                decimal.Parse(buyerAgentComission.Replace("%", string.Empty), CultureInfo.InvariantCulture);
        }

        private static DateTime? IsValidDate(DateTime? inputDate) => (inputDate <= DateTime.MinValue) ? (DateTime?)null : inputDate;

        private void MapResidentialDownloaderToListingDto()
        {
            this.CreateMap<ResidentialValueMessage, Application.Models.Listing.ListingDto>()
                .ForMember(dto => dto.MlsNumber, config => config.MapFrom(dto => dto.MlsNumber))
                .ForMember(dto => dto.ExpirationDate, config => config.MapFrom(x => IsValidDate(x.ExpirationDate)))
                .ForMember(dto => dto.PropertyId, lsm => lsm.Ignore())
                .ForMember(dto => dto.MarketUniqueId, lsm => lsm.Ignore())
                .ForMember(dto => dto.MarketModifiedOn, lsm => lsm.Ignore())
                .ForMember(dto => dto.ListDate, lsm => lsm.Ignore())
                .ForMember(dto => dto.ListPrice, lsm => lsm.Ignore())
                .ForMember(dto => dto.MlsStatus, lsm => lsm.Ignore())
                .ForMember(dto => dto.ListType, lsm => lsm.Ignore());
        }

        private void MapResidentialDownloaderToFullListingDto()
        {
            this.CreateMap<ResidentialValueMessage, FullListingSaleDto>()
                .ForMember(dto => dto.DOM, config => config.MapFrom(dto => dto.DOM))
                .ForMember(dto => dto.CDOM, config => config.MapFrom(dto => dto.CDOM))
                .ForMember(dto => dto.ListPrice, config => config.MapFrom(dto => dto.ListPrice))
                .ForMember(dto => dto.ListDate, config => config.MapFrom(dto => dto.ListDate))
                .ForMember(dto => dto.ListType, config => config.MapFrom(x => x.Type.ToListType() ?? ListType.Residential))
                .ForMember(dto => dto.MlsStatus, config => config.MapFrom(x => x.Status.ToMarketStatus() ?? MarketStatuses.Active))
                .ForMember(dto => dto.MarketUniqueId, config => config.MapFrom(dto => dto.ListingId))
                .ForMember(dto => dto.MarketModifiedOn, config => config.MapFrom(dto => dto.ResidentialUpdateDate))
                .ForMember(dto => dto.MlsNumber, config => config.MapFrom(dto => dto.MlsNumber))
                .ForPath(dto => dto.StatusFieldsInfo.ClosePrice, config => config.MapFrom(dto => dto.SoldPrice))
                .ForPath(dto => dto.StatusFieldsInfo.ClosedDate, config => config.MapFrom(dto => dto.CloseDate))
                .ForPath(dto => dto.StatusFieldsInfo.PendingDate, config => config.MapFrom(dto => dto.ContractDate))
                .ForPath(dto => dto.StatusFieldsInfo.HasContingencyInfo, config => config.MapFrom(dto => !string.IsNullOrEmpty(dto.ContigencyInfo)))
                .ForPath(dto => dto.StatusFieldsInfo.ContingencyInfo, config => config.MapFrom(dto => dto.ContigencyInfo.CsvToEnum<ContingencyInfo>(true)))
                .ForPath(dto => dto.StatusFieldsInfo.OffMarketDate, config => config.MapFrom(dto => dto.OffMarketDate))
                .ForPath(dto => dto.StatusFieldsInfo.SellConcess, config => config.MapFrom(dto => dto.SellConcess))
                .ForPath(dto => dto.StatusFieldsInfo.SaleTerms, config => config.MapFrom(dto => dto.HowSold.CsvToEnum<SaleTerms>(true)))
                .ForPath(dto => dto.SaleProperty.SalePropertyInfo.OwnerName, config => config.MapFrom(dto => dto.BuilderName))
                .ForPath(dto => dto.SaleProperty.AddressInfo.StreetNumber, config => config.MapFrom(dto => dto.StreetNumber))
                .ForPath(dto => dto.SaleProperty.AddressInfo.StreetName, config => config.MapFrom(dto => dto.StreetName))
                .ForPath(dto => dto.SaleProperty.AddressInfo.City, config => config.MapFrom(dto => dto.City.ToEnumFromEnumMember<Cities>()))
                .ForPath(dto => dto.SaleProperty.AddressInfo.State, config => config.MapFrom(dto => dto.State))
                .ForPath(dto => dto.SaleProperty.AddressInfo.ZipCode, config => config.MapFrom(dto => dto.ZipCode))
                .ForPath(dto => dto.SaleProperty.AddressInfo.County, config => config.MapFrom(dto => dto.County))
                .ForPath(dto => dto.SaleProperty.AddressInfo.Subdivision, config => config.MapFrom(dto => dto.Subdivision))
                .ForPath(dto => dto.SaleProperty.PropertyInfo.ConstructionStage, lsm => lsm.Ignore())
                .ForPath(dto => dto.SaleProperty.PropertyInfo.ConstructionStartYear, config => config.MapFrom(x => x.ConstructionStartYear.ConstructionStartYearToIntConverter()))
                .ForPath(dto => dto.SaleProperty.PropertyInfo.LegalDescription, config => config.MapFrom(dto => dto.LegalDescription))
                .ForPath(dto => dto.SaleProperty.PropertyInfo.TaxId, config => config.MapFrom(dto => dto.TaxId))
                .ForPath(dto => dto.SaleProperty.PropertyInfo.MlsArea, config => config.MapFrom(dto => (MlsArea?)dto.MlsArea))
                .ForPath(dto => dto.SaleProperty.PropertyInfo.LotSize, config => config.MapFrom(dto => dto.LotSize))
                .ForPath(dto => dto.SaleProperty.PropertyInfo.LotDimension, config => config.MapFrom(dto => dto.LotDimensions))
                .ForPath(dto => dto.SaleProperty.PropertyInfo.LotDescription, config => config.MapFrom(dto => dto.LotDescription.CsvToEnum<LotDescription>(true)))
                .ForPath(dto => dto.SaleProperty.PropertyInfo.Latitude, config => config.MapFrom(dto => dto.Latitude))
                .ForPath(dto => dto.SaleProperty.PropertyInfo.Longitude, config => config.MapFrom(dto => dto.Longitude))
                .ForPath(dto => dto.SaleProperty.SpacesDimensionsInfo.StoriesTotal, config => config.MapFrom(dto => dto.Stories.ToStories()))
                .ForPath(dto => dto.SaleProperty.SpacesDimensionsInfo.SqFtTotal, config => config.MapFrom(dto => dto.SquareFeet))
                .ForPath(dto => dto.SaleProperty.SpacesDimensionsInfo.MainLevelBedroomTotal, config => config.MapFrom(dto => dto.Bedrooms))
                .ForPath(dto => dto.SaleProperty.SpacesDimensionsInfo.FullBathsTotal, config => config.MapFrom(dto => dto.FullBaths))
                .ForPath(dto => dto.SaleProperty.SpacesDimensionsInfo.HalfBathsTotal, config => config.MapFrom(dto => dto.HalfBaths))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.PropertyDescription, config => config.MapFrom(dto => dto.PropertyDescription))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.Fireplaces, config => config.MapFrom(dto => dto.Fireplaces.FireplacesToIntConverter()))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.FireplaceDescription, config => config.MapFrom(dto => dto.FireplaceDescription.CsvToEnum<FireplaceDescription>(true)))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.Floors, config => config.MapFrom(dto => dto.Flooring.CsvToEnum<Flooring>(true)))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.WindowFeatures, config => config.MapFrom(dto => dto.WindowCoverings.CsvToEnum<WindowFeatures>(true)))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.ExteriorFeatures, config => config.MapFrom(dto => dto.ExteriorFeatures.CsvToEnum<ExteriorFeatures>(true)))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.RoofDescription, config => config.MapFrom(dto => dto.Roof.CsvToEnum<RoofDescription>(true)))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.Foundation, config => config.MapFrom(dto => dto.Foundation.CsvToEnum<Foundation>(true)))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.HomeFaces, config => config.MapFrom(dto => dto.HomeFaces))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.CoolingSystem, config => config.MapFrom(dto => dto.CoolingSystemDescription.CsvToEnum<CoolingSystem>(true)))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.WaterSewer, config => config.MapFrom(dto => dto.WaterSewer.CsvToEnum<WaterSewer>(true)))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.NeighborhoodAmenities, config => config.MapFrom(dto => dto.NeighborhoodAmenities.CsvToEnum<NeighborhoodAmenities>(true)))
                .ForPath(dto => dto.SaleProperty.FeaturesInfo.IsNewConstruction, config => config.MapFrom(dto => dto.Construction == "NEW"))
                .ForPath(dto => dto.SaleProperty.FinancialInfo.TaxRate, config => config.MapFrom(dto => dto.TotalTax))
                .ForPath(dto => dto.SaleProperty.FinancialInfo.TaxYear, config => config.MapFrom(dto => dto.TaxYear))
                .ForPath(dto => dto.SaleProperty.FinancialInfo.TitleCompany, config => config.MapFrom(dto => dto.TitleCompany))
                .ForPath(dto => dto.SaleProperty.FinancialInfo.BuyersAgentCommission, config => config.MapFrom(x => GetBuyersAgentCommission(x.BuyerAgentComission)))
                .ForPath(dto => dto.SaleProperty.FinancialInfo.BuyersAgentCommissionType, config => config.MapFrom(x => GetBuyersAgentCommissionType(x.BuyerAgentComission)))
                .ForPath(dto => dto.SaleProperty.FinancialInfo.HOARequirement, config => config.MapFrom(dto => dto.HOAMandatory))
                .ForPath(dto => dto.SaleProperty.ShowingInfo.ContactPhone, config => config.MapFrom(dto => dto.ApptPhone))
                .ForPath(dto => dto.SaleProperty.ShowingInfo.AgentPrivateRemarks, config => config.MapFrom(dto => dto.AgentPrivateRemarks))
                .ForPath(dto => dto.SaleProperty.ShowingInfo.Directions, config => config.MapFrom(dto => dto.Directions))
                .ForPath(dto => dto.SaleProperty.ShowingInfo.ShowingInstructions, config => config.MapFrom(dto => dto.Showing))
                .ForPath(dto => dto.SaleProperty.SchoolsInfo.SchoolDistrict, config => config.MapFrom(dto => dto.SchoolDistrict))
                .ForPath(dto => dto.SaleProperty.SchoolsInfo.MiddleSchool, config => config.MapFrom(dto => dto.MiddleSchool))
                .ForPath(dto => dto.SaleProperty.SchoolsInfo.ElementarySchool, config => config.MapFrom(dto => dto.ElementarySchool))
                .ForPath(dto => dto.SaleProperty.SchoolsInfo.HighSchool, config => config.MapFrom(dto => dto.HighSchool))
                .ForMember(dto => dto.PropertyId, lsm => lsm.Ignore());
        }

        private void MapFullListingDtoToListingValueObject()
        {
            this.CreateMap<FullListingSaleDto, ListingValueObject>()
                .ForMember(vo => vo.CDOM, dto => dto.MapFrom(dto => dto.CDOM))
                .ForMember(vo => vo.DOM, dto => dto.MapFrom(dto => dto.DOM))
                .ForMember(vo => vo.ExpirationDate, dto => dto.MapFrom(dto => dto.ExpirationDate))
                .ForMember(vo => vo.ListDate, dto => dto.MapFrom(dto => dto.ListDate))
                .ForMember(vo => vo.ListPrice, dto => dto.MapFrom(dto => dto.ListPrice))
                .ForMember(vo => vo.ListType, dto => dto.MapFrom(dto => dto.ListType))
                .ForMember(vo => vo.MarketModifiedOn, dto => dto.MapFrom(dto => dto.MarketModifiedOn))
                .ForMember(vo => vo.MlsNumber, dto => dto.MapFrom(dto => dto.MlsNumber))
                .ForMember(vo => vo.MlsStatus, dto => dto.MapFrom(dto => dto.MlsStatus))
                .ForMember(vo => vo.PropertyId, dto => dto.MapFrom(dto => dto.PropertyId));
        }

        private void MapWithConverters()
        {
            this.CreateMap<RoomMessage, RoomDto>()
                .ConvertUsing<RoomConverter>();

            this.CreateMap<OpenHouseMessage, OpenHouseDto>()
                .ConvertUsing<OpenHouseConverter>();
        }
    }
}
