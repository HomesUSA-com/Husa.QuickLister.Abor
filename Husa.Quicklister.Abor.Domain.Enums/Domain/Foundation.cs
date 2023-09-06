namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Foundation
    {
        [EnumMember(Value = "PIER")]
        [Description("Pillar/Post/Pier")]
        PillarPostPier,
        [EnumMember(Value = "SLAB")]
        [Description("Slab")]
        Slab,
    }
}
