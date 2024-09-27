namespace Husa.Quicklister.Abor.Api.Mappings.Downloader
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Husa.Downloader.CTX.Api.Contracts.Response.Residential;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Api.Mappings.Downloader.Converters;
    using Husa.Quicklister.Abor.Api.Mappings.EnumTransformations;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.ValueObjects;

    public class ListingSaleMappingProfile : Profile
    {
        public ListingSaleMappingProfile()
        {
            this.MapResidentialDownloaderToListingDto();
            this.ResidentialMessageMapping();
            this.MapFullListingDtoToListingValueObject();
            this.MapWithConverters();

            this.CreateMap<ListingSalePublishInfoDto, PublishInfo>();
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

        private void ResidentialMessageMapping()
        {
            this.CreateMap<ResidentialResponse, FullListingSaleDto>()
                .ForMember(vo => vo.CDOM, dto => dto.MapFrom(src => src.OtherMessage.CDOM))
                .ForMember(vo => vo.DOM, dto => dto.MapFrom(src => src.ListingMessage.DOM))
                .ForMember(vo => vo.ListPrice, dto => dto.MapFrom(src => src.ListingMessage.ListPrice))
                .ForMember(vo => vo.ExpirationDate, dto => dto.MapFrom(src => src.ListingMessage.ExpirationDate))
                .ForMember(vo => vo.ListDate, dto => dto.MapFrom(src => src.ListingMessage.ListDate))
                .ForMember(vo => vo.ListType, dto => dto.MapFrom(src => src.ListingMessage.ListingType.FirstOrDefault().ToAborEnum()))
                .ForMember(vo => vo.MarketModifiedOn, dto => dto.MapFrom(src => src.ListingMessage.ModificationTimestamp.ToUtcDateTime()))
                .ForMember(vo => vo.MarketUniqueId, dto => dto.MapFrom(src => src.ListingMessage.EntityKey))
                .ForMember(vo => vo.MlsNumber, dto => dto.MapFrom(src => src.ListingMessage.MlsId))
                .ForMember(vo => vo.MlsStatus, dto => dto.MapFrom(src => src.ListingMessage.MlsStatus.ToAborEnum()))
                .ForMember(vo => vo.PropertyId, dto => dto.Ignore())
                .ForMember(vo => vo.SellingAgentId, dto => dto.MapFrom(src => src.OtherMessage.AgentSell))
                .ForMember(vo => vo.StatusFieldsInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.SaleProperty, dto => dto.MapFrom(src => src));

            this.CreateMap<ResidentialResponse, ListingSaleStatusFieldsDto>()
                .ForMember(vo => vo.SellConcess, dto => dto.MapFrom(src => src.ListingMessage.ConcessionsAmount))
                .ForMember(vo => vo.ClosePrice, dto => dto.MapFrom(src => src.ListingMessage.ClosePrice))
                .ForMember(vo => vo.EstimatedClosedDate, dto => dto.MapFrom(src => src.ListingMessage.EstimatedCloseDate))
                .ForMember(vo => vo.AgentId, dto => dto.Ignore())
                .ForMember(vo => vo.AgentIdSecond, dto => dto.Ignore())
                .ForMember(vo => vo.HasBuyerAgent, dto => dto.MapFrom(src => !string.IsNullOrEmpty(src.OtherMessage.AgentSell)))
                .ForMember(vo => vo.HasSecondBuyerAgent, dto => dto.Ignore())
                .ForMember(vo => vo.ClosedDate, dto => dto.MapFrom(src => src.ListingMessage.CloseDate))
                .ForMember(vo => vo.BackOnMarketDate, dto => dto.MapFrom(src => src.ListingMessage.OnMarketTimestamp.ToUtcDateTime()))
                .ForMember(vo => vo.OffMarketDate, dto => dto.MapFrom(src => src.ListingMessage.OffMarketTimestamp.ToUtcDateTime()))
                .ForMember(vo => vo.HasContingencyInfo, dto => dto.Ignore())
                .ForMember(vo => vo.ContingencyInfo, dto => dto.Ignore())
                .ForMember(vo => vo.SaleTerms, dto => dto.MapFrom(src => src.ShowingMessage.SoldTerms.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.PendingDate, dto => dto.MapFrom(src => src.ListingMessage.PendingDate.ToUtcDateTime()))
                .ForMember(vo => vo.CancelledReason, dto => dto.Ignore());

            this.SalePropertyDetailDtoMessageMapping();
        }

        private void SalePropertyDetailDtoMessageMapping()
        {
            this.CreateMap<ResidentialResponse, SalePropertyDetailDto>()
                .ForMember(vo => vo.SalePropertyInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.AddressInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.PropertyInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.SpacesDimensionsInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.FeaturesInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.FinancialInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.ShowingInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.SchoolsInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.Rooms, dto => dto.Ignore())
                .ForMember(vo => vo.OpenHouses, dto => dto.Ignore())
                .ForMember(vo => vo.Id, dto => dto.Ignore());

            this.CreateMap<ResidentialResponse, SalePropertyDto>()
                .ForMember(vo => vo.OwnerName, dto => dto.MapFrom(src => src.ListingMessage.OwnerName))
                .ForMember(vo => vo.CompanyId, dto => dto.Ignore())
                .ForMember(vo => vo.CommunityId, dto => dto.Ignore())
                .ForMember(vo => vo.PlanId, dto => dto.Ignore());

            this.CreateMap<ResidentialResponse, SaleAddressDto>()
                .ForMember(vo => vo.StreetNumber, dto => dto.MapFrom(src => src.ListingMessage.StreetNumber))
                .ForMember(vo => vo.StreetName, dto => dto.MapFrom(src => src.ListingMessage.StreetName))
                .ForMember(vo => vo.StreetType, dto => dto.MapFrom(src => src.ListingMessage.StreetType.ToAborEnum()))
                .ForMember(vo => vo.UnitNumber, dto => dto.MapFrom(src => src.ListingMessage.UnitNumber))
                .ForMember(vo => vo.City, dto => dto.MapFrom(src => src.ListingMessage.City.GetEnumFromText<Cities>()))
                .ForMember(vo => vo.State, dto => dto.MapFrom(src => src.ListingMessage.State.ToAborEnum()))
                .ForMember(vo => vo.ZipCode, dto => dto.MapFrom(src => src.ListingMessage.ZipCode))
                .ForMember(vo => vo.County, dto => dto.MapFrom(src => src.ListingMessage.County.GetEnumFromText<Counties>()))
                .ForMember(vo => vo.Subdivision, dto => dto.MapFrom(src => src.ListingMessage.Subdivision));

            this.CreateMap<ResidentialResponse, PropertyDto>()
                .ForMember(vo => vo.ConstructionStartYear, dto => dto.MapFrom(src => src.ListingMessage.YearBuilt))
                .ForMember(vo => vo.ConstructionStage, dto => dto.Ignore())
                .ForMember(vo => vo.ConstructionCompletionDate, dto => dto.Ignore())
                .ForMember(vo => vo.LegalDescription, dto => dto.MapFrom(src => src.ListingMessage.LegalDescription))
                .ForMember(vo => vo.TaxId, dto => dto.MapFrom(src => src.ListingMessage.PropertyId))
                .ForMember(vo => vo.PropertyType, dto => dto.MapFrom(src => src.ListingMessage.PropertySubType.FirstOrDefault().ToAborEnum()))
                .ForMember(vo => vo.MlsArea, dto => dto.MapFrom(src => src.ListingMessage.MLSAreaMajor.GetEnumFromText<MlsArea>()))
                .ForMember(vo => vo.UpdateGeocodes, dto => dto.Ignore())
                .ForMember(vo => vo.Latitude, dto => dto.MapFrom(src => src.ListingMessage.Latitude))
                .ForMember(vo => vo.Longitude, dto => dto.MapFrom(src => src.ListingMessage.Longitude))
                .ForMember(vo => vo.LotDescription, dto => dto.MapFrom(src => src.OtherMessage.LotDescription.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.LotSize, dto => dto.MapFrom(src => src.OtherMessage.LotSize.ToString()))
                .ForMember(vo => vo.LotDimension, dto => dto.MapFrom(src => src.OtherMessage.LotDimensions))
                .ForMember(vo => vo.TaxLot, dto => dto.MapFrom(src => src.FinancialMessage.TaxLot))
                .ForMember(vo => vo.IsXmlManaged, dto => dto.Ignore())
                .ForMember(vo => vo.FemaFloodPlain, dto => dto.Ignore());

            this.CreateMap<ResidentialResponse, SpacesDimensionsDto>()
                .ForMember(vo => vo.MainLevelBedroomTotal, dto => dto.MapFrom(src => src.RoomsMessage.MainLevelBedrooms))
                .ForMember(vo => vo.FullBathsTotal, dto => dto.MapFrom(src => src.RoomsMessage.BathroomsFull))
                .ForMember(vo => vo.HalfBathsTotal, dto => dto.MapFrom(src => src.RoomsMessage.BathroomsHalf))
                .ForMember(vo => vo.DiningAreasTotal, dto => dto.Ignore())
                .ForMember(vo => vo.LivingAreasTotal, dto => dto.Ignore())
                .ForMember(vo => vo.OtherLevelsBedroomTotal, dto => dto.MapFrom(src => src.RoomsMessage.NumBedrooms - src.RoomsMessage.MainLevelBedrooms))
                .ForMember(vo => vo.StoriesTotal, dto => dto.MapFrom(src => src.FeaturesMessage.Levels.ToStories()))
                .ForMember(vo => vo.SqFtTotal, dto => dto.MapFrom(src => src.ListingMessage.SquareFeetTotal));

            this.CreateMap<ResidentialResponse, FeaturesDto>()
                .ForMember(vo => vo.HomeFaces, dto => dto.MapFrom(src => src.ListingMessage.FrontFaces.ToAborEnum()))
                .ForMember(vo => vo.Foundation, dto => dto.MapFrom(src => src.FeaturesMessage.Foundation.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.RoofDescription, dto => dto.MapFrom(src => src.FeaturesMessage.RoofDescription.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.ConstructionMaterials, dto => dto.MapFrom(src => src.FeaturesMessage.Construction.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.FireplaceDescription, dto => dto.MapFrom(src => src.FeaturesMessage.FireplaceDescription.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.Fireplaces, dto => dto.MapFrom(src => src.RoomsMessage.Fireplaces))
                .ForMember(vo => vo.Floors, dto => dto.MapFrom(src => src.FeaturesMessage.Floors.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.LaundryLocation, dto => dto.MapFrom(src => src.FeaturesMessage.Laundry.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.GarageSpaces, dto => dto.MapFrom(src => src.FeaturesMessage.GarageSpaces))
                .ForMember(vo => vo.GarageDescription, dto => dto.MapFrom(src => src.FeaturesMessage.GarageDescription.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.PatioAndPorchFeatures, dto => dto.MapFrom(src => src.FeaturesMessage.PatioAndPorchFeatures.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.InteriorFeatures, dto => dto.MapFrom(src => src.FeaturesMessage.InteriorFeatures.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.Appliances, dto => dto.MapFrom(src => src.FeaturesMessage.AppliancesAndEquipment.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.Fencing, dto => dto.MapFrom(src => src.OtherMessage.Fencing.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.WaterSource, dto => dto.MapFrom(src => src.OtherMessage.WaterAccessType.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.WaterBodyName, dto => dto.MapFrom(src => src.ListingMessage.City.GetEnumFromText<WaterBodyName>()))
                .ForMember(vo => vo.View, dto => dto.MapFrom(src => src.OtherMessage.ViewFeatures.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.ExteriorFeatures, dto => dto.MapFrom(src => src.OtherMessage.ExteriorFeatures.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.NeighborhoodAmenities, dto => dto.MapFrom(src => src.OtherMessage.NeighborhoodAmenities.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.HeatSystem, dto => dto.MapFrom(src => src.OtherMessage.HeatingSystem.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.CoolingSystem, dto => dto.MapFrom(src => src.OtherMessage.CoolingSystem.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.WaterSewer, dto => dto.MapFrom(src => src.OtherMessage.WaterSewer.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.UtilitiesDescription, dto => dto.MapFrom(src => src.OtherMessage.OtherUtilities.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.IsNewConstruction, dto => dto.Ignore())
                .ForMember(vo => vo.PropertyDescription, dto => dto.MapFrom(src => src.ListingMessage.PublicRemarks))
                .ForMember(vo => vo.RestrictionsDescription, dto => dto.Ignore())
                .ForMember(vo => vo.SecurityFeatures, dto => dto.MapFrom(src => src.FeaturesMessage.SecurityFeatures.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.WindowFeatures, dto => dto.MapFrom(src => src.OtherMessage.WindowFeatures.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.DistanceToWaterAccess, dto => dto.Ignore())
                .ForMember(vo => vo.WaterfrontFeatures, dto => dto.MapFrom(src => src.OtherMessage.WaterfrontFeatures.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.UnitStyle, dto => dto.Ignore())
                .ForMember(vo => vo.GuestAccommodationsDescription, dto => dto.Ignore())
                .ForMember(vo => vo.GuestBedroomsTotal, dto => dto.Ignore())
                .ForMember(vo => vo.GuestFullBathsTotal, dto => dto.Ignore())
                .ForMember(vo => vo.GuestHalfBathsTotal, dto => dto.Ignore())
                .ForMember(vo => vo.Disclosures, dto => dto.Ignore())
                .ForMember(vo => vo.DocumentsAvailable, dto => dto.Ignore());

            this.CreateMap<ResidentialResponse, FinancialDto>()
                .ForMember(vo => vo.AcceptableFinancing, dto => dto.MapFrom(src => src.FinancialMessage.AcceptableFinancing.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.TaxExemptions, dto => dto.Ignore())
                .ForMember(vo => vo.TaxYear, dto => dto.MapFrom(src => src.FinancialMessage.TaxYear))
                .ForMember(vo => vo.TaxRate, dto => dto.Ignore())
                .ForMember(vo => vo.HOARequirement, dto => dto.Ignore())
                .ForMember(vo => vo.TitleCompany, dto => dto.Ignore())
                .ForMember(vo => vo.HoaName, dto => dto.MapFrom(src => src.FinancialMessage.HoaName))
                .ForMember(vo => vo.HasHoa, dto => dto.MapFrom(src => src.FinancialMessage.HoaRequirement))
                .ForMember(vo => vo.HoaIncludes, dto => dto.MapFrom(src => src.FinancialMessage.HoaFeeIncludes.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.HoaFee, dto => dto.MapFrom(src => src.FinancialMessage.HoaAmount))
                .ForMember(vo => vo.BuyersAgentCommission, dto => dto.Ignore())
                .ForMember(vo => vo.BuyersAgentCommissionType, dto => dto.Ignore())
                .ForMember(vo => vo.AgentBonusAmount, dto => dto.Ignore())
                .ForMember(vo => vo.AgentBonusAmountType, dto => dto.Ignore())
                .ForMember(vo => vo.HasBonusWithAmount, dto => dto.Ignore())
                .ForMember(vo => vo.BonusExpirationDate, dto => dto.Ignore())
                .ForMember(vo => vo.HasAgentBonus, dto => dto.Ignore())
                .ForMember(vo => vo.BillingFrequency, dto => dto.MapFrom(src => src.FinancialMessage.HoaFeeFrequency.ToAborEnum()));

            this.CreateMap<ResidentialResponse, ShowingDto>()
                .ForMember(vo => vo.LockBoxType, dto => dto.MapFrom(src => src.ShowingMessage.LockBoxType.ToAborEnum()))
                .ForMember(vo => vo.ContactPhone, dto => dto.MapFrom(src => src.ShowingMessage.ShowingPhone))
                .ForMember(vo => vo.OccupantPhone, dto => dto.Ignore())
                .ForMember(vo => vo.LockBoxSerialNumber, dto => dto.MapFrom(src => src.ShowingMessage.AccessCode))
                .ForMember(vo => vo.ShowingRequirements, dto => dto.MapFrom(src => src.ShowingMessage.Showing.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.Directions, dto => dto.MapFrom(src => src.ListingMessage.Directions))
                .ForMember(vo => vo.ShowingInstructions, dto => dto.MapFrom(src => src.ShowingMessage.ShowingInstructions))
                .ForMember(vo => vo.AgentPrivateRemarks, dto => dto.MapFrom(src => src.ListingMessage.PrivateRemarks))
                .ForMember(vo => vo.OwnerName, dto => dto.MapFrom(src => src.ListingMessage.OwnerName))
                .ForMember(vo => vo.EnableOpenHouses, dto => dto.Ignore())
                .ForMember(vo => vo.ShowOpenHousesPending, dto => dto.Ignore())
                .ForMember(vo => vo.AgentPrivateRemarksAdditional, dto => dto.Ignore())
                .ForMember(vo => vo.ShowOpenHousesPending, dto => dto.Ignore())
                .ForMember(vo => vo.RealtorContactEmail, dto => dto.Ignore());

            this.CreateMap<ResidentialResponse, SchoolsDto>()
                .ForMember(vo => vo.SchoolDistrict, dto => dto.MapFrom(src => src.ListingMessage.HighSchoolDistrict.GetEnumFromText<SchoolDistrict>()))
                .ForMember(vo => vo.MiddleSchool, dto => dto.MapFrom(src => src.ListingMessage.MiddleSchool.GetEnumFromSchools<MiddleSchool>()))
                .ForMember(vo => vo.ElementarySchool, dto => dto.MapFrom(src => src.ListingMessage.ElementarySchool.ToElementarySchool()))
                .ForMember(vo => vo.HighSchool, dto => dto.MapFrom(src => src.ListingMessage.HighSchool.GetEnumFromSchools<HighSchool>()))
                .ForMember(vo => vo.OtherElementarySchool, dto => dto.Ignore())
                .ForMember(vo => vo.OtherMiddleSchool, dto => dto.Ignore())
                .ForMember(vo => vo.OtherHighSchool, dto => dto.Ignore());
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
        }
    }
}
