namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CancelledOptions
    {
        [Description("BuildingAnotherLot")]
        [EnumMember(Value = "BALOT")]
        BuildingAnotherLot = 0,
        [Description("NotBuildingThisLot")]
        [EnumMember(Value = "NBOTL")]
        NotBuildingThisLot = 1,
        [Description("Other")]
        [EnumMember(Value = "OTHER")]
        Other = 2,
        [Description("Repairs")]
        [EnumMember(Value = "REPAIRS")]
        Repairs = 3,
        [Description("ChangingPlans")]
        [EnumMember(Value = "CPLAN")]
        ChangingPlans = 4,
        [Description("ResetDOM")]
        [EnumMember(Value = "RDOM")]
        ResetDOM = 5,
        [Description("SomethingTragic")]
        [EnumMember(Value = "STRAGIC")]
        SomethingTragic = 6,
        [Description("ListingWithRealtor")]
        [EnumMember(Value = "LWREALTOR")]
        ListingWithRealtor = 7,
    }
}
