namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Foundation
    {
        [EnumMember(Value = "PillarPostPier")]
        [Description("Pillar/Post/Pier")]
        PillarPostPier,
        [EnumMember(Value = "Slab")]
        [Description("Slab")]
        Slab,
    }
}
