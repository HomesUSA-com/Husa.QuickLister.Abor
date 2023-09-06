namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RoofDescription
    {
        [EnumMember(Value = "BUILTUP")]
        [Description("Built-Up")]
        BuiltUp,
        [EnumMember(Value = "COMPSHIN")]
        [Description("Composition")]
        Composition,
        [EnumMember(Value = "Copper")]
        [Description("Copper")]
        Copper,
        [EnumMember(Value = "FLAT")]
        [Description("Flat Tile")]
        FlatTile,
        [EnumMember(Value = "GGARDN")]
        [Description("Green Roof")]
        GreenRoof,
        [EnumMember(Value = "WOODSHIN")]
        [Description("Shingle")]
        Shingle,
        [EnumMember(Value = "SLATE")]
        [Description("Slate")]
        Slate,
        [EnumMember(Value = "STILE")]
        [Description("Spanish Tile")]
        SpanishTile,
        [EnumMember(Value = "SYN")]
        [Description("Synthetic")]
        Synthetic,
        [EnumMember(Value = "TLE")]
        [Description("Tile")]
        Tile,
        [EnumMember(Value = "WD")]
        [Description("Wood")]
        Wood,
    }
}
