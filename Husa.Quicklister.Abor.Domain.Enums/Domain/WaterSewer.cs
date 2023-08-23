namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WaterSewer
    {
        [EnumMember(Value = "AerobicSeptic")]
        [Description("Aerobic Septic")]
        AerobicSeptic,
        [EnumMember(Value = "EngineeredSeptic")]
        [Description("Engineered Septic")]
        EngineeredSeptic,
        [EnumMember(Value = "MUD")]
        [Description("MUD")]
        MUD,
        [EnumMember(Value = "PrivateSewer")]
        [Description("Private Sewer")]
        PrivateSewer,
        [EnumMember(Value = "PublicSewer")]
        [Description("Public Sewer")]
        PublicSewer,
        [EnumMember(Value = "SepticTank")]
        [Description("Septic Tank")]
        SepticTank,
    }
}
