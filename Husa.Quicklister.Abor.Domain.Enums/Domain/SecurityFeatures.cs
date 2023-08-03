namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum SecurityFeatures
    {
        [EnumMember(Value = "CarbonMonoxideDetectors")]
        [Description("Carbon Monoxide Detector(s)")]
        CarbonMonoxideDetectors,
        [EnumMember(Value = "FireAlarm")]
        [Description("Fire Alarm")]
        FireAlarm,
        [EnumMember(Value = "FireSprinklerSystem")]
        [Description("Fire Sprinkler System")]
        FireSprinklerSystem,
        [EnumMember(Value = "GatedwithGuard")]
        [Description("Gated with Guard")]
        GatedwithGuard,
        [EnumMember(Value = "None")]
        [Description("None ")]
        None,
        [EnumMember(Value = "Prewired")]
        [Description("Prewired")]
        Prewired,
        [EnumMember(Value = "SecuredGarageParking")]
        [Description("Secured Garage/Parking")]
        SecuredGarageParking,
        [EnumMember(Value = "SecuritySystem")]
        [Description("Security System")]
        SecuritySystem,
        [EnumMember(Value = "SecuritySystemLeased")]
        [Description("Security System Leased")]
        SecuritySystemLeased,
        [EnumMember(Value = "SecuritySystemOwned")]
        [Description("Security System Owned")]
        SecuritySystemOwned,
        [EnumMember(Value = "SeeRemarks")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "SmokeDetectors")]
        [Description("Smoke Detector(s)")]
        SmokeDetectors,
    }
}
