namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum UnitStyle
    {
        [EnumMember(Value = "1st Floor Entry")]
        [Description("1st Floor Entry")]
        FirstFloorEntry,
        [EnumMember(Value = "2nd Floor Entry")]
        [Description("2nd Floor Entry")]
        SecondFloorEntry,
        [EnumMember(Value = "3rd+ Floor Entry")]
        [Description("3rd+ Floor Entry")]
        ThirdPlusFloorEntry,
        [EnumMember(Value = "Elevator")]
        [Description("Elevator")]
        Elevator,
        [EnumMember(Value = "End Unit")]
        [Description("End Unit")]
        EndUnit,
        [EnumMember(Value = "Entry Steps")]
        [Description("Entry Steps")]
        EntrySteps,
        [EnumMember(Value = "Multi level Floor Plan")]
        [Description("Multi-level Floor Plan")]
        MultilevelFloorPlan,
        [EnumMember(Value = "Single level Floor Plan")]
        [Description("Single-level Floor Plan")]
        SinglelevelFloorPlan,
    }
}
