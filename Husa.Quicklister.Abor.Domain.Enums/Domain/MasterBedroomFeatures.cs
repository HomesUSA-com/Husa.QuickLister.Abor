namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum MasterBedroomFeatures
    {
        [EnumMember(Value = "CeilingFans")]
        [Description("Ceiling Fan(s)")]
        CeilingFans,
        [EnumMember(Value = "CrownMolding")]
        [Description("Crown Molding")]
        CrownMolding,
        [EnumMember(Value = "HighCeilings")]
        [Description("High Ceilings")]
        HighCeilings,
        [EnumMember(Value = "HisHerClosets")]
        [Description("His Her Closets")]
        HisHerClosets,
        [EnumMember(Value = "PrimaryBedroomSittingStudyRoom")]
        [Description("Primary Bedroom Sitting/Study Room")]
        PrimaryBedroomSittingStudyRoom,
        [EnumMember(Value = "RecessedLighting")]
        [Description("Recessed Lighting")]
        RecessedLighting,
        [EnumMember(Value = "TrayCeilings")]
        [Description("Tray Ceiling(s)")]
        TrayCeilings,
        [EnumMember(Value = "VaultedCeilings")]
        [Description("Vaulted Ceiling(s)")]
        VaultedCeilings,
        [EnumMember(Value = "WalkInClosets")]
        [Description("Walk-In Closet(s)")]
        WalkInClosets,
        [EnumMember(Value = "WiredforData")]
        [Description("Wired for Data")]
        WiredforData,
        [EnumMember(Value = "WiredforSound")]
        [Description("Wired for Sound")]
        WiredforSound,
    }
}
