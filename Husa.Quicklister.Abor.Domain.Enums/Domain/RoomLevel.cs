namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RoomLevel
    {
        [EnumMember(Value = "FIRST")]
        [Description("First")]
        First,
        [EnumMember(Value = "LOWER")]
        [Description("Lower")]
        Lower,
        [EnumMember(Value = "MAIN")]
        [Description("Main")]
        Main,
        [EnumMember(Value = "SECOND")]
        [Description("Second")]
        Second,
        [EnumMember(Value = "THIRD")]
        [Description("Third")]
        Third,
        [EnumMember(Value = "UPPER")]
        [Description("Upper")]
        Upper,
    }
}
