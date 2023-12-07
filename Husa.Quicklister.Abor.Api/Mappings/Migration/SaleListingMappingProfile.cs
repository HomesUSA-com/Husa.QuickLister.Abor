namespace Husa.Quicklister.Abor.Api.Mappings.Migration
{
    using System.Linq;
    using AutoMapper;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Migration.Api.Contracts.Response;
    using Husa.Migration.Api.Contracts.Response.SaleListing;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Data.Configuration;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class SaleListingMappingProfile : Profile
    {
        public SaleListingMappingProfile()
        {
            this.CreateMap<AddressResponse, AddressDto>()
                .ForMember(dto => dto.City, pr => pr.MapFrom(x => x.City.ToEnumFromEnumMember<Cities>()))
                .ForMember(dto => dto.County, pr => pr.MapFrom(x => x.County.ToEnumFromEnumMember<Counties>()))
                .ForMember(dto => dto.UnitNumber, pr => pr.MapFrom(x => x.UnitNum))
                .ForMember(dto => dto.State, pr => pr.MapFrom(x => x.State.ToEnumFromEnumMember<States>()));
            this.CreateMap<PropertyResponse, PropertyDto>()
                .ForMember(dto => dto.ConstructionStage, pr => pr.MapFrom(x => x.ConstructionStage.ToEnumFromEnumMember<ConstructionStage>()))
                .ForMember(dto => dto.LotDescription, pr => pr.MapFrom(x => x.LotDescription.ToLotDescription()))
                .ForMember(dto => dto.LegalDescription, pr => pr.MapFrom(x => x.LegalDescription.GetSubstring(SalePropertyConfiguration.LegalDescriptionLength)))
                .ForMember(dto => dto.TaxId, pr => pr.MapFrom(x => x.TaxId.GetSubstring(SalePropertyConfiguration.TaxIdLength)))
                .ForMember(dto => dto.PropertyType, pr => pr.MapFrom(x => x.PropertyType.ToEnumFromEnumMember<PropertySubType>()))
                .ForMember(dto => dto.MlsArea, pr => pr.MapFrom(x => x.MlsArea.ToEnumFromEnumMember<MlsArea>()));
            this.CreateMap<FeaturesResponse, FeaturesDto>()
                .ForMember(dto => dto.GarageDescription, pr => pr.Ignore())
                .ForMember(dto => dto.GarageSpaces, pr => pr.MapFrom(x => x.GarageCapacity))
                .ForMember(dto => dto.PatioAndPorchFeatures, pr => pr.MapFrom(x => x.PatioAndPorchFeatures.CsvToEnum<PatioAndPorchFeatures>(true)))
                .ForMember(dto => dto.ConstructionMaterials, pr => pr.MapFrom(x => x.Exterior.CsvToEnum<ConstructionMaterials>(true)))
                .ForMember(dto => dto.Fencing, pr => pr.MapFrom(x => x.FenceDescription.CsvToEnum<Fencing>(true)))
                .ForMember(dto => dto.Foundation, pr => pr.MapFrom(x => x.Foundation.CsvToEnum<Foundation>(true)))
                .ForMember(dto => dto.WindowFeatures, pr => pr.MapFrom(x => x.WindowCoverings.CsvToEnum<WindowFeatures>(true)))
                .ForMember(dto => dto.SecurityFeatures, pr => pr.MapFrom(x => x.SecurityFeatures.CsvToEnum<SecurityFeatures>(true)))
                .ForMember(dto => dto.InteriorFeatures, pr => pr.MapFrom(x => x.InteriorFeatures.CsvToEnum<InteriorFeatures>(true)))
                .ForMember(dto => dto.LaundryLocation, pr => pr.MapFrom(x => x.LaundryLocation.CsvToEnum<LaundryLocation>(true)))
                .ForMember(dto => dto.Appliances, pr => pr.MapFrom(x => x.Appliances.CsvToEnum<Appliances>(true)))
                .ForMember(dto => dto.WaterSource, pr => pr.MapFrom(x => x.WaterSource.CsvToEnum<WaterSource>(true)))
                .ForMember(dto => dto.WaterSewer, pr => pr.MapFrom(x => x.WaterSewer.CsvToEnum<WaterSewer>(true)))
                .ForMember(dto => dto.HeatSystem, pr => pr.MapFrom(x => x.HeatSystem.CsvToEnum<HeatingSystem>(true)))
                .ForMember(dto => dto.CoolingSystem, pr => pr.MapFrom(x => x.CoolingSystem.CsvToEnum<CoolingSystem>(true)))
                .ForMember(dto => dto.UtilitiesDescription, pr => pr.MapFrom(x => x.LotImprovements.CsvToEnum<UtilitiesDescription>(true)))
                .ForMember(dto => dto.RestrictionsDescription, pr => pr.MapFrom(x => x.RestrictionsDescription.CsvToEnum<RestrictionsDescription>(true)))
                .ForMember(dto => dto.Floors, pr => pr.MapFrom(x => x.Floors.CsvToEnum<Flooring>(true)))
                .ForMember(dto => dto.FireplaceDescription, pr => pr.MapFrom(x => x.FireplaceDescription.CsvToEnum<FireplaceDescription>(true)))
                .ForMember(dto => dto.HomeFaces, pr => pr.MapFrom(x => x.HomeFaces.ToEnumFromEnumMember<HomeFaces>()))
                .ForMember(dto => dto.NeighborhoodAmenities, pr => pr.MapFrom(x => x.NeighborhoodAmenities.CsvToEnum<NeighborhoodAmenities>(true)))
                .ForMember(dto => dto.ExteriorFeatures, pr => pr.MapFrom(x => x.ExteriorFeatures.CsvToEnum<ExteriorFeatures>(true)))
                .ForMember(dto => dto.RoofDescription, pr => pr.MapFrom(x => x.RoofDescription.CsvToEnum<RoofDescription>(true)))
                .ForMember(dto => dto.Foundation, pr => pr.MapFrom(x => x.Foundation.CsvToEnum<Foundation>(true)))
                .ForMember(dto => dto.HeatSystem, pr => pr.MapFrom(x => x.HeatSystem.CsvToEnum<HeatingSystem>(true)))
                .ForMember(dto => dto.CoolingSystem, pr => pr.MapFrom(x => x.CoolingSystem.CsvToEnum<CoolingSystem>(true)))
                .ForMember(dto => dto.View, pr => pr.MapFrom(x => x.ViewDescription.CsvToEnum<View>(true)))
                .ForMember(dto => dto.WaterfrontFeatures, pr => pr.MapFrom(x => x.WaterfrontFeatures.CsvToEnum<WaterfrontFeatures>(true)))
                .ForMember(dto => dto.UnitStyle, pr => pr.MapFrom(x => x.HousingStyle.CsvToEnum<UnitStyle>(true)))
                .ForMember(dto => dto.GuestAccommodationsDescription, pr => pr.MapFrom(x => x.GuestAccommodationsDescription.CsvToEnum<GuestAccommodationsDescription>(true)))
                .ForMember(dto => dto.WaterBodyName, pr => pr.MapFrom(x => x.WaterBodyName.ToEnumFromEnumMember<WaterBodyName>()))
                .ForMember(dto => dto.DistanceToWaterAccess, pr => pr.MapFrom(x => x.DistanceToWaterAccess.ToEnumFromEnumMember<DistanceToWaterAccess>()));
            this.CreateMap<SpacesDimensionsResponse, SpacesDimensionsDto>()
                .ForMember(dto => dto.MainLevelBedroomTotal, pr => pr.MapFrom(x => x.NumBedrooms))
                .ForMember(dto => dto.HalfBathsTotal, pr => pr.MapFrom(x => x.BathsHalf))
                .ForMember(dto => dto.FullBathsTotal, pr => pr.MapFrom(x => x.BathsFull))
                .ForMember(dto => dto.StoriesTotal, pr => pr.MapFrom(x => x.Stories.ToEnumFromEnumMember<Stories>()));
            this.CreateMap<FinancialResponse, FinancialDto>()
                .ForMember(dto => dto.AcceptableFinancing, pr => pr.MapFrom(x => x.AcceptableFinancing.ToAcceptableFinancing()))
                .ForMember(dto => dto.TaxExemptions, pr => pr.MapFrom(x => x.TaxExemptions.ToTaxExemptions()))
                .ForMember(dto => dto.HoaIncludes, pr => pr.MapFrom(x => x.HoaIncludes.CsvToEnum<HoaIncludes>(true)))
                .ForMember(dto => dto.BillingFrequency, pr => pr.MapFrom(x => x.BillingFrequency.ToEnumFromEnumMember<BillingFrequency>()))
                .ForMember(dto => dto.HOARequirement, pr => pr.MapFrom(x => x.HOARequirement.ToEnumFromEnumMember<HoaRequirement>()))
                .ForMember(dto => dto.AgentBonusAmountType, pr => pr.MapFrom(x => x.AgentBonusAmountType.ToEnumFromEnumMember<CommissionType>()))
                .ForMember(dto => dto.BuyersAgentCommissionType, pr => pr.MapFrom(x => x.BuyersAgentCommissionType.ToEnumFromEnumMember<CommissionType>()));
            this.CreateMap<ShowingResponse, ShowingDto>()
                .ForMember(dto => dto.OwnerName, pr => pr.Ignore())
                .ForMember(dto => dto.OccupantPhone, pr => pr.MapFrom(x => x.AltPhoneCommunity.CleanPhoneValue()))
                .ForMember(dto => dto.ContactPhone, pr => pr.MapFrom(x => x.AgentListApptPhone.CleanPhoneValue()))
                .ForMember(dto => dto.RealtorContactEmail, pr => pr.MapFrom(x => x.RealtorContactEmail.ToCollectionFromString(";")))
                .ForMember(dto => dto.ShowingInstructions, pr => pr.MapFrom(x => x.Showing))
                .ForMember(dto => dto.ShowingRequirements, pr => pr.MapFrom(x => x.ShowingRequirements.CsvToEnum<ShowingRequirements>(true)))
                .ForMember(dto => dto.LockBoxType, pr => pr.MapFrom(x => x.LockBoxType.ToEnumFromEnumMember<LockBoxType>()));
            this.CreateMap<SchoolsResponse, SchoolsDto>()
                .ForMember(dto => dto.ElementarySchool, pr => pr.MapFrom(x => x.ElementarySchool.ToEnumFromEnumMember<ElementarySchool>()))
                .ForMember(dto => dto.MiddleSchool, pr => pr.MapFrom(x => x.MiddleSchool.ToEnumFromEnumMember<MiddleSchool>()))
                .ForMember(dto => dto.HighSchool, pr => pr.MapFrom(x => x.HighSchool.ToEnumFromEnumMember<HighSchool>()))
                .ForMember(dto => dto.SchoolDistrict, pr => pr.MapFrom(x => x.SchoolDistrict.ToEnumFromEnumMember<SchoolDistrict>()));
            this.CreateMap<StatusFieldsResponse, ListingSaleStatusFieldsDto>()
                .ForMember(dto => dto.ContingencyInfo, pr => pr.MapFrom(x => x.ContingencyInfo.CsvToEnum<ContingencyInfo>(true)))
                .ForMember(dto => dto.SaleTerms, pr => pr.MapFrom(x => x.SellerConcessionDescription.CsvToEnum<SaleTerms>(true)))
                .ForMember(dto => dto.AgentId, pr => pr.Ignore());

            this.CreateMap<SalePropertyResponse, SalePropertyDetailDto>()
                .ForMember(dto => dto.SalePropertyInfo, pr => pr.MapFrom(x => new SalePropertyDto()))
                .ForMember(dto => dto.Id, pr => pr.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.FeaturesInfo.GarageDescription = src.SpacesDimensionsInfo.GarageDescription.CsvToEnum<GarageDescription>(true).ToList();
                    dest.ShowingInfo.OwnerName = src.OwnerName;
                });

            this.CreateMap<SaleListingResponse, SaleListingDto>()
                .ForMember(dto => dto.MlsStatus, pr => pr.MapFrom(x => x.MlsStatus.ToEnumFromEnumMember<MarketStatuses>()))
                .ForMember(dto => dto.SaleProperty, pr => pr.MapFrom(x => x.SaleProperty))
                .ForMember(dto => dto.Id, pr => pr.Ignore())
                .ForMember(dto => dto.ListType, pr => pr.Ignore())
                .ForMember(dto => dto.MarketModifiedOn, pr => pr.Ignore())
                .ForMember(dto => dto.IsManuallyManaged, pr => pr.Ignore());

            this.CreateMap<OpenHouseResponse, OpenHouseDto>()
                .ForMember(dto => dto.Refreshments, pr => pr.MapFrom(x => x.Refreshments.CsvToEnum<Refreshments>(true)))
                .ForMember(dto => dto.Type, oh => oh.MapFrom(x => x.Type.ToOpenHouseType()));
            this.CreateMap<RoomResponse, RoomDto>()
                .ForMember(dto => dto.Features, pr => pr.MapFrom(x => x.Features.CsvToEnum<RoomFeatures>(true)))
                .ForMember(dto => dto.RoomType, pr => pr.MapFrom(x => x.RoomType.ToRoomType()))
                .ForMember(dto => dto.Level, pr => pr.MapFrom(x => x.Level.ToEnumFromEnumMember<RoomLevel>()))
                .ForMember(dto => dto.Id, pr => pr.Ignore());
        }
    }
}
