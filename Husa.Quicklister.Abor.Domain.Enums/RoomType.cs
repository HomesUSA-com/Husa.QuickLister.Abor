namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RoomType
    {
        [EnumMember(Value = "entry")]
        [Description("Entry")]
        Entry = 0,
        [EnumMember(Value = "living")]
        [Description("Living")]
        Living = 1,
        [EnumMember(Value = "family")]
        [Description("Family")]
        Family = 2,
        [EnumMember(Value = "other")]
        [Description("Mud Room (Other)")]
        Other = 3,
        [EnumMember(Value = "game")]
        [Description("Game (Other)")]
        Game = 4,
        [EnumMember(Value = "media")]
        [Description("Media (Other)")]
        Media = 5,
        [EnumMember(Value = "dining")]
        [Description("Dining")]
        Dining = 6,
        [EnumMember(Value = "breakfast")]
        [Description("Breakfast")]
        Breakfast = 7,
        [EnumMember(Value = "kitchen")]
        [Description("Kitchen")]
        Kitchen = 8,
        [EnumMember(Value = "study")]
        [Description("Study")]
        Study = 9,
        [EnumMember(Value = "masterBedroom")]
        [Description("Master Bedroom")]
        MasterBedroom = 10,
        [EnumMember(Value = "masterBedroomCloset")]
        [Description("Master Bedroom Closet")]
        MasterBedroomCloset = 11,
        [EnumMember(Value = "bed")]
        [Description("Bedroom")]
        Bed = 12,
        [EnumMember(Value = "masterBath")]
        [Description("Master Bath (Other)")]
        MasterBath = 13,
        [EnumMember(Value = "utility")]
        [Description("Utility")]
        Utility = 14,
        [EnumMember(Value = "fullBath")]
        [Description("FullBath")]
        FullBath = 15,
        [EnumMember(Value = "office")]
        [Description("Office")]
        Office = 16,
        [EnumMember(Value = "student")]
        [Description("Student")]
        Student = 17,
        [EnumMember(Value = "halfBath")]
        [Description("HalfBath")]
        HalfBath = 18,
    }
}
