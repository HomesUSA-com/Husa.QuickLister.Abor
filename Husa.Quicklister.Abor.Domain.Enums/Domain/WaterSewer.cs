namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WaterSewer
    {
        [EnumMember(Value = "ASEP")]
        [Description("Aerobic Septic")]
        AerobicSeptic,
        [EnumMember(Value = "ENGSPT")]
        [Description("Engineered Septic")]
        EngineeredSeptic,
        [EnumMember(Value = "MUD")]
        [Description("MUD")]
        MUD,
        [EnumMember(Value = "PSWR")]
        [Description("Private Sewer")]
        PrivateSewer,
        [EnumMember(Value = "PBSWR")]
        [Description("Public Sewer")]
        PublicSewer,
        [EnumMember(Value = "SPTNT")]
        [Description("Septic Tank")]
        SepticTank,
    }
}
