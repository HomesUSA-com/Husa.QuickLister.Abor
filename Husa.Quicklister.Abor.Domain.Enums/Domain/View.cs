namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum View
    {
        [EnumMember(Value = "Bridges")]
        [Description("Bridge(s)")]
        Bridges,
        [EnumMember(Value = "Canal")]
        [Description("Canal")]
        Canal,
        [EnumMember(Value = "Canyon")]
        [Description("Canyon")]
        Canyon,
        [EnumMember(Value = "City")]
        [Description("City")]
        City,
        [EnumMember(Value = "CityLights")]
        [Description("City Lights")]
        CityLights,
        [EnumMember(Value = "CreekStream")]
        [Description("Creek/Stream")]
        CreekStream,
        [EnumMember(Value = "Downtown")]
        [Description("Downtown")]
        Downtown,
        [EnumMember(Value = "Garden")]
        [Description("Garden")]
        Garden,
        [EnumMember(Value = "GolfCourse")]
        [Description("Golf Course")]
        GolfCourse,
        [EnumMember(Value = "Hills")]
        [Description("Hill Country")]
        HillCountry,
        [EnumMember(Value = "Lake")]
        [Description("Lake")]
        Lake,
        [EnumMember(Value = "Marina")]
        [Description("Marina")]
        Marina,
        [EnumMember(Value = "Neighborhood")]
        [Description("Neighborhood")]
        Neighborhood,
        [EnumMember(Value = "NONE")]
        [Description("None")]
        None,
        [EnumMember(Value = "Orchard")]
        [Description("Orchard")]
        Orchard,
        [EnumMember(Value = "Panoramic")]
        [Description("Panoramic")]
        Panoramic,
        [EnumMember(Value = "ParkGreenbelt")]
        [Description("Park/Greenbelt")]
        ParkGreenbelt,
        [EnumMember(Value = "Pasture")]
        [Description("Pasture")]
        Pasture,
        [EnumMember(Value = "Pond")]
        [Description("Pond")]
        Pond,
        [EnumMember(Value = "Pool")]
        [Description("Pool")]
        Pool,
        [EnumMember(Value = "River")]
        [Description("River")]
        River,
        [EnumMember(Value = "Rural")]
        [Description("Rural")]
        Rural,
        [EnumMember(Value = "Skyline")]
        [Description("Skyline")]
        Skyline,
        [EnumMember(Value = "TreesWoods")]
        [Description("Trees/Woods")]
        TreesWoods,
        [EnumMember(Value = "Vineyard")]
        [Description("Vineyard")]
        Vineyard,
        [EnumMember(Value = "Water")]
        [Description("Water")]
        Water,
    }
}
