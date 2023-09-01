namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum LaundryFeatures
    {
        [EnumMember(Value = "ELCCN")]
        [Description("Electric Dryer Hookup")]
        ElectricDryerHookup,
        [EnumMember(Value = "GASCN")]
        [Description("Gas Dryer Hookup")]
        GasDryerHookup,
        [EnumMember(Value = "CHTE")]
        [Description("Laundry Chute")]
        LaundryChute,
        [EnumMember(Value = "SINK")]
        [Description("Laundry Sink")]
        LaundrySink,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "STCON")]
        [Description("Stackable W/D Connections")]
        StackableConnections,
        [EnumMember(Value = "WSHCN")]
        [Description("Washer Hookup")]
        WasherHookup,
    }
}
