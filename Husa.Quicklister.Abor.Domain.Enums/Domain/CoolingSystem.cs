namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum CoolingSystem
    {
        [EnumMember(Value = "CFAN")]
        [Description("Ceiling Fan(s)")]
        CeilingFan,
        [EnumMember(Value = "CENTRAL")]
        [Description("Central Air")]
        CentralAir,
        [EnumMember(Value = "Dual")]
        [Description("Dual")]
        Dual,
        [EnumMember(Value = "ELEC")]
        [Description("Electric")]
        Electric,
        [EnumMember(Value = "EQEQ")]
        [Description("ENERGY STAR Qualified Equipment")]
        EnergyStarQualifiedEquipment,
        [EnumMember(Value = "MU")]
        [Description("Multi Units")]
        MultiUnits,
        [EnumMember(Value = "ZND")]
        [Description("Zoned")]
        Zoned,
    }
}
