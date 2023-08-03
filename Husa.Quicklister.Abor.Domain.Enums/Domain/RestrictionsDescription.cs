namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RestrictionsDescription
    {
        [EnumMember(Value = "Adult 55+")]
        [Description("Adult 55+")]
        Adult55,
        [EnumMember(Value = "Adult 62+")]
        [Description("Adult 62+")]
        Adult62,
        [EnumMember(Value = "Building Size")]
        [Description("Building Size")]
        BuildingSize,
        [EnumMember(Value = "Building Style")]
        [Description("Building Style")]
        BuildingStyle,
        [EnumMember(Value = "City Restrictions")]
        [Description("City Restrictions")]
        CityRestrictions,
        [EnumMember(Value = "Covenant")]
        [Description("Covenant")]
        Covenant,
        [EnumMember(Value = "Deed Restrictions")]
        [Description("Deed Restrictions")]
        DeedRestrictions,
        [EnumMember(Value = "DevelopmentType")]
        [Description("Development Type")]
        DevelopmentType,
        [EnumMember(Value = "Easement/R.O.W.")]
        [Description("Easement/R.O.W.")]
        EasementROW,
        [EnumMember(Value = "Environmental")]
        [Description("Environmental")]
        Environmental,
        [EnumMember(Value = "Lease")]
        [Description("Lease")]
        Lease,
        [EnumMember(Value = "Limited # Vehicles")]
        [Description("Limited # Vehicles")]
        LimitedVehicles,
        [EnumMember(Value = "Livestock")]
        [Description("Livestock")]
        Livestock,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "Seller Imposed")]
        [Description("Seller Imposed")]
        SellerImposed,
        [EnumMember(Value = "Zoning")]
        [Description("Zoning")]
        Zoning,
    }
}
