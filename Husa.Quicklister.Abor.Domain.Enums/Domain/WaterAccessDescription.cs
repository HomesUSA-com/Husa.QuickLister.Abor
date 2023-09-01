namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WaterAccessDescription
    {
        [EnumMember(Value = "CORTP")]
        [Description("Corian Counters")]
        CorianCounters,
        [EnumMember(Value = "DBVAN")]
        [Description("Double Vanity")]
        DoubleVanity,
        [EnumMember(Value = "RFBTH")]
        [Description("Full Bath")]
        FullBath,
        [EnumMember(Value = "FRGDN")]
        [Description("Garden Tub")]
        GardenTub,
        [EnumMember(Value = "GranCnt")]
        [Description("Granite Counters")]
        GraniteCounters,
        [EnumMember(Value = "RJTUB")]
        [Description("Jetted Tub")]
        JettedTub,
        [EnumMember(Value = "LFPUM")]
        [Description("Low Flow Plumbing Fixtures")]
        LowFlowPlumbingFixtures,
        [EnumMember(Value = "QURT")]
        [Description("Quartz Counters")]
        QuartzCounters,
        [EnumMember(Value = "RSSWR")]
        [Description("Separate Shower")]
        SeparateShower,
        [EnumMember(Value = "STNCNT")]
        [Description("Stone Counters")]
        StoneCounters,
        [EnumMember(Value = "TLECNT")]
        [Description("Tile Counters")]
        TileCounters,
        [EnumMember(Value = "TRYC")]
        [Description("Tray Ceiling(s)")]
        TrayCeilings,
        [EnumMember(Value = "VULC")]
        [Description("Vaulted Ceiling(s)")]
        VaultedCeilings,
    }
}
