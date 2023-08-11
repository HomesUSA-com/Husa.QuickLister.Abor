namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum DistanceToWaterAccess
    {
        [EnumMember(Value = "1-2 Miles")]
        [Description("1-2 Miles")]
        OneTwoMiles,
        [EnumMember(Value = "2+ Miles")]
        [Description("2+ Miles")]
        TwoPlusMiles,
        [EnumMember(Value = "In Subdivision")]
        [Description("In Subdivision")]
        InSubdivision,
        [EnumMember(Value = "Less Than 1 Mile")]
        [Description("Less Than 1 Mile")]
        LessThanOneMile,
        [EnumMember(Value = "See Remarks")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
