namespace Husa.Quicklister.Abor.Api.Mappings.Migration
{
    using AutoMapper;
    using Husa.Extensions.Common;
    using Husa.Migration.Api.Contracts.Response;
    using Husa.Migration.Api.Contracts.Response.Community;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Extensions;

    public class CommunityMappingProfile : Profile
    {
        public CommunityMappingProfile()
        {
            this.CreateMap<ProfileResponse, ProfileInfo>()
                .ForMember(dto => dto.OwnerName, cr => cr.Ignore())
                .ForMember(dto => dto.EmailMailViolationsWarnings, cr => cr.MapFrom(x => x.EmailMailViolationsWarnings.ToCollectionFromString(";")));
            this.CreateMap<PropertyResponse, Property>()
                .ForMember(dto => dto.City, pr => pr.MapFrom(x => x.City.ToCity()))
                .ForMember(dto => dto.County, pr => pr.MapFrom(x => x.County.ToEnumFromEnumMember<Counties>()))
                .ForMember(dto => dto.ConstructionStage, pr => pr.MapFrom(x => x.ConstructionStage.ToEnumFromEnumMember<ConstructionStage>()))
                .ForMember(dto => dto.LotDescription, pr => pr.MapFrom(x => x.LotDescription.ToLotDescription()))
                .ForMember(dto => dto.PropertyType, pr => pr.MapFrom(x => x.PropertyType.ToEnumFromEnumMember<PropertySubType>()))
                .ForMember(dto => dto.MlsArea, pr => pr.MapFrom(x => x.MlsArea.ToEnumFromEnumMember<MlsArea>()));
            this.CreateMap<SchoolsResponse, SchoolsInfo>()
                .ForMember(dto => dto.ElementarySchool, pr => pr.MapFrom(x => x.ElementarySchool.ToEnumFromEnumMember<ElementarySchool>()))
                .ForMember(dto => dto.MiddleSchool, pr => pr.MapFrom(x => x.MiddleSchool.ToEnumFromEnumMember<MiddleSchool>()))
                .ForMember(dto => dto.HighSchool, pr => pr.MapFrom(x => x.HighSchool.ToEnumFromEnumMember<HighSchool>()))
                .ForMember(dto => dto.SchoolDistrict, pr => pr.MapFrom(x => x.SchoolDistrict.ToEnumFromEnumMember<SchoolDistrict>()));
            this.CreateMap<UtilitiesReponse, Utilities>()
                .ForMember(dto => dto.WaterSource, cr => cr.MapFrom(x => x.WaterDesc.CsvToEnum<WaterSource>(true)))
                .ForMember(dto => dto.WaterSewer, cr => cr.MapFrom(x => x.SewerDesc.CsvToEnum<WaterSewer>(true)))
                .ForMember(dto => dto.NeighborhoodAmenities, cr => cr.MapFrom(x => x.CommonFeatures.CsvToEnum<NeighborhoodAmenities>(true)))
                .ForMember(dto => dto.ConstructionMaterials, cr => cr.MapFrom(x => x.Exterior.CsvToEnum<ConstructionMaterials>(true)))
                .ForMember(dto => dto.GarageDescription, cr => cr.MapFrom(x => x.GarageDescription.CsvToEnum<GarageDescription>(true)))
                .ForMember(dto => dto.GarageSpaces, pr => pr.MapFrom(x => x.GarageCapacity))
                .ForMember(dto => dto.PatioAndPorchFeatures, pr => pr.MapFrom(x => x.PatioAndPorchFeatures.CsvToEnum<PatioAndPorchFeatures>(true)))
                .ForMember(dto => dto.Fencing, pr => pr.MapFrom(x => x.FenceDescription.CsvToEnum<Fencing>(true)))
                .ForMember(dto => dto.Foundation, pr => pr.MapFrom(x => x.Foundation.CsvToEnum<Foundation>(true)))
                .ForMember(dto => dto.WindowFeatures, pr => pr.MapFrom(x => x.WindowCoverings.CsvToEnum<WindowFeatures>(true)))
                .ForMember(dto => dto.SecurityFeatures, pr => pr.MapFrom(x => x.SecurityFeatures.CsvToEnum<SecurityFeatures>(true)))
                .ForMember(dto => dto.InteriorFeatures, pr => pr.MapFrom(x => x.InteriorFeatures.CsvToEnum<InteriorFeatures>(true)))
                .ForMember(dto => dto.LaundryLocation, pr => pr.MapFrom(x => x.LaundryLocation.CsvToEnum<LaundryLocation>(true)))
                .ForMember(dto => dto.Appliances, pr => pr.MapFrom(x => x.Appliances.CsvToEnum<Appliances>(true)))
                .ForMember(dto => dto.HeatSystem, pr => pr.MapFrom(x => x.HeatSystem.CsvToEnum<HeatingSystem>(true)))
                .ForMember(dto => dto.CoolingSystem, pr => pr.MapFrom(x => x.CoolingSystem.CsvToEnum<CoolingSystem>(true)))
                .ForMember(dto => dto.UtilitiesDescription, pr => pr.MapFrom(x => x.LotImprovements.CsvToEnum<UtilitiesDescription>(true)))
                .ForMember(dto => dto.RestrictionsDescription, pr => pr.MapFrom(x => x.RestrictionsDescription.CsvToEnum<RestrictionsDescription>(true)))
                .ForMember(dto => dto.Floors, pr => pr.MapFrom(x => x.Floors.CsvToEnum<Flooring>(true)))
                .ForMember(dto => dto.FireplaceDescription, pr => pr.MapFrom(x => x.FireplaceDescription.ToFireplaceDescription()))
                .ForMember(dto => dto.ExteriorFeatures, pr => pr.MapFrom(x => x.ExteriorFeatures.CsvToEnum<ExteriorFeatures>(true)))
                .ForMember(dto => dto.RoofDescription, pr => pr.MapFrom(x => x.RoofDescription.CsvToEnum<RoofDescription>(true)))
                .ForMember(dto => dto.Foundation, pr => pr.MapFrom(x => x.Foundation.CsvToEnum<Foundation>(true)))
                .ForMember(dto => dto.HeatSystem, pr => pr.MapFrom(x => x.HeatSystem.CsvToEnum<HeatingSystem>(true)))
                .ForMember(dto => dto.CoolingSystem, pr => pr.MapFrom(x => x.CoolingSystem.CsvToEnum<CoolingSystem>(true)))
                .ForMember(dto => dto.View, pr => pr.MapFrom(x => x.ViewDescription.ToView()));
            this.CreateMap<FinancialResponse, CommunityFinancialInfo>()
                .ForMember(dto => dto.AcceptableFinancing, pr => pr.MapFrom(x => x.AcceptableFinancing.ToAcceptableFinancing()))
                .ForMember(dto => dto.TaxExemptions, pr => pr.MapFrom(x => x.TaxExemptions.ToTaxExemptions()))
                .ForMember(dto => dto.HoaIncludes, pr => pr.MapFrom(x => x.HoaIncludes.ToHoaIncludes()))
                .ForMember(dto => dto.BillingFrequency, pr => pr.MapFrom(x => x.BillingFrequency.ToEnumFromEnumMember<BillingFrequency>()))
                .ForMember(dto => dto.HOARequirement, pr => pr.MapFrom(x => x.HOARequirement.ToEnumFromEnumMember<HoaRequirement>()))
                .ForMember(dto => dto.AgentBonusAmountType, pr => pr.MapFrom(x => x.AgentBonusAmountType.ToEnumFromEnumMember<CommissionType>()))
                .ForMember(dto => dto.ReadableBuyersAgentCommission, pr => pr.MapFrom(x => x.BuyersAgentCommission.GetCommissionAmount(x.BuyersAgentCommissionType.ToEnumFromEnumMember<CommissionType>())))
                .ForMember(dto => dto.BuyersAgentCommissionType, pr => pr.MapFrom(x => x.BuyersAgentCommissionType.ToEnumFromEnumMember<CommissionType>()))
                .ForMember(dto => dto.BonusExpirationDate, pr => pr.MapFrom(x => x.HasBonusWithAmount ? x.BonusExpirationDate : null));
            this.CreateMap<ShowingResponse, CommunityShowingInfo>()
                .ForMember(dto => dto.OwnerName, pr => pr.Ignore())
                .ForMember(dto => dto.OccupantPhone, pr => pr.MapFrom(x => x.AltPhoneCommunity))
                .ForMember(dto => dto.ContactPhone, pr => pr.MapFrom(x => x.AgentListApptPhone))
                .ForMember(dto => dto.ShowingInstructions, pr => pr.MapFrom(x => x.Showing))
                .ForMember(dto => dto.RealtorContactEmail, pr => pr.MapFrom(x => x.RealtorContactEmail.ToCollectionFromString(";")))
                .ForMember(dto => dto.ShowingRequirements, pr => pr.MapFrom(x => x.ShowingRequirements.CsvToEnum<ShowingRequirements>(true)))
                .ForMember(dto => dto.LockBoxType, pr => pr.MapFrom(x => x.LockBoxType.ToLockBoxType()))
                .ForMember(dto => dto.Directions, cr => cr.MapFrom(x => x.Directions.GetSubstring(CommunityShowingInfo.MaxDirectionsLength)));
            this.CreateMap<EmailLeadResponse, EmailLead>()
                .ForMember(dto => dto.EmailLeadPrincipal, cr => cr.MapFrom(x => x.Email))
                .ForMember(dto => dto.EmailLeadSecondary, cr => cr.MapFrom(x => x.Email2))
                .ForMember(dto => dto.EmailLeadOther, cr => cr.MapFrom(x => x.Email3));

            this.CreateMap<OpenHouseResponse, CommunityOpenHouse>()
                .ForMember(dto => dto.Refreshments, pr => pr.MapFrom(x => x.Refreshments.CsvToEnum<Refreshments>(true)))
                .ForMember(dto => dto.Type, oh => oh.MapFrom(x => x.Type.ToOpenHouseType()))
                .ForMember(dto => dto.IsDeleted, oh => oh.MapFrom(x => false))
                .ForMember(dto => dto.OpenHouseType, oh => oh.Ignore())
                .ForMember(dto => dto.CommunityId, oh => oh.Ignore())
                .ForMember(dto => dto.Community, oh => oh.Ignore())
                .ForMember(dto => dto.Id, oh => oh.Ignore())
                .ForMember(dto => dto.SysModifiedOn, oh => oh.Ignore())
                .ForMember(dto => dto.SysCreatedOn, oh => oh.Ignore())
                .ForMember(dto => dto.SysModifiedBy, oh => oh.Ignore())
                .ForMember(dto => dto.CompanyId, oh => oh.Ignore())
                .ForMember(dto => dto.SysCreatedBy, oh => oh.Ignore())
                .ForMember(dto => dto.SysTimestamp, oh => oh.Ignore());
            this.CreateMap<SalesOfficeResponse, CommunitySaleOffice>()
                .ForMember(dto => dto.SalesOfficeCity, oh => oh.MapFrom(x => x.SalesOfficeCity.ToCity()))
                .ForMember(dto => dto.IsSalesOffice, oh => oh.Ignore());
            this.CreateMap<CommunityResponse, CommunitySale>()
                .ForMember(dto => dto.Property, cr => cr.MapFrom(x => x.PropertyInfo))
                .ForMember(dto => dto.Utilities, cr => cr.MapFrom(x => x.UtilitiesInfo))
                .ForMember(dto => dto.Financial, cr => cr.MapFrom(x => x.FinancialInfo))
                .ForMember(dto => dto.Showing, cr => cr.MapFrom(x => x.ShowingInfo))
                .ForMember(dto => dto.EmailLead, cr => cr.MapFrom(x => x.EmailLeads))
                .ForMember(dto => dto.LegacyProfileId, cr => cr.MapFrom(x => x.LegacyCommunityId))
                .ForMember(dto => dto.IsDeleted, cr => cr.MapFrom(x => false))
                .ForMember(dto => dto.CommunityType, cr => cr.MapFrom(x => CommunityType.SaleCommunity))
                .ForMember(dto => dto.Id, cr => cr.Ignore())
                .ForMember(dto => dto.LegacyId, cr => cr.Ignore())
                .ForMember(dto => dto.OpenHouses, cr => cr.Ignore())
                .ForMember(dto => dto.SysModifiedOn, cr => cr.Ignore())
                .ForMember(dto => dto.SysCreatedOn, cr => cr.Ignore())
                .ForMember(dto => dto.SysModifiedBy, cr => cr.Ignore())
                .ForMember(dto => dto.CompanyId, cr => cr.Ignore())
                .ForMember(dto => dto.SysCreatedBy, cr => cr.Ignore())
                .ForMember(dto => dto.SysTimestamp, cr => cr.Ignore())
                .ForMember(dto => dto.Employees, cr => cr.Ignore())
                .ForMember(dto => dto.Changes, cr => cr.Ignore())
                .ForMember(dto => dto.XmlStatus, cr => cr.Ignore())
                .ForMember(dto => dto.LastPhotoRequestCreationDate, cr => cr.Ignore())
                .ForMember(dto => dto.LastPhotoRequestId, cr => cr.Ignore())
                .ForMember(dto => dto.LotListings, cr => cr.Ignore())
                .ForMember(dto => dto.SaleProperties, cr => cr.Ignore());
        }
    }
}
