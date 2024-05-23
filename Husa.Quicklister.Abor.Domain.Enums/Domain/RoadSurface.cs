namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RoadSurface
    {
        [EnumMember(Value = "APAVD")]
        [Description("Alley Paved")]
        AlleyPaved,
        [EnumMember(Value = "ASPHALT")]
        [Description("Asphalt")]
        Asphalt,
        [EnumMember(Value = "CLICH")]
        [Description("Caliche")]
        Caliche,
        [EnumMember(Value = "CNSL")]
        [Description("Chip And Seal")]
        ChipAndSeal,
        [EnumMember(Value = "CNCRT")]
        [Description("Concrete")]
        Concrete,
        [EnumMember(Value = "CRBGT")]
        [Description("Curb/Gutter")]
        CurbGutter,
        [EnumMember(Value = "DIRT")]
        [Description("Dirt")]
        Dirt,
        [EnumMember(Value = "GRVL")]
        [Description("Gravel")]
        Gravel,
        [EnumMember(Value = "PVD")]
        [Description("Paved")]
        Paved,
        [EnumMember(Value = "SIDWL")]
        [Description("Sidewalks")]
        Sidewalks,
        [EnumMember(Value = "UNIMP")]
        [Description("Unimproved")]
        Unimproved,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
