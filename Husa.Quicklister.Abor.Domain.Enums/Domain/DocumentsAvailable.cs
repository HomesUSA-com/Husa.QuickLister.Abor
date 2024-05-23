namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum DocumentsAvailable
    {
        [EnumMember(Value = "AERIAL")]
        [Description("Aerial Photos")]
        AerialPhotos,
        [EnumMember(Value = "ALTAS")]
        [Description("ALTA Survey")]
        ALTASurvey,
        [EnumMember(Value = "ABKUPAVL")]
        [Description("APOD/Backup Available")]
        APODBackupAvailable,
        [EnumMember(Value = "APPRAIS")]
        [Description("Appraisal")]
        Appraisal,
        [EnumMember(Value = "ARCHL")]
        [Description("Archeological Site")]
        ArcheologicalSite,
        [EnumMember(Value = "BLDGPLAN")]
        [Description("Building Plans")]
        BuildingPlans,
        [EnumMember(Value = "CNDDA")]
        [Description("Condo Docs Available")]
        CondoDocsAvailable,
        [EnumMember(Value = "COSTEST")]
        [Description("Cost Estimates")]
        CostEstimates,
        [EnumMember(Value = "DEEDRS")]
        [Description("Deed Restrictions")]
        DeedRestrictions,
        [EnumMember(Value = "DEVPL")]
        [Description("Development Plan")]
        DevelopmentPlan,
        [EnumMember(Value = "ECRA")]
        [Description("ECRA Clearance")]
        ECRAClearance,
        [EnumMember(Value = "ECAD")]
        [Description("Energy Conservation Audit Disclosure")]
        EnergyConservationAuditDisclosure,
        [EnumMember(Value = "EES")]
        [Description("Energy, Environment and Sustainability")]
        EnergyEnvironmentAndSustainability,
        [EnumMember(Value = "ENGRPT")]
        [Description("Engineering Report")]
        EngineeringReport,
        [EnumMember(Value = "ENVIRNMT")]
        [Description("Environmental Study")]
        EnvironmentalStudy,
        [EnumMember(Value = "ENVST")]
        [Description("Environmental Study Complete")]
        EnvironmentalStudyComplete,
        [EnumMember(Value = "FSBST")]
        [Description("Feasibility Study")]
        FeasibilityStudy,
        [EnumMember(Value = "FINSTMNT")]
        [Description("Financial Statement")]
        FinancialStatement,
        [EnumMember(Value = "FINANCNG")]
        [Description("Financing")]
        Financing,
        [EnumMember(Value = "FLOEAS")]
        [Description("Flowage Easement")]
        FlowageEasement,
        [EnumMember(Value = "GEOLG")]
        [Description("Geological")]
        Geological,
        [EnumMember(Value = "HIST")]
        [Description("Historical")]
        Historical,
        [EnumMember(Value = "INEXS")]
        [Description("Income & Expense Statement")]
        IncomeAndExpenseStatement,
        [EnumMember(Value = "LEASE")]
        [Description("Leases")]
        Leases,
        [EnumMember(Value = "LEGALDOC")]
        [Description("Legal Documents")]
        LegalDocuments,
        [EnumMember(Value = "MUD")]
        [Description("Municipal Utility District (MUD)")]
        MunicipalUtilityDistrictMUD,
        [EnumMember(Value = "OAKWILT")]
        [Description("Oak Wilt Test")]
        OakWiltTest,
        [EnumMember(Value = "OFFSITE")]
        [Description("Off-Site Requirements")]
        OffSiteRequirements,
        [EnumMember(Value = "PL")]
        [Description("Profit & Loss Statement")]
        ProfitAndLossStatement,
        [EnumMember(Value = "PRCND")]
        [Description("Perc Test Needed")]
        PercTestNeeded,
        [EnumMember(Value = "PERC")]
        [Description("Perc Test Results/Map")]
        PercTestResultsMap,
        [EnumMember(Value = "PID")]
        [Description("Public Improvement District")]
        PublicImprovementDistrict,
        [EnumMember(Value = "RENT")]
        [Description("Rent Roll")]
        RentRoll,
        [EnumMember(Value = "SCHPP")]
        [Description("Schedule of Personal Property")]
        ScheduleofPersonalProperty,
        [EnumMember(Value = "SECOD")]
        [Description("Security Codes")]
        SecurityCodes,
        [EnumMember(Value = "SEPCERT")]
        [Description("Septic Certification")]
        SepticCertification,
        [EnumMember(Value = "SITE")]
        [Description("Site Plan")]
        SitePlan,
        [EnumMember(Value = "SOIL")]
        [Description("Soil Test")]
        SoilTest,
        [EnumMember(Value = "SPASDIST")]
        [Description("Special Assessment District")]
        SpecialAssessmentDistrict,
        [EnumMember(Value = "SIC")]
        [Description("Standard Industrial Classification")]
        StandardIndustrialClassification,
        [EnumMember(Value = "SUV")]
        [Description("Survey")]
        Survey,
        [EnumMember(Value = "TAXRTA")]
        [Description("Tax Return Available")]
        TaxReturnAvailable,
        [EnumMember(Value = "TOPMP")]
        [Description("Topography Map")]
        TopographyMap,
        [EnumMember(Value = "UNITMIX")]
        [Description("Unit Mix Schedule")]
        UnitMixSchedule,
        [EnumMember(Value = "UTEAS")]
        [Description("Utility Easement")]
        UtilityEasement,
        [EnumMember(Value = "VENDOR")]
        [Description("Vendor Leases")]
        VendorLeases,
        [EnumMember(Value = "WATER")]
        [Description("Water Capacity/Quality Report")]
        WaterCapacityQualityReport,
        [EnumMember(Value = "WARPT")]
        [Description("Water/Well Report")]
        WaterWellReport,
        [EnumMember(Value = "WATEST")]
        [Description("Water/Well Test")]
        WaterWellTest,
        [EnumMember(Value = "WETAP")]
        [Description("Wetlands Approval/Waiver")]
        WetlandsApprovalWaiver,
        [EnumMember(Value = "WETDEL")]
        [Description("Wetlands Delineation Map")]
        WetlandsDelineationMap,
        [EnumMember(Value = "NA")]
        [Description("None Available")]
        NoneAvailable,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
