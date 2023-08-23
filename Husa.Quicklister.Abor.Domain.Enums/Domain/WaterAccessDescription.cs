namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WaterAccessDescription
    {
        [EnumMember(Value = "CorianCounters")]
        [Description("Corian Counters")]
        CorianCounters,
        [EnumMember(Value = "DoubleVanity")]
        [Description("Double Vanity")]
        DoubleVanity,
        [EnumMember(Value = "FullBath")]
        [Description("Full Bath")]
        FullBath,
        [EnumMember(Value = "GardenTubRomanTub")]
        [Description("Garden Tub")]
        GardenTub,
        [EnumMember(Value = "GraniteCounters")]
        [Description("Granite Counters")]
        GraniteCounters,
        [EnumMember(Value = "JettedTub")]
        [Description("Jetted Tub")]
        JettedTub,
        [EnumMember(Value = "LowFlowPlumbingFixtures")]
        [Description("Low Flow Plumbing Fixtures")]
        LowFlowPlumbingFixtures,
        [EnumMember(Value = "QuartzCounters")]
        [Description("Quartz Counters")]
        QuartzCounters,
        [EnumMember(Value = "SeparateShower")]
        [Description("Separate Shower")]
        SeparateShower,
        [EnumMember(Value = "StoneCounters")]
        [Description("Stone Counters")]
        StoneCounters,
        [EnumMember(Value = "TileCounters")]
        [Description("Tile Counters")]
        TileCounters,
        [EnumMember(Value = "TrayCeilings")]
        [Description("Tray Ceiling(s)")]
        TrayCeilings,
        [EnumMember(Value = "VaultedCeilings")]
        [Description("Vaulted Ceiling(s)")]
        VaultedCeilings,
    }
}
