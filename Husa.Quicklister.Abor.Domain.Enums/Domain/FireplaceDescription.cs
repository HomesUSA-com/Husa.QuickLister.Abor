namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum FireplaceDescription
    {
        [EnumMember(Value = "Bathroom")]
        [Description("Bathroom")]
        Bathroom,
        [EnumMember(Value = "Bedroom")]
        [Description("Bedroom")]
        Bedroom,
        [EnumMember(Value = "Circulating")]
        [Description("Circulating")]
        Circulating,
        [EnumMember(Value = "Decorative")]
        [Description("Decorative")]
        Decorative,
        [EnumMember(Value = "Den")]
        [Description("Den")]
        Den,
        [EnumMember(Value = "DoubleSided")]
        [Description("Double Sided")]
        DoubleSided,
        [EnumMember(Value = "Electric")]
        [Description("Electric")]
        Electric,
        [EnumMember(Value = "EPACertifiedWoodStove")]
        [Description("EPA Certified Wood Stove")]
        EPACertifiedWoodStove,
        [EnumMember(Value = "FamilyRoom")]
        [Description("Family Room")]
        FamilyRoom,
        [EnumMember(Value = "Gas")]
        [Description("Gas")]
        Gas,
        [EnumMember(Value = "GasLog")]
        [Description("Gas Log")]
        GasLog,
        [EnumMember(Value = "GasStarter")]
        [Description("Gas Starter")]
        GasStarter,
        [EnumMember(Value = "GreatRoom")]
        [Description("Great Room")]
        GreatRoom,
        [EnumMember(Value = "LivingRoom")]
        [Description("Living Room")]
        LivingRoom,
        [EnumMember(Value = "Masonry")]
        [Description("Masonry")]
        Masonry,
        [EnumMember(Value = "Metal")]
        [Description("Metal")]
        Metal,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "Outside")]
        [Description("Outside")]
        Outside,
        [EnumMember(Value = "SeeThrough")]
        [Description("See Through")]
        SeeThrough,
        [EnumMember(Value = "Ventless")]
        [Description("Ventless")]
        Ventless,
        [EnumMember(Value = "WoodBurning")]
        [Description("Wood Burning")]
        WoodBurning,
    }
}
