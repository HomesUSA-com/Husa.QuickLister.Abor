namespace Husa.Quicklister.Abor.Api.Mappings.Migration
{
    using System.Linq;
    using AutoMapper;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Migration.Api.Contracts.Response;
    using Husa.Migration.Api.Contracts.Response.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Extensions;

    public class ListingRequestMappingProfile : Profile
    {
        public ListingRequestMappingProfile()
        {
            this.CreateMap<AddressResponse, AddressRecord>()
                .ForMember(dto => dto.FormalAddress, pr => pr.Ignore())
                .ForMember(dto => dto.City, pr => pr.MapFrom(x => x.City.ToCity()))
                .ForMember(dto => dto.ReadableCity, pr => pr.MapFrom(x => x.City.ToEnumFromEnumMember<Cities>().GetEnumDescription()))
                .ForMember(dto => dto.County, pr => pr.MapFrom(x => x.County.ToEnumFromEnumMember<Counties>()))
                .ForMember(dto => dto.UnitNumber, pr => pr.MapFrom(x => x.UnitNum))
                .ForMember(dto => dto.StreetType, pr => pr.MapFrom(x => x.StreetType.ToEnumFromEnumMember<StreetType>()))
                .ForMember(dto => dto.State, pr => pr.MapFrom(x => x.State.ToEnumFromEnumMember<States>()));
            this.CreateMap<PropertyResponse, PropertyRecord>()
                .ForMember(dto => dto.ConstructionStage, pr => pr.MapFrom(x => x.ConstructionStage.ToEnumFromEnumMember<ConstructionStage>()))
                .ForMember(dto => dto.LotDescription, pr => pr.MapFrom(x => x.LotDescription.ToLotDescription()))
                .ForMember(dto => dto.PropertyType, pr => pr.MapFrom(x => x.PropertyType.ToEnumFromEnumMember<PropertySubType>()))
                .ForMember(dto => dto.MlsArea, pr => pr.MapFrom(x => x.MlsArea.ToEnumFromEnumMember<MlsArea>()))
                .ForMember(dto => dto.FemaFloodPlain, pr => pr.MapFrom(x => x.FemaFloodPlain.ToFemaFloodPlain()));
            this.CreateMap<FeaturesResponse, FeaturesRecord>()
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
                .ForMember(dto => dto.FireplaceDescription, pr => pr.MapFrom(x => x.FireplaceDescription.ToFireplaceDescription()))
                .ForMember(dto => dto.HomeFaces, pr => pr.MapFrom(x => x.HomeFaces.ToEnumFromEnumMember<HomeFaces>()))
                .ForMember(dto => dto.NeighborhoodAmenities, pr => pr.MapFrom(x => x.NeighborhoodAmenities.CsvToEnum<NeighborhoodAmenities>(true)))
                .ForMember(dto => dto.ExteriorFeatures, pr => pr.MapFrom(x => x.ExteriorFeatures.CsvToEnum<ExteriorFeatures>(true)))
                .ForMember(dto => dto.RoofDescription, pr => pr.MapFrom(x => x.RoofDescription.CsvToEnum<RoofDescription>(true)))
                .ForMember(dto => dto.Foundation, pr => pr.MapFrom(x => x.Foundation.CsvToEnum<Foundation>(true)))
                .ForMember(dto => dto.HeatSystem, pr => pr.MapFrom(x => x.HeatSystem.CsvToEnum<HeatingSystem>(true)))
                .ForMember(dto => dto.CoolingSystem, pr => pr.MapFrom(x => x.CoolingSystem.CsvToEnum<CoolingSystem>(true)))
                .ForMember(dto => dto.View, pr => pr.MapFrom(x => x.ViewDescription.ToView()))
                .ForMember(dto => dto.WaterfrontFeatures, pr => pr.MapFrom(x => x.WaterfrontFeatures.CsvToEnum<WaterfrontFeatures>(true)))
                .ForMember(dto => dto.UnitStyle, pr => pr.MapFrom(x => x.HousingStyle.CsvToEnum<UnitStyle>(true)))
                .ForMember(dto => dto.GuestAccommodationsDescription, pr => pr.MapFrom(x => x.GuestAccommodationsDescription.CsvToEnum<GuestAccommodationsDescription>(true)))
                .ForMember(dto => dto.WaterBodyName, pr => pr.MapFrom(x => x.WaterBodyName.ToEnumFromEnumMember<WaterBodyName>()))
                .ForMember(dto => dto.DistanceToWaterAccess, pr => pr.MapFrom(x => x.DistanceToWaterAccess.ToEnumFromEnumMember<DistanceToWaterAccess>()));
            this.CreateMap<SpacesDimensionsResponse, SpacesDimensionsRecord>()
                .ForMember(dto => dto.MainLevelBedroomTotal, pr => pr.MapFrom(x => x.NumBedrooms))
                .ForMember(dto => dto.HalfBathsTotal, pr => pr.MapFrom(x => x.BathsHalf))
                .ForMember(dto => dto.FullBathsTotal, pr => pr.MapFrom(x => x.BathsFull))
                .ForMember(dto => dto.StoriesTotal, pr => pr.MapFrom(x => x.Stories.ToEnumFromEnumMember<Stories>()));
            this.CreateMap<FinancialResponse, FinancialRecord>()
                .ForMember(dto => dto.AcceptableFinancing, pr => pr.MapFrom(x => x.AcceptableFinancing.ToAcceptableFinancing()))
                .ForMember(dto => dto.TaxExemptions, pr => pr.MapFrom(x => x.TaxExemptions.ToTaxExemptions()))
                .ForMember(dto => dto.HoaIncludes, pr => pr.MapFrom(x => x.HoaIncludes.ToHoaIncludes()))
                .ForMember(dto => dto.BillingFrequency, pr => pr.MapFrom(x => x.BillingFrequency.ToEnumFromEnumMember<BillingFrequency>()))
                .ForMember(dto => dto.HOARequirement, pr => pr.MapFrom(x => x.HOARequirement.ToEnumFromEnumMember<HoaRequirement>()))
                .ForMember(dto => dto.ReadableAgentBonusAmount, pr => pr.MapFrom(x => x.AgentBonusAmount.GetCommissionAmount(x.AgentBonusAmountType.ToEnumFromEnumMember<CommissionType>())))
                .ForMember(dto => dto.AgentBonusAmountType, pr => pr.MapFrom(x => x.AgentBonusAmountType.ToEnumFromEnumMember<CommissionType>()))
                .ForMember(dto => dto.ReadableBuyersAgentCommission, pr => pr.MapFrom(x => x.BuyersAgentCommission.GetCommissionAmount(x.BuyersAgentCommissionType.ToEnumFromEnumMember<CommissionType>())))
                .ForMember(dto => dto.BuyersAgentCommissionType, pr => pr.MapFrom(x => x.BuyersAgentCommissionType.ToEnumFromEnumMember<CommissionType>()));
            this.CreateMap<ShowingResponse, ShowingRecord>()
                .ForMember(dto => dto.OwnerName, pr => pr.Ignore())
                .ForMember(dto => dto.OccupantPhone, pr => pr.MapFrom(x => x.AltPhoneCommunity.CleanPhoneValue()))
                .ForMember(dto => dto.ContactPhone, pr => pr.MapFrom(x => x.AgentListApptPhone.CleanPhoneValue()))
                .ForMember(dto => dto.RealtorContactEmail, pr => pr.MapFrom(x => x.RealtorContactEmail.ToCollectionFromString(";")))
                .ForMember(dto => dto.ShowingInstructions, pr => pr.MapFrom(x => x.Showing))
                .ForMember(dto => dto.ShowingRequirements, pr => pr.MapFrom(x => x.ShowingRequirements.CsvToEnum<ShowingRequirements>(true)))
                .ForMember(dto => dto.LockBoxType, pr => pr.MapFrom(x => x.LockBoxType.ToEnumFromEnumMember<LockBoxType>()));
            this.CreateMap<SchoolsResponse, SchoolRecord>()
                .ForMember(dto => dto.ElementarySchool, pr => pr.MapFrom(x => x.ElementarySchool.ToEnumFromEnumMember<ElementarySchool>()))
                .ForMember(dto => dto.MiddleSchool, pr => pr.MapFrom(x => x.MiddleSchool.ToEnumFromEnumMember<MiddleSchool>()))
                .ForMember(dto => dto.HighSchool, pr => pr.MapFrom(x => x.HighSchool.ToEnumFromEnumMember<HighSchool>()))
                .ForMember(dto => dto.SchoolDistrict, pr => pr.MapFrom(x => x.SchoolDistrict.ToEnumFromEnumMember<SchoolDistrict>()));
            this.CreateMap<SalesOfficeResponse, SalesOfficeRecord>()
                .ForMember(dto => dto.SalesOfficeCity, pr => pr.MapFrom(x => x.SalesOfficeCity.ToCity()));
            this.CreateMap<StatusFieldsResponse, SaleStatusFieldsRecord>()
                .ForMember(dto => dto.ContingencyInfo, pr => pr.MapFrom(x => x.ContingencyInfo.CsvToEnum<ContingencyInfo>(true)))
                .ForMember(dto => dto.SaleTerms, pr => pr.MapFrom(x => x.SellerConcessionDescription.CsvToEnum<SaleTerms>(true)))
                .ForMember(dto => dto.AgentId, pr => pr.Ignore());
            this.CreateMap<PublishResponse, PublishFieldsRecord>()
                .ForMember(dto => dto.PublishUser, pr => pr.Ignore())
                .ForMember(dto => dto.PublishStatus, pr => pr.MapFrom(x => x.PublishStatus.ToEnumFromEnumMember<MarketStatuses>()))
                .ForMember(dto => dto.PublishType, pr => pr.MapFrom(x => x.PublishType.ToActionType()));

            this.CreateMap<SalePropertyResponse, SalePropertyRecord>()
                .ForMember(dto => dto.IsDeleted, pr => pr.MapFrom(x => 0))
                .ForMember(dto => dto.SysCreatedOn, pr => pr.Ignore())
                .ForMember(dto => dto.SysModifiedOn, pr => pr.Ignore())
                .ForMember(dto => dto.SysModifiedBy, pr => pr.Ignore())
                .ForMember(dto => dto.SysCreatedBy, pr => pr.Ignore())
                .ForMember(dto => dto.SysTimestamp, pr => pr.Ignore())
                .ForMember(dto => dto.CompanyId, pr => pr.Ignore())
                .ForMember(dto => dto.Address, pr => pr.Ignore())
                .ForMember(dto => dto.Id, pr => pr.Ignore())
                .ForMember(dto => dto.PlanId, pr => pr.Ignore())
                .ForMember(dto => dto.PlanName, pr => pr.Ignore())
                .ForMember(dto => dto.CommunityId, pr => pr.Ignore())
                .ForMember(dto => dto.PlanName, pr => pr.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.FeaturesInfo.GarageDescription = src.SpacesDimensionsInfo.GarageDescription.CsvToEnum<GarageDescription>(true).ToList();
                    dest.ShowingInfo.OwnerName = src.OwnerName;
                });

            this.CreateMap<SaleListingRequestResponse, SaleListingRequest>()
                .ForMember(dest => dest.EntityId, pr => pr.Ignore())
                .ForMember(dto => dto.Id, pr => pr.Ignore())
                .ForMember(dto => dto.IsDeleted, pr => pr.MapFrom(x => 0))
                .ForMember(dto => dto.CompanyId, pr => pr.Ignore())
                .ForMember(dto => dto.SysModifiedBy, pr => pr.Ignore())
                .ForMember(dto => dto.SysCreatedBy, pr => pr.Ignore())
                .ForMember(dto => dto.ListingSaleId, pr => pr.Ignore())
                .ForMember(dto => dto.MlsStatus, pr => pr.MapFrom(x => x.MlsStatus.ToMarketStatuses()))
                .ForMember(dto => dto.LegacyId, pr => pr.MapFrom(x => x.LegacyListingRequestId))
                .ForMember(dto => dto.RequestState, pr => pr.MapFrom(x => ListingRequestState.Completed))
                .ForMember(dto => dto.SaleProperty, pr => pr.MapFrom(x => x.SaleProperty));

            this.CreateMap<OpenHouseResponse, OpenHouseRecord>()
                .ForMember(dto => dto.Refreshments, pr => pr.MapFrom(x => x.Refreshments.CsvToEnum<Refreshments>(true)))
                .ForMember(dto => dto.Id, pr => pr.Ignore())
                .ForMember(dto => dto.IsDeleted, pr => pr.MapFrom(x => 0))
                .ForMember(dto => dto.Type, oh => oh.MapFrom(x => x.Type.ToOpenHouseType()));
            this.CreateMap<RoomResponse, RoomRecord>()
                .ForMember(dto => dto.Features, pr => pr.MapFrom(x => x.Features.CsvToEnum<RoomFeatures>(true)))
                .ForMember(dto => dto.RoomType, pr => pr.MapFrom(x => x.RoomType.ToRoomType()))
                .ForMember(dto => dto.Id, pr => pr.Ignore())
                .ForMember(dto => dto.Level, pr => pr.MapFrom(x => x.Level.ToEnumFromEnumMember<RoomLevel>()))
                .ForMember(dto => dto.SalePropertyId, pr => pr.Ignore())
                .ForMember(dto => dto.IsDeleted, pr => pr.MapFrom(x => 0))
                .ForMember(dto => dto.FieldType, pr => pr.Ignore())
                .ForMember(dto => dto.SysCreatedOn, pr => pr.Ignore())
                .ForMember(dto => dto.SysModifiedOn, pr => pr.Ignore())
                .ForMember(dto => dto.SysModifiedBy, pr => pr.Ignore())
                .ForMember(dto => dto.SysCreatedBy, pr => pr.Ignore())
                .ForMember(dto => dto.SysTimestamp, pr => pr.Ignore())
                .ForMember(dto => dto.CompanyId, pr => pr.Ignore());
        }
    }
}
