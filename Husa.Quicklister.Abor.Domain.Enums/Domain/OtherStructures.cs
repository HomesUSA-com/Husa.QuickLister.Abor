namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum OtherStructures
    {
        [EnumMember(Value = "HNGR")]
        [Description("Airplane Hangar")]
        AirplaneHangar,
        [EnumMember(Value = "Arena")]
        [Description("Arena")]
        Arena,
        [EnumMember(Value = "Barn")]
        [Description("Barn(s)")]
        Barns,
        [EnumMember(Value = "Bath")]
        [Description("Bathroom")]
        Bathroom,
        [EnumMember(Value = "BHS")]
        [Description("Boat House")]
        BoatHouse,
        [EnumMember(Value = "CABAN")]
        [Description("Cabana")]
        Cabana,
        [EnumMember(Value = "CVS")]
        [Description("Cave(s)")]
        Caves,
        [EnumMember(Value = "Corrals")]
        [Description("Corral(s)")]
        Corrals,
        [EnumMember(Value = "CARENA")]
        [Description("Covered Arena")]
        CoveredArena,
        [EnumMember(Value = "GRGS")]
        [Description("Garage(s)")]
        Garages,
        [EnumMember(Value = "GAZE")]
        [Description("Gazebo")]
        Gazebo,
        [EnumMember(Value = "GRNST")]
        [Description("Grain Storage")]
        GrainStorage,
        [EnumMember(Value = "GRNHS")]
        [Description("Greenhouse")]
        Greenhouse,
        [EnumMember(Value = "GHSE")]
        [Description("Guest House")]
        GuestHouse,
        [EnumMember(Value = "KNDG")]
        [Description("Kennel/Dog Run")]
        KennelDogRun,
        [EnumMember(Value = "MBHM")]
        [Description("Mobile Home")]
        MobileHome,
        [EnumMember(Value = "OBLDG")]
        [Description("Outbuilding")]
        Outbuilding,
        [EnumMember(Value = "OKIT")]
        [Description("Outdoor Kitchen")]
        OutdoorKitchen,
        [EnumMember(Value = "PKSD")]
        [Description("Packing Shed")]
        PackingShed,
        [EnumMember(Value = "Pergola")]
        [Description("Pergola")]
        Pergola,
        [EnumMember(Value = "Hse")]
        [Description("Pool House")]
        PoolHouse,
        [EnumMember(Value = "POULT")]
        [Description("Poultry Coop")]
        PoultryCoop,
        [EnumMember(Value = "Residence")]
        [Description("Residence")]
        Residence,
        [EnumMember(Value = "RVST")]
        [Description("RV/Boat Storage")]
        RVBoatStorage,
        [EnumMember(Value = "2RES")]
        [Description("Second Residence")]
        SecondResidence,
        [EnumMember(Value = "SHD")]
        [Description("Shed")]
        Shed,
        [EnumMember(Value = "STRG")]
        [Description("Storage")]
        Storage,
        [EnumMember(Value = "TNNS")]
        [Description("Tennis Court(s)")]
        TennisCourts,
        [EnumMember(Value = "WKSP")]
        [Description("Workshop")]
        Workshop,
        [EnumMember(Value = "NONE")]
        [Description("None")]
        None,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
