namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum CommunityFeatures
    {
        [EnumMember(Value = "RUNW")]
        [Description("Airport/Runway")]
        AirportRunway,
        [EnumMember(Value = "BBQ")]
        [Description("BBQ Pit/Grill")]
        BBQPitGrill,
        [EnumMember(Value = "BIKESTG")]
        [Description("Bike Storage/Locker")]
        BikeStorageLocker,
        [EnumMember(Value = "BUSCTR")]
        [Description("Business Center")]
        BusinessCenter,
        [EnumMember(Value = "CARSHR")]
        [Description("Car Share Available")]
        CarShareAvailable,
        [EnumMember(Value = "CLUBHSE")]
        [Description("Clubhouse")]
        Clubhouse,
        [EnumMember(Value = "CLSTRMBX")]
        [Description("Cluster Mailbox")]
        ClusterMailbox,
        [EnumMember(Value = "COMMAREA")]
        [Description("Commong Grounds")]
        CommongGrounds,
        [EnumMember(Value = "CONC")]
        [Description("Concierge")]
        Concierge,
        [EnumMember(Value = "CNFMTG")]
        [Description("Conference/Meeting Room")]
        ConferenceMeetingRoom,
        [EnumMember(Value = "CNTLDA")]
        [Description("Controlled Access")]
        ControlledAccess,
        [EnumMember(Value = "CRTYRD")]
        [Description("Courtyard")]
        Courtyard,
        [EnumMember(Value = "CVRDPKG")]
        [Description("Covered Parking")]
        CoveredParking,
        [EnumMember(Value = "COFFSPC")]
        [Description("Creative Office Space")]
        CreativeOfficeSpace,
        [EnumMember(Value = "CURBS")]
        [Description("Curbs")]
        Curbs,
        [EnumMember(Value = "DCAR2GO")]
        [Description("Designated Car-2-Go Space(s)")]
        DesignatedCarToGoSpaces,
        [EnumMember(Value = "DOGPRK")]
        [Description("Dog Park/Play Area")]
        DogParkPlayArea,
        [EnumMember(Value = "EPYMT")]
        [Description("Electronic Payments")]
        ElectronicPayments,
        [EnumMember(Value = "EQUEST")]
        [Description("Equestrian Community")]
        EquestrianCommunity,
        [EnumMember(Value = "FSHG")]
        [Description("Fishing")]
        Fishing,
        [EnumMember(Value = "FITCTR")]
        [Description("Fitness Center")]
        FitnessCenter,
        [EnumMember(Value = "GMRM")]
        [Description("Game Room")]
        GameRoom,
        [EnumMember(Value = "GRGPKG")]
        [Description("Garage Parking")]
        GarageParking,
        [EnumMember(Value = "GTED")]
        [Description("Gated")]
        Gated,
        [EnumMember(Value = "SMAIRPRT")]
        [Description("General Aircarft Airport")]
        GeneralAircarftAirport,
        [EnumMember(Value = "GOLFCRSE")]
        [Description("Golf Course")]
        GolfCourse,
        [EnumMember(Value = "GOGLFBR")]
        [Description("Google Fiber")]
        GoogleFiber,
        [EnumMember(Value = "HSPCN")]
        [Description("High Speed Internet")]
        HighSpeedInternet,
        [EnumMember(Value = "HTTUB")]
        [Description("Hot Tub Community")]
        HotTubCommunity,
        [EnumMember(Value = "HOUSKPG")]
        [Description("Housekeeping")]
        Housekeeping,
        [EnumMember(Value = "KITCHFAC")]
        [Description("Kitchen Facilities")]
        KitchenFacilities,
        [EnumMember(Value = "LKE")]
        [Description("Lake")]
        Lake,
        [EnumMember(Value = "LNDRY")]
        [Description("Laundry Facility(s)")]
        LaundryFacilitys,
        [EnumMember(Value = "LNDRYSERV")]
        [Description("Laundry Service")]
        LaundryService,
        [EnumMember(Value = "LIB")]
        [Description("Library")]
        Library,
        [EnumMember(Value = "LOCKLV")]
        [Description("Lock and Leave")]
        LockandLeave,
        [EnumMember(Value = "LOUNGE")]
        [Description("Lounge")]
        Lounge,
        [EnumMember(Value = "MTNCONSITE")]
        [Description("Maintenance On-Site")]
        MaintenanceOnSite,
        [EnumMember(Value = "MDMVTHR")]
        [Description("Media Center/Movie Theatre")]
        MediaCenterMovieTheatre,
        [EnumMember(Value = "MGOLF")]
        [Description("Mini-Golf")]
        MiniGolf,
        [EnumMember(Value = "NTHERMO")]
        [Description("Nest Thermostat")]
        NestThermostat,
        [EnumMember(Value = "OSRETAIL")]
        [Description("On-Site Retail")]
        OnSiteRetail,
        [EnumMember(Value = "ONLINESERV")]
        [Description("Online Services")]
        OnlineServices,
        [EnumMember(Value = "PKGSERV")]
        [Description("Package Service")]
        PackageService,
        [EnumMember(Value = "PARK")]
        [Description("Park")]
        Park,
        [EnumMember(Value = "PETAM")]
        [Description("Pet Amenities")]
        PetAmenities,
        [EnumMember(Value = "PICNIC")]
        [Description("Picnic Area")]
        PicnicArea,
        [EnumMember(Value = "PLDSOCIAL")]
        [Description("Planned Social Activities")]
        PlannedSocialActivities,
        [EnumMember(Value = "PLGD")]
        [Description("Playground")]
        Playground,
        [EnumMember(Value = "PL")]
        [Description("Pool")]
        Pool,
        [EnumMember(Value = "PMGRS")]
        [Description("Property Manager On-Site")]
        PropertyManagerOnSite,
        [EnumMember(Value = "PTGRN")]
        [Description("Putting Green")]
        PuttingGreen,
        [EnumMember(Value = "RACQ")]
        [Description("Racquetball")]
        Racquetball,
        [EnumMember(Value = "RECYAREA")]
        [Description("Recycling Area/Center")]
        RecyclingAreaCenter,
        [EnumMember(Value = "REST")]
        [Description("Restaurant")]
        Restaurant,
        [EnumMember(Value = "ROOFLNG")]
        [Description("Rooftop Lounge")]
        RooftopLounge,
        [EnumMember(Value = "RMSERV")]
        [Description("Room Service")]
        RoomService,
        [EnumMember(Value = "SAU")]
        [Description("Sauna")]
        Sauna,
        [EnumMember(Value = "SHPGMALL")]
        [Description("Shopping Mall")]
        ShoppingMall,
        [EnumMember(Value = "SIDWL")]
        [Description("Sidewalks")]
        Sidewalks,
        [EnumMember(Value = "SMCCH")]
        [Description("Smart Car Charging")]
        SmartCarCharging,
        [EnumMember(Value = "SPA")]
        [Description("Spa/Hot Tub")]
        SpaHotTub,
        [EnumMember(Value = "SPRTFAC")]
        [Description("Sport Court(s)/Facility")]
        SportCourtsFacility,
        [EnumMember(Value = "STB")]
        [Description("Stable(s)")]
        Stables,
        [EnumMember(Value = "STRG")]
        [Description("Storage")]
        Storage,
        [EnumMember(Value = "STLT")]
        [Description("Street Lights")]
        StreetLights,
        [EnumMember(Value = "SBRBN")]
        [Description("Suburban")]
        Suburban,
        [EnumMember(Value = "SDCK")]
        [Description("Sundeck")]
        Sundeck,
        [EnumMember(Value = "TANNING")]
        [Description("Tanning Salon")]
        TanningSalon,
        [EnumMember(Value = "TNTCARGO")]
        [Description("Tenant Access Cargo Elevator")]
        TenantAccessCargoElevator,
        [EnumMember(Value = "TNNS")]
        [Description("Tennis Court(s)")]
        TennisCourts,
        [EnumMember(Value = "TRDD")]
        [Description("Trash Pickup - Door to Door")]
        TrashPickupDoortoDoor,
        [EnumMember(Value = "UVERSE")]
        [Description("U-Verse")]
        UVerse,
        [EnumMember(Value = "UDUTL")]
        [Description("Underground Utilities")]
        UndergroundUtilities,
        [EnumMember(Value = "VALET")]
        [Description("Valet Parking")]
        ValetParking,
        [EnumMember(Value = "JPATH")]
        [Description("Walk/Bike/Hike/Jog Trails(s)")]
        WalkBikeHikeJogTrails,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
