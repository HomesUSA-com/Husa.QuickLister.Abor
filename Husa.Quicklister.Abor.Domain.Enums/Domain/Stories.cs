namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Stories
    {
        [EnumMember(Value = "One")]
        [Description("1")]
        One,
        [EnumMember(Value = "1dot5")]
        [Description("1.5")]
        OnePointFive,
        [EnumMember(Value = "2")]
        [Description("2")]
        Two,
        [EnumMember(Value = "3plus")]
        [Description("3+")]
        ThreePlus,
        [EnumMember(Value = "MUL")]
        [Description("Multi-Level")]
        MultiLevel,
    }
}
