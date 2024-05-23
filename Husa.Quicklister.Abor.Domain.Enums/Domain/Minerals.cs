namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Minerals
    {
        [EnumMember(Value = "MRALL")]
        [Description("All")]
        All,
        [EnumMember(Value = "PART")]
        [Description("Partial")]
        Partial,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
