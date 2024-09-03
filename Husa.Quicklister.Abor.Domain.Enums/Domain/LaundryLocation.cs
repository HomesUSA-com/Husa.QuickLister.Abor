namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum LaundryLocation
    {
        [EnumMember(Value = "KTCH")]
        [Description("Kitchen")]
        Kitchen,
        [EnumMember(Value = "LCLST")]
        [Description("Laundry Closet")]
        LaundryCloset,
        [EnumMember(Value = "LURM")]
        [Description("Laundry Room")]
        LaundryRoom,
        [EnumMember(Value = "MAIN")]
        [Description("Main Level")]
        MainLevel,
        [EnumMember(Value = "UPR")]
        [Description("Upper Level")]
        UpperLevel,
        [EnumMember(Value = "Inside")]
        [Description("Inside")]
        Inside,
    }
}
