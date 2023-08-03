namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum TaxExemptions
    {
        [EnumMember(Value = "AGCLT")]
        [Description("Agricultural")]
        Agricultural,
        [EnumMember(Value = "DSBLY")]
        [Description("Disability")]
        Disability,
        [EnumMember(Value = "HIST")]
        [Description("Historical")]
        Historical,
        [EnumMember(Value = "HMSTD")]
        [Description("Homestead")]
        Homestead,
        [EnumMember(Value = "NONE")]
        [Description("None")]
        None,
        [EnumMember(Value = "OV65E")]
        [Description("Over 65")]
        Over65,
        [EnumMember(Value = "WLDLF")]
        [Description("Wildlife")]
        Wildlife,
    }
}
