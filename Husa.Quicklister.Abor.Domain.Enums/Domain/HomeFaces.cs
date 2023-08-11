namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HomeFaces
    {
        [EnumMember(Value = "East")]
        [Description("East")]
        East,
        [EnumMember(Value = "North")]
        [Description("North")]
        North,
        [EnumMember(Value = "Northeast")]
        [Description("North-East")]
        NorthEast,
        [EnumMember(Value = "Northwest")]
        [Description("North-West")]
        NorthWest,
        [EnumMember(Value = "South")]
        [Description("South")]
        South,
        [EnumMember(Value = "Southeast")]
        [Description("South-East")]
        SouthEast,
        [EnumMember(Value = "Southwest")]
        [Description("South-West")]
        SouthWest,
        [EnumMember(Value = "West")]
        [Description("West")]
        West,
    }
}
