namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Showing
    {
        [EnumMember(Value = "OFFICE")]
        [Description("Office")]
        Office,
        [EnumMember(Value = "OWNER")]
        [Description("Owner")]
        Owner,
        [EnumMember(Value = "AGENT")]
        [Description("Agent")]
        Agent,
    }
}
