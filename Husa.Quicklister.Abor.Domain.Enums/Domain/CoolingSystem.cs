namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum CoolingSystem
    {
        [EnumMember(Value = "1CNTR")]
        [Description("One Central")]
        OneCentral,
        [EnumMember(Value = "2CNTR")]
        [Description("Two Central")]
        TwoCentral,
        [EnumMember(Value = "3+CNT")]
        [Description("Three+ Central")]
        ThreePlusCentral,
        [EnumMember(Value = "OTHER")]
        [Description("Other")]
        Other,
        [EnumMember(Value = "ZONED")]
        [Description("Zoned")]
        Zoned,
    }
}
