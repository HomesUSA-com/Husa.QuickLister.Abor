namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum LockBoxType
    {
        [EnumMember(Value = "BOTH")]
        [Description("Both")]
        Both,
        [EnumMember(Value = "COMBO")]
        [Description("Combo")]
        Combo,
        [EnumMember(Value = "LBMLS")]
        [Description("SUPRA")]
        Supra,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
