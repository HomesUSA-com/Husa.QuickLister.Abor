namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ContingencyInfo
    {
        [EnumMember(Value = "INSPEC")]
        [Description("Inspection")]
        Inspection,
        [EnumMember(Value = "TITLE")]
        [Description("Title")]
        Title,
        [EnumMember(Value = "FINANC")]
        [Description("Financing")]
        Financing,
        [EnumMember(Value = "SEEREM")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "APPRA")]
        [Description("Appraisal")]
        Appraisal,
    }
}
