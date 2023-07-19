namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HomeFaces
    {
        [EnumMember(Value = "E")]
        [Description("East")]
        East,
        [EnumMember(Value = "N")]
        [Description("North")]
        North,
        [EnumMember(Value = "S")]
        [Description("South")]
        South,
        [EnumMember(Value = "W")]
        [Description("West")]
        West,
    }
}
