namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WindowCoverings
    {
        [EnumMember(Value = "ALL")]
        [Description("All Remain")]
        AllRemain,
        [EnumMember(Value = "NONE")]
        [Description("None Remain")]
        NoneRemain,
        [EnumMember(Value = "SOME")]
        [Description("Some Remain")]
        SomeRemain,
    }
}
