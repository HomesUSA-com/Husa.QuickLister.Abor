namespace Husa.Quicklister.Abor.Api.Mappings.Migration
{
    using AutoMapper;
    using Husa.Extensions.Common;
    using Husa.Migration.Api.Contracts.Response;
    using Husa.Migration.Api.Contracts.Response.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using MigrationOpenHouseType = Husa.Migration.Crosscutting.Enums.OpenHouseType;

    public class CommuntiyMappingProfile : Profile
    {
        public CommuntiyMappingProfile()
        {
            this.CreateMap<ProfileResponse, ProfileInfo>()
                .ForMember(dto => dto.OwnerName, cr => cr.Ignore());
            this.CreateMap<PropertyResponse, Property>()
                .ForMember(dto => dto.LotSize, cr => cr.Ignore())
                .ForMember(dto => dto.PropertyType, cr => cr.Ignore())
                .ForMember(dto => dto.LotDimension, cr => cr.Ignore())
                .ForMember(dto => dto.ConstructionStage, cr => cr.Ignore())
                .ForMember(dto => dto.LotDescription, cr => cr.Ignore());
            this.CreateMap<SchoolsResponse, SchoolsInfo>()
                .ForMember(dto => dto.ElementarySchool, cr => cr.MapFrom(x => x.SchoolName1))
                .ForMember(dto => dto.MiddleSchool, cr => cr.MapFrom(x => x.SchoolName2))
                .ForMember(dto => dto.HighSchool, cr => cr.MapFrom(x => x.SchoolName3))
                .ForMember(dto => dto.OtherMiddleSchool, cr => cr.Ignore())
                .ForMember(dto => dto.OtherHighSchool, cr => cr.Ignore())
                .ForMember(dto => dto.OtherElementarySchool, cr => cr.Ignore());
            this.CreateMap<UtilitiesReponse, Utilities>()
                .ForMember(dto => dto.WaterSewer, cr => cr.MapFrom(x => x.WaterDesc.CsvToEnum<WaterSewer>(true)))
                .ForMember(dto => dto.CoolingSystem, cr => cr.MapFrom(x => x.CoolingSystem.CsvToEnum<CoolingSystem>(true)))
                .ForMember(dto => dto.ExteriorFeatures, cr => cr.MapFrom(x => x.ExteriorDesc.CsvToEnum<ExteriorFeatures>(true)))
                .ForMember(dto => dto.FireplaceDescription, cr => cr.MapFrom(x => x.FireplaceDescription.CsvToEnum<FireplaceDescription>(true)))
                .ForMember(dto => dto.NeighborhoodAmenities, cr => cr.MapFrom(x => x.CommonFeatures.CsvToEnum<NeighborhoodAmenities>(true)))
                .ForMember(dto => dto.Foundation, cr => cr.MapFrom(x => x.Foundation.CsvToEnum<Foundation>(true)))
                .ForMember(dto => dto.Floors, cr => cr.MapFrom(x => x.Floors.CsvToEnum<Flooring>(true)))
                .ForMember(dto => dto.HeatSystem, cr => cr.MapFrom(x => x.HeatSystem.CsvToEnum<HeatingSystem>(true)))
                .ForMember(dto => dto.RoofDescription, cr => cr.MapFrom(x => x.RoofDescription.CsvToEnum<RoofDescription>(true)))
                .ForMember(dto => dto.WaterSource, cr => cr.Ignore())
                .ForMember(dto => dto.RestrictionsDescription, cr => cr.Ignore())
                .ForMember(dto => dto.UtilitiesDescription, cr => cr.Ignore())
                .ForMember(dto => dto.GarageSpaces, cr => cr.Ignore())
                .ForMember(dto => dto.Appliances, cr => cr.Ignore())
                .ForMember(dto => dto.GarageDescription, cr => cr.Ignore())
                .ForMember(dto => dto.LaundryFeatures, cr => cr.Ignore())
                .ForMember(dto => dto.LaundryLocation, cr => cr.Ignore())
                .ForMember(dto => dto.InteriorFeatures, cr => cr.Ignore())
                .ForMember(dto => dto.KitchenFeatures, cr => cr.Ignore())
                .ForMember(dto => dto.MasterBedroomFeatures, cr => cr.Ignore())
                .ForMember(dto => dto.WaterAccessDescription, cr => cr.Ignore())
                .ForMember(dto => dto.SecurityFeatures, cr => cr.Ignore())
                .ForMember(dto => dto.WindowFeatures, cr => cr.Ignore())
                .ForMember(dto => dto.Foundation, cr => cr.Ignore())
                .ForMember(dto => dto.RoofDescription, cr => cr.Ignore())
                .ForMember(dto => dto.Fencing, cr => cr.Ignore())
                .ForMember(dto => dto.ConstructionMaterials, cr => cr.Ignore())
                .ForMember(dto => dto.PatioAndPorchFeatures, cr => cr.Ignore())
                .ForMember(dto => dto.View, cr => cr.Ignore())
                .ForMember(dto => dto.ExteriorFeatures, cr => cr.Ignore());
            this.CreateMap<FinancialResponse, CommunityFinancialInfo>()
                .ForMember(dto => dto.BuyersAgentCommission, cr => cr.MapFrom(x => GetBuyersAgentCommissionNumber(x.CompBuy)))
                .ForMember(dto => dto.BuyersAgentCommissionType, cr => cr.MapFrom(x => GetBuyersAgentCommissionType(x.CompBuy)))
                .ForMember(dto => dto.HOARequirement, cr => cr.MapFrom(x => x.HOARequirement))
                .ForMember(dto => dto.TaxExemptions, cr => cr.Ignore())
                .ForMember(dto => dto.HoaIncludes, cr => cr.Ignore())
                .ForMember(dto => dto.HoaName, cr => cr.Ignore())
                .ForMember(dto => dto.HoaFee, cr => cr.Ignore())
                .ForMember(dto => dto.HasHoa, cr => cr.Ignore())
                .ForMember(dto => dto.HasAgentBonus, cr => cr.Ignore())
                .ForMember(dto => dto.HasBonusWithAmount, cr => cr.Ignore())
                .ForMember(dto => dto.BillingFrequency, cr => cr.Ignore())
                .ForMember(dto => dto.BonusExpirationDate, cr => cr.Ignore())
                .ForMember(dto => dto.AgentBonusAmount, cr => cr.Ignore())
                .ForMember(dto => dto.AgentBonusAmountType, cr => cr.Ignore())
                .ForMember(dto => dto.HasBuyerIncentive, cr => cr.Ignore())
                .ForMember(dto => dto.AcceptableFinancing, cr => cr.Ignore());
            this.CreateMap<ShowingResponse, CommunityShowingInfo>()
                .ForMember(dto => dto.ShowingRequirements, cr => cr.Ignore())
                .ForMember(dto => dto.LockBoxType, cr => cr.Ignore())
                .ForMember(dto => dto.OccupantPhone, cr => cr.MapFrom(x => x.AltPhoneCommunity))
                .ForMember(dto => dto.ContactPhone, cr => cr.MapFrom(x => x.AgentListApptPhone))
                .ForMember(dto => dto.ShowingInstructions, cr => cr.MapFrom(x => x.Showing))
                .ForMember(dto => dto.Directions, cr => cr.MapFrom(x => x.Directions.Length > CommunityShowingInfo.MaxDirectionsLength ? x.Directions.Substring(CommunityShowingInfo.MaxDirectionsLength) : x.Directions));
            this.CreateMap<EmailLeadResponse, EmailLead>()
                .ForMember(dto => dto.EmailLeadPrincipal, cr => cr.MapFrom(x => x.Email))
                .ForMember(dto => dto.EmailLeadSecondary, cr => cr.MapFrom(x => x.Email2))
                .ForMember(dto => dto.EmailLeadOther, cr => cr.MapFrom(x => x.Email3));

            this.CreateMap<OpenHouseResponse, CommunityOpenHouse>()
                .ForMember(dto => dto.OpenHouseType, oh => oh.MapFrom(x => ToOpenHouseType(x.Type)))
                .ForMember(dto => dto.IsDeleted, oh => oh.MapFrom(x => false))
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
                .ForMember(dto => dto.SalesOfficeCity, oh => oh.MapFrom(x => x.SalesOfficeCity.GetEnumValueFromDescription<Cities>()))
                .ForMember(dto => dto.IsSalesOffice, oh => oh.Ignore());
            this.CreateMap<CommunityResponse, CommunitySale>()
                .ForMember(dto => dto.Property, cr => cr.MapFrom(x => x.PropertyInfo))
                .ForMember(dto => dto.Utilities, cr => cr.MapFrom(x => x.UtilitiesInfo))
                .ForMember(dto => dto.Financial, cr => cr.MapFrom(x => x.FinancialInfo))
                .ForMember(dto => dto.Showing, cr => cr.MapFrom(x => x.ShowingInfo))
                .ForMember(dto => dto.EmailLead, cr => cr.MapFrom(x => x.EmailLeads))
                .ForMember(dto => dto.LegacyId, cr => cr.MapFrom(x => x.Id))
                .ForMember(dto => dto.IsDeleted, cr => cr.MapFrom(x => false))
                .ForMember(dto => dto.CommunityType, cr => cr.MapFrom(x => CommunityType.SaleCommunity))
                .ForMember(dto => dto.Id, cr => cr.Ignore())
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
                .ForMember(dto => dto.SaleProperties, cr => cr.Ignore());
        }

        private static OpenHouseType ToOpenHouseType(MigrationOpenHouseType type) => type switch
        {
            MigrationOpenHouseType.Monday => OpenHouseType.Monday,
            MigrationOpenHouseType.Tuesday => OpenHouseType.Tuesday,
            MigrationOpenHouseType.Wednesday => OpenHouseType.Wednesday,
            MigrationOpenHouseType.Thursday => OpenHouseType.Thursday,
            MigrationOpenHouseType.Friday => OpenHouseType.Friday,
            MigrationOpenHouseType.Saturday => OpenHouseType.Saturday,
            MigrationOpenHouseType.Sunday => OpenHouseType.Sunday,
            _ => OpenHouseType.Monday,
        };

        private static decimal? GetBuyersAgentCommissionNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var newValue = value.Replace("%", string.Empty).Replace("$", string.Empty);
            return decimal.TryParse(newValue, out var commission) ? commission : null;
        }

        private static CommissionType GetBuyersAgentCommissionType(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && value.Contains('$') ? CommissionType.Amount : CommissionType.Percent;
        }
    }
}
