namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WindowFeatures
    {
        [EnumMember(Value = "BWND")]
        [Description("Bay Window(s)")]
        BayWindows,
        [EnumMember(Value = "Blinds")]
        [Description("Blinds")]
        Blinds,
        [EnumMember(Value = "DPWND")]
        [Description("Double Pane Windows")]
        DoublePaneWindows,
        [EnumMember(Value = "EQWIN")]
        [Description("ENERGYSTAR Qualified Windows")]
        EnergyStarQualifiedWindows,
        [EnumMember(Value = "INWND")]
        [Description("Insulated Windows")]
        InsulatedWindows,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "PSHUT")]
        [Description("Plantation Shutters")]
        PlantationShutters,
        [EnumMember(Value = "SCR")]
        [Description("Screens")]
        Screens,
        [EnumMember(Value = "SHTRS")]
        [Description("Shutters")]
        Shutters,
        [EnumMember(Value = "STWND")]
        [Description("Storm Windows")]
        StormWindows,
        [EnumMember(Value = "VWNDW")]
        [Description("Vinyl Windows")]
        VinylWindows,
    }
}
