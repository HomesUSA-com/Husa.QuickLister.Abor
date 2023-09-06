namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum FireplaceDescription
    {
        [EnumMember(Value = "Bath")]
        [Description("Bathroom")]
        Bathroom,
        [EnumMember(Value = "BDRM")]
        [Description("Bedroom")]
        Bedroom,
        [EnumMember(Value = "CIR")]
        [Description("Circulating")]
        Circulating,
        [EnumMember(Value = "DECRTV")]
        [Description("Decorative")]
        Decorative,
        [EnumMember(Value = "Den")]
        [Description("Den")]
        Den,
        [EnumMember(Value = "DBSD")]
        [Description("Double Sided")]
        DoubleSided,
        [EnumMember(Value = "ELEC")]
        [Description("Electric")]
        Electric,
        [EnumMember(Value = "CWDST")]
        [Description("EPA Certified Wood Stove")]
        EPACertifiedWoodStove,
        [EnumMember(Value = "FAMRM")]
        [Description("Family Room")]
        FamilyRoom,
        [EnumMember(Value = "Gas")]
        [Description("Gas")]
        Gas,
        [EnumMember(Value = "GLLHT")]
        [Description("Gas Log")]
        GasLog,
        [EnumMember(Value = "GSTR")]
        [Description("Gas Starter")]
        GasStarter,
        [EnumMember(Value = "GRM")]
        [Description("Great Room")]
        GreatRoom,
        [EnumMember(Value = "LIVRM")]
        [Description("Living Room")]
        LivingRoom,
        [EnumMember(Value = "MSRY")]
        [Description("Masonry")]
        Masonry,
        [EnumMember(Value = "MTL")]
        [Description("Metal")]
        Metal,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "OUTS")]
        [Description("Outside")]
        Outside,
        [EnumMember(Value = "STHRU")]
        [Description("See Through")]
        SeeThrough,
        [EnumMember(Value = "VNLS")]
        [Description("Ventless")]
        Ventless,
        [EnumMember(Value = "WDBN")]
        [Description("Wood Burning")]
        WoodBurning,
    }
}
