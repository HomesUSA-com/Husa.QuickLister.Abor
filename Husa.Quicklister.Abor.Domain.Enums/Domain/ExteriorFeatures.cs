namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ExteriorFeatures
    {
        [EnumMember(Value = "Balcony")]
        [Description("Balcony")]
        Balcony,
        [EnumMember(Value = "EXTER")]
        [Description("Exterior Steps")]
        ExteriorSteps,
        [EnumMember(Value = "EGRLL")]
        [Description("Grill-Electric")]
        GrillElectric,
        [EnumMember(Value = "GGRLL")]
        [Description("Grill-Gas")]
        GrillGas,
        [EnumMember(Value = "LIGHTN")]
        [Description("Lighting")]
        Lighting,
        [EnumMember(Value = "GUTPL")]
        [Description("Gutters-Partial")]
        GuttersPartial,
        [EnumMember(Value = "NOENS")]
        [Description("No Exterior Steps")]
        NoExteriorSteps,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "OGRLL")]
        [Description("Outdoor Grill")]
        OutdoorGrill,
        [EnumMember(Value = "PLGD")]
        [Description("Playground")]
        Playground,
        [EnumMember(Value = "PVTBF")]
        [Description("Private Yard")]
        PrivateYard,
        [EnumMember(Value = "TNNS")]
        [Description("Tennis Court(s)")]
        TennisCourts,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
