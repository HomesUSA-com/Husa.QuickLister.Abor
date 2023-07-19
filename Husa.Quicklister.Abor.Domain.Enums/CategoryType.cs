namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum CategoryType
    {
        [EnumMember(Value = "SFD")]
        [Description("SFD")]
        SingleFamilyDetached = 0,
        [EnumMember(Value = "TWNHM")]
        [Description("TWNHM")]
        Townhome = 1,
    }
}
