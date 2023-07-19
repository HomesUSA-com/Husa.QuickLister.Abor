namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RoomLevel
    {
        [EnumMember(Value = "B")]
        [Description("Basement")]
        Basement,
        [EnumMember(Value = "1")]
        [Description("Main Level")]
        MainLevel,
        [EnumMember(Value = "2")]
        [Description("Second Level")]
        SecondLevel,
        [EnumMember(Value = "3")]
        [Description("Third Level")]
        ThirdLevel,
    }
}
