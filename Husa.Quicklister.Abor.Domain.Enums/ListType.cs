namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ListType
    {
        [EnumMember(Value = "COM")]
        [Description("Commercial")]
        Commercial = 0,
        [EnumMember(Value = "LOT")]
        [Description("Lots")]
        Lots = 1,
        [EnumMember(Value = "LSE")]
        [Description("Lease")]
        Lease = 2,
        [EnumMember(Value = "MUL")]
        [Description("Multi-Family")]
        MultiFamily = 3,
        [EnumMember(Value = "RES")]
        [Description("Residential")]
        Residential = 4,
    }
}
