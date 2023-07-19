namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HowSold
    {
        [EnumMember(Value = "1SLLR")]
        [Description("1st Seller")]
        FirstSeller,
        [EnumMember(Value = "2SLLR")]
        [Description("2nd Seller")]
        SecondSeller,
        [EnumMember(Value = "ASSUM")]
        [Description("Assumable")]
        Assumable,
        [EnumMember(Value = "CARRY")]
        [Description("Carry")]
        Carry,
        [EnumMember(Value = "CASH")]
        [Description("Cash")]
        Cash,
        [EnumMember(Value = "CNTRC")]
        [Description("Contract for Deed")]
        ContractForDeed,
        [EnumMember(Value = "CONV")]
        [Description("Conventional")]
        Conventional,
        [EnumMember(Value = "CONVTC")]
        [Description("Conventional/Texas Vet")]
        ConventionalTexasVet,
        [EnumMember(Value = "FHA")]
        [Description("FHA")]
        FHA,
        [EnumMember(Value = "FHATX")]
        [Description("FHA/Texas Vet")]
        FHATexasVet,
        [EnumMember(Value = "OTHER")]
        [Description("Other")]
        Other,
        [EnumMember(Value = "USDA")]
        [Description("USDA")]
        USDA,
        [EnumMember(Value = "VA")]
        [Description("VA")]
        VA,
        [EnumMember(Value = "VATX")]
        [Description("VA/Texas Vet")]
        VATexasVet,
        [EnumMember(Value = "WRAP")]
        [Description("Wrap")]
        Wrap,
    }
}
