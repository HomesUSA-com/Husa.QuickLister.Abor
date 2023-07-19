namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum NeighborhoodAmenities
    {
        [Description("None")]
        [EnumMember(Value = "NONE")]
        None,
        [Description("Basketball Court")]
        [EnumMember(Value = "BBALLCT")]
        BasketballCourt,
        [Description("BBQ/Grill")]
        [EnumMember(Value = "BBQ")]
        BbqGrill,
        [Description("Bike Trails")]
        [EnumMember(Value = "BIKETRL")]
        BikeTrails,
        [Description("Clubhouse")]
        [EnumMember(Value = "CLBHS")]
        Clubhouse,
        [Description("Controlled Access")]
        [EnumMember(Value = "CNTRL")]
        ControlledAccess,
        [Description("Sports Court")]
        [EnumMember(Value = "COURT")]
        SportsCourt,
        [Description("Golf Course")]
        [EnumMember(Value = "GOLF")]
        GolfCourse,
        [Description("Guarded Access")]
        [EnumMember(Value = "GUARDEDACCESS")]
        GuardedAccess,
        [Description("Park/Playground")]
        [EnumMember(Value = "PARK")]
        ParkPlayground,
        [Description("Pool")]
        [EnumMember(Value = "POOL")]
        Pool,
        [Description("Tennis")]
        [EnumMember(Value = "TNNIS")]
        Tennis,
        [Description("Jogging Trails")]
        [EnumMember(Value = "TRAIL")]
        JoggingTrails,
        [Description("Volleyball Court")]
        [EnumMember(Value = "VBALLCT")]
        VolleyballCourt,
        [Description("Fishing Pier")]
        [EnumMember(Value = "FISHPIER")]
        FishingPier,
    }
}
