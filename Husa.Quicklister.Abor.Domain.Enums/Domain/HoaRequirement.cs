namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HoaRequirement
    {
        [EnumMember(Value = "M")]
        [Description("Mandatory")]
        Mandatory,
        [EnumMember(Value = "V")]
        [Description("Voluntary")]
        Voluntary,
    }
}
