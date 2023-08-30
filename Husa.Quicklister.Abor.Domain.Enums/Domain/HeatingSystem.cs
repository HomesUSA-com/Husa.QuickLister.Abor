namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HeatingSystem
    {
        [EnumMember(Value = "CEIL")]
        [Description("Ceiling")]
        Ceiling,
        [EnumMember(Value = "CENTRAL")]
        [Description("Central")]
        Central,
        [EnumMember(Value = "ELEC")]
        [Description("Electric")]
        Electric,
        [EnumMember(Value = "EQEQ")]
        [Description("ENERGY STAR Qualified Equipment")]
        EnergyStarQualifiedEquipment,
        [EnumMember(Value = "EXFEN")]
        [Description("Exhaust Fan")]
        ExhaustFan,
        [EnumMember(Value = "FRPL")]
        [Description("Fireplace(s)")]
        Fireplace,
        [EnumMember(Value = "GAS")]
        [Description("Natural Gas")]
        NaturalGas,
        [EnumMember(Value = "PSOLR")]
        [Description("Passive Solar")]
        PassiveSolar,
        [EnumMember(Value = "PROPN")]
        [Description("Propane")]
        Propane,
        [EnumMember(Value = "ZND")]
        [Description("Zoned")]
        Zoned,
    }
}
