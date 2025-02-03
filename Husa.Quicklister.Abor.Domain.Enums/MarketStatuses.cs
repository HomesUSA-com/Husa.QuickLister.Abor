namespace Husa.Quicklister.Abor.Domain.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MarketStatuses
    {
        [EnumMember(Value = "Active")]
        [Description("Active")]
        Active,
        [EnumMember(Value = "ActiveUnderContract")]
        [Description("Active Under Contract")]
        ActiveUnderContract,
        [EnumMember(Value = "Canceled")]
        [Description("Canceled")]
        Canceled,
        [EnumMember(Value = "Closed")]
        [Description("Closed")]
        Closed,
        [EnumMember(Value = "Expired")]
        [Description("Expired")]
        Expired,
        [EnumMember(Value = "Hold")]
        [Description("Hold")]
        Hold,
        [EnumMember(Value = "Pending")]
        [Description("Pending")]
        Pending,
    }
}
