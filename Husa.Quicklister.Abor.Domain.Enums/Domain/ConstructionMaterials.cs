namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ConstructionMaterials
    {
        [EnumMember(Value = "ADB")]
        [Description("Adobe")]
        Adobe,
        [EnumMember(Value = "ASPHALT")]
        [Description("Asphalt")]
        Asphalt,
        [EnumMember(Value = "ATTINS")]
        [Description("Attic/Crawl Hatchway(s) Insulated")]
        AtticCrawlHatchwaysInsulated,
        [EnumMember(Value = "BIINS")]
        [Description("Blown-In Insulation")]
        BlownInInsulation,
        [EnumMember(Value = "Brick")]
        [Description("Brick")]
        Brick,
        [EnumMember(Value = "BRICKV")]
        [Description("Brick Veneer")]
        BrickVeneer,
        [EnumMember(Value = "CDR")]
        [Description("Cedar")]
        Cedar,
        [EnumMember(Value = "CLAPBRD")]
        [Description("Clapboard")]
        ClapBoard,
        [EnumMember(Value = "CNCRT")]
        [Description("Concrete")]
        Concrete,
        [EnumMember(Value = "FRAME")]
        [Description("Frame")]
        Frame,
        [EnumMember(Value = "GLASS")]
        [Description("Glass")]
        Glass,
        [EnumMember(Value = "HARDI")]
        [Description("HardiPlank Type")]
        HardiPlankType,
        [EnumMember(Value = "ICATRecessedLighting")]
        [Description("ICAT Recessed Lighting")]
        ICatRecessedLighting,
        [EnumMember(Value = "ICFCNT")]
        [Description("ICFs (Insulated Concrete Forms)")]
        InsulatedConcreteForms,
        [EnumMember(Value = "LOG")]
        [Description("Log")]
        Log,
        [EnumMember(Value = "MSRYA")]
        [Description("Masonry-All Sides")]
        MasonryAllSides,
        [EnumMember(Value = "MSRYP")]
        [Description("Masonry-Partial")]
        MasonryPartial,
        [EnumMember(Value = "NATBLDG")]
        [Description("Natural Building")]
        NaturalBuilding,
        [EnumMember(Value = "RBAR")]
        [Description("Radiant Barrier")]
        RadiantBarrier,
        [EnumMember(Value = "RECYBIO")]
        [Description("Recycled/Bio-Based Insulation")]
        RecycledBioBasedInsulation,
        [EnumMember(Value = "RDWD")]
        [Description("Siding-Redwood")]
        SidingRedwood,
        [EnumMember(Value = "WDSD")]
        [Description("Siding-Wood")]
        SidingWood,
        [EnumMember(Value = "VNYL")]
        [Description("Siding-Vinyl")]
        SidingVinyl,
        [EnumMember(Value = "SPFOAM")]
        [Description("Spray Foam Insulation")]
        SprayFoamInsulation,
        [EnumMember(Value = "STN")]
        [Description("Stone")]
        Stone,
        [EnumMember(Value = "STVNR")]
        [Description("Stone Veneer")]
        StoneVeneer,
        [EnumMember(Value = "STUC")]
        [Description("Stucco")]
        Stucco,
        [EnumMember(Value = "SYNST")]
        [Description("Synthetic Stucco")]
        SyntheticStucco,
    }
}
