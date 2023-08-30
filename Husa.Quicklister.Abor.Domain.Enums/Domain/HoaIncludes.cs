namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum HoaIncludes
    {
        [EnumMember(Value = "CABLE")]
        [Description("Cable")]
        Cable,
        [EnumMember(Value = "CAM")]
        [Description("Common Areas Maintenance")]
        CommonAreasMaintenance,
        [EnumMember(Value = "COMMAREA")]
        [Description("Maintenance Grounds")]
        MaintenanceGrounds,
        [EnumMember(Value = "EXTRMNT")]
        [Description("Maintenance Structure")]
        MaintenanceStructure,
        [EnumMember(Value = "INTRNT")]
        [Description("Internet")]
        Internet,
        [EnumMember(Value = "LNDSP")]
        [Description("Landscaping")]
        Landscaping,
        [EnumMember(Value = "SECU")]
        [Description("Security")]
        Security,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
