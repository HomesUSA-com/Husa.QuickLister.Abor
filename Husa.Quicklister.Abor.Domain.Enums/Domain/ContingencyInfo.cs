namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ContingencyInfo
    {
        [EnumMember(Value = "INS")]
        [Description("Inspection")]
        Inspection,
        [EnumMember(Value = "TTL")]
        [Description("Title")]
        Title,
        [EnumMember(Value = "FIN")]
        [Description("Financing")]
        Financing,
        [EnumMember(Value = "RMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "APP")]
        [Description("Appraisal")]
        Appraisal,
    }
}
