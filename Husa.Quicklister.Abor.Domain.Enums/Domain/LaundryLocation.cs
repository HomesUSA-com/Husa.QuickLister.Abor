namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum LaundryLocation
    {
        [EnumMember(Value = "Kitchen")]
        [Description("Kitchen")]
        Kitchen,
        [EnumMember(Value = "Laundry Closet")]
        [Description("Laundry Closet")]
        LaundryCloset,
        [EnumMember(Value = "Laundry Room")]
        [Description("Laundry Room")]
        LaundryRoom,
        [EnumMember(Value = "Main Level")]
        [Description("Main Level")]
        MainLevel,
        [EnumMember(Value = "Upper Level")]
        [Description("Upper Level")]
        UpperLevel,
    }
}
