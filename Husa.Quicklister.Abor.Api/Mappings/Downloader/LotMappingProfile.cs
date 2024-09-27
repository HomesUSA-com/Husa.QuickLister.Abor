namespace Husa.Quicklister.Abor.Api.Mappings.Downloader
{
    using System.Linq;
    using AutoMapper;
    using Husa.Downloader.CTX.Api.Contracts.Response.Residential;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Api.Mappings.EnumTransformations;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotMappingProfile : Profile
    {
        public LotMappingProfile()
        {
            this.ResidentialMessageMapping();
        }

        private void ResidentialMessageMapping()
        {
            this.CreateMap<ResidentialResponse, LotListingDto>()
                .ForMember(vo => vo.OwnerName, dto => dto.MapFrom(src => src.ListingMessage.OwnerName))
                .ForMember(vo => vo.ListPrice, dto => dto.MapFrom(src => src.ListingMessage.ListPrice))
                .ForMember(vo => vo.ExpirationDate, dto => dto.MapFrom(src => src.ListingMessage.ExpirationDate))
                .ForMember(vo => vo.ListDate, dto => dto.MapFrom(src => src.ListingMessage.ListDate))
                .ForMember(vo => vo.ListType, dto => dto.MapFrom(src => src.ListingMessage.ListingType.FirstOrDefault().ToAborEnum()))
                .ForMember(vo => vo.MlsNumber, dto => dto.MapFrom(src => src.ListingMessage.MlsId))
                .ForMember(vo => vo.MlsStatus, dto => dto.MapFrom(src => src.ListingMessage.MlsStatus.ToAborEnum()))
                .ForMember(vo => vo.MarketModifiedOn, dto => dto.MapFrom(src => src.ListingMessage.ModificationTimestamp.ToUtcDateTime()))
                .ForMember(vo => vo.IsManuallyManaged, dto => dto.MapFrom(src => false))
                .ForMember(vo => vo.AddressInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.PropertyInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.FeaturesInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.FinancialInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.ShowingInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.SchoolsInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.StatusFieldsInfo, dto => dto.MapFrom(src => src))
                .ForMember(vo => vo.Id, dto => dto.Ignore());

            this.CreateMap<ResidentialResponse, ListingStatusFieldsDto>()
                .ForMember(vo => vo.SellConcess, dto => dto.MapFrom(src => src.ListingMessage.ConcessionsAmount))
                .ForMember(vo => vo.ClosePrice, dto => dto.MapFrom(src => src.ListingMessage.ClosePrice))
                .ForMember(vo => vo.EstimatedClosedDate, dto => dto.MapFrom(src => src.ListingMessage.EstimatedCloseDate))
                .ForMember(vo => vo.HasBuyerAgent, dto => dto.MapFrom(src => !string.IsNullOrEmpty(src.OtherMessage.AgentSell)))
                .ForMember(vo => vo.ClosedDate, dto => dto.MapFrom(src => src.ListingMessage.CloseDate))
                .ForMember(vo => vo.BackOnMarketDate, dto => dto.MapFrom(src => src.ListingMessage.OnMarketTimestamp.ToUtcDateTime()))
                .ForMember(vo => vo.OffMarketDate, dto => dto.MapFrom(src => src.ListingMessage.OffMarketTimestamp.ToUtcDateTime()))
                .ForMember(vo => vo.SaleTerms, dto => dto.MapFrom(src => src.ShowingMessage.SoldTerms.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.PendingDate, dto => dto.MapFrom(src => src.ListingMessage.PendingDate.ToUtcDateTime()))
                .ForMember(vo => vo.AgentId, dto => dto.Ignore())
                .ForMember(vo => vo.AgentIdSecond, dto => dto.Ignore())
                .ForMember(vo => vo.HasSecondBuyerAgent, dto => dto.Ignore())
                .ForMember(vo => vo.HasContingencyInfo, dto => dto.Ignore())
                .ForMember(vo => vo.ContingencyInfo, dto => dto.Ignore())
                .ForMember(vo => vo.CancelledReason, dto => dto.Ignore());

            this.SalePropertyDetailDtoMessageMapping();
        }

        private void SalePropertyDetailDtoMessageMapping()
        {
            this.CreateMap<ResidentialResponse, LotAddressDto>()
                .ForMember(vo => vo.StreetNumber, dto => dto.MapFrom(src => src.ListingMessage.StreetNumber))
                .ForMember(vo => vo.StreetName, dto => dto.MapFrom(src => src.ListingMessage.StreetName))
                .ForMember(vo => vo.StreetType, dto => dto.MapFrom(src => src.ListingMessage.StreetType.ToAborEnum()))
                .ForMember(vo => vo.UnitNumber, dto => dto.MapFrom(src => src.ListingMessage.UnitNumber))
                .ForMember(vo => vo.City, dto => dto.MapFrom(src => src.ListingMessage.City.GetEnumFromText<Cities>()))
                .ForMember(vo => vo.State, dto => dto.MapFrom(src => src.ListingMessage.State.ToAborEnum()))
                .ForMember(vo => vo.ZipCode, dto => dto.MapFrom(src => src.ListingMessage.ZipCode))
                .ForMember(vo => vo.County, dto => dto.MapFrom(src => src.ListingMessage.County.GetEnumFromText<Counties>()))
                .ForMember(vo => vo.Subdivision, dto => dto.MapFrom(src => src.ListingMessage.Subdivision))
                .ForMember(vo => vo.StreetDirPrefix, dto => dto.MapFrom(src => src.ListingMessage.StreetDirPrefix.ToAborEnum()))
                .ForMember(vo => vo.StreetDirSuffix, dto => dto.MapFrom(src => src.ListingMessage.StreetDirSuffix.ToAborEnum()));

            this.CreateMap<ResidentialResponse, LotPropertyDto>()
                .ForMember(vo => vo.LegalDescription, dto => dto.MapFrom(src => src.ListingMessage.LegalDescription))
                .ForMember(vo => vo.TaxId, dto => dto.MapFrom(src => src.ListingMessage.PropertyId))
                .ForMember(vo => vo.PropertyType, dto => dto.MapFrom(src => src.ListingMessage.PropertySubType.FirstOrDefault().ToAborEnum()))
                .ForMember(vo => vo.MlsArea, dto => dto.MapFrom(src => src.ListingMessage.MLSAreaMajor.GetEnumFromText<MlsArea>()))
                .ForMember(vo => vo.Latitude, dto => dto.MapFrom(src => src.ListingMessage.Latitude))
                .ForMember(vo => vo.Longitude, dto => dto.MapFrom(src => src.ListingMessage.Longitude))
                .ForMember(vo => vo.LotDescription, dto => dto.MapFrom(src => src.OtherMessage.LotDescription.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.LotSize, dto => dto.MapFrom(src => src.OtherMessage.LotSize.ToString()))
                .ForMember(vo => vo.LotDimension, dto => dto.MapFrom(src => src.OtherMessage.LotDimensions))
                .ForMember(vo => vo.TaxLot, dto => dto.MapFrom(src => src.FinancialMessage.TaxLot))
                .ForMember(vo => vo.UpdateGeocodes, dto => dto.Ignore())
                .ForMember(vo => vo.FemaFloodPlain, dto => dto.Ignore())
                .ForMember(vo => vo.PropCondition, dto => dto.Ignore())
                .ForMember(vo => vo.PropertySubType, dto => dto.Ignore())
                .ForMember(vo => vo.TypeOfHomeAllowed, dto => dto.Ignore())
                .ForMember(vo => vo.SoilType, dto => dto.Ignore())
                .ForMember(vo => vo.SurfaceWater, dto => dto.Ignore())
                .ForMember(vo => vo.NumberOfPonds, dto => dto.Ignore())
                .ForMember(vo => vo.NumberOfWells, dto => dto.Ignore())
                .ForMember(vo => vo.LiveStock, dto => dto.Ignore())
                .ForMember(vo => vo.CommercialAllowed, dto => dto.Ignore())
                .ForMember(vo => vo.AlsoListedAs, dto => dto.Ignore())
                .ForMember(vo => vo.BuilderRestrictions, dto => dto.Ignore())
                .ForMember(vo => vo.TaxBlock, dto => dto.Ignore());

            this.CreateMap<ResidentialResponse, LotFeaturesDto>()
                .ForMember(vo => vo.Fencing, dto => dto.MapFrom(src => src.OtherMessage.Fencing.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.WaterSource, dto => dto.MapFrom(src => src.OtherMessage.WaterAccessType.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.WaterBodyName, dto => dto.MapFrom(src => src.ListingMessage.City.GetEnumFromText<WaterBodyName>()))
                .ForMember(vo => vo.View, dto => dto.MapFrom(src => src.OtherMessage.ViewFeatures.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.ExteriorFeatures, dto => dto.MapFrom(src => src.OtherMessage.ExteriorFeatures.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.NeighborhoodAmenities, dto => dto.MapFrom(src => src.OtherMessage.NeighborhoodAmenities.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.WaterSewer, dto => dto.MapFrom(src => src.OtherMessage.WaterSewer.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.UtilitiesDescription, dto => dto.MapFrom(src => src.OtherMessage.OtherUtilities.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.WaterfrontFeatures, dto => dto.MapFrom(src => src.OtherMessage.WaterfrontFeatures.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.RestrictionsDescription, dto => dto.Ignore())
                .ForMember(vo => vo.DistanceToWaterAccess, dto => dto.Ignore())
                .ForMember(vo => vo.GroundWaterConservDistric, dto => dto.Ignore())
                .ForMember(vo => vo.HorseAmenities, dto => dto.Ignore())
                .ForMember(vo => vo.MineralsFeatures, dto => dto.Ignore())
                .ForMember(vo => vo.RoadSurface, dto => dto.Ignore())
                .ForMember(vo => vo.OtherStructures, dto => dto.Ignore())
                .ForMember(vo => vo.Disclosures, dto => dto.Ignore())
                .ForMember(vo => vo.DocumentsAvailable, dto => dto.Ignore());

            this.CreateMap<ResidentialResponse, LotFinancialDto>()
                .ForMember(vo => vo.AcceptableFinancing, dto => dto.MapFrom(src => src.FinancialMessage.AcceptableFinancing.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.TaxYear, dto => dto.MapFrom(src => src.FinancialMessage.TaxYear))
                .ForMember(vo => vo.HoaName, dto => dto.MapFrom(src => src.FinancialMessage.HoaName))
                .ForMember(vo => vo.HasHoa, dto => dto.MapFrom(src => src.FinancialMessage.HoaRequirement))
                .ForMember(vo => vo.HoaIncludes, dto => dto.MapFrom(src => src.FinancialMessage.HoaFeeIncludes.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.HoaFee, dto => dto.MapFrom(src => src.FinancialMessage.HoaAmount))
                .ForMember(vo => vo.BillingFrequency, dto => dto.MapFrom(src => src.FinancialMessage.HoaFeeFrequency.ToAborEnum()))
                .ForMember(vo => vo.TaxRate, dto => dto.Ignore())
                .ForMember(vo => vo.HOARequirement, dto => dto.Ignore())
                .ForMember(vo => vo.BuyersAgentCommission, dto => dto.Ignore())
                .ForMember(vo => vo.BuyersAgentCommissionType, dto => dto.Ignore())
                .ForMember(vo => vo.HasAgentBonus, dto => dto.Ignore())
                .ForMember(vo => vo.HasBonusWithAmount, dto => dto.Ignore())
                .ForMember(vo => vo.AgentBonusAmount, dto => dto.Ignore())
                .ForMember(vo => vo.AgentBonusAmountType, dto => dto.Ignore())
                .ForMember(vo => vo.BonusExpirationDate, dto => dto.Ignore())
                .ForMember(vo => vo.EstimatedTax, dto => dto.Ignore())
                .ForMember(vo => vo.TaxExemptions, dto => dto.Ignore())
                .ForMember(vo => vo.TaxAssesedValue, dto => dto.Ignore())
                .ForMember(vo => vo.PreferredTitleCompany, dto => dto.Ignore())
                .ForMember(vo => vo.LandTitleEvidence, dto => dto.Ignore());

            this.CreateMap<ResidentialResponse, LotShowingDto>()
                .ForMember(vo => vo.ShowingRequirements, dto => dto.MapFrom(src => src.ShowingMessage.Showing.Select(x => x.ToAborEnum()).Where(y => y != null)))
                .ForMember(vo => vo.Directions, dto => dto.MapFrom(src => src.ListingMessage.Directions))
                .ForMember(vo => vo.ShowingInstructions, dto => dto.MapFrom(src => src.ShowingMessage.ShowingInstructions))
                .ForMember(vo => vo.OwnerName, dto => dto.MapFrom(src => src.ListingMessage.OwnerName))
                .ForMember(vo => vo.ApptPhone, dto => dto.Ignore())
                .ForMember(vo => vo.ShowingServicePhone, dto => dto.Ignore())
                .ForMember(vo => vo.PublicRemarks, dto => dto.Ignore())
                .ForMember(vo => vo.ShowingContactType, dto => dto.Ignore())
                .ForMember(vo => vo.ShowingContactName, dto => dto.Ignore());

            this.CreateMap<ResidentialResponse, LotSchoolsDto>()
                .ForMember(vo => vo.SchoolDistrict, dto => dto.MapFrom(src => src.ListingMessage.HighSchoolDistrict.GetEnumFromText<SchoolDistrict>()))
                .ForMember(vo => vo.MiddleSchool, dto => dto.MapFrom(src => src.ListingMessage.MiddleSchool.GetEnumFromSchools<MiddleSchool>()))
                .ForMember(vo => vo.ElementarySchool, dto => dto.MapFrom(src => src.ListingMessage.ElementarySchool.ToElementarySchool()))
                .ForMember(vo => vo.HighSchool, dto => dto.MapFrom(src => src.ListingMessage.HighSchool.GetEnumFromSchools<HighSchool>()))
                .ForMember(vo => vo.OtherElementarySchool, dto => dto.Ignore())
                .ForMember(vo => vo.OtherMiddleSchool, dto => dto.Ignore())
                .ForMember(vo => vo.OtherHighSchool, dto => dto.Ignore());
        }
    }
}
