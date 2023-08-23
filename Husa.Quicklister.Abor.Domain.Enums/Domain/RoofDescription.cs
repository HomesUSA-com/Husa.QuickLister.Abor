namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RoofDescription
    {
        [EnumMember(Value = "Built-Up")]
        [Description("Built-Up")]
        BuiltUp,
        [EnumMember(Value = "Composition")]
        [Description("Composition")]
        Composition,
        [EnumMember(Value = "Copper")]
        [Description("Copper")]
        Copper,
        [EnumMember(Value = "FlatTile")]
        [Description("Flat Tile")]
        FlatTile,
        [EnumMember(Value = "GreenRoof")]
        [Description("Green Roof")]
        GreenRoof,
        [EnumMember(Value = "Shingle")]
        [Description("Shingle")]
        Shingle,
        [EnumMember(Value = "Slate")]
        [Description("Slate")]
        Slate,
        [EnumMember(Value = "SpanishTile")]
        [Description("Spanish Tile")]
        SpanishTile,
        [EnumMember(Value = "Synthetic")]
        [Description("Synthetic")]
        Synthetic,
        [EnumMember(Value = "Tile")]
        [Description("Tile")]
        Tile,
        [EnumMember(Value = "Wood")]
        [Description("Wood")]
        Wood,
    }
}
