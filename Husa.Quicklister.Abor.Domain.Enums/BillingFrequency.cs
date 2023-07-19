namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum BillingFrequency
    {
        [EnumMember(Value = "A")]
        [Description("Annually")]
        Annually = 0,
        [EnumMember(Value = "Q")]
        [Description("Quarterly")]
        Quarterly = 1,
        [EnumMember(Value = "M")]
        [Description("Monthly")]
        Monthly = 2,
        [EnumMember(Value = "S")]
        [Description("Semi-Annually")]
        SemiAnnually = 3,
    }
}
