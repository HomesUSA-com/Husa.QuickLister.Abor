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
        [EnumMember(Value = "NE")]
        [Description("North-East")]
        NorthEast,
        [EnumMember(Value = "NW")]
        [Description("North-West")]
        NorthWest,
        [EnumMember(Value = "SOUTH")]
        [Description("South")]
        South,
        [EnumMember(Value = "SE")]
        [Description("South-East")]
        SouthEast,
        [EnumMember(Value = "SW")]
        [Description("South-West")]
        SouthWest,
        [EnumMember(Value = "WEST")]
        [Description("West")]
        West,
    }
}
