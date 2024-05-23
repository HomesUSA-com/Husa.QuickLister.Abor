namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HorseAmenities
    {
        [EnumMember(Value = "Arena")]
        [Description("Arena")]
        Arena,
        [EnumMember(Value = "Barn")]
        [Description("Barn")]
        Barn,
        [EnumMember(Value = "BDFC")]
        [Description("Boarding Facilities")]
        BoardingFacilities,
        [EnumMember(Value = "BRDPT")]
        [Description("Bridle Path")]
        BridlePath,
        [EnumMember(Value = "Corrals")]
        [Description("Corral(s)")]
        Corrals,
        [EnumMember(Value = "HYST")]
        [Description("Hay Storage")]
        HayStorage,
        [EnumMember(Value = "PADD")]
        [Description("Paddocks")]
        Paddocks,
        [EnumMember(Value = "PACHU")]
        [Description("Palpation Chute")]
        PalpationChute,
        [EnumMember(Value = "PSTRE")]
        [Description("Pasture")]
        Pasture,
        [EnumMember(Value = "RTRL")]
        [Description("Riding Trail")]
        RidingTrail,
        [EnumMember(Value = "RPEN")]
        [Description("Round Pen")]
        RoundPen,
        [EnumMember(Value = "SHBIN")]
        [Description("Shaving Bin")]
        ShavingBin,
        [EnumMember(Value = "STB")]
        [Description("Stable(s)")]
        Stables,
        [EnumMember(Value = "TCK")]
        [Description("Tack Room")]
        TackRoom,
        [EnumMember(Value = "TRSTRG")]
        [Description("Trailer Storage")]
        TrailerStorage,
        [EnumMember(Value = "WRK")]
        [Description("Wash Rack")]
        WashRack,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
