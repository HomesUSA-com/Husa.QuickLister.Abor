namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HeatingFuel
    {
        [EnumMember(Value = "ELEC")]
        [Description("Electric")]
        Electric,
        [EnumMember(Value = "PPLNS")]
        [Description("Propane Leased")]
        PropaneLeased,
        [EnumMember(Value = "OTHER")]
        [Description("Other")]
        Other,
        [EnumMember(Value = "NTGAS")]
        [Description("Natural Gas")]
        NaturalGas,
    }
}
