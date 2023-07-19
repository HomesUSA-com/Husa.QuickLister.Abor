namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum GarageDescription
    {
        [EnumMember(Value = "NONE")]
        [Description("None/Not Applicable")]
        NotApplicable,
        [EnumMember(Value = "1GAR")]
        [Description("One Car Garage")]
        OneCarGarage,
        [EnumMember(Value = "2GAR")]
        [Description("Two Car Garage")]
        TwoCarGarage,
        [EnumMember(Value = "3GAR")]
        [Description("Three Car Garage")]
        ThreeCarGarage,
        [EnumMember(Value = "4+GAR")]
        [Description("Four or More Car Garage")]
        FourPlusCarGarage,
        [EnumMember(Value = "ATT")]
        [Description("Attached")]
        Attached,
        [EnumMember(Value = "DTCHD")]
        [Description("Detached")]
        Detached,
        [EnumMember(Value = "OVRSZ")]
        [Description("Oversized")]
        Oversized,
        [EnumMember(Value = "REAR")]
        [Description("Rear Entry")]
        RearEntry,
        [EnumMember(Value = "SIDE")]
        [Description("Side Entry")]
        SideEntry,
        [EnumMember(Value = "TANDEM")]
        [Description("Tandem")]
        Tandem,
    }
}
