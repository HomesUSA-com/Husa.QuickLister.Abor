namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Floors
    {
        [Description("Brick")]
        [EnumMember(Value = "BRICK")]
        Brick,
        [Description("Carpeting")]
        [EnumMember(Value = "CRPT")]
        Carpeting,
        [Description("Ceramic Tile")]
        [EnumMember(Value = "CTILE")]
        CeramicTile,
        [Description("Laminate")]
        [EnumMember(Value = "LMNAT")]
        Laminate,
        [Description("Other")]
        [EnumMember(Value = "OTHER")]
        Other,
        [Description("Saltillo Tile")]
        [EnumMember(Value = "STILE")]
        SaltilloTile,
        [Description("Stone")]
        [EnumMember(Value = "STONE")]
        Stone,
        [Description("Vinyl")]
        [EnumMember(Value = "VINYL")]
        Vinyl,
        [Description("Wood")]
        [EnumMember(Value = "WOOD")]
        Wood,
    }
}
