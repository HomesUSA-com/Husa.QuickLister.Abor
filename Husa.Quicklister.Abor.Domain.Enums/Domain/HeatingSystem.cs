namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HeatingSystem
    {
        [EnumMember(Value = "1UNIT")]
        [Description("1 Unit")]
        OneUnit,
        [EnumMember(Value = "2UNIT")]
        [Description("2 Units")]
        TwoUnits,
        [EnumMember(Value = "CNTRL")]
        [Description("Central")]
        Central,
        [EnumMember(Value = "ZONED")]
        [Description("Zoned")]
        Zoned,
        [EnumMember(Value = "HTPMP")]
        [Description("Heat Pump")]
        HeatPump,
    }
}
