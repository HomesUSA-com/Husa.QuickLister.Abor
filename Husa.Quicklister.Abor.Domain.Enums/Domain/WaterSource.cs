namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WaterSource
    {
        [EnumMember(Value = "MUD")]
        [Description("MUD")]
        MUD,
        [EnumMember(Value = "PRV")]
        [Description("Private")]
        Private,
        [EnumMember(Value = "CITY")]
        [Description("Public")]
        Public,
    }
}
