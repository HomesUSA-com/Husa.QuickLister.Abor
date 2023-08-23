namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RoomType
    {
        [EnumMember(Value = "MSTRBED")]
        [Description("Primary Bedroom")]
        PrimaryBedroom,
        [EnumMember(Value = "BDRM")]
        [Description("Bedroom")]
        Bedroom,
        [EnumMember(Value = "MSTRBATH")]
        [Description("Primary Bathroom")]
        PrimaryBathroom,
        [EnumMember(Value = "BA")]
        [Description("Bathroom")]
        Bathroom,
        [EnumMember(Value = "KITCHEN")]
        [Description("Kitchen")]
        Kitchen,
        [EnumMember(Value = "AKIT")]
        [Description("Additional Kitchen")]
        AdditionalKitchen,
        [EnumMember(Value = "RMATR")]
        [Description("Atrium")]
        Atrium,
        [EnumMember(Value = "BSMNT")]
        [Description("Basement")]
        Basement,
        [EnumMember(Value = "BONUS")]
        [Description("Bonus Room")]
        Bonus,
        [EnumMember(Value = "RMCNS")]
        [Description("Conservatory")]
        Conservatory,
        [EnumMember(Value = "RMCGR")]
        [Description("Converted Garage")]
        ConvertedGarage,
        [EnumMember(Value = "DEN")]
        [Description("Den")]
        Den,
        [EnumMember(Value = "DINING")]
        [Description("Dining Room")]
        Dining,
        [EnumMember(Value = "RMEXC")]
        [Description("Exercise Room")]
        ExerciseRoom,
        [EnumMember(Value = "FAMILY")]
        [Description("Family Room")]
        FamilyRoom,
        [EnumMember(Value = "RMFYR")]
        [Description("Foyer")]
        Foyer,
        [EnumMember(Value = "GAME")]
        [Description("Game Room")]
        Game,
        [EnumMember(Value = "GREAT")]
        [Description("Great Room")]
        GreatRoom,
        [EnumMember(Value = "GYM")]
        [Description("Gym")]
        Gym,
        [EnumMember(Value = "LAUNDRY")]
        [Description("Laundry")]
        Laundry,
        [EnumMember(Value = "LIBRARY")]
        [Description("Library")]
        Library,
        [EnumMember(Value = "LIVING")]
        [Description("Living Room")]
        Living,
        [EnumMember(Value = "LOFT")]
        [Description("Loft")]
        Loft,
        [EnumMember(Value = "MEDIA")]
        [Description("Media Room")]
        MediaRoom,
        [EnumMember(Value = "OFFICE")]
        [Description("Office")]
        Office,
        [EnumMember(Value = "SAUNA")]
        [Description("Sauna")]
        Sauna,
        [EnumMember(Value = "RMSPP")]
        [Description("Screened Patio/Porch")]
        ScreenedPatioPorch,
        [EnumMember(Value = "STRG")]
        [Description("Storage")]
        Storage,
        [EnumMember(Value = "RMSUN")]
        [Description("Sun")]
        Sun,
        [EnumMember(Value = "WNE")]
        [Description("Wine")]
        Wine,
        [EnumMember(Value = "WORKSHOP")]
        [Description("Workshop")]
        Workshop,
    }
}
