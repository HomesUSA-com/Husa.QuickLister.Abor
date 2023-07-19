namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum GreenCertification
    {
        [EnumMember(Value = "BSAG")]
        [Description("Build San Antonio Green")]
        BuildSanAntonioGreen,
        [EnumMember(Value = "ESC")]
        [Description("Energy Star Certified")]
        EnergyStarCertified,
        [EnumMember(Value = "H0-85")]
        [Description("HERS 0-85")]
        HersZeroEigthyFive,
        [EnumMember(Value = "H100+")]
        [Description("HERS 101+")]
        HersHundredOnePlus,
        [EnumMember(Value = "H86-100")]
        [Description("HERS 86-100")]
        HersEigthySixHundred,
        [EnumMember(Value = "HRTD")]
        [Description("HERS Rated")]
        HersRated,
        [EnumMember(Value = "LCRT")]
        [Description("LEED Certified")]
        LeedCertified,
        [EnumMember(Value = "LGLD")]
        [Description("LEED Gold")]
        LeedGold,
    }
}
