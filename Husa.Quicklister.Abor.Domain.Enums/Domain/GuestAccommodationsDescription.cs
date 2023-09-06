namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum GuestAccommodationsDescription
    {
        [EnumMember(Value = "GCON")]
        [Description("Connected")]
        Connected,
        [EnumMember(Value = "GGAP")]
        [Description("Garage Apartment")]
        GarageApartment,
        [EnumMember(Value = "GHSE")]
        [Description("Guest House")]
        GuestHouse,
        [EnumMember(Value = "MAIN")]
        [Description("Main Level")]
        MainLevel,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "GNCN")]
        [Description("Not Connected")]
        NotConnected,
        [EnumMember(Value = "GPBT")]
        [Description("Room with Private Bath")]
        RoomWithPrivateBath,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "GSEN")]
        [Description("Separate Entrance")]
        SeparateEntrance,
        [EnumMember(Value = "GSKF")]
        [Description("Separate Kit Facilities")]
        SeparateKitFacilities,
        [EnumMember(Value = "GSLQ")]
        [Description("Separate Living Quarters")]
        SeparateLivingQuarters,
        [EnumMember(Value = "GSUT")]
        [Description("Separate Utilities")]
        SeparateUtilities,
    }
}
