namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RoomType
    {
        [EnumMember(Value = "living")]
        [Description("Living")]
        Living,
        [EnumMember(Value = "other")]
        [Description("Other Room")]
        Other,
        [EnumMember(Value = "game")]
        [Description("Game")]
        Game,
        [EnumMember(Value = "dining")]
        [Description("Dining")]
        Dining,
        [EnumMember(Value = "kitchen")]
        [Description("Kitchen")]
        Kitchen,
        [EnumMember(Value = "masterBedroom")]
        [Description("Master Bedroom")]
        MasterBedroom,
        [EnumMember(Value = "masterBath")]
        [Description("Master Bath")]
        MasterBath,
        [EnumMember(Value = "office")]
        [Description("Office")]
        Office,
        [EnumMember(Value = "Loft")]
        [Description("Loft")]
        Loft,
        [EnumMember(Value = "Library")]
        [Description("Library")]
        Library,
        [EnumMember(Value = "Bonus")]
        [Description("Bonus")]
        Bonus,
    }
}
