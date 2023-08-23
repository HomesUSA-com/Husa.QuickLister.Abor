namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Fencing
    {
        [EnumMember(Value = "BackYard")]
        [Description("Back Yard")]
        BackYard,
        [EnumMember(Value = "Block")]
        [Description("Block")]
        Block,
        [EnumMember(Value = "Brick")]
        [Description("Brick")]
        Brick,
        [EnumMember(Value = "Gate")]
        [Description("Gate")]
        Gate,
        [EnumMember(Value = "Masonry")]
        [Description("Masonry")]
        Masonry,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "Partial")]
        [Description("Partial")]
        Partial,
        [EnumMember(Value = "Privacy")]
        [Description("Privacy")]
        Privacy,
        [EnumMember(Value = "Security")]
        [Description("Security")]
        Security,
        [EnumMember(Value = "Stone")]
        [Description("Stone")]
        Stone,
        [EnumMember(Value = "Vinyl")]
        [Description("Vinyl")]
        Vinyl,
        [EnumMember(Value = "Wood")]
        [Description("Wood")]
        Wood,
        [EnumMember(Value = "WroughtIron")]
        [Description("Wrought Iron")]
        WroughtIron,
    }
}
