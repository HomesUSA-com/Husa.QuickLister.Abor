namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum PropertySubType
    {
        [EnumMember(Value = "CONAT")]
        [Description("Condominium")]
        Condominium,
        [EnumMember(Value = "HOUSE")]
        [Description("Single Family Residence")]
        SingleFamilyResidence,
        [EnumMember(Value = "TN")]
        [Description("Townhouse")]
        Townhouse,
    }
}
