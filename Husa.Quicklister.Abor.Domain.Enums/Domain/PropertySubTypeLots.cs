namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum PropertySubTypeLots
    {
        [EnumMember(Value = "MULTIPLE")]
        [Description("MultiLots (Adjacent)")]
        MultiLots,
        [EnumMember(Value = "SINGLE")]
        [Description("Single Lot")]
        SingleLot,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
