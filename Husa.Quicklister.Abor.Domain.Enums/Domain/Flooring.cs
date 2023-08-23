namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Flooring
    {
        [EnumMember(Value = "Carpet")]
        [Description("Carpet")]
        Carpet,
        [EnumMember(Value = "Concrete")]
        [Description("Concrete")]
        Concrete,
        [EnumMember(Value = "Laminate")]
        [Description("Laminate")]
        Laminate,
        [EnumMember(Value = "Linoleum")]
        [Description("Linoleum")]
        Linoleum,
        [EnumMember(Value = "Marble")]
        [Description("Marble")]
        Marble,
        [EnumMember(Value = "Parquet")]
        [Description("Parquet")]
        Parquet,
        [EnumMember(Value = "SeeRemarks")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "Slate")]
        [Description("Slate")]
        Slate,
        [EnumMember(Value = "Terrazzo")]
        [Description("Terrazzo")]
        Terrazzo,
        [EnumMember(Value = "Tile")]
        [Description("Tile")]
        Tile,
        [EnumMember(Value = "Vinyl")]
        [Description("Vinyl")]
        Vinyl,
        [EnumMember(Value = "Wood")]
        [Description("Wood")]
        Wood,
    }
}
