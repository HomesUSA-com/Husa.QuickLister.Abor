namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum InteriorFeatures
    {
        [EnumMember(Value = "R2MBT")]
        [Description("2 Primary Bath")]
        TwoPrimaryBath,
        [EnumMember(Value = "R2MST")]
        [Description("2 Primary Suites")]
        TwoPrimarySuites,
        [EnumMember(Value = "BAR")]
        [Description("Bar")]
        Bar,
        [EnumMember(Value = "BBKCA")]
        [Description("Bookcases")]
        Bookcases,
        [EnumMember(Value = "BRFAR")]
        [Description("Breakfast Bar")]
        BreakfastBar,
        [EnumMember(Value = "BIFEAT")]
        [Description("Built-in Features")]
        BuiltinFeatures,
        [EnumMember(Value = "CFAN")]
        [Description("Ceiling Fan(s)")]
        CeilingFans,
        [EnumMember(Value = "CLBM")]
        [Description("Ceiling(s)-Beamed")]
        CeilingsBeamed,
        [EnumMember(Value = "CLCTH")]
        [Description("Ceiling(s)-Cathedral")]
        CeilingsCathedral,
        [EnumMember(Value = "CLCFF")]
        [Description("Ceiling(s)-Coffered")]
        CeilingsCoffered,
        [EnumMember(Value = "HCLG")]
        [Description("Ceiling(s)-High")]
        CeilingsHigh,
        [EnumMember(Value = "TRAY")]
        [Description("Ceiling(s)-Tray")]
        CeilingsTray,
        [EnumMember(Value = "CLVLT")]
        [Description("Ceiling(s)-Vaulted")]
        CeilingsVaulted,
        [EnumMember(Value = "CHNDLR")]
        [Description("Chandelier")]
        Chandelier,
        [EnumMember(Value = "CRMLD")]
        [Description("Crown Molding")]
        CrownMolding,
        [EnumMember(Value = "DBVAN")]
        [Description("Double Vanity")]
        DoubleVanity,
        [EnumMember(Value = "ELCCN")]
        [Description("Dryer-Electric Hookup")]
        DryerElectricHookup,
        [EnumMember(Value = "GASCN")]
        [Description("Dryer-Gas Hookup")]
        DryerGasHookup,
        [EnumMember(Value = "EIKT")]
        [Description("Eat-in Kitchen")]
        EatinKitchen,
        [EnumMember(Value = "ENFOYR")]
        [Description("Entrance Foyer")]
        EntranceFoyer,
        [EnumMember(Value = "FRNDR")]
        [Description("French Doors")]
        FrenchDoors,
        [EnumMember(Value = "HSPCN")]
        [Description("High Speed Internet")]
        HighSpeedInternet,
        [EnumMember(Value = "IILPL")]
        [Description("In-Law Floorplan")]
        InLawFloorplan,
        [EnumMember(Value = "INTER")]
        [Description("Interior Steps")]
        InteriorSteps,
        [EnumMember(Value = "KITIS")]
        [Description("Kitchen Island")]
        KitchenIsland,
        [EnumMember(Value = "PLFX")]
        [Description("Low Flow Plumbing Fixtures")]
        LowFlowPlumbingFixtures,
        [EnumMember(Value = "MDIN")]
        [Description("Multiple Dining Areas")]
        MultipleDiningAreas,
        [EnumMember(Value = "MLIV")]
        [Description("Multiple Living Areas")]
        MultipleLivingAreas,
        [EnumMember(Value = "NWWK")]
        [Description("Natural Woodwork")]
        NaturalWoodwork,
        [EnumMember(Value = "NOINT")]
        [Description("No Interior Steps")]
        NoInteriorSteps,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "Open")]
        [Description("Open Floorplan")]
        OpenFloorplan,
        [EnumMember(Value = "PNTY")]
        [Description("Pantry")]
        Pantry,
        [EnumMember(Value = "MSTDW")]
        [Description("Primary Bedroom on Main")]
        PrimaryBedroomonMain,
        [EnumMember(Value = "LRECE")]
        [Description("Recessed Lighting")]
        RecessedLighting,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "SMHM")]
        [Description("Smart Home")]
        SmartHome,
        [EnumMember(Value = "SMTH")]
        [Description("Smart Thermostat")]
        SmartThermostat,
        [EnumMember(Value = "SOAK")]
        [Description("Soaking Tub")]
        SoakingTub,
        [EnumMember(Value = "STUB")]
        [Description("Solar Tube(s)")]
        SolarTubes,
        [EnumMember(Value = "SSYS")]
        [Description("Sound System")]
        SoundSystem,
        [EnumMember(Value = "STRG")]
        [Description("Storage")]
        Storage,
        [EnumMember(Value = "TKLGH")]
        [Description("Track Lighting")]
        TrackLighting,
        [EnumMember(Value = "RWINC")]
        [Description("Walk-In Closet(s)")]
        WalkInClosets,
        [EnumMember(Value = "WSHCN")]
        [Description("Washer Hookup")]
        WasherHookup,
        [EnumMember(Value = "WSNFX")]
        [Description("WaterSense Fixture(s)")]
        WaterSenseFixtures,
        [EnumMember(Value = "WDTA")]
        [Description("Wired for Data")]
        WiredforData,
        [EnumMember(Value = "WRSSD")]
        [Description("Wired for Sound")]
        WiredforSound,
    }
}
