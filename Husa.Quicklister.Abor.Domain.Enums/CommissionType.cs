namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum CommissionType
    {
        [Description("$")]
        [EnumMember(Value = "$")]
        Amount = 0,
        [EnumMember(Value = "%")]
        [Description("%")]
        Percent = 1,
    }
}
