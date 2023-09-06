namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Fencing
    {
        [EnumMember(Value = "BKYRD")]
        [Description("Back Yard")]
        BackYard,
        [EnumMember(Value = "Block")]
        [Description("Block")]
        Block,
        [EnumMember(Value = "Brick")]
        [Description("Brick")]
        Brick,
        [EnumMember(Value = "GATE")]
        [Description("Gate")]
        Gate,
        [EnumMember(Value = "MSRY")]
        [Description("Masonry")]
        Masonry,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "PART")]
        [Description("Partial")]
        Partial,
        [EnumMember(Value = "PRV")]
        [Description("Privacy")]
        Privacy,
        [EnumMember(Value = "SECU")]
        [Description("Security")]
        Security,
        [EnumMember(Value = "STN")]
        [Description("Stone")]
        Stone,
        [EnumMember(Value = "VNYL")]
        [Description("Vinyl")]
        Vinyl,
        [EnumMember(Value = "WD")]
        [Description("Wood")]
        Wood,
        [EnumMember(Value = "WIRON")]
        [Description("Wrought Iron")]
        WroughtIron,
    }
}
