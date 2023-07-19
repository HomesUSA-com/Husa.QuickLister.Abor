namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ProposedTerms
    {
        [EnumMember(Value = "CASH")]
        [Description("Cash")]
        Cash,
        [EnumMember(Value = "CONV")]
        [Description("Conventional")]
        Conventional,
        [EnumMember(Value = "FHA")]
        [Description("FHA")]
        FHA,
        [EnumMember(Value = "INVOK")]
        [Description("Investors OK")]
        InvestorsOK,
        [EnumMember(Value = "OTHER")]
        [Description("Other")]
        Other,
        [EnumMember(Value = "TXVET")]
        [Description("TX Vet")]
        TxVet,
        [EnumMember(Value = "USDA")]
        [Description("USDA")]
        USDA,
        [EnumMember(Value = "VA")]
        [Description("VA")]
        VA,
    }
}
