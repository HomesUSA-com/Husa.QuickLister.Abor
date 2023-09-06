namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum KitchenFeatures
    {
        [EnumMember(Value = "BRAREA")]
        [Description("Breakfast Area")]
        BreakfastArea,
        [EnumMember(Value = "BRFAR")]
        [Description("Breakfast Bar")]
        BreakfastBar,
        [EnumMember(Value = "BPANT")]
        [Description("Butler Pantry")]
        ButlerPantry,
        [EnumMember(Value = "CNISL")]
        [Description("Center Island")]
        CenterIsland,
        [EnumMember(Value = "CORTP")]
        [Description("Corian Counters")]
        CorianCounters,
        [EnumMember(Value = "DNAR")]
        [Description("Dining Area")]
        DiningArea,
        [EnumMember(Value = "EIKT")]
        [Description("Eat In Kitchen")]
        EatInKitchen,
        [EnumMember(Value = "GRMKT")]
        [Description("Gourmet Kitchen")]
        GourmetKitchen,
        [EnumMember(Value = "GranCnt")]
        [Description("Granite Counters")]
        GraniteCounters,
        [EnumMember(Value = "OFMRM")]
        [Description("Open to Family Room")]
        OpentoFamilyRoom,
        [EnumMember(Value = "KPNCL")]
        [Description("Pantry")]
        Pantry,
        [EnumMember(Value = "PLMIC")]
        [Description("Plumbed for Icemaker")]
        PlumbedforIcemaker,
        [EnumMember(Value = "QURT")]
        [Description("Quartz Counters")]
        QuartzCounters,
        [EnumMember(Value = "RCSS")]
        [Description("Recessed Lighting")]
        RecessedLighting,
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
