namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum SoilType
    {
        [EnumMember(Value = "BLKLN")]
        [Description("Black Land")]
        BlackLand,
        [EnumMember(Value = "CLICH")]
        [Description("Caliche")]
        Caliche,
        [EnumMember(Value = "CLAY")]
        [Description("Clay")]
        Clay,
        [EnumMember(Value = "GRVL")]
        [Description("Gravel")]
        Gravel,
        [EnumMember(Value = "LMSTN")]
        [Description("Limestone")]
        Limestone,
        [EnumMember(Value = "SANDY")]
        [Description("Sandy Loam")]
        SandyLoam,
        [EnumMember(Value = "SHOAL")]
        [Description("Shoals")]
        Shoals,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
