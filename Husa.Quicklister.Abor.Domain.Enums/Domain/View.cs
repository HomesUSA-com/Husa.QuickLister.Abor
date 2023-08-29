namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum View
    {
        [EnumMember(Value = "BRDGE")]
        [Description("Bridge(s)")]
        Bridges,
        [EnumMember(Value = "CNAL")]
        [Description("Canal")]
        Canal,
        [EnumMember(Value = "CNY")]
        [Description("Canyon")]
        Canyon,
        [EnumMember(Value = "CITY")]
        [Description("City")]
        City,
        [EnumMember(Value = "CityLt")]
        [Description("City Lights")]
        CityLights,
        [EnumMember(Value = "CREEK")]
        [Description("Creek/Stream")]
        CreekStream,
        [EnumMember(Value = "DTWN")]
        [Description("Downtown")]
        Downtown,
        [EnumMember(Value = "Garden")]
        [Description("Garden")]
        Garden,
        [EnumMember(Value = "GOLFCRSE")]
        [Description("Golf Course")]
        GolfCourse,
        [EnumMember(Value = "HILCNTRY")]
        [Description("Hill Country")]
        HillCountry,
        [EnumMember(Value = "LKE")]
        [Description("Lake")]
        Lake,
        [EnumMember(Value = "MRNA")]
        [Description("Marina")]
        Marina,
        [EnumMember(Value = "NEIHD")]
        [Description("Neighborhood")]
        Neighborhood,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "ORCHD")]
        [Description("Orchard")]
        Orchard,
        [EnumMember(Value = "PANOR")]
        [Description("Panoramic")]
        Panoramic,
        [EnumMember(Value = "GRENBELT")]
        [Description("Park/Greenbelt")]
        ParkGreenbelt,
        [EnumMember(Value = "PSTRE")]
        [Description("Pasture")]
        Pasture,
        [EnumMember(Value = "PD")]
        [Description("Pond")]
        Pond,
        [EnumMember(Value = "PL")]
        [Description("Pool")]
        Pool,
        [EnumMember(Value = "River")]
        [Description("River")]
        River,
        [EnumMember(Value = "Rural")]
        [Description("Rural")]
        Rural,
        [EnumMember(Value = "SKYLN")]
        [Description("Skyline")]
        Skyline,
        [EnumMember(Value = "WOODS")]
        [Description("Trees/Woods")]
        TreesWoods,
        [EnumMember(Value = "VNYD")]
        [Description("Vineyard")]
        Vineyard,
        [EnumMember(Value = "WTR")]
        [Description("Water")]
        Water,
    }
}
