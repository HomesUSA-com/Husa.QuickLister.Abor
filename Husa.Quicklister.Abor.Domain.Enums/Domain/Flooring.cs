namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Flooring
    {
        [EnumMember(Value = "CARPET")]
        [Description("Carpet")]
        Carpet,
        [EnumMember(Value = "CNCRT")]
        [Description("Concrete")]
        Concrete,
        [EnumMember(Value = "LAMINATE")]
        [Description("Laminate")]
        Laminate,
        [EnumMember(Value = "LINOLEUM")]
        [Description("Linoleum")]
        Linoleum,
        [EnumMember(Value = "MARBLE")]
        [Description("Marble")]
        Marble,
        [EnumMember(Value = "PARQUET")]
        [Description("Parquet")]
        Parquet,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "SLATE")]
        [Description("Slate")]
        Slate,
        [EnumMember(Value = "TERRZ")]
        [Description("Terrazzo")]
        Terrazzo,
        [EnumMember(Value = "TLE")]
        [Description("Tile")]
        Tile,
        [EnumMember(Value = "VNYL")]
        [Description("Vinyl")]
        Vinyl,
        [EnumMember(Value = "WD")]
        [Description("Wood")]
        Wood,
    }
}
