namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum SellerConcessionDescription
    {
        [EnumMember(Value = "NONE")]
        [Description("None")]
        None,
        [EnumMember(Value = "CLOSINGCOSTS")]
        [Description("Closing Costs")]
        ClosingCosts,
        [EnumMember(Value = "HOMEWARRANTY")]
        [Description("Home Warranty")]
        HomeWarranty,
        [EnumMember(Value = "PREPAID")]
        [Description("Prepaid")]
        Prepaid,
        [EnumMember(Value = "REPAIRS")]
        [Description("Repairs")]
        Repairs,
        [EnumMember(Value = "OTHER")]
        [Description("Other - See Agent Remarks")]
        OtherSeeAgentRemarks,
    }
}
