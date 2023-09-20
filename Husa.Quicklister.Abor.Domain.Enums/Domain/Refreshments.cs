namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;
    public enum Refreshments
    {
        [EnumMember(Value = "A")]
        [Description("Appetizers")]
        Appetizers,
        [EnumMember(Value = "B")]
        [Description("Beverages")]
        Beverages,
        [EnumMember(Value = "C")]
        [Description("Catered Lunch")]
        CateredLunch,
        [EnumMember(Value = "D")]
        [Description("Desserts")]
        Desserts,
        [EnumMember(Value = "L")]
        [Description("Lunch")]
        Lunch,
        [EnumMember(Value = "P")]
        [Description("Pastries")]
        Pastries,
        [EnumMember(Value = "S")]
        [Description("Snacks")]
        Snacks,
    }
}
