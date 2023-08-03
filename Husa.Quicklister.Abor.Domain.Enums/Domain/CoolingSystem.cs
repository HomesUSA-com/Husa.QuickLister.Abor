namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum CoolingSystem
    {
        [EnumMember(Value = "CeilingFans")]
        [Description("Ceiling Fan(s)")]
        CeilingFan,
        [EnumMember(Value = "CentralAir")]
        [Description("Central Air")]
        CentralAir,
        [EnumMember(Value = "Dual")]
        [Description("Dual")]
        Dual,
        [EnumMember(Value = "Electric")]
        [Description("Electric")]
        Electric,
        [EnumMember(Value = "ENERGYSTARQualifiedEquipment")]
        [Description("ENERGY STAR Qualified Equipment")]
        EnergyStarQualifiedEquipment,
        [EnumMember(Value = "MultiUnits")]
        [Description("Multi Units")]
        MultiUnits,
        [EnumMember(Value = "Zoned")]
        [Description("Zoned")]
        Zoned,
    }
}
