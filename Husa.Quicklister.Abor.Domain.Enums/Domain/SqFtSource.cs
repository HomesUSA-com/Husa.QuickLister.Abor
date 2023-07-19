namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum SqFtSource
    {
        [EnumMember(Value = "A")]
        [Description("Appraiser")]
        Appraiser,
        [EnumMember(Value = "B")]
        [Description("Bldr Plans")]
        BuilderPlans,
        [EnumMember(Value = "D")]
        [Description("Appsl Dist")]
        AppraisalDist,
        [EnumMember(Value = "H")]
        [Description("HUD")]
        Hud,
    }
}
