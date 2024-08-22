namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum LotDescription
    {
        [EnumMember(Value = "AGCLT")]
        [Description("Agricultural")]
        Agricultural,
        [EnumMember(Value = "ALACS")]
        [Description("Alley Access")]
        AlleyAccess,
        [EnumMember(Value = "BACKPARK")]
        [Description("Back to Park/Greenbelt")]
        BackstoGreenbeltPark,
        [EnumMember(Value = "BKYRD")]
        [Description("Back Yard")]
        BackYard,
        [EnumMember(Value = "BACKGOLF")]
        [Description("On Golf Course")]
        OnGolfCourse,
        [EnumMember(Value = "Bluff")]
        [Description("Bluff")]
        Bluff,
        [EnumMember(Value = "CityLot")]
        [Description("City Lot")]
        CityLot,
        [EnumMember(Value = "CLEARED")]
        [Description("Cleared")]
        Cleared,
        [EnumMember(Value = "CLCLBHS")]
        [Description("Close to Clubhouse")]
        ClosetoClubhouse,
        [EnumMember(Value = "CRNR")]
        [Description("Corner Lot")]
        CornerLot,
        [EnumMember(Value = "CUDSC")]
        [Description("Cul-De-Sac")]
        CulDeSac,
        [EnumMember(Value = "CURBS")]
        [Description("Curbs")]
        Curbs,
        [EnumMember(Value = "Farm")]
        [Description("Farm")]
        Farm,
        [EnumMember(Value = "FTR")]
        [Description("Few Tree")]
        FewTree,
        [EnumMember(Value = "FLAGLOT")]
        [Description("Flag Lot")]
        FlagLot,
        [EnumMember(Value = "FTYRD")]
        [Description("Front Yard")]
        FrontYard,
        [EnumMember(Value = "Garden")]
        [Description("Garden")]
        Garden,
        [EnumMember(Value = "GSLO")]
        [Description("Gentle Sloping")]
        GentleSloping,
        [EnumMember(Value = "INTERIOR")]
        [Description("Interior Lot")]
        InteriorLot,
        [EnumMember(Value = "IRREG")]
        [Description("Irregular Lot")]
        IrregularLot,
        [EnumMember(Value = "LNDCP")]
        [Description("Landscaped")]
        Landscaped,
        [EnumMember(Value = "Level")]
        [Description("Level")]
        Level,
        [EnumMember(Value = "NPLN")]
        [Description("Native Plants  ")]
        NativePlants,
        [EnumMember(Value = "GOLFCOMM")]
        [Description("Near Golf Course")]
        NearGolfCourse,
        [EnumMember(Value = "PUB")]
        [Description("Near Public Transit")]
        NearPublicTransit,
        [EnumMember(Value = "Open")]
        [Description("Open Lot")]
        OpenLot,
        [EnumMember(Value = "ORCHD")]
        [Description("Orchard(s)")]
        Orchard,
        [EnumMember(Value = "PIE")]
        [Description("Pie Shaped Lot")]
        PieShapedLot,
        [EnumMember(Value = "PRV")]
        [Description("Private")]
        Private,
        [EnumMember(Value = "PRVRD")]
        [Description("Private Maintained Road")]
        PrivateMaintainedRoad,
        [EnumMember(Value = "PMNRD")]
        [Description("Public Maintained Road")]
        PublicMaintainedRoad,
        [EnumMember(Value = "RMTAG")]
        [Description("Road Maintenance Agreement")]
        RoadMaintenanceAgreement,
        [EnumMember(Value = "ROUGH")]
        [Description("Rock Outcropping")]
        RockOutcropping,
        [EnumMember(Value = "ROLLING")]
        [Description("Rolling Slope")]
        RollingSlope,
        [EnumMember(Value = "SLDN")]
        [Description("Sloped Down")]
        SlopedDown,
        [EnumMember(Value = "SLUp")]
        [Description("Sloped Up")]
        SlopedUp,
        [EnumMember(Value = "SPPOS")]
        [Description("Split Possible")]
        SplitPossible,
        [EnumMember(Value = "SPAU")]
        [Description("Sprinkler - Automatic")]
        SprinklerAutomatic,
        [EnumMember(Value = "SPREA")]
        [Description("Sprinkler - Back Yard")]
        SprinklerBackYard,
        [EnumMember(Value = "SPDP")]
        [Description("Sprinkler - Drip Only/Bubblers")]
        SprinklerDripOnlyBubblers,
        [EnumMember(Value = "SPFNT")]
        [Description("Sprinkler - In Front")]
        SprinklerInFront,
        [EnumMember(Value = "SPING")]
        [Description("Sprinkler - In-ground")]
        SprinklerInground,
        [EnumMember(Value = "MNUAL")]
        [Description("Sprinkler - Manual")]
        SprinklerManual,
        [EnumMember(Value = "SPPAR")]
        [Description("Sprinkler - Partial")]
        SprinklerPartial,
        [EnumMember(Value = "SPRAIN")]
        [Description("Sprinkler - Rain Sensor")]
        SprinklerRainSensor,
        [EnumMember(Value = "SPSD")]
        [Description("Sprinkler - Side Yard")]
        SprinklerSideYard,
        [EnumMember(Value = "STEEP")]
        [Description("Steep Slope")]
        SteepSlope,
        [EnumMember(Value = "SUBD")]
        [Description("Subdivided")]
        Subdivided,
        [EnumMember(Value = "HEAVY")]
        [Description("Trees-Heavy")]
        TreesHeavy,
        [EnumMember(Value = "LARGE")]
        [Description("Trees-Large (Over 40 Ft)")]
        TreesLarge,
        [EnumMember(Value = "Wooded")]
        [Description("Trees-Many")]
        TreesMany,
        [EnumMember(Value = "MEDIUM")]
        [Description("Trees-Medium (20 Ft - 40 Ft)")]
        TreesMedium,
        [EnumMember(Value = "MODERATE")]
        [Description("Trees-Moderate")]
        TreesModerate,
        [EnumMember(Value = "SMALL")]
        [Description("Trees-Small (Under 20 Ft)")]
        TreesSmall,
        [EnumMember(Value = "SPARSE")]
        [Description("Trees-Sparse")]
        TreesSparse,
        [EnumMember(Value = "VWS")]
        [Description("Views")]
        View,
        [EnumMember(Value = "WFALL")]
        [Description("Waterfall")]
        Waterfall,
        [EnumMember(Value = "WETLAND")]
        [Description("Wetlands")]
        Wetlands,
        [EnumMember(Value = "XERISCAP")]
        [Description("Xeriscape")]
        Xeriscape,
        [EnumMember(Value = "ZLOT")]
        [Description("Zero Lot Line")]
        ZeroLotLine,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
