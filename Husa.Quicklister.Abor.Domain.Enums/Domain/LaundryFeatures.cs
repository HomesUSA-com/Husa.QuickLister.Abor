namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum LaundryFeatures
    {
        [EnumMember(Value = "ElectricDryerHookup")]
        [Description("Electric Dryer Hookup")]
        ElectricDryerHookup,
        [EnumMember(Value = "GasDryerHookup")]
        [Description("Gas Dryer Hookup")]
        GasDryerHookup,
        [EnumMember(Value = "LaundryChute")]
        [Description("Laundry Chute")]
        LaundryChute,
        [EnumMember(Value = "Sink")]
        [Description("Laundry Sink")]
        LaundrySink,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "Stacked")]
        [Description("Stackable W/D Connections")]
        StackableConnections,
        [EnumMember(Value = "WasherHookup")]
        [Description("Washer Hookup")]
        WasherHookup,
    }
}
