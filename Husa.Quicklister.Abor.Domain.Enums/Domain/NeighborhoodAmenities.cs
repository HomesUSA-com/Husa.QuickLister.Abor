namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum NeighborhoodAmenities
    {
        [EnumMember(Value = "CLUBHSE")]
        [Description("Clubhouse")]
        Clubhouse,
        [EnumMember(Value = "CLSTRMBX")]
        [Description("Cluster Mailbox")]
        ClusterMailbox,
        [EnumMember(Value = "COMMAREA")]
        [Description("Common Grounds")]
        CommonGrounds,
        [EnumMember(Value = "CNTLDA")]
        [Description("Controlled Access")]
        ControlledAccess,
        [EnumMember(Value = "CRTYRD")]
        [Description("Courtyard")]
        Courtyard,
        [EnumMember(Value = "CURBS")]
        [Description("Curbs")]
        Curbs,
        [EnumMember(Value = "DOGPRK")]
        [Description("Dog Park/Play Area")]
        DogParkPlayArea,
        [EnumMember(Value = "FSHG")]
        [Description("Fishing")]
        Fishing,
        [EnumMember(Value = "FITCTR")]
        [Description("Fitness Center")]
        FitnessCenter,
        [EnumMember(Value = "GMRM")]
        [Description("Game Room")]
        GameRoom,
        [EnumMember(Value = "GTED")]
        [Description("Gated")]
        Gated,
        [EnumMember(Value = "GOLFCRSE")]
        [Description("Golf Course")]
        GolfCourse,
        [EnumMember(Value = "GOGLFBR")]
        [Description("Google Fiber")]
        GoogleFiber,
        [EnumMember(Value = "HSPCN")]
        [Description("High Speed Internet")]
        HighSpeedInternet,
        [EnumMember(Value = "KITCHFAC")]
        [Description("Kitchen Facilities")]
        KitchenFacilities,
        [EnumMember(Value = "LKE")]
        [Description("Lake")]
        Lake,
        [EnumMember(Value = "MDMVTHR")]
        [Description("Media Center/Movie Theatre")]
        MediaCenterMovieTheatre,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "PARK")]
        [Description("Park")]
        Park,
        [EnumMember(Value = "PETAM")]
        [Description("Pet Amenities")]
        PetAmenities,
        [EnumMember(Value = "PICNIC")]
        [Description("Picnic Area")]
        PicnicArea,
        [EnumMember(Value = "PLGD")]
        [Description("Playground")]
        Playground,
        [EnumMember(Value = "PL")]
        [Description("Pool")]
        Pool,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "SIDWL")]
        [Description("Sidewalks")]
        Sidewalks,
        [EnumMember(Value = "SPRTFAC")]
        [Description("Sport Court(s)/Facility")]
        SportCourtFacility,
        [EnumMember(Value = "STRG")]
        [Description("Storage")]
        Storage,
        [EnumMember(Value = "STLT")]
        [Description("Street Lights")]
        StreetLights,
        [EnumMember(Value = "TNNS")]
        [Description("Tennis Court(s)")]
        TennisCourt,
        [EnumMember(Value = "UDUTL")]
        [Description("Underground Utilities")]
        UndergroundUtilities,
        [EnumMember(Value = "UVERSE")]
        [Description("U-Verse")]
        UVerse,
        [EnumMember(Value = "JPATH")]
        [Description("Walk/Bike/Hike/Jog Trails")]
        WalkBikeHikeJogTrails,
    }
}
