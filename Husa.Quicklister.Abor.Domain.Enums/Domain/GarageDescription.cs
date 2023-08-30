namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum GarageDescription
    {
        [EnumMember(Value = "ALLEYACC")]
        [Description("Alley Access")]
        AlleyAccess,
        [EnumMember(Value = "Attached")]
        [Description("Attached")]
        Attached,
        [EnumMember(Value = "CIRDW")]
        [Description("Circular Driveway")]
        CircularDriveway,
        [EnumMember(Value = "CNCRT")]
        [Description("Concrete")]
        Concrete,
        [EnumMember(Value = "CVR")]
        [Description("Covered")]
        Covered,
        [EnumMember(Value = "DET")]
        [Description("Detached")]
        Detached,
        [EnumMember(Value = "DRCT")]
        [Description("Direct Access")]
        DirectAccess,
        [EnumMember(Value = "GDMLP")]
        [Description("Door-Multi")]
        DoorMulti,
        [EnumMember(Value = "GDRSG")]
        [Description("Door-Single")]
        DoorSingle,
        [EnumMember(Value = "DTHRU")]
        [Description("Drive Through")]
        DriveThrough,
        [EnumMember(Value = "DWY")]
        [Description("Driveway")]
        Driveway,
        [EnumMember(Value = "EGATE")]
        [Description("Electric Gate")]
        ElectricGate,
        [EnumMember(Value = "Garage")]
        [Description("Garage")]
        Garage,
        [EnumMember(Value = "GDROP")]
        [Description("Garage Door Opener")]
        GarageDoorOpener,
        [EnumMember(Value = "GENFR")]
        [Description("Garage Faces Front")]
        GarageFacesFront,
        [EnumMember(Value = "GENRR")]
        [Description("Garage Faces Rear")]
        GarageFacesRear,
        [EnumMember(Value = "GENSD")]
        [Description("Garage Faces Side")]
        GarageFacesSide,
        [EnumMember(Value = "GRVL")]
        [Description("Gravel")]
        Gravel,
        [EnumMember(Value = "INEN")]
        [Description("Inside Entrance")]
        InsideEntrance,
        [EnumMember(Value = "KITLV")]
        [Description("Kitchen Level")]
        KitchenLevel,
        [EnumMember(Value = "Lighted")]
        [Description("Lighted")]
        Lighted,
        [EnumMember(Value = "OSZD")]
        [Description("Oversized")]
        Oversized,
        [EnumMember(Value = "PRV")]
        [Description("Private")]
        Private,
        [EnumMember(Value = "SDXSD")]
        [Description("Side By Side")]
        SideBySide,
        [EnumMember(Value = "STRG")]
        [Description("Storage")]
        Storage,
        [EnumMember(Value = "Tandem")]
        [Description("Tandem")]
        Tandem,
    }
}
