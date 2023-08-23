namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum BillingFrequency
    {
        [EnumMember(Value = "A")]
        [Description("Annually")]
        Annually,
        [EnumMember(Value = "MN")]
        [Description("Monthly")]
        Monthly,
        [EnumMember(Value = "Q")]
        [Description("Quarterly")]
        Quarterly,
        [EnumMember(Value = "SEMIA")]
        [Description("Semi-Annually")]
        SemiAnnually,
    }
}
