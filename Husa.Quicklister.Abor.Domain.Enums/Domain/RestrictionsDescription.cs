namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RestrictionsDescription
    {
        [EnumMember(Value = "ADLT55")]
        [Description("Adult 55+")]
        Adult55,
        [EnumMember(Value = "ADLT62")]
        [Description("Adult 62+")]
        Adult62,
        [EnumMember(Value = "BLDGSZ")]
        [Description("Building Size")]
        BuildingSize,
        [EnumMember(Value = "BLDGST")]
        [Description("Building Style")]
        BuildingStyle,
        [EnumMember(Value = "CITRS")]
        [Description("City Restrictions")]
        CityRestrictions,
        [EnumMember(Value = "CVNANT")]
        [Description("Covenant")]
        Covenant,
        [EnumMember(Value = "DEEDRS")]
        [Description("Deed Restrictions")]
        DeedRestrictions,
        [EnumMember(Value = "DVLMPT")]
        [Description("Development Type")]
        DevelopmentType,
        [EnumMember(Value = "EASE")]
        [Description("Easement")]
        EasementROW,
        [EnumMember(Value = "ENVRO")]
        [Description("Environmental")]
        Environmental,
        [EnumMember(Value = "LEASE")]
        [Description("Lease")]
        Lease,
        [EnumMember(Value = "LMTDVH")]
        [Description("Limited # Vehicles")]
        LimitedVehicles,
        [EnumMember(Value = "LVSTCK")]
        [Description("Livestock")]
        Livestock,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "SIMP")]
        [Description("Seller Imposed")]
        SellerImposed,
        [EnumMember(Value = "ZONE")]
        [Description("Zoning")]
        Zoning,
    }
}
