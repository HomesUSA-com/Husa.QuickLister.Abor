namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Stories
    {
        [EnumMember(Value = "1")]
        [Description("1")]
        One,
        [EnumMember(Value = "1.5")]
        [Description("1.5")]
        OnePointFive,
        [EnumMember(Value = "2")]
        [Description("2")]
        Two,
        [EnumMember(Value = "3+")]
        [Description("3+")]
        ThreePlus,
    }
}
