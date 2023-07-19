namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HoaRequirement
    {
        [EnumMember(Value = "MAND")]
        [Description("Mandatory")]
        Mandatory,
        [EnumMember(Value = "VOLNT")]
        [Description("Voluntary")]
        Voluntary,
        [EnumMember(Value = "NONE")]
        [Description("None")]
        None,
    }
}
