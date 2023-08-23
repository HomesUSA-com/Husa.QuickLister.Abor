namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ExteriorFeatures
    {
        [EnumMember(Value = "Balcony")]
        [Description("Balcony")]
        Balcony,
        [EnumMember(Value = "ExteriorSteps")]
        [Description("Exterior Steps")]
        ExteriorSteps,
        [EnumMember(Value = "ElectricGrill")]
        [Description("Grill-Electric")]
        GrillElectric,
        [EnumMember(Value = "GasGrill")]
        [Description("Grill-Gas")]
        GrillGas,
        [EnumMember(Value = "Lighting")]
        [Description("Lighting")]
        Lighting,
        [EnumMember(Value = "NoExteriorSteps")]
        [Description("No Exterior Steps")]
        NoExteriorSteps,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "OutdoorGrill")]
        [Description("Outdoor Grill")]
        OutdoorGrill,
        [EnumMember(Value = "Playground")]
        [Description("Playground")]
        Playground,
        [EnumMember(Value = "PrivateYard")]
        [Description("Private Yard")]
        PrivateYard,
        [EnumMember(Value = "TennisCourts")]
        [Description("Tennis Court(s)")]
        TennisCourts,
    }
}
