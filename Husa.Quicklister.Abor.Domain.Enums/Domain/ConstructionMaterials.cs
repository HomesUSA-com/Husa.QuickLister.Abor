namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ConstructionMaterials
    {
        [EnumMember(Value = "Adobe")]
        [Description("Adobe")]
        Adobe,
        [EnumMember(Value = "Asphalt")]
        [Description("Asphalt")]
        Asphalt,
        [EnumMember(Value = "AtticCrawlHatchwaysInsulated")]
        [Description("Attic/Crawl Hatchway(s) Insulated")]
        AtticCrawlHatchwaysInsulated,
        [EnumMember(Value = "BlownInInsulation")]
        [Description("Blown-In Insulation")]
        BlownInInsulation,
        [EnumMember(Value = "Brick")]
        [Description("Brick")]
        Brick,
        [EnumMember(Value = "BrickVeneer")]
        [Description("Brick Veneer")]
        BrickVeneer,
        [EnumMember(Value = "Cedar")]
        [Description("Cedar")]
        Cedar,
        [EnumMember(Value = "ClapBoard")]
        [Description("ClapBoard")]
        ClapBoard,
        [EnumMember(Value = "ConcreteBlock")]
        [Description("Concrete")]
        Concrete,
        [EnumMember(Value = "Frame")]
        [Description("Frame")]
        Frame,
        [EnumMember(Value = "Glass")]
        [Description("Glass")]
        Glass,
        [EnumMember(Value = "HardiPlankType")]
        [Description("HardiPlank Type")]
        HardiPlankType,
        [EnumMember(Value = "ICATRecessedLighting")]
        [Description("ICAT Recessed Lighting")]
        ICatRecessedLighting,
        [EnumMember(Value = "ICFsInsulatedConcreteForms")]
        [Description("ICFs (Insulated Concrete Forms)")]
        InsulatedConcreteForms,
        [EnumMember(Value = "Log")]
        [Description("Log")]
        Log,
        [EnumMember(Value = "AllSidesMasonry")]
        [Description("Masonry - All Sides")]
        MasonryAllSides,
        [EnumMember(Value = "MasonryPartial")]
        [Description("Masonry - Partial")]
        MasonryPartial,
        [EnumMember(Value = "NaturalBuilding")]
        [Description("Natural Building ")]
        NaturalBuilding,
        [EnumMember(Value = "RadiantBarrier")]
        [Description("Radiant Barrier")]
        RadiantBarrier,
        [EnumMember(Value = "RecycledBioBasedInsulation")]
        [Description("Recycled/Bio-Based Insulation")]
        RecycledBioBasedInsulation,
        [EnumMember(Value = "SidingRedwood")]
        [Description("Siding - Redwood")]
        SidingRedwood,
        [EnumMember(Value = "SidingWood")]
        [Description("Siding - Wood")]
        SidingWood,
        [EnumMember(Value = "VerticalSiding")]
        [Description("Siding-Vinyl")]
        SidingVinyl,
        [EnumMember(Value = "SprayFoamInsulation")]
        [Description("Spray Foam Insulation")]
        SprayFoamInsulation,
        [EnumMember(Value = "Stone")]
        [Description("Stone")]
        Stone,
        [EnumMember(Value = "StoneVeneer")]
        [Description("Stone Veneer")]
        StoneVeneer,
        [EnumMember(Value = "Stucco")]
        [Description("Stucco")]
        Stucco,
        [EnumMember(Value = "SyntheticStucco")]
        [Description("Synthetic Stucco")]
        SyntheticStucco,
    }
}
