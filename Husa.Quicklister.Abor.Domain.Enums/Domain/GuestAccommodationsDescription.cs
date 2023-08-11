namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum GuestAccommodationsDescription
    {
        [EnumMember(Value = "Connected")]
        [Description("Connected")]
        Connected,
        [EnumMember(Value = "GarageApartment")]
        [Description("Garage Apartment")]
        GarageApartment,
        [EnumMember(Value = "GuestHouse")]
        [Description("Guest House")]
        GuestHouse,
        [EnumMember(Value = "MainLevel")]
        [Description("Main Level")]
        MainLevel,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "NotConnected")]
        [Description("Not Connected")]
        NotConnected,
        [EnumMember(Value = "RoomwithPrivateBath")]
        [Description("Room with Private Bath")]
        RoomWithPrivateBath,
        [EnumMember(Value = "SeeAgent")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "SeparateEntrance")]
        [Description("Separate Entrance")]
        SeparateEntrance,
        [EnumMember(Value = "SeparateKitFacilities")]
        [Description("Separate Kit Facilities")]
        SeparateKitFacilities,
        [EnumMember(Value = "SeparateLivingQuarters")]
        [Description("Separate Living Quarters")]
        SeparateLivingQuarters,
        [EnumMember(Value = "SeparateUtilities")]
        [Description("Separate Utilities")]
        SeparateUtilities,
    }
}
