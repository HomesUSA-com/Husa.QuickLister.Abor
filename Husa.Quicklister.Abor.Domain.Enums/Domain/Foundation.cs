namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Foundation
    {
        [EnumMember(Value = "BSMNT")]
        [Description("Basement")]
        Basement,
        [EnumMember(Value = "OTHER")]
        [Description("Other")]
        Other,
        [EnumMember(Value = "SLAB")]
        [Description("Slab")]
        Slab,
    }
}
