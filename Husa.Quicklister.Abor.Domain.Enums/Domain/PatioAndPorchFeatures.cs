namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum PatioAndPorchFeatures
    {
        [EnumMember(Value = "ARBOR")]
        [Description("Arbor")]
        Arbor,
        [EnumMember(Value = "AWNG")]
        [Description("Awning(s)")]
        Awnings,
        [EnumMember(Value = "CVR")]
        [Description("Covered")]
        Covered,
        [EnumMember(Value = "Deck")]
        [Description("Deck")]
        Deck,
        [EnumMember(Value = "ENCLD")]
        [Description("Enclosed")]
        Enclosed,
        [EnumMember(Value = "FPRCH")]
        [Description("Front Porch")]
        FrontPorch,
        [EnumMember(Value = "GLSEN")]
        [Description("Glass Enclosed")]
        GlassEnclosed,
        [EnumMember(Value = "MOSQ")]
        [Description("Mosquito System")]
        MosquitoSystem,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "Patio")]
        [Description("Patio")]
        Patio,
        [EnumMember(Value = "PCH")]
        [Description("Porch")]
        Porch,
        [EnumMember(Value = "RPRCH")]
        [Description("Rear Porch")]
        RearPorch,
        [EnumMember(Value = "SCRNPR")]
        [Description("Screened")]
        Screened,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "SPCH")]
        [Description("Side Porch")]
        SidePorch,
        [EnumMember(Value = "TERR")]
        [Description("Terrace")]
        Terrace,
        [EnumMember(Value = "WRAP")]
        [Description("Wrap Around")]
        WrapAround,
    }
}
