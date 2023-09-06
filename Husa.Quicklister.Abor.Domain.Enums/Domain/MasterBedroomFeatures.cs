namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum MasterBedroomFeatures
    {
        [EnumMember(Value = "FAN")]
        [Description("Ceiling Fan(s)")]
        CeilingFans,
        [EnumMember(Value = "CRWN")]
        [Description("Crown Molding")]
        CrownMolding,
        [EnumMember(Value = "HGHC")]
        [Description("High Ceilings")]
        HighCeilings,
        [EnumMember(Value = "R2CLO")]
        [Description("His Her Closets")]
        HisHerClosets,
        [EnumMember(Value = "RMSIT")]
        [Description("Primary Bedroom Sitting/Study Room")]
        PrimaryBedroomSittingStudyRoom,
        [EnumMember(Value = "RCSS")]
        [Description("Recessed Lighting")]
        RecessedLighting,
        [EnumMember(Value = "TRYC")]
        [Description("Tray Ceiling(s)")]
        TrayCeilings,
        [EnumMember(Value = "VULC")]
        [Description("Vaulted Ceiling(s)")]
        VaultedCeilings,
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
