namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum LotDescription
    {
        [EnumMember(Value = "BACKPARK")]
        [Description("Backs to Greenbelt Park")]
        BackstoGreenbeltPark,
        [EnumMember(Value = "BKYRD")]
        [Description("Back Yard")]
        BackYard,
        [EnumMember(Value = "Bluff")]
        [Description("Bluff")]
        Bluff,
        [EnumMember(Value = "City Lot")]
        [Description("City Lot")]
        CityLot,
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
        [EnumMember(Value = "FTR")]
        [Description("Few Tree")]
        FewTree,
        [EnumMember(Value = "FTYRD")]
        [Description("Front Yard")]
        FrontYard,
        [EnumMember(Value = "GOLFCOMM")]
        [Description("Near Golf Course")]
        NearGolfCourse,
        [EnumMember(Value = "GSLO")]
        [Description("Gentle Sloping")]
        GentleSloping,
        [EnumMember(Value = "HEAVY")]
        [Description("Trees-Heavy")]
        TreesHeavy,
        [EnumMember(Value = "INTERIOR")]
        [Description("Interior Lot")]
        InteriorLot,
        [EnumMember(Value = "IRREG")]
        [Description("Irregular Lot")]
        IrregularLot,
        [EnumMember(Value = "LARGE")]
        [Description("Trees-Large (Over 40 Ft)")]
        TreesLarge,
        [EnumMember(Value = "LEVEL")]
        [Description("Level")]
        Level,
        [EnumMember(Value = "LNDCP")]
        [Description("Landscaped")]
        Landscaped,
        [EnumMember(Value = "MEDIUM")]
        [Description("Trees-Medium (20 Ft - 40 Ft)")]
        TreesMedium,
        [EnumMember(Value = "MNUAL")]
        [Description("Sprinkler - Manual")]
        SprinklerManual,
        [EnumMember(Value = "MODERATE")]
        [Description("Trees-Moderate")]
        TreesModerate,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "NPLN")]
        [Description("Native Plants  ")]
        NativePlants,
        [EnumMember(Value = "ONGLFCRS")]
        [Description("On Golf Course")]
        OnGolfCourse,
        [EnumMember(Value = "PIE")]
        [Description("Pie Shaped Lot")]
        PieShapedLot,
        [EnumMember(Value = "PMNRD")]
        [Description("Public Maintained Road")]
        PublicMaintainedRoad,
        [EnumMember(Value = "PUB")]
        [Description("Near Public Transit")]
        NearPublicTransit,
        [EnumMember(Value = "SLDN")]
        [Description("Sloped Down")]
        SlopedDown,
        [EnumMember(Value = "SLUp")]
        [Description("Sloped Up")]
        SlopedUp,
        [EnumMember(Value = "SMALL")]
        [Description("Trees-Small (Under 20 Ft)")]
        TreesSmall,
        [EnumMember(Value = "SPARSE")]
        [Description("Trees-Sparse")]
        TreesSparse,
        [EnumMember(Value = "SPAU")]
        [Description("Sprinkler - Automatic")]
        SprinklerAutomatic,
        [EnumMember(Value = "SPDP")]
        [Description("Sprinkler - Drip Only/Bubblers")]
        SprinklerDripOnlyBubblers,
        [EnumMember(Value = "SPFNT")]
        [Description("Sprinkler - In Front")]
        SprinklerInFront,
        [EnumMember(Value = "SPING")]
        [Description("Sprinkler - In-ground")]
        SprinklerInground,
        [EnumMember(Value = "SPPAR")]
        [Description("Sprinkler - Partial")]
        SprinklerPartial,
        [EnumMember(Value = "SPRAIN")]
        [Description("Sprinkler - Rain Sensor")]
        SprinklerRainSensor,
        [EnumMember(Value = "SPREA")]
        [Description("Sprinkler - Back Yard")]
        SprinklerBackYard,
        [EnumMember(Value = "SPSD")]
        [Description("Sprinkler - Side Yard")]
        SprinklerSideYard,
        [EnumMember(Value = "VWS")]
        [Description("View")]
        View,
        [EnumMember(Value = "Wooded")]
        [Description("Trees-Many")]
        TreesMany,
        [EnumMember(Value = "ZLOT")]
        [Description("Zero Lot Line")]
        ZeroLotLine,
    }
}
