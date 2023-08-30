namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum SecurityFeatures
    {
        [EnumMember(Value = "CMDTR")]
        [Description("Carbon Monoxide Detector(s)")]
        CarbonMonoxideDetectors,
        [EnumMember(Value = "FRAST")]
        [Description("Fire Alarm")]
        FireAlarm,
        [EnumMember(Value = "FSPRK")]
        [Description("Fire Sprinkler System")]
        FireSprinklerSystem,
        [EnumMember(Value = "GTEDGD")]
        [Description("Gated with Guard")]
        GatedwithGuard,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "Prewired")]
        [Description("Prewired")]
        Prewired,
        [EnumMember(Value = "SGAR")]
        [Description("Secured Garage/Parking")]
        SecuredGarageParking,
        [EnumMember(Value = "SECSYS")]
        [Description("Security System")]
        SecuritySystem,
        [EnumMember(Value = "SCSYL")]
        [Description("Security System Leased")]
        SecuritySystemLeased,
        [EnumMember(Value = "SCSYO")]
        [Description("Security System Owned")]
        SecuritySystemOwned,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "SMKDT")]
        [Description("Smoke Detector(s)")]
        SmokeDetectors,
    }
}
