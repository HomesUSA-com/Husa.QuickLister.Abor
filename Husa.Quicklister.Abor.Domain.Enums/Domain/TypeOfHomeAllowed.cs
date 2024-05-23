namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum TypeOfHomeAllowed
    {
        [EnumMember(Value = "HMAPR")]
        [Description("Approval Required")]
        ApprovalRequired,
        [EnumMember(Value = "MANUF")]
        [Description("Manufactured")]
        Manufactured,
        [EnumMember(Value = "HMMBL")]
        [Description("Mobile")]
        Mobile,
        [EnumMember(Value = "MODU")]
        [Description("Modular")]
        Modular,
        [EnumMember(Value = "HMSTB")]
        [Description("Site Built")]
        SiteBuilt,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
