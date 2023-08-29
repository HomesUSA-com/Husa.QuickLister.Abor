namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum DistanceToWaterAccess
    {
        [EnumMember(Value = "Less2")]
        [Description("1-2 Miles")]
        OneTwoMiles,
        [EnumMember(Value = "M2nMore")]
        [Description("2+ Miles")]
        TwoPlusMiles,
        [EnumMember(Value = "INS")]
        [Description("In Subdivision")]
        InSubdivision,
        [EnumMember(Value = "Less1")]
        [Description("Less Than 1 Mile")]
        LessThanOneMile,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
