namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum LotImprovements
    {
        [EnumMember(Value = "STHWY")]
        [Description("State Highway")]
        StateHighway,
        [EnumMember(Value = "STGTR")]
        [Description("Street Gutters")]
        StreetGutters,
        [EnumMember(Value = "SDWLK")]
        [Description("Sidewalks")]
        Sidewalks,
        [EnumMember(Value = "PRIVATERD")]
        [Description("Private Road")]
        PrivateRoad,
        [EnumMember(Value = "PAVED")]
        [Description("Street Paved")]
        StreetPaved,
        [EnumMember(Value = "OTHER")]
        [Description("Other")]
        Other,
        [EnumMember(Value = "FRHYD")]
        [Description("Fire Hydrant w/in 500'")]
        FireHydrantWithinFiveHundred,
        [EnumMember(Value = "DIRT")]
        [Description("Dirt")]
        Dirt,
        [EnumMember(Value = "CURBS")]
        [Description("Curbs")]
        Curbs,
        [EnumMember(Value = "CITYST")]
        [Description("City Street")]
        CityStreet,
        [EnumMember(Value = "ASPHALT")]
        [Description("Asphalt")]
        Asphalt,
        [EnumMember(Value = "STLGT")]
        [Description("Streetlights")]
        Streetlights,
        [EnumMember(Value = "INTERSTHWY")]
        [Description("Interstate Hwy - 1 Mile or less")]
        InterstateHighwayOneMileOrLess,
    }
}
