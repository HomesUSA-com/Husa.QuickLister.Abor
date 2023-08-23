namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HeatingSystem
    {
        [EnumMember(Value = "Ceiling")]
        [Description("Ceiling")]
        Ceiling,
        [EnumMember(Value = "Central")]
        [Description("Central")]
        Central,
        [EnumMember(Value = "Electric")]
        [Description("Electric")]
        Electric,
        [EnumMember(Value = "ENERGYSTARQualifiedEquipment")]
        [Description("ENERGY STAR Qualified Equipment")]
        EnergyStarQualifiedEquipment,
        [EnumMember(Value = "ExhaustFan")]
        [Description("Exhaust Fan")]
        ExhaustFan,
        [EnumMember(Value = "Fireplaces")]
        [Description("Fireplace(s)")]
        Fireplace,
        [EnumMember(Value = "NaturalGas")]
        [Description("Natural Gas")]
        NaturalGas,
        [EnumMember(Value = "PassiveSolar")]
        [Description("Passive Solar")]
        PassiveSolar,
        [EnumMember(Value = "Propane")]
        [Description("Propane")]
        Propane,
        [EnumMember(Value = "Zoned")]
        [Description("Zoned")]
        Zoned,
    }
}
