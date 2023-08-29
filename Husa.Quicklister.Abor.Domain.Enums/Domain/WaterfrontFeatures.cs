namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum WaterfrontFeatures
    {
        [EnumMember(Value = "CNLFT")]
        [Description("Canal Front")]
        CanalFront,
        [EnumMember(Value = "CREEK")]
        [Description("Creek")]
        Creek,
        [EnumMember(Value = "LKEF")]
        [Description("Lake Front")]
        LakeFront,
        [EnumMember(Value = "LKPV")]
        [Description("Lake Privileges")]
        LakePrivileges,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "PD")]
        [Description("Pond")]
        Pond,
        [EnumMember(Value = "RIVF")]
        [Description("River Front")]
        RiverFront,
        [EnumMember(Value = "WTFNT")]
        [Description("Water Front")]
        WaterFront,
    }
}
