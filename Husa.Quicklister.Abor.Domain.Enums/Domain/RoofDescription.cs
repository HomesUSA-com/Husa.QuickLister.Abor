namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum RoofDescription
    {
        [EnumMember(Value = "COMP")]
        [Description("Composition")]
        Composition,
        [EnumMember(Value = "CONCR")]
        [Description("Concrete")]
        Concrete,
        [EnumMember(Value = "HVCMP")]
        [Description("Heavy Composition")]
        HeavyComposition,
        [EnumMember(Value = "METAL")]
        [Description("Metal")]
        Metal,
        [EnumMember(Value = "TILE")]
        [Description("Tile")]
        Tile,
        [EnumMember(Value = "WDSHN")]
        [Description("Wood Shingle/Shake")]
        WoodShingleShake,
        [EnumMember(Value = "WOOD")]
        [Description("Wood")]
        Wood,
    }
}
