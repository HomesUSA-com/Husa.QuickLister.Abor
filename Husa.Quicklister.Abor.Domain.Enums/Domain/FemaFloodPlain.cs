namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum FemaFloodPlain
    {
        [EnumMember(Value = "N")]
        [Description("No")]
        No,
        [EnumMember(Value = "P")]
        [Description("Partial")]
        Partial,
        [EnumMember(Value = "Y")]
        [Description("Yes-100 yr")]
        Yes100yr,
        [EnumMember(Value = "Y500")]
        [Description("Yes-500 yr")]
        Yes500yr,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
