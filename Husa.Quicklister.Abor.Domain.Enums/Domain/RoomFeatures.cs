namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RoomFeatures
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
        [EnumMember(Value = "FAN")]
        [Description("Ceiling Fan(s)")]
        CeilingFans,
        [EnumMember(Value = "CORTP")]
        [Description("Corian Counters")]
        CorianCounters,
        [EnumMember(Value = "CRWN")]
        [Description("Crown Molding")]
        CrownMolding,
        [EnumMember(Value = "DNAR")]
        [Description("Dining Area")]
        DiningArea,
        [EnumMember(Value = "DBVAN")]
        [Description("Double Vanity")]
        DoubleVanity,
        [EnumMember(Value = "EIKT")]
        [Description("Eat In Kitchen")]
        EatInKitchen,
        [EnumMember(Value = "ELCCN")]
        [Description("Electric Dryer Hookup")]
        ElectricDryerHookup,
        [EnumMember(Value = "RFBTH")]
        [Description("Full Bath")]
        FullBath,
        [EnumMember(Value = "GASCN")]
        [Description("Gas Dryer Hookup")]
        GasDryerHookup,
        [EnumMember(Value = "FRGDN")]
        [Description("Garden Tub")]
        GardenTub,
        [EnumMember(Value = "GRMKT")]
        [Description("Gourmet Kitchen")]
        GourmetKitchen,
        [EnumMember(Value = "GranCnt")]
        [Description("Granite Counters")]
        GraniteCounters,
        [EnumMember(Value = "HGHC")]
        [Description("High Ceilings")]
        HighCeilings,
        [EnumMember(Value = "R2CLO")]
        [Description("His Her Closets")]
        HisHerClosets,
        [EnumMember(Value = "RJTUB")]
        [Description("Jetted Tub")]
        JettedTub,
        [EnumMember(Value = "CHTE")]
        [Description("Laundry Chute")]
        LaundryChute,
        [EnumMember(Value = "SINK")]
        [Description("Laundry Sink")]
        LaundrySink,
        [EnumMember(Value = "LFPUM")]
        [Description("Low Flow Plumbing Fixtures")]
        LowFlowPlumbingFixtures,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "OFMRM")]
        [Description("Open to Family Room")]
        OpentoFamilyRoom,
        [EnumMember(Value = "KPNCL")]
        [Description("Pantry")]
        Pantry,
        [EnumMember(Value = "PLMIC")]
        [Description("Plumbed for Icemaker")]
        PlumbedforIcemaker,
        [EnumMember(Value = "RMSIT")]
        [Description("Primary Bedroom Sitting/Study Room")]
        PrimaryBedroomSittingStudyRoom,
        [EnumMember(Value = "QURT")]
        [Description("Quartz Counters")]
        QuartzCounters,
        [EnumMember(Value = "RCSS")]
        [Description("Recessed Lighting")]
        RecessedLighting,
        [EnumMember(Value = "RSSWR")]
        [Description("Separate Shower")]
        SeparateShower,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "STCON")]
        [Description("Stackable W/D Connections")]
        StackableConnections,
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
        [EnumMember(Value = "WSHCN")]
        [Description("Washer Hookup")]
        WasherHookup,
        [EnumMember(Value = "RWINC")]
        [Description("Walk-In Closet(s)")]
        WalkInClosets,
        [EnumMember(Value = "WDT")]
        [Description("Wired for Data")]
        WiredforData,
        [EnumMember(Value = "WSUND")]
        [Description("Wired for Sound")]
        WiredforSound,
    }
}
