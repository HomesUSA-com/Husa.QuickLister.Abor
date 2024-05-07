namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum LandTitleEvidence
    {
        [EnumMember(Value = "BUY")]
        [Description("Buyer")]
        Buyer,
        [EnumMember(Value = "NEGO")]
        [Description("Negotiable")]
        Negotiable,
        [EnumMember(Value = "SELL")]
        [Description("Seller")]
        Seller,
    }
}
