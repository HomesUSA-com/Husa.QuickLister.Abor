namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ShowingContactType
    {
        [EnumMember(Value = "AGT")]
        [Description("Agent")]
        Agent,
        [EnumMember(Value = "OCC")]
        [Description("Occupant")]
        Occupant,
        [EnumMember(Value = "OWN")]
        [Description("Owner")]
        Owner,
        [EnumMember(Value = "PMR")]
        [Description("Property Manager")]
        PropertyManager,
    }
}
