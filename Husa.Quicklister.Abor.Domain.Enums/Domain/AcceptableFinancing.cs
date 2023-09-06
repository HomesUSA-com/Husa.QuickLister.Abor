namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum AcceptableFinancing
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
        [EnumMember(Value = "FMHA")]
        [Description("FMHA (Fannie Mae)")]
        FMHA,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "SALEL")]
        [Description("Lease Back")]
        LeaseBack,
        [EnumMember(Value = "TVET")]
        [Description("Texas Vet")]
        TexasVet,
        [EnumMember(Value = "USDA")]
        [Description("USDA Loan")]
        USDALoan,
        [EnumMember(Value = "VA")]
        [Description("VA Loan")]
        VALoan,
    }
}
