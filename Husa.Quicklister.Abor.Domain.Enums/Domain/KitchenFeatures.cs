namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum KitchenFeatures
    {
        [EnumMember(Value = "BreakfastArea")]
        [Description("Breakfast Area")]
        BreakfastArea,
        [EnumMember(Value = "BreakfastBar")]
        [Description("Breakfast Bar")]
        BreakfastBar,
        [EnumMember(Value = "ButlerPantry")]
        [Description("Butler Pantry")]
        ButlerPantry,
        [EnumMember(Value = "KitchenIsland")]
        [Description("Center Island")]
        CenterIsland,
        [EnumMember(Value = "CorianCounters")]
        [Description("Corian Counters")]
        CorianCounters,
        [EnumMember(Value = "DiningArea")]
        [Description("Dining Area")]
        DiningArea,
        [EnumMember(Value = "EatInKitchen")]
        [Description("Eat In Kitchen")]
        EatInKitchen,
        [EnumMember(Value = "GourmetKitchen")]
        [Description("Gourmet Kitchen")]
        GourmetKitchen,
        [EnumMember(Value = "GraniteCounters")]
        [Description("Granite Counters")]
        GraniteCounters,
        [EnumMember(Value = "OpentoFamilyRoom")]
        [Description("Open to Family Room")]
        OpentoFamilyRoom,
        [EnumMember(Value = "Pantry")]
        [Description("Pantry")]
        Pantry,
        [EnumMember(Value = "PlumbedforIcemaker")]
        [Description("Plumbed for Icemaker")]
        PlumbedforIcemaker,
        [EnumMember(Value = "QuartzCounters")]
        [Description("Quartz Counters")]
        QuartzCounters,
        [EnumMember(Value = "RecessedLighting")]
        [Description("Recessed Lighting")]
        RecessedLighting,
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
