namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Occupancy
    {
        [EnumMember(Value = "OWNER")]
        [Description("Owner")]
        Owner,
        [EnumMember(Value = "VACNT")]
        [Description("Vacant")]
        Vacant,
    }
}
