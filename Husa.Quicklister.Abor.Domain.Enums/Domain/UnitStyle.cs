namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum UnitStyle
    {
        [EnumMember(Value = "1STFE")]
        [Description("1st Floor Entry")]
        FirstFloorEntry,
        [EnumMember(Value = "2NDFE")]
        [Description("2nd Floor Entry")]
        SecondFloorEntry,
        [EnumMember(Value = "3RDFE")]
        [Description("3rd+ Floor Entry")]
        ThirdPlusFloorEntry,
        [EnumMember(Value = "ELVTR")]
        [Description("Elevator")]
        Elevator,
        [EnumMember(Value = "ENDUT")]
        [Description("End Unit")]
        EndUnit,
        [EnumMember(Value = "ENTST")]
        [Description("Entry Steps")]
        EntrySteps,
        [EnumMember(Value = "MUFLR")]
        [Description("Multi-level Floor Plan")]
        MultilevelFloorPlan,
        [EnumMember(Value = "SLFPL")]
        [Description("Single-level Floor Plan")]
        SinglelevelFloorPlan,
    }
}
