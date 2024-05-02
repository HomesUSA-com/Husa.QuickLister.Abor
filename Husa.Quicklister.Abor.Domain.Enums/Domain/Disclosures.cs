namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Disclosures
    {
        [EnumMember(Value = "APSENPR")]
        [Description("Approved Seniors Project")]
        ApprovedSeniorsProject,
        [EnumMember(Value = "CORPLST")]
        [Description("Corporate Listing")]
        CorporateListing,
        [EnumMember(Value = "CORPOWND")]
        [Description("Corporate Owned")]
        CorporateOwned,
        [EnumMember(Value = "EXCLU")]
        [Description("Exclusions")]
        Exclusions,
        [EnumMember(Value = "FAMILREL")]
        [Description("Familial Relation")]
        FamilialRelation,
        [EnumMember(Value = "HMPRT")]
        [Description("Home Protection Plan")]
        HomeProtectionPlan,
        [EnumMember(Value = "LP")]
        [Description("Lead Base Paint Addendum")]
        LeadBasePaintAddendum,
        [EnumMember(Value = "MLNDA")]
        [Description("Mi/Lenders Approval")]
        MiLendersApproval,
        [EnumMember(Value = "MD")]
        [Description("Municipal Utility District (MUD)")]
        MunicipalUtilityDistrictMUD,
        [EnumMember(Value = "OTHDC")]
        [Description("Other Disclosures")]
        OtherDisclosures,
        [EnumMember(Value = "OREO")]
        [Description("Other Real Estate Owned")]
        OtherRealEstateOwned,
        [EnumMember(Value = "OWNRAGT")]
        [Description("Owner/Agent")]
        OwnerAgent,
        [EnumMember(Value = "PD")]
        [Description("Planned Unit Development")]
        PlannedUnitDevelopment,
        [EnumMember(Value = "PRSRESVR")]
        [Description("Prospects Reserved")]
        ProspectsReserved,
        [EnumMember(Value = "PID")]
        [Description("Public Improvement District")]
        PublicImprovementDistrict,
        [EnumMember(Value = "REO")]
        [Description("Real Estate Owned (Lender)")]
        RealEstateOwnedLender,
        [EnumMember(Value = "RELO")]
        [Description("Relo Addendum Required")]
        ReloAddendumRequired,
        [EnumMember(Value = "RNREC")]
        [Description("Rental Records Available")]
        RentalRecordsAvailable,
        [EnumMember(Value = "WARRSAGT")]
        [Description("Residential Service Contract")]
        ResidentialServiceContract,
        [EnumMember(Value = "ROFR")]
        [Description("Right of First Refusal (ROFR)")]
        RightofFirstRefusalROFR,
        [EnumMember(Value = "SLRDC")]
        [Description("Seller Disclosure")]
        SellerDisclosure,
        [EnumMember(Value = "SPROV")]
        [Description("Seller Provided Survey Available")]
        SellerProvidedSurveyAvailable,
        [EnumMember(Value = "SPADD")]
        [Description("Special Addendum")]
        SpecialAddendum,
        [EnumMember(Value = "WARR")]
        [Description("Warranty/See Remarks")]
        WarrantySeeRemarks,
        [EnumMember(Value = "WTRDIST")]
        [Description("Water District")]
        WaterDistrict,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
