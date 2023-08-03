namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WindowFeatures
    {
        [EnumMember(Value = "BayWindows")]
        [Description("Bay Window(s)")]
        BayWindows,
        [EnumMember(Value = "Blinds")]
        [Description("Blinds")]
        Blinds,
        [EnumMember(Value = "DoublePaneWindows")]
        [Description("Double Pane Windows")]
        DoublePaneWindows,
        [EnumMember(Value = "ENERGYSTARQualifiedWindows")]
        [Description("ENERGYSTAR Qualified Windows")]
        EnergyStarQualifiedWindows,
        [EnumMember(Value = "InsulatedWindows")]
        [Description("Insulated Windows")]
        InsulatedWindows,
        [EnumMember(Value = "none")]
        [Description("None")]
        None,
        [EnumMember(Value = "PlantationShutters")]
        [Description("Plantation Shutters")]
        PlantationShutters,
        [EnumMember(Value = "Screens")]
        [Description("Screens")]
        Screens,
        [EnumMember(Value = "Shutters ")]
        [Description("Shutters")]
        Shutters,
        [EnumMember(Value = "StormWindows")]
        [Description("Storm Windows")]
        StormWindows,
        [EnumMember(Value = "Vinyls")]
        [Description("Vinyl Windows")]
        VinylWindows,
    }
}
