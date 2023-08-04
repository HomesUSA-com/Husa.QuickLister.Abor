namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum PatioAndPorchFeatures
    {
        [EnumMember(Value = "Arbor")]
        [Description("Arbor")]
        Arbor,
        [EnumMember(Value = "Awnings")]
        [Description("Awning(s)")]
        Awnings,
        [EnumMember(Value = "Covered")]
        [Description("Covered")]
        Covered,
        [EnumMember(Value = "Deck")]
        [Description("Deck")]
        Deck,
        [EnumMember(Value = "Enclosed")]
        [Description("Enclosed")]
        Enclosed,
        [EnumMember(Value = "FrontPorch")]
        [Description("Front Porch")]
        FrontPorch,
        [EnumMember(Value = "GlassEnclosed")]
        [Description("Glass Enclosed")]
        GlassEnclosed,
        [EnumMember(Value = "MosquitoSystem")]
        [Description("Mosquito System")]
        MosquitoSystem,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "Patio")]
        [Description("Patio")]
        Patio,
        [EnumMember(Value = "Porch")]
        [Description("Porch")]
        Porch,
        [EnumMember(Value = "RearPorch")]
        [Description("Rear Porch")]
        RearPorch,
        [EnumMember(Value = "Screened")]
        [Description("Screened")]
        Screened,
        [EnumMember(Value = "SeeRemarks")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "SidePorch")]
        [Description("Side Porch")]
        SidePorch,
        [EnumMember(Value = "Terrace")]
        [Description("Terrace")]
        Terrace,
        [EnumMember(Value = "WrapAround")]
        [Description("Wrap Around")]
        WrapAround,
    }
}
