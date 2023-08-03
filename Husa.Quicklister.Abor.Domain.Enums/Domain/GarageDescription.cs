namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum GarageDescription
    {
        [EnumMember(Value = "AlleyAccess")]
        [Description("Alley Access")]
        AlleyAccess,
        [EnumMember(Value = "Attached")]
        [Description("Attached")]
        Attached,
        [EnumMember(Value = "CircularDriveway")]
        [Description("Circular Driveway")]
        CircularDriveway,
        [EnumMember(Value = "Concrete")]
        [Description("Concrete")]
        Concrete,
        [EnumMember(Value = "Covered")]
        [Description("Covered")]
        Covered,
        [EnumMember(Value = "Detached")]
        [Description("Detached")]
        Detached,
        [EnumMember(Value = "DirectAccess")]
        [Description("Direct Access")]
        DirectAccess,
        [EnumMember(Value = "Door-Multi")]
        [Description("Door-Multi")]
        DoorMulti,
        [EnumMember(Value = "Door-Single")]
        [Description("Door-Single")]
        DoorSingle,
        [EnumMember(Value = "DriveThrough")]
        [Description("Drive Through")]
        DriveThrough,
        [EnumMember(Value = "Driveway")]
        [Description("Driveway")]
        Driveway,
        [EnumMember(Value = "ElectricGate")]
        [Description("Electric Gate")]
        ElectricGate,
        [EnumMember(Value = "Garage")]
        [Description("Garage")]
        Garage,
        [EnumMember(Value = "GarageDoorOpener")]
        [Description("Garage Door Opener")]
        GarageDoorOpener,
        [EnumMember(Value = "GarageFacesFront")]
        [Description("Garage Faces Front")]
        GarageFacesFront,
        [EnumMember(Value = "GarageFacesRear")]
        [Description("Garage Faces Rear")]
        GarageFacesRear,
        [EnumMember(Value = "GarageFacesSide")]
        [Description("Garage Faces Side")]
        GarageFacesSide,
        [EnumMember(Value = "Gravel")]
        [Description("Gravel")]
        Gravel,
        [EnumMember(Value = "InsideEntrance")]
        [Description("Inside Entrance")]
        InsideEntrance,
        [EnumMember(Value = "KitchenLevel")]
        [Description("Kitchen Level")]
        KitchenLevel,
        [EnumMember(Value = "Lighted")]
        [Description("Lighted")]
        Lighted,
        [EnumMember(Value = "Oversized")]
        [Description("Oversized")]
        Oversized,
        [EnumMember(Value = "Private")]
        [Description("Private")]
        Private,
        [EnumMember(Value = "SideBySide")]
        [Description("Side By Side")]
        SideBySide,
        [EnumMember(Value = "Storage")]
        [Description("Storage")]
        Storage,
        [EnumMember(Value = "Tandem")]
        [Description("Tandem")]
        Tandem,
    }
}
