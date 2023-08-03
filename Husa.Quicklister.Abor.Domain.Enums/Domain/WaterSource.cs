namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WaterSource
    {
        [EnumMember(Value = "MUD")]
        [Description("MUD")]
        MUD,
        [EnumMember(Value = "Private")]
        [Description("Private")]
        Private,
        [EnumMember(Value = "Public")]
        [Description("Public")]
        Public,
    }
}
