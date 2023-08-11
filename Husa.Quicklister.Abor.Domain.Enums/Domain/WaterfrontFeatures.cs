namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WaterfrontFeatures
    {
        [EnumMember(Value = "CanalFront")]
        [Description("Canal Front")]
        CanalFront,
        [EnumMember(Value = "Creek")]
        [Description("Creek")]
        Creek,
        [EnumMember(Value = "LakeFront")]
        [Description("Lake Front")]
        LakeFront,
        [EnumMember(Value = "LakePrivileges")]
        [Description("Lake Privileges")]
        LakePrivileges,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "Pond")]
        [Description("Pond")]
        Pond,
        [EnumMember(Value = "RiverFront")]
        [Description("River Front")]
        RiverFront,
        [EnumMember(Value = "Waterfront")]
        [Description("Water Front")]
        WaterFront,
    }
}
