namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MarketStatuses
    {
        [EnumMember(Value = "ACT")]
        [Description("Active")]
        Active = 0,
        [EnumMember(Value = "AO")]
        [Description("Active Option")]
        ActiveOption = 1,
        [EnumMember(Value = "RFR")]
        [Description("Active RFR")]
        ActiveRFR = 2,
        [EnumMember(Value = "BOM")]
        [Description("Back on Market")]
        BackOnMarket = 3,
        [EnumMember(Value = "CAN")]
        [Description("Cancelled")]
        Cancelled = 4,
        [EnumMember(Value = "PND")]
        [Description("Pending")]
        Pending = 5,
        [EnumMember(Value = "PDB")]
        [Description("Pending SB")]
        PendingSB = 6,
        [EnumMember(Value = "PCH")]
        [Description("Price Change")]
        PriceChange = 7,
        [EnumMember(Value = "SLD")]
        [Description("Sold")]
        Sold = 8,
        [EnumMember(Value = "WDN")]
        [Description("Withdrawn")]
        Withdrawn = 9,
    }
}
